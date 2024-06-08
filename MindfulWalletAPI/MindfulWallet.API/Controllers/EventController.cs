using Microsoft.AspNetCore.Mvc;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindfulWalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetEventsByUserId(int userId)
        {
            var events = await _eventService.GetEventsByUserIdAsync(userId);
            return Ok(events);
        }
    }
}
