using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository;
using ConFin.Common.Repository.Extension;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.Transferencia;
using System;
using System.Collections.Generic;

namespace ConFin.Repository
{
    public class TransferenciaRepository: BaseRepository, ITransferenciaRepository
    {
        public TransferenciaRepository(IDatabaseConnection connection) : base(connection)
        {
        }

        private enum Procedures
        {
            SP_SelTransferencias,
            SP_SelTransferencia,
            SP_InsTransferencia,
            SP_UpdTransferencia,
            SP_DelTransferencia,
            SP_UpdTransferenciaIndicadorPago
        }

        public IEnumerable<TransferenciaDto> GetAll(int idUsuario)
        {
            ExecuteProcedure(Procedures.SP_SelTransferencias);
            AddParameter("IdUsuario", idUsuario);

            var transferencias = new List<TransferenciaDto>();
            using (var reader = ExecuteReader())
            {
                while (reader.Read())
                {
                    transferencias.Add(new TransferenciaDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        IdContaOrigem = reader.ReadAttr<int>("IdContaOrigem"),
                        NomeContaOrigem = reader.ReadAttr<string>("NomeContaOrigem"),
                        IdContaDestino = reader.ReadAttr<int>("IdContaDestino"),
                        NomeContaDestino = reader.ReadAttr<string>("NomeContaDestino"),
                        Valor = reader.ReadAttr<decimal>("Valor"),
                        Descricao = reader.ReadAttr<string>("Descricao"),
                        Data = reader.ReadAttr<DateTime>("DataTransferencia"),
                        IdCategoria = reader.ReadAttr<int>("IdCategoria"),
                        NomeCategoria = reader.ReadAttr<string>("NomeCategoria"),
                        CorCategoria = reader.ReadAttr<string>("CorCategoria"),
                        IndicadorPagoRecebido = reader.ReadAttr<string>("IndicadorPagoRecebido")
                    });
                }
            }
            return transferencias;
        }

        public TransferenciaDto Get(int idTransferencia)
        {
            ExecuteProcedure(Procedures.SP_SelTransferencia);
            AddParameter("IdTransferencia", idTransferencia);

            using (var reader = ExecuteReader())
            {
                return !reader.Read()
                    ? null
                    : new TransferenciaDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        IdContaOrigem = reader.ReadAttr<int>("IdContaOrigem"),
                        IdContaDestino = reader.ReadAttr<int>("IdContaDestino"),
                        Valor = reader.ReadAttr<decimal>("Valor"),
                        Descricao = reader.ReadAttr<string>("Descricao"),
                        Data = reader.ReadAttr<DateTime>("DataTransferencia"),
                        IdCategoria = reader.ReadAttr<int>("IdCategoria"),
                        IndicadorPagoRecebido = reader.ReadAttr<string>("IndicadorPagoRecebido"),
                        // manut
                        IdUsuarioCadastro = reader.ReadAttr<int>("IdUsuarioCadastro"),
                        NomeUsuarioCadastro = reader.ReadAttr<string>("NomeUsuarioCadastro"),
                        DataCadastro = reader.ReadAttr<DateTime>("DataCadastro"),
                        IdUsuarioUltimaAlteracao = reader.ReadAttr<int?>("IdUsuarioUltimaAlteracao"),
                        NomeUsuarioUltimaAlteracao = reader.ReadAttr<string>("NomeUsuarioUltimaAlteracao"),
                        DataUltimaAlteracao = reader.ReadAttr<DateTime?>("DataUltimaAlteracao")
                    };
            }
        }

        public void Post(TransferenciaDto transferencia)
        {
            ExecuteProcedure(Procedures.SP_InsTransferencia);
            AddParameter("IdContaOrigem", transferencia.IdContaOrigem);
            AddParameter("IdContaDestino", transferencia.IdContaDestino);
            AddParameter("Valor", transferencia.Valor);
            AddParameter("Descricao", transferencia.Descricao);
            AddParameter("Data", transferencia.Data);
            AddParameter("IdCategoria", transferencia.IdCategoria);
            AddParameter("IndicadorPagoRecebido", transferencia.IndicadorPagoRecebido);
            AddParameter("IdUsuarioCadastro", transferencia.IdUsuarioCadastro);
            ExecuteNonQuery();
        }

        public void Put(TransferenciaDto transferencia)
        {
            ExecuteProcedure(Procedures.SP_UpdTransferencia);
            AddParameter("Id", transferencia.Id);
            AddParameter("IdContaOrigem", transferencia.IdContaOrigem);
            AddParameter("IdContaDestino", transferencia.IdContaDestino);
            AddParameter("Valor", transferencia.Valor);
            AddParameter("Descricao", transferencia.Descricao);
            AddParameter("Data", transferencia.Data);
            AddParameter("IdCategoria", transferencia.IdCategoria);
            AddParameter("IndicadorPagoRecebido", transferencia.IndicadorPagoRecebido);
            AddParameter("IdUsuario", transferencia.IdUsuarioUltimaAlteracao);
            ExecuteNonQuery();
        }

        public void Delete(int idTransferencia)
        {
            ExecuteProcedure(Procedures.SP_DelTransferencia);
            AddParameter("Id", idTransferencia);
            ExecuteNonQuery();
        }

        public void PutIndicadorPagoRecebido(TransferenciaDto transferencia)
        {
            ExecuteProcedure(Procedures.SP_UpdTransferenciaIndicadorPago);
            AddParameter("IdTransferencia", transferencia.Id);
            AddParameter("IndicadorPagoRecebido", transferencia.IndicadorPagoRecebido);
            AddParameter("IdUsuario", transferencia.IdUsuarioUltimaAlteracao);
            ExecuteNonQuery();
        }
    }
}
