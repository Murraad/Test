using Microsoft.EntityFrameworkCore;
using Vessels.Data;
using Vessels.Data.Models;

namespace Vessels.Repositories
{
    public class SQLServerDataRepository : IDataRepository
    {
        private readonly DataContext dataContext;

        public SQLServerDataRepository(DataContext dataContext) => this.dataContext = dataContext;

        public async Task AddPositionAsync(VesselPosition position)
        {
            dataContext.Add(position);
            await dataContext.SaveChangesAsync();
        }

        public async Task DeletePositionAsync(int id)
        {
            var vesselPosition = await dataContext.Positions.FindAsync(id);
            if (vesselPosition != null) { dataContext.Positions.Remove(vesselPosition); }

            await dataContext.SaveChangesAsync();
        }

        public async Task<VesselPosition?> GetPositionAsync(int id)
        {
            var result = await dataContext.Positions.FirstOrDefaultAsync(m => m.Id == id);
            if(result is not null) dataContext.Entry(result).Reference(p => p.Vessel).Load();
            return result;
        }

        public async Task<IEnumerable<VesselPosition>> GetAllPositionsAsync() => await dataContext.Positions.Include(p => p.Vessel).ToListAsync();

        public async Task UpdatePositionAsync(VesselPosition position)
        {
            this.dataContext.Positions.Update(position);
            await this.dataContext.SaveChangesAsync();
        }

        public bool PositionExists(int id) => this.dataContext.Positions.Any(p => p.Id == id);

        public async Task<IEnumerable<Vessel>> GetVesselsAsync() => await this.dataContext.Vessels.OrderBy(v => v.Name).ToListAsync();

        public async Task<Vessel?> GetVesselAsync(string imo) => await this.dataContext.Vessels.FirstOrDefaultAsync(v => v.IMO == imo);

        public void Dispose() => this.dataContext.Dispose();
    }
}
