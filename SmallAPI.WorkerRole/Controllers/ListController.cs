using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmallApi.Data.Models;
using SmallApi.Services.Services;
using SmallApi.WorkerRole.Attributes;

namespace SmallApi.WorkerRole.Controllers
{
    [RequiresApiKey]
    public class ListController : BaseApiController
    {
        private readonly IListService listService;
        public ListController(IListService listService)
        {
            this.listService = listService;
        }

        public IEnumerable<List> Get()
        {
            return listService.All(ApiKey.Key);
        }

        public List Get(string name)
        {
            var list = listService.Get(ApiKey.Key, name);
            if (list == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Error: List {name} not found!"));
            }
            return list;
        }


        public HttpResponseMessage Post(List newList)
        {
            newList.ApiKey = ApiKey.Key;

            if (string.IsNullOrWhiteSpace(newList.Name))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error: Name of list must be supplied. Please contact jahmai.osullivan@onetomany.io to recover it!"));
            }

            var result = listService.Add(newList);
            if (!result)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Found, "Error: You have already created a list by this name. Please delete it or create one by a different name!"));
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent($"List {newList.Name} has been created")
            };
        }

        public HttpResponseMessage Put(List l)
        {
            l.ApiKey = ApiKey.Key;
            listService.Update(l);
            return new HttpResponseMessage
            {
                Content = new StringContent($"List {l.Name} has been updated")
            };
        }

        public HttpResponseMessage Delete(string name)
        {
            var listoDelete = Get(name.ToLower().Trim());
            if (listoDelete == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error: List not found."));
            }

            listService.Delete(listoDelete);
            return new HttpResponseMessage()
            {
                Content = new StringContent($"List {listoDelete.Name} has been deleted")
            };
        }
    }
}
