using ConFin.Common.Domain.Dto;
using System.Collections.Generic;

namespace ConFin.Domain.LancamentoCategoria
{
    public interface ILancamentoCategoriaRepository
    {
        IEnumerable<LancamentoCategoriaDto> Get(int idUsuario);
        LancamentoCategoriaDto Get(int idUsuario, int idCategoria);
        IEnumerable<LancamentoCategoriaDto> GetCategoriasConta(int idConta);

        void Post(LancamentoCategoriaDto categoria);
        void Put(LancamentoCategoriaDto categoria);
        void Delete(int idUsuario, int idCategoria);
        bool PossuiVinculos(int idConta);
        void PostCategoriasIniciaisUsuario(int idUsuario);
        int GetPostId(string nomeCategoria, int idUsuario);
    }
}
