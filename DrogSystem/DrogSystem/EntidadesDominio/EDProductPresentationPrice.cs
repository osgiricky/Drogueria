using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrogSystem.EntidadesDominio
{
    public class EDProductPresentationPrice
    {
        public int PrecioProductoId { get; set; }
        public decimal Precio { get; set; }
        public int PresentationId { get; set; }
        public string NombrePresentacion { get; set; }
        public int CantPresentacion { get; set; }
        public int MarkerId { get; set; }
        public string NombreFabricante { get; set; }
        public int DetailProductId { get; set; }
        public string CodBarras { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public List<EDPresentacion> ListaPresentacion { get; set; }
    }
}