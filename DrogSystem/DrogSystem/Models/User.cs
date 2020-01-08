using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrogSystem.Models
{
    [Table("Usuario")]
    public class User
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required()]
        [DisplayName("Identificación Usuario")]
        [MaxLength(30)]
        public string CodUsuario { get; set; }

        [Required()]
        [DisplayName("Nombre Usuario")]
        [MaxLength(30)]
        public string Nombre { get; set; }

        [Required()]
        [DisplayName("Clave Usuario")]
        [MaxLength(30)]
        public string Clave { get; set; }

        [DisplayName("Tipo Usuario")]
        public int TipoUsuarioId { get; set; }

        public virtual UserType UserType { get; set; }
    }
}