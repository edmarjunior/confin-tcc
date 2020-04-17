
USE conFin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelTransferencias]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelTransferencias]
GO

CREATE PROCEDURE [dbo].[SP_SelTransferencias]
	@IdUsuario	int

	AS

	/*
		Documentação
		Arquivo Fonte.....: Transferencia.sql
		Objetivo..........: Retorna as transferencias vinculadas a um usuário  
		Autor.............: Edmar Costa
 		Data..............: 20/08/2017
		Ex................: EXEC [dbo].[SP_SelTransferencias] 8002
	*/

	BEGIN

		SELECT DISTINCT  tr.Id,
				tr.IdContaOrigem,
				(cfo.Nome + CASE WHEN cfo.IdUsuarioCadastro <> @IdUsuario THEN ' ('+usuCfo.Nome+')' ELSE '' END) AS NomeContaOrigem,
				tr.IdContaDestino,
				(cfd.Nome + CASE WHEN cfd.IdUsuarioCadastro <> @IdUsuario THEN ' ('+usuCfd.Nome+')' ELSE '' END) AS NomeContaDestino,
				tr.Valor,
				tr.Descricao,
				tr.DataTransferencia,
				tr.IdCategoria,
				lc.Nome AS NomeCategoria,
				lc.Cor AS CorCategoria,
				tr.IndicadorPagoRecebido,
				tr.IdUsuarioCadastro
			FROM Transferencia tr WITH(NOLOCK)
				INNER JOIN ContaFinanceira cfo WITH(NOLOCK)
					ON cfo.Id = tr.IdContaOrigem
				INNER JOIN ContaFinanceira cfd WITH(NOLOCK)
					ON cfd.Id = tr.IdContaDestino
				INNER JOIN LancamentoCategoria lc WITH(NOLOCK)
					ON lc.Id = tr.IdCategoria
				INNER JOIN Usuario usuCfo WITH(NOLOCK)
					ON usuCfo.Id = cfo.IdUsuarioCadastro
				INNER JOIN Usuario usuCfd WITH(NOLOCK)
					ON usuCfd.Id = cfd.IdUsuarioCadastro
				LEFT OUTER JOIN ContaConjunta cc WITH(NOLOCK)
					ON cc.IdConta IN (cfo.Id, cfd.Id)
						AND cc.IndicadorAprovado = 'A'
			WHERE (cfo.IdUsuarioCadastro = @IdUsuario OR cfd.IdUsuarioCadastro = @IdUsuario OR cc.IdUsuarioConvidado = @IdUsuario)

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelTransferencia]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelTransferencia]
GO

CREATE PROCEDURE [dbo].[SP_SelTransferencia]
	@IdTransferencia	int

	AS

	/*
		Documentação
		Arquivo Fonte.....: Transferencia.sql
		Objetivo..........: Retorna os dados de um transferencia
		Autor.............: Edmar Costa
 		Data..............: 20/08/2017
		Ex................: EXEC [dbo].[SP_SelTransferencia] 1
	*/

	BEGIN

		SELECT  tr.Id,
				tr.IdContaOrigem,
				tr.IdContaDestino,
				tr.Valor,
				tr.Descricao,
				tr.DataTransferencia,
				tr.IdCategoria,
				tr.IndicadorPagoRecebido,
				tr.IdUsuarioCadastro,
				usuCad.Id AS NomeUsuarioCadastro,
				tr.DataCadastro,
				tr.IdUsuarioUltimaAlteracao,
				usuAlt.Nome AS NomeUsuarioUltimaAlteracao,
				tr.DataUltimaAlteracao
			FROM Transferencia tr WITH(NOLOCK)
				INNER JOIN Usuario usuCad WITH(NOLOCK)
					ON usuCad.Id = tr.IdUsuarioCadastro
				LEFT OUTER JOIN Usuario usuAlt WITH(NOLOCK)
					ON usuAlt.Id = tr.IdUsuarioUltimaAlteracao
			WHERE tr.Id = @IdTransferencia

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_InsTransferencia]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_InsTransferencia]
GO

CREATE PROCEDURE [dbo].[SP_InsTransferencia]
	@IdContaOrigem			int,
	@IdContaDestino			int,
	@Valor					decimal(12,2),
	@Descricao				varchar(100) = NULL,
	@Data					datetime,
	@IdCategoria			int,
	@IndicadorPagoRecebido	varchar(1),
	@IdUsuarioCadastro		int

	AS

	/*
	Documentação
	Arquivo Fonte.....: Transferencia.sql
	Objetivo..........: Insere uma nova transferencia de valores entre contas financeiras
	Autor.............: Edmar Costa
 	Data..............: 20/08/2017
	Ex................: BEGIN TRANSACTION
						DECLARE @Ret int
						EXEC @Ret = [dbo].[SP_InsTransferencia]	  @IdContaOrigem			= 1,
																  @IdContaDestino			= 2,
																  @Valor					= 100.50,
																  @Descricao				= 'transferencia de teste'
																  @Data						= '2017-08-20'
																  @IdCategoria				= 1
																  @IndicadorPagoRecebido	= 'S'
																  @IdUsuarioCadastro		= 1
						SELECT @Ret
						ROLLBACK TRANSACTION
	*/

	BEGIN

		INSERT INTO Transferencia(IdContaOrigem, IdContaDestino, Valor, Descricao, DataTransferencia, IdCategoria, IndicadorPagoRecebido, IdUsuarioCadastro, DataCadastro) 
			VALUES (@IdContaOrigem, @IdContaDestino, @Valor, @Descricao, @Data, @IdCategoria, @IndicadorPagoRecebido, @IdUsuarioCadastro, GETDATE())

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_UpdTransferencia]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_UpdTransferencia]
GO

CREATE PROCEDURE [dbo].[SP_UpdTransferencia]
	@Id						int,
	@IdContaOrigem			int,
	@IdContaDestino			int,
	@Valor					decimal(12,2),
	@Descricao				varchar(100) = NULL,
	@Data					datetime,
	@IdCategoria			int,
	@IndicadorPagoRecebido	varchar(1),
	@IdUsuario				int

	AS

	/*
	Documentação
	Arquivo Fonte.....: Transferencia.sql
	Objetivo..........: Realiza a edição nos dados de uma transferencia  
	Autor.............: Edmar Costa
 	Data..............: 20/08/2017
	Ex................: BEGIN TRANSACTION
						DECLARE @Ret int
						EXEC @Ret = [dbo].[SP_UpdTransferencia]	  @Id						= 1
																  @IdContaOrigem			= 1,
																  @IdContaDestino			= 2,
																  @Valor					= 100.50,
																  @Descricao				= 'transferencia de teste'
																  @Data						= '2017-08-20'
																  @IdCategoria				= 1
																  @IndicadorPagoRecebido	= 'S'
																  @IdUsuario				= 1
						SELECT @Ret
						ROLLBACK TRANSACTION
	*/

	BEGIN

		UPDATE Transferencia
			SET IdUsuarioUltimaAlteracao = @IdUsuario,	
				DataUltimaAlteracao		 = GETDATE(),		
				IdContaOrigem			 = @IdContaOrigem,
				IdContaDestino			 = @IdContaDestino,
				Valor					 = @Valor,
				Descricao				 = @Descricao,
				DataTransferencia		 = @Data,
				IdCategoria				 = @IdCategoria,
				IndicadorPagoRecebido	 = @IndicadorPagoRecebido 
			WHERE Id = @Id

	END
GO





IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_DelTransferencia]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_DelTransferencia]
GO

CREATE PROCEDURE [dbo].[SP_DelTransferencia]
	@Id			int

	AS

	/*
	Documentação
	Arquivo Fonte.....: Transferencia.sql
	Objetivo..........: Realiza a remoção de uma transferencia realizada
	Autor.............: Edmar Costa
 	Data..............: 20/08/2017
	Ex................: BEGIN TRANSACTION
						DECLARE @Ret int
						EXEC @Ret = [dbo].[SP_DelTransferencia] 1
						SELECT @Ret
						ROLLBACK TRANSACTION
	*/

	BEGIN

		DELETE FROM Transferencia
			WHERE Id = @Id

	END
GO








IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_UpdTransferenciaIndicadorPago]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_UpdTransferenciaIndicadorPago]
GO

CREATE PROCEDURE [dbo].[SP_UpdTransferenciaIndicadorPago]
	@IdTransferencia		int,
	@IndicadorPagoRecebido	varchar(1),
	@IdUsuario				int


	AS

	/*
		Documentação
		Arquivo Fonte.....: Transferencia.sql
		Objetivo..........: Realiza a edição no campo IndicadorPagoRecebido de uma transferencia
		Autor.............: Edmar Costa
 		Data..............: 20/08/2017
		Ex................: EXEC [dbo].[SP_UpdTransferenciaIndicadorPago] 1, 'S', 1
	*/

	BEGIN
		
		UPDATE Transferencia
				SET IdUsuarioUltimaAlteracao = @IdUsuario,	
					IndicadorPagoRecebido	 = @IndicadorPagoRecebido,	
					DataUltimaAlteracao		 = GETDATE()			
				WHERE Id = @IdTransferencia

	END
GO
