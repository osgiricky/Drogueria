using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DrogSystem.Models
{
    [Table("Presentacion")]
    public class Presentation
    {
        [Key]
        public int PresentationId { get; set; }

        [Required()]
        [MaxLength(50)]
        [DisplayName("Descripción Presentación")]
        public string NombrePresentacion { get; set; }

        [Required()]
        [DisplayName("Cantidad Presentación")]
        public int CantPresentacion { get; set; }


        public virtual ICollection<ProductPresentationPrice> ProductPresentationPrices { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}