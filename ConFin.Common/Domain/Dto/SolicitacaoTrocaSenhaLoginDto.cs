using System;

namespace ConFin.Common.Domain.Dto
{
    public class SolicitacaoTrocaSenhaLoginDto
    {
        public int IdUsuario { get; set; }
        public string Token { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataUsuarioConfirmacao { get; set; }
    }
}
