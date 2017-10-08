using ConFin.Common.Domain;
using ConFin.Domain.Usuario;
using System.Linq;

namespace ConFin.Domain.ContaConjunta
{
    public class ContaConjuntaService: IContaConjuntaService
    {
        private readonly IContaConjuntaRepository _contaConjuntaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly Notification _notification;


        public ContaConjuntaService(IContaConjuntaRepository contaConjuntaRepository, IUsuarioRepository usuarioRepository, Notification notification)
        {
            _contaConjuntaRepository = contaConjuntaRepository;
            _usuarioRepository = usuarioRepository;
            _notification = notification;
        }

        public void Post(int idConta, int idUsuarioEnvio, string emailUsuarioConvidado)
        {
            var usuario = _usuarioRepository.Get(null, emailUsuarioConvidado);

            if (usuario == null)
            {
                _notification.Add("Não foi encontrado nenhum usuário com o e-mail informado.");
                return;
            }

            if (usuario.Id == idUsuarioEnvio)
            {
                _notification.Add("O e-mail informado não pode ser o mesmo do usuário desta conta");
                return;
            }

            if (_contaConjuntaRepository.Get(null, idConta).Any(x => x.IdUsuarioConvidado == usuario.Id))
            {
                _notification.Add("O usuário já esta vinculado com esta conta");
                return;
            }

            _contaConjuntaRepository.Post(idConta, idUsuarioEnvio, usuario.Id);
        }
    }
}
