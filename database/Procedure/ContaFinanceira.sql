
USE conFin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelContasFinanceira]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelContasFinanceira]
GO

CREATE PROCEDURE [dbo].[SP_SelContasFinanceira]
	@IdUsuario	int

	AS

	/*
		Documentação
		Arquivo Fonte.....: ContaFinanceira.sql
		Objetivo..........: Retorna as contas financeiras vinculadas a um usuário  
		Autor.............: Edmar Costa
 		Data..............: 11/08/2017
		Ex................: EXEC [dbo].[SP_SelContasFinanceira] 8002
	*/

	BEGIN

		SELECT  cf.Id,
				cf.Nome,
				cf.IdTipo,
				cft.Nome AS NomeTipo,
				cf.ValorSaldoInicial,
				cf.Descricao,
				[dbo].[FNC_CalculaSaldoAtualConta](cf.Id) AS Saldo,
				(CASE WHEN x.Id IS NOT NULL THEN 'A' 
										    ELSE (CASE WHEN y.Id IS NOT NULL THEN 'P'
																			 ELSE ''
												  END)
				END) AS IndicadorContaConjunta,
				'S' AS IndicadorProprietarioConta
			FROM ContaFinanceira cf WITH(NOLOCK)
				INNER JOIN ContaFinanceiraTipo cft WITH(NOLOCK)
					ON cft.Id = cf.IdTipo
				OUTER APPLY ( SELECT TOP 1 cc.Id
								FROM ContaConjunta cc
								WHERE cc.IdConta = cf.Id
									AND cc.IndicadorAprovado = 'A'
							) x
				OUTER APPLY ( SELECT TOP 1 cc2.Id 
								FROM ContaConjunta cc2
								WHERE cc2.IdConta = cf.Id
									AND cc2.DataAnalise IS NULL
							) y
			WHERE cf.IdUsuarioCadastro = @IdUsuario

		UNION 

		-- contas que o usuário foi convidado a participar

		SELECT cf.Id,
			   cf.Nome + ' (' + us.Nome + ')',
			   cf.IdTipo,
			   cft.Nome AS NomeTipo,
			   cf.ValorSaldoInicial,
			   cf.Descricao,
			   [dbo].[FNC_CalculaSaldoAtualConta](cf.Id) AS Saldo,
			   'A' AS IndicadorContaConjunta,
			   'N' AS IndicadorProprietarioConta
			FROM ContaConjunta cc WITH(NOLOCK)
				INNER JOIN Usuario us WITH(NOLOCK)
					ON us.Id = cc.IdUsuarioEnvio
				INNER JOIN ContaFinanceira cf WITH(NOLOCK)
					ON cf.Id = cc.IdConta
				INNER JOIN ContaFinanceiraTipo cft WITH(NOLOCK)
					ON cft.Id = cf.IdTipo
			WHERE cc.IdUsuarioConvidado = @IdUsuario
				AND cc.IndicadorAprovado = 'A'

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelContaFinanceira]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelContaFinanceira]
GO

CREATE PROCEDURE [dbo].[SP_SelContaFinanceira]
	@IdConta	int,
	@IdUsuario	int = NULL

	AS

	/*
		Documentação
		Arquivo Fonte.....: ContaFinanceira.sql
		Objetivo..........: Retorna os dados de uma conta financeira.
		Autor.............: Edmar Costa
 		Data..............: 11/08/2017
		Ex................: EXEC [dbo].[SP_SelContaFinanceira] 3
	*/

	BEGIN

		SELECT  cf.Id,
				(cf.Nome + CASE WHEN @IdUsuario IS NOT NULL AND @IdUsuario <> cf.IdUsuarioCadastro THEN ' ('+usuCad.Nome+')' ELSE '' END) AS Nome,
				cf.IdTipo,
				cf.ValorSaldoInicial,
				cf.Descricao,
				cf.IdUsuarioCadastro,
				usuCad.Id AS NomeUsuarioCadastro,
				cf.DataCadastro,
				IdUsuarioUltimaAlteracao,
				usuAlt.Nome AS NomeUsuarioUltimaAlteracao,
				cf.DataUltimaAlteracao,
				120.50 AS Saldo
			FROM ContaFinanceira cf WITH(NOLOCK)
				INNER JOIN Usuario usuCad WITH(NOLOCK)
					ON usuCad.Id = cf.IdUsuarioCadastro
				LEFT OUTER JOIN Usuario usuAlt WITH(NOLOCK)
					ON usuAlt.Id = cf.IdUsuarioUltimaAlteracao
			WHERE cf.Id = @IdConta

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_InsContaFinanceira]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_InsContaFinanceira]
GO

CREATE PROCEDURE [dbo].[SP_InsContaFinanceira]
	@Nome				varchar(100),
	@IdTipo				tinyint,
	@ValorSaldoInicial	decimal(12,2),
	@Descricao			varchar(100) = NULL,
	@IdUsuarioCadastro	int

	AS

	/*
	Documentação
	Arquivo Fonte.....: ContaFinanceira.sql
	Objetivo..........: Insere uma nova conta financeira  
	Autor.............: Edmar Costa
 	Data..............: 11/08/2017
	Ex................: BEGIN TRANSACTION
						DECLARE @Ret int
						EXEC @Ret = [dbo].[SP_InsContaFinanceira] @Nome				 = '',
																  @IdTipo			 = 1,
																  @ValorSaldoInicial = 0,
																  @Descricao		 = 'Conta para guardar dinheiro'
																  @IdUsuarioCadastro = 1
						SELECT @Ret
						ROLLBACK TRANSACTION
	*/

	BEGIN

		INSERT INTO ContaFinanceira(Nome, IdTipo, ValorSaldoInicial, Descricao, IdUsuarioCadastro, DataCadastro) 
			VALUES (@Nome, @IdTipo, @ValorSaldoInicial, @Descricao, @IdUsuarioCadastro, GETDATE())

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_UpdContaFinanceira]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_UpdContaFinanceira]
GO

CREATE PROCEDURE [dbo].[SP_UpdContaFinanceira]
	@IdConta			int,
	@IdUsuario			int,
	@Nome				varchar(100),
	@IdTipo				tinyint,
	@ValorSaldoInicial	decimal(12,2),
	@Descricao			varchar(100) = NULL

	AS

	/*
	Documentação
	Arquivo Fonte.....: ContaFinanceira.sql
	Objetivo..........: Realiza a edição nos dados de uma conta financeira  
	Autor.............: Edmar Costa
 	Data..............: 11/08/2017
	Ex................: BEGIN TRANSACTION
						DECLARE @Ret int
						EXEC @Ret = [dbo].[SP_UpdContaFinanceira] @IdConta			 = 1,
																  @IdUsuario		 = 1,
																  @Nome				 = '',
																  @IdTipo			 = 1
																  @ValorSaldoInicial = 100.50
																  @Descricao		 = ''
						SELECT @Ret
						ROLLBACK TRANSACTION
	*/

	BEGIN

		UPDATE ContaFinanceira
			SET IdUsuarioUltimaAlteracao = @IdUsuario,			
				Nome					 = @Nome,			
				IdTipo					 = @IdTipo,				
				ValorSaldoInicial		 = @ValorSaldoInicial,	
				Descricao				 = @Descricao,
				DataUltimaAlteracao		 = GETDATE()			
			WHERE Id = @IdConta

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_DelContaFinanceira]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_DelContaFinanceira]
GO

CREATE PROCEDURE [dbo].[SP_DelContaFinanceira]
	@IdConta			int

	AS

	/*
	Documentação
	Arquivo Fonte.....: ContaFinanceira.sql
	Objetivo..........: Realiza a remoção de um conta financeira  
	Autor.............: Edmar Costa
 	Data..............: 11/08/2017
	Ex................: BEGIN TRANSACTION
						DECLARE @Ret int
						EXEC @Ret = [dbo].[SP_DelContaFinanceira] 1
						SELECT @Ret
						ROLLBACK TRANSACTION
	*/

	BEGIN

		DELETE FROM ContaFinanceira
			WHERE Id = @IdConta

	END
GO