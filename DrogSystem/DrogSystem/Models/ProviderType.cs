using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DrogSystem.Models
{
    public class ProviderType
    {
        [Key]
        public int ProviderTypeId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Tipo Tercero")]
        [MaxLength(30)]
        public string TipoTercero { get; set; }

        public virtual ICollection<Provider> Provider { get; set; }
    }
}