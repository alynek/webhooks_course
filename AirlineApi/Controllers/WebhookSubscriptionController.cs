using AirlineApi.Data;
using AirlineApi.Dtos;
using AirlineApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirlineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookSubscriptionController : ControllerBase
    {
        private readonly AirlineDbContext _context;
        private readonly IMapper _mapper;

        public WebhookSubscriptionController(AirlineDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{secret}")]
        public ActionResult<WebhookSubscriptionReadDto> GetSubscriptionBySecret(string secret)
        {
            var subscription = _context.WebhookSubscriptions
                                .AsNoTracking()
                                .FirstOrDefault(s => s.Secret == secret);

            if(subscription is null) return NotFound();

            return Ok(_mapper.Map<WebhookSubscriptionReadDto>(subscription));
        }

        [HttpPost]
        public ActionResult<WebhookSubscriptionReadDto> CreateSubscription(WebhookSubscriptionCreateDto webSubscription)
        {
            var subscription = _context.WebhookSubscriptions
                                .AsNoTracking().
                                FirstOrDefault(s => s.WebhookURI == webSubscription.WebhookURI);

            if(subscription is null)
            {
                subscription = _mapper.Map<WebhookSubscription>(webSubscription);

                subscription.Secret = Guid.NewGuid().ToString();
                subscription.WebhookPublisher = "PanAus";
                try
                {
                    _context.WebhookSubscriptions.Add(subscription);
                    _context.SaveChanges();
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return Ok(_mapper.Map<WebhookSubscriptionReadDto>(subscription));
            }
            return BadRequest("Subscription already exists");
        }
    }
}