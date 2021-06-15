using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace TPICAP.API.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public RequestResponseLoggingMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            this.next = next;
            this.logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var watcher = Stopwatch.StartNew();
            var userNameText = GetUserName(context);
            string urlText = GetUrlText(context.Request);
            this.logger.LogInformation(await FormatRequest(context.Request, userNameText, urlText));

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await this.next(context);

                watcher.Stop();
                this.logger.LogInformation(await FormatResponse(context.Response, userNameText, urlText, watcher.ElapsedMilliseconds));
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request, string userNameText, string urlText)
        {
            request.EnableBuffering();
            var body = request.Body;

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            request.Body.Seek(0, SeekOrigin.Begin);
            var text = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            var bodyText = this.GetBodyText(text);

            return $"{ DateTime.UtcNow.ToString("yyyy-dd-MM HH:mm:ss")} Request: {urlText}" +
                $"{userNameText}" +
                $"{bodyText}";
        }

        private async Task<string> FormatResponse(HttpResponse response, string userNameText, string urlText, long elapsedMilliseconds)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            
            var bodyText = this.GetBodyText(text);

            return $"{ DateTime.UtcNow.ToString("yyyy-dd-MM HH:mm:ss")} Response: {urlText}" +
                $"{Environment.NewLine}StatusCode: {response.StatusCode.ToString()}" +
                $"{userNameText}" +
                $"{bodyText}" +
                $"{Environment.NewLine}ElapsedMilliseconds: {elapsedMilliseconds.ToString()}";
        }

        private string GetUrlText(HttpRequest request)
        {
            return $"{request.Method} {request.Scheme} {request.Host}{request.Path} {request.QueryString}";
        }

        private string GetBodyText(string bodyText)
        {
            if (string.IsNullOrEmpty(bodyText) == false)
            {
                return $"{Environment.NewLine}Body: {bodyText}";
            }
            return string.Empty;
        }

        private string GetUserName(HttpContext context)
        {
            var userName =  context.User.Claims.SingleOrDefault(claim => claim.Type == "UserName")?.Value;
            if (userName != null) 
            {
                return $"{Environment.NewLine}User: {userName}";
            }
            return string.Empty;
        }
    }
}
