using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net.WebSockets;
using Vessels.Data.Models;
using Vessels.Hubs;
using Vessels.Models;
using Vessels.Repositories;

namespace Vessels.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IDataRepository dataRepository;
        private readonly IHubContext<VesselPositionsHub> hubContext;

        public MessageController(IDataRepository dataRepository, IHubContext<VesselPositionsHub> hubContext) 
        { 
            this.dataRepository = dataRepository;
            this.hubContext = hubContext;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MessageModel? message)
        {
            var vessel = await this.dataRepository.GetVesselAsync(message.IMO);
            if(vessel is not null && vessel.Name == message.Name) // check if data is valid
            {
                var position = new VesselPosition
                {
                    Date = message.DateTime,
                    Latitude = message.Position.Latitude,
                    Longitude = message.Position.Longitude,
                    VesselIMO = message.IMO,
                    Vessel = vessel,
                };
                await this.dataRepository.AddPositionAsync(position);

                position.Vessel.VesselPositions = null; //help ui to catch object(prevent cicle)
                await this.hubContext.Clients.All.SendAsync("ReceiveMessage", position);
                return Accepted();
            }
            return BadRequest();
        }
    }
}
