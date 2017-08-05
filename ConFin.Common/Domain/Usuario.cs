using System;

namespace ConFin.Common.Domain
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

        public bool IsValid(Notification notification)
        {

            if (string.IsNullOrEmpty(Nome))
                notification.Add("O campo Nome não foi recebido");

            if (string.IsNullOrEmpty(Email))
                notification.Add("O campo Email não foi recebido");

            if (string.IsNullOrEmpty(Senha))
                notification.Add("O campo Senha não foi recebido");

            if (string.IsNullOrEmpty(ConfirmacaoSenha))
                notification.Add("O campo Confirmacao Senha não foi recebido");

            if (!string.IsNullOrEmpty(Senha) && !string.IsNullOrEmpty(ConfirmacaoSenha) && !Senha.Equals(ConfirmacaoSenha))
                notification.Add("Senha não confere com a comfirmação");

            return !notification.Any;
        }
    }
}
