using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrogSystem.Models
{
    [Table("ProductoDetalle")]
    public class ProductDetail
    {
        [Key]
        public int ProductDetailId { get; set; }

        [Required()]
        [DisplayName("Codigo Barras")]
        public int CodBarras { get; set; }

        [DisplayName("Registro Invima")]
        [MaxLength(30)]
        public string RegInvima { get; set; }

        [Required()]
        [DisplayName("Existencias")]
        public int Existencias { get; set; }

        public int ProductoId { get; set; }

        public int MarkerId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Marker Marker { get; set; }

        public virtual ICollection<ProductPresentationPrice> ProductPresentationPrices { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}