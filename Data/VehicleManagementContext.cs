using Microsoft.EntityFrameworkCore;
using VehicleManagement.Models;

namespace VehicleManagement.Data
{
    /// <summary>
    /// The database context for the Vehicle Management application.
    /// Manages access to the Vehicles, Brands, and Models tables.
    /// </summary>
    public class VehicleManagementContext : DbContext
    {
        /// <summary>
        /// Gets or sets the Vehicles table.
        /// </summary>
        public DbSet<Vehicle> Vehicles { get; set; }

        /// <summary>
        /// Gets or sets the Brands table.
        /// </summary>
        public DbSet<Brand> Brands { get; set; }

        /// <summary>
        /// Gets or sets the Models table.
        /// </summary>
        public DbSet<VehicleModel> Models { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleManagementContext"/> class.
        /// </summary>
        /// <param name="options">The options to configure the context.</param>
        public VehicleManagementContext(DbContextOptions<VehicleManagementContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Prevent deletion of a brand if it has vehicles
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Brand)
                .WithMany()
                .HasForeignKey(v => v.BrandID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

            // Prevent deletion of a model if it has vehicles
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Model)
                .WithMany()
                .HasForeignKey(v => v.ModelID)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete
        }
    }
}
