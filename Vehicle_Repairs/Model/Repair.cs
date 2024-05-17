using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Repairs.Model
{
    public class Repair
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required int YearOfService { get; set; }

        [Required]
        public required string ServiceType { get; set; }
        public string? Description { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public Repair() { }

        public Repair(int yearOfService, string serviceType, string description, int vehicleId, Vehicle vehicle)
        {
            YearOfService = yearOfService;
            ServiceType = serviceType;
            Description = description;
            VehicleId = vehicleId;
            Vehicle = vehicle;
        }
    }
}
