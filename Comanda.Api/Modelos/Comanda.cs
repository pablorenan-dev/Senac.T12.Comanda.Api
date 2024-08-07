using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeComandas.Modelos
{
    public class Comanda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int NumeroMesa { get; set; }
        public string NomeCliente { get; set; }
        //colocando valor padrao como 1.
        public int SituacaoComanda { get; set; } = 1;
        //esse collection so server pro c# navegar, nao tem como comparar com algo do dbBeaver por exemplo
        public ICollection<ComandaItem> ComandaItems { get; set;}
        
    }
}
