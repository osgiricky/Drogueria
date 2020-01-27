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
                    EDProductPresentationPrice.DetailProductId = item.PD.ProductDetailId;
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
                                  where PPP.PrecioProductoId == ID
                                  select new { PPP, PD, P, M }).ToList();

            EDProductPresentationPrice EDProductPresentationPrice = new EDProductPresentationPrice();
            if (PrecioProducto != null)
            {
                foreach (var item in PrecioProducto)
                {
                    FuncUsuarios FuncUsuarios = new FuncUsuarios();
                    List<EDPresentacion> ListaEDPresentacion = new List<EDPresentacion>();
                    ListaEDPresentacion = FuncUsuarios.ListaPresentacion();
                    EDProductPresentationPrice.PrecioProductoId = item.PPP.PrecioProductoId;
                    EDProductPresentationPrice.Precio = item.PPP.Precio;
                    EDProductPresentationPrice.PresentationId = item.PPP.PresentationId;
                    EDProductPresentationPrice.MarkerId = item.PD.MarkerId;
                    EDProductPresentationPrice.NombreFabricante = item.M.NombreFabricante;
                    EDProductPresentationPrice.DetailProductId = item.PD.ProductDetailId;
                    EDProductPresentationPrice.CodBarras = item.PD.CodBarras;
                    EDProductPresentationPrice.ProductoId = item.PD.ProductoId;
                    EDProductPresentationPrice.NombreProducto = item.P.NombreProducto;
                    EDPresentacion EDPresentacion = ListaEDPresentacion.Find(u => u.PresentationId == EDProductPresentationPrice.PresentationId);
                    EDProductPresentationPrice.NombrePresentacion = EDPresentacion.NombrePresentacion;
                    EDProductPresentationPrice.CantPresentacion = EDPresentacion.CantPresentacion;
                    EDProductPresentationPrice.ListaPresentacion = ListaEDPresentacion;
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
            EDProductPresentationPrice.CodBarras = ProductoDetalle.CodBarras;
            EDProductPresentationPrice.RegInvima = ProductoDetalle.RegInvima;
            EDProductPresentationPrice.Existencias = ProductoDetalle.Existencias;
            EDProductPresentationPrice.ProductoId = ProductoDetalle.ProductoId;
            EDProductPresentationPrice.MarkerId = ProductoDetalle.MarkerId;

            ProductDetail ProductDetail = db.ProductDetails.Find(ProductoDetalle.PrecioProductoId);
            if (ProductDetail == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + EDProductPresentationPrice.CodBarras;
            }
            else
            {
                try
                {
                    ProductDetail.CodBarras = EDProductPresentationPrice.CodBarras;
                    ProductDetail.RegInvima = EDProductPresentationPrice.RegInvima;
                    ProductDetail.Existencias = EDProductPresentationPrice.Existencias;
                    ProductDetail.MarkerId = EDProductPresentationPrice.MarkerId;
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

        public JsonResult Crear(EDProductPresentationPrice ProductoDetalle)
        {
            bool Probar = true;
            string Mensaje = "";
            EDProductPresentationPrice EDProductPresentationPrice = new EDProductPresentationPrice();
            EDProductPresentationPrice.CodBarras = ProductoDetalle.CodBarras;
            EDProductPresentationPrice.RegInvima = ProductoDetalle.RegInvima;
            EDProductPresentationPrice.Existencias = ProductoDetalle.Existencias;
            EDProductPresentationPrice.ProductoId = ProductoDetalle.ProductoId;
            EDProductPresentationPrice.MarkerId = ProductoDetalle.MarkerId;
            try
            {
                ProductDetail ProductDetail = new ProductDetail();
                ProductDetail.CodBarras = EDProductPresentationPrice.CodBarras;
                ProductDetail.RegInvima = EDProductPresentationPrice.RegInvima;
                ProductDetail.Existencias = EDProductPresentationPrice.Existencias;
                ProductDetail.ProductoId = EDProductPresentationPrice.ProductoId;
                ProductDetail.MarkerId = EDProductPresentationPrice.MarkerId;
                db.ProductDetails.Add(ProductDetail);
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

        public JsonResult BuscarXNombre(EDProduct producto)
        {
            var Productos = (from PD in db.Products
                             where PD.NombreProducto.Contains(producto.NombreProducto)
                             select new { PD }).ToList();

            EDProductPresentationPrice EDProductPresentationPrice = new EDProductPresentationPrice();
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
                EDProductPresentationPrice.ListaProductos = ListaEDProduct;
            }
            return Json(EDProductPresentationPrice, JsonRequestBehavior.AllowGet);
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
