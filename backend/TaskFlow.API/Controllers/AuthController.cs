using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TaskFlow.API.Data;
using TaskFlow.API.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user is null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var passwordIsValid = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash
        );

        if (!passwordIsValid)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }
var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Name, user.Name),
    new Claim(ClaimTypes.Email, user.Email)
};

var jwtKey = _configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT key not configured.");

var key = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(jwtKey)
);

var credentials = new SigningCredentials(
    key,
    SecurityAlgorithms.HmacSha256
);

var token = new JwtSecurityToken(
    issuer: _configuration["Jwt:Issuer"],
    audience: _configuration["Jwt:Audience"],
    claims: claims,
    expires: DateTime.UtcNow.AddMinutes(
        _configuration.GetValue<int>("Jwt:ExpiresMinutes")
    ),
    signingCredentials: credentials
);

var tokenValue = new JwtSecurityTokenHandler()
    .WriteToken(token);
        return Ok(new
        {
            token = tokenValue,
            user = new
           { 
            user.Id,
            user.Name,
            user.Email
          }  
        });
    }

[Authorize]
[HttpGet("protected")]
public IActionResult Protected()
{
    return Ok(new
    {
        message = "Access granted."
    });
}

}