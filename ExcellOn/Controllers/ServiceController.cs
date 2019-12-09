using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcellOn.Repositories;
using ExcellOn.Repositories.Sessions;
using Smooth.IoC.UnitOfWork.Interfaces;
using ExcellOn.Models;
using PagedList;

namespace ExcellOn.Controllers
{
    public class ServiceController : BaseController
    {
        private readonly ServiceRepository _serviceRepository;

        public ServiceController(
                                IDbFactory dbFactory, 
                                ServiceRepository serviceRepository
                                ) : base(dbFactory)
        {
            _serviceRepository = serviceRepository;

        }
        // GET: Service

        public ActionResult Index()
        {
                return View("Index");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Service service)
        {
            using (var session = _dbFactory.Create<IAppSession>())
            {
                
                return View();
            }

        }
        public ActionResult Details(int id)
        {
            using (var session = _dbFactory.Create<IAppSession>())
            {
                return View();
            }

        }
        public ActionResult Edit(int id)
        {
            using (var session = _dbFactory.Create<IAppSession>())

            {
                var editItem = this._serviceRepository.GetKey(id, session);

                if (editItem != null)
                {
                   return View(editItem);
                }
                else
                    return HttpNotFound();

            }

        }
        [HttpPost]
        public ActionResult Edit(Service service)
        {
            using (var session = _dbFactory.Create<IAppSession>())
            {
                return View();
            }

        }
        public ActionResult Delete(int id)
        {
            using (var session = _dbFactory.Create<IAppSession>())
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}