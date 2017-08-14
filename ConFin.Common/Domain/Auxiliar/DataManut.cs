using System;

namespace ConFin.Common.Domain.Auxiliar
{
    public class DataManut
    {
        // atributos do CADASTRO
        public int IdUsuarioCadastro { get; set; }
        public string NomeUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }

        // atributos da EDIÇÃO
        public int? IdUsuarioUltimaAlteracao { get; set; }
        public string NomeUsuarioUltimaAlteracao { get; set; }
        public DateTime? DataUltimaAlteracao { get; set; }
    }
}
