namespace MinimalApiExample.PostgreSQL.Data.Models
{
    /// <summary>
    /// Сущность пользователя, представляющая запись в таблице Users базы данных.
    /// Внутренний класс, не предназначен для прямого использования в API.
    /// </summary>
    internal class User
    {
        /// <summary>
        /// Уникальный идентификатор пользователя (первичный ключ).
        /// </summary>
        /// <example>123</example>
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя. Может содержать до 30 символов.
        /// </summary>
        /// <example>"Иван"</example>
        public string? firstName { get; set; }

        /// <summary>
        /// Фамилия пользователя. Может содержать до 50 символов.
        /// </summary>
        /// <example>"Петров"</example>
        public string? lastName { get; set; }

        /// <summary>
        /// Дата рождения пользователя в формате ГГГГ-ММ-ДД.
        /// </summary>
        /// <example>1990-05-15</example>
        public DateOnly dateOfBirth { get; set; }

        /// <summary>
        /// Навигационное свойство для коллекции блогов пользователя.
        /// Связь один-ко-многим с сущностью <see cref="Blog"/>.
        /// </summary>
        /// <remarks>
        /// Инициализируется пустой коллекцией для предотвращения null reference.
        /// </remarks>
        public ICollection<Blog> Blogs { get; set; } = [];
    }
}