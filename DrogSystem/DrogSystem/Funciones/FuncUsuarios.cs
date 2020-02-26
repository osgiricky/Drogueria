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

        public List<EDEntryDetails> ListaDetalleEntrada(int IdEntrada)
        {
            List<EDEntryDetails> ListaEDDetalle = new List<EDEntryDetails>();
            using (DrogSystemContext db = new DrogSystemContext())
            {

                var Detalle = (from ED in db.EntryDetails
                               join PD in db.ProductDetails on ED.ProductDetailId equals PD.ProductDetailId
                               join P in db.Products on PD.ProductoId equals P.ProductoId
                               join M in db.Markers on PD.MarkerId equals M.MarkerId
                               where ED.EntradaId == IdEntrada
                               select new { ED, P.NombreProducto, M.NombreFabricante }).ToList();
                if (Detalle != null)
                {
                    foreach (var item in Detalle)
                    {
                        EDEntryDetails EDEntryDetails = new EDEntryDetails();
                        EDEntryDetails.EntryDetailId = item.ED.EntryDetailId;
                        EDEntryDetails.Cantidad = item.ED.Cantidad;
                        EDEntryDetails.Lote = item.ED.Lote;
                        EDEntryDetails.FechaVence = item.ED.FechaVence.ToString("dd/MM/yyyy");
                        EDEntryDetails.ProductDetailId = item.ED.ProductDetailId;
                        EDEntryDetails.NombreFabricante = item.NombreFabricante;
                        EDEntryDetails.NombreProducto = item.NombreProducto;
                        ListaEDDetalle.Add(EDEntryDetails);
                    }
                }
            }
            ListaEDDetalle = ListaEDDetalle.OrderBy(o => o.EntryDetailId).ToList();
            return ListaEDDetalle;
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
                    EDPresentacion.NombrePresentacion = item.Key;
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

        public List<EDPresentacion> ListaProductPresentacion(int IdProduct)
        {
            List<EDPresentacion> ListaEDPresentacion = new List<EDPresentacion>();
            List<Presentation> ListaPresentation = new List<Presentation>();
            using (DrogSystemContext db = new DrogSystemContext())
            {
                var Presentation = (from P in db.Presentations
                                    join R in db.ProductPresentationPrices on P.PresentationId equals R.PresentationId
                                    where R.ProductDetailId == IdProduct
                                    select P).ToList();
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
            return ListaEDPresentacion;
        }

        public decimal PrecioProducto(int ProductDetailId, int IdPresentacion)
        {
            using (DrogSystemContext db = new DrogSystemContext())
            {
                var precio = (from R in db.ProductPresentationPrices
                              where R.ProductDetailId == ProductDetailId && R.PresentationId == IdPresentacion
                              select R.Precio).FirstOrDefault();

                return precio;
            }
        }
    }
}