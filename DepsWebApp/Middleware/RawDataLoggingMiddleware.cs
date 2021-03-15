using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace DepsWebApp.Middleware
{
    public class RawDataLoggingMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RawDataLoggingMiddleware> _logger;
        private static readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager = 
            new RecyclableMemoryStreamManager();
        public RawDataLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, 
            RecyclableMemoryStreamManager memoryStreamManager)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RawDataLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var requestBody = await ObtainRequestBody(context.Request);
            if (string.IsNullOrEmpty(requestBody))
            {
                _logger.LogDebug($"Request \"{context.Request.Path}\" body is empty!");
            }
            else
            {
                _logger.LogDebug(requestBody);
            }

            var originalBody = context.Response.Body;
            await using var tempResponseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = tempResponseBody;
            
            await _next(context);

            var responseBody = await ObtainResponseBody(context);
            if (string.IsNullOrEmpty(responseBody))
            {
                _logger.LogDebug($"Response \"{context.Request.Path}\" body is empty!");
            }
            else
            {
                _logger.LogDebug(responseBody);
            }
            await tempResponseBody.CopyToAsync(originalBody);
        }

        private static async Task<string> ObtainRequestBody(HttpRequest request) 
        {
            if (request.Body == null || string.IsNullOrEmpty(request.ContentType)) return string.Empty;
            request.EnableBuffering();
            var encoding = GetEncodingFromContentType(request.ContentType);
            string bodyStr;
            using (var reader = new StreamReader(request.Body, encoding, true, 1024, true)) 
            {
                bodyStr = await reader.ReadToEndAsync().ConfigureAwait(false);
            }
            request.Body.Seek(0, SeekOrigin.Begin); 
            return bodyStr; 
        }
        private static async Task<string> ObtainResponseBody(HttpContext context) 
        {
            var response = context.Response;
            response.Body.Seek(0, SeekOrigin.Begin);
            var encoding = GetEncodingFromContentType(response.ContentType);
            using (var reader = new StreamReader(response.Body, encoding,
                detectEncodingFromByteOrderMarks: false, bufferSize: 4096, leaveOpen: true)) 
            {
                var text = await reader.ReadToEndAsync().ConfigureAwait(false);
                response.Body.Seek(0, SeekOrigin.Begin);
                return text;
            }
        }
        private static Encoding GetEncodingFromContentType(string contentTypeStr)
        {
            if (string.IsNullOrEmpty(contentTypeStr))
            {
                return Encoding.UTF8;
            }
            ContentType contentType;
            try
            {
                contentType = new ContentType(contentTypeStr);
            }
            catch (FormatException)
            {
                return Encoding.UTF8;
            }
            if (string.IsNullOrEmpty(contentType.CharSet))
            {
                return Encoding.UTF8;
            }
            return Encoding.GetEncoding(contentType.CharSet, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
        }
    }
}
