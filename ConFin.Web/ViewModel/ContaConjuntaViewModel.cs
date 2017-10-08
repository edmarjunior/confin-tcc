using ConFin.Common.Domain.Dto;

namespace ConFin.Web.ViewModel
{
    public class ContaConjuntaViewModel
    {
        public ContaConjuntaViewModel()
        {
            
        }

        public ContaConjuntaViewModel(ContaConjuntaDto dto, string indicadorProprietarioConta, int idUsuarioLogado)
        {
            Id = dto.Id;
            IdConta = dto.IdConta;
            DataAnalise = dto.DataAnalise.HasValue ? $"{dto.DataAnalise:d}" : "";
            DataCadastro = dto.DataCadastro.ToShortDateString();
            IdUsuarioEnvio = dto.IdUsuarioEnvio;
            NomeUsuarioEnvio = dto.NomeUsuarioEnvio;
            IdUsuarioConvidado = dto.IdUsuarioConvidado;
            NomeUsuarioConvidado = dto.NomeUsuarioConvidado;
            EmailUsuarioConvidado = dto.EmailUsuarioConvidado;
            IndicadorAprovado = dto.IndicadorAprovado;
            EmailUsuarioEnvio = dto.EmailUsuarioEnvio;
            NomeConta = dto.NomeConta;
            PodeRemoverContaConjunta = indicadorProprietarioConta == "S" || idUsuarioLogado == IdUsuarioConvidado;
        }

        public ContaConjuntaViewModel(ContaConjuntaDto dto)
        {
            Id = dto.Id;
            IdConta = dto.IdConta;
            DataAnalise = dto.DataAnalise.HasValue ? $"{dto.DataAnalise:d}" : "";
            DataCadastro = dto.DataCadastro.ToShortDateString();
            IdUsuarioEnvio = dto.IdUsuarioEnvio;
            NomeUsuarioEnvio = dto.NomeUsuarioEnvio;
            IdUsuarioConvidado = dto.IdUsuarioConvidado;
            NomeUsuarioConvidado = dto.NomeUsuarioConvidado;
            EmailUsuarioConvidado = dto.EmailUsuarioConvidado;
            IndicadorAprovado = dto.IndicadorAprovado;
            EmailUsuarioEnvio = dto.EmailUsuarioEnvio;
            NomeConta = dto.NomeConta;
        }

        public int Id { get; set; }
        public int IdConta { get; set; }
        public string DataAnalise { get; set; }
        public int IdUsuarioEnvio { get; set; }
        public string NomeUsuarioEnvio { get; set; }
        public int IdUsuarioConvidado { get; set; }
        public string NomeUsuarioConvidado { get; set; }
        public string EmailUsuarioConvidado { get; set; }
        public string DataCadastro { get; set; }
        public string IndicadorAprovado { get; set; }
        public string EmailUsuarioEnvio { get; set; }
        public string NomeConta { get; set; }
        public bool PodeRemoverContaConjunta { get; set; }

        public string Status => string.IsNullOrEmpty(IndicadorAprovado)
                                    ? "Pendente"
                                    : IndicadorAprovado == "A" ? "Compartilhando" : "Recusado";
    }
}
