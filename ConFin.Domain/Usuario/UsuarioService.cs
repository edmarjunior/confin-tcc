using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
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

        public void Put(UsuarioDto usuario)
        {
            _usuarioRepository.OpenTransaction();

            if (!string.IsNullOrEmpty(usuario.NovaSenha))
                RedefinirSenha(usuario.Id, usuario.Senha, usuario.NovaSenha);

            if (_notification.Any)
            {
                _usuarioRepository.RollbackTransaction();
                return;
            }

            _usuarioRepository.Put(usuario);
            _usuarioRepository.CommitTransaction();

        }

        private void RedefinirSenha(int idUsuario, string senhaAtual, string novaSenha)
        {
            var usuario = _usuarioRepository.Get(idUsuario);

            if (usuario == null)
            {
                _notification.Add($"Não foi encontrado usuario com o id {idUsuario}");
                return;
            }

            if (!_usuarioRepository.SenhaCorreta(idUsuario, senhaAtual))
            {
                _notification.Add("A senha atual enviada esta incorreta");
                return;
            }

            _usuarioRepository.PutSenha(idUsuario, novaSenha);
        }
    }
}
