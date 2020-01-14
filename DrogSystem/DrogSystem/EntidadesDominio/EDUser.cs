using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrogSystem.EntidadesDominio
{
    public class EDUser
    {
        public int UsuarioId { get; set; }

        public string CodUsuario { get; set; }

        public string Nombre { get; set; }

        public string Clave { get; set; }

        public int TipoUsuarioId { get; set; }

        public string Descripcion { get; set; }

    }
}