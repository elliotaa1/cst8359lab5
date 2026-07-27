namespace EventManagerAPI.DTOs
{
    public class CreateEventDto
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public string? Location { get; set; }
    }
}