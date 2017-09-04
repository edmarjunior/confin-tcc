using ConFin.Common.Domain.Dto;
using System.Collections.Generic;

namespace ConFin.Domain.Lancamento
{
    public interface ILancamentoService
    {
        IEnumerable<LancamentoDto> GetAll(int idUsuario, byte? mes = null, short? ano = null, int? idConta = null, int? idCategoria = null);
        void Post(LancamentoDto lancamento);
        void Delete(int idLancamento);
    }
}
