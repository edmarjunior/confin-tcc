
------------------------------- BEGIN CREATE DATABASE -------------------------------------------------------------

IF (EXISTS (SELECT name FROM dbo.sysdatabases WHERE ('[' + name + ']' = 'ConFin' OR name = 'ConFin')))
	PRINT 'confin database already exists'
ELSE
	BEGIN
		CREATE DATABASE confin
		PRINT 'created confin database with success!'
	END

------------------------------ END CREATE DATABASE ----------------------------------------------------------------

USE confin

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Usuario')
    PRINT 'Usuario table already exists'
ELSE
	BEGIN
		CREATE TABLE Usuario(
			Id						int identity(1,1) primary key,
			Nome					varchar(200) NOT NULL,
			Email					varchar(200) NOT NULL UNIQUE,
			Senha					varchar(100) NOT NULL,
			DataCadastro			datetime NOT NULL,
			DataSolConfirmCadastro	datetime,
			DataConfirmCadastro		datetime,
			DataUltimaAlteracao		datetime,
			DataDesativacao			datetime
		);
		PRINT 'created Usuario table with success!'
	END 
-- DROP TABLE Usuario




IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'LoginSolicitacaoTrocaSenha')
    PRINT 'LoginSolicitacaoTrocaSenha table already exists'
ELSE
	BEGIN
		CREATE TABLE LoginSolicitacaoTrocaSenha(
			IdUsuario				 int NOT NULL,
			Token					 varchar(100) NOT NULL,
			DataCadastro			 datetime NOT NULL,
			DataUsuarioConfirmacao	 datetime

			CONSTRAINT PK_LoginSolicitacaoTrocaSenha PRIMARY KEY NONCLUSTERED (IdUsuario, Token)
			CONSTRAINT FK_LoginSolicitacaoTrocaSenha_Usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id)

		);
		PRINT 'LoginSolicitacaoTrocaSenha table with success!'
	END
-- ALTER TABLE LoginSolicitacaoTrocaSenha DROP CONSTRAINT FK_LoginSolicitacaoTrocaSenha_Usuario;
-- DROP TABLE LoginSolicitacaoTrocaSenha 




IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'ContaFinanceiraTipo')
    PRINT 'ContaFinanceiraTipo table already exists'
ELSE
	BEGIN
		CREATE TABLE ContaFinanceiraTipo(
			Id						tinyint identity(1,1) primary key,
			Nome					varchar(100) NOT NULL,
		);
		PRINT 'created ContaFinanceiraTipo table with success!'
	END 
-- DROP TABLE ContaFinanceiraTipo




IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'ContaFinanceira')
    PRINT 'ContaFinanceira table already exists'
ELSE
	BEGIN
		CREATE TABLE ContaFinanceira(
			Id						 int identity(1,1) primary key,
			Nome					 varchar(100) NOT NULL,
			IdTipo					 tinyint NOT NULL,
			ValorSaldoInicial		 decimal(12,2) NOT NULL,
			Descricao				 varchar(250),
			IdUsuarioCadastro		 int NOT NULL,
			DataCadastro			 datetime NOT NULL,
			IdUsuarioUltimaAlteracao int,
			DataUltimaAlteracao		 datetime,

			CONSTRAINT FK_ContaFinanceira_ContaFinanceiraTipo FOREIGN KEY (IdTipo) REFERENCES ContaFinanceiraTipo(Id),
			CONSTRAINT FK_ContaFinanceira_Usuario	FOREIGN KEY (IdUsuarioCadastro) REFERENCES Usuario(Id),
			CONSTRAINT FK2_ContaFinanceira_Usuario	FOREIGN KEY (IdUsuarioUltimaAlteracao) REFERENCES Usuario(Id)

		);
		PRINT 'created ContaFinanceira table with success!'
	END 
--ALTER TABLE ContaFinanceira DROP CONSTRAINT FK_ContaFinanceira_ContaFinanceiraTipo;
--ALTER TABLE ContaFinanceira DROP CONSTRAINT FK_ContaFinanceira_Usuario;
--ALTER TABLE ContaFinanceira DROP CONSTRAINT FK2_ContaFinanceira_Usuario;
--DROP TABLE ContaFinanceira



IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'LancamentoCategoria')
    PRINT 'LancamentoCategoria table already exists'
ELSE
	BEGIN
		CREATE TABLE LancamentoCategoria(
			Id						 int identity(1,1) primary key,
			Nome					 varchar(100) NOT NULL,
			IdCategoriaSuperior		 int NULL,
			Cor						 varchar(10) NOT NULL,
			IdUsuarioCadastro		 int NOT NULL,
			DataCadastro			 datetime NOT NULL,
			IdUsuarioUltimaAlteracao int,
			DataUltimaAlteracao		 datetime,

			CONSTRAINT FK_LancamentoCategoria_LancamentoCategoria FOREIGN KEY (IdCategoriaSuperior) REFERENCES LancamentoCategoria(Id),
			CONSTRAINT FK_LancamentoCategoria_Usuario	FOREIGN KEY (IdUsuarioCadastro) REFERENCES Usuario(Id),
			CONSTRAINT FK2_LancamentoCategoria_Usuario	FOREIGN KEY (IdUsuarioUltimaAlteracao) REFERENCES Usuario(Id)

		);
		PRINT 'created LancamentoCategoria table with success!'
	END 
-- DROP TABLE LancamentoCategoria




IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Lancamento')
    PRINT 'Lancamento table already exists'
ELSE
	BEGIN
		CREATE TABLE Lancamento(
			Id						 int identity(1,1) primary key,
			IndicadorReceitaDespesa	 varchar(1) NOT NULL,
			Descricao				 varchar(100) NOT NULL,
			Valor					 decimal(12,2) NULL,
			DataLancamento			 datetime NOT NULL,
			IdConta					 int NOT NULL,
			IdCategoria				 int NOT NULL,
			IndicadorPagoRecebido	 varchar(1) NOT NULL,
			IdUsuarioCadastro		 int NOT NULL,
			DataCadastro			 datetime NOT NULL,
			IdUsuarioUltimaAlteracao int,
			DataUltimaAlteracao		 datetime,

			CONSTRAINT FK_Lancamento_ContaFinanceira FOREIGN KEY (IdConta) REFERENCES ContaFinanceira(Id),
			CONSTRAINT FK_Lancamento_LancamentoCategoria FOREIGN KEY (IdCategoria) REFERENCES LancamentoCategoria(Id),
			CONSTRAINT FK_Lancamento_Usuario	FOREIGN KEY (IdUsuarioCadastro) REFERENCES Usuario(Id),
			CONSTRAINT FK2_Lancamento_Usuario	FOREIGN KEY (IdUsuarioUltimaAlteracao) REFERENCES Usuario(Id)

		);
		PRINT 'created Lancamento table with success!'
	END 
-- DROP TABLE Lancamento





IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Transferencia')
    PRINT 'Transferencia table already exists'
ELSE
	BEGIN
		CREATE TABLE Transferencia(
			Id						 int identity(1,1) primary key,
			IdContaOrigem			 int NOT NULL,
			IdContaDestino			 int NOT NULL,
			Valor					 decimal(12,2) NULL,
			Descricao				 varchar(100) NOT NULL,
			DataTransferencia		 datetime NOT NULL,
			IdCategoria				 int NOT NULL,
			IndicadorPagoRecebido	 varchar(1) NOT NULL,
			IdUsuarioCadastro		 int NOT NULL,
			DataCadastro			 datetime NOT NULL,
			IdUsuarioUltimaAlteracao int,
			DataUltimaAlteracao		 datetime,

			CONSTRAINT FK_Transferencia_ContaFinanceira FOREIGN KEY (IdContaOrigem) REFERENCES ContaFinanceira(Id),
			CONSTRAINT FK2_Transferencia_ContaFinanceira FOREIGN KEY (IdContaDestino) REFERENCES ContaFinanceira(Id),
			CONSTRAINT FK_Transferencia_LancamentoCategoria FOREIGN KEY (IdCategoria) REFERENCES LancamentoCategoria(Id),
			CONSTRAINT FK_Transferencia_Usuario	FOREIGN KEY (IdUsuarioCadastro) REFERENCES Usuario(Id),
			CONSTRAINT FK2_Transferencia_Usuario	FOREIGN KEY (IdUsuarioUltimaAlteracao) REFERENCES Usuario(Id)

		);
		PRINT 'created Transferencia table with success!'
	END 
-- DROP TABLE Transferencia



IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Periodo')
    PRINT 'Periodo table already exists'
ELSE
	BEGIN
		CREATE TABLE Periodo(
			Id						 tinyint identity(1,1) primary key,
			Descricao				 varchar(20) NOT NULL,
			Quantidade				 tinyint NOT NULL,
			IndicadorDiaMes			 varchar(1) NOT NULL -- D(dia)   M(mês)
		);
		PRINT 'created Periodo table with success!'
	END 
-- DROP TABLE Periodo



IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Compromisso')
    PRINT 'Compromisso table already exists'
ELSE
	BEGIN
		CREATE TABLE Compromisso(
			Id						int identity(1,1) primary key,
			Descricao				varchar(100) NOT NULL,
			IdPeriodo				tinyint NOT NULL,
			DataInicio				datetime NOT NULL,
			TotalParcelasOriginal	smallint,
			IdUsuarioCadastro		int NOT NULL,
			DataCadastro			datetime NOT NULL,
			IdConta					int NOT NULL,


			CONSTRAINT FK_Compromisso_Periodo FOREIGN KEY (IdPeriodo) REFERENCES Periodo(Id),
			CONSTRAINT FK_Compromisso_ContaFinanceira FOREIGN KEY (IdConta) REFERENCES ContaFinanceira(Id)

		);
		PRINT 'created Compromisso table with success!'
	END 
--ALTER TABLE Compromisso DROP CONSTRAINT FK_Compromisso_Periodo;
-- DROP TABLE Compromisso

	


IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'CompromissoLancamento')
    PRINT 'CompromissoLancamento table already exists'
ELSE
	BEGIN
		CREATE TABLE CompromissoLancamento(
			IdCompromisso		int,
			IdLancamento		int NOT NULL,
			NumeroLancamento	smallint NOT NULL,

			CONSTRAINT PK_CompromissoLancamento PRIMARY KEY NONCLUSTERED (IdCompromisso, IdLancamento),
			CONSTRAINT FK_CompromissoLancamento_Compromisso FOREIGN KEY (IdCompromisso) REFERENCES Compromisso(Id),
			CONSTRAINT FK_CompromissoLancamento_Lancamento FOREIGN KEY (IdLancamento) REFERENCES Lancamento(Id)
		);
		PRINT 'created CompromissoLancamento table with success!'
	END 
-- DROP TABLE CompromissoLancamento



IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'ContaConjunta')
    PRINT 'ContaConjunta table already exists'
ELSE
	BEGIN
		CREATE TABLE ContaConjunta(
			Id						 int identity(1,1) primary key,
			IdConta					 int NOT NULL,
			IdUsuarioEnvio			 int NOT NULL,
			IdUsuarioConvidado		 int NOT NULL,
			DataCadastro			 datetime NOT NULL,
			DataAnalise				 datetime,
			IndicadorAprovado		 varchar(1)

			CONSTRAINT FK_ContaConjunta_ContaFinanceira	FOREIGN KEY (IdConta) REFERENCES ContaFinanceira(Id),
			CONSTRAINT FK_ContaConjunta_Usuario	FOREIGN KEY (IdUsuarioEnvio) REFERENCES Usuario(Id),
			CONSTRAINT FK2_ContaConjunta_Usuario	FOREIGN KEY (IdUsuarioConvidado) REFERENCES Usuario(Id)

		);
		PRINT 'created ContaConjunta table with success!'
	END 
-- DROP TABLE ContaConjunta



IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'ContaConjuntaCategoria')
    PRINT 'ContaConjuntaCategoria table already exists'
ELSE
	BEGIN
		CREATE TABLE ContaConjuntaCategoria(
			IdConta					 int NOT NULL,
			IdCategoria				 int NOT NULL,

			CONSTRAINT FK_ContaConjuntaCategoria_ContaFinanceira	FOREIGN KEY (IdConta) REFERENCES ContaFinanceira(Id),
			CONSTRAINT FK_ContaConjuntaCategoria_LancamentoCategoria	FOREIGN KEY (IdCategoria) REFERENCES LancamentoCategoria(Id),
		);
		PRINT 'created ContaConjuntaCategoria table with success!'
	END 
-- DROP TABLE ContaConjuntaCategoria



IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'NotificacaoTipo')
    PRINT 'NotificacaoTipo table already exists'
ELSE
	BEGIN
		CREATE TABLE NotificacaoTipo(
			Id					smallint primary key NOT NULL,
			Descricao			varchar(200) NOT NULL,
			DataCadastro		datetime NOT NULL
		);
		PRINT 'created NotificacaoTipo table with success!'
	END 
-- DROP TABLE NotificacaoTipo



IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Notificacao')
    PRINT 'Notificacao table already exists'
ELSE
	BEGIN
		CREATE TABLE Notificacao(
			Id					int primary key identity(1,1) NOT NULL,
			IdTipo				smallint NOT NULL,
			IdUsuarioEnvio		int NOT NULL,
			IdUsuarioDestino	int NOT NULL,
			DataCadastro		datetime NOT NULL,
			DataLeitura			datetime,
			Mensagem			varchar(500),
			ParametrosUrl		varchar(400)

			CONSTRAINT FK_Notificacao_NotificacaoTipo FOREIGN KEY (IdTipo) REFERENCES NotificacaoTipo(Id),
			CONSTRAINT FK_Notificacao_Usuario FOREIGN KEY (IdUsuarioEnvio) REFERENCES Usuario(Id),
			CONSTRAINT FK2_Notificacao_Usuario FOREIGN KEY (IdUsuarioDestino) REFERENCES Usuario(Id)
		);
		PRINT 'created Notificacao table with success!'
	END 
-- DROP TABLE Notificacao



IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'OpcaoMenu')
    PRINT 'OpcaoMenu table already exists'
ELSE
	BEGIN
		CREATE TABLE OpcaoMenu(
			Codigo				int primary key NOT NULL,
			CodigoMae			int,
			Descricao			varchar(400) NOT NULL,
			DataCadastro		datetime NOT NULL,
			Uri					varchar(400)
		);
		PRINT 'created OpcaoMenu table with success!'
	END
-- DROP TABLE OpcaoMenu
 


IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'OpcaoMenuAcessos')
    PRINT 'OpcaoMenuAcessos table already exists'
ELSE
	BEGIN
		CREATE TABLE OpcaoMenuAcessos(
			CodigoOpcao		int NOT NULL,
			IdUsuario		int NOT NULL,
			DataAcesso		datetime NOT NULL,

			CONSTRAINT PK_OpcaoMenuAcessos PRIMARY KEY NONCLUSTERED (CodigoOpcao, IdUsuario, DataAcesso)
		);
		PRINT 'created OpcaoMenuAcessos table with success!'
	END
-- DROP TABLE OpcaoMenuAcessos