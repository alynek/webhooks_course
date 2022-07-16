
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgentApi.Data;
using TravelAgentApi.Dtos;

namespace TravelAgentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly TravelAgentDbContext _context;
        public NotificationsController(TravelAgentDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult FlightChanged(FlightDetailUpdateDto flightDto)
        {
            var secret = _context.WebhoookSecrets
                        .AsNoTracking()
                        .FirstOrDefault(f => f.Publisher == flightDto.Publisher && f.Secret == flightDto.Secret);

            if(secret is null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Secret - Ignore Webhook");
                Console.ResetColor();
                return Ok();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Valid Webhook!");
                Console.WriteLine($"Old Price {flightDto.OldPrice}, New Price {flightDto.NewPrice}");
                Console.ResetColor();
                return Ok();
            }
        }
    }
}