using AirlineApi.Data;
using AirlineApi.Dtos;
using AirlineApi.MessageBus;
using AirlineApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirlineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly AirlineDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public FlightsController(AirlineDbContext context, IMapper mapper, IMessageBusClient messageBusClient)
        {
            _context = _=context;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }

        [HttpGet("{flightCode}")]
        public ActionResult<FlightDetailReadDto> GetFlightDetailsByCode(string flightCode)
        {
            var flight = _context.FlightDetails.AsNoTracking()
                        .FirstOrDefault(f => f.FlightCode == flightCode);

            if(flight is null) return NotFound();

            return Ok(_mapper.Map<FlightDetailReadDto>(flight));
        }

        [HttpPost]
        public ActionResult<FlightDetailReadDto> CreateFLight(FlightDetailCreateDto flight)
        {
            var flightDto = _context.FlightDetails.AsNoTracking()
                            .FirstOrDefault(f => f.FlightCode == flight.FlightCode);

            if(flightDto is null)
            {
                var flightCreated = _mapper.Map<FlightDetail>(flight);

                _context.FlightDetails.Add(flightCreated);
                _context.SaveChanges();

                return Ok(_mapper.Map<FlightDetailReadDto>(flightCreated));
            }
            return BadRequest("Flight already exists");
        }

        [HttpPut("{id}")]
        public ActionResult UpdateFlightDetail(int id, FlightDetailUpdateDto flightDetailUpdateDto)
        {
            var flight = _context.FlightDetails.FirstOrDefault(f => f.Id == id);

            if(flight is null) return NotFound();

            decimal oldPrice = flight.Price;

            _mapper.Map(flightDetailUpdateDto, flight);
            _context.SaveChanges();

            sendNewPriceMessage(oldPrice, flight);

            return NoContent();
        }

        private void sendNewPriceMessage(decimal oldPrice, FlightDetail flight)
        {
            if(oldPrice != flight.Price)
            {
                var message = new NotificationMessageDto
                {
                    WebhookType = "pricechange",
                    OldPrice = oldPrice,
                    NewPrice = flight.Price,
                    FlightCode = flight.FlightCode
                };
                _messageBusClient.SendMessage(message);
            }
        }
    }
}