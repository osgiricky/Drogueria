using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DrogSystem.Models;
using DrogSystem.EntidadesDominio;
using DrogSystem.Funciones;

namespace DrogSystem.Controllers
{
    public class EntriesController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: Entries
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            List<EDEntry> ListaEDEntry = new List<EDEntry>();
            var Listaux = (from E in db.Entries
                           join T in db.Providers on E.TerceroId equals T.TerceroId
                           where E.Aprobado == "N"
                           select new { E, T }).ToList();
            if (Listaux != null)
            {
                foreach (var item in Listaux)
                {
                    EDEntry EDEntry = new EDEntry();
                    EDEntry.EntryId = item.E.EntradaId;
                    EDEntry.FechaIngreso = item.E.FechaIngreso.ToString("dd/MM/yyyy");
                    EDEntry.TerceroId = item.E.TerceroId;
                    EDEntry.NombreTercero = item.T.NombreTercero;
                    ListaEDEntry.Add(EDEntry);
                }
                ListaEDEntry = ListaEDEntry.OrderBy(o => o.FechaIngreso).ToList();
            }
            return Json(ListaEDEntry, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int? ID)
        {
            var Entrada = (from E in db.Entries
                                   where E.EntradaId == ID
                                   select  E ).ToList();

            EDEntry EDEntry = new EDEntry();
            if (Entrada != null)
            {
                foreach (var item in Entrada)
                {
                    FuncUsuarios FuncUsuarios = new FuncUsuarios();
                    List<EDProvider> ListaProveedor = new List<EDProvider>();
                    List<EDEntryDetails> ListaDetalle = new List<EDEntryDetails>();
                    ListaProveedor = FuncUsuarios.ListaProveedores();
                    EDEntry.EntryId = item.EntradaId;
                    EDEntry.FechaIngreso = item.FechaIngreso.ToString("dd/MM/yyyy");
                    EDEntry.Aprobado = item.Aprobado;
                    EDEntry.TerceroId = item.TerceroId;
                    EDEntry.ListaTerceros = ListaProveedor;
                    ListaDetalle = FuncUsuarios.ListaDetalleEntrada(item.EntradaId);
                    EDEntry.ListaEntradas = ListaDetalle;
                }
            }
            return Json(EDEntry, JsonRequestBehavior.AllowGet);
        }

       public JsonResult Borrar(int ID)
        {
            bool Probar = true;
            string Mensaje = "";

            Entry Entradas = db.Entries.Find(ID);
            if (Entradas == null)
            {
                Probar = false;
                Mensaje = " No se encuentra el registro: ";
            }
            else
            {
                try
                {
                    var IdABorrar = (from E in db.EntryDetails
                                     where E.EntradaId == ID
                                     select E).ToList();

                    if (IdABorrar != null)
                    {
                        foreach (var detalle in IdABorrar)
                        {
                            EntryDetail EntradaDetalle = db.EntryDetails.Find(detalle.EntryDetailId);
                            db.EntryDetails.Remove(EntradaDetalle);
                            db.SaveChanges();
                        }
                    }
                    db.Entries.Remove(Entradas);
                    db.SaveChanges();
                    Mensaje = " Registro eliminado con exito.";
                }
                catch (Exception)
                {
                    Probar = false;
                    Mensaje = " Se produjo un error al borrar el registro.";

                }
            }

            return Json(new { Probar, Mensaje }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Crear(List<EDEntryDetails> DetalleEntrada, EDEntry Entradas, List<int> IdABorrar)
        {
            bool Probar = true;
            string Mensaje = "";
            EDEntry EDEntry = new EDEntry();
            EDEntry.EntryId = Entradas.EntryId;
            EDEntry.FechaIngreso = Entradas.FechaIngreso;
            EDEntry.Aprobado = Entradas.Aprobado;
            EDEntry.TerceroId = Entradas.TerceroId;

            List<EDEntryDetails> EDEntryDetails = new List<EDEntryDetails>();
            foreach (var item in DetalleEntrada)
            {
                EDEntryDetails EDEntryDetail = new EDEntryDetails();
                EDEntryDetail.EntryDetailId = item.EntryDetailId;
                EDEntryDetail.Cantidad = item.Cantidad;
                EDEntryDetail.Lote = item.Lote;
                EDEntryDetail.FechaVence = item.FechaVence;
                EDEntryDetail.ProductDetailId = item.ProductDetailId;
                EDEntryDetails.Add(EDEntryDetail);
            }
            try
            {
                Entry Entry = new Entry();
                if (EDEntry.EntryId > 0)
                {
                    Entry entrada = db.Entries.Find(EDEntry.EntryId);
                    Entry = entrada;
                }
                Entry.FechaIngreso = DateTime.Parse(EDEntry.FechaIngreso);
                Entry.TerceroId = EDEntry.TerceroId;
                Entry.Aprobado = EDEntry.Aprobado = Entradas.Aprobado;
                if (EDEntry.EntryId > 0)
                {
                    db.Entry(Entry).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    db.Entries.Add(Entry);
                    db.SaveChanges();
                }
                int IdEntrada = Entry.EntradaId;
                if (IdABorrar != null)
                {
                    foreach (var detalle in IdABorrar)
                    {
                        EntryDetail EntradaDetalle = db.EntryDetails.Find(detalle);
                        db.EntryDetails.Remove(EntradaDetalle);
                        db.SaveChanges();
                    }
                }
                
               foreach (var item1 in EDEntryDetails)
                {
                    EntryDetail EntryDetail = new EntryDetail();
                    if (item1.EntryDetailId > 0)
                    {
                        EntryDetail entradaDetalle = db.EntryDetails.Find(item1.EntryDetailId);
                        EntryDetail = entradaDetalle;
                    }
                    EntryDetail.Cantidad = item1.Cantidad;
                    EntryDetail.Lote = item1.Lote;
                    EntryDetail.FechaVence = DateTime.Parse(item1.FechaVence);
                    EntryDetail.EntradaId = IdEntrada;
                    EntryDetail.ProductDetailId = item1.ProductDetailId;                    
                    if (EntryDetail.EntryDetailId > 0)
                    {
                        db.Entry(EntryDetail).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        db.EntryDetails.Add(EntryDetail);
                        db.SaveChanges();
                    }
                    if (Entry.Aprobado == "S")
                    {
                        ProductDetail ProductDetail = db.ProductDetails.Find(EntryDetail.ProductDetailId);
                        ProductDetail.Existencias += EntryDetail.Cantidad;
                        db.Entry(ProductDetail).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                Mensaje = " Registro Agregado con exito.";
            }
            catch (Exception)
            {
                Probar = false;
                Mensaje = " Se produjo un error al agregar el registro.";

            }            
            return Json(new { Probar, Mensaje }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult listaProveedores()
        {
            FuncUsuarios FuncUsuarios = new FuncUsuarios();
            List<EDProvider> ListaTerceros = new List<EDProvider>();
            ListaTerceros = FuncUsuarios.ListaProveedores();
            return Json(ListaTerceros, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult BuscarProducto(string CodBarras)
        {
            var ProductoDetalle = (from PD in db.ProductDetails
                                   join P in db.Products on PD.ProductoId equals P.ProductoId
                                   join M in db.Markers on PD.MarkerId equals M.MarkerId
                                   where PD.CodBarras == CodBarras
                                   select new { PD, P, M }).ToList();

            EDProductDetail EDProductDetail = new EDProductDetail();
            if (ProductoDetalle != null)
            {
                foreach (var item in ProductoDetalle)
                {
                    EDProductDetail.ProductDetailId = item.PD.ProductDetailId;
                    EDProductDetail.CodBarras = item.PD.CodBarras;
                    EDProductDetail.RegInvima = item.PD.RegInvima;
                    EDProductDetail.Existencias = item.PD.Existencias;
                    EDProductDetail.ProductoId = item.PD.ProductoId;
                    EDProductDetail.NombreProducto = item.P.NombreProducto;
                    EDProductDetail.MarkerId = item.PD.MarkerId;
                    EDProductDetail.NombreFabricante = item.M.NombreFabricante;
                }
            }
            return Json(EDProductDetail, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarXId(int ProductDetailId)
        {
            var ProductoDetalle = (from PD in db.ProductDetails
                                   join P in db.Products on PD.ProductoId equals P.ProductoId
                                   join M in db.Markers on PD.MarkerId equals M.MarkerId
                                   where PD.ProductDetailId == ProductDetailId
                                   select new { PD, P, M }).ToList();

            EDProductDetail EDProductDetail = new EDProductDetail();
            if (ProductoDetalle != null)
            {
                foreach (var item in ProductoDetalle)
                {
                    EDProductDetail.ProductDetailId = item.PD.ProductDetailId;
                    EDProductDetail.CodBarras = item.PD.CodBarras;
                    EDProductDetail.RegInvima = item.PD.RegInvima;
                    EDProductDetail.Existencias = item.PD.Existencias;
                    EDProductDetail.ProductoId = item.PD.ProductoId;
                    EDProductDetail.NombreProducto = item.P.NombreProducto;
                    EDProductDetail.MarkerId = item.PD.MarkerId;
                    EDProductDetail.NombreFabricante = item.M.NombreFabricante;
                }
            }
            return Json(EDProductDetail, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarXNombre(string NombreProducto)
        {
            var ProductoDetalle = (from PD in db.ProductDetails
                                   join P in db.Products on PD.ProductoId equals P.ProductoId
                                   join M in db.Markers on PD.MarkerId equals M.MarkerId
                                   where P.NombreProducto.Contains(NombreProducto)
                                   select new { PD, P, M }).ToList();

            List<EDProductPresentationPrice> ListaEDProductPresentationPrice = new List<EDProductPresentationPrice>();
            if (ProductoDetalle != null)
            {
                foreach (var item in ProductoDetalle)
                {
                    EDProductPresentationPrice EDProductPresentationPrice = new EDProductPresentationPrice();
                    EDProductPresentationPrice.ProductDetailId = item.PD.ProductDetailId;
                    EDProductPresentationPrice.CodBarras = item.PD.CodBarras;
                    EDProductPresentationPrice.ProductoId = item.PD.ProductoId;
                    EDProductPresentationPrice.NombreProducto = item.P.NombreProducto;
                    EDProductPresentationPrice.MarkerId = item.PD.MarkerId;
                    EDProductPresentationPrice.NombreFabricante = item.M.NombreFabricante;
                    ListaEDProductPresentationPrice.Add(EDProductPresentationPrice);
                }
            }
            return Json(ListaEDProductPresentationPrice, JsonRequestBehavior.AllowGet);
        }

        // GET: Entries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entry entry = db.Entries.Find(id);
            if (entry == null)
            {
                return HttpNotFound();
            }
            return View(entry);
        }

        // GET: Entries/Create
        public ActionResult Create()
        {
            ViewBag.TerceroId = new SelectList(db.Providers, "TerceroId", "NombreTercero");
            return View();
        }

        // POST: Entries/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EntradaId,FechaIngreso,Aprobado,TerceroId")] Entry entry)
        {
            if (ModelState.IsValid)
            {
                db.Entries.Add(entry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TerceroId = new SelectList(db.Providers, "TerceroId", "NombreTercero", entry.TerceroId);
            return View(entry);
        }

        // GET: Entries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entry entry = db.Entries.Find(id);
            if (entry == null)
            {
                return HttpNotFound();
            }
            ViewBag.TerceroId = new SelectList(db.Providers, "TerceroId", "NombreTercero", entry.TerceroId);
            return View(entry);
        }

        // POST: Entries/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EntradaId,FechaIngreso,Aprobado,TerceroId")] Entry entry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TerceroId = new SelectList(db.Providers, "TerceroId", "NombreTercero", entry.TerceroId);
            return View(entry);
        }

        // GET: Entries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entry entry = db.Entries.Find(id);
            if (entry == null)
            {
                return HttpNotFound();
            }
            return View(entry);
        }

        // POST: Entries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entry entry = db.Entries.Find(id);
            db.Entries.Remove(entry);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class list<T>
    {
    }
}
