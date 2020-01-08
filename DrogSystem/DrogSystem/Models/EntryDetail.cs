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

    public class EntryDetail
    {
        [Key]

        public int EntryDetailId { get; set; }

        [Required()]
        [DisplayName("Cantidad")]
        public int Cantidad { get; set; }

        [Required()]
        [DisplayName("Lote")]
        [MaxLength(15)]
        public string Lote { get; set; }

        [DisplayName("Registro Invima")]
        [MaxLength(30)]
        public string RegInvima { get; set; }


        [Required()]
        [DisplayName("Fecha Vencimiento")]
        [MaxLength(15)]
        public DateTime FechaVence { get; set; }
        public int EntryId { get; set; }

        public int ProductDetailId { get; set; }

        public virtual Entry Entry { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        
    }
}