using Core.Services.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Core.Services.Helpers
{
    public static class Helper
    {
       public static void LogInfo(ILogger logger,string transactionName,string request,string url,string requestMethodType,string statusCode,string response)
        {          
            byte[] byteArray = Encoding.UTF8.GetBytes(request);
            using (var messageStream = new MemoryStream(byteArray))
            {                
                using (var stream = new StreamReader(messageStream))
                {
                    var requestInfo = stream.ReadToEnd();
                    requestInfo = requestInfo.Replace(Environment.NewLine, "");
                    NewRelicLogging(transactionName, url, requestMethodType, request, statusCode, null, response);
                    logger.LogInformation($"@timestamp: {DateTime.Now},@site: {"core-service"}, @level: error, @threadid: {Thread.CurrentThread.ManagedThreadId}, @message: request -{requestInfo},  requestUrl: {url},requestMethod: {requestMethodType}, responseCode: { statusCode},response:{response}");
                }
            }
        }
        
        public static void NewRelicLogging(string transactionName, string url,string requestMethod,string requestMessage,string statusCode, Exception exception,string responseMessage)
        {
            NewRelic.Api.Agent.NewRelic.SetTransactionName("Other", "/" + transactionName);
            NewRelic.Api.Agent.NewRelic.AddCustomParameter("URL: ", url);
            NewRelic.Api.Agent.NewRelic.AddCustomParameter("RequestType: ", requestMethod);
            NewRelic.Api.Agent.NewRelic.AddCustomParameter("Request: ", requestMessage);
            NewRelic.Api.Agent.NewRelic.AddCustomParameter("ResponseStatus: ", statusCode);

            if (exception!=null)
            {
                NewRelic.Api.Agent.NewRelic.AddCustomParameter("Error: ", exception.Message);
                if(!string.IsNullOrEmpty(exception.StackTrace) )
                {
                    NewRelic.Api.Agent.NewRelic.AddCustomParameter("StackTrace: ", exception.StackTrace);
                    if ( exception.StackTrace.Length > 250)
                        NewRelic.Api.Agent.NewRelic.AddCustomParameter("StackTrace1: ", exception.StackTrace.Substring(250));
                }               
            }        
            NewRelic.Api.Agent.NewRelic.AddCustomParameter("Response: ", responseMessage);           
        }
        public static ResponseMessage ConvertToErrorResponse(string errorMessage, string messageId, string messageType)
        {
            ResponseMessage response = new ResponseMessage()
            {
                Message = errorMessage,
                FriendlyMessage = "Some error has occured....",
                MessageId = messageId,
                MessageType = messageType
            };
            return response;
        }
    }
}
