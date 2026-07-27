using EventManagerAPI.Models;

namespace EventManagerAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Events.Any())
            {
                return;
            }

            var events = new Event[]
            {
                new Event
                {
                    Title = "Career Fair",
                    Description = "A conference for jobs",
                    Date = DateTime.Now.AddDays(10),
                    Location = "Gym"
                },

                new Event
                {
                    Title = "Tech Expo",
                    Description = "A conference for technology",
                    Date = DateTime.Now.AddDays(20),
                    Location = "Auditorium"
                },

                new Event
                {
                    Title = "Hack Night",
                    Description = "Organization of hackers compete",
                    Date = DateTime.Now.AddDays(7),
                    Location = "Library"
                }
            };

            context.Events.AddRange(events);
            context.SaveChanges();

            var attendees = new Attendee[]
            {
                new Attendee
                {
                    Name = "John Smith",
                    Email = "john@email.com",
                    EventId = events[0].Id
                },

                new Attendee
                {
                    Name = "Elliot A",
                    Email = "elliota@email.com",
                    EventId = events[0].Id
                },

                new Attendee
                {
                    Name = "Keeto D",
                    Email = "keetod@email.com",
                    EventId = events[1].Id
                },

                new Attendee
                {
                    Name = "Maya F",
                    Email = "mayaf@email.com",
                    EventId = events[1].Id
                },

                new Attendee
                {
                    Name = "Chippo F",
                    Email = "chippof@email.com",
                    EventId = events[2].Id
                },

                new Attendee
                {
                    Name = "Chippa F",
                    Email = "chippaf@email.com",
                    EventId = events[2].Id
                }
            };

            context.Attendees.AddRange(attendees);
            context.SaveChanges();
        }
    }
}