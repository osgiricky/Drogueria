using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DrogSystem.Models
{
    [Table("Factura")]
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }

        [Required()]
        [DisplayName("Número Factura")]
        public int NroFactura { get; set; }

        [Required()]
        [DisplayName("Fecha Factura")]
        public DateTime FechaFactura { get; set; }

        [Required()]
        [DisplayName("Valor Factura")]
        public decimal  ValorFactura { get; set; }


        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}