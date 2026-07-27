using EventManagerAPI.Data;
using EventManagerAPI.DTOs;
using EventManagerAPI.Models;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.Attendees)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }


            var result = new EventDto
            {
                Id = eventItem.Id,
                Title = eventItem.Title,
                Description = eventItem.Description,
                Date = eventItem.Date,
                Location = eventItem.Location,

                Attendees = eventItem.Attendees.Select(a => new AttendeeDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Email = a.Email

                }).ToList()
            };


            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var newEvent = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                Date = dto.Date,
                Location = dto.Location
            };


            _context.Events.Add(newEvent);

            await _context.SaveChangesAsync();


            return CreatedAtAction(
                nameof(GetEvent),
                new { id = newEvent.Id },
                newEvent
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, UpdateEventDto dto)
        {
            var existingEvent = await _context.Events.FindAsync(id);

            if (existingEvent == null)
            {
                return NotFound();
            }


            existingEvent.Title = dto.Title;
            existingEvent.Description = dto.Description;
            existingEvent.Date = dto.Date;
            existingEvent.Location = dto.Location;


            await _context.SaveChangesAsync();


            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var existingEvent = await _context.Events
                .FindAsync(id);

            if (existingEvent == null)
            {
                return NotFound();
            }


            _context.Events.Remove(existingEvent);

            await _context.SaveChangesAsync();


            return NoContent();
        }
        [HttpPost("{eventId}/attendees")]
        public async Task<IActionResult> RegisterAttendee(
        int eventId,
        RegisterAttendeeDto dto)
        {
            var eventItem = await _context.Events
                .FindAsync(eventId);

            if (eventItem == null)
            {
                return NotFound();
            }


            var attendee = new Attendee
            {
                Name = dto.Name,
                Email = dto.Email,
                EventId = eventId
            };


            _context.Attendees.Add(attendee);

            await _context.SaveChangesAsync();


            var attendeeDto = new AttendeeDto
            {
                Id = attendee.Id,
                Name = attendee.Name,
                Email = attendee.Email
            };


            return CreatedAtAction(
                nameof(GetEvent),
                new { id = eventId },
                attendeeDto
            );
        }

        [HttpDelete("{eventId}/attendees/{attendeeId}")]
        public async Task<IActionResult> RemoveAttendee(
    int eventId,
    int attendeeId)
        {
            var eventItem = await _context.Events
                .FindAsync(eventId);

            if (eventItem == null)
            {
                return NotFound();
            }


            var attendee = await _context.Attendees
                .FirstOrDefaultAsync(a =>
                    a.Id == attendeeId &&
                    a.EventId == eventId);


            if (attendee == null)
            {
                return NotFound();
            }


            _context.Attendees.Remove(attendee);

            await _context.SaveChangesAsync();


            return NoContent();
        }

    }
}