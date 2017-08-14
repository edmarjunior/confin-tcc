using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository;
using ConFin.Common.Repository.Extension;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.ContaFinanceiraTipo;
using System.Collections.Generic;

namespace ConFin.Repository
{
    public class ContaFinanceiraTipoRepository: BaseRepository, IContaFinanceiraTipoRepository
    {
        public ContaFinanceiraTipoRepository(IDatabaseConnection connection) : base(connection)
        {
        }

        private enum Procedures
        {
            SP_SelContaFinanceiraTipo
        }

        public IEnumerable<ContaFinanceiraTipoDto> Get()
        {
            ExecuteProcedure(Procedures.SP_SelContaFinanceiraTipo);
            var tiposConta = new List<ContaFinanceiraTipoDto>();
            using (var reader = ExecuteReader())
                while (reader.Read())
                    tiposConta.Add(new ContaFinanceiraTipoDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        Nome = reader.ReadAttr<string>("Nome")
                    });

            return tiposConta;
        }
    }
}
