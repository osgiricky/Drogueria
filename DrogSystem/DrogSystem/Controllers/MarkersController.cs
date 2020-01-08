﻿using System;
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
    public class MarkersController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: Markers
        public ActionResult Index()
        {
            return View(db.Markers.ToList());
        }

        // GET: Markers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marker marker = db.Markers.Find(id);
            if (marker == null)
            {
                return HttpNotFound();
            }
            return View(marker);
        }

        // GET: Markers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Markers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MarkerId,NombreFabricante")] Marker marker)
        {
            if (ModelState.IsValid)
            {
                db.Markers.Add(marker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(marker);
        }

        // GET: Markers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marker marker = db.Markers.Find(id);
            if (marker == null)
            {
                return HttpNotFound();
            }
            return View(marker);
        }

        // POST: Markers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MarkerId,NombreFabricante")] Marker marker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(marker);
        }

        // GET: Markers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marker marker = db.Markers.Find(id);
            if (marker == null)
            {
                return HttpNotFound();
            }
            return View(marker);
        }

        // POST: Markers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Marker marker = db.Markers.Find(id);
            db.Markers.Remove(marker);
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