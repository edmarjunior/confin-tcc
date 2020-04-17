using ConFin.Application.AppService.AcessoOpcaoMenu;
using ConFin.Application.AppService.ContaConjunta;
using ConFin.Application.AppService.ContaFinanceira;
using ConFin.Application.AppService.ContaFinanceiraTipo;
using ConFin.Application.AppService.Lancamento;
using ConFin.Application.AppService.LancamentoCategoria;
using ConFin.Application.AppService.Login;
using ConFin.Application.AppService.Notificacao;
using ConFin.Application.AppService.Transferencia;
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
            container.Register<IContaFinanceiraTipoAppService, ContaFinanceiraTipoAppService>();
            container.Register<IContaFinanceiraAppService, ContaFinanceiraAppService>();
            container.Register<ILancamentoCategoriaAppService, LancamentoCategoriaAppService>();
            container.Register<ILancamentoAppService, LancamentoAppService>();
            container.Register<ITransferenciaAppService, TransferenciaAppService>();
            container.Register<IContaConjuntaAppService, ContaConjuntaAppService>();
            container.Register<INotificacaoAppService, NotificacaoAppService>();
            container.Register<IAcessoOpcaoMenuAppService, AcessoOpcaoMenuAppService>();

            container.Verify();
            return container;
        }
    }
}
