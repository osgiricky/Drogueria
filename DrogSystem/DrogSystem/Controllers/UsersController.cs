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
                           orderby u.Nombre
                           select  u).ToList();
            if (Listaux != null)
            {
                foreach (var item in Listaux)
                {
                    EDUser EDUser = new EDUser();
                    EDUser.UsuarioId = item.UsuarioId;
                    EDUser.CodUsuario = item.CodUsuario;
                    EDUser.Nombre = item.Nombre;
                    EDUser.Clave = item.Clave;
                    EDUser.TipoUsuario = item.TipoUsuario;
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
                EDUsuario.UsuarioId = user.UsuarioId;
                EDUsuario.CodUsuario = user.CodUsuario;
                EDUsuario.Nombre = user.Nombre;
                EDUsuario.Clave = user.Clave;
                EDUsuario.TipoUsuario = user.TipoUsuario;
            }
            return Json(EDUsuario, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Borrar(int ID)
        {
            bool Probar = true;
            string Mensaje = "";
            User user = db.Users.Find(ID);
            if (user == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + user.Nombre;
            }
            else
            {
                try
                {
                    db.Users.Remove(user);
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

        public JsonResult Editar(EDUser usuario)
        {
            bool Probar = true;
            string Mensaje = "";
            EDUser EDUser = new EDUser();
            EDUser.UsuarioId = usuario.UsuarioId;
            EDUser.CodUsuario = usuario.CodUsuario;
            EDUser.Nombre = usuario.Nombre;
            EDUser.Clave = usuario.Clave;
            EDUser.TipoUsuario = usuario.TipoUsuario;

            User user = db.Users.Find(EDUser.UsuarioId);
            if (user == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + EDUser.Nombre;
            }
            else
            {
                try
                {
                    user.CodUsuario = EDUser.CodUsuario;
                    user.Nombre = EDUser.Nombre;
                    user.Clave = EDUser.Clave;
                    user.TipoUsuario = EDUser.TipoUsuario;
                    db.Entry(user).State = EntityState.Modified;
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

        public JsonResult Crear(EDUser usuario)
        {
            bool Probar = true;
            string Mensaje = "";
            EDUser EDUser = new EDUser();
            EDUser.UsuarioId = usuario.UsuarioId;
            EDUser.CodUsuario = usuario.CodUsuario;
            EDUser.Nombre = usuario.Nombre;
            EDUser.Clave = usuario.Clave;
            EDUser.TipoUsuario = usuario.TipoUsuario;
            try
            {
                User user = new User();
                user.CodUsuario = EDUser.CodUsuario;
                user.Nombre = EDUser.Nombre;
                user.Clave = EDUser.Clave;
                user.TipoUsuario = EDUser.TipoUsuario;
                db.Users.Add(user);
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
            //ViewBag.TipoUsuarioId = new SelectList(db.UserTypes, "TipoUsuarioId", "Descripcion");
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

            //ViewBag.TipoUsuarioId = new SelectList(db.UserTypes, "TipoUsuarioId", "Descripcion", user.TipoUsuarioId);
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
            //ViewBag.TipoUsuarioId = new SelectList(db.UserTypes, "TipoUsuarioId", "Descripcion", user.TipoUsuarioId);
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
            //ViewBag.TipoUsuarioId = new SelectList(db.UserTypes, "TipoUsuarioId", "Descripcion", user.TipoUsuarioId);
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
