
USE conFin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelNotificacao]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelNotificacao]
GO

CREATE PROCEDURE [dbo].[SP_SelNotificacao]
	@IdUsuario		int 

	AS

	/*
		Documentação
		Arquivo Fonte.....: Notificacao.sql
		Objetivo..........: lista as notificações de um usuário
		Autor.............: Edmar Costa
 		Data..............: 14/10/2017
		Ex................: EXEC [dbo].[SP_SelNotificacao] 9002
	*/

	BEGIN

		SELECT  n.Id,
				n.IdTipo,
				nt.Descricao AS DescricaoTipo,
				n.IdUsuarioEnvio,
				us.Nome AS NomeUsuarioEnvio,
				n.IdUsuarioDestino,
				n.DataCadastro,
				n.DataLeitura,
				n.Mensagem,
				n.ParametrosUrl
			FROM Notificacao n WITH(NOLOCK)
				INNER JOIN NotificacaoTipo nt WITH(NOLOCK)
					ON nt.Id = n.IdTipo
				INNER JOIN Usuario us WITH(NOLOCK)
					ON us.Id = n.IdUsuarioEnvio
			WHERE n.IdUsuarioDestino = @IdUsuario

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_InsNotificacao]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_InsNotificacao]
GO

CREATE PROCEDURE [dbo].[SP_InsNotificacao]
	@IdTipo				smallint, 
	@IdUsuarioEnvio		int, 
	@IdUsuarioDestino	int, 
	@Mensagem			varchar(500),
	@ParametrosUrl		varchar(400) = NULL

	AS

	/*
		Documentação
		Arquivo Fonte.....: Notificacao.sql
		Objetivo..........: Cadastrar nova notificação para um usuário
		Autor.............: Edmar Costa
 		Data..............: 14/10/2017
		Ex................: BEGIN TRANSACTION
							EXEC [dbo].[SP_InsNotificacao] 10, 8002, 9002
							ROLLBACK TRANSACTION
	*/

	BEGIN

		INSERT INTO Notificacao(IdTipo, IdUsuarioEnvio, IdUsuarioDestino, DataCadastro, Mensagem, ParametrosUrl) 
			VALUES (@IdTipo, @IdUsuarioEnvio, @IdUsuarioDestino, GETDATE(), @Mensagem, @ParametrosUrl)

	END
GO




IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_UpdDataLeituraNotificacao]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_UpdDataLeituraNotificacao]
GO

CREATE PROCEDURE [dbo].[SP_UpdDataLeituraNotificacao]
	@Id	int

	AS

	/*
		Documentação
		Arquivo Fonte.....: Notificacao.sql
		Objetivo..........: Insere data de leitura em uma notificação
		Autor.............: Edmar Costa
 		Data..............: 14/10/2017
		Ex................: BEGIN TRANSACTION
							EXEC [dbo].[SP_UpdDataLeituraNotificacao] 1
							ROLLBACK TRANSACTION
	*/

	BEGIN

		UPDATE Notificacao SET DataLeitura = GETDATE()
			WHERE Id = @Id

	END
GO
