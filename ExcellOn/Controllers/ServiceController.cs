﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcellOn.Repositories;
using ExcellOn.Repositories.Sessions;
using Smooth.IoC.UnitOfWork.Interfaces;
using ExcellOn.Models;
using PagedList;
using Dapper;
using ExcellOn.ViewModels;

namespace ExcellOn.Controllers
{
    public class ServiceController : BaseController
    {
        private readonly ServiceRepository _serviceRepository;
        private readonly CategoryRepository<CategoryService> _categoryServiceRepository;
        public ServiceController(
                                IDbFactory dbFactory, 
                                ServiceRepository serviceRepository,
                                CategoryRepository<CategoryService> categoryServiceRepository
                                ) : base(dbFactory)
        {
            _serviceRepository = serviceRepository;
            _categoryServiceRepository = categoryServiceRepository;
        }


        public ActionResult GetAllServiceCategories()
        {
            var items = _categoryServiceRepository.GetAllServiceCategories();
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteServiceCategory(int id)
        {
            using (var session = GetSession())
            {
                try
                {
                    session.Query("Delete from cat_services where id=" + id);
                    return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete Service Categories fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }


        [HttpPost]
        public ActionResult CreateOrUpdateServiceCategory(CategoryService entity)
        {
            using (var session = GetSession())
            {
                using (var uow = session.UnitOfWork())
                {
                    try
                    {
                        if (!_categoryServiceRepository.IsCategoryServiceExist(entity))
                        {
                            _categoryServiceRepository.SaveOrUpdate(entity, uow);
                            return Json(new ResponseInfo(true, "Update service category successfully!"), JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new ResponseInfo(false, "Dupplicate name!"), JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        return Json(new ResponseInfo(false, "Update service category fail!"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }
        //============== Service===================
        public ActionResult GetAllService()
        {
            var items = _serviceRepository.GetAllService();
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);

        }
        public ActionResult DeleteService(int id)
        {
            using (var session = GetSession())
            {
                try
                {
                    session.Query("Delete from services where id=" + id);
                    return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete Service fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }


        [HttpPost]
        public ActionResult CreateOrUpdateService(Service entity)
        {
            using (var session = GetSession())
            {
                using (var uow = session.UnitOfWork())
                {
                    try
                    {
                        if (!_serviceRepository.IsServiceExist(entity))
                        {
                            _serviceRepository.SaveOrUpdate(entity, uow);
                            return Json(new ResponseInfo(true, "Update service successfully!"), JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new ResponseInfo(false, "Dupplicate name!"), JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        return Json(new ResponseInfo(false, "Update service fail!"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        // GET: Service

        public ActionResult Index(int catId = 0)
        {
            if (catId == 0)
                ViewBag.Services = _serviceRepository.GetAllService();
            else
                ViewBag.Services = _serviceRepository.GetServiceByCategoryId(catId);
            ViewBag.Categories = _categoryServiceRepository.GetAllServiceCategories();
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
                try
                {
                    _serviceRepository.DeleteKey(id, session);
                    return Json(new ResponseInfo(true,"Deleted complete!"), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false,"Deleted fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }

    }
}