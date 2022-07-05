using Vessels.Data.Models;

namespace Vessels.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            if (context.Vessels.Any()) { return; }

            var vessels = new Vessel[]
            {
                new Vessel { IMO = "Test-1", Name = "Victory" },
                new Vessel { IMO = "Test-2", Name = "Santa-Maria" },
                new Vessel { IMO = "Test-3", Name = "Black Pearl" },
                new Vessel { IMO = "Test-4", Name = "Sahaydachny" },
            };

            context.Vessels.AddRange(vessels);
            context.SaveChanges();
        }
    }
}
