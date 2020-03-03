using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DrogSystem.Models
{
    [Table("Contabilidad")]
    public class Accounting
    {
        [Key]
        public int ContabilidadId { get; set; }

        [Required()]
        public DateTime FechaCierre { get; set; }

        [Required()]
        public decimal Ingresos { get; set; }

        [Required()]
        public decimal Egresos { get; set; }

        [Required()]
        public decimal BaseCaja { get; set; }
    }
}