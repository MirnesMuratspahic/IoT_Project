using AlarmSystem.Models;
using AlarmSystem.Models.DTO;
using AlarmSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace AlarmSystem.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<DeviceResponse> DeviceResponses { get; set; }

    }

}
