using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.ContaConjunta;
using ConFin.Domain.ContaFinanceira;
using System.Collections.Generic;
using System.Linq;

namespace ConFin.Domain.LancamentoCategoria
{
    public class LancamentoCategoriaService: ILancamentoCategoriaService
    {
        public readonly ILancamentoCategoriaRepository LancamentoCategoriaRepository;
        public readonly IContaFinanceiraRepository ContaFinanceiraRepository;
        public readonly IContaConjuntaRepository ContaConjuntaRepository;
        public readonly Notification Notification;

        public LancamentoCategoriaService(ILancamentoCategoriaRepository lancamentoCategoriaRepository, Notification notification, IContaFinanceiraRepository contaFinanceiraRepository, IContaConjuntaRepository contaConjuntaRepository)
        {
            LancamentoCategoriaRepository = lancamentoCategoriaRepository;
            Notification = notification;
            ContaFinanceiraRepository = contaFinanceiraRepository;
            ContaConjuntaRepository = contaConjuntaRepository;
        }

        public IEnumerable<LancamentoCategoriaDto> GetCategorias(int idUsuario, int idConta)
        {
            return ContaFinanceiraRepository.Get(idConta).IdUsuarioCadastro == idUsuario 
                ? LancamentoCategoriaRepository.GetAll(idUsuario) 
                : ContaConjuntaRepository.GetCategoria(idConta);
        }

        public void Post(LancamentoCategoriaDto categoria)
        {
            if (string.IsNullOrEmpty(categoria.Nome))
            {
                Notification.Add("O Nome da categoria é obrigatório");
                return;
            }

            categoria.Nome = categoria.Nome.Trim();
            var categoriasAtual = LancamentoCategoriaRepository.GetAll(categoria.IdUsuarioCadastro).ToList();
            if (categoriasAtual.Any(x => x.Nome.Trim().ToLower().Equals(categoria.Nome.ToLower())))
            {
                Notification.Add($"Já existe uma categoria com o Nome: {categoria.Nome}");
                return;
            }

            LancamentoCategoriaRepository.Post(categoria);
        }

        public void Put(LancamentoCategoriaDto categoria)
        {
            if (categoria.IdUsuarioUltimaAlteracao == null)
            {
                Notification.Add("O codigo identificador do usuário não foi enviado pelo sistema, favor reportar o erro");
                return;
            }

            if (string.IsNullOrEmpty(categoria.Nome))
            {
                Notification.Add("O Nome da categoria é obrigatório");
                return;
            }

            categoria.Nome = categoria.Nome.Trim();
            var categoriasAtual = LancamentoCategoriaRepository.GetAll((int)categoria.IdUsuarioUltimaAlteracao).Where(x => x.Id != categoria.Id).ToList();
            if (categoriasAtual.Any(x => x.Nome.Trim().ToLower().Equals(categoria.Nome.ToLower())))
            {
                Notification.Add($"Já existe uma categoria com o Nome: {categoria.Nome}");
                return;
            }

            LancamentoCategoriaRepository.Put(categoria);
        }

        public void Delete(int idUsuario, int idCategoria)
        {
            if (!LancamentoCategoriaRepository.PossuiVinculos(idCategoria))
                LancamentoCategoriaRepository.Delete(idUsuario, idCategoria);
        }
    }
}
