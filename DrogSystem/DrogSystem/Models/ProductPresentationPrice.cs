using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrogSystem.Models
{
    [Table("PrecioProducto")]
    public class ProductPresentationPrice
    {
        [Key]
        public int PrecioProductoId { get; set; }

        [Required()]
        [DisplayName("Precio")]
        public decimal Precio { get; set; }

        public int PresentationId { get; set; }
        public int DetailProductId { get; set; }
        public virtual Presentation Presentation { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }

    }
}