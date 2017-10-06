using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.Usuario;
using System.Linq;

namespace ConFin.Domain.ContaFinanceira
{
    public class ContaFinanceiraService : IContaFinanceiraService
    {
        private readonly IContaFinanceiraRepository _contaFinanceiraRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly Notification _notification;

        public ContaFinanceiraService(Notification notification, IContaFinanceiraRepository contaFinanceiraRepository, IUsuarioRepository usuarioRepository)
        {
            _notification = notification;
            _contaFinanceiraRepository = contaFinanceiraRepository;
            _usuarioRepository = usuarioRepository;
        }

        public void Post(ContaFinanceiraDto conta)
        {
            if (string.IsNullOrEmpty(conta.Nome))
            {
                _notification.Add("O Nome da conta é obrigatório");
                return;
            }

            conta.Nome = conta.Nome.Trim();

            var contasAtuais = _contaFinanceiraRepository.GetAll(conta.IdUsuarioCadastro).ToList();
            if (contasAtuais.Any(x => x.IdTipo == conta.IdTipo
                                      && x.Nome.Trim().ToLower().Equals(conta.Nome.ToLower())))
            {
                _notification.Add($"Já existe uma conta do Tipo: {contasAtuais.First().NomeTipo}, com o Nome: {conta.Nome}");
                return;
            }

            

            if (!string.IsNullOrEmpty(conta.Descricao))
                conta.Descricao = conta.Descricao.Trim();

            _contaFinanceiraRepository.Post(conta);

        }

        public void Put(ContaFinanceiraDto conta)
        {
            if (conta.IdUsuarioUltimaAlteracao == null)
            {
                _notification.Add("O codigo identificador do usuário não foi enviado pelo sistema, favor reportar o erro");
                return;
            }

            if (string.IsNullOrEmpty(conta.Nome))
            {
                _notification.Add("O Nome da conta é obrigatório");
                return;
            }

            conta.Nome = conta.Nome.Trim();

            var contasAtuais = _contaFinanceiraRepository.GetAll((int)conta.IdUsuarioUltimaAlteracao).Where(x => x.Id != conta.Id).ToList();

            if (contasAtuais.Any(x => x.IdTipo == conta.IdTipo
                                      && x.Nome.Trim().ToLower().Equals(conta.Nome.ToLower())))
            {
                _notification.Add($"Já existe uma conta do Tipo: {contasAtuais.First().NomeTipo}, com o Nome: {conta.Nome}");
                return;
            }



            if (!string.IsNullOrEmpty(conta.Descricao))
                conta.Descricao = conta.Descricao.Trim();

            _contaFinanceiraRepository.Put(conta);
        }

        public void Delete(int idUsuario, int idConta)
        {
            if(!_contaFinanceiraRepository.PossuiVinculos(idConta))
                _contaFinanceiraRepository.Delete(idUsuario, idConta);
        }

        public void PostConviteContaConjunta(int idConta, int idUsuarioEnvio, string emailUsuarioConvidado)
        {
            var usuario = _usuarioRepository.Get(null, emailUsuarioConvidado);

            if (usuario == null)
            {
                _notification.Add("Não foi encontrado nenhum usuário com o e-mail informado.");
                return;
            }

            var usuarioConvidado = _contaFinanceiraRepository.GetUsuarioContaConjunta(idConta, usuario.Id);

            if (usuarioConvidado != null)
            {
                _notification.Add("O usuário já esta vinculado com esta conta");
                return;
            };
            _contaFinanceiraRepository.PostConviteContaConjunta(idConta, idUsuarioEnvio, usuario.Id);
        }

        public void PutConviteContaConjunta(int idSolicitacao, int idUsuario, string indicadorAprovado)
        {
            _contaFinanceiraRepository.OpenTransaction();
            _contaFinanceiraRepository.PutAprovaReprovaConviteContaConjunta(idSolicitacao, indicadorAprovado);

            if (indicadorAprovado == "A")
                _contaFinanceiraRepository.PostContaConjunta(idSolicitacao);

            _contaFinanceiraRepository.CommitTransaction();

        }
    }
}
