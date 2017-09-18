using ConFin.Common.Domain.Auxiliar;
using System;

namespace ConFin.Common.Domain.Dto
{
    public class LancamentoDto : DataManut
    {
        public int Id { get; set; }
        public string IndicadorReceitaDespesa { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public int IdConta { get; set; }
        public string NomeContaOrigem { get; set; }
        public int? IdContaDestino { get; set; }
        public string NomeContaDestino { get; set; }
        public int IdCategoria { get; set; }
        public string NomeCategoria { get; set; }
        public string CorCategoria { get; set; }
        public string IndicadorPagoRecebido { get; set; }
        public string IndicadorFixoParcelado { get; set; }
        public byte? IdPeriodo { get; set; }
        public short? TotalParcelasOriginal { get; set; }
        public int? IdCompromisso { get; set; }
        public string IndicadorAcaoCompromisso { get; set; }
    }
}
