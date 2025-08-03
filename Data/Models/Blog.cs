namespace MinimalApiExample.PostgreSQL.Data.Models
{
    /// <summary>
    /// Сущность блога, представляющая запись в таблице Blogs базы данных.
    /// Внутренний класс, не предназначен для прямого использования в API.
    /// </summary>
    internal class Blog
    {
        /// <summary>
        /// Уникальный идентификатор блога (первичный ключ).
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Заголовок блога. Может содержать до 30 символов.
        /// </summary>
        /// <example>"Новости разработки"</example>
        public string? Title { get; set; }

        /// <summary>
        /// Дата и время создания блога в UTC.
        /// </summary>
        /// <example>2023-10-15T14:30:00Z</example>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Содержимое блога.
        /// </summary>
        /// <example>"Сегодня мы выпустили новую версию продукта..."</example>
        public string? Context { get; set; }

        /// <summary>
        /// Внешний ключ, связывающий с таблицей Users.
        /// </summary>
        /// <example>42</example>
        public int UserId { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с пользователем (владельцем блога).
        /// </summary>
        public User User { get; set; }
    }
}