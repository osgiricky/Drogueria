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
    public class UsersController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            List<EDUser> EDUserLista = new List<EDUser>();
            var Listaux = (from u in db.Users
                           join ut in db.UserTypes on u.TipoUsuarioId equals ut.TipoUsuarioId
                           select new { u, ut }).ToList();
            if (Listaux != null)
            {
                foreach (var item in Listaux)
                {
                    EDUser EDUser = new EDUser();
                    EDUser.UsuarioId = item.u.UsuarioId;
                    EDUser.CodUsuario = item.u.CodUsuario;
                    EDUser.Nombre = item.u.Nombre;
                    EDUser.Clave = item.u.Clave;
                    EDUser.TipoUsuarioId = item.u.TipoUsuarioId;
                    EDUser.Descripcion = item.ut.Descripcion;
                    EDUserLista.Add(EDUser);
                }
            }
            return Json(EDUserLista, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int? ID)
        {
            User user = db.Users.Find(ID);
            EDUser EDUsuario = new EDUser();
            if (user != null)
            {
                FuncUsuarios FuncUsuarios = new FuncUsuarios();
                List<EDUserType> ListaTipos = new List<EDUserType>();
                ListaTipos = FuncUsuarios.ListaTiposUsuario();                
                EDUsuario.UsuarioId = user.UsuarioId;
                EDUsuario.CodUsuario = user.CodUsuario;
                EDUsuario.Nombre = user.Nombre;
                EDUsuario.Clave = user.Clave;
                EDUsuario.TipoUsuarioId = user.TipoUsuarioId;
                EDUsuario.ListaTipoUsuario = ListaTipos;

            }
            return Json(EDUsuario, JsonRequestBehavior.AllowGet);
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

        public JsonResult Editar(EDUser Fabric)
        {
            bool Probar = true;
            string Mensaje = "";
            EDUser EDUser = new EDUser();
            EDUser.PresentationId = Fabric.PresentationId;
            EDUser.NombrePresentacion = Fabric.NombrePresentacion;
            EDUser.CantPresentacion = Fabric.CantPresentacion;

            Presentation Presentation = db.Presentations.Find(EDUser.PresentationId);
            if (Presentation == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + EDUser.PresentationId;
            }
            else
            {
                try
                {
                    Presentation.NombrePresentacion = EDUser.NombrePresentacion;
                    Presentation.CantPresentacion = EDUser.CantPresentacion;
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

        public JsonResult Crear(EDUser Fabric)
        {
            bool Probar = true;
            string Mensaje = "";
            EDUser EDUser = new EDUser();
            EDUser.PresentationId = Fabric.PresentationId;
            EDUser.NombrePresentacion = Fabric.NombrePresentacion;
            EDUser.CantPresentacion = Fabric.CantPresentacion;
            try
            {
                Presentation Presentation = new Presentation();
                Presentation.NombrePresentacion = EDUser.NombrePresentacion;
                Presentation.CantPresentacion = EDUser.CantPresentacion;
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

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.TipoUsuarioId = new SelectList(db.UserTypes, "TipoUsuarioId", "Descripcion");
            return View();
        }

        // POST: Users/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsuarioId,CodUsuario,Nombre,Clave,TipoUsuarioId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TipoUsuarioId = new SelectList(db.UserTypes, "TipoUsuarioId", "Descripcion", user.TipoUsuarioId);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipoUsuarioId = new SelectList(db.UserTypes, "TipoUsuarioId", "Descripcion", user.TipoUsuarioId);
            return View(user);
        }

        // POST: Users/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UsuarioId,CodUsuario,Nombre,Clave,TipoUsuarioId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipoUsuarioId = new SelectList(db.UserTypes, "TipoUsuarioId", "Descripcion", user.TipoUsuarioId);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
