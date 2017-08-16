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
        public string NomeConta { get; set; }
        public int IdCategoria { get; set; }
        public string NomeCategoria { get; set; }
        public string CorCategoria { get; set; }
        public string IndicadorPagoRecebido { get; set; }
    }
}
