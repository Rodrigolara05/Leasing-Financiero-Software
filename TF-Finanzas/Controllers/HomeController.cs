using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TF_Finanzas.Authorization;

namespace TF_Finanzas.Controllers
{
    [UserLoggedOn]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Index_Admin()
        {
            return View();
        }
        public ActionResult Index_Cliente()
        {
            return View();
        }
    }
}