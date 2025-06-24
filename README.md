# JWT Authentication & Authorization API (ASP.NET Core + MongoDB)

This is a simple **JWT-based Authentication and Authorization API** built with **ASP.NET Core Web API** and **MongoDB**. It allows users to register, login, and access protected endpoints using JSON Web Tokens (JWT).

---

## 🚀 Features

- ✅ User registration
- ✅ User login with JWT generation
- ✅ Password hashing with SHA-256
- ✅ Secure endpoints using `[Authorize]`
- ✅ MongoDB integration via MongoDB.Driver
- ✅ Built-in Swagger UI for testing

---

## 🛠️ Technologies Used

- ASP.NET Core 6 Web API
- MongoDB
- JWT (JSON Web Token)
- SHA-256 (for password hashing)
- Swagger / OpenAPI

---

## 📁 Project Structure

JWT_Authentication_Sistemi/
│
├── Controllers/
│ └── AuthController.cs
│
├── Models/
│ ├── User.cs
│ ├── LoginDto.cs
│ └── RegisterDto.cs
│
├── JwtTokenHelper.cs
├── Program.cs
├── appsettings.json
└── README.md

