using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace REST_Api_Application
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public RequestResponseLoggingMiddleware(RequestDelegate next,
                                                ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory
                      .CreateLogger<RequestResponseLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);

            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            await using var requestStream = new MemoryStream();
            await context.Request.Body.CopyToAsync(requestStream);

            requestStream.Position = 0;
            var RequestBodyData = await new StreamReader(requestStream).ReadToEndAsync();

            _logger.LogInformation($"Http Request Information:\n" +
                                   $"               Schema:{context.Request.Scheme}\n" +
                                   $"               Host: {context.Request.Host}\n" +
                                   $"               Method: {context.Request.Method}\n" +
                                   $"               Path: {context.Request.Path}\n" +
                                   $"               QueryString: {context.Request.QueryString}\n" +
                                   $"               Request Body: {RequestBodyData}\n");
            context.Request.Body.Position = 0;
        }

        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            var watch = Stopwatch.StartNew();
            await _next(context);
            watch.Stop();

            _logger.LogInformation($"Time: {watch.ElapsedMilliseconds}");

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var ResponseBodyData = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation($"Http Response Information:\n" +
                                   $"               StatusCode: {context.Response.StatusCode}\n" +
                                   $"               ContentLength: {context.Response.ContentLength}\n" +
                                   $"               ContentType: {context.Response.ContentType}\n" +
                                   $"               Response Body: {ResponseBodyData}\n");

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
