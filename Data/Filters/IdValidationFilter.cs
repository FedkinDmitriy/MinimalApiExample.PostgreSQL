
namespace MinimalApiExample.PostgreSQL.Data.Filters
{
    public class IdValidationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var id = context.GetArgument<int>(1); // Индекс зависит от порядка параметров

            if (id <= 0)
                return TypedResults.Problem(
                    detail: "ID должен быть положительным числом",
                    statusCode: StatusCodes.Status400BadRequest
                );
            return await next(context);
        }
    }
}
