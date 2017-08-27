using ConFin.Common.Domain.Auxiliar;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web.Extension;
using System.Collections.Generic;

namespace ConFin.Web.ViewModel
{
    public class ContaFinanceiraViewModel: DataManut
    {
        public ContaFinanceiraViewModel(IEnumerable<ContaFinanceiraTipoDto> tiposConta)
        {
            TiposConta = tiposConta;
        }

        public ContaFinanceiraViewModel(ContaFinanceiraDto model, IEnumerable<ContaFinanceiraTipoDto> tiposConta = null)
        {
            Id = model.Id;
            Nome = model.Nome;
            IdTipo = model.IdTipo;
            NomeTipo = model.NomeTipo;
            ValorSaldoInicial = model.ValorSaldoInicial;
            Descricao = model.Descricao;
            Saldo = model.Saldo;

            IdUsuarioCadastro = model.IdUsuarioCadastro;
            NomeUsuarioCadastro = model.NomeUsuarioCadastro;
            DataCadastro = model.DataCadastro;
            IdUsuarioUltimaAlteracao = model.IdUsuarioUltimaAlteracao;
            NomeUsuarioUltimaAlteracao = model.NomeUsuarioUltimaAlteracao;
            DataUltimaAlteracao = model.DataUltimaAlteracao;
            TiposConta = tiposConta;
        }

        public int? Id { get; set; }
        public string Nome { get; set; }
        public string NomeTipo { get; set; }
        public byte? IdTipo { get; set; }
        public decimal? ValorSaldoInicial { get; set; }
        public string Descricao { get; set; }
        public decimal? Saldo { get; set; }

        public string ValorSaldoInicialFormat => ValorSaldoInicial.ToMoney("0,00");
        public string ValorSaldoAtual => Saldo.ToMoney("0,00");


        public IEnumerable<ContaFinanceiraTipoDto> TiposConta { get; set; }
    }
}
