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
    public class PresentationsController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: Presentations
        public ActionResult Index()
        {
            var presentations = db.Presentations.Include(p => p.PresentationType);
            return View(presentations.ToList());
        }

        // GET: Presentations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presentation presentation = db.Presentations.Find(id);
            if (presentation == null)
            {
                return HttpNotFound();
            }
            return View(presentation);
        }

        // GET: Presentations/Create
        public ActionResult Create()
        {
            ViewBag.PresentationTypeId = new SelectList(db.PresentationTypes, "PresentationTypeId", "NombrePresentacion");
            return View();
        }

        // POST: Presentations/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PresentationId,CantPresentacion,PresentationTypeId")] Presentation presentation)
        {
            if (ModelState.IsValid)
            {
                db.Presentations.Add(presentation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PresentationTypeId = new SelectList(db.PresentationTypes, "PresentationTypeId", "NombrePresentacion", presentation.PresentationTypeId);
            return View(presentation);
        }

        // GET: Presentations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presentation presentation = db.Presentations.Find(id);
            if (presentation == null)
            {
                return HttpNotFound();
            }
            ViewBag.PresentationTypeId = new SelectList(db.PresentationTypes, "PresentationTypeId", "NombrePresentacion", presentation.PresentationTypeId);
            return View(presentation);
        }

        // POST: Presentations/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PresentationId,CantPresentacion,PresentationTypeId")] Presentation presentation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(presentation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PresentationTypeId = new SelectList(db.PresentationTypes, "PresentationTypeId", "NombrePresentacion", presentation.PresentationTypeId);
            return View(presentation);
        }

        // GET: Presentations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presentation presentation = db.Presentations.Find(id);
            if (presentation == null)
            {
                return HttpNotFound();
            }
            return View(presentation);
        }

        // POST: Presentations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Presentation presentation = db.Presentations.Find(id);
            db.Presentations.Remove(presentation);
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
