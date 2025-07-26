namespace MinimalApiExample.PostgreSQL.Data.Models
{
    public class User
    {
        /// <summary>
        /// ID пользователя
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string? firstName { get; set; }
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string? lastName { get; set; }
        /// <summary>
        /// Дата рождения пользователя
        /// </summary>
        public DateOnly dateOfBirth { get; set; }
    }
}
