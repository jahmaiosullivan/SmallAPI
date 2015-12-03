using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using SmallApi.Data.Azure;
using SmallApi.Data.Repository;
using StructureMap.Graph;

namespace SmallApi.Services.Configuration
{
    public class CoreRegistry : StructureMap.Registry
    {
        public CoreRegistry()
        {
            For(typeof(ITableRepository<>)).Use(typeof(TableRepository<>));
            For(typeof(IBlobStorageRepository<>)).Use(typeof(BlobStorageRepository<>));
            For(typeof(IAzureTableService<>)).Use(typeof(AzureTableService<>));
            For(typeof(ITableStorageRepository<>)).Use(typeof(TableStorageRepository<>));
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
            });

            For<ICloudClientWrapper>().Use<CloudClientWrapper>().Ctor<CloudStorageAccount>("storageAccount").Is(CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString));
        }
    }
}