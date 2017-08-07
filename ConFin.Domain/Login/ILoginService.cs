namespace ConFin.Domain.Login
{
    public interface ILoginService
    {
        Common.Domain.Usuario Get(string email, string senha = null);
        void Post(Common.Domain.Usuario usuario);
        void PostReenviarSenha(string email);
        void GetVerificaTokenValidoRedefinirSenha(int idUsuario, string token);
    }
}
