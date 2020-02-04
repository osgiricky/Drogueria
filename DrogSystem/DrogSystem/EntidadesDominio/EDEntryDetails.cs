﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrogSystem.EntidadesDominio
{
    public class EDEntryDetails
    {
        public int EntryDetailId { get; set; }
        public int Cantidad { get; set; }
        public string Lote { get; set; }
        public DateTime FechaVence { get; set; }
        public int EntryId { get; set; }
        public int ProductDetailId { get; set; }
        public string NombreProducto { get; set; }
        public string Componentes { get; set; }
        public string CodBarras { get; set; }
        public string NombreFabricante { get; set; }
        public string RegInvima { get; set; }

    }
}