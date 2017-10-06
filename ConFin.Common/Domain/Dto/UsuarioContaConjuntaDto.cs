using System;

namespace ConFin.Common.Domain.Dto
{
    public class UsuarioContaConjuntaDto
    {
        public int IdContaConjunta { get; set; }
        public int IdConta { get; set; }
        public DateTime DataAnalise { get; set; }
        public int IdUsuarioEnvio { get; set; }
        public string NomeUsuarioEnvio { get; set; }
        public int IdUsuarioConvidado { get; set; }
        public string NomeUsuarioConvidado { get; set; }
        public string EmailUsuarioConvidado { get; set; }
        public DateTime? DataBloqueio { get; set; }
        public DateTime? DataDesbloqueio { get; set; }
    }
}
