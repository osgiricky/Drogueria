using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DrogSystem.Models;

namespace DrogSystem.Controllers
{
    public class SaleDetailsController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: SaleDetails
        public ActionResult Index()
        {
            var saleDetails = db.SaleDetails.Include(s => s.Presentation).Include(s => s.ProductDetail).Include(s => s.Sale);
            return View(saleDetails.ToList());
        }

        // GET: SaleDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleDetail saleDetail = db.SaleDetails.Find(id);
            if (saleDetail == null)
            {
                return HttpNotFound();
            }
            return View(saleDetail);
        }

        // GET: SaleDetails/Create
        public ActionResult Create()
        {
            ViewBag.PresentationId = new SelectList(db.Presentations, "PresentationId", "NombrePresentacion");
            ViewBag.ProductDetailId = new SelectList(db.ProductDetails, "ProductDetailId", "CodBarras");
            ViewBag.SaleId = new SelectList(db.Sales, "SaleId", "SaleId");
            return View();
        }

        // POST: SaleDetails/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SaleDetailId,SaleId,ProductDetailId,PresentationId,Quantity,PrecioTotal")] SaleDetail saleDetail)
        {
            if (ModelState.IsValid)
            {
                db.SaleDetails.Add(saleDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PresentationId = new SelectList(db.Presentations, "PresentationId", "NombrePresentacion", saleDetail.PresentationId);
            ViewBag.ProductDetailId = new SelectList(db.ProductDetails, "ProductDetailId", "CodBarras", saleDetail.ProductDetailId);
            ViewBag.SaleId = new SelectList(db.Sales, "SaleId", "SaleId", saleDetail.SaleId);
            return View(saleDetail);
        }

        // GET: SaleDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleDetail saleDetail = db.SaleDetails.Find(id);
            if (saleDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.PresentationId = new SelectList(db.Presentations, "PresentationId", "NombrePresentacion", saleDetail.PresentationId);
            ViewBag.ProductDetailId = new SelectList(db.ProductDetails, "ProductDetailId", "CodBarras", saleDetail.ProductDetailId);
            ViewBag.SaleId = new SelectList(db.Sales, "SaleId", "SaleId", saleDetail.SaleId);
            return View(saleDetail);
        }

        // POST: SaleDetails/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SaleDetailId,SaleId,ProductDetailId,PresentationId,Quantity,PrecioTotal")] SaleDetail saleDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saleDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PresentationId = new SelectList(db.Presentations, "PresentationId", "NombrePresentacion", saleDetail.PresentationId);
            ViewBag.ProductDetailId = new SelectList(db.ProductDetails, "ProductDetailId", "CodBarras", saleDetail.ProductDetailId);
            ViewBag.SaleId = new SelectList(db.Sales, "SaleId", "SaleId", saleDetail.SaleId);
            return View(saleDetail);
        }

        // GET: SaleDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleDetail saleDetail = db.SaleDetails.Find(id);
            if (saleDetail == null)
            {
                return HttpNotFound();
            }
            return View(saleDetail);
        }

        // POST: SaleDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SaleDetail saleDetail = db.SaleDetails.Find(id);
            db.SaleDetails.Remove(saleDetail);
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
