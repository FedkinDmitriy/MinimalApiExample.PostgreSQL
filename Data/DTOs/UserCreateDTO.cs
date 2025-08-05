
namespace MinimalApiExample.PostgreSQL.Data.DTOs
{
    /// <summary>
    /// DTO для создания пользователя
    /// </summary>
    public class UserCreateDTO
    {
        /// <summary>
        /// Имя пользователя (обязательное поле, максимум 30 символов)
        /// </summary>
        public string? firstName { get; set; }

        /// <summary>
        /// Фамилия пользователя (обязательное поле, максимум 50 символов)
        /// </summary>
        public string? lastName { get; set; }

        /// <summary>
        /// Дата рождения пользователя (обязательное поле)
        /// </summary>
        public DateOnly dateOfBirth { get; set; }
    }
}
