using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeComandas.Modelos
{
    public class CardapioItem
    {
        //Key = Chave primaria da tabela
        //DatabaseGenerated = Valor gerado da coluna sera realizado pelo DB
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        //[Required]
        //required significado que eh NOT NULL no banco
        public decimal Preco { get; set; }
        //perguntar se eh correto usar bool, mesmo que no banco eh INT
        public bool PossuiPreparo { get; set; }
    }
}
