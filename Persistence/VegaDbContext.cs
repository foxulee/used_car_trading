using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using project_vega.Core.Models;

namespace project_vega.Persistence
{
    public class VegaDbContext : DbContext
    {
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Photo> Photos { get; set; }

        //using dependency injection to initiate DbContext
        public VegaDbContext(DbContextOptions<VegaDbContext> options) : base(options)
        {

        }

        //Fluent API is an advanced way of specifying model configuration that covers everything that data annotations can do in addition to some more advanced configuration not possible with data annotations.Data annotations and the fluent API can be used together, but Code First gives precedence to Fluent API > data annotations > default conventions.

        //Fluent API is another way to configure your domain classes.

        //The Code First Fluent API is most commonly accessed by overriding the OnModelCreating method on your derived DbContext.

        //Fluent API provides more functionality for configuration than DataAnnotations.Fluent API supports the following types of mappings.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleFeature>().HasKey(vf => new { vf.VehicleId, vf.FeatureId });
        }
    }
}