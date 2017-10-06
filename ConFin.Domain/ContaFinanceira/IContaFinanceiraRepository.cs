using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository.Infra;
using System.Collections.Generic;

namespace ConFin.Domain.ContaFinanceira
{
    public interface IContaFinanceiraRepository: IBaseRepository
    {
        IEnumerable<ContaFinanceiraDto> GetAll(int idUsuario);
        ContaFinanceiraDto Get(int idConta);
        void Post(ContaFinanceiraDto conta);
        void Put(ContaFinanceiraDto conta);
        void Delete(int idUsuario, int idConta);
        bool PossuiVinculos(int idConta);
        IEnumerable<UsuarioContaConjuntaDto> GetUsuariosContaConjunta(int idConta);
        UsuarioContaConjuntaDto GetUsuarioContaConjunta(int idConta, int idUsuarioConvidado);
        void PostConviteContaConjunta(int idConta, int idUsuarioEnvio, int idUsuarioConvidado);
        IEnumerable<ContaConjuntaSolicitacaoDto> GetConviteContaConjunta(int idUsuario);
        void PutAprovaReprovaConviteContaConjunta(int idSolicitacao, string indicadorAprovado);
        void PostContaConjunta(int idSolicitacao);
    }
}
