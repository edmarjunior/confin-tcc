using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
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

        public void Post(ContaConjuntaDto contaConjunta)
        {
            var usuario = _usuarioRepository.Get(null, contaConjunta.EmailUsuarioConvidado);

            if (usuario == null)
            {
                _notification.Add("Não foi encontrado nenhum usuário com o e-mail informado.");
                return;
            }

            if (usuario.Id == contaConjunta.IdUsuarioEnvio)
            {
                _notification.Add("O e-mail informado não pode ser o mesmo do usuário desta conta");
                return;
            }

            if (_contaConjuntaRepository.Get(null, contaConjunta.IdConta).Any(x => x.IdUsuarioConvidado == usuario.Id))
            {
                _notification.Add("O usuário já esta vinculado com esta conta");
                return;
            }

            contaConjunta.IdUsuarioConvidado = usuario.Id;
            _contaConjuntaRepository.Post(contaConjunta);
        }

        public void Put(ContaConjuntaDto contaConjunta)
        {
            _contaConjuntaRepository.OpenTransaction();
            _contaConjuntaRepository.Put(contaConjunta);

            if(contaConjunta.IndicadorAprovado == "A" && !_contaConjuntaRepository.GetCategoria(contaConjunta.IdConta).Any())
                _contaConjuntaRepository.PostCategorias(contaConjunta.IdConta);

            _contaConjuntaRepository.CommitTransaction();
        }
    }
}
