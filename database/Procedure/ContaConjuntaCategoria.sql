
USE conFin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelContaConjuntaCategoria]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelContaConjuntaCategoria]
GO

CREATE PROCEDURE [dbo].[SP_SelContaConjuntaCategoria]
	@IdConta			int

	AS

	/*
		Documentação
		Arquivo Fonte.....: ContaConjuntaCategoria.sql
		Objetivo..........: Lista as categorias de uma conta conjunta.
		Autor.............: Edmar Costa
 		Data..............: 08/10/2017
		Ex................: EXEC [dbo].[SP_SelContaConjuntaCategoria] 3003
	*/

	BEGIN

		SELECT  lc.Id,
				lc.Nome
			FROM ContaConjuntaCategoria ccc WITH(NOLOCK)
				INNER JOIN LancamentoCategoria lc WITH(NOLOCK)
					ON lc.Id = ccc.IdCategoria
			WHERE ccc.IdConta = @IdConta
	END
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_InsContaConjuntaCategorias]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_InsContaConjuntaCategorias]
GO

CREATE PROCEDURE [dbo].[SP_InsContaConjuntaCategorias]
	@IdConta			int

	AS

	/*
		Documentação
		Arquivo Fonte.....: ContaConjuntaCategoria.sql
		Objetivo..........: Vincula todas as categorias de uma conta para a tabela de conta conjunta categoria.
		Autor.............: Edmar Costa
 		Data..............: 08/10/2017
		Ex................: EXEC [dbo].[SP_InsContaConjuntaCategorias] 3003
	*/

	BEGIN
		
		INSERT INTO ContaConjuntaCategoria (IdConta, IdCategoria)
			SELECT DISTINCT IdConta, IdCategoria 
				FROM Lancamento lc WITH(NOLOCK)
				WHERE lc.IdConta = @IdConta

				
	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_InsContaConjuntaCategoria]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_InsContaConjuntaCategoria]
GO

CREATE PROCEDURE [dbo].[SP_InsContaConjuntaCategoria]
	@IdConta		int,
	@IdCategoria	int

	AS

	/*
		Documentação
		Arquivo Fonte.....: ContaConjuntaCategoria.sql
		Objetivo..........: Vincula uma categoria a uma conta conjunta
		Autor.............: Edmar Costa
 		Data..............: 09/10/2017
		Ex................: EXEC [dbo].[SP_InsContaConjuntaCategoria] 3003
	*/

	BEGIN
		
		INSERT INTO ContaConjuntaCategoria (IdConta, IdCategoria) VALUES (@IdConta, @IdCategoria)
				
	END
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_DelContaConjuntaCategoria]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_DelContaConjuntaCategoria]
GO

CREATE PROCEDURE [dbo].[SP_DelContaConjuntaCategoria]
	@IdConta		int,
	@IdCategoria	int

	AS

	/*
		Documentação
		Arquivo Fonte.....: ContaConjuntaCategoria.sql
		Objetivo..........: Exclui uma categoria da tabela de conta conjunta
		Autor.............: Edmar Costa
 		Data..............: 09/10/2017
		Ex................: EXEC [dbo].[SP_DelContaConjuntaCategoria] 3003, 1002
	*/

	BEGIN
		
		DELETE FROM ContaConjuntaCategoria
			WHERE IdConta = @IdConta
			AND IdCategoria = @IdCategoria
				
	END
GO