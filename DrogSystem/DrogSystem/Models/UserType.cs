using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrogSystem.Models
{
    [Table("TipoUsuario")]
    public class UserType
    {
        [Key]
        public int TipoUsuarioId { get; set; }

        [Required()]
        [DisplayName("Tipo Usuario")]
        [MaxLength(30)]
        public string Descripcion { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}