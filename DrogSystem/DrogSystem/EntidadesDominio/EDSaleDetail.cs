using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrogSystem.EntidadesDominio
{
    public class EDSaleDetail
    {
        public int SaleDetailId { get; set; }
        public int SaleId { get; set; }
        public int ProductDetailId { get; set; }
        public string CodBarras { get; set; }
        public int Quantity { get; set; }
        public decimal PrecioTotal { get; set; }
        public string NombreProducto { get; set; }
        public int MarkerId { get; set; }
        public string NombreFabricante { get; set; }
        public int PresentationId { get; set; }
        public string NombrePresentacion { get; set; }
        public int CantPresentacion { get; set; }
        public decimal Precio { get; set; }
    }
}