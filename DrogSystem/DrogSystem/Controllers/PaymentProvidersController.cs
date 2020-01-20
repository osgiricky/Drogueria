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
    public class PaymentProvidersController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: PaymentProviders
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            List<EDPaymentProvider> EDPaymentProviderLista = new List<EDPaymentProvider>();
            var Listaux = (from P in db.PaymentProviders
                           join T in db.Providers on P.TerceroId equals T.TerceroId
                           select new { P, T }).ToList();
            if (Listaux != null)
            {
                foreach (var item in Listaux)
                {
                    EDPaymentProvider EDPaymentProvider = new EDPaymentProvider();
                    EDPaymentProvider.Id_Pago = item.P.Id_Pago;
                    EDPaymentProvider.Valor_Pago = item.P.Valor_Pago;
                    EDPaymentProvider.Fecha_Pago = item.P.Fecha_Pago.ToString("dd/MM/yyyy");
                    EDPaymentProvider.Observacion = item.P.Observacion;
                    EDPaymentProvider.TerceroId = item.P.TerceroId;
                    EDPaymentProvider.NombreTercero = item.T.NombreTercero;
                    EDPaymentProviderLista.Add(EDPaymentProvider);
                }
            }
            return Json(EDPaymentProviderLista, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int? ID)
        {
            PaymentProvider PaymentProvider = db.PaymentProviders.Find(ID);
            EDPaymentProvider EDPaymentProvider = new EDPaymentProvider();
            if (PaymentProvider != null)
            {
                FuncUsuarios FuncUsuarios = new FuncUsuarios();
                List<EDProvider> ListaTerceros = new List<EDProvider>();
                ListaTerceros = FuncUsuarios.ListaTerceros();
                EDPaymentProvider.Id_Pago = PaymentProvider.Id_Pago;
                EDPaymentProvider.Valor_Pago = PaymentProvider.Valor_Pago;
                EDPaymentProvider.Fecha_Pago = PaymentProvider.Fecha_Pago.ToString("dd/MM/yyyy");
                EDPaymentProvider.Observacion = PaymentProvider.Observacion;
                EDPaymentProvider.TerceroId = PaymentProvider.TerceroId;
                EDProvider providername = ListaTerceros.Find(u => u.TerceroId == EDPaymentProvider.TerceroId);
                EDPaymentProvider.NombreTercero = providername.NombreTercero;
                EDPaymentProvider.ListaTerceros = ListaTerceros;
            }
            return Json(EDPaymentProvider, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Borrar(int ID)
        {
            bool Probar = true;
            string Mensaje = "";
            PaymentProvider PaymentProvider = db.PaymentProviders.Find(ID);
            if (PaymentProvider == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + PaymentProvider.Id_Pago;
            }
            else
            {
                try
                {
                    db.PaymentProviders.Remove(PaymentProvider);
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

        public JsonResult Editar(EDPaymentProvider Payment)
        {
            bool Probar = true;
            string Mensaje = "";
            EDPaymentProvider EDPaymentProvider = new EDPaymentProvider();
            EDPaymentProvider.Id_Pago = Payment.Id_Pago;
            EDPaymentProvider.Valor_Pago = Payment.Valor_Pago;
            EDPaymentProvider.Fecha_Pago = Payment.Fecha_Pago;
            EDPaymentProvider.Observacion = Payment.Observacion;
            EDPaymentProvider.TerceroId = Payment.TerceroId;

            PaymentProvider PaymentProvider = db.PaymentProviders.Find(EDPaymentProvider.TerceroId);
            if (PaymentProvider == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + EDPaymentProvider.Id_Pago;
            }
            else
            {
                try
                {
                    PaymentProvider.Valor_Pago = EDPaymentProvider.Valor_Pago;
                    PaymentProvider.Fecha_Pago = DateTime.Parse(EDPaymentProvider.Fecha_Pago);
                    PaymentProvider.Observacion = EDPaymentProvider.Observacion;
                    PaymentProvider.TerceroId = EDPaymentProvider.TerceroId;
                    db.Entry(PaymentProvider).State = EntityState.Modified;
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

        public JsonResult Crear(EDPaymentProvider Payment)
        {
            bool Probar = true;
            string Mensaje = "";
            EDPaymentProvider EDPaymentProvider = new EDPaymentProvider();
            EDPaymentProvider.Id_Pago = Payment.Id_Pago;
            EDPaymentProvider.Valor_Pago = Payment.Valor_Pago;
            EDPaymentProvider.Fecha_Pago = Payment.Fecha_Pago;
            EDPaymentProvider.Observacion = Payment.Observacion;
            EDPaymentProvider.TerceroId = Payment.TerceroId;
            try
            {
                PaymentProvider PaymentProvider = new PaymentProvider();
                PaymentProvider.Valor_Pago =  EDPaymentProvider.Valor_Pago;
                PaymentProvider.Valor_Pago = EDPaymentProvider.Valor_Pago;
                PaymentProvider.Fecha_Pago = DateTime.Parse(EDPaymentProvider.Fecha_Pago);
                PaymentProvider.Observacion = EDPaymentProvider.Observacion;
                PaymentProvider.TerceroId = EDPaymentProvider.TerceroId;
                db.PaymentProviders.Add(PaymentProvider);
                db.SaveChanges();
                Mensaje = " Registro Agregado con exito.";
            }
            catch (Exception)
            {
                Probar = false;
                Mensaje = " Se produjo un error al agregar el registro.";

            }


            return Json(new { Probar, Mensaje }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult listaTerceros()
        {
            FuncUsuarios FuncUsuarios = new FuncUsuarios();
            List<EDProvider> ListaTerceros = new List<EDProvider>();
            ListaTerceros = FuncUsuarios.ListaTerceros();
            return Json(ListaTerceros, JsonRequestBehavior.AllowGet);
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
