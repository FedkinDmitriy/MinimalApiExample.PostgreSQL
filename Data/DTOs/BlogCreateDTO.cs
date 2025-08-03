namespace MinimalApiExample.PostgreSQL.Data.DTOs
{
    public class BlogCreateDTO
    {
        public string? Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Context { get; set; }
    }
}
