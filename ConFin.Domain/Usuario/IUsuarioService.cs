using ConFin.Common.Domain.Dto;

namespace ConFin.Domain.Usuario
{
    public interface IUsuarioService
    {
        void PutSenha(int id, string token, string novaSenha);
        void Put(UsuarioDto usuario);
    }
}
