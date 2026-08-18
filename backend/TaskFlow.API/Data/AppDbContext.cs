using Microsoft.EntityFrameworkCore;
using TaskFlow.API.Models;

namespace TaskFlow.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
}