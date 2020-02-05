using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DrogSystem.Models
{
    [Table("EntradaDetalle")]

    public class EntryDetail
    {
        [Key]

        public int EntryDetailId { get; set; }

        [Required()]
        [DisplayName("Cantidad")]
        public int Cantidad { get; set; }

        [DisplayName("Lote")]
        [MaxLength(15)]
        public string Lote { get; set; }

        [DisplayName("Fecha Vencimiento")]
        public DateTime FechaVence { get; set; }

        public int EntradaId { get; set; }
        public virtual Entry Entry { get; set; }

        public int ProductDetailId { get; set; }       
        public virtual ProductDetail ProductDetails { get; set; }
        
    }
}