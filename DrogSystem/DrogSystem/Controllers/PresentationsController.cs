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

namespace DrogSystem.Controllers
{
    public class PresentationsController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: Presentations
         public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            List<EDPresentacion> EDPresenta = new List<EDPresentacion>();
            var Listaux = (from s in db.Presentations
                           orderby s.NombrePresentacion, s.CantPresentacion
                           select s).ToList<Presentation>();
            if (Listaux != null)
            {
                foreach (var item in Listaux)
                {
                    EDPresentacion EDPresentacion = new EDPresentacion();
                    EDPresentacion.PresentationId = item.PresentationId;
                    EDPresentacion.NombrePresentacion = item.NombrePresentacion;
                    EDPresentacion.CantPresentacion = item.CantPresentacion;
                    EDPresenta.Add(EDPresentacion);
                }
            }
            return Json(EDPresenta, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int? ID)
        {
            Presentation Presentation = db.Presentations.Find(ID);
            EDPresentacion EDPresentacion = new EDPresentacion();
            if (Presentation != null)
            {
                EDPresentacion.PresentationId = Presentation.PresentationId;
                EDPresentacion.NombrePresentacion = Presentation.NombrePresentacion;
                EDPresentacion.CantPresentacion = Presentation.CantPresentacion;
            }
            return Json(EDPresentacion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Borrar(int ID)
        {
            bool Probar = true;
            string Mensaje = "";
            Presentation Presentation = db.Presentations.Find(ID);
            if (Presentation == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + Presentation.NombrePresentacion;
            }
            else
            {
                try
                {
                    db.Presentations.Remove(Presentation);
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

        public JsonResult Editar(EDPresentacion Fabric)
        {
            bool Probar = true;
            string Mensaje = "";
            EDPresentacion EDPresentacion = new EDPresentacion();
            EDPresentacion.PresentationId = Fabric.PresentationId;
            EDPresentacion.NombrePresentacion = Fabric.NombrePresentacion;
            EDPresentacion.CantPresentacion = Fabric.CantPresentacion;

            Presentation Presentation = db.Presentations.Find(EDPresentacion.PresentationId);
            if (Presentation == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + EDPresentacion.PresentationId;
            }
            else
            {
                try
                {
                    Presentation.NombrePresentacion = EDPresentacion.NombrePresentacion;
                    Presentation.CantPresentacion = EDPresentacion.CantPresentacion;
                    db.Entry(Presentation).State = EntityState.Modified;
                    db.SaveChanges();
                    Mensaje = " Registro modificado con exito.";
                }
                catch (Exception)
                {
                    Probar = false;
                    Mensaje = " Se produjo un error al modificar el registro.";

                }
            }

            return Json(new { Probar, Mensaje }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Crear(EDPresentacion Fabric)
        {
            bool Probar = true;
            string Mensaje = "";
            EDPresentacion EDPresentacion = new EDPresentacion();
            EDPresentacion.PresentationId = Fabric.PresentationId;
            EDPresentacion.NombrePresentacion = Fabric.NombrePresentacion;
            EDPresentacion.CantPresentacion = Fabric.CantPresentacion;
            try
            {
                Presentation Presentation = new Presentation();
                Presentation.NombrePresentacion = EDPresentacion.NombrePresentacion;
                Presentation.CantPresentacion = EDPresentacion.CantPresentacion;
                db.Presentations.Add(Presentation);
                db.SaveChanges();
                Mensaje = " Registro modificado con exito.";
            }
            catch (Exception)
            {
                Probar = false;
                Mensaje = " Se produjo un error al modificar el registro.";

            }


            return Json(new { Probar, Mensaje }, JsonRequestBehavior.AllowGet);
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
            //ViewBag.PresentationsId = new SelectList(db.Presentation, "PresentationsId", "NombrePresentacion");
            return View();
        }

        // POST: Presentations/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PresentationId,CantPresentacion,PresentationsId")] Presentation presentation)
        {
            if (ModelState.IsValid)
            {
                db.Presentations.Add(presentation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.PresentationsId = new SelectList(db.Presentation, "PresentationsId", "NombrePresentacion", presentation.PresentationsId);
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
            //ViewBag.PresentationsId = new SelectList(db.Presentation, "PresentationsId", "NombrePresentacion", presentation.PresentationsId);
            return View(presentation);
        }

        // POST: Presentations/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PresentationId,CantPresentacion,PresentationsId")] Presentation presentation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(presentation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.PresentationsId = new SelectList(db.Presentation, "PresentationsId", "NombrePresentacion", presentation.PresentationsId);
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
