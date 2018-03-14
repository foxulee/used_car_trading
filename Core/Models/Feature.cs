using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project_vega.Core.Models
{
    public class Feature
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        
    }
}