using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.Login;

namespace ConFin.Domain.Usuario
{
    public class UsuarioService: IUsuarioService
    {
        public readonly IUsuarioRepository UsuarioRepository;
        public readonly ILoginRepository LoginRepository;
        public readonly ILoginService LoginService;
        public readonly Notification Notification;

        public UsuarioService(IUsuarioRepository usuarioRepository, Notification notification, ILoginService loginService, ILoginRepository loginRepository)
        {
            UsuarioRepository = usuarioRepository;
            Notification = notification;
            LoginService = loginService;
            LoginRepository = loginRepository;
        }

        public void PutSenha(int id, string token, string novaSenha)
        {
            LoginService.GetVerificaTokenValidoRedefinirSenha(id, token);
            if (Notification.Any)
                return;

            UsuarioRepository.OpenTransaction();
            LoginRepository.PutSolicitacaoTrocaSenhaLogin(id, token);
            UsuarioRepository.PutSenha(id, novaSenha);
            UsuarioRepository.CommitTransaction();
        }

        public void Put(UsuarioDto usuario)
        {
            UsuarioRepository.OpenTransaction();

            if (!string.IsNullOrEmpty(usuario.NovaSenha))
                RedefinirSenha(usuario.Id, usuario.Senha, usuario.NovaSenha);

            if (Notification.Any)
            {
                UsuarioRepository.RollbackTransaction();
                return;
            }

            UsuarioRepository.Put(usuario);
            UsuarioRepository.CommitTransaction();

        }

        private void RedefinirSenha(int idUsuario, string senhaAtual, string novaSenha)
        {
            var usuario = UsuarioRepository.Get(idUsuario);

            if (usuario == null)
            {
                Notification.Add($"Não foi encontrado usuario com o id {idUsuario}");
                return;
            }

            if (!UsuarioRepository.SenhaCorreta(idUsuario, senhaAtual))
            {
                Notification.Add("A senha atual enviada esta incorreta");
                return;
            }

            UsuarioRepository.PutSenha(idUsuario, novaSenha);
        }
    }
}
