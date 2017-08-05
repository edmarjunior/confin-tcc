using ConFin.Application.AppService;
using ConFin.Application.Interfaces;
using SimpleInjector;

namespace ConFin.Web
{
    public class SimpleInjectorContainer
    {
        public static Container RegisterServices()
        {
            var container = new Container();
           
            container.Register<ILoginAppService, LoginAppService>();

            container.Verify();
            return container;
        }
    }
}
