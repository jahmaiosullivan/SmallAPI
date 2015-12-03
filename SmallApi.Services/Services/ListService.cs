using SmallApi.Data.Azure;
using SmallApi.Data.Models;

namespace SmallApi.Services.Services
{
    public class ListService : AzureTableService<List>, IListService
    {
        private readonly IEmailInfoService emailInfoService;
        public ListService(ITableRepository<List> repository, IEmailInfoService emailInfoService) : base(repository)
        {
            this.emailInfoService = emailInfoService;
        }

        public override void Delete(List entity)
        {
            var emailInfos = emailInfoService.GetAll(entity.Name, entity.ApiKey);
            foreach (var em in emailInfos)
            {
                emailInfoService.Delete(em);
            }
            base.Delete(entity);
            
        }
    }
}
