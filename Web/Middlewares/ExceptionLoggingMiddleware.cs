using Microsoft.AspNetCore.Http.Extensions;

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
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.WriteAsync($"<h1>Processing of the request failed. Please provide technical support with the URL, time and request ID</h1><h2>URL: {context.Request.GetEncodedUrl()}<br/>Time: {DateTime.Now}<br/>Request ID: {context.TraceIdentifier}<br/>Technical support contact: </h2>");// Сейчас юзеру отдается время конца обработки запроса. Соответственно, будет некоторый разрыв с временем начала запроса. Если нужно будет отдать время начала, то создаешь и подключаешь в начале конвейера middleware, делающий что-то типа "HttpContext.Items["RequestTime"] = DateTime.Now" или "HttpContext.Features.Set<IHttpRequestTimeFeature>()"
        }
    }
}