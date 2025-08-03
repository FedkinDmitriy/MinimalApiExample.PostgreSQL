
namespace MinimalApiExample.PostgreSQL.Data.DTOs
{
    public class UserCreateDTO
    {
        public string? firstName { get; set; }

        public string? lastName { get; set; }

        public DateOnly dateOfBirth { get; set; }

    }
}
