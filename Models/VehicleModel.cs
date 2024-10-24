using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleManagement.Models
{
    public class VehicleModel
    {
        [Key]
        public int ModelID { get; set; }
        public string ModelName { get; set; }

        [ForeignKey("Brand")]
        public int BrandID { get; set; } 
        public Brand? Brand { get; set; }  // Optional navigation property
    }
}
