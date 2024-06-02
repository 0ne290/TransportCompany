namespace Web.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger logger) // Если нужно добавить каждому логу, сделанному в рамках одного запроса, какую-то инфу об этом запросе, юзай этот middleware. Все логи, сделанные в рамках скоупа, содержат в себе его инфу
{
    public async Task Invoke(HttpContext httpContext)
     {
         using (logger.BeginScope(Array.Empty<KeyValuePair<string, int>>()))// Вместо пустого массива пар "строковый ключ - числовое значение" вставь свои собственные данные
         {
             await next(httpContext);
         }
     }
}