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
    public class ProductPresentationPricesController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: ProductPresentationPrices
        public ActionResult Index()
        {
            var productPresentationPrices = db.ProductPresentationPrices.Include(p => p.Presentation);
            return View(productPresentationPrices.ToList());
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
