USE confin

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[FNC_CalculaSaldoAtualConta]') AND objectproperty(id, N'IsScalarFunction')=1)
	DROP FUNCTION [dbo].[FNC_CalculaSaldoAtualConta]
GO 

CREATE FUNCTION [dbo].[FNC_CalculaSaldoAtualConta]
	(@IdConta	int)

	RETURNS decimal(14,2)

	AS

	/*
		Documentação
		Objetivo..........:	Busca aliquota personalizada de IPTU
		Autor.............:	SMN - Edmar Costa
 		Data..............:	24/08/2017
		Exemplo...........: SELECT [dbo].[FNC_CalculaSaldoAtualConta](9)
	*/

	BEGIN
		
		DECLARE @ValorSaldoInicialConta	   decimal(14,2),
				@TotReceitasRealizada	   decimal(14,2),
				@TotDespesasRealizada	   decimal(14,2),
				@TotReceitasRealizadaTrans decimal(14,2),
				@TotDespesasRealizadaTrans decimal(14,2)
						
		SELECT  @ValorSaldoInicialConta		= ISNULL(cf.ValorSaldoInicial, 0),
				@TotReceitasRealizada		= ISNULL(la.TotReceitasRealizada, 0),
				@TotDespesasRealizada		= ISNULL(la.TotDespesasRealizada, 0),
				@TotReceitasRealizadaTrans	= ISNULL(tr.TotReceitasRealizadaTrans, 0),
				@TotDespesasRealizadaTrans	= ISNULL(tr.TotDespesasRealizadaTrans, 0)
			FROM ContaFinanceira cf WITH(NOLOCK)
				CROSS APPLY (
								SELECT  SUM(CASE WHEN l.IndicadorReceitaDespesa = 'R' AND l.IndicadorPagoRecebido = 'S' THEN ISNULL(l.Valor,0) ELSE 0 END) AS TotReceitasRealizada,
										SUM(CASE WHEN l.IndicadorReceitaDespesa = 'D' AND l.IndicadorPagoRecebido = 'S' THEN ISNULL(l.Valor,0) ELSE 0 END) AS TotDespesasRealizada
									FROM Lancamento l WITH(NOLOCK)
										WHERE l.IdConta = cf.Id
							) la
				CROSS APPLY (
								SELECT  SUM(CASE WHEN t.IdContaDestino = @IdConta
													AND t.IdContaOrigem <> @IdConta
													AND t.IndicadorPagoRecebido = 'S'
												 THEN ISNULL(t.Valor,0) ELSE 0 END) AS TotReceitasRealizadaTrans,
										SUM(CASE WHEN t.IdContaDestino <> @IdConta AND t.IndicadorPagoRecebido = 'S' THEN ISNULL(t.Valor,0) ELSE 0 END) AS TotDespesasRealizadaTrans
									FROM Transferencia t WITH(NOLOCK)
										WHERE  t.IdContaOrigem = cf.Id OR t.IdContaDestino = cf.Id
							) tr
			WHERE cf.Id = @IdConta

		-- SALDO = SALDO INICIAL DA CONTA (+) TOTAL DE RECEITAS (-) TOTAL DE DESPESAS

		RETURN    @ValorSaldoInicialConta 								-- SALDO INICIAL DA CONTA
				+ (@TotReceitasRealizada + @TotReceitasRealizadaTrans) 	-- RECEITAS (LANÇAMENTOS E TRANSFERENCIAS)
				- (@TotDespesasRealizada + @TotDespesasRealizadaTrans)  -- DESPESAS (LANÇAMENTOS E TRANSFERENCIAS)

	END
GO
