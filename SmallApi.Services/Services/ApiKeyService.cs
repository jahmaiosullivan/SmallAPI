using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SmallApi.Data.Azure;
using SmallApi.Data.Models;

namespace SmallApi.Services.Services
{
    public class ApiKeyService : AzureTableService<ApiKey>, IApiKeyService
    {
        public ApiKeyService(ITableRepository<ApiKey> repository) : base(repository)
        {
        }

        public ApiKey GetByEmail(string email)
        {
            return this.Repository.FindAll()
                .FirstOrDefault(x => string.Compare(x.Email, email, StringComparison.OrdinalIgnoreCase) == 0);
        }
    }
}
