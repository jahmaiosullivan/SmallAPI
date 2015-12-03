using System;
using System.Collections.Generic;
using System.Linq;
using SmallApi.Data.Azure;
using SmallApi.Data.Models;

namespace SmallApi.Services.Services
{
    public class EmailInfoService : AzureTableService<EmailInfo>, IEmailInfoService
    {
        public EmailInfoService(ITableRepository<EmailInfo> repository) : base(repository)
        {
        }

        public EmailInfo GetByEmail(string list, string email, string apiKey)
        {
            return GetAll(list, apiKey)
                .FirstOrDefault(x => string.Compare(x.Email, email.Trim(), StringComparison.OrdinalIgnoreCase) == 0);
        }

        public IEnumerable<EmailInfo> GetAll(string list, string partitionKey)
        {
            return Repository.FindAll(partitionKey)
                .Where(x => string.Compare(x.List, list.Trim(), StringComparison.OrdinalIgnoreCase) == 0).OrderByDescending(x => x.Timestamp);
        }
    }
}
