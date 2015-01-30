using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Domain.Abstract;


namespace InoReader.Controllers
{
    public class HomeController : Controller
    {
         [AllowAnonymous]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.Title = "InoReader - Все ссылки и RSS";
                return RedirectToAction("Index", "InoReader");
            }
            ViewBag.Title = "InoReader - Удобный менеджер ссылок и RSS новостей";
            return View("_SignInLayout");
        }
    }
}
