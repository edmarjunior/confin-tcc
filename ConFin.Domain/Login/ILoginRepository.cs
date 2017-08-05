using ConFin.Common.Domain;
using ConFin.Common.Repository.Infra;

namespace ConFin.Domain.Login
{
    public interface ILoginRepository: IBaseRepository
    {
        void Post(Usuario usuario);
        Usuario Get(string email, string senha = null);
        void PutConfirmacaoCadastro(int idUsuario);
    }
}
