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
using DrogSystem.Funciones;

namespace DrogSystem.Controllers
{
    public class ProviderTypesController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: ProviderTypes
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            FuncUsuarios FuncUsuarios = new FuncUsuarios();
            List<EDProviderType> TipoProveedor = new List<EDProviderType>();
            TipoProveedor = FuncUsuarios.ListaTiposTerceros();

            return Json(TipoProveedor, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int? ID)
        {
            ProviderType ProviderType = db.ProviderTypes.Find(ID);
            EDProviderType EDProviderType = new EDProviderType();
            if (ProviderType != null)
            {
                EDProviderType.ProviderTypeId = ProviderType.ProviderTypeId;
                EDProviderType.TipoTercero = ProviderType.TipoTercero;
            }
            return Json(EDProviderType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Borrar(int ID)
        {
            bool Probar = true;
            string Mensaje = "";
            ProviderType ProviderType = db.ProviderTypes.Find(ID);
            if (ProviderType == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + ProviderType.TipoTercero;
            }
            else
            {
                try
                {
                    db.ProviderTypes.Remove(ProviderType);
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

        public JsonResult Editar(EDProviderType TipoProv)
        {
            bool Probar = true;
            string Mensaje = "";
            EDProviderType EDProviderType = new EDProviderType();
            EDProviderType.ProviderTypeId = TipoProv.ProviderTypeId;
            EDProviderType.TipoTercero = TipoProv.TipoTercero;


            ProviderType ProviderType = db.ProviderTypes.Find(EDProviderType.ProviderTypeId);
            if (ProviderType == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + EDProviderType.TipoTercero;
            }
            else
            {
                try
                {
                    ProviderType.TipoTercero = EDProviderType.TipoTercero;
                    db.Entry(ProviderType).State = EntityState.Modified;
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

        public JsonResult Crear(EDProviderType TipoProv)
        {
            bool Probar = true;
            string Mensaje = "";
            EDProviderType EDProviderType = new EDProviderType();
            EDProviderType.TipoTercero = TipoProv.TipoTercero;
            try
            {
                ProviderType ProviderType = new ProviderType();
                ProviderType.TipoTercero = EDProviderType.TipoTercero;
                db.ProviderTypes.Add(ProviderType);
                db.SaveChanges();
                Mensaje = " Registro creado con exito.";
            }
            catch (Exception)
            {
                Probar = false;
                Mensaje = " Se produjo un error al modificar el registro.";

            }


            return Json(new { Probar, Mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: ProviderTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderType providerType = db.ProviderTypes.Find(id);
            if (providerType == null)
            {
                return HttpNotFound();
            }
            return View(providerType);
        }

        // GET: ProviderTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProviderTypes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProviderTypeId,TipoTercero")] ProviderType providerType)
        {
            if (ModelState.IsValid)
            {
                db.ProviderTypes.Add(providerType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(providerType);
        }

        // GET: ProviderTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderType providerType = db.ProviderTypes.Find(id);
            if (providerType == null)
            {
                return HttpNotFound();
            }
            return View(providerType);
        }

        // POST: ProviderTypes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProviderTypeId,TipoTercero")] ProviderType providerType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(providerType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(providerType);
        }

        // GET: ProviderTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderType providerType = db.ProviderTypes.Find(id);
            if (providerType == null)
            {
                return HttpNotFound();
            }
            return View(providerType);
        }

        // POST: ProviderTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProviderType providerType = db.ProviderTypes.Find(id);
            db.ProviderTypes.Remove(providerType);
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
