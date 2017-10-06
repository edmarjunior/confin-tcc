using System;

namespace ConFin.Common.Domain.Dto
{
    public class ContaConjuntaSolicitacaoDto
    {
        public int Id { get; set; }
        public int IdConta { get; set; }
        public string NomeConta { get; set; }
        public int IdUsuarioEnvio { get; set; }
        public string NomeUsuarioEnvio { get; set; }
        public int IdUsuarioConvidado { get; set; }
        public string NomeUsuarioConvidado { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAnalise { get; set; }
        public string IndicadorAprovado { get; set; }
        public string IndicadorEnviadoConvidado { get; set; }
    }
}
