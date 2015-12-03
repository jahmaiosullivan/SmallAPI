using System.Collections.Generic;
using SmallApi.Data.Azure;
using SmallApi.Data.Models;

namespace SmallApi.Services.Services
{
    public interface IEmailInfoService : IAzureTableService<EmailInfo>
    {
        EmailInfo GetByEmail(string list, string email, string apiKey);
        IEnumerable<EmailInfo> GetAll(string list, string apiKey);
    }
}