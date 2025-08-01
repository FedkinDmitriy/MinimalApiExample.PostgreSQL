namespace MinimalApiExample.PostgreSQL.Data.Models
{
    public class BlogDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Context { get; set; }
    }
}
