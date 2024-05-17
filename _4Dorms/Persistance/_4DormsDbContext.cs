using _4Dorms.Models;
using Microsoft.EntityFrameworkCore;

namespace _4Dorms.Persistance
{
    public class _4DormsDbContext : DbContext
    {
        public virtual DbSet<Administrator> Administrators { get; set; }
        public virtual DbSet<Dormitory> Dormitories { get; set; }
        public virtual DbSet<Booking> DormitoriesBooking { get; set; }
        public virtual DbSet<DormitoryOwner> DormitoryOwners { get; set; }
        public virtual DbSet<FavoriteList> FavoriteLists { get; set; }
        public virtual DbSet<PaymentGate> PaymentGateways { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<DormitoryImage> DormitoryImages { get; set; }  
        public virtual DbSet<LogIn> LogIn { get; set; }

        public _4DormsDbContext(DbContextOptions<_4DormsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Administrator>().HasData(
                new Administrator
                {
                    AdministratorId = 1,
                    Name = "Ruaa",
                    Email = "Ruaa@example.com",
                    PhoneNumber = "1234567890",
                    Password = "000",
                    ProfilePictureUrl = "none"
                }
            );
            modelBuilder.Entity<FavoriteList>()
            .HasMany(fl => fl.Dormitories)
            .WithMany(d => d.Favorites)
            .UsingEntity(j => j.ToTable("DormitoryFavoriteList"));
        }

    }
}
