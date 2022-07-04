using AirlineApi.Data;
using AirlineApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AirlineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookSubscriptionController : ControllerBase
    {
        private readonly AirlineDbContext _context;

        public WebhookSubscriptionController(AirlineDbContext context)
        {
            _context = context;
        }

        public ActionResult<WebhookSubscriptionReadDto> CreateSubscription(WebhookSubscriptionCreateDto webSubscription)
        {

        }
    }
}