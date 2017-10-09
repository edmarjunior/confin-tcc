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
        private readonly ILancamentoCategoriaRepository _lancamentoCategoriaRepository;
        private readonly IContaFinanceiraRepository _contaFinanceiraRepository;
        private readonly IContaConjuntaRepository _contaConjuntaRepository;
        private readonly Notification _notification;

        public LancamentoCategoriaService(ILancamentoCategoriaRepository lancamentoCategoriaRepository, Notification notification, IContaFinanceiraRepository contaFinanceiraRepository, IContaConjuntaRepository contaConjuntaRepository)
        {
            _lancamentoCategoriaRepository = lancamentoCategoriaRepository;
            _notification = notification;
            _contaFinanceiraRepository = contaFinanceiraRepository;
            _contaConjuntaRepository = contaConjuntaRepository;
        }

        public IEnumerable<LancamentoCategoriaDto> GetCategorias(int idUsuario, int idConta)
        {
            return _contaFinanceiraRepository.Get(idConta).IdUsuarioCadastro == idUsuario 
                ? _lancamentoCategoriaRepository.Get(idUsuario) 
                : _contaConjuntaRepository.GetCategoria(idConta);
        }

        public void Post(LancamentoCategoriaDto categoria)
        {
            if (string.IsNullOrEmpty(categoria.Nome))
            {
                _notification.Add("O Nome da categoria é obrigatório");
                return;
            }

            categoria.Nome = categoria.Nome.Trim();
            var categoriasAtual = _lancamentoCategoriaRepository.Get(categoria.IdUsuarioCadastro).ToList();
            if (categoriasAtual.Any(x => x.Nome.Trim().ToLower().Equals(categoria.Nome.ToLower())))
            {
                _notification.Add($"Já existe uma categoria com o Nome: {categoria.Nome}");
                return;
            }

            _lancamentoCategoriaRepository.Post(categoria);
        }

        public void Put(LancamentoCategoriaDto categoria)
        {
            if (categoria.IdUsuarioUltimaAlteracao == null)
            {
                _notification.Add("O codigo identificador do usuário não foi enviado pelo sistema, favor reportar o erro");
                return;
            }

            if (string.IsNullOrEmpty(categoria.Nome))
            {
                _notification.Add("O Nome da categoria é obrigatório");
                return;
            }

            categoria.Nome = categoria.Nome.Trim();
            var categoriasAtual = _lancamentoCategoriaRepository.Get((int)categoria.IdUsuarioUltimaAlteracao).Where(x => x.Id != categoria.Id).ToList();
            if (categoriasAtual.Any(x => x.Nome.Trim().ToLower().Equals(categoria.Nome.ToLower())))
            {
                _notification.Add($"Já existe uma categoria com o Nome: {categoria.Nome}");
                return;
            }

            _lancamentoCategoriaRepository.Put(categoria);
        }

        public void Delete(int idUsuario, int idCategoria)
        {
            if (!_lancamentoCategoriaRepository.PossuiVinculos(idCategoria))
                _lancamentoCategoriaRepository.Delete(idUsuario, idCategoria);
        }
    }
}
