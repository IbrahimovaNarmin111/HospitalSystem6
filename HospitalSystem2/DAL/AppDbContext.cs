using HospitalSystem2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem2.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Room>Rooms { get; set; }
        
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Checkup> Checkups { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<ReceptionEnumModel> ReceptionEnumModels { get; set; }
        public DbSet<Randevu>Randevus { get; set; }
        public DbSet<Total> Totals { get; set; }  
        public DbSet<Profit> Profits { get; set; }  
        public DbSet<Cost> Costs { get; set; }
        
    }
}
