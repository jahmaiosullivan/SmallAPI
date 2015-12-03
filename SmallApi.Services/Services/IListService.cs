using SmallApi.Data.Azure;
using SmallApi.Data.Models;

namespace SmallApi.Services.Services
{
    public interface IListService : IAzureTableService<List>
    {
    }
}