using ConFin.Common.Domain.Auxiliar;
using System;

namespace ConFin.Common.Domain.Dto
{
    public class TransferenciaDto: DataManut
    {
        public int Id { get; set; }
        public int IdContaOrigem { get; set; }
        public string NomeContaOrigem { get; set; }
        public int IdContaDestino { get; set; }
        public string NomeContaDestino { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public int IdCategoria { get; set; }
        public string NomeCategoria { get; set; }
        public string CorCategoria { get; set; }
        public string IndicadorPagoRecebido { get; set; }
    }
}
