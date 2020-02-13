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
    public class EntryDetailsController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: EntryDetails
        public ActionResult Index()
        {
            var entryDetails = db.EntryDetails.Include(e => e.Entry).Include(e => e.ProductDetails);
            return View(entryDetails.ToList());
        }

        // GET: EntryDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntryDetail entryDetail = db.EntryDetails.Find(id);
            if (entryDetail == null)
            {
                return HttpNotFound();
            }
            return View(entryDetail);
        }

        // GET: EntryDetails/Create
        public ActionResult Create()
        {
            ViewBag.EntradaId = new SelectList(db.Entries, "EntradaId", "Aprobado");
            ViewBag.ProductDetailId = new SelectList(db.ProductDetails, "ProductDetailId", "CodBarras");
            return View();
        }

        // POST: EntryDetails/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EntryDetailId,Cantidad,Lote,FechaVence,EntradaId,ProductDetailId")] EntryDetail entryDetail)
        {
            if (ModelState.IsValid)
            {
                db.EntryDetails.Add(entryDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EntradaId = new SelectList(db.Entries, "EntradaId", "Aprobado", entryDetail.EntradaId);
            ViewBag.ProductDetailId = new SelectList(db.ProductDetails, "ProductDetailId", "CodBarras", entryDetail.ProductDetailId);
            return View(entryDetail);
        }

        // GET: EntryDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntryDetail entryDetail = db.EntryDetails.Find(id);
            if (entryDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.EntradaId = new SelectList(db.Entries, "EntradaId", "Aprobado", entryDetail.EntradaId);
            ViewBag.ProductDetailId = new SelectList(db.ProductDetails, "ProductDetailId", "CodBarras", entryDetail.ProductDetailId);
            return View(entryDetail);
        }

        // POST: EntryDetails/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EntryDetailId,Cantidad,Lote,FechaVence,EntradaId,ProductDetailId")] EntryDetail entryDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entryDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EntradaId = new SelectList(db.Entries, "EntradaId", "Aprobado", entryDetail.EntradaId);
            ViewBag.ProductDetailId = new SelectList(db.ProductDetails, "ProductDetailId", "CodBarras", entryDetail.ProductDetailId);
            return View(entryDetail);
        }

        // GET: EntryDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntryDetail entryDetail = db.EntryDetails.Find(id);
            if (entryDetail == null)
            {
                return HttpNotFound();
            }
            return View(entryDetail);
        }

        // POST: EntryDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EntryDetail entryDetail = db.EntryDetails.Find(id);
            db.EntryDetails.Remove(entryDetail);
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
