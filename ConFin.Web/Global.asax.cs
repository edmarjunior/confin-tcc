using System.Web.Mvc;
using System.Web.Routing;

namespace ConFin.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            DependencyResolver.SetResolver(SimpleInjectorContainer.RegisterServices());
        }
    }
}
