using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcellOn.Repositories;
using ExcellOn.Repositories.Sessions;
using Smooth.IoC.UnitOfWork.Interfaces;

namespace ExcellOn.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(
                                IDbFactory dbFactory
                                ) : base(dbFactory)
        {

        }

        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Login()
        {

            return View();
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
        public JsonResult GetData()
        {
            using (var session = _dbFactory.Create<IAppSession>())
            {
                return Json(new { xhr = ""}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}