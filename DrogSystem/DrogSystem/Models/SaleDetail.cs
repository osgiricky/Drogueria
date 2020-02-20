using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DrogSystem.Models
{
    [Table("DetalleFactura")]
    public class SaleDetail
    {
        [Key]
        public int SaleDetailId { get; set; }
        public int SaleId { get; set; }

        public int ProductDetailId { get; set; }
        public int PresentationId { get; set; }

        [Required()]
        [DisplayName("Cantidad")]
        public int Quantity { get; set; }

        [Required()]
        [DisplayName("Precio")]
        public decimal PrecioTotal { get; set; }

        public virtual Presentation Presentation { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public virtual Sale Sale { get; set; }

    }
}