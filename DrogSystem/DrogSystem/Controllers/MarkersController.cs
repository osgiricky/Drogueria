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
    public class MarkersController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: Markers
        public ActionResult Index()
        {
            return View();
           
        }

        public JsonResult List()
        {
            List<EDMarker> ListaFabric = new List<EDMarker>();
            var Listaux = (from s in db.Markers
                           select s).ToList<Marker>();
            if (Listaux != null)
            {
                foreach (var item in Listaux)
                {
                    EDMarker EDMarker = new EDMarker();
                    EDMarker.MarkerId = item.MarkerId;
                    EDMarker.NombreFabricante = item.NombreFabricante;
                    ListaFabric.Add(EDMarker);
                }
            }
            return Json(ListaFabric, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetbyID(int ID)
        {
            var Marker = (from s in db.Markers
                          where s.MarkerId == ID
                          select s).ToList<Marker>(); 
            return Json(Marker, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int ID)
        {
            bool Probar = true;
            string Mensaje = "";
            Marker marker = db.Markers.Find(ID);
            if (marker == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + marker.NombreFabricante;
            }
            else
            {
                try
                {
                    Marker marker1 = db.Markers.Find(ID);
                    db.Markers.Remove(marker1);
                    db.SaveChanges();
                    Mensaje = " Registro eliminado con exito.";
                }
                catch (Exception)
                {
                    Probar = false;
                    Mensaje = " Se produjo un error al borrar el registro.";

                }
            }

            return Json(Probar, JsonRequestBehavior.AllowGet);
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
