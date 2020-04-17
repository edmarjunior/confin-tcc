using System;

namespace ConFin.Common.Domain.Dto
{
    public class CompromissoLancamentoDto
    {
        public int IdCompromisso { get; set; }
        public int IdLancamento { get; set; }
        public short NumeroLancamento { get; set; }
        public DateTime DataLancamento { get; set; }
    }
}
