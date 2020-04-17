
USE conFin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelLancamentos]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelLancamentos]
GO

CREATE PROCEDURE [dbo].[SP_SelLancamentos]
	@IdUsuario	 int,
	@IdConta	 int = NULL,
	@IdCategoria int = NULL,
	@Mes		 tinyint = NULL,
	@Ano		 smallint = NULL

	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Retorna os lançamentos vinculadas a um usuário e/ou conta
		Autor.............: Edmar Costa
 		Data..............: 14/08/2017
		Ex................: EXEC [dbo].[SP_SelLancamentos] 8002, null, null, 10, 2017
	*/

	BEGIN

		SELECT  l.Id,
				l.IndicadorReceitaDespesa,
				l.Descricao + (CASE WHEN co.TotalParcelasOriginal IS NOT NULL
					 THEN ' ' + CAST(cl.NumeroLancamento AS VARCHAR(2))   + '/' + CAST(co.TotalParcelasOriginal AS VARCHAR(2))
					 ELSE '' END) AS Descricao,
				l.Valor,
				l.DataLancamento,
				l.IdConta,
				(c.Nome + CASE WHEN c.IdUsuarioCadastro <> @IdUsuario THEN ' ('+usuCadConta.Nome+')' ELSE '' END) AS NomeContaOrigem,
				'' AS IdContaDestino,
				'' AS NomeContaDestino,
				l.IdCategoria,
				lc.Nome AS NomeCategoria,
				lc.Cor AS CorCategoria,
				l.IndicadorPagoRecebido,
				l.IdUsuarioCadastro,
				usuCad.Id AS NomeUsuarioCadastro,
				l.DataCadastro,
				l.IdUsuarioUltimaAlteracao,
				usuAlt.Nome AS NomeUsuarioUltimaAlteracao,
				l.DataUltimaAlteracao,
				cl.IdCompromisso
			FROM Lancamento l WITH(NOLOCK)
				INNER JOIN ContaFinanceira c WITH(NOLOCK)
					ON c.Id = l.IdConta
				LEFT OUTER JOIN ContaConjunta cc WITH(NOLOCK)
					ON cc.IdConta = c.Id
						AND cc.IndicadorAprovado = 'A'
				INNER JOIN LancamentoCategoria lc WITH(NOLOCK)
					ON lc.Id = l.IdCategoria
				INNER JOIN Usuario usuCad WITH(NOLOCK)
					ON usuCad.Id = l.IdUsuarioCadastro
				INNER JOIN Usuario usuCadConta WITH(NOLOCK)
					ON usuCadConta.Id = c.IdUsuarioCadastro
				LEFT OUTER JOIN Usuario usuAlt WITH(NOLOCK)
					ON usuAlt.Id = l.IdUsuarioUltimaAlteracao
				LEFT OUTER JOIN CompromissoLancamento cl WITH(NOLOCK)
					ON cl.IdLancamento = l.Id
				LEFT OUTER JOIN Compromisso co WITH(NOLOCK)
					ON co.Id = cl.IdCompromisso
			WHERE (c.IdUsuarioCadastro = @IdUsuario OR cc.IdUsuarioConvidado = @IdUsuario)
				AND (@IdConta IS NULL OR l.IdConta = @IdConta)
				AND (@IdCategoria IS NULL OR l.IdCategoria = @IdCategoria)
				AND (@Mes IS NULL OR MONTH(l.DataLancamento) = @Mes)
				AND (@ano IS NULL OR YEAR(l.DataLancamento) = @ano)

		UNION

			SELECT  tr.Id,
					'T' AS IndicadorReceitaDespesa,
					tr.Descricao,
					tr.Valor,
					tr.DataTransferencia AS DataLancamento,
					tr.IdContaOrigem AS IdConta,
					(cfo.Nome + CASE WHEN cfo.IdUsuarioCadastro <> @IdUsuario THEN ' ('+usuCfo.Nome+')' ELSE '' END) AS NomeContaOrigem,
					tr.IdContaDestino,
					(cfd.Nome + CASE WHEN cfd.IdUsuarioCadastro <> @IdUsuario THEN ' ('+usuCfd.Nome+')' ELSE '' END) AS NomeContaDestino,
					tr.IdCategoria,
					lc.Nome AS NomeCategoria,
					lc.Cor AS CorCategoria,
					tr.IndicadorPagoRecebido,
					tr.IdUsuarioCadastro,
					usuCad.Id AS NomeUsuarioCadastro,
					tr.DataCadastro,
					tr.IdUsuarioUltimaAlteracao,
					usuAlt.Nome AS NomeUsuarioUltimaAlteracao,
					tr.DataUltimaAlteracao,
					''
			FROM Transferencia tr WITH(NOLOCK)
				INNER JOIN ContaFinanceira cfo WITH(NOLOCK)
					ON cfo.Id = tr.IdContaOrigem
				INNER JOIN ContaFinanceira cfd WITH(NOLOCK)
					ON cfd.Id = tr.IdContaDestino
				LEFT OUTER JOIN ContaConjunta cc WITH(NOLOCK)
					ON cc.IdConta IN (cfo.Id, cfd.Id)
						AND cc.IndicadorAprovado = 'A'
				INNER JOIN LancamentoCategoria lc WITH(NOLOCK)
					ON lc.Id = tr.IdCategoria
				INNER JOIN Usuario usuCad WITH(NOLOCK)
					ON usuCad.Id = tr.IdUsuarioCadastro
				LEFT OUTER JOIN Usuario usuAlt WITH(NOLOCK)
					ON usuAlt.Id = tr.IdUsuarioUltimaAlteracao
				INNER JOIN Usuario usuCfo WITH(NOLOCK)
					ON usuCfo.Id = cfo.IdUsuarioCadastro
				INNER JOIN Usuario usuCfd WITH(NOLOCK)
					ON usuCfd.Id = cfd.IdUsuarioCadastro
			WHERE (cfo.IdUsuarioCadastro = @IdUsuario OR cfd.IdUsuarioCadastro = @IdUsuario OR cc.IdUsuarioConvidado = @IdUsuario)
				AND (@IdConta IS NULL OR (tr.IdContaOrigem = @IdConta OR tr.IdContaDestino = @IdConta))
				AND (@IdCategoria IS NULL OR tr.IdCategoria = @IdCategoria)
				AND (@Mes IS NULL OR MONTH(tr.DataTransferencia) = @Mes)
				AND (@ano IS NULL OR YEAR(tr.DataTransferencia) = @ano)

	END
GO





IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelLancamento]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelLancamento]
GO

CREATE PROCEDURE [dbo].[SP_SelLancamento]
	@IdLancamento	int

	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Retorna os dados de um lançamento
		Autor.............: Edmar Costa
 		Data..............: 14/08/2017
		Ex................: EXEC [dbo].[SP_SelLancamento] 1
	*/

	BEGIN

		SELECT  l.Id,
				l.IndicadorReceitaDespesa,
				l.Descricao,
				l.Valor,
				l.DataLancamento,
				l.IdConta,
				l.IdCategoria,
				l.IndicadorPagoRecebido,
				co.IdCompromisso,
				l.IdUsuarioCadastro,
				usuCad.Id AS NomeUsuarioCadastro,
				l.DataCadastro,
				l.IdUsuarioUltimaAlteracao,
				usuAlt.Nome AS NomeUsuarioUltimaAlteracao,
				l.DataUltimaAlteracao
			FROM Lancamento l WITH(NOLOCK)
				INNER JOIN Usuario usuCad WITH(NOLOCK)
					ON usuCad.Id = l.IdUsuarioCadastro
				LEFT OUTER JOIN Usuario usuAlt WITH(NOLOCK)
					ON usuAlt.Id = l.IdUsuarioUltimaAlteracao
				LEFT OUTER JOIN CompromissoLancamento co WITH(NOLOCK)
					ON co.IdLancamento = l.Id
			WHERE l.Id = @IdLancamento

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_InsLancamento]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_InsLancamento]
GO

CREATE PROCEDURE [dbo].[SP_InsLancamento]
	@IndicadorReceitaDespesa	varchar(1),
	@Descricao					varchar(100),
	@Valor						decimal(12,2),
	@DataLancamento				datetime,
	@IdConta					int,
	@IdCategoria				int,
	@IndicadorPagoRecebido		varchar(1),
	@IdUsuario					int

	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Cadastra um novo lançamento
		Autor.............: Edmar Costa
 		Data..............: 14/08/2017
		Ex................: EXEC [dbo].[SP_InsLancamento] 1
	*/

	BEGIN
		
		INSERT INTO Lancamento( IndicadorReceitaDespesa,
								Descricao,
								Valor,
								DataLancamento,
								IdConta,
								IdCategoria,
								IndicadorPagoRecebido,
								IdUsuarioCadastro,
							    DataCadastro)
			VALUES(@IndicadorReceitaDespesa,
				   @Descricao,
				   @Valor,
				   @DataLancamento,
				   @IdConta,
				   @IdCategoria,
				   @IndicadorPagoRecebido,
		 		   @IdUsuario,
				   GETDATE())

			RETURN SCOPE_IDENTITY()

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_UpdLancamento]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_UpdLancamento]
GO

CREATE PROCEDURE [dbo].[SP_UpdLancamento]
	@IndicadorReceitaDespesa	varchar(1),
	@Descricao					varchar(100),
	@Valor						decimal(12,2),
	@DataLancamento				datetime,
	@IdConta					int,
	@IdCategoria				int,
	@IndicadorPagoRecebido		varchar(1),
	@IdUsuario					int,
	@IdLancamento				int


	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Realiza a edição de um lançamento
		Autor.............: Edmar Costa
 		Data..............: 14/08/2017
		Ex................: EXEC [dbo].[SP_UpdLancamento] 1
	*/

	BEGIN
		
		UPDATE Lancamento
				SET IndicadorReceitaDespesa  = @IndicadorReceitaDespesa,
					IdUsuarioUltimaAlteracao = @IdUsuario,			
					Descricao				 = @Descricao,			
					Valor					 = @Valor,				
					DataLancamento			 = @DataLancamento,	
					IdConta					 = @IdConta,	
					IdCategoria				 = @IdCategoria,	
					IndicadorPagoRecebido	 = @IndicadorPagoRecebido,	
					DataUltimaAlteracao		 = GETDATE()			
				WHERE Id = @IdLancamento

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_DelLancamento]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_DelLancamento]
GO

CREATE PROCEDURE [dbo].[SP_DelLancamento]
	@IdLancamento		int


	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Excluir um lançamento
		Autor.............: Edmar Costa
 		Data..............: 14/08/2017
		Ex................: EXEC [dbo].[SP_DelLancamento] 1
	*/

	BEGIN
		
		DELETE FROM Lancamento WHERE Id = @IdLancamento

	END
GO







IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_UpdLancamentoIndicadorPagoRecebido]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_UpdLancamentoIndicadorPagoRecebido]
GO

CREATE PROCEDURE [dbo].[SP_UpdLancamentoIndicadorPagoRecebido]
	@IdLancamento			int,
	@IndicadorPagoRecebido	varchar(1),
	@IdUsuario				int


	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Realiza a edição no campo IndicadorPagoRecebido de um lançamento
		Autor.............: Edmar Costa
 		Data..............: 16/08/2017
		Ex................: EXEC [dbo].[SP_UpdLancamentoIndicadorPagoRecebido] 1, 'S', 1
	*/

	BEGIN
		
		UPDATE Lancamento
				SET IdUsuarioUltimaAlteracao = @IdUsuario,	
					IndicadorPagoRecebido	 = @IndicadorPagoRecebido,	
					DataUltimaAlteracao		 = GETDATE()			
				WHERE Id = @IdLancamento

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelLancamentosResumo]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelLancamentosResumo]
GO

CREATE PROCEDURE [dbo].[SP_SelLancamentosResumo]
	@IdUsuario	 int,
	@IdConta	 int = NULL,
	@IdCategoria int = NULL,
	@Mes		 tinyint,
	@Ano		 smallint

	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Retorna o resumo de lançamentos (despesas, receitas e saldos previstos e realizados)
		Autor.............: Edmar Costa
 		Data..............: 14/08/2017
		Ex................: EXEC [dbo].[SP_SelLancamentosResumo] 9002, NULL, null, 10, 2017
	*/
		
	BEGIN

		DECLARE @ValorSaldoInicialConta	 decimal(14,2),
				@TotReceitasPrevistaMes	 decimal(14,2),
				@TotReceitasRealizadaMes decimal(14,2),
				@TotDespesasPrevistaMes	 decimal(14,2),
				@TotDespesasRealizadaMes decimal(14,2),
				@TotSaldoAnterior		 decimal(14,2),
				@TotSaldoPrevisto		 decimal(14,2),
				@Data_BaseMes			 datetime = DATEFROMPARTS(@Ano, @Mes, 1)
		DECLARE @Data_BaseProxMes		datetime = DATEADD(MONTH, 1, @Data_BaseMes)
		
		----------------- somando o saldo inicial da conta(s) do usuario -------------------
				
		SELECT  @ValorSaldoInicialConta	= SUM(cf.ValorSaldoInicial)
			FROM ContaFinanceira cf WITH(NOLOCK)
				LEFT OUTER JOIN ContaConjunta cc WITH(NOLOCK)
					ON cc.IdConta = cf.Id
						AND cc.IndicadorAprovado = 'A'
			WHERE (IdUsuarioCadastro = @IdUsuario OR cc.IdUsuarioConvidado = @IdUsuario)
				AND (@IdConta IS NULL OR cf.Id = @IdConta) 
		-----------------------------------------------------------------------------------

		DECLARE @Lancamentos TABLE (
			IndicadorReceitaDespesa varchar(1), 
			IndicadorPagoRecebido	varchar(1), 
			Valor					decimal(14,2), 
			IdConta					int,
			DataLancamento			datetime 
		)

		INSERT INTO @Lancamentos EXEC [dbo].[SP_SelLancamentosSimulados] @IdUsuario, @IdConta, @Data_BaseProxMes
		
		SELECT  @TotSaldoAnterior = SUM(CASE WHEN IndicadorReceitaDespesa = 'R' 
											 AND IndicadorPagoRecebido = 'S' 
											 AND DataLancamento < @Data_BaseMes THEN Valor ELSE 0 END) 
								  - SUM(CASE WHEN IndicadorReceitaDespesa = 'D' 
											 AND IndicadorPagoRecebido = 'S' 
											 AND DataLancamento < @Data_BaseMes THEN Valor ELSE 0 END),
				@TotSaldoPrevisto = SUM(CASE WHEN IndicadorReceitaDespesa = 'R' THEN Valor ELSE 0 END)
								  - SUM(CASE WHEN IndicadorReceitaDespesa = 'D' THEN Valor ELSE 0 END),
				@TotReceitasPrevistaMes = SUM(CASE WHEN IndicadorReceitaDespesa = 'R' 
												   AND YEAR(DataLancamento) = @Ano 
												   AND MONTH(DataLancamento) = @Mes THEN Valor ELSE 0 END),
				@TotReceitasRealizadaMes = SUM(CASE WHEN IndicadorReceitaDespesa = 'R' 
													AND IndicadorPagoRecebido = 'S' 
													AND YEAR(DataLancamento) = @Ano 
													AND MONTH(DataLancamento) = @Mes THEN Valor ELSE 0 END),
				@TotDespesasPrevistaMes = SUM(CASE WHEN IndicadorReceitaDespesa = 'D' 
												   AND YEAR(DataLancamento) = @Ano 
												   AND MONTH(DataLancamento) = @Mes THEN Valor ELSE 0 END),
				@TotDespesasRealizadaMes = SUM(CASE WHEN IndicadorReceitaDespesa = 'D' 
													AND IndicadorPagoRecebido = 'S' 
													AND YEAR(DataLancamento) = @Ano 
													AND MONTH(DataLancamento) = @Mes THEN Valor ELSE 0 END)
			FROM @Lancamentos

		SET @TotSaldoAnterior = ISNULL(@TotSaldoAnterior, 0) + ISNULL(@ValorSaldoInicialConta, 0)

		SELECT  @TotReceitasPrevistaMes  AS TotReceitasPrevista,
				@TotReceitasRealizadaMes AS TotReceitasRealizada,
				@TotDespesasPrevistaMes  AS TotDespesasPrevista,
				@TotDespesasRealizadaMes AS TotDespesasRealizada,
				@TotSaldoPrevisto		 AS TotSaldoPrevisto,
				(@TotSaldoAnterior + @TotReceitasRealizadaMes - @TotDespesasRealizadaMes) AS TotSaldoAtual,
				@TotSaldoAnterior AS TotValorSaldoInicialConta

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelPeriodo]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelPeriodo]
GO

CREATE PROCEDURE [dbo].[SP_SelPeriodo]
	@Id	int = NULL

	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Retorna os periodos disponiveis para cadastro de lançamentos parcelados
		Autor.............: Edmar Costa
 		Data..............: 03/09/2017
		Ex................: EXEC [dbo].[SP_SelPeriodo] 1
	*/

	BEGIN

		SELECT  Id,
				Descricao,
				Quantidade,
				IndicadorDiaMes
			FROM Periodo WITH(NOLOCK)
			WHERE @Id IS NULL OR Id = @Id

	END
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelLancamentosSimulados]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelLancamentosSimulados]
GO

CREATE PROCEDURE [dbo].[SP_SelLancamentosSimulados]
	@IdUsuario  int,
	@IdConta	int = NULL,
	@DataBase	datetime

	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Além de retornar os lançamentos cadastrados, tambem é simulados lançamentos de receita e despesa referente as transferencias
		Autor.............: Edmar Costa
 		Data..............: 12/10/2017
		Ex................: EXEC [dbo].[SP_SelLancamentosSimulados] 8002, null, '2017-10-31'
	*/

	BEGIN

		SELECT  l.IndicadorReceitaDespesa,
				l.IndicadorPagoRecebido,
				l.Valor,
				l.IdConta,
				l.DataLancamento
		FROM Lancamento l WITH(NOLOCK)
			INNER JOIN ContaFinanceira cf WITH(NOLOCK)
				ON cf.Id = l.IdConta
			LEFT OUTER JOIN ContaConjunta cc WITH(NOLOCK)
				ON cc.IdConta = cf.Id
					AND cc.IndicadorAprovado = 'A'
		WHERE (cf.IdUsuarioCadastro = @IdUsuario OR cc.IdUsuarioConvidado = @IdUsuario)
			AND (@IdConta IS NULL OR cf.Id = @IdConta)
			AND l.DataLancamento < @DataBase
	
		UNION 

		SELECT  'D' AS IndicadorReceitaDespesa,
				tr.IndicadorPagoRecebido,
				tr.Valor,
				tr.IdContaOrigem AS IdConta,
				tr.DataTransferencia AS DataLancamento
			FROM Transferencia tr WITH(NOLOCK)
				INNER JOIN ContaFinanceira cf WITH(NOLOCK)
					ON cf.Id = tr.IdContaOrigem
				LEFT OUTER JOIN ContaConjunta cc WITH(NOLOCK)
					ON cc.IdConta = cf.Id
						AND cc.IndicadorAprovado = 'A'
			WHERE (cf.IdUsuarioCadastro = @IdUsuario OR cc.IdUsuarioConvidado = @IdUsuario)
				AND (@IdConta IS NULL OR cf.Id = @IdConta)
				AND tr.DataTransferencia < @DataBase

		UNION

		SELECT  'R' AS IndicadorReceitaDespesa,
				tr.IndicadorPagoRecebido,
				tr.Valor,
				tr.IdContaDestino AS IdConta,
				tr.DataTransferencia AS DataLancamento
			FROM Transferencia tr WITH(NOLOCK)
				INNER JOIN ContaFinanceira cf WITH(NOLOCK)
					ON cf.Id = tr.IdContaDestino
				LEFT OUTER JOIN ContaConjunta cc WITH(NOLOCK)
					ON cc.IdConta = cf.Id
						AND cc.IndicadorAprovado = 'A'
			WHERE (cf.IdUsuarioCadastro = @IdUsuario OR cc.IdUsuarioConvidado = @IdUsuario)
				AND (@IdConta IS NULL OR cf.Id = @IdConta)
				AND tr.DataTransferencia < @DataBase


	END
GO


