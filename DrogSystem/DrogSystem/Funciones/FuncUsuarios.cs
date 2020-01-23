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
            ListaEDTipoTercero = ListaEDTipoTercero.OrderBy(o => o.TipoTercero).ToList();
            return ListaEDTipoTercero;
        }

        public List<EDProvider> ListaTerceros()
        {
            List<EDProvider> ListaEDTercero = new List<EDProvider>();
            List<Provider> ListaProvider = new List<Provider>();
            using (DrogSystemContext db = new DrogSystemContext())
            {

                var Terceros = (from s in db.Providers
                                select s).ToList<Provider>();
                if (Terceros != null)
                {
                    ListaProvider = Terceros;
                }
            }
            foreach (var item in ListaProvider)
            {
                EDProvider EDProvider = new EDProvider();
                EDProvider.TerceroId = item.TerceroId;
                EDProvider.NombreTercero = item.NombreTercero;
                EDProvider.Codtercero = item.Codtercero;
                ListaEDTercero.Add(EDProvider);
            }
            ListaEDTercero = ListaEDTercero.OrderBy(o => o.NombreTercero).ToList();
            return ListaEDTercero;
        }

        public List<EDMarker> ListaFabricante()
        {
            List<EDMarker> ListaEDMarker = new List<EDMarker>();
            List<Marker> ListaMarker = new List<Marker>();
            using (DrogSystemContext db = new DrogSystemContext())
            {

                var Marker = (from s in db.Markers
                                select s).ToList<Marker>();
                if (Marker != null)
                {
                    ListaMarker = Marker;
                }
            }
            foreach (var item in ListaMarker)
            {
                EDMarker EDMarker = new EDMarker();
                EDMarker.MarkerId = item.MarkerId;
                EDMarker.NombreFabricante = item.NombreFabricante;
                ListaEDMarker.Add(EDMarker);
            }
            ListaEDMarker = ListaEDMarker.OrderBy(o => o.NombreFabricante).ToList();
            return ListaEDMarker;
        }
    }
}