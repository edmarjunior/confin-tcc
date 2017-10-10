using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.ContaConjunta;
using ConFin.Domain.Usuario;
using System.Linq;

namespace ConFin.Domain.ContaFinanceira
{
    public class ContaFinanceiraService : IContaFinanceiraService
    {
        private readonly IContaFinanceiraRepository _contaFinanceiraRepository;
        private readonly IContaConjuntaRepository _contaConjuntaRepository;
        private readonly Notification _notification;

        public ContaFinanceiraService(Notification notification, IContaFinanceiraRepository contaFinanceiraRepository, IUsuarioRepository usuarioRepository, IContaConjuntaRepository contaConjuntaRepository)
        {
            _notification = notification;
            _contaFinanceiraRepository = contaFinanceiraRepository;
            _contaConjuntaRepository = contaConjuntaRepository;
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
            if (_contaFinanceiraRepository.PossuiVinculos(idConta))
                return;

            _contaFinanceiraRepository.OpenTransaction();

            // excluindo categorias vinculadas a conta conjunta (caso a conta à ser excluida for uma conta conjunta)
            var categoriasContaConjunta = _contaConjuntaRepository.GetCategoria(idConta);
            foreach (var categoria in categoriasContaConjunta)
                _contaConjuntaRepository.DeleteCategoria(idConta, categoria.Id);

            // excluindo a conta
            _contaFinanceiraRepository.Delete(idUsuario, idConta);

            _contaFinanceiraRepository.CommitTransaction();
        }

        
    }
}
