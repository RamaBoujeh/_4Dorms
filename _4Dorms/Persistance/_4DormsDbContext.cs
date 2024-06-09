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

            modelBuilder.Entity<Dormitory>()
                .Property(d => d.PriceFullYear)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Dormitory>()
                .Property(d => d.PriceHalfYear)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PaymentGate>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            // Configure cascade delete for Dormitory and related entities
            modelBuilder.Entity<Dormitory>()
                .HasMany(d => d.Rooms)
                .WithOne(r => r.Dormitory)
                .HasForeignKey(r => r.DormitoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Dormitory>()
                .HasMany(d => d.ImageUrls)
                .WithOne(i => i.Dormitory)
                .HasForeignKey(i => i.DormitoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Dormitory>()
                .HasMany(d => d.Reviews)
                .WithOne(r => r.Dormitory)
                .HasForeignKey(r => r.DormitoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Dormitory>()
                .HasMany(d => d.Bookings)
                .WithOne(b => b.Dormitory)
                .HasForeignKey(b => b.DormitoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure many-to-many relationship with cascade delete
            modelBuilder.Entity<FavoriteList>()
                .HasMany(f => f.Dormitories)
                .WithMany(d => d.Favorites)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteListDormitory",
                    j => j
                        .HasOne<Dormitory>()
                        .WithMany()
                        .HasForeignKey("DormitoryId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<FavoriteList>()
                        .WithMany()
                        .HasForeignKey("FavoriteListId")
                        .OnDelete(DeleteBehavior.Cascade)
                );
        }

        }
}
