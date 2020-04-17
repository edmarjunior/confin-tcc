
USE conFin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelLancamentoCategorias]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelLancamentoCategorias]
GO

CREATE PROCEDURE [dbo].[SP_SelLancamentoCategorias]
	@IdUsuario	int

	AS

	/*
		Documentação
		Arquivo Fonte.....: LancamentoCategoria.sql
		Objetivo..........: Retorna as categorias de lançamentos vinculadas a um usuário  
		Autor.............: Edmar Costa
 		Data..............: 14/08/2017
		Ex................: EXEC [dbo].[SP_SelLancamentoCategorias] 1
	*/

	BEGIN

		SELECT  Id,
				Nome,
				Cor
			FROM LancamentoCategoria WITH(NOLOCK)
			WHERE IdUsuarioCadastro = @IdUsuario

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelLancamentoCategoria]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelLancamentoCategoria]
GO

CREATE PROCEDURE [dbo].[SP_SelLancamentoCategoria]
	@IdCategoria	int,
	@IdUsuario		int = NULL



	AS

	/*
		Documentação
		Arquivo Fonte.....: LancamentoCategoria.sql
		Objetivo..........: Retorna os dados de uma categoria de lançamento  
		Autor.............: Edmar Costa
 		Data..............: 14/08/2017
		Ex................: EXEC [dbo].[SP_SelLancamentoCategoria] 1, 9002
	*/

	BEGIN

		SELECT  lc.Id,
				(lc.Nome + CASE WHEN @IdUsuario IS NOT NULL AND @IdUsuario <> lc.IdUsuarioCadastro THEN ' ('+usuCad.Nome+')' ELSE '' END) AS Nome,
				lc.Cor,
				lc.IdUsuarioCadastro,
				usuCad.Id AS NomeUsuarioCadastro,
				lc.DataCadastro,
				IdUsuarioUltimaAlteracao,
				usuAlt.Nome AS NomeUsuarioUltimaAlteracao,
				lc.DataUltimaAlteracao
			FROM LancamentoCategoria lc WITH(NOLOCK)
				INNER JOIN Usuario usuCad WITH(NOLOCK)
					ON usuCad.Id = lc.IdUsuarioCadastro
				LEFT OUTER JOIN Usuario usuAlt WITH(NOLOCK)
					ON usuAlt.Id = lc.IdUsuarioUltimaAlteracao
			WHERE lc.Id =  @IdCategoria

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_InsLancamentoCategoria]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_InsLancamentoCategoria]
GO

CREATE PROCEDURE [dbo].[SP_InsLancamentoCategoria]
	@Nome					varchar(100),
	@IdCategoriaSuperior	int = NULL,
	@Cor					varchar(10),
	@IdUsuarioCadastro		int


	AS

	/*
		Documentação
		Arquivo Fonte.....: LancamentoCategoria.sql
		Objetivo..........: Realiza o cadastro de uma categoria de lançamentos  
		Autor.............: Edmar Costa
 		Data..............: 14/08/2017
		Ex................: EXEC [dbo].[SP_InsLancamentoCategoria] 1, 1
	*/

	BEGIN

		INSERT INTO LancamentoCategoria(Nome, IdCategoriaSuperior, Cor, IdUsuarioCadastro, DataCadastro) 
			VALUES (@Nome, @IdCategoriaSuperior, @Cor, @IdUsuarioCadastro, GETDATE())

	END
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_UpdLancamentoCategoria]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_UpdLancamentoCategoria]
GO

CREATE PROCEDURE [dbo].[SP_UpdLancamentoCategoria]
	@Nome					varchar(100),
	@IdCategoriaSuperior	int = NULL,
	@Cor					varchar(10),
	@IdUsuario				int,
	@IdCategoria			int


	AS

	/*
		Documentação
		Arquivo Fonte.....: LancamentoCategoria.sql
		Objetivo..........: Realiza a edição nos dados de uma categoria de lançamento  
		Autor.............: Edmar Costa
 		Data..............: 14/08/2017
		Ex................: EXEC [dbo].[SP_UpdLancamentoCategoria] 1, 1
	*/

	BEGIN
		UPDATE LancamentoCategoria
				SET IdUsuarioUltimaAlteracao = @IdUsuario,			
					Nome					 = @Nome,			
					IdCategoriaSuperior		 = @IdCategoriaSuperior,				
					Cor						 = @Cor,	
					DataUltimaAlteracao		 = GETDATE()			
				WHERE Id = @IdCategoria


	END
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_DelLancamentoCategoria]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_DelLancamentoCategoria]
GO

CREATE PROCEDURE [dbo].[SP_DelLancamentoCategoria]
	@IdCategoria			int

	AS

	/*
		Documentação
		Arquivo Fonte.....: LancamentoCategoria.sql
		Objetivo..........: Realiza a remoção de uma categoria de lançamento  
		Autor.............: Edmar Costa
 		Data..............: 14/08/2017
		Ex................: EXEC [dbo].[SP_DelLancamentoCategoria] 1, 1
	*/

	BEGIN

		DELETE FROM LancamentoCategoria WHERE Id = @IdCategoria

	END
GO







IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_InsCategoriasIniciaisUsuario]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_InsCategoriasIniciaisUsuario]
GO

CREATE PROCEDURE [dbo].[SP_InsCategoriasIniciaisUsuario]
	@IdUsuario	int


	AS

	/*
		Documentação
		Arquivo Fonte.....: LancamentoCategoria.sql
		Objetivo..........: Realiza o cadastro das categorias iniciais de um usuário que esta sendo recém cadastrado  
		Autor.............: Edmar Costa
 		Data..............: 14/08/2017
		Ex................: EXEC [dbo].[SP_InsCategoriasIniciaisUsuario] 3
	*/

	BEGIN
		
		DECLARE @Dat_Atual datetime = GETDATE()

		INSERT INTO LancamentoCategoria(Nome, IdCategoriaSuperior, Cor, IdUsuarioCadastro, DataCadastro) 
			VALUES ('Alimentação',  NULL, '#FF0000', @IdUsuario, @Dat_Atual),	-- red
				   ('Bebida',		NULL, '#FFFF00', @IdUsuario, @Dat_Atual),	-- yellow
				   ('Transporte',	NULL, '#0000FF', @IdUsuario, @Dat_Atual),	-- blue
				   ('Vestuário',	NULL, '#EE82EE', @IdUsuario, @Dat_Atual),	-- Violet
				   ('Salário',		NULL, '#00FF00', @IdUsuario, @Dat_Atual),	-- Lime
				   ('Outros',		NULL, '#000000', @IdUsuario, @Dat_Atual)	-- Black

	END
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelInsIdLancamentoCategoria]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelInsIdLancamentoCategoria]
GO

CREATE PROCEDURE [dbo].[SP_SelInsIdLancamentoCategoria]
	@Nome		varchar(100),
	@IdUsuario	int

	AS

	/*
		Documentação
		Arquivo Fonte.....: LancamentoCategoria.sql
		Objetivo..........: Pega o id da categoria de lançamento, caso não existir é criado uma.  
		Autor.............: Edmar Costa
 		Data..............: 17/09/2017
		Ex................: EXEC [dbo].[SP_SelInsIdLancamentoCategoria] 'Transporte', 1
	*/

	BEGIN
		
		DECLARE @Id int

		SELECT @Id = Id
			FROM LancamentoCategoria WITH(NOLOCK)
			WHERE Nome = @Nome
				AND IdUsuarioCadastro = @IdUsuario

		IF @Id IS NULL
			BEGIN

				INSERT INTO LancamentoCategoria (Nome, Cor, IdUsuarioCadastro, DataCadastro)
					VALUES (@Nome, '#808080', @IdUsuario, GETDATE())

				SET @Id = SCOPE_IDENTITY()

			END
		
		SELECT @Id AS Id

	END
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelLancamentoCategoriasConta]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelLancamentoCategoriasConta]
GO

CREATE PROCEDURE [dbo].[SP_SelLancamentoCategoriasConta]
	@IdConta	int

	AS

	/*
		Documentação
		Arquivo Fonte.....: LancamentoCategoria.sql
		Objetivo..........: Retorna as categorias de lançamentos distintas vinculadas a uma conta financeira 
		Autor.............: Edmar Costa
 		Data..............: 09/10/2017
		Ex................: EXEC [dbo].[SP_SelLancamentoCategoriasConta] 3003
	*/

	BEGIN

		SELECT DISTINCT  
				lc.Id,
				lc.Nome,
				lc.Cor
			FROM Lancamento l WITH(NOLOCK)
				INNER JOIN LancamentoCategoria lc WITH(NOLOCK)
					ON lc.Id = l.IdCategoria
			WHERE IdConta = @IdConta

	END
GO