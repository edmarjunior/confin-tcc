using ConFin.Common.Domain.Dto;

namespace ConFin.Domain.LancamentoCategoria
{
    public interface ILancamentoCategoriaService
    {
        void Post(LancamentoCategoriaDto categoria);
        void Put(LancamentoCategoriaDto categoria);
        void Delete(int idUsuario, int idCategoria);
    }
}
