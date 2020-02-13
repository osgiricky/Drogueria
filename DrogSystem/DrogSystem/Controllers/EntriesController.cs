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
            List<EDProductDetail> EDProductDetailLista = new List<EDProductDetail>();
            var Listaux = (from PD in db.ProductDetails
                           join P in db.Products on PD.ProductoId equals P.ProductoId
                           join M in db.Markers on PD.MarkerId equals M.MarkerId
                           select new { PD, P, M }).ToList();
            if (Listaux != null)
            {
                foreach (var item in Listaux)
                {
                    EDProductDetail EDProductDetail = new EDProductDetail();
                    EDProductDetail.ProductDetailId = item.PD.ProductDetailId;
                    EDProductDetail.CodBarras = item.PD.CodBarras;
                    EDProductDetail.RegInvima = item.PD.RegInvima;
                    EDProductDetail.Existencias = item.PD.Existencias;
                    EDProductDetail.ProductoId = item.PD.ProductoId;
                    EDProductDetail.NombreProducto = item.P.NombreProducto;
                    EDProductDetail.MarkerId = item.M.MarkerId;
                    EDProductDetail.NombreFabricante = item.M.NombreFabricante;
                    EDProductDetailLista.Add(EDProductDetail);
                }
            }
            return Json(EDProductDetailLista, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int? ID)
        {
            var ProductoDetalle = (from PD in db.ProductDetails
                                   join P in db.Products on PD.ProductoId equals P.ProductoId
                                   where PD.ProductDetailId == ID
                                   select new { PD, P }).ToList();

            EDProductDetail EDProductDetail = new EDProductDetail();
            if (ProductoDetalle != null)
            {
                foreach (var item in ProductoDetalle)
                {
                    FuncUsuarios FuncUsuarios = new FuncUsuarios();
                    List<EDMarker> ListaEDMarker = new List<EDMarker>();
                    ListaEDMarker = FuncUsuarios.ListaFabricante();
                    EDProductDetail.ProductDetailId = item.PD.ProductDetailId;
                    EDProductDetail.CodBarras = item.PD.CodBarras;
                    EDProductDetail.RegInvima = item.PD.RegInvima;
                    EDProductDetail.Existencias = item.PD.Existencias;
                    EDProductDetail.ProductoId = item.PD.ProductoId;
                    EDProductDetail.NombreProducto = item.P.NombreProducto;
                    EDProductDetail.MarkerId = item.PD.MarkerId;
                    EDMarker EDMarker = ListaEDMarker.Find(u => u.MarkerId == EDProductDetail.MarkerId);
                    EDProductDetail.NombreFabricante = EDMarker.NombreFabricante;
                    EDProductDetail.ListaFabricantes = ListaEDMarker;
                }
            }
            return Json(EDProductDetail, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Borrar(int ID)
        {
            bool Probar = true;
            string Mensaje = "";
            ProductDetail ProductDetail = db.ProductDetails.Find(ID);
            if (ProductDetail == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + ProductDetail.ProductDetailId;
            }
            else
            {
                try
                {
                    db.ProductDetails.Remove(ProductDetail);
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

        public JsonResult Editar(EDProductDetail ProductoDetalle)
        {
            bool Probar = true;
            string Mensaje = "";
            EDProductDetail EDProductDetail = new EDProductDetail();
            EDProductDetail.CodBarras = ProductoDetalle.CodBarras;
            EDProductDetail.RegInvima = ProductoDetalle.RegInvima;
            EDProductDetail.Existencias = ProductoDetalle.Existencias;
            EDProductDetail.ProductoId = ProductoDetalle.ProductoId;
            EDProductDetail.MarkerId = ProductoDetalle.MarkerId;

            ProductDetail ProductDetail = db.ProductDetails.Find(ProductoDetalle.ProductDetailId);
            if (ProductDetail == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + EDProductDetail.CodBarras;
            }
            else
            {
                try
                {
                    ProductDetail.CodBarras = EDProductDetail.CodBarras;
                    ProductDetail.RegInvima = EDProductDetail.RegInvima;
                    ProductDetail.Existencias = EDProductDetail.Existencias;
                    ProductDetail.MarkerId = EDProductDetail.MarkerId;
                    db.Entry(ProductDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    Mensaje = " Registro modificado con exito.";
                }
                catch (Exception e)
                {
                    Probar = false;
                    Mensaje = " Se produjo un error al modificar el registro.";

                }
            }

            return Json(new { Probar, Mensaje }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Crear(List<EDEntryDetails> DetalleEntrada, EDEntry Entradas)
        {
            bool Probar = true;
            string Mensaje = "";
            EDEntry EDEntry = new EDEntry();
            EDEntry.FechaIngreso = Entradas.FechaIngreso;
            EDEntry.Aprobado = Entradas.Aprobado;
            EDEntry.TerceroId = Entradas.TerceroId;
            try
            {
                Entry Entry = new Entry();
                Entry.FechaIngreso = DateTime.Parse(EDEntry.FechaIngreso);
                Entry.TerceroId = EDEntry.TerceroId;
                Entry.Aprobado = EDEntry.Aprobado = Entradas.Aprobado;
                db.Entries.Add(Entry);
                db.SaveChanges();
                int IdEntrada = Entry.EntradaId;
                Mensaje = " Registro Agregado con exito.";
            }
            catch (Exception)
            {
                Probar = false;
                Mensaje = " Se produjo un error al agregar el registro.";

            }


            return Json(new { Probar, Mensaje }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult listaFabricantes(int? ID)
        {
            FuncUsuarios FuncUsuarios = new FuncUsuarios();
            List<EDMarker> ListaEDMarker = new List<EDMarker>();
            ListaEDMarker = FuncUsuarios.ListaFabricante();
            Product Product = db.Products.Find(ID);
            EDProduct EDProduct = new EDProduct();
            if (Product != null)
            {
                EDProduct.ProductoId = Product.ProductoId;
                EDProduct.NombreProducto = Product.NombreProducto;
                EDProduct.MinStock = Product.MinStock;
                EDProduct.Descripcion = Product.Descripcion;
                EDProduct.Componentes = Product.Componentes;
            }
            return Json(new { ListaEDMarker, EDProduct }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult listaProveedores()
        {
            FuncUsuarios FuncUsuarios = new FuncUsuarios();
            List<EDProvider> ListaTerceros = new List<EDProvider>();
            ListaTerceros = FuncUsuarios.ListaProveedores();
            return Json(ListaTerceros, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarXNombre(EDProduct producto)
        {
            var Productos = (from PD in db.Products
                             where PD.NombreProducto.Contains(producto.NombreProducto)
                             select new { PD }).ToList();

            EDProductDetail EDProductDetail = new EDProductDetail();
            if (Productos != null)
            {
                List<EDProduct> ListaEDProduct = new List<EDProduct>();
                foreach (var item in Productos)
                {
                    EDProduct EDProduct = new EDProduct();
                    EDProduct.ProductoId = item.PD.ProductoId;
                    EDProduct.NombreProducto = item.PD.NombreProducto;
                    EDProduct.Descripcion = item.PD.Descripcion;
                    ListaEDProduct.Add(EDProduct);
                }
                EDProductDetail.ListaProductos = ListaEDProduct;
            }
            return Json(EDProductDetail, JsonRequestBehavior.AllowGet);
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
