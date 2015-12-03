using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using SmallApi.Data.Models;
using SmallApi.Services;
using SmallApi.Services.Services;

namespace SmallApi.WorkerRole.Attributes
{
    public class RequiresApiKeyAttribute : ActionFilterAttribute
    {
        protected const string ApiKeyHeaderKey = "apikey";

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                IEnumerable<string> headerValues;

                if (!actionContext.Request.Headers.TryGetValues(ApiKeyHeaderKey, out headerValues) || ((headerValues.ToList()).Count == 0))
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Valid api key is required for api requests.");
                    return;
                }

                var apiKeyService = ServiceContainer.Current.GetInstance<IApiKeyService>();
                var apiKey = apiKeyService.Get(headerValues.ToList()[0]);

                if (apiKey == null)
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "The specified api key is not valid.");
                }
                else if (!apiKey.IsActive)
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "The specified api key has been suspended. Please contact jahmai.osullivan@onetomany.io to have this sorted out.");
                }

                actionContext.Request.Properties.Add(new KeyValuePair<string, object>("ApiKey", apiKey));
            }
            catch (Exception)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unknown error has occurred. Please try again or contact jahmai.osullivan@onetomany.io.");
            }
        }

        
    }
}