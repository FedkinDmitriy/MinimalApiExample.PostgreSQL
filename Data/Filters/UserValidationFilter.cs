
using MinimalApiExample.PostgreSQL.Data.Models;

namespace MinimalApiExample.PostgreSQL.Data.Filters
{
    public class UserValidationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            // 1. Получаем аргумент User из контекста
            var user = context.Arguments.OfType<User>().FirstOrDefault();

            // 2. Создаем словарь для ошибок
            var errors = new Dictionary<string, string[]>();

            // 3. Валидация
            if (user is null)
            {
                errors.Add("", new[] { "Тело запроса не может быть пустым" });
                return TypedResults.ValidationProblem(errors);
            }

            if (string.IsNullOrEmpty(user.firstName))
            {
                errors.Add("firstName", new[] { "Имя обязательно для заполнения" });
            }
            else if (user.firstName.Length > 30)
            {
                errors.Add("firstName", new[] { "Имя не должно превышать 30 символов" });
            }

            if (string.IsNullOrEmpty(user.lastName))
            {
                errors.Add("lastName", new[] { "Фамилия обязательна для заполнения" });
            }
            else if (user.lastName.Length > 50)
            {
                errors.Add("lastName", new[] { "Фамилия не должна превышать 50 символов" });
            }


            if (errors.Count > 0)
            {
                return TypedResults.ValidationProblem(errors);
            }

            // Если валидация успешна - передаем дальше
            return await next(context);

        }
    }
}
