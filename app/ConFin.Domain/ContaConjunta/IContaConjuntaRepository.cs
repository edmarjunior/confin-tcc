﻿using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository.Infra;
using System.Collections.Generic;

namespace ConFin.Domain.ContaConjunta
{
    public interface IContaConjuntaRepository : IBaseRepository
    {
        IEnumerable<ContaConjuntaDto> Get(int? idUsuario, int? idConta = null, int? idContaConjunta = null);
        void Post(ContaConjuntaDto contaConjunta);
        void Delete(int idContaConjunta);
        void Put(ContaConjuntaDto contaConjunta);
        IEnumerable<LancamentoCategoriaDto> GetCategoria(int idConta);
        void PostCategorias(int idConta);
        void PostCategoria(int idConta, int idCategoria);
        void DeleteCategoria(int idConta, int idCategoria);
    }
}
