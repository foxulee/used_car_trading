using System.ComponentModel.DataAnnotations.Schema;

namespace project_vega.Core.Models
{
    // using EF Core 2.0 on generating a many-to-many relationship, need using an explicit class for the join and use FluentAPI to setup in DbContext
    [Table("VehicleFeatures")]
    public class VehicleFeature
    {
        // compositive primary key
        // to use compositive primary key, need to setup using FluentAPI
        public int VehicleId { get; set; }
        public int FeatureId { get; set; }

        // navigation property
        public Vehicle Vehicle { get; set; }
        public Feature Feature { get; set; }
    }
}