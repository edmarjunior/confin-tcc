using System;

namespace ConFin.Common.Domain.Dto
{
    public class NotificacaoDto
    {
        public int Id { get; set; }
        public short IdTipo { get; set; }
        public string DescricaoTipo { get; set; }
        public int IdUsuarioEnvio { get; set; }
        public int IdUsuarioDestino { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataLeitura { get; set; }
        public string ParametrosUrl { get; set; }
    }
}
