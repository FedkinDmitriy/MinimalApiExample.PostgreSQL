namespace MinimalApiExample.PostgreSQL.Data.Models
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateOnly Birth { get; set; }

        public List<BlogDTO> Blogs { get; set; } = [];
    }
}
