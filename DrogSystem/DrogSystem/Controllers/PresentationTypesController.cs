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
    public class PresentationTypesController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: PresentationTypes
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            List<EDPresentationType> TipoUser = new List<EDPresentationType>();
            var Listaux = (from s in db.PresentationTypes
                           select s).ToList<PresentationType>();
            if (Listaux != null)
            {
                foreach (var item in Listaux)
                {
                    EDPresentationType EDPresentationType = new EDPresentationType();
                    EDPresentationType.PresentationTypeId = item.PresentationTypeId;
                    EDPresentationType.NombrePresentacion = item.NombrePresentacion;
                    TipoUser.Add(EDPresentationType);
                }
            }
            return Json(TipoUser, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int? ID)
        {
            PresentationType PresentationType = db.PresentationTypes.Find(ID);
            EDPresentationType EDPresentationType = new EDPresentationType();
            if (PresentationType != null)
            {
                EDPresentationType.PresentationTypeId = PresentationType.PresentationTypeId;
                EDPresentationType.NombrePresentacion = PresentationType.NombrePresentacion;
            }
            return Json(EDPresentationType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Borrar(int ID)
        {
            bool Probar = true;
            string Mensaje = "";
            PresentationType PresentationType = db.PresentationTypes.Find(ID);
            if (PresentationType == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + PresentationType.NombrePresentacion;
            }
            else
            {
                try
                {
                    db.PresentationTypes.Remove(PresentationType);
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

        public JsonResult Editar(EDPresentationType Fabric)
        {
            bool Probar = true;
            string Mensaje = "";
            EDPresentationType EDPresentationType = new EDPresentationType();
            EDPresentationType.PresentationTypeId = Fabric.PresentationTypeId;
            EDPresentationType.NombrePresentacion = Fabric.NombrePresentacion;


            PresentationType PresentationType = db.PresentationTypes.Find(EDPresentationType.PresentationTypeId);
            if (PresentationType == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + EDPresentationType.PresentationTypeId;
            }
            else
            {
                try
                {
                    PresentationType.NombrePresentacion = EDPresentationType.NombrePresentacion;
                    db.Entry(PresentationType).State = EntityState.Modified;
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

        public JsonResult Crear(EDPresentationType Fabric)
        {
            bool Probar = true;
            string Mensaje = "";
            EDPresentationType EDPresentationType = new EDPresentationType();
            EDPresentationType.PresentationTypeId = Fabric.PresentationTypeId;
            EDPresentationType.NombrePresentacion = Fabric.NombrePresentacion;
            try
            {
                PresentationType PresentationType = new PresentationType();
                PresentationType.NombrePresentacion = EDPresentationType.NombrePresentacion;
                db.PresentationTypes.Add(PresentationType);
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
