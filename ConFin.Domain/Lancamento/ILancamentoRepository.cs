using ConFin.Common.Domain.Dto;
using System.Collections.Generic;

namespace ConFin.Domain.Lancamento
{
    public interface ILancamentoRepository
    {
        IEnumerable<LancamentoDto> GetAll(int idUsuario, int? idConta = null);
        LancamentoDto Get(int idLancamento);
        void Post(LancamentoDto lancamento);
        void Put(LancamentoDto lancamento);
        void Delete(int idLancamento);
    }
}
