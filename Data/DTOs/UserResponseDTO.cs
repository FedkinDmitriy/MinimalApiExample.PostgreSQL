namespace MinimalApiExample.PostgreSQL.Data.DTOs
{
    /// <summary>
    /// DTO для ответа с данными пользователя
    /// </summary>
    public class UserResponseDTO
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
        /// Признак наличия блогов у пользователя
        /// </summary>
        public bool HasBlogs { get; set; }
    }

}
