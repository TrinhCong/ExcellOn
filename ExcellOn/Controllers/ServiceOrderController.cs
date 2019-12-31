using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcellOn.Repositories;
using ExcellOn.ViewModels;
using Smooth.IoC.UnitOfWork.Interfaces;

namespace ExcellOn.Controllers
{
    public class ServiceOrderController : BaseController
    {
        private readonly ServiceOrderRepository _serviceOrderRepository;
        public ServiceOrderController(IDbFactory dbFactory,
                                       ServiceOrderRepository serviceOrderRepository
            ) : base(dbFactory)
        {
            _serviceOrderRepository = serviceOrderRepository;
        }

        // GET: ServiceOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllNoCare()
        {
            var items = _serviceOrderRepository.getAllServiceNoCare();
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);
        }
    }
}