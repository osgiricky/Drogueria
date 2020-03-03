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
    public class SalesController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: Sales
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
                           select E).ToList();

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

        public JsonResult Crear(List<EDSaleDetail> DetalleFactura, EDSale Factura)
        {
            bool Probar = true;
            string Mensaje = "";
            EDSale EDSale = new EDSale();
            EDSale.FechaFactura = Factura.FechaFactura;
            EDSale.NroFactura = Factura.NroFactura;
            EDSale.ValorFactura = Factura.ValorFactura;

            List<EDSaleDetail> ListaEDSaleDetail = new List<EDSaleDetail>();
            foreach (var item in DetalleFactura)
            {
                EDSaleDetail EDSaleDetail = new EDSaleDetail();
                EDSaleDetail.ProductDetailId = item.ProductDetailId;
                EDSaleDetail.Quantity = item.Quantity;
                EDSaleDetail.PrecioTotal = item.PrecioTotal;
                EDSaleDetail.PresentationId = item.PresentationId;
                ListaEDSaleDetail.Add(EDSaleDetail);
            }
            try
            {
                Sale Sale = new Sale();
                Sale.FechaFactura = DateTime.Parse(EDSale.FechaFactura);
                Sale.NroFactura = EDSale.NroFactura;
                Sale.ValorFactura = EDSale.ValorFactura;
                db.Sales.Add(Sale);
                db.SaveChanges();
                int IdSale = Sale.SaleId;

                foreach (var item1 in ListaEDSaleDetail)
                {
                    SaleDetail SaleDetail = new SaleDetail();
                    SaleDetail.SaleId = IdSale;
                    SaleDetail.ProductDetailId = item1.ProductDetailId;
                    SaleDetail.PresentationId = item1.PresentationId;
                    SaleDetail.Quantity = item1.Quantity;
                    SaleDetail.PrecioTotal = item1.PrecioTotal;

                    db.SaleDetails.Add(SaleDetail);
                    db.SaveChanges();

                    Presentation Presentation = db.Presentations.Find(SaleDetail.PresentationId);
                    ProductDetail ProductDetail = db.ProductDetails.Find(SaleDetail.ProductDetailId);
                    ProductDetail.Existencias -= SaleDetail.Quantity * Presentation.CantPresentacion;
                    db.Entry(ProductDetail).State = EntityState.Modified;
                    db.SaveChanges();

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

            EDProductPresentationPrice EDProductPresentationPrice = new EDProductPresentationPrice();
            if (ProductoDetalle != null)
            {
                foreach (var item in ProductoDetalle)
                {
                    EDProductPresentationPrice.ProductDetailId = item.PD.ProductDetailId;
                    EDProductPresentationPrice.CodBarras = item.PD.CodBarras;
                    EDProductPresentationPrice.ProductoId = item.PD.ProductoId;
                    EDProductPresentationPrice.NombreProducto = item.P.NombreProducto;
                    EDProductPresentationPrice.MarkerId = item.PD.MarkerId;
                    EDProductPresentationPrice.NombreFabricante = item.M.NombreFabricante;
                    FuncUsuarios FuncUsuarios = new FuncUsuarios();
                    EDProductPresentationPrice.ListaPresentacion = FuncUsuarios.ListaProductPresentacion(EDProductPresentationPrice.ProductDetailId);
                    EDProductPresentationPrice.Precio = FuncUsuarios.PrecioProducto(item.PD.ProductDetailId,
                        EDProductPresentationPrice.ListaPresentacion[0].PresentationId);
                }
            }
            return Json(EDProductPresentationPrice, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListaPresentacion(int ProductDetailId)
        {
            List<EDPresentacion> ListaPresentacion = new List<EDPresentacion>();
            FuncUsuarios FuncUsuarios = new FuncUsuarios();
            ListaPresentacion = FuncUsuarios.ListaProductPresentacion(ProductDetailId);
            return Json(ListaPresentacion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarPrecio(int ProductDetailId, int IdPresentacion)
        {
            FuncUsuarios FuncUsuarios = new FuncUsuarios();
            var Precio = FuncUsuarios.PrecioProducto(ProductDetailId, IdPresentacion);

            return Json(Precio, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NroFactura()
        {
            var NroFact = (from PD in db.Sales
                           select PD).ToList();
            int FacturaNum = 0;
            if (NroFact.Count != 0)
            {
                FacturaNum = NroFact.Max(o => o.NroFactura) + 1;
            }

            return Json(FacturaNum, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarExitencias(int ProductDetailId, int PresentacionID, int Cantidad)
        {
            bool HayExistencias = true;
            var CantPResentacion = (from P in db.Presentations
                                    where P.PresentationId == PresentacionID
                                    select P.CantPresentacion).FirstOrDefault();
            var CantNecesaria = Cantidad * CantPResentacion;

            var CantExistente = (from R in db.ProductDetails
                                 where R.ProductDetailId == ProductDetailId
                                 select R.Existencias).FirstOrDefault();
            if (CantExistente < CantNecesaria)
                HayExistencias = false;

            return Json(new { HayExistencias, CantExistente }, JsonRequestBehavior.AllowGet);
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

        public JsonResult BuscarXId(int ProductDetailId)
        {
            var ProductoDetalle = (from PD in db.ProductDetails
                                   join P in db.Products on PD.ProductoId equals P.ProductoId
                                   join M in db.Markers on PD.MarkerId equals M.MarkerId
                                   where PD.ProductDetailId == ProductDetailId
                                   select new { PD, P, M }).ToList();

            EDProductPresentationPrice EDProductPresentationPrice = new EDProductPresentationPrice();
            if (ProductoDetalle != null)
            {
                foreach (var item in ProductoDetalle)
                {
                    EDProductPresentationPrice.ProductDetailId = item.PD.ProductDetailId;
                    EDProductPresentationPrice.CodBarras = item.PD.CodBarras;
                    EDProductPresentationPrice.ProductoId = item.PD.ProductoId;
                    EDProductPresentationPrice.NombreProducto = item.P.NombreProducto;
                    EDProductPresentationPrice.MarkerId = item.PD.MarkerId;
                    EDProductPresentationPrice.NombreFabricante = item.M.NombreFabricante;
                    FuncUsuarios FuncUsuarios = new FuncUsuarios();
                    EDProductPresentationPrice.ListaPresentacion = FuncUsuarios.ListaProductPresentacion(EDProductPresentationPrice.ProductDetailId);
                    EDProductPresentationPrice.Precio = FuncUsuarios.PrecioProducto(item.PD.ProductDetailId,
                        EDProductPresentationPrice.ListaPresentacion[0].PresentationId);
                }
            }
            return Json(EDProductPresentationPrice, JsonRequestBehavior.AllowGet);
        }

        // GET: Sales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SaleId,NroFactura,FechaFactura,ValorFactura")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Sales.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sale);
        }

        // GET: Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Sales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SaleId,NroFactura,FechaFactura,ValorFactura")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sale);
        }

        // GET: Sales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sale sale = db.Sales.Find(id);
            db.Sales.Remove(sale);
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
