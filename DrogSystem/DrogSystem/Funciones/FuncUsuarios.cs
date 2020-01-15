using DrogSystem.EntidadesDominio;
using DrogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrogSystem.Funciones
{
    public class FuncUsuarios
    {
        public List<EDUserType> ListaTiposUsuario()
        {
            List<EDUserType> ListaEDUserType = new List<EDUserType>();
            List<UserType> ListaUserType = new List<UserType>();
            using (DrogSystemContext db = new DrogSystemContext())
            {

                var Cargos = (from s in db.UserTypes
                              select s).ToList<UserType>();
                if (Cargos != null)
                {
                    ListaUserType = Cargos;
                }
            }
            foreach (var item in ListaUserType)
            {
                EDUserType EDUserType = new EDUserType();
                EDUserType.TipoUsuarioId = item.TipoUsuarioId;
                EDUserType.Descripcion = item.Descripcion;
                ListaEDUserType.Add(EDUserType);
            }
            return ListaEDUserType;
        }

        public List<EDProviderType> ListaTiposTerceros()
        {
            List<EDProviderType> ListaEDTipoTercero = new List<EDProviderType>();
            List<ProviderType> ListaProviderType = new List<ProviderType>();
            using (DrogSystemContext db = new DrogSystemContext())
            {

                var Terceros = (from s in db.ProviderTypes
                              select s).ToList<ProviderType>();
                if (Terceros != null)
                {
                    ListaProviderType = Terceros;
                }
            }
            foreach (var item in ListaProviderType)
            {
                EDProviderType EDProviderType = new EDProviderType();
                EDProviderType.ProviderTypeId = item.ProviderTypeId;
                EDProviderType.TipoTercero = item.TipoTercero;
                ListaEDTipoTercero.Add(EDProviderType);
            }
            return ListaEDTipoTercero;
        }
    }
}