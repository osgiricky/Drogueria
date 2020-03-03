using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrogSystem.EntidadesDominio
{
    public class EDAccounting
    {
        public int ContabilidadId { get; set; }
        public string FechaCierre { get; set; }
        public decimal Ingresos { get; set; }
        public decimal Egresos { get; set; }
        public decimal BaseCaja { get; set; }
        public decimal BaseInicial { get; set; }
    }
}