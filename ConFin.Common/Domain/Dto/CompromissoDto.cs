using ConFin.Common.Domain.Auxiliar;
using System;

namespace ConFin.Common.Domain.Dto
{
    public class CompromissoDto : DataManut
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public byte IdPeriodo { get; set; }
        public DateTime DataInicio { get; set; }
        public short? TotalParcelasOriginal { get; set; }
        public int IdConta { get; set; }
    }
}
