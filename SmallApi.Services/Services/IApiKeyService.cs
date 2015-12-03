using SmallApi.Data.Azure;
using SmallApi.Data.Models;

namespace SmallApi.Services.Services
{
    public interface IApiKeyService : IAzureTableService<ApiKey>
    {
        ApiKey GetByEmail(string email);
    }
}