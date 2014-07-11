using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TesoroQR.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "CODIGO ESCONDIDO";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Pagina de contacto";

            return View();
        }

        public ActionResult ComoJugar()
        {
            return View();
        }
    }
}