
USE conFin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_InsAcessoOpcaoMenu]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_InsAcessoOpcaoMenu]
GO

CREATE PROCEDURE [dbo].[SP_InsAcessoOpcaoMenu]
	@IdUsuario		int,
	@CodigoOpcao	int

	AS

	/*
		Documentação
		Arquivo Fonte.....: Acesso.sql
		Objetivo..........: Incrementar um acesso a uma opção de menu
		Autor.............: Edmar Costa
 		Data..............: 06/11/2017
		Ex................: EXEC [dbo].[SP_InsAcessoOpcaoMenu] 1
	*/

	BEGIN
		
		INSERT INTO OpcaoMenuAcessos(CodigoOpcao, IdUsuario, DataAcesso)
			VALUES( @CodigoOpcao, @IdUsuario, GETDATE())

	END
GO