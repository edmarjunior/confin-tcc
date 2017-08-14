using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.Login.Dto;

namespace ConFin.Domain.Login
{
    public interface ILoginRepository: IBaseRepository
    {
        void Post(UsuarioDto usuario);
        UsuarioDto Get(string email, string senha = null);
        void PutConfirmacaoCadastro(int idUsuario);
        void PostSolicitacaoTrocaSenhaLogin(int idUsuario, string token);
        SolicitacaoTrocaSenhaLoginDto GetSolicitacaoTrocaSenhaLogin(int idUsuario, string token);
        void PutSolicitacaoTrocaSenhaLogin(int idUsuario, string token);
    }
}
