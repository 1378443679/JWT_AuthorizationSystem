using JWT_Authentication_Sistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;

namespace JWT_Authentication_Sistemi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenHelper _jwtHelper;
        private readonly IMongoCollection<User> _usersCollection;

        public AuthController(IConfiguration config, JwtTokenHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;

            var connectionString = config["JWT_Authentication_Sistemi:ConnectionString"];
            var databaseName = config["JWT_Authentication_Sistemi:DatabaseName"];
            var collectionName = config["JWT_Authentication_Sistemi:UsersCollectionName"];

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _usersCollection = database.GetCollection<User>(collectionName);
        }

        // Register için DTO kullanıyoruz
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _usersCollection.Find(u => u.Email == registerDto.Email).FirstOrDefaultAsync();
            if (existingUser != null)
                return BadRequest("Bu email ile kayıtlı kullanıcı zaten var.");

            var hashedPassword = HashPassword(registerDto.Password);

            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = hashedPassword
            };

            await _usersCollection.InsertOneAsync(user);

            return Ok("Kayıt başarılı");
        }

        // Login için DTO kullanıyoruz
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _usersCollection.Find(u => u.Email == loginDto.Email).FirstOrDefaultAsync();
            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
                return Unauthorized("Email veya şifre hatalı.");

            var token = _jwtHelper.GenerateToken(user.Email);

            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("get-user-info")]
        public IActionResult GetUserInfo()
        {
            var userEmail = User.Identity?.Name; // Token'daki email bilgisini al
            return Ok(new { Email = userEmail });
        }

        // Şifreyi SHA256 ile hash'leme
        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // Hash karşılaştırma
        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(inputPassword) || string.IsNullOrEmpty(storedHash))
                return false;

            return HashPassword(inputPassword) == storedHash;
        }
    }
}