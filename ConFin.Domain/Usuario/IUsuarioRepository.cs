using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository.Infra;

namespace ConFin.Domain.Usuario
{
    public interface IUsuarioRepository: IBaseRepository
    {
        UsuarioDto Get(int? id = null, string email = null);
        void PutSenha(int id, string novaSenha);
        void Put(UsuarioDto usuario);
        bool SenhaCorreta(int idUsuario, string senha);
    }
}
