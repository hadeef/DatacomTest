using DatacomTest.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DatacomTest.Server.Repositories;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Application>? Applications { get; set; }
}