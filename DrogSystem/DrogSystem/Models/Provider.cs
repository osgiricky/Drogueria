using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrogSystem.Models
{
    [Table("Terceros")]
    public class Provider
    {
        [Key]
        public int TerceroId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Nombre Tercero")]
        [MaxLength(30)]
        public string NombreTercero { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Identificación Tercero")]
        [MaxLength(30)]
        public string Codtercero { get; set; }

        [Required]
        [MaxLength(1)]
        public string TipoTercero { get; set; }


        public virtual ICollection<PaymentProvider> PaymentProvider { get; set; }
    }
}