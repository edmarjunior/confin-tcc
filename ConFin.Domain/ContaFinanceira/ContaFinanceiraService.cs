using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.ContaConjunta;
using ConFin.Domain.Usuario;
using System.Linq;

namespace ConFin.Domain.ContaFinanceira
{
    public class ContaFinanceiraService : IContaFinanceiraService
    {
        public readonly IContaFinanceiraRepository ContaFinanceiraRepository;
        public readonly IContaConjuntaRepository ContaConjuntaRepository;
        public readonly Notification Notification;

        public ContaFinanceiraService(Notification notification, IContaFinanceiraRepository contaFinanceiraRepository, IUsuarioRepository usuarioRepository, IContaConjuntaRepository contaConjuntaRepository)
        {
            Notification = notification;
            ContaFinanceiraRepository = contaFinanceiraRepository;
            ContaConjuntaRepository = contaConjuntaRepository;
        }

        public void Post(ContaFinanceiraDto conta)
        {
            if (string.IsNullOrEmpty(conta.Nome))
            {
                Notification.Add("O Nome da conta é obrigatório");
                return;
            }

            conta.Nome = conta.Nome.Trim();

            var contasAtuais = ContaFinanceiraRepository.GetAll(conta.IdUsuarioCadastro).ToList();
            if (contasAtuais.Any(x => x.IdTipo == conta.IdTipo
                                      && x.Nome.Trim().ToLower().Equals(conta.Nome.ToLower())))
            {
                Notification.Add($"Já existe uma conta do Tipo: {contasAtuais.First().NomeTipo}, com o Nome: {conta.Nome}");
                return;
            }

            

            if (!string.IsNullOrEmpty(conta.Descricao))
                conta.Descricao = conta.Descricao.Trim();

            ContaFinanceiraRepository.Post(conta);

        }

        public void Put(ContaFinanceiraDto conta)
        {
            if (conta.IdUsuarioUltimaAlteracao == null)
            {
                Notification.Add("O codigo identificador do usuário não foi enviado pelo sistema, favor reportar o erro");
                return;
            }

            if (string.IsNullOrEmpty(conta.Nome))
            {
                Notification.Add("O Nome da conta é obrigatório");
                return;
            }

            conta.Nome = conta.Nome.Trim();

            var contasAtuais = ContaFinanceiraRepository.GetAll((int)conta.IdUsuarioUltimaAlteracao).Where(x => x.Id != conta.Id).ToList();

            if (contasAtuais.Any(x => x.IdTipo == conta.IdTipo
                                      && x.Nome.Trim().ToLower().Equals(conta.Nome.ToLower())))
            {
                Notification.Add($"Já existe uma conta do Tipo: {contasAtuais.First().NomeTipo}, com o Nome: {conta.Nome}");
                return;
            }



            if (!string.IsNullOrEmpty(conta.Descricao))
                conta.Descricao = conta.Descricao.Trim();

            ContaFinanceiraRepository.Put(conta);
        }

        public void Delete(int idUsuario, int idConta)
        {
            if (ContaFinanceiraRepository.PossuiVinculos(idConta))
                return;

            ContaFinanceiraRepository.OpenTransaction();

            // excluindo categorias vinculadas a conta conjunta (caso a conta à ser excluida for uma conta conjunta)
            var categoriasContaConjunta = ContaConjuntaRepository.GetCategoria(idConta);
            foreach (var categoria in categoriasContaConjunta)
                ContaConjuntaRepository.DeleteCategoria(idConta, categoria.Id);

            // excluindo a conta
            ContaFinanceiraRepository.Delete(idUsuario, idConta);

            ContaFinanceiraRepository.CommitTransaction();
        }

        
    }
}
