using Microsoft.AspNet.SignalR;


namespace ConFin.Web.Hubs
{
    public class Chat : Hub
    {
        public void EnviarMensagem(string apelido, string mensagem)
        {
            if (apelido != "Ed")
            {
                Clients.All.PublicarMensagem(apelido, mensagem);
            }
            else
            {
                Clients.All.NaoExecutaNada(apelido);
            }
        }

        public void AtualizarNotificacoes()
        {
            Clients.All.AtualizaNotificacoes();
        }
    }
}
