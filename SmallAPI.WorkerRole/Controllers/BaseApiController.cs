using System.Web.Http;
using SmallApi.Data.Models;

namespace SmallApi.WorkerRole.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected ApiKey ApiKey
        {
            get
            {
                object customObject;
                if (Request.Properties.TryGetValue("ApiKey", out customObject))
                {
                    return ((ApiKey)customObject);
                }
                return null;
            }
        }
    }
}
