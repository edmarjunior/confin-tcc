using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using System;
using System.Web;

namespace ConFin.Web.ViewModel
{
    public class NotificacaoViewModel
    {
        public NotificacaoViewModel(NotificacaoDto dto)
        {
            Id = dto.Id;
            IdTipo = dto.IdTipo;
            DescricaoTipo = dto.DescricaoTipo;
            IdUsuarioEnvio = dto.IdUsuarioEnvio;
            NomeUsuarioEnvio = dto.NomeUsuarioEnvio;
            IdUsuarioDestino = dto.IdUsuarioDestino;
            DataCadastro = $"{dto.DataCadastro:U}";
            DataLeitura = dto.DataLeitura;
            ParametrosUrl = dto.ParametrosUrl;
            Mensagem = dto.Mensagem;
        }

        public int Id { get; set; }
        public short IdTipo { get; set; }
        public string DescricaoTipo { get; set; }
        public int IdUsuarioEnvio { get; set; }
        public string NomeUsuarioEnvio { get; set; }
        public int IdUsuarioDestino { get; set; }
        public string DataCadastro { get; set; }
        public DateTime? DataLeitura { get; set; }
        public string ParametrosUrl { get; set; }
        public string Mensagem { get; set; }

        public string Url
        {
            get
            {
                var controllerAction = "";

                switch (IdTipo)
                {
                    case 1: controllerAction = "ContaConjunta/ContaConjunta"; break;
                    case 2: controllerAction = "ContaFinanceira/ContaFinanceira"; break;
                    case 3: controllerAction = "ContaFinanceira/ContaFinanceira"; break;
                    case 4: controllerAction = "Lancamento/Lancamento"; break;
                    case 5: controllerAction = "Lancamento/Lancamento"; break;
                    case 6: controllerAction = "Lancamento/Lancamento"; break;
                    case 7: controllerAction = "Transferencia/Transferencia"; break;
                    case 8: controllerAction = "Transferencia/Transferencia"; break;
                    case 9: controllerAction = "Transferencia/Transferencia"; break;
                }

                return HttpContext.Current.Server.UrlEncode($"{new Parameters().UriWeb}{controllerAction}");
            }
        }
    }
}