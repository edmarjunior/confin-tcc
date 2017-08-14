using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.Lancamento;
using System;
using System.Collections.Generic;

namespace ConFin.Repository
{
    public class LancamentoRepository: BaseRepository, ILancamentoRepository
    {
        public LancamentoRepository(IDatabaseConnection connection) : base(connection)
        {
        }

        private enum Procedures
        {
            
        }

        public IEnumerable<LancamentoDto> GetAll(int idUsuario, int? idConta = null)
        {
            throw new NotImplementedException();
        }

        public LancamentoDto Get(int idLancamento)
        {
            throw new NotImplementedException();
        }

        public void Post(LancamentoDto lancamento)
        {
            throw new NotImplementedException();
        }

        public void Put(LancamentoDto lancamento)
        {
            throw new NotImplementedException();
        }

        public void Delete(int idLancamento)
        {
            throw new NotImplementedException();
        }
    }
}
