using EventManagerAPI.Data;
using EventManagerAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagerAPI.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events
                .Include(e => e.Attendees)
                .ToListAsync();


            var result = events.Select(e => new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                Location = e.Location,

                Attendees = e.Attendees.Select(a => new AttendeeDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Email = a.Email

                }).ToList()

            });


            return Ok(result);
        }
    }
}