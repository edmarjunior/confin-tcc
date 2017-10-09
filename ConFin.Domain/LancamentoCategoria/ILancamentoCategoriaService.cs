using ConFin.Common.Domain.Dto;
using System.Collections.Generic;

namespace ConFin.Domain.LancamentoCategoria
{
    public interface ILancamentoCategoriaService
    {
        IEnumerable<LancamentoCategoriaDto> GetCategorias(int idUsuario, int idConta);
        void Post(LancamentoCategoriaDto categoria);
        void Put(LancamentoCategoriaDto categoria);
        void Delete(int idUsuario, int idCategoria);
    }
}
