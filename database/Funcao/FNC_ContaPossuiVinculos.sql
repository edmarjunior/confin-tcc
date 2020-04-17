USE confin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[FNC_ContaPossuiVinculos]') AND objectproperty(id, N'IsScalarFunction')=1)
	DROP FUNCTION [dbo].[FNC_ContaPossuiVinculos]
GO 

CREATE FUNCTION [dbo].[FNC_ContaPossuiVinculos]
	(@IdConta	int)

	RETURNS varchar(1)

	AS

	/*
		Documentação
		Objetivo..........:	Verifica se a conta possui vinculos entre outras tabelas
		Autor.............:	Edmar Costa
 		Data..............:	27/08/2017
		Exemplo...........: SELECT [dbo].[FNC_ContaPossuiVinculos](1)
		Retornos..........: 0 - Não possui nenhum vinculo
							1 - possui vinculo com lançamento
							2 - possui vinculo com transferencia
	*/

	BEGIN
		
		-- VERIFICA LANÇAMENTOS
		IF(EXISTS(				
					SELECT TOP 1 1
						FROM Lancamento WITH(NOLOCK)
						WHERE IdConta = @IdConta
				 ))
			RETURN 1
		
		
		-- VERIFICA TRANSFERENCIAS
		IF(EXISTS(				
					SELECT TOP 1 1
						FROM Transferencia WITH(NOLOCK)
						WHERE IdContaOrigem = @IdConta OR IdContaDestino = @IdConta
				 ))
			RETURN 2


		RETURN 0 -- Não possui nenhum vinculo 
	END
GO
