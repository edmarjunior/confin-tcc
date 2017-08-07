using ConFin.Common.Repository.Infra;

namespace ConFin.Domain.Usuario
{
    public interface IUsuarioRepository: IBaseRepository
    {
        Common.Domain.Usuario Get(int id);
        void PutSenha(int id, string novaSenha);
    }
}
