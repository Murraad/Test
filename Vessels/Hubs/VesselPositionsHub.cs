using Microsoft.AspNetCore.SignalR;
using Vessels.Data.Models;

namespace Vessels.Hubs
{
    public class VesselPositionsHub : Hub
    {
        public async Task SendVesselPosition(VesselPosition position) => await this.Clients.All.SendAsync("ReceiveMessage", position);
    }
}
