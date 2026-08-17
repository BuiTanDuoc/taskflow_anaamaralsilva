using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.API.Data;
using TaskFlow.API.Models;
using TaskFlow.API.DTOs;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(RegisterUserRequest request)
    {
         var emailAlreadyExists = await _context.Users.AnyAsync(u => u.Email == request.Email);

    if (emailAlreadyExists)
    {
        return Conflict(new
        {
            message = "A user with this email already exists."
        });
    }
        var user = new User
{
    Name = request.Name,
    Email = request.Email,
    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
};

_context.Users.Add(user);
await _context.SaveChangesAsync();

return CreatedAtAction(nameof(CreateUser), new { id = user.Id }, new
{
    user.Id,
    user.Name,
    user.Email,
    user.CreatedAt
});
}
}