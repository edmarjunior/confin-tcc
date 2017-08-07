using ConFin.Application.AppService.Login;
using ConFin.Application.AppService.Usuario;
using SimpleInjector;

namespace ConFin.Web
{
    public class SimpleInjectorContainer
    {
        public static Container RegisterServices()
        {
            var container = new Container();
           
            container.Register<ILoginAppService, LoginAppService>();
            container.Register<IUsuarioAppService, UsuarioAppService>();

            container.Verify();
            return container;
        }
    }
}
