-- POPULA BASE TABELA [NotificacaoTipo]
USE confin

DECLARE @DataAtual datetime = GETDATE()

INSERT INTO NotificacaoTipo(Id, Descricao, DataCadastro) 
	VALUES (1, 'Convite para conta conjunta', @DataAtual),
		   (2, 'Aceitação de convite para conta conjunta', @DataAtual),
		   (3, 'Recuso de convite para conta conjunta', @DataAtual),
		   (4, 'Cadastro de lançamento em conta conjunta', @DataAtual),
		   (5, 'Edição de lançamento em conta conjunta', @DataAtual),
		   (6, 'Remoção de lançamento em conta conjunta', @DataAtual),
		   (7, 'Cadastro de transferência em conta conjunta', @DataAtual),
		   (8, 'Edição de transferência em conta conjunta', @DataAtual),
		   (9, 'Remoção de transferência em conta conjunta', @DataAtual),
		   (10, 'Cancelamento de compartilhamento de conta conjunta', @DataAtual)