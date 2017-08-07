using ConFin.Common.Domain;
using ConFin.Domain.Login;

namespace ConFin.Domain.Usuario
{
    public class UsuarioService: IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly ILoginService _loginService;
        
        private readonly Notification _notification;

        public UsuarioService(IUsuarioRepository usuarioRepository, Notification notification, ILoginService loginService, ILoginRepository loginRepository)
        {
            _usuarioRepository = usuarioRepository;
            _notification = notification;
            _loginService = loginService;
            _loginRepository = loginRepository;
        }

        public void PutSenha(int id, string token, string novaSenha)
        {
            _loginService.GetVerificaTokenValidoRedefinirSenha(id, token);
            if (_notification.Any)
                return;

            _usuarioRepository.OpenTransaction();
            _loginRepository.PutSolicitacaoTrocaSenhaLogin(id, token);
            _usuarioRepository.PutSenha(id, novaSenha);
            _usuarioRepository.CommitTransaction();
        }
    }
}
