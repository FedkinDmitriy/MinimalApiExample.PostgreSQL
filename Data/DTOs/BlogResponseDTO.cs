
namespace MinimalApiExample.PostgreSQL.Data.DTOs
{
    /// <summary>
    /// DTO для ответа с данными блога
    /// </summary>
    public class BlogResponseDTO
    {
        /// <summary>
        /// Идентификатор блога
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Заголовок блога
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
