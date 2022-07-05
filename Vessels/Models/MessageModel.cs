namespace Vessels.Models
{
    public class MessageModel
    {
        public string IMO { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public Position Position { get; set; }
    }

    public class Position
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
