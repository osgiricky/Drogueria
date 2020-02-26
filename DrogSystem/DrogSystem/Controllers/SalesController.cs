﻿using System;
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