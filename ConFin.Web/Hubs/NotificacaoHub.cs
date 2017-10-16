using Microsoft.AspNet.SignalR;


namespace ConFin.Web.Hubs
{
    public class NotificacaoHub : Hub
    {
        public void AtualizarNotificacoes()
        {
            Clients.All.AtualizaNotificacoes();
        }
    }
}
