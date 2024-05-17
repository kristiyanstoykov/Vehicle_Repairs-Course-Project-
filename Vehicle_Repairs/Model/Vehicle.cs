using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Repairs.Model
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string Brand { get; set; }

        [Required]
        public required string Model { get; set; }

        public int? YearMade { get; set; }

        [Required]
        public required string RegistrationNumber { get; set; }

        public virtual ICollection<Repair>? Repairs { get; set; }

        public Vehicle() { }
    }
}
