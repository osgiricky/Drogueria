﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrogSystem.EntidadesDominio
{
    public class EDEntry
    {
        public int EntryId { get; set; }
        public string FechaIngreso { get; set; }
        public string Aprobado { get; set; }
        public int TerceroId { get; set; }
        public string NombreTercero { get; set; }
        public List<EDProvider> ListaTerceros { get; set; }
        public List<EDEntryDetails> ListaEntradas { get; set; }
    }
}