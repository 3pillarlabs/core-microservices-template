using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Logger
{
    public class LogResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private Func<dynamic,System.Exception,string> _defaultFormatter = (state, exception) => state;

        public LogResponseMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger("HyperLogFilter");
        }

        public async Task Invoke(HttpContext context)
        {

            context.Request.EnableRewind();
            var body = context.Request.Body;
            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            context.Request.Body = body;


            var bodyStream = context.Response.Body;
            var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(responseBodyStream).ReadToEnd();

            var responseCode = context.Response.StatusCode;
            var requestpath = context.Request.Path;
            var requestMethod = context.Request.Method;
            if (responseCode == 200 || responseCode == 404 || responseCode == 400 || responseCode == 401)
            {
                var transactionName = "invoices/v1";
                _logger.LogInformation($"@timestamp: {DateTime.Now},@site: {"hyper-invoices"}, @level: info, @threadid: {Thread.CurrentThread.ManagedThreadId}, @message: Logger Middleware response -{responseBody},requestBody:{requestBody}, requestUrl: {requestpath},requestMethod: {requestMethod}, responseCode: {responseCode}");

                if (!string.IsNullOrEmpty(requestpath.Value) && requestpath.Value.Contains("/"))
                {
                    var requestPaths = requestpath.Value.Split("/").Where(ee => !string.IsNullOrEmpty(ee)).Select(ee => Regex.Replace(ee, @"^\d+$", "{invoiceId}"));
                    if(requestPaths.Any())
                    transactionName = String.Join("/", requestPaths.ToArray());
                }
                Helpers.Helper.NewRelicLogging(transactionName, requestpath, requestMethod, requestBody, responseCode.ToString(), null, responseBody);
            }
            
            responseBodyStream.Seek(0, SeekOrigin.Begin);       
            await responseBodyStream.CopyToAsync(bodyStream);
        }
    }
}
