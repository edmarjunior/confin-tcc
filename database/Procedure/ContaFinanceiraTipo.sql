
USE conFin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelContaFinanceiraTipo]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelContaFinanceiraTipo]
GO

CREATE PROCEDURE [dbo].[SP_SelContaFinanceiraTipo]

	AS

	/*
		Documentação
		Arquivo Fonte.....: ContaFinanceiraTipo.sql
		Objetivo..........: Retorna os tipos conta financeira  
		Autor.............: Edmar Costa
 		Data..............: 11/08/2017
		Ex................: EXEC [dbo].[SP_SelContaFinanceiraTipo]
	*/

	BEGIN

		SELECT  Id, Nome
			FROM ContaFinanceiraTipo WITH(NOLOCK)

	END
GO