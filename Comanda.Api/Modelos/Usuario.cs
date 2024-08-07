using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeComandas.Modelos
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUsuario { get; set; }
        public string nomeUsuario { get; set; } = string.Empty;
        public string emailUsuario { get; set; } = string.Empty;
        public string senhaUsuario { get; set; } = string.Empty;
        public int idFuncaoUsuario { get; set; }
    }
}
