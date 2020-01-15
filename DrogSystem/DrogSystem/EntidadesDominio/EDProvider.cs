using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrogSystem.EntidadesDominio
{
    public class EDProvider
    {
        public int TerceroId { get; set; }
        public string NombreTercero { get; set; }
        public string Codtercero { get; set; }
        public int ProviderTypeId { get; set; }
        public string TipoTercero { get; set; }
        public List<EDProviderType> ListaTipoTercero { get; set; }
    }
}