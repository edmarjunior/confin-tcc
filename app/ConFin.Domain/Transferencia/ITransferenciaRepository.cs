﻿using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository.Infra;
using System.Collections.Generic;

namespace ConFin.Domain.Transferencia
{
    public interface ITransferenciaRepository: IBaseRepository
    {
        IEnumerable<TransferenciaDto> GetAll(int idUsuario);
        TransferenciaDto Get(int idTransferencia);
        void Post(TransferenciaDto transferencia);
        void Put(TransferenciaDto transferencia);
        void Delete(int idTransferencia);
        void PutIndicadorPagoRecebido(TransferenciaDto transferencia);
        bool GetVerificaClientePossuiTransferenciaHabilitada(int idUsuario);
    }
}
