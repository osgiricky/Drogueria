using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrogSystem.EntidadesDominio
{
    public class EDEntry
    {
        public int EntryId { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Aprobado { get; set; }
        public int TerceroId { get; set; }
        public string NombreTercero { get; set; }
    }
}