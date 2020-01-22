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
    public class ProductsController : Controller
    {
        private DrogSystemContext db = new DrogSystemContext();

        // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            List<EDProduct> EDProductLista = new List<EDProduct>();
            var Listaux = db.Products.ToList();
            if (Listaux != null)
            {
                foreach (var item in Listaux)
                {
                    EDProduct EDProduct = new EDProduct();
                    EDProduct.ProductoId = item.ProductoId;
                    EDProduct.NombreProducto = item.NombreProducto;
                    EDProduct.MinStock = item.MinStock;
                    EDProduct.Descripcion = item.Descripcion;
                    EDProduct.Componentes = item.Componentes;
                    EDProductLista.Add(EDProduct);
                }
            }
            return Json(EDProductLista, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int? ID)
        {
            Product Product = db.Products.Find(ID);
            EDProduct EDProduct = new EDProduct();
            if (Product != null)
            {
                FuncUsuarios FuncUsuarios = new FuncUsuarios();
                List<EDProvider> ListaTerceros = new List<EDProvider>();
                ListaTerceros = FuncUsuarios.ListaTerceros();
                EDProduct.ProductoId = Product.ProductoId;
                EDProduct.NombreProducto = Product.NombreProducto;
                EDProduct.MinStock = Product.MinStock;
                EDProduct.Descripcion = Product.Descripcion;
                EDProduct.Componentes = Product.Componentes;
                //EDProvider providername = ListaTerceros.Find(u => u.Descripcion == EDProduct.Descripcion);
                //EDProduct.Componentes = providername.Componentes;
                //EDProduct.ListaTerceros = ListaTerceros;
            }
            return Json(EDProduct, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Borrar(int ID)
        {
            bool Probar = true;
            string Mensaje = "";
            Product Product = db.Products.Find(ID);
            if (Product == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + Product.ProductoId;
            }
            else
            {
                try
                {
                    db.Products.Remove(Product);
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

        public JsonResult Editar(EDProduct Producto)
        {
            bool Probar = true;
            string Mensaje = "";
            EDProduct EDProduct = new EDProduct();
            EDProduct.ProductoId = Producto.ProductoId;
            EDProduct.NombreProducto = Producto.NombreProducto;
            EDProduct.MinStock = Producto.MinStock;
            EDProduct.Descripcion = Producto.Descripcion;
            EDProduct.Componentes = Producto.Componentes;

            Product Product = db.Products.Find(EDProduct.ProductoId);
            if (Product == null)
            {
                Probar = false;
                Mensaje = " No se encuntra el registro: " + EDProduct.ProductoId;
            }
            else
            {
                try
                {
                    Product.NombreProducto = EDProduct.NombreProducto;
                    Product.MinStock = EDProduct.MinStock;
                    Product.Descripcion = EDProduct.Descripcion;
                    Product.Componentes = EDProduct.Componentes;
                    db.Entry(Product).State = EntityState.Modified;
                    db.SaveChanges();
                    Mensaje = " Registro modificado con exito.";
                }
                catch (Exception e)
                {
                    Probar = false;
                    Mensaje = " Se produjo un error al modificar el registro.";

                }
            }

            return Json(new { Probar, Mensaje }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Crear(EDProduct Producto)
        {
            bool Probar = true;
            string Mensaje = "";
            EDProduct EDProduct = new EDProduct();
            EDProduct.ProductoId = Producto.ProductoId;
            EDProduct.NombreProducto = Producto.NombreProducto;
            EDProduct.MinStock = Producto.MinStock;
            EDProduct.Descripcion = Producto.Descripcion;
            EDProduct.Componentes = Producto.Componentes;
            try
            {
                Product Product = new Product();
                Product.NombreProducto = EDProduct.NombreProducto;
                Product.NombreProducto = EDProduct.NombreProducto;
                Product.MinStock = EDProduct.MinStock;
                Product.Descripcion = EDProduct.Descripcion;
                Product.Componentes = EDProduct.Componentes;
                db.Products.Add(Product);
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

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductoId,NombreProducto,MinStock,MaxStock,Descripcion,Componentes")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductoId,NombreProducto,MinStock,MaxStock,Descripcion,Componentes")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
