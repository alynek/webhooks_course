using AirlineApi.Data;
using AirlineApi.Dtos;
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

        public FlightsController(AirlineDbContext context, IMapper mapper)
        {
            _context = _=context;
            _mapper = mapper;
        }

        [HttpGet("{flightCode}", Name = "GetFlightDetailsByCode")]
        public ActionResult<FlightDetailReadDto> GetFlightDetailsByCode(string flightCode)
        {
            var flight = _context.FlightDetails.AsNoTracking()
                        .FirstOrDefault(f => f.FlightCode == flightCode);

            if(flight is null) return NotFound();

            return Ok(_mapper.Map<FlightDetailReadDto>(flight));
        }
    }
}