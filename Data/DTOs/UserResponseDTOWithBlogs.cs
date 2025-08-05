namespace MinimalApiExample.PostgreSQL.Data.DTOs
{
    /// <summary>
    /// DTO для ответа с данными пользователя и списком его блогов
    /// </summary>
    public class UserResponseDTOWithBlogs
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Дата рождения пользователя
        /// </summary>
        public DateOnly DateOfBirth { get; set; }

        /// <summary>
        /// Список блогов пользователя
        /// </summary>
        public List<BlogResponseDTO> Blogs { get; set; } = [];
    }

}
