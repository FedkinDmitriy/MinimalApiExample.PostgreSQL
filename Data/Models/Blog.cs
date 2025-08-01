namespace MinimalApiExample.PostgreSQL.Data.Models
{
    public class Blog
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Context { get; set; }

        public int UserId { get; set; } //Foreign key

        public User User { get; set; } // навигационое свойство
    }
}
