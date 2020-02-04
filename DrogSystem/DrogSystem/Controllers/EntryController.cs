using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DrogSystem.Controllers
{
    public class EntryController : Controller
    {
        // GET: Entry
        public ActionResult Index()
        {
            return View();
        }
    }
}