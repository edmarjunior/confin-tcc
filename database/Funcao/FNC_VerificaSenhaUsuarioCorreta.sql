USE confin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[FNC_VerificaSenhaUsuarioCorreta]') AND objectproperty(id, N'IsScalarFunction')=1)
	DROP FUNCTION [dbo].[FNC_VerificaSenhaUsuarioCorreta]
GO 

CREATE FUNCTION [dbo].[FNC_VerificaSenhaUsuarioCorreta]
	(@Id int, @Senha varchar(200))

	RETURNS varchar(1)

	AS

	/*
		Documentação
		Objetivo..........:	Verifica se a senha passada para a function é a mesma senha do usuario
		Autor.............:	Edmar Costa
 		Data..............:	27/08/2017
		Exemplo...........: SELECT [dbo].[FNC_VerificaSenhaUsuarioCorreta](8002, '123')

	*/

	BEGIN
		
		 SET @Senha = HashBytes('MD5', @Senha)

		 IF EXISTS (SELECT TOP 1 1 FROM Usuario WHERE Id = @Id AND Senha = @Senha)
			RETURN 'S'

		RETURN 'N'
		
	END
GO


