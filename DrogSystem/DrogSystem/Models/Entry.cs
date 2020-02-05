using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DrogSystem.Models
{
    [Table("Entradas")]
    public class Entry
    {
        [Key]
        public int EntradaId { get; set; }

        [Required()]
        [DisplayName("Fecha Ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Required()]
        [DisplayName("Aprobado")]
        [MaxLength(1)]
        public string Aprobado { get; set; }

        public int TerceroId { get; set; }
        public virtual Provider Provider { get; set; }

        public virtual ICollection<EntryDetail> EntryDetails { get; set; }

    }
}