
USE conFin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelContaConjunta]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelContaConjunta]
GO

CREATE PROCEDURE [dbo].[SP_SelContaConjunta]
	@IdUsuario			int = NULL,
	@IdConta			int = NULL,
	@Id					int = NULL

	AS

	/*
		Documentação
		Arquivo Fonte.....: ContaConjunta.sql
		Objetivo..........: Lista os usuarios de uma conta conjunta
		Autor.............: Edmar Costa
 		Data..............: 05/10/2017
		Ex................: EXEC [dbo].[SP_SelContaConjunta] null, 3003
	*/

	BEGIN

		SELECT  cc.Id,
				cc.IdUsuarioEnvio,
				cc.IdUsuarioEnvio,
				us.Nome AS NomeUsuarioEnvio,
				us.Email AS EmailUsuarioEnvio,
				us2.Nome AS NomeUsuarioConvidado,
				us2.Email AS EmailUsuarioConvidado,
				cc.DataCadastro,
				cc.DataAnalise,
				cc.IndicadorAprovado,
				cc.IdUsuarioConvidado,
				cf.Nome + ' (' + us.Nome + ')' AS NomeConta,
				cc.IdConta
			FROM ContaConjunta cc WITH(NOLOCK)
				INNER JOIN Usuario us WITH(NOLOCK)
					ON us.Id = cc.IdUsuarioEnvio
				INNER JOIN Usuario us2 WITH(NOLOCK)
					ON us2.Id = cc.IdUsuarioConvidado
				INNER JOIN ContaFinanceira cf WITH(NOLOCK)
					ON cf.Id = cc.IdConta
			WHERE (@IdConta IS NULL OR cc.IdConta = @IdConta)
				AND (@IdUsuario IS NULL OR cc.IdUsuarioConvidado = @IdUsuario)
				AND (@Id IS NULL OR cc.Id = @Id)

	END
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_InsContaConjunta]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_InsContaConjunta]
GO

CREATE PROCEDURE [dbo].[SP_InsContaConjunta]
	@IdUsuarioEnvio			int,
	@IdUsuarioConvidado		int,
	@IdConta				int

	AS

	/*
		Documentação
		Arquivo Fonte.....: ContaConjunta.sql
		Objetivo..........: Cadastra uma nova solicitação de ingresso em conta conjunta
		Autor.............: Edmar Costa
 		Data..............: 05/10/2017
		Ex................: EXEC [dbo].[SP_InsContaConjunta] 8002, 8003, 1
	*/

	BEGIN

		INSERT INTO ContaConjunta (IdConta, IdUsuarioEnvio, IdUsuarioConvidado, DataCadastro)
			VALUES (@IdConta, @IdUsuarioEnvio, @IdUsuarioConvidado, GETDATE())

	END
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_DelContaConjunta]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_DelContaConjunta]
GO

CREATE PROCEDURE [dbo].[SP_DelContaConjunta]
	@Id int

	AS

	/*
		Documentação
		Arquivo Fonte.....: ContaConjunta.sql
		Objetivo..........: Remove uma conta conjunta.
		Autor.............: Edmar Costa
 		Data..............: 05/10/2017
		Ex................: EXEC [dbo].[SP_DelContaConjunta] 1
	*/

	BEGIN
		
		DELETE FROM ContaConjunta WHERE Id = @Id

	END
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_UpdContaConjunta]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_UpdContaConjunta]
GO

CREATE PROCEDURE [dbo].[SP_UpdContaConjunta]
	@Id					int,
	@IndicadorAprovado	varchar(1) -- A: Aprovação, R: Reprovação

	AS

	/*
		Documentação
		Arquivo Fonte.....: ContaConjuntaSolicitacao.sql
		Objetivo..........: Alterar a tabela de conta conjunta para setar dados de aprovação/reprovação
		Autor.............: Edmar Costa
 		Data..............: 06/10/2017
		Ex................: EXEC [dbo].[SP_UpdContaConjunta] 1, 'A'
	*/

	BEGIN

		UPDATE ContaConjunta
			SET DataAnalise = GETDATE(),
				IndicadorAprovado = @IndicadorAprovado
			WHERE Id = @Id

	END
GO
