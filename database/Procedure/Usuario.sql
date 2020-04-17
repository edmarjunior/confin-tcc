
USE confin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_InsUsuario]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_InsUsuario]
GO

CREATE PROCEDURE [dbo].[SP_InsUsuario]
	@Nome	varchar(100),
	@Email	varchar(100),
	@Senha	varchar(50)

	AS

	/*
	Documentação
	Arquivo Fonte.....: Usuario.sql
	Objetivo..........: Cadastrar novo usuario
	Autor.............: Edmar Costa
 	Data..............: 01/08/2017
	Ex................: BEGIN TRANSACTION
						DECLARE @Ret int
						EXEC @Ret = [dbo].[SP_InsUsuario] 'FULANO', 'fulano@gmail.com.br', 'teste123'
						SELECT @Ret
						ROLLBACK TRANSACTION
	*/

	BEGIN

		DECLARE @Data_Atual datetime = GETDATE()

		INSERT INTO Usuario(Nome, Email, Senha, DataCadastro, DataSolConfirmCadastro) 
			VALUES (@Nome, @Email, HashBytes('MD5', @Senha), @Data_Atual, @Data_Atual)

		RETURN SCOPE_IDENTITY()

	END
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_SelUsuario]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_SelUsuario]
GO

CREATE PROCEDURE [dbo].[SP_SelUsuario]
	@Email	varchar(100) = NULL,
	@Senha	varchar(50) = NULL,
	@Id		int = NULL

	AS

	/*
	Documentação
	Arquivo Fonte.....: Usuario.sql
	Objetivo..........: buscar dados do usuario atraves do email e senha
	Autor.............: Edmar Costa
 	Data..............: 01/08/2017
	Ex................: EXEC [dbo].[SP_SelUsuario] null, null, 3004

	*/

	BEGIN
		
		IF @Senha IS NOT NULL
			SET @Senha = HashBytes('MD5', @Senha)

		SELECT  Id,
				Nome,
				Email,
				Senha,
				DataCadastro,
				DataSolConfirmCadastro,
				DataConfirmCadastro,
				DataUltimaAlteracao,
				DataDesativacao
			FROM Usuario WITH(NOLOCK)
			WHERE (@Email IS NULL OR Email = @Email)
				AND (@Senha IS NULL OR Senha = @Senha)
				AND (@Id IS NULL OR Id = @Id)

	END
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_UpdConfirmacaoCadastroUsuario]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_UpdConfirmacaoCadastroUsuario]
GO

CREATE PROCEDURE [dbo].[SP_UpdConfirmacaoCadastroUsuario]
	@Id	int

	AS

	/*
	Documentação
	Arquivo Fonte.....: Usuario.sql
	Objetivo..........: Realiza a confirmação do cadastro do usuário
	Autor.............: Edmar Costa
 	Data..............: 02/08/2017
	Ex................: EXEC [dbo].[SP_UpdConfirmacaoCadastroUsuario] 7

	*/

	BEGIN
	
		UPDATE Usuario SET DataConfirmCadastro = GETDATE()
			WHERE Id = @Id

	END
GO



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_UpdUsuarioSenha]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_UpdUsuarioSenha]
GO

CREATE PROCEDURE [dbo].[SP_UpdUsuarioSenha]
	@Id			int,
	@NovaSenha	varchar(100)

	AS

	/*
	Documentação
	Arquivo Fonte.....: Usuario.sql
	Objetivo..........: Altera a senha de acesso do usuário
	Autor.............: Edmar Costa
 	Data..............: 07/08/2017
	Ex................: EXEC [dbo].[SP_UpdUsuarioSenha] 7

	*/

	BEGIN
	
		UPDATE Usuario 
			SET Senha = HashBytes('MD5', @NovaSenha),
			    DataUltimaAlteracao = GETDATE()
			WHERE Id = @Id

	END
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_UpdUsuario]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_UpdUsuario]
GO

CREATE PROCEDURE [dbo].[SP_UpdUsuario]
	@Id			int,
	@Nome		varchar(200),
	@Email		varchar(200)

	AS

	/*
	Documentação
	Arquivo Fonte.....: Usuario.sql
	Objetivo..........: Altera os dados do usuário
	Autor.............: Edmar Costa
 	Data..............: 04/10/2017
	Ex................: EXEC [dbo].[SP_UpdUsuario] 7

	*/

	BEGIN
	
		UPDATE Usuario 
			SET Nome = @Nome,
			    Email = @Email,
			    DataUltimaAlteracao = GETDATE()
			WHERE Id = @Id

	END
GO
