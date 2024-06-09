using Web.Pages;

namespace Web.Middlewares;

public class ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try 
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Application error");
            await ErrorPages.Return500(context);
        }
    }
}