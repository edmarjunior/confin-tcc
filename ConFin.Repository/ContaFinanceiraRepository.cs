using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository;
using ConFin.Common.Repository.Extension;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.ContaFinanceira;
using System;
using System.Collections.Generic;

namespace ConFin.Repository
{
    public class ContaFinanceiraRepository : BaseRepository, IContaFinanceiraRepository
    {
        private readonly Notification _notification;

        public ContaFinanceiraRepository(IDatabaseConnection connection, Notification notification) : base(connection)
        {
            _notification = notification;
        }

        private enum Procedures
        {
            SP_SelContasFinanceira,
            SP_SelContaFinanceira,
            SP_InsContaFinanceira,
            SP_UpdContaFinanceira,
            SP_DelContaFinanceira,
            FNC_ContaPossuiVinculos,
            SP_InsContaConjuntaSolicitacao,
            SP_SelContaConjuntaUsuarios,
            SP_SelContaConjuntaSolicitacao,
            SP_UpdAprovaReprovaContaConjuntaSolicitacao,
            SP_InsContaConjunta
        }

        public IEnumerable<ContaFinanceiraDto> GetAll(int idUsuario)
        {
            ExecuteProcedure(Procedures.SP_SelContasFinanceira);
            AddParameter("IdUsuario", idUsuario);
            var contas = new List<ContaFinanceiraDto>();
            using (var reader = ExecuteReader())
                while (reader.Read())
                    contas.Add(new ContaFinanceiraDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        Nome = reader.ReadAttr<string>("Nome"),
                        IdTipo = reader.ReadAttr<byte>("IdTipo"),
                        NomeTipo = reader.ReadAttr<string>("NomeTipo"),
                        ValorSaldoInicial = reader.ReadAttr<decimal>("ValorSaldoInicial"),
                        Descricao = reader.ReadAttr<string>("Descricao"),
                        Saldo = reader.ReadAttr<decimal>("Saldo")
                    });

            return contas;
        }

        public ContaFinanceiraDto Get(int idConta)
        {
            ExecuteProcedure(Procedures.SP_SelContaFinanceira);
            AddParameter("IdConta", idConta);

            using (var reader = ExecuteReader())
            {
                return !reader.Read()
                    ? null
                    : new ContaFinanceiraDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        Nome = reader.ReadAttr<string>("Nome"),
                        IdTipo = reader.ReadAttr<byte>("IdTipo"),
                        ValorSaldoInicial = reader.ReadAttr<decimal>("ValorSaldoInicial"),
                        Descricao = reader.ReadAttr<string>("Descricao"),
                        Saldo = reader.ReadAttr<decimal>("Saldo"),
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

        public void Post(ContaFinanceiraDto conta)
        {
            ExecuteProcedure(Procedures.SP_InsContaFinanceira);
            AddParameter("Nome", conta.Nome);
            AddParameter("IdTipo", conta.IdTipo);
            AddParameter("ValorSaldoInicial", conta.ValorSaldoInicial);
            AddParameter("Descricao", conta.Descricao);
            AddParameter("IdUsuarioCadastro", conta.IdUsuarioCadastro);
            ExecuteNonQuery();
        }

        public void Put(ContaFinanceiraDto conta)
        {
            ExecuteProcedure(Procedures.SP_UpdContaFinanceira);
            AddParameter("IdConta", conta.Id);
            AddParameter("IdUsuario", conta.IdUsuarioUltimaAlteracao);
            AddParameter("Nome", conta.Nome);
            AddParameter("IdTipo", conta.IdTipo);
            AddParameter("ValorSaldoInicial", conta.ValorSaldoInicial);
            AddParameter("Descricao", conta.Descricao);
            ExecuteNonQuery();
        }

        public void Delete(int idUsuario, int idConta)
        {
            ExecuteProcedure(Procedures.SP_DelContaFinanceira);
            // AddParameter("IdUsuario", idUsuario);
            AddParameter("IdConta", idConta);
            ExecuteNonQuery();
        }

        public bool PossuiVinculos(int idConta)
        {
            ExecuteProcedure(Procedures.FNC_ContaPossuiVinculos);
            AddParameter("IdConta", idConta);
            var retorno = ExecuteNonQueryWithReturn<byte>();

            string msg;

            switch (retorno)
            {
                case 0:
                    return false;
                case 1:
                    msg = "A conta não pode ser excluida por possuir vinculo com lançamentos";
                    break;
                case 2:
                    msg = "A conta não pode ser excluida por possuir vinculo com transferências";
                    break;
                default:
                    msg = "Erro não esperado durante verificação de vinculos da conta";
                    break;
            }

            _notification.Add(msg);
            return true;
        }

        public IEnumerable<UsuarioContaConjuntaDto> GetUsuariosContaConjunta(int idConta)
        {
            ExecuteProcedure(Procedures.SP_SelContaConjuntaUsuarios);
            AddParameter("IdConta", idConta);
            var usuariosContaConjunta = new List<UsuarioContaConjuntaDto>();
            using (var reader = ExecuteReader())
            {
                while (reader.Read())
                {

                    usuariosContaConjunta.Add(new UsuarioContaConjuntaDto
                    {
                        IdContaConjunta = reader.ReadAttr<int>("IdContaConjunta"),
                        IdConta = reader.ReadAttr<int>("IdConta"),
                        DataAnalise = reader.ReadAttr<DateTime>("DataAnalise"),
                        IdUsuarioEnvio = reader.ReadAttr<int>("IdUsuarioEnvio"),
                        NomeUsuarioEnvio = reader.ReadAttr<string>("NomeUsuarioEnvio"),
                        IdUsuarioConvidado = reader.ReadAttr<int>("IdUsuarioConvidado"),
                        NomeUsuarioConvidado = reader.ReadAttr<string>("NomeUsuarioConvidado"),
                        DataBloqueio = reader.ReadAttr<DateTime>("DataBloqueio"),
                        DataDesbloqueio = reader.ReadAttr<DateTime>("DataDesbloqueio"),
                        EmailUsuarioConvidado = reader.ReadAttr<string>("EmailUsuarioConvidado")
                    });
                }

                return usuariosContaConjunta;
            }
        }

        public UsuarioContaConjuntaDto GetUsuarioContaConjunta(int idConta, int idUsuarioConvidado)
        {
            ExecuteProcedure(Procedures.SP_SelContaConjuntaUsuarios);
            AddParameter("IdConta", idConta);
            AddParameter("IdUsuarioConvidado", idUsuarioConvidado);

            using (var reader = ExecuteReader())
            {
                return !reader.Read()
                    ? null
                    : new UsuarioContaConjuntaDto
                    {
                        IdContaConjunta = reader.ReadAttr<int>("IdContaConjunta"),
                        IdConta = reader.ReadAttr<int>("IdConta"),
                        DataAnalise = reader.ReadAttr<DateTime>("DataAnalise"),
                        IdUsuarioEnvio = reader.ReadAttr<int>("IdUsuarioEnvio"),
                        NomeUsuarioEnvio = reader.ReadAttr<string>("NomeUsuarioEnvio"),
                        IdUsuarioConvidado = reader.ReadAttr<int>("IdUsuarioConvidado"),
                        NomeUsuarioConvidado = reader.ReadAttr<string>("NomeUsuarioConvidado"),
                        DataBloqueio = reader.ReadAttr<DateTime>("DataBloqueio"),
                        DataDesbloqueio = reader.ReadAttr<DateTime>("DataDesbloqueio"),
                        EmailUsuarioConvidado = reader.ReadAttr<string>("EmailUsuarioConvidado")

                    };
            }
        }

        public void PostConviteContaConjunta(int idConta, int idUsuarioEnvio, int idUsuarioConvidado)
        {
            ExecuteProcedure(Procedures.SP_InsContaConjuntaSolicitacao);
            AddParameter("IdConta", idConta);
            AddParameter("IdUsuarioEnvio", idUsuarioEnvio);
            AddParameter("IdUsuarioConvidado", idUsuarioConvidado);
            ExecuteNonQuery();
        }

        public IEnumerable<ContaConjuntaSolicitacaoDto> GetConviteContaConjunta(int idUsuario)
        {
            ExecuteProcedure(Procedures.SP_SelContaConjuntaSolicitacao);
            AddParameter("IdUsuario", idUsuario);
            var solicitacoes = new List<ContaConjuntaSolicitacaoDto>();
            using (var reader = ExecuteReader())
                while (reader.Read())
                    solicitacoes.Add(new ContaConjuntaSolicitacaoDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        IdConta = reader.ReadAttr<int>("IdConta"),
                        NomeConta = reader.ReadAttr<string>("NomeConta"),
                        IdUsuarioEnvio = reader.ReadAttr<int>("IdUsuarioEnvio"),
                        NomeUsuarioEnvio = reader.ReadAttr<string>("NomeUsuarioEnvio"),
                        IdUsuarioConvidado = reader.ReadAttr<int>("IdUsuarioConvidado"),
                        NomeUsuarioConvidado = reader.ReadAttr<string>("NomeUsuarioConvidado"),
                        DataCadastro = reader.ReadAttr<DateTime>("DataCadastro"),
                        DataAnalise = reader.ReadAttr<DateTime?>("DataAnalise"),
                        IndicadorAprovado = reader.ReadAttr<string>("IndicadorAprovado"),
                        IndicadorEnviadoConvidado = reader.ReadAttr<string>("IndicadorEnviadoConvidado")
                    });

            return solicitacoes;
        }

        public void PutAprovaReprovaConviteContaConjunta(int idSolicitacao, string indicadorAprovado)
        {
            ExecuteProcedure(Procedures.SP_UpdAprovaReprovaContaConjuntaSolicitacao);
            AddParameter("IdSolicitacao", idSolicitacao);
            AddParameter("IndicadorAprovado", indicadorAprovado);
            ExecuteNonQuery();
        }

        public void PostContaConjunta(int idSolicitacao)
        {
            ExecuteProcedure(Procedures.SP_InsContaConjunta);
            AddParameter("IdSolicitacao", idSolicitacao);
            ExecuteNonQuery();
        }
    }
}