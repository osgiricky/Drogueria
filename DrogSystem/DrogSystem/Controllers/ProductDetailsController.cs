using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DrogSystem.Models;

namespace DrogSystem.Controllers
{
    public class ProductDetailsController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: ProductDetails
        public ActionResult Index()
        {
            var productDetails = db.ProductDetails.Include(p => p.Marker).Include(p => p.Product);
            return View(productDetails.ToList());
        }

        // GET: ProductDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetail productDetail = db.ProductDetails.Find(id);
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            return View(productDetail);
        }

        // GET: ProductDetails/Create
        public ActionResult Create()
        {
            ViewBag.MarkerId = new SelectList(db.Markers, "MarkerId", "NombreFabricante");
            ViewBag.ProductoId = new SelectList(db.Products, "ProductoId", "NombreProducto");
            return View();
        }

        // POST: ProductDetails/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductDetailId,CodBarras,RegInvima,Existencias,ProductoId,MarkerId")] ProductDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                db.ProductDetails.Add(productDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MarkerId = new SelectList(db.Markers, "MarkerId", "NombreFabricante", productDetail.MarkerId);
            ViewBag.ProductoId = new SelectList(db.Products, "ProductoId", "NombreProducto", productDetail.ProductoId);
            return View(productDetail);
        }

        // GET: ProductDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetail productDetail = db.ProductDetails.Find(id);
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.MarkerId = new SelectList(db.Markers, "MarkerId", "NombreFabricante", productDetail.MarkerId);
            ViewBag.ProductoId = new SelectList(db.Products, "ProductoId", "NombreProducto", productDetail.ProductoId);
            return View(productDetail);
        }

        // POST: ProductDetails/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductDetailId,CodBarras,RegInvima,Existencias,ProductoId,MarkerId")] ProductDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MarkerId = new SelectList(db.Markers, "MarkerId", "NombreFabricante", productDetail.MarkerId);
            ViewBag.ProductoId = new SelectList(db.Products, "ProductoId", "NombreProducto", productDetail.ProductoId);
            return View(productDetail);
        }

        // GET: ProductDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetail productDetail = db.ProductDetails.Find(id);
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            return View(productDetail);
        }

        // POST: ProductDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductDetail productDetail = db.ProductDetails.Find(id);
            db.ProductDetails.Remove(productDetail);
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
