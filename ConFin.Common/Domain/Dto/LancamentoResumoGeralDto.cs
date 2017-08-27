namespace ConFin.Common.Domain.Dto
{
    public class LancamentoResumoGeralDto
    {
        public decimal TotReceitasPrevista { get; set; }
        public decimal TotReceitasRealizada { get; set; }
        public decimal TotDespesasPrevista { get; set; }
        public decimal TotDespesasRealizada { get; set; }
        public decimal TotSaldoPrevisto { get; set; }
        public decimal TotSaldoAtual { get; set; }
        public decimal TotValorSaldoInicialConta { get; set; }
    }
}
