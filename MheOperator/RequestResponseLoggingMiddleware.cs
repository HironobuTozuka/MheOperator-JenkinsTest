using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Extensions;

namespace MheOperator
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {

                //First, get the incoming request
                var requestBodyStream = new MemoryStream();
                var originalRequestBody = context.Request.Body;

                await context.Request.Body.CopyToAsync(requestBodyStream);
                requestBodyStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = requestBodyStream;

                var request = await FormatRequest(context.Request);

                _logger.LogInformation(request);

                //Create a new memory stream...
                using (var responseBody = new MemoryStream())
                {
                    var originalBodyStream = context.Response.Body;

                    context.Response.Body = responseBody;
                    //Continue down the Middleware pipeline, eventually returning to this class
                    await _next(context);
                    context.Request.Body = originalRequestBody;

                    //Format the response from the server
                    var response = await FormatResponse(context.Response);

                    _logger.LogInformation(response);
                    await responseBody.CopyToAsync(originalBodyStream);

                }
            }
            else
            {
                await _next(context);
            }
            

            
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;


            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //..and finally, set the stream pointer back to 0
            request.Body.Seek(0, SeekOrigin.Begin);

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"{response.StatusCode}: {text}";
        }
    }
}
