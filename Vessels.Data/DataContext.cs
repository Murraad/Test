using Microsoft.EntityFrameworkCore;
using Vessels.Data.Models;

namespace Vessels.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Vessel> Vessels { get; set; }
        public DbSet<VesselPosition> Positions { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
    }
}
