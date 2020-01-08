using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DrogSystem.Models
{
    [Table("TipoPresentacion")]
    public class PresentationType
    {
        [Key]
        public int PresentationTypeId { get; set; }
        [Required()]
        [MaxLength(50)]
        [DisplayName("Nombre Presentación")]
        public string NombrePresentacion { get; set; }
    }
}