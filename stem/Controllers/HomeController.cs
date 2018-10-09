using Dhand.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace stem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string a = Encrypt.DesDecrypt("B1F358FBE29C638F", "heiban");

            return Json(a, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}