using ConFin.Common.Domain;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.Compromisso;
using ConFin.Domain.ContaConjunta;
using ConFin.Domain.ContaFinanceira;
using ConFin.Domain.ContaFinanceiraTipo;
using ConFin.Domain.Lancamento;
using ConFin.Domain.LancamentoCategoria;
using ConFin.Domain.Login;
using ConFin.Domain.Notificacao;
using ConFin.Domain.Transferencia;
using ConFin.Domain.Usuario;
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
            Container.Register<IUsuarioRepository, UsuarioRepository>();
            Container.Register<IContaFinanceiraRepository, ContaFinanceiraRepository>();
            Container.Register<IContaFinanceiraTipoRepository, ContaFinanceiraTipoRepository>();
            Container.Register<ILancamentoCategoriaRepository, LancamentoCategoriaRepository>();
            Container.Register<ILancamentoRepository, LancamentoRepository>();
            Container.Register<ITransferenciaRepository, TransferenciaRepository>();
            Container.Register<ICompromissoRepository, CompromissoRepository>();
            Container.Register<IContaConjuntaRepository, ContaConjuntaRepository>();
            Container.Register<INotificacaoRepository, NotificacaoRepository>();
        }

        private static void RegisterServices()
        {
            Container.Register<ILoginService, LoginService>();
            Container.Register<IUsuarioService, UsuarioService>();
           // Container.Register<IContaFinanceiraService, ContaFinanceiraService>();
            Container.Register<ILancamentoCategoriaService, LancamentoCategoriaService>();
            Container.Register<ILancamentoService, LancamentoService>();
            Container.Register<IContaConjuntaService, ContaConjuntaService>();
            Container.Register<INotificacaoService, NotificacaoService>();
            Container.Register<ITransferenciaService, TransferenciaService>();
        }
    }
}
