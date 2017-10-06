using ConFin.Common.Domain.Dto;
using System;

namespace ConFin.Web.ViewModel
{
    public class ContaConjuntaSolicitacaoViewModel
    {
        public ContaConjuntaSolicitacaoViewModel()
        {
        }

        public ContaConjuntaSolicitacaoViewModel(ContaConjuntaSolicitacaoDto dto)
        {
            Id = dto.Id;
            IdConta = dto.IdConta;
            NomeConta = dto.NomeConta;
            IdUsuarioEnvio = dto.IdUsuarioEnvio;
            NomeUsuarioEnvio = dto.NomeUsuarioEnvio;
            IdUsuarioConvidado = dto.IdUsuarioConvidado;
            NomeUsuarioConvidado = dto.NomeUsuarioConvidado;
            DataCadastro = $"{dto.DataCadastro:G}";
            DataAnalise = dto.DataAnalise;
            IndicadorAprovado = dto.IndicadorAprovado;
            IndicadorEnviadoConvidado = dto.IndicadorEnviadoConvidado;
        }

        public int Id { get; set; }
        public int IdConta { get; set; }
        public string NomeConta { get; set; }
        public int IdUsuarioEnvio { get; set; }
        public string NomeUsuarioEnvio { get; set; }
        public int IdUsuarioConvidado { get; set; }
        public string NomeUsuarioConvidado { get; set; }
        public string DataCadastro { get; set; }
        public DateTime? DataAnalise { get; set; }
        public string IndicadorAprovado { get; set; }
        public string IndicadorEnviadoConvidado { get; set; }
    }
}
