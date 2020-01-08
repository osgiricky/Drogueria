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
    public class PaymentProvidersController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: PaymentProviders
        public ActionResult Index()
        {
            var paymentProviders = db.PaymentProviders.Include(p => p.Provider);
            return View(paymentProviders.ToList());
        }

        // GET: PaymentProviders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentProvider paymentProvider = db.PaymentProviders.Find(id);
            if (paymentProvider == null)
            {
                return HttpNotFound();
            }
            return View(paymentProvider);
        }

        // GET: PaymentProviders/Create
        public ActionResult Create()
        {
            ViewBag.TerceroId = new SelectList(db.Providers, "TerceroId", "NombreTercero");
            return View();
        }

        // POST: PaymentProviders/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Pago,Valor_Pago,Fecha_Pago,Observacion,TerceroId")] PaymentProvider paymentProvider)
        {
            if (ModelState.IsValid)
            {
                db.PaymentProviders.Add(paymentProvider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TerceroId = new SelectList(db.Providers, "TerceroId", "NombreTercero", paymentProvider.TerceroId);
            return View(paymentProvider);
        }

        // GET: PaymentProviders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentProvider paymentProvider = db.PaymentProviders.Find(id);
            if (paymentProvider == null)
            {
                return HttpNotFound();
            }
            ViewBag.TerceroId = new SelectList(db.Providers, "TerceroId", "NombreTercero", paymentProvider.TerceroId);
            return View(paymentProvider);
        }

        // POST: PaymentProviders/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Pago,Valor_Pago,Fecha_Pago,Observacion,TerceroId")] PaymentProvider paymentProvider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentProvider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TerceroId = new SelectList(db.Providers, "TerceroId", "NombreTercero", paymentProvider.TerceroId);
            return View(paymentProvider);
        }

        // GET: PaymentProviders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentProvider paymentProvider = db.PaymentProviders.Find(id);
            if (paymentProvider == null)
            {
                return HttpNotFound();
            }
            return View(paymentProvider);
        }

        // POST: PaymentProviders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentProvider paymentProvider = db.PaymentProviders.Find(id);
            db.PaymentProviders.Remove(paymentProvider);
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
