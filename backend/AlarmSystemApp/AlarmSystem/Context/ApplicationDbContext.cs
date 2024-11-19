using AlarmSystem.Models;
using AlarmSystem.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace AlarmSystem.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }

    }

}
