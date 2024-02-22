using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data
{
    public class AppDbContext : DbContext
    {
        /* Constructor */
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /* DbSets */
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Command> Commands { get; set; }
    }
}