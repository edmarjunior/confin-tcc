using ConFin.Common.Domain.Auxiliar;

namespace ConFin.Common.Domain.Dto
{
    public class LancamentoCategoriaDto: DataManut
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int? IdCategoriaSuperior { get; set; }
        public string Cor { get; set; }
    }
}
