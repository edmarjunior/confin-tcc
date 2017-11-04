using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.Compromisso;
using ConFin.Domain.ContaConjunta;
using ConFin.Domain.ContaFinanceira;
using ConFin.Domain.LancamentoCategoria;
using ConFin.Domain.Notificacao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConFin.Domain.Lancamento
{
    public class LancamentoService : ILancamentoService
    {
        public readonly ILancamentoRepository LancamentoRepository;
        public readonly ICompromissoRepository CompromissoRepository;
        public readonly ILancamentoCategoriaRepository LancamentoCategoriaRepository;
        public readonly IContaConjuntaRepository ContaConjuntaRepository;
        public readonly INotificacaoService NotificacaoService;
        public readonly IContaFinanceiraRepository ContaFinanceiraRepository;
        public readonly Notification Notification;

        public LancamentoService(Notification notification, ILancamentoRepository lancamentoRepository,
            ICompromissoRepository compromissoRepository, ILancamentoCategoriaRepository lancamentoCategoriaRepository, 
            IContaConjuntaRepository contaConjuntaRepository, INotificacaoService notificacaoService, IContaFinanceiraRepository contaFinanceiraRepository)
        {
            Notification = notification;
            LancamentoRepository = lancamentoRepository;
            CompromissoRepository = compromissoRepository;
            LancamentoCategoriaRepository = lancamentoCategoriaRepository;
            ContaConjuntaRepository = contaConjuntaRepository;
            NotificacaoService = notificacaoService;
            ContaFinanceiraRepository = contaFinanceiraRepository;
        }

        public IEnumerable<LancamentoDto> GetAll(int idUsuario, byte? mes = null, short? ano = null, int? idConta = null, int? idCategoria = null)
        {
            return LancamentoRepository.GetAll(idUsuario, mes, ano, idConta, idCategoria);
        }

        public void Post(LancamentoDto lancamento)
        {
            LancamentoRepository.OpenTransaction();

            // caso seja conta conjunta aprovada
            AtualizaCategoriasContaConjunta(lancamento.IdConta, lancamento.IdCategoria);

            var conta = ContaFinanceiraRepository.Get(lancamento.IdConta);
            var msg = $"Cadastrou uma nova {(lancamento.IndicadorReceitaDespesa == "R" ? "receita" : "despesa")} ({lancamento.Descricao}) na conta {conta.Nome.ToUpper()}";

            // Cadastra notificações para todos os usuarios (caso seja conta conjunta)
            NotificacaoService.Post(lancamento.IdUsuarioCadastro, lancamento.IdConta, 4, msg); // 4: Cadastro de lançamento em conta conjunta

            if (string.IsNullOrEmpty(lancamento.IndicadorFixoParcelado))
            {
                LancamentoRepository.Post(lancamento);
                LancamentoRepository.CommitTransaction();
                return;
            }

            // get periodos

            if (!lancamento.IdPeriodo.HasValue)
            {
                Notification.Add("Favor informar o periodo para lançamentos fixo/parcelado");
                return;
            }

            var periodo = LancamentoRepository.GetPeriodo((byte)lancamento.IdPeriodo);
            if (periodo == null)
            {
                Notification.Add("Os dados do periodo informado não foram encontrados");
                return;
            }

            var idCompromisso = CompromissoRepository.Post(new CompromissoDto
            {
                Descricao = lancamento.Descricao,
                IdPeriodo = (byte)lancamento.IdPeriodo,
                DataInicio = lancamento.Data,
                TotalParcelasOriginal = lancamento.TotalParcelasOriginal,
                IdUsuarioCadastro = lancamento.IdUsuarioCadastro,
                DataCadastro = DateTime.Now,
                IdConta = lancamento.IdConta
            });

            if (Notification.Any)
                return;

            var cont = 1;
            if (lancamento.IndicadorFixoParcelado == "F")
            {
                var dataLimite = DateTime.Today.AddYears(2);
                while (lancamento.Data <= dataLimite)
                {
                    var idLancamento = LancamentoRepository.Post(lancamento);
                    CompromissoRepository.PostCompromissoLancamento(idCompromisso, idLancamento, cont++);
                    lancamento.Data = AddDate(lancamento.Data, periodo);
                }

                LancamentoRepository.CommitTransaction();
                return;
            }

            // lançamentos parcelados

            if (!lancamento.TotalParcelasOriginal.HasValue || lancamento.TotalParcelasOriginal < 2)
            {
                Notification.Add("Favor informar mais que 1(uma) parcela");
                return;
            }

            for (; cont <= lancamento.TotalParcelasOriginal; cont++)
            {
                var idLancamento = LancamentoRepository.Post(lancamento);
                CompromissoRepository.PostCompromissoLancamento(idCompromisso, idLancamento, cont);
                lancamento.Data = AddDate(lancamento.Data, periodo);
            }

            LancamentoRepository.CommitTransaction();

        }

        public void Post(IEnumerable<LancamentoDto> lancamentos)
        {
            lancamentos = lancamentos.ToList();
            if (!lancamentos.Any())
            {
                Notification.Add("Nenhum lançamento encontrado para cadastrar");
                return;
            }

            var categorias = new Dictionary<string, int>();

            LancamentoRepository.OpenTransaction();

            foreach (var nomeCategoria in lancamentos.GroupBy(x => x.NomeCategoria).Select(y => y.Key))
            {
                // busca o id da categoria, caso não exista é cadastrado uma nova.
                categorias.Add(nomeCategoria,LancamentoCategoriaRepository.GetPostId(nomeCategoria, lancamentos.First().IdUsuarioCadastro));

                if (!Notification.Any)
                    continue;

                LancamentoRepository.RollbackTransaction();
                return;
            }

            foreach (var lancamento in lancamentos)
            {
                lancamento.IdCategoria = categorias[lancamento.NomeCategoria];
                LancamentoRepository.Post(lancamento);
            }

            LancamentoRepository.CommitTransaction();
        }

        public void Delete(int idLancamento, string indTipoDelete, int idUsuario)
        {

            LancamentoRepository.OpenTransaction();

            var dadosLancamento = LancamentoRepository.Get(idLancamento);

            var conta = ContaFinanceiraRepository.Get(dadosLancamento.IdConta);
            var msg = $"Excluiu uma {(dadosLancamento.IndicadorReceitaDespesa == "R" ? "receita" : "despesa")} ({dadosLancamento.Descricao}) da conta {conta.Nome.ToUpper()}";

            // Cadastra notificações para todos os usuarios (caso seja conta conjunta)
            NotificacaoService.Post(idUsuario, dadosLancamento.IdConta, 6, msg); // 6: Remoção de lançamento em conta conjunta

            var compromisso = CompromissoRepository.GetCompromissoLancamento(idLancamento);

            if (compromisso == null)
            {
                LancamentoRepository.Delete(idLancamento);
                LancamentoRepository.CommitTransaction();
                return;
            }

            if (string.IsNullOrEmpty(indTipoDelete) || !new List<string>() { "U", "P", "T" }.Contains(indTipoDelete))
            {
                Notification.Add("Falha ao excluir lançamento fixo/parcelado, o tipo de remoção não foi enviado ou esta inválido");
                return;
            }

            var lancamentosVinculados = CompromissoRepository.GetCompromissoLancamentos(compromisso.Id).ToList();
            List<CompromissoLancamentoDto> lancamentosVinculadosAux;

            switch (indTipoDelete)
            {
                case "U": lancamentosVinculadosAux = lancamentosVinculados.Where(x => x.IdLancamento == idLancamento).ToList(); break;
                case "P": lancamentosVinculadosAux = lancamentosVinculados.Where(x => x.IdLancamento >= idLancamento).ToList(); break;
                default: lancamentosVinculadosAux = lancamentosVinculados.ToList(); break;
            }

            foreach (var lancamento in lancamentosVinculadosAux)
            {
                CompromissoRepository.DeleteCompromissoLancamento(lancamento.IdLancamento);
                LancamentoRepository.Delete(lancamento.IdLancamento);
                lancamentosVinculados.RemoveAll(x => x.IdLancamento == lancamento.IdLancamento);
            }

            if(!lancamentosVinculados.Any())
                CompromissoRepository.Delete(compromisso.Id);

            if (Notification.Any)
                LancamentoRepository.RollbackTransaction();
            else
                LancamentoRepository.CommitTransaction();
        }

        public void Put(LancamentoDto lancamento)
        {
            LancamentoRepository.OpenTransaction();

            // caso seja conta conjunta aprovada
            AtualizaCategoriasContaConjunta(lancamento.IdConta, lancamento.IdCategoria);

            var conta = ContaFinanceiraRepository.Get(lancamento.IdConta);
            var msg = $"Editou o lançamento ({lancamento.Descricao}) da conta {conta.Nome.ToUpper()}";

            if (lancamento.IdUsuarioUltimaAlteracao == null)
            {
                Notification.Add("Não foi passado o usuário que esta realizando a edição no lançamento");
                return;
            }

            // Cadastra notificações para todos os usuarios (caso seja conta conjunta)
            NotificacaoService.Post((int)lancamento.IdUsuarioUltimaAlteracao, lancamento.IdConta, 5, msg); // 5: Edição de lançamento em conta conjunta

            if (!lancamento.IdCompromisso.HasValue || lancamento.IndicadorAcaoCompromisso == "S")
            {
                LancamentoRepository.Put(lancamento);
                LancamentoRepository.CommitTransaction();
                return;
            }

            if (string.IsNullOrEmpty(lancamento.IndicadorAcaoCompromisso))
            {
                Notification.Add("Não foi passado a forma que serão alterados os lançamentos fixo/parcelado que estão vínculados");
                return;
            }

            if (!new List<string> {"P", "T"}.Contains(lancamento.IndicadorAcaoCompromisso))
            {
                Notification.Add("Foi passado uma forma de alteração de lançamentos fixo/parcelado inválida, ");
                return;
            }

            // busca todos lançamentos vínvulados posteriores ao atual;
            var lancamentos = CompromissoRepository.GetCompromissoLancamentos((int)lancamento.IdCompromisso).ToList();
            var dataAnteriorLancamento = lancamentos.First(x => x.IdLancamento == lancamento.Id).DataLancamento;
            var diferenciaDias = (lancamento.Data - dataAnteriorLancamento).TotalDays;

            // caso o IndicadorAcaoCompromisso == "P" altera somente este e os próximos lançamentos vínculados
            if (lancamento.IndicadorAcaoCompromisso == "P")
                lancamentos = lancamentos.Where(x => x.IdLancamento >= lancamento.Id).ToList();

            foreach (var lanc in lancamentos)
            {
                lancamento.Data = lanc.DataLancamento.AddDays(diferenciaDias);
                lancamento.Id = lanc.IdLancamento;
                LancamentoRepository.Put(lancamento);
            }

            LancamentoRepository.CommitTransaction();
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

        private void AtualizaCategoriasContaConjunta(int idConta, int idCategoria)
        {
            if (ContaConjuntaRepository.Get(null, idConta).Any(x => x.IndicadorAprovado == "A")
               && ContaConjuntaRepository.GetCategoria(idConta).All(x => x.Id != idCategoria))
            {
                ContaConjuntaRepository.PostCategoria(idConta, idCategoria);
            }
        }
    }
}
