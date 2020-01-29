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
    public class ProductPresentationPricesController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: ProductPresentationPrices
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            List<EDProductPresentationPrice> EDProductPresentationPriceLista = new List<EDProductPresentationPrice>();
            var Listaux = (from PPP in db.ProductPresentationPrices
                           join PD in db.ProductDetails on PPP.ProductDetailId equals PD.ProductDetailId
                           join P in db.Products on PD.ProductoId equals P.ProductoId
                           join PR in db.Presentations on PPP.PresentationId equals PR.PresentationId
                           join M in db.Markers on PD.MarkerId equals M.MarkerId
                           select new { PPP, PD, P, PR, M }).ToList();
            if (Listaux != null)
            {
                foreach (var item in Listaux)
                {
                    EDProductPresentationPrice EDProductPresentationPrice = new EDProductPresentationPrice();
                    EDProductPresentationPrice.PrecioProductoId = item.PPP.PrecioProductoId;
                    EDProductPresentationPrice.Precio = item.PPP.Precio;
                    EDProductPresentationPrice.ProductoId = item.PD.ProductoId;
                    EDProductPresentationPrice.NombreProducto = item.P.NombreProducto;
                    EDProductPresentationPrice.PresentationId = item.PPP.PresentationId;
                    EDProductPresentationPrice.NombrePresentacion = item.PR.NombrePresentacion;
                    EDProductPresentationPrice.CantPresentacion = item.PR.CantPresentacion;
                    EDProductPresentationPrice.MarkerId = item.M.MarkerId;
                    EDProductPresentationPrice.ProductDetailId = item.PD.ProductDetailId;
                    EDProductPresentationPrice.CodBarras = item.PD.CodBarras;
                    EDProductPresentationPrice.NombreFabricante = item.M.NombreFabricante;
                    EDProductPresentationPriceLista.Add(EDProductPresentationPrice);
                }
            }
            return Json(EDProductPresentationPriceLista, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int? ID)
        {
            var PrecioProducto = (from PPP in db.ProductPresentationPrices
                                  join PD in db.ProductDetails on PPP.ProductDetailId equals PD.ProductDetailId
                                  join P in db.Products on PD.ProductoId equals P.ProductoId
                                  join M in db.Markers on PD.MarkerId equals M.MarkerId
                                  join PR in db.Presentations on PPP.PresentationId equals PR.PresentationId
                                  where PPP.PrecioProductoId == ID
                                  select new { PPP, PD, P, M, PR }).ToList();

            EDProductPresentationPrice EDProductPresentationPrice = new EDProductPresentationPrice();
            if (PrecioProducto != null)
            {
                foreach (var item in PrecioProducto)
                {
                    FuncUsuarios FuncUsuarios = new FuncUsuarios();
                    List<EDPresentacion> ListaEDPresentacion = new List<EDPresentacion>();
                    List<EDPresentacion> ListaEDPresentacionNombre = new List<EDPresentacion>();
                    ListaEDPresentacionNombre = FuncUsuarios.ListaNombrePresentacion();
                    EDProductPresentationPrice.PrecioProductoId = item.PPP.PrecioProductoId;
                    EDProductPresentationPrice.Precio = item.PPP.Precio;
                    EDProductPresentationPrice.PresentationId = item.PPP.PresentationId;
                    EDProductPresentationPrice.MarkerId = item.PD.MarkerId;
                    EDProductPresentationPrice.NombreFabricante = item.M.NombreFabricante;
                    EDProductPresentationPrice.ProductDetailId = item.PD.ProductDetailId;
                    EDProductPresentationPrice.CodBarras = item.PD.CodBarras;
                    EDProductPresentationPrice.ProductoId = item.PD.ProductoId;
                    EDProductPresentationPrice.NombreProducto = item.P.NombreProducto;
                    EDProductPresentationPrice.CantPresentacion = item.PR.CantPresentacion;
                    EDProductPresentationPrice.NombrePresentacion = item.PR.NombrePresentacion;
                    ListaEDPresentacion = FuncUsuarios.ListaPresentacion(EDProductPresentationPrice.NombrePresentacion);
                    EDProductPresentationPrice.ListaPresentacion = ListaEDPresentacion;
                    EDProductPresentationPrice.ListaNombrePresentacion = ListaEDPresentacionNombre;
                }
            }
            return Json(EDProductPresentationPrice, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Borrar(int ID)
        {
            bool Probar = true;
            string Mensaje = "";
            ProductPresentationPrice ProductPresentationPrice = db.ProductPresentationPrices.Find(ID);
            if (ProductPresentationPrice == null)
            {
                Probar = false;
                Mensaje = " No se encuentra el registro";
            }
            else
            {
                try
                {
                    db.ProductPresentationPrices.Remove(ProductPresentationPrice);
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

        public JsonResult Editar(EDProductPresentationPrice ProductoDetalle)
        {
            bool Probar = true;
            string Mensaje = "";
            EDProductPresentationPrice EDProductPresentationPrice = new EDProductPresentationPrice();
            EDProductPresentationPrice.PrecioProductoId = ProductoDetalle.PrecioProductoId;
            EDProductPresentationPrice.Precio = ProductoDetalle.Precio;
            EDProductPresentationPrice.PresentationId = ProductoDetalle.PresentationId;
            EDProductPresentationPrice.ProductDetailId = ProductoDetalle.ProductDetailId;

            ProductPresentationPrice ProductPresentationPrice = db.ProductPresentationPrices.Find(ProductoDetalle.PrecioProductoId);
            if (ProductPresentationPrice == null)
            {
                Probar = false;
                Mensaje = " No se encuentra el registro.";
            }
            else
            {
                try
                {
                    ProductPresentationPrice.Precio = EDProductPresentationPrice.Precio;
                    ProductPresentationPrice.PresentationId = EDProductPresentationPrice.PresentationId;
                    ProductPresentationPrice.ProductDetailId = EDProductPresentationPrice.ProductDetailId;
                    db.Entry(ProductPresentationPrice).State = EntityState.Modified;
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

        public JsonResult Crear(EDProductPresentationPrice ProductoDetalle)
        {
            bool Probar = true;
            string Mensaje = "";
            EDProductPresentationPrice EDProductPresentationPrice = new EDProductPresentationPrice();
            EDProductPresentationPrice.PrecioProductoId = ProductoDetalle.PrecioProductoId;
            EDProductPresentationPrice.Precio = ProductoDetalle.Precio;
            EDProductPresentationPrice.PresentationId = ProductoDetalle.PresentationId;
            EDProductPresentationPrice.ProductDetailId = ProductoDetalle.ProductDetailId;
            try
            {
                ProductPresentationPrice ProductPresentationPrice = new ProductPresentationPrice();
                ProductPresentationPrice.Precio = EDProductPresentationPrice.Precio;
                ProductPresentationPrice.PresentationId = EDProductPresentationPrice.PresentationId;
                ProductPresentationPrice.ProductDetailId = EDProductPresentationPrice.ProductDetailId;
                db.ProductPresentationPrices.Add(ProductPresentationPrice);
                db.SaveChanges();
                Mensaje = " Registro Agregado con exito.";
            }
            catch (Exception)
            {
                Probar = false;
                Mensaje = " Se produjo un error al agregar el registro.";

            }


            return Json(new { Probar, Mensaje }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult listaPresentacion()
        {
            FuncUsuarios FuncUsuarios = new FuncUsuarios();
            List<EDPresentacion> ListaEDPresentacion = new List<EDPresentacion>();
            ListaEDPresentacion = FuncUsuarios.ListaNombrePresentacion();
            return Json(ListaEDPresentacion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarXNombrePresentation(string presentacion)
        {
            FuncUsuarios FuncUsuarios = new FuncUsuarios();
            List<EDPresentacion> ListaEDPresentacion = new List<EDPresentacion>();
            ListaEDPresentacion = FuncUsuarios.ListaPresentacion(presentacion);
            return Json(ListaEDPresentacion, JsonRequestBehavior.AllowGet);
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


        // GET: ProductPresentationPrices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPresentationPrice productPresentationPrice = db.ProductPresentationPrices.Find(id);
            if (productPresentationPrice == null)
            {
                return HttpNotFound();
            }
            return View(productPresentationPrice);
        }

        // GET: ProductPresentationPrices/Create
        public ActionResult Create()
        {
            ViewBag.PresentationId = new SelectList(db.Presentations, "PresentationId", "PresentationId");
            return View();
        }

        // POST: ProductPresentationPrices/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrecioProductoId,Precio,PresentationId,DetailProductId")] ProductPresentationPrice productPresentationPrice)
        {
            if (ModelState.IsValid)
            {
                db.ProductPresentationPrices.Add(productPresentationPrice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PresentationId = new SelectList(db.Presentations, "PresentationId", "PresentationId", productPresentationPrice.PresentationId);
            return View(productPresentationPrice);
        }

        // GET: ProductPresentationPrices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPresentationPrice productPresentationPrice = db.ProductPresentationPrices.Find(id);
            if (productPresentationPrice == null)
            {
                return HttpNotFound();
            }
            ViewBag.PresentationId = new SelectList(db.Presentations, "PresentationId", "PresentationId", productPresentationPrice.PresentationId);
            return View(productPresentationPrice);
        }

        // POST: ProductPresentationPrices/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PrecioProductoId,Precio,PresentationId,DetailProductId")] ProductPresentationPrice productPresentationPrice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productPresentationPrice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PresentationId = new SelectList(db.Presentations, "PresentationId", "PresentationId", productPresentationPrice.PresentationId);
            return View(productPresentationPrice);
        }

        // GET: ProductPresentationPrices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPresentationPrice productPresentationPrice = db.ProductPresentationPrices.Find(id);
            if (productPresentationPrice == null)
            {
                return HttpNotFound();
            }
            return View(productPresentationPrice);
        }

        // POST: ProductPresentationPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductPresentationPrice productPresentationPrice = db.ProductPresentationPrices.Find(id);
            db.ProductPresentationPrices.Remove(productPresentationPrice);
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
}
