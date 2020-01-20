using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("")]
    public class CarController : ControllerBase
    {
        private readonly ECarsContext _context;

        public CarController(ECarsContext context)
        {
            _context = context;
        }

        [HttpGet("cars")]
        public async Task<IEnumerable<Car>> Get([FromQuery] DateTime? date = null)
        {
            if (!date.HasValue)
            {
                return _context.Cars.ToList();
            }

            return _context.Cars.Include(c => c.Bookings).AsEnumerable().Where(c => !c.Bookings.Exists(b => b.Date.Date.CompareTo(date.Value.Date) == 0)).Select(c =>
            {
                c.Bookings = null;
                return c;
            });
        }

        [HttpPost("cars/{carId}")]
        public async Task BookCar([FromBody] DateTime date, int carId)
        {
            Console.WriteLine(date);
            Booking booking = new Booking { Date = date.Date, CarId= carId };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }
    }
}
