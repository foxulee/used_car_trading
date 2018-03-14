using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using project_vega.Core.Models;

namespace project_vega.Controllers.Resources
{
    //allow client to send, invoid any overpost attacking
    public class SaveVehicleResource
    {
        public int Id { get; set; }
        public int ModelId { get; set; }

        public bool IsRegistered { get; set; }

        [Required]
        public ContactResource Contact { get; set; }
        
        public ICollection<int> Features { get; set; }

        public SaveVehicleResource()
        {
            Features = new Collection<int>();
        }

    }
}