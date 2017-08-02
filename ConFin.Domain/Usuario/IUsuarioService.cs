using ConFin.Domain.Usuario.Dto;

namespace ConFin.Domain.Usuario
{
    public interface IUsuarioService
    {
        void Post(UsuarioDto usuario);
    }
}
