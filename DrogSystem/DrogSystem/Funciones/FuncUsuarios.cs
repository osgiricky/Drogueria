using DrogSystem.EntidadesDominio;
using DrogSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace DrogSystem.Funciones
{
    public class FuncUsuarios
    {
        public List<EDProvider> ListaProveedores()
        {
            List<Provider> ListaProveedores = new List<Provider>();
            List<EDProvider> ListaEDProveedores = new List<EDProvider>();
            using (DrogSystemContext db = new DrogSystemContext())
            {

                var Proveedor = (from p in db.Providers
                                where p.TipoTercero == "P"
                                select p).ToList();
                if (Proveedor != null)
                {
                    ListaProveedores = Proveedor;
                }
            }
            foreach (var item in ListaProveedores)
            {
                EDProvider EDProvider = new EDProvider();
                EDProvider.TerceroId = item.TerceroId;
                EDProvider.NombreTercero = item.NombreTercero;
                ListaEDProveedores.Add(EDProvider);
            }
            ListaEDProveedores = ListaEDProveedores.OrderBy(o => o.NombreTercero).ToList();
            return ListaEDProveedores;
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

        public List<EDPresentacion> ListaNombrePresentacion()
        {
            List<EDPresentacion> ListaEDPresentacion = new List<EDPresentacion>();
            List<Presentation> ListaPresentation = new List<Presentation>();
            using (DrogSystemContext db = new DrogSystemContext())
            {

                var Presentacion = (from s in db.Presentations
                                    group s by s.NombrePresentacion into presenta
                                    select presenta).ToList();
                
                foreach (var item in Presentacion)
                {
                    EDPresentacion EDPresentacion = new EDPresentacion();
                    //EDPresentacion.PresentationId = item.PresentationId;
                    EDPresentacion.NombrePresentacion = item.Key;
                    //EDPresentacion.CantPresentacion = item.CantPresentacion;
                    ListaEDPresentacion.Add(EDPresentacion);
                }
            }
            ListaEDPresentacion = ListaEDPresentacion.OrderBy(o => o.NombrePresentacion).ToList();
            return ListaEDPresentacion;
        }
        public List<EDPresentacion> ListaPresentacion(string NombrePresentacion)
        {
            List<EDPresentacion> ListaEDPresentacion = new List<EDPresentacion>();
            List<Presentation> ListaPresentation = new List<Presentation>();
            using (DrogSystemContext db = new DrogSystemContext())
            {
                var Presentation = (from PD in db.Presentations
                                    where PD.NombrePresentacion == NombrePresentacion
                                    select PD).ToList();
                if (Presentation != null)
                {
                    ListaPresentation = Presentation;
                }
            }

           foreach (var item in ListaPresentation)
           {
                EDPresentacion EDPresentacion = new EDPresentacion();
                EDPresentacion.PresentationId = item.PresentationId;
                EDPresentacion.NombrePresentacion = item.NombrePresentacion;
                EDPresentacion.CantPresentacion = item.CantPresentacion;
                ListaEDPresentacion.Add(EDPresentacion);
            }
            ListaEDPresentacion = ListaEDPresentacion.OrderBy(o => o.CantPresentacion).ToList();
            return ListaEDPresentacion;

        }
    }
}