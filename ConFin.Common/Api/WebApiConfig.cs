using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dependencies;

namespace ConFin.Common.Api
{
    public static class WebApiConfig
    {
        public static void Register(this HttpConfiguration config, IDependencyResolver dependencyResolver)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}", new { controller = "Ping", action = RouteParameter.Optional });
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
            });

            config.DependencyResolver = dependencyResolver;
        }
    }
}
