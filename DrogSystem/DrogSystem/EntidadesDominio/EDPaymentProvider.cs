using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrogSystem.EntidadesDominio
{
    public class EDPaymentProvider
    {
        public int Id_Pago { get; set; }
        public decimal Valor_Pago { get; set; }
        public string Fecha_Pago { get; set; }
        public string Observacion { get; set; }
        public int TerceroId { get; set; }
        public string NombreTercero { get; set; }
        public List<EDProvider> ListaTerceros { get; set; }

    }
}