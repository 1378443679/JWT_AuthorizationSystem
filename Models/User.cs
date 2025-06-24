using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JWT_Authentication_Sistemi.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
