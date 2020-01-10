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
    public class UserTypesController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: UserTypes
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            List<EDUserType> TipoUser = new List<EDUserType>();
            var Listaux = (from s in db.UserTypes
                           select s).ToList<UserType>();
            if (Listaux != null)
            {
                foreach (var item in Listaux)
                {
                    EDUserType EDUserType = new EDUserType();
                    EDUserType.TipoUsuarioId = item.TipoUsuarioId;
                    EDUserType.Descripcion = item.Descripcion;
                    TipoUser.Add(EDUserType);
                }
            }
            return Json(TipoUser, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int? ID)
        {
            UserType UserType = db.UserTypes.Find(ID);
            EDUserType EDUserType = new EDUserType();
            if (UserType != null)
            {
                EDUserType.TipoUsuarioId = UserType.TipoUsuarioId;
                EDUserType.Descripcion = UserType.Descripcion;
            }
            return Json(EDUserType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Borrar(int ID)
        {
            bool Probar = true;
            string Mensaje = "";
            UserType UserType = db.UserTypes.Find(ID);
            if (UserType == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + UserType.Descripcion;
            }
            else
            {
                try
                {
                    db.UserTypes.Remove(UserType);
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

        public JsonResult Editar(EDUserType Fabric)
        {
            bool Probar = true;
            string Mensaje = "";
            EDUserType EDUserType = new EDUserType();
            EDUserType.TipoUsuarioId = Fabric.TipoUsuarioId;
            EDUserType.Descripcion = Fabric.Descripcion;


            UserType UserType = db.UserTypes.Find(EDUserType.TipoUsuarioId);
            if (UserType == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + EDUserType.TipoUsuarioId;
            }
            else
            {
                try
                {
                    UserType.Descripcion = EDUserType.Descripcion;
                    db.Entry(UserType).State = EntityState.Modified;
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

        public JsonResult Crear(EDUserType Fabric)
        {
            bool Probar = true;
            string Mensaje = "";
            EDUserType EDUserType = new EDUserType();
            EDUserType.TipoUsuarioId = Fabric.TipoUsuarioId;
            EDUserType.Descripcion = Fabric.Descripcion;
            try
            {
                UserType UserType = new UserType();
                UserType.Descripcion = EDUserType.Descripcion;
                db.UserTypes.Add(UserType);
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

        // GET: UserTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserType userType = db.UserTypes.Find(id);
            if (userType == null)
            {
                return HttpNotFound();
            }
            return View(userType);
        }

        // GET: UserTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserTypes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TipoUsuarioId,Descripcion")] UserType userType)
        {
            if (ModelState.IsValid)
            {
                db.UserTypes.Add(userType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userType);
        }

        // GET: UserTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserType userType = db.UserTypes.Find(id);
            if (userType == null)
            {
                return HttpNotFound();
            }
            return View(userType);
        }

        // POST: UserTypes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TipoUsuarioId,Descripcion")] UserType userType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userType);
        }

        // GET: UserTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserType userType = db.UserTypes.Find(id);
            if (userType == null)
            {
                return HttpNotFound();
            }
            return View(userType);
        }

        // POST: UserTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserType userType = db.UserTypes.Find(id);
            db.UserTypes.Remove(userType);
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
