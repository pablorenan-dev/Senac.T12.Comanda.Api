 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeComandas.Modelos
{
    public class ComandaItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CardapioItemId { get; set; }
        public virtual CardapioItem CardapioItem { get; set; }
        //Cardapio
        public int ComandaId { get; set; }
        //essa virtual indica quem eh o pai dele(comandaItem qtem um paiChamado comanda)
        public virtual Comanda Comanda { get; set; }
    }
}
