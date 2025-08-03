using MinimalApiExample.PostgreSQL.Data.Models;

namespace MinimalApiExample.PostgreSQL.Data.DTOs
{
    public class BlogResponseDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Context { get; set; }

    }
}
