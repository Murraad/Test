using System.ComponentModel.DataAnnotations;

namespace Vessels.Data.Models
{
    public class Vessel
    {
        [Key]
        [Required]
        public string IMO { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<VesselPosition> VesselPositions { get; set; }
    }
}
