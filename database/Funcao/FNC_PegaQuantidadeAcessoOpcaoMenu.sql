USE confin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[FNC_PegaQuantidadeAcessoOpcaoMenu]') AND objectproperty(id, N'IsScalarFunction')=1)
	DROP FUNCTION [dbo].[FNC_PegaQuantidadeAcessoOpcaoMenu]
GO 

CREATE FUNCTION [dbo].[FNC_PegaQuantidadeAcessoOpcaoMenu]
	(@CodigoOpcao int = NULL, @IdUsuario int)

	RETURNS int

	AS

	/*
		Documentação
		Objetivo..........:	Calcular e retornar a quantidade de acesso de uma opção de menu
		Autor.............:	Edmar Costa
 		Data..............:	27/08/2017
		Exemplo...........: SELECT [dbo].[FNC_PegaQuantidadeAcessoOpcaoMenu](8002, '123')

	*/

	BEGIN
		 DECLARE @TotAcessos int

		 SELECT @TotAcessos = COUNT(*) FROM OpcaoMenuAcessos
			WHERE CodigoOpcao = @CodigoOpcao
				AND (@IdUsuario IS NULL OR IdUsuario = @IdUsuario)

		 RETURN ISNULL(@TotAcessos,0)
		
	END
GO


