using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrogSystem.EntidadesDominio
{
    public class EDProduct
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int MinStock { get; set; }
        public string Descripcion { get; set; }
        public string Componentes { get; set; }
    }
}