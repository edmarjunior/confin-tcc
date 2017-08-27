using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using System.Linq;

namespace ConFin.Domain.LancamentoCategoria
{
    public class LancamentoCategoriaService: ILancamentoCategoriaService
    {
        private readonly ILancamentoCategoriaRepository _lancamentoCategoriaRepository;
        private readonly Notification _notification;

        public LancamentoCategoriaService(ILancamentoCategoriaRepository lancamentoCategoriaRepository, Notification notification)
        {
            _lancamentoCategoriaRepository = lancamentoCategoriaRepository;
            _notification = notification;
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
