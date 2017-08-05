using System;

namespace ConFin.Common.Web
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string ConfirmacaoSenha { get; set; }
        public DateTime DataCadastro { get; set; }

        public DateTime? DataConfirmCadastro { get; set; }
        public DateTime? DataUltimaAlteracao { get; set; }
        public DateTime? DataDesativacao { get; set; }
        public DateTime? DataSolConfirmCadastro { get; set; }
    }
}
