using Microsoft.EntityFrameworkCore;
using MusicStreaming.Api.Entities;

namespace MusicStreaming.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}