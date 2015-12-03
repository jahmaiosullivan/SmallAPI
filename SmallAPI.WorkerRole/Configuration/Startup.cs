using System.Web.Http;
using Owin;
using SmallApi.Services;
using SmallApi.WorkerRole.DependencyResolution;

namespace SmallApi.WorkerRole.Configuration
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration
            {
                DependencyResolver = new StructureMapWebApiDependencyResolver(ServiceContainer.Current)
            };

            MapRoutes(config);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        private void MapRoutes(HttpConfiguration config)
        {
            const string emailListsApi = "subscriberlists";

            config.Routes.MapHttpRoute(
                "Emails",
                emailListsApi + "/{list}/emails",
                new { controller = "EmailInfo" });

            config.Routes.MapHttpRoute(
                "Lists",
                emailListsApi + "/{list}",
                new { controller = "List", list = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                "Default",
                "{controller}/{id}",
                new { id = RouteParameter.Optional });
        }
    }
}