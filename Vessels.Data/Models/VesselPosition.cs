using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vessels.Data.Models
{
    public class VesselPosition
    {
        private double latitude;
        private double longitude;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Range(-90, 90)]
        public double Latitude 
        { 
            get => this.latitude; 
            set
            {
                if(value > 90 || value < -90) { throw new ArgumentOutOfRangeException(); }
                else { this.latitude = value; }
            }
        }
        [Range(-180, 180)]
        public double Longitude
        {
            get => this.longitude;
            set
            {
                if (value > 180 || value < -180) { throw new ArgumentOutOfRangeException(); }
                else { this.longitude = value; }
            }
        }
        public string VesselIMO { get; set; }
        public Vessel? Vessel { get; set; }

    }
}
