using ConFin.Common.Domain.Dto;

namespace ConFin.Domain.Login
{
    public interface ILoginService
    {
        UsuarioDto Get(string email, string senha = null);
        void Post(UsuarioDto usuario);
        void PostReenviarSenha(string email);
        void GetVerificaTokenValidoRedefinirSenha(int idUsuario, string token);
    }
}
