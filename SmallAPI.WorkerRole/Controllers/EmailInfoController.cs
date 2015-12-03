using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmallApi.Data.Models;
using SmallApi.Services.Services;
using SmallApi.WorkerRole.Attributes;

namespace SmallApi.WorkerRole.Controllers
{
    [RequiresApiKey]
    public class EmailInfoController : BaseApiController
    {
        private readonly IEmailInfoService emailInfoService;
        private readonly IListService listService;

        public EmailInfoController(IEmailInfoService emailInfoService, IListService listService)
        {
            this.emailInfoService = emailInfoService;
            this.listService = listService;
        }

        public EmailInfo Get(string email, string list)
        {
            var emailInfo = emailInfoService.GetByEmail(list, email, ApiKey.Key);
            if (emailInfo == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Error: Email {email} not found on list {list}!"));
            }
            return emailInfo;
        }


        public IEnumerable<EmailInfo> Get(string list)
        {
            var listObj = listService.Get(ApiKey.Key, list);
            if (listObj == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Error: List {list} not found!"));
            }
            return emailInfoService.GetAll(listObj.Name, listObj.PartitionKey); ;
        }
        public HttpResponseMessage Post(string list, EmailInfo newEmail)
        {
            newEmail.List = list;
            var listObj = listService.Get(ApiKey.Key, newEmail.List);
            if (listObj == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, $"No list with name {newEmail.List} was found for your account."));
            }

            newEmail.ApiKey = ApiKey.Key;
            var result = emailInfoService.Add(newEmail);

            if(!result)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Found, $"Email {newEmail.Email} is already on list  {newEmail.List}."));
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent($"Email {newEmail.Email} has been added to list {newEmail.List}")
            };
        }

        public HttpResponseMessage Delete(EmailInfo email)
        {
            email.ApiKey = ApiKey.Key;
            var list = listService.Get(ApiKey.Key, email.List);
            if (list == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, $"No list with name {email.List} was found for your account."));
            }

            emailInfoService.Delete(email);
            return new HttpResponseMessage
            {
                Content = new StringContent($"{email.Email} has been deleted from list {email.List}")
            };
        }
    }
}
