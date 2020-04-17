-- POPULA BASE TABELA [Periodo]
USE confin

INSERT INTO Periodo(Descricao, Quantidade, IndicadorDiaMes) 
	VALUES ('Mensal', 1, 'M'),
		   ('Diário', 1, 'D'),
		   ('Semanal', 7, 'D'),
		   ('Bimestral', 2, 'M'),
		   ('Trimestral', 3, 'M'),
		   ('Semestral', 6, 'M'),
		   ('Anual', 12, 'M')
