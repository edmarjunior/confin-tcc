using ConFin.Common.Domain.Dto;
using System;

namespace ConFin.Web.ViewModel
{
    public class UsuarioContaConjuntaViewModel
    {
        public UsuarioContaConjuntaViewModel()
        {
            
        }

        public UsuarioContaConjuntaViewModel(UsuarioContaConjuntaDto dto)
        {
            IdContaConjunta = dto.IdContaConjunta;
            IdConta = dto.IdConta;
            DataAnalise = dto.DataAnalise.ToShortDateString();
            IdUsuarioEnvio = dto.IdUsuarioEnvio;
            NomeUsuarioEnvio = dto.NomeUsuarioEnvio;
            IdUsuarioConvidado = dto.IdUsuarioConvidado;
            NomeUsuarioConvidado = dto.NomeUsuarioConvidado;
            EmailUsuarioConvidado = dto.EmailUsuarioConvidado;
            DataBloqueio = dto.DataBloqueio;
            DataDesbloqueio = dto.DataDesbloqueio;
        }

        public int IdContaConjunta { get; set; }
        public int IdConta { get; set; }
        public string DataAnalise { get; set; }
        public int IdUsuarioEnvio { get; set; }
        public string NomeUsuarioEnvio { get; set; }
        public int IdUsuarioConvidado { get; set; }
        public string NomeUsuarioConvidado { get; set; }
        public string EmailUsuarioConvidado { get; set; }
        public DateTime? DataBloqueio { get; set; }
        public DateTime? DataDesbloqueio { get; set; }
    }
}