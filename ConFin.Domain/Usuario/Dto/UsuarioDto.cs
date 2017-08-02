using ConFin.Common.Domain;
using System;

namespace ConFin.Domain.Usuario.Dto
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataConfirmacaoEmail { get; set; }
        public DateTime? DataUltimaAlteracao { get; set; }
        public DateTime? DataDesativacao { get; set; }


        public bool IsValid(Notification notification)
        {

            if (string.IsNullOrEmpty(Nome))
                notification.Add("O campo [Nome] não foi recebido");

            if (string.IsNullOrEmpty(Email))
                notification.Add("O campo [Email] não foi recebido");

            if (string.IsNullOrEmpty(Senha))
                notification.Add("O campo [Senha] não foi recebido");

            return !notification.Any;

        }
    }
}
