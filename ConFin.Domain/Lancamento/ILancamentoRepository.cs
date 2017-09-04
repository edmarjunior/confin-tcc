using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository.Infra;
using System.Collections.Generic;

namespace ConFin.Domain.Lancamento
{
    public interface ILancamentoRepository: IBaseRepository
    {
        IEnumerable<LancamentoDto> GetAll(int idUsuario, byte? mes = null, short? ano = null, int? idConta = null, int? idCategoria = null);
        LancamentoDto Get(int idLancamento);
        int Post(LancamentoDto lancamento);
        void Put(LancamentoDto lancamento);
        void Delete(int idLancamento);
        void PutIndicadorPagoRecebido(LancamentoDto lancamento);
        LancamentoResumoGeralDto GetResumo(int idUsuario, byte mes, short ano, int? idConta = null, int? idCategoria = null);

        int PostCompromisso(CompromissoDto compromisso);
        int PostCompromissoLancamento(int idCompromisso, int idLancamento, int numeroLancamento);
        void DeleteCompromissoLancamento(int idLancamento);
        IEnumerable<PeriodoDto> GetPeriodo();
        PeriodoDto GetPeriodo(byte id);
    }
}
