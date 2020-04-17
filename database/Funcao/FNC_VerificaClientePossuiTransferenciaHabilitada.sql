USE confin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[FNC_VerificaClientePossuiTransferenciaHabilitada]') AND objectproperty(id, N'IsScalarFunction')=1)
	DROP FUNCTION [dbo].[FNC_VerificaClientePossuiTransferenciaHabilitada]
GO 

CREATE FUNCTION [dbo].[FNC_VerificaClientePossuiTransferenciaHabilitada]
	(@IdUsuario	int)

	RETURNS varchar(1)

	AS

	/*
		Documentação
		Objetivo..........:	Verifica se a conta possui mais de uma conta para que possa ser liberado a opção de transferencia
		Autor.............:	Edmar Costa
 		Data..............:	27/08/2017
		Exemplo...........: SELECT [dbo].[FNC_VerificaClientePossuiTransferenciaHabilitada](8002)

	*/

	BEGIN
		
			DECLARE @TotContas smallint = 0
			
			SELECT @TotContas = COUNT(*)
				FROM ContaFinanceira WITH(NOLOCK)
				WHERE IdUsuarioCadastro = @IdUsuario

			IF @TotContas > 1
				RETURN 'S'
			
			RETURN 'N'
		
	END
GO


