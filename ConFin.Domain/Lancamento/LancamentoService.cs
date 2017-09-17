using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.Compromisso;
using ConFin.Domain.LancamentoCategoria;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConFin.Domain.Lancamento
{
    public class LancamentoService : ILancamentoService
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ICompromissoRepository _compromissoRepository;
        private readonly ILancamentoCategoriaRepository _lancamentoCategoriaRepository;
        private readonly Notification _notification;

        public LancamentoService(Notification notification, ILancamentoRepository lancamentoRepository, ICompromissoRepository compromissoRepository, ILancamentoCategoriaRepository lancamentoCategoriaRepository)
        {
            _notification = notification;
            _lancamentoRepository = lancamentoRepository;
            _compromissoRepository = compromissoRepository;
            _lancamentoCategoriaRepository = lancamentoCategoriaRepository;
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

            var periodo = _lancamentoRepository.GetPeriodo((byte)lancamento.IdPeriodo);
            if (periodo == null)
            {
                _notification.Add("Os dados do periodo informado não foram encontrados");
                return;
            }

            _lancamentoRepository.OpenTransaction();

            var idCompromisso = _compromissoRepository.Post(new CompromissoDto
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
                    _compromissoRepository.PostCompromissoLancamento(idCompromisso, idLancamento, cont++);
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
                _compromissoRepository.PostCompromissoLancamento(idCompromisso, idLancamento, cont);
                lancamento.Data = AddDate(lancamento.Data, periodo);
            }

            _lancamentoRepository.CommitTransaction();

        }

        public void Post(IEnumerable<LancamentoDto> lancamentos)
        {
            lancamentos = lancamentos.ToList();
            if (!lancamentos.Any())
            {
                _notification.Add("Nenhum lançamento encontrado para cadastrar");
                return;
            }

            var categorias = new Dictionary<string, int>();

            _lancamentoRepository.OpenTransaction();

            foreach (var nomeCategoria in lancamentos.GroupBy(x => x.NomeCategoria).Select(y => y.Key))
            {
                // busca o id da categoria, caso não exista é cadastrado uma nova.
                categorias.Add(nomeCategoria,_lancamentoCategoriaRepository.GetPostId(nomeCategoria, lancamentos.First().IdUsuarioCadastro));

                if (!_notification.Any)
                    continue;

                _lancamentoRepository.RollbackTransaction();
                return;
            }

            foreach (var lancamento in lancamentos)
            {
                lancamento.IdCategoria = categorias[lancamento.NomeCategoria];
                _lancamentoRepository.Post(lancamento);
            }

            _lancamentoRepository.CommitTransaction();
        }

        public void Delete(int idLancamento, string indTipoDelete)
        {
            var compromisso = _compromissoRepository.GetCompromissoLancamento(idLancamento);

            if (compromisso == null)
            {
                _lancamentoRepository.Delete(idLancamento);
                return;
            }

            if (string.IsNullOrEmpty(indTipoDelete) || !new List<string>() { "U", "P", "T" }.Contains(indTipoDelete))
            {
                _notification.Add("Falha ao excluir lançamento fixo/parcelado, o tipo de remoção não foi enviado ou esta inválido");
                return;
            }

            var lancamentosVinculados = _compromissoRepository.GetCompromissoLancamentos(compromisso.Id).ToList();
            List<CompromissoLancamentoDto> lancamentosVinculadosAux;

            switch (indTipoDelete)
            {
                case "U": lancamentosVinculadosAux = lancamentosVinculados.Where(x => x.IdLancamento == idLancamento).ToList(); break;
                case "P": lancamentosVinculadosAux = lancamentosVinculados.Where(x => x.IdLancamento >= idLancamento).ToList(); break;
                default: lancamentosVinculadosAux = lancamentosVinculados.ToList(); break;
            }

            _lancamentoRepository.OpenTransaction();

            foreach (var lancamento in lancamentosVinculadosAux)
            {
                _compromissoRepository.DeleteCompromissoLancamento(lancamento.IdLancamento);
                _lancamentoRepository.Delete(lancamento.IdLancamento);
                lancamentosVinculados.RemoveAll(x => x.IdLancamento == lancamento.IdLancamento);
            }

            if(!lancamentosVinculados.Any())
                _compromissoRepository.Delete(compromisso.Id);

            if (_notification.Any)
                _lancamentoRepository.RollbackTransaction();
            else
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
