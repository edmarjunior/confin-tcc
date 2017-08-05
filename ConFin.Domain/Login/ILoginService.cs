using ConFin.Common.Domain;

namespace ConFin.Domain.Login
{
    public interface ILoginService
    {
        Usuario Get(string email, string senha = null);
        void Post(Usuario usuario);
    }
}
