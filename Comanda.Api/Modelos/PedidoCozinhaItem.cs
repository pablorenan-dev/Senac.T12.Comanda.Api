
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeComandas.Modelos
{
    public class PedidoCozinhaItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PedidoCozinhaId { get; set; }
        //virutal quer dizer que nao vai ser criada no banco de dados, vai ser usada somente para navegar nas propriedades
        public virtual PedidoCozinha PedidoCozinha { get; set; }
        public int ComandaItemId { get; set; }
        public virtual ComandaItem ComandaItem { get; set; }
    }
}
