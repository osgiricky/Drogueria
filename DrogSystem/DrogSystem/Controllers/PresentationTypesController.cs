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
    public class PresentationTypesController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: PresentationTypes
        public ActionResult Index()
        {
            return View(db.PresentationTypes.ToList());
        }

        // GET: PresentationTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PresentationType presentationType = db.PresentationTypes.Find(id);
            if (presentationType == null)
            {
                return HttpNotFound();
            }
            return View(presentationType);
        }

        // GET: PresentationTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PresentationTypes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PresentationTypeId,NombrePresentacion")] PresentationType presentationType)
        {
            if (ModelState.IsValid)
            {
                db.PresentationTypes.Add(presentationType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(presentationType);
        }

        // GET: PresentationTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PresentationType presentationType = db.PresentationTypes.Find(id);
            if (presentationType == null)
            {
                return HttpNotFound();
            }
            return View(presentationType);
        }

        // POST: PresentationTypes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PresentationTypeId,NombrePresentacion")] PresentationType presentationType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(presentationType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(presentationType);
        }

        // GET: PresentationTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PresentationType presentationType = db.PresentationTypes.Find(id);
            if (presentationType == null)
            {
                return HttpNotFound();
            }
            return View(presentationType);
        }

        // POST: PresentationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PresentationType presentationType = db.PresentationTypes.Find(id);
            db.PresentationTypes.Remove(presentationType);
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
