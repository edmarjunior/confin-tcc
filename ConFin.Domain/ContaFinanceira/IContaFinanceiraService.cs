using ConFin.Common.Domain.Dto;

namespace ConFin.Domain.ContaFinanceira
{
    public interface IContaFinanceiraService
    {
        void Post(ContaFinanceiraDto conta);
        void Put(ContaFinanceiraDto conta);
        void Delete(int idUsuario, int idConta);
        void PostConviteContaConjunta(int idConta, int idUsuarioEnvio, string emailUsuarioConvidado);
        void PutConviteContaConjunta(int idSolicitacao, int idUsuario, string indicadorAprovado);
    }
}
