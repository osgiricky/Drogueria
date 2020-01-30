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
    public class ProvidersController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: Providers
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            List<EDProvider> EDProviderLista = new List<EDProvider>();
            var Listaux = (from u in db.Providers
                          select new { u }).ToList();
            if (Listaux != null)
            {
                foreach (var item in Listaux)
                {
                    EDProvider EDprovider = new EDProvider();
                    EDprovider.TerceroId = item.u.TerceroId;
                    EDprovider.NombreTercero = item.u.NombreTercero;
                    EDprovider.Codtercero = item.u.Codtercero;
                    EDprovider.TipoTercero = item.u.TipoTercero;
                    EDProviderLista.Add(EDprovider);
                }
            }
            return Json(EDProviderLista, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int? ID)
        {
            Provider Provider = db.Providers.Find(ID);
            EDProvider EDprovider = new EDProvider();
            if (Provider != null)
            {
                FuncUsuarios FuncUsuarios = new FuncUsuarios();
                List<EDProviderType> ListaTipos = new List<EDProviderType>();
                ListaTipos = FuncUsuarios.ListaTiposTerceros();
                EDprovider.TerceroId = Provider.TerceroId;
                EDprovider.NombreTercero = Provider.NombreTercero;
                EDprovider.Codtercero = Provider.Codtercero;
                //EDprovider.ProviderTypeId = Provider.ProviderTypeId;
                EDProviderType providerdescrip = ListaTipos.Find(u => u.ProviderTypeId == EDprovider.ProviderTypeId);
                EDprovider.TipoTercero = providerdescrip.TipoTercero;
                EDprovider.ListaTipoTercero = ListaTipos;
            }
            return Json(EDprovider, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Borrar(int ID)
        {
            bool Probar = true;
            string Mensaje = "";
            Provider Provider = db.Providers.Find(ID);
            if (Provider == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + Provider.NombreTercero;
            }
            else
            {
                try
                {
                    db.Providers.Remove(Provider);
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

        public JsonResult Editar(EDProvider proveedor)
        {
            bool Probar = true;
            string Mensaje = "";
            EDProvider EDProvider = new EDProvider();
            EDProvider.TerceroId = proveedor.TerceroId;
            EDProvider.NombreTercero = proveedor.NombreTercero;
            EDProvider.Codtercero = proveedor.Codtercero;
            EDProvider.ProviderTypeId = proveedor.ProviderTypeId;

            Provider provider = db.Providers.Find(EDProvider.TerceroId);
            if (provider == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + EDProvider.NombreTercero;
            }
            else
            {
                try
                {
                    provider.NombreTercero = EDProvider.NombreTercero;
                    provider.Codtercero = EDProvider.Codtercero;
                    provider.ProviderTypeId = EDProvider.ProviderTypeId;
                    db.Entry(provider).State = EntityState.Modified;
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

        public JsonResult Crear(EDProvider proveedor)
        {
            bool Probar = true;
            string Mensaje = "";
            EDProvider EDProvider = new EDProvider();
            EDProvider.TerceroId = proveedor.TerceroId;
            EDProvider.NombreTercero = proveedor.NombreTercero;
            EDProvider.Codtercero = proveedor.Codtercero;
            EDProvider.ProviderTypeId = proveedor.ProviderTypeId;
            try
            {
                Provider provider = new Provider();
                provider.NombreTercero = EDProvider.NombreTercero;
                provider.Codtercero = EDProvider.Codtercero;
                provider.ProviderTypeId = EDProvider.ProviderTypeId;
                db.Providers.Add(provider);
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

        public JsonResult listatipos()
        {
            FuncUsuarios FuncUsuarios = new FuncUsuarios();
            List<EDProviderType> ListaTipos = new List<EDProviderType>();
            ListaTipos = FuncUsuarios.ListaTiposTerceros();
            return Json(ListaTipos, JsonRequestBehavior.AllowGet);
        }

        // GET: Providers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // GET: Providers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Providers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TerceroId,NombreTercero,Codtercero")] Provider provider)
        {
            if (ModelState.IsValid)
            {
                db.Providers.Add(provider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(provider);
        }

        // GET: Providers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // POST: Providers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TerceroId,NombreTercero,Codtercero")] Provider provider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(provider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(provider);
        }

        // GET: Providers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Provider provider = db.Providers.Find(id);
            db.Providers.Remove(provider);
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
