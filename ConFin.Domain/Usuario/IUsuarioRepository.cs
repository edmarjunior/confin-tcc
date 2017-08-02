using ConFin.Domain.Usuario.Dto;

namespace ConFin.Domain.Usuario
{
    public interface IUsuarioRepository
    {
        void Post(UsuarioDto usuario);
        UsuarioDto Get(string email, string senha);
    }
}
