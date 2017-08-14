using ConFin.Common.Domain.Auxiliar;

namespace ConFin.Common.Domain.Dto
{
    public class ContaFinanceiraDto: DataManut
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public byte IdTipo { get; set; }
        public string NomeTipo { get; set; }
        public decimal ValorSaldoInicial { get; set; }
        public string Descricao { get; set; }

        // atributos não mapeados
        public decimal Saldo { get; set; }

    }
}
