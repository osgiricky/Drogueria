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
    public class AccountingsController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: Accountings
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            List<EDAccounting> ListaEDAccounting = new List<EDAccounting>();
            var Listaux = (from A in db.Accountings
                           orderby A.FechaCierre descending
                           select A).ToList();
            if (Listaux != null)
            {
                foreach (var item in Listaux)
                {
                    EDAccounting EDAccounting = new EDAccounting();
                    EDAccounting.ContabilidadId = item.ContabilidadId;
                    EDAccounting.FechaCierre = item.FechaCierre.ToString("dd/MM/yyyy");
                    EDAccounting.Ingresos = item.Ingresos;
                    EDAccounting.Egresos = item.Egresos;
                    EDAccounting.BaseCaja = item.BaseCaja;
                    ListaEDAccounting.Add(EDAccounting);
                }
                ListaEDAccounting = ListaEDAccounting.OrderBy(o => o.FechaCierre).ToList();
            }
            return Json(ListaEDAccounting, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int? ID)
        {
            var Entrada = (from A in db.Accountings
                           where A.ContabilidadId == ID
                           select A);

            EDAccounting EDAccounting = new EDAccounting();
            if (Entrada != null)
            {
                foreach (var item in Entrada)
                {
                    EDAccounting.ContabilidadId = item.ContabilidadId;
                    EDAccounting.FechaCierre = item.FechaCierre.ToString("dd/MM/yyyy");
                    EDAccounting.Ingresos = item.Ingresos;
                    EDAccounting.Egresos = item.Egresos;
                    EDAccounting.BaseCaja = item.BaseCaja;
                }
            }
            return Json(EDAccounting, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DatosCierre()
        {
            var fecha = DateTime.Now.Date;
            var Entrada = (from A in db.Sales
                           where A.FechaFactura == fecha
                           select A).ToList();
            var Ingresos = Entrada.Sum(o => o.ValorFactura);

            var Pagos = (from A in db.PaymentProviders
                         where A.Fecha_Pago == fecha
                         select A).ToList();
            var Egresos = Pagos.Sum(o => o.Valor_Pago);

            var ayer = fecha.AddDays(-1);
            var BaseIni = (from A in db.Accountings
                           where A.FechaCierre == ayer
                           select A.BaseCaja).FirstOrDefault(); 


            EDAccounting EDAccounting = new EDAccounting();
            EDAccounting.FechaCierre = DateTime.Now.ToString("dd/MM/yyyy");
            EDAccounting.Ingresos = Ingresos;
            EDAccounting.Egresos = Egresos;
            EDAccounting.BaseInicial = BaseIni;

            return Json(EDAccounting, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarCierre()
        {
            bool Probar = false;
            var fecha = DateTime.Now.Date;
            var ContabilidadId = (from A in db.Accountings
                           where A.FechaCierre == fecha
                           select A.ContabilidadId).FirstOrDefault();
            if(ContabilidadId > 0)
                Probar = true;
            return Json(Probar, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Crear(EDAccounting CierreDiario)
        {
            bool Probar = true;
            string Mensaje = "";
            EDAccounting EDAccounting = new EDAccounting();
            EDAccounting.FechaCierre = CierreDiario.FechaCierre;
            EDAccounting.Ingresos = CierreDiario.Ingresos;
            EDAccounting.Egresos = CierreDiario.Egresos;
            EDAccounting.BaseCaja = CierreDiario.BaseCaja;

            try
            {
                Accounting Accounting = new Accounting();
                Accounting.FechaCierre = DateTime.Parse(EDAccounting.FechaCierre);
                Accounting.Ingresos = EDAccounting.Ingresos;
                Accounting.Egresos = EDAccounting.Egresos;
                Accounting.BaseCaja = EDAccounting.BaseCaja;
                db.Accountings.Add(Accounting);
                db.SaveChanges();
                Mensaje = " Registro agregado con exito.";
            }
            catch (Exception)
            {
                Probar = false;
                Mensaje = " Se produjo un error al modificar el registro.";

            }


            return Json(new { Probar, Mensaje }, JsonRequestBehavior.AllowGet);
        }


        // GET: Accountings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounting accounting = db.Accountings.Find(id);
            if (accounting == null)
            {
                return HttpNotFound();
            }
            return View(accounting);
        }

        // GET: Accountings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accountings/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContabilidadId,FechaCierre,Ingresos,Egresos,BaseCaja")] Accounting accounting)
        {
            if (ModelState.IsValid)
            {
                db.Accountings.Add(accounting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accounting);
        }

        // GET: Accountings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounting accounting = db.Accountings.Find(id);
            if (accounting == null)
            {
                return HttpNotFound();
            }
            return View(accounting);
        }

        // POST: Accountings/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContabilidadId,FechaCierre,Ingresos,Egresos,BaseCaja")] Accounting accounting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accounting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accounting);
        }

        // GET: Accountings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounting accounting = db.Accountings.Find(id);
            if (accounting == null)
            {
                return HttpNotFound();
            }
            return View(accounting);
        }

        // POST: Accountings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Accounting accounting = db.Accountings.Find(id);
            db.Accountings.Remove(accounting);
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
