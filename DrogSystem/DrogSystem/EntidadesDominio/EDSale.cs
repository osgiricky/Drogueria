using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrogSystem.EntidadesDominio
{
    public class EDSale
    {
        public int SaleId { get; set; }
        public int NroFactura { get; set; }
        public string FechaFactura { get; set; }
        public decimal ValorFactura { get; set; }
        public List<EDSaleDetail> ListaFactura { get; set; }
    }
}