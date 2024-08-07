using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeComandas.Modelos
{
    public class PedidoCozinha
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ComandaId { get; set; }
        public int SituacaoId { get; set; } = 1;
        //virtual, pq nao quero ela no banco
        public virtual ICollection<PedidoCozinhaItem> PedidoCozinhaItems { get; set; }
    }
}
