namespace MinimalApiExample.PostgreSQL.Data.DTOs
{
    /// <summary>
    /// DTO для создания блога
    /// </summary>
    public class BlogCreateDTO
    {
        /// <summary>
        /// Заголовок блога (обязательное поле, максимум 100 символов)
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Дата создания блога
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Содержимое блога
        /// </summary>
        public string? Context { get; set; }
    }
}
