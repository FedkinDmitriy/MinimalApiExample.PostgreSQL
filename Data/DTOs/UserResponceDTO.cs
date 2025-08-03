namespace MinimalApiExample.PostgreSQL.Data.DTOs
{
    public class UserResponseDTO
    {
        public int Id { get; set; }

        public string? firstName { get; set; }

        public string? lastName { get; set; }

        public DateOnly dateOfBirth { get; set; }

        public bool HasBlogs { get; set; }
    }
}
