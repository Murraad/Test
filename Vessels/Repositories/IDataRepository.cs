using Vessels.Data.Models;

namespace Vessels.Repositories
{
    public interface IDataRepository
    {
        Task<IEnumerable<VesselPosition>> GetAllPositionsAsync();
        Task<VesselPosition?> GetPositionAsync(int id);
        Task AddPositionAsync(VesselPosition position);
        Task UpdatePositionAsync(VesselPosition position);
        Task DeletePositionAsync(int id);
        bool PositionExists(int id);
        Task<IEnumerable<Vessel>> GetVesselsAsync();
        Task<Vessel?> GetVesselAsync(string imo);
    }
}
