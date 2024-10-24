using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleManagement.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleID { get; set; }

        [ForeignKey("Brand")]
        public int BrandID { get; set; }
        public Brand? Brand { get; set; }

        [ForeignKey("Model")]
        public int ModelID { get; set; }
        public VehicleModel? Model { get; set; }
        public int Year { get; set; }
    }
}
