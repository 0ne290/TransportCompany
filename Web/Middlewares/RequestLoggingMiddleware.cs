namespace Web.Middlewares;

public class RequestLoggingMiddleware// Если нужно добавить каждому логу, сделанному в рамках одного запроса, какую-то инфу об этом запросе, юзай этот middleware. Все логи, сделанные в рамках скоупа, содержат в себе его инфу
{
     public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
     {
         _next = next;
         _logger = logger;
     }

     public async Task Invoke(HttpContext httpContext)
     {
         using (_logger.BeginScope(Array.Empty<KeyValuePair<string, int>>()))// Вместо пустого массива пар "строковый ключ - числовое значение" вставь свои собственные данные
         {
             await _next(httpContext);
         }
     }
     
     private readonly RequestDelegate _next;

     private readonly ILogger<RequestLoggingMiddleware> _logger;
}