using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using System;
using System.Collections.Generic;

namespace ConFin.Domain.Lancamento
{
    public class LancamentoService: ILancamentoService
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly Notification _notification;

        public LancamentoService(Notification notification, ILancamentoRepository lancamentoRepository)
        {
            _notification = notification;
            _lancamentoRepository = lancamentoRepository;
        }


        public IEnumerable<LancamentoDto> GetAll(int idUsuario, byte? mes = null, short? ano = null, int? idConta = null, int? idCategoria = null)
        {
            return _lancamentoRepository.GetAll(idUsuario, mes, ano, idConta, idCategoria);
        }

        public void Post(LancamentoDto lancamento)
        {
            if (string.IsNullOrEmpty(lancamento.IndicadorFixoParcelado))
            {
                _lancamentoRepository.Post(lancamento);
                return;
            }

            // get periodos

            if (!lancamento.IdPeriodo.HasValue)
            {
                _notification.Add("Favor informar o periodo para lançamentos fixo/parcelado");
                return;
            }

            var periodo = _lancamentoRepository.GetPeriodo((byte) lancamento.IdPeriodo);
            if (periodo == null)
            {
                _notification.Add("Os dados do periodo informado não foram encontrados");
                return;
            }

            _lancamentoRepository.OpenTransaction();

            var idCompromisso = _lancamentoRepository.PostCompromisso(new CompromissoDto
            {
                Descricao = lancamento.Descricao,
                IdPeriodo = (byte)lancamento.IdPeriodo,
                DataInicio = lancamento.Data,
                TotalParcelasOriginal = lancamento.TotalParcelasOriginal,
                IdUsuarioCadastro = lancamento.IdUsuarioCadastro,
                DataCadastro = DateTime.Now,
                IdConta = lancamento.IdConta
            });

            if (_notification.Any)
                return;

            var cont = 1;
            if (lancamento.IndicadorFixoParcelado == "F")
            {
                var dataLimite = DateTime.Today.AddYears(2);
                while (lancamento.Data <= dataLimite)
                {
                    var idLancamento = _lancamentoRepository.Post(lancamento);
                    _lancamentoRepository.PostCompromissoLancamento(idCompromisso, idLancamento, cont++);
                    lancamento.Data = AddDate(lancamento.Data, periodo);
                }

                _lancamentoRepository.CommitTransaction();
                return;
            }

            // lançamentos parcelados

            if (!lancamento.TotalParcelasOriginal.HasValue || lancamento.TotalParcelasOriginal < 2)
            {
                _notification.Add("Favor informar mais que 1(uma) parcela");
                return;
            }

            for (; cont <= lancamento.TotalParcelasOriginal; cont++)
            {
                var idLancamento = _lancamentoRepository.Post(lancamento);
                _lancamentoRepository.PostCompromissoLancamento(idCompromisso, idLancamento, cont);
                lancamento.Data = AddDate(lancamento.Data, periodo);
            }

            _lancamentoRepository.CommitTransaction();

        }

        public void Delete(int idLancamento)
        {
            _lancamentoRepository.OpenTransaction();
            _lancamentoRepository.DeleteCompromissoLancamento(idLancamento);
            _lancamentoRepository.Delete(idLancamento);
            _lancamentoRepository.CommitTransaction();
        }

        private static DateTime AddDate(DateTime dataAtual, PeriodoDto periodo)
        {
            switch (periodo.IndicadorDiaMes)
            {
                case "D": return dataAtual.AddDays(periodo.Quantidade);
                case "M": return dataAtual.AddMonths(periodo.Quantidade);
                default: return dataAtual.AddMonths(periodo.Quantidade);
            }
        }
    }
}
