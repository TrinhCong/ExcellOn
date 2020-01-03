using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper.FastCrud;
using ExcellOn.Models;
using ExcellOn.Repositories;
using ExcellOn.ViewModels;
using Smooth.IoC.UnitOfWork.Interfaces;

namespace ExcellOn.Controllers
{
    public class ServiceOrderController : BaseController
    {
        private readonly ServiceOrderRepository _serviceOrderRepository;
        private readonly ServiceRepository _serviceRepository;
        public ServiceOrderController(IDbFactory dbFactory, ServiceRepository serviceRepository,
                                       ServiceOrderRepository serviceOrderRepository
            ) : base(dbFactory)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _serviceRepository = serviceRepository;
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

        [HttpPost]
        public ActionResult saveServiceOrder(string message, int serviceId)
        {
            try
            {
                using (var session = GetSession())
                {
                    var service = _serviceRepository.GetKey(serviceId, session);

                    var item = new ServiceOrder
                    {
                        customer_id = _customer.id,
                        registered_date = DateTime.Now,
                        expired_date = DateTime.Now.AddHours(service.hours),
                        finished_pay_date = DateTime.Now.AddDays(3),
                        message = message,
                        pay_type_id = 1
                    };
                    session.Insert(item);
                    return Json(new ResponseInfo(success: true), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception err)
            {
                return Json(new ResponseInfo(success: false), JsonRequestBehavior.AllowGet);
            }
        }
    }
}