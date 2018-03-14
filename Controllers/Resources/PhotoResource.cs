using System.ComponentModel.DataAnnotations;

namespace project_vega.Controllers.Resources
{
    public class PhotoResource
    {
        public int Id { get; set; }
        
        public string FileName { get; set; }

        public int VehicleId { get; set; }
    }
}