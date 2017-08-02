using ConFin.Common.Domain;
using ConFin.Domain.Usuario.Dto;

namespace ConFin.Domain.Usuario
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private Notification _notification;


        public UsuarioService(IUsuarioRepository usuarioRepository, Notification notification)
        {
            _usuarioRepository = usuarioRepository;
            _notification = notification;
        }

        public void Post(UsuarioDto usuario)
        {
            if(usuario == null)
            {
                _notification.Add("Usuário não enviado para cadastro");
                return;
            }

            if (!usuario.IsValid(_notification))
                return;

            _usuarioRepository.Post(usuario);
        }
    }
}
