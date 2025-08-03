namespace MinimalApiExample.PostgreSQL.Data.DTOs
{
    public class UserResponseDTO
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public bool HasBlogs { get; set; }
    }
}
