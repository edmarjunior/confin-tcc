
USE conFin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_InsCompromisso]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_InsCompromisso]
GO

CREATE PROCEDURE [dbo].[SP_InsCompromisso]
	@Descricao					varchar(100),
	@IdPeriodo					tinyint,
	@DataInicio					datetime,
	@TotalParcelasOriginal		smallint = NULL,
	@IdUsuarioCadastro			int,
	@DataCadastro				datetime,
	@IdConta					int

	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Cadastra um novo compromisso de lançamento
		Autor.............: Edmar Costa
 		Data..............: 03/09/2017
		Ex................: EXEC [dbo].[SP_InsCompromisso] 1
	*/

	BEGIN
		
		INSERT INTO Compromisso(Descricao,
								IdPeriodo,
								DataInicio,
								TotalParcelasOriginal,
								IdUsuarioCadastro,
								DataCadastro,
								IdConta)
			VALUES( @Descricao,
					@IdPeriodo,
					@DataInicio,
					@TotalParcelasOriginal,
					@IdUsuarioCadastro,
					@DataCadastro,
					@IdConta)

			RETURN SCOPE_IDENTITY()

	END
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_DelCompromisso]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_DelCompromisso]
GO

CREATE PROCEDURE [dbo].[SP_DelCompromisso]
	@Id	 int

	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Exclui um registro de compromisso
		Autor.............: Edmar Costa
 		Data..............: 10/09/2017
		Ex................: EXEC [dbo].[SP_DelCompromisso] 1
	*/

	BEGIN

		DELETE FROM Compromisso WHERE Id = @Id

	END
GO





IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelCompromissoLancamentos]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelCompromissoLancamentos]
GO

CREATE PROCEDURE [dbo].[SP_SelCompromissoLancamentos]
	@IdCompromisso	 int

	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Retorna os lançamentos vinculados pelo compromisso
		Autor.............: Edmar Costa
 		Data..............: 10/09/2017
		Ex................: EXEC [dbo].[SP_SelCompromissoLancamentos] 105
	*/

	BEGIN

		SELECT  c.IdCompromisso,
				c.IdLancamento,
				c.NumeroLancamento,
				l.DataLancamento
			FROM CompromissoLancamento c WITH(NOLOCK)
				INNER JOIN Lancamento l WITH(NOLOCK)
					ON l.Id = c.IdLancamento
			WHERE IdCompromisso = @IdCompromisso

	END
GO







IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelCompromissoLancamento]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelCompromissoLancamento]
GO

CREATE PROCEDURE [dbo].[SP_SelCompromissoLancamento]
	@IdLancamento	 int

	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Retorna o compromisso vínculado ao lançamento
		Autor.............: Edmar Costa
 		Data..............: 10/09/2017
		Ex................: EXEC [dbo].[SP_SelCompromissoLancamento] 105
	*/

	BEGIN

		SELECT  co.Id
			FROM Compromisso co WITH(NOLOCK)
				INNER JOIN CompromissoLancamento cl WITH(NOLOCK)
					ON cl.IdLancamento = @IdLancamento
						AND cl.IdCompromisso = co.Id

	END
GO





IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_InsCompromissoLancamento]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_InsCompromissoLancamento]
GO

CREATE PROCEDURE [dbo].[SP_InsCompromissoLancamento]
	@IdCompromisso		int,
	@IdLancamento		int,
	@NumeroLancamento	smallint
	
	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Vincula um lançamento a um compromisso
		Autor.............: Edmar Costa
 		Data..............: 03/09/2017
		Ex................: EXEC [dbo].[SP_InsCompromissoLancamento] 1
	*/

	BEGIN
		
		INSERT INTO CompromissoLancamento(IdCompromisso, IdLancamento, NumeroLancamento)
			VALUES(@IdCompromisso, @IdLancamento, @NumeroLancamento)

			RETURN SCOPE_IDENTITY()

	END
GO





IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_DelCompromissoLancamento]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_DelCompromissoLancamento]
GO

CREATE PROCEDURE [dbo].[SP_DelCompromissoLancamento]
	@IdLancamento		int


	AS

	/*
		Documentação
		Arquivo Fonte.....: Lancamento.sql
		Objetivo..........: Excluir o vinculo do lançamento com o compromisso
		Autor.............: Edmar Costa
 		Data..............: 04/09/2017
		Ex................: EXEC [dbo].[SP_DelCompromissoLancamento] 1
	*/

	BEGIN
		
		DELETE FROM CompromissoLancamento WHERE IdLancamento = @IdLancamento

	END
GO





