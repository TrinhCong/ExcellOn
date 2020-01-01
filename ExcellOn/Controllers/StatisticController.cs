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

        public ActionResult UnresolvedProductOrders()
        {

            return View();
        }
    }
}