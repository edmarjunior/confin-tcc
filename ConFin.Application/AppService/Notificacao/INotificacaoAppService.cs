using System.Net.Http;

namespace ConFin.Application.AppService.Notificacao
{
    public interface INotificacaoAppService
    {
        HttpResponseMessage Get(int idUsuario, bool notificacaoLida);
        HttpResponseMessage GetTotalNaoLidas(int idUsuario);
    }
}
