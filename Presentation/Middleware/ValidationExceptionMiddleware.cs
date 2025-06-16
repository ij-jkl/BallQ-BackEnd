namespace Presentation.Middleware;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var errorMessages = ex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList();

            var response = new ResponseObjectJsonDto
            {
                Code = 400,
                Message = "Validation failed.",
                Response = errorMessages
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}