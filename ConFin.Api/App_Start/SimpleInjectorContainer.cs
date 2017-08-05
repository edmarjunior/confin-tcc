using ConFin.Common.Domain;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.Login;
using ConFin.Repository;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace ConFin.Api
{
    public class SimpleInjectorContainer
    {
        private static readonly Container Container = new Container();

        public static Container Build()
        {
            Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            Container.Register<IDatabaseConnection, DatabaseConnection>(Lifestyle.Scoped);
            Container.Register<Notification>(Lifestyle.Scoped);

            RegisterRepositories();
            RegisterServices();

            Container.Verify();
            return Container;
        }

        private static void RegisterRepositories()
        {
            Container.Register<ILoginRepository, LoginRepository>();
        }

        private static void RegisterServices()
        {
            Container.Register<ILoginService, LoginService>();
        }
    }
}