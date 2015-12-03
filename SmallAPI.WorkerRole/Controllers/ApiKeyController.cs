using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using SmallApi.Data.Models;
using SmallApi.Services.Services;
using SmallApi.WorkerRole.Attributes;

namespace SmallApi.WorkerRole.Controllers
{
    public class ApiKeyController : BaseApiController
    {
        private readonly IApiKeyService apiKeyService;

        public ApiKeyController(IApiKeyService apiKeyService)
        {
            this.apiKeyService = apiKeyService;
        }

        [RequiresApiKey]
        public ApiKey Get(string email)
        {   
            if (string.Compare(ApiKey.Email, "jahmai.osullivan@gmail.com", StringComparison.OrdinalIgnoreCase) !=
                0)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Error: Only jahmai.osullivan@onetomany.io can recover api keys!"));
            }

            return apiKeyService.GetByEmail(email);
        }

        public HttpResponseMessage Post(ApiKey key)
        {
            if (string.IsNullOrWhiteSpace(key.Email))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error: Email must be supplied."));
            }

            if (!string.IsNullOrWhiteSpace(key.Email) && apiKeyService.GetByEmail(key.Email) != null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Found, "Error: An APIKey already exists for this email address. Please contact jahmai.osullivan@onetomany.io to recover it!"));
            }

            var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);

            key.Key = Convert.ToBase64String(bytes);
            apiKeyService.Add(key);

            return new HttpResponseMessage
            {
                Content = new StringContent(key.Key)
            };
        }
    }
}
