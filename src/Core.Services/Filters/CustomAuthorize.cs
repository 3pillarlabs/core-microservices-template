using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Core.Services.Configurations;

namespace Core.Services.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CustomAuthorize : ActionFilterAttribute
    {
        private readonly IAppSettings _apiSettings;

        public CustomAuthorize(IAppSettings apiSettings) : base()
        {
            _apiSettings = apiSettings;
        }

        public CustomAuthorize() { }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var headers = context.HttpContext.Request.Headers;
            bool isAuthorized = false;
            var authKeyName = _apiSettings.AuthKeyName;
            var allowedAuthKeys = _apiSettings.ApiKey;

            var allowedKeysList = allowedAuthKeys.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (headers.Keys.Contains(authKeyName) && allowedKeysList.Any())
            {
                var header = headers.FirstOrDefault(x => x.Key == authKeyName).Value.FirstOrDefault();
                if (header != null)
                    isAuthorized = Array.Exists(allowedKeysList, key => key.Equals(header));
            }

            if (!isAuthorized)
            {
                context.Result = new ContentResult()
                {
                    Content = "Authorization has been denied !!",
                    ContentType = "text/plain",
                    StatusCode = 401
                };
            }
        }
    }


}
