using ConFin.Common.Repository.Infra;
using ConFin.Domain.Usuario.Dto;

namespace ConFin.Domain.Usuario
{
    public interface IUsuarioRepository: IBaseRepository
    {
        void Post(UsuarioDto usuario);
        UsuarioDto Get(string email, string senha = null);
        void PutConfirmacaoCadastro(int idUsuario);
    }
}
