using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrogSystem.EntidadesDominio
{
    public class EDProductDetail
    {
        public int ProductDetailId { get; set; }
        public string CodBarras { get; set; }
        public string RegInvima { get; set; }
        public int Existencias { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int MarkerId { get; set; }
        public string NombreFabricante { get; set; }
        public List<EDMarker> ListaFabricantes { get; set; }
        public List<EDProduct> ListaProductos { get; set; }

    }
}