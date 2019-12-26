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
using ExcellOn.ViewModels;
using Dapper;
using System.IO;
using Dapper.FastCrud;

namespace ExcellOn.Controllers
{
    public class ServiceController : BaseController
    {
        private readonly ServiceRepository _serviceRepository;
        private readonly CategoryRepository<CategoryService> _categoryServiceRepository;


        public ServiceController(
                                IDbFactory dbFactory,
                                ServiceRepository serviceRepository,
                                CategoryRepository<CategoryService> categoryRepository
                                ) : base(dbFactory)
        {
            _serviceRepository = serviceRepository;
            _categoryServiceRepository = categoryRepository;
        }



        public ActionResult GetAllServiceCategories()
        {
            var items = _categoryServiceRepository.GetAllServiceCategories();
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllServices()
        {
            var service = _serviceRepository.GetAllServices();
            return Json(new ResponseInfo(success: true, data: service), JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteCategory(int id)
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
                    return Json(new ResponseInfo(false, "Delete category fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }


        [HttpPost]
        public ActionResult CreateOrUpdateCategory(CategoryService entity)
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
                            return Json(new ResponseInfo(true, "Update category successfully!"), JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new ResponseInfo(false, "Dupplicate name!"), JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        return Json(new ResponseInfo(false, "Update category fail!"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        // GET: Service

        public ActionResult Index()
        {
            ViewBag.Categories = _categoryServiceRepository.GetAllServiceCategories();

            ViewBag.Services = _serviceRepository.GetAllServices();

            return View("Index");
        }
        public ActionResult Create()
        {
            return View();
        }

        //Creat or Update Service table
        [HttpPost]
        public ActionResult CreateOrUpdateService(Service entity, IEnumerable<HttpPostedFileBase> files)
        {
            using (var session = GetSession())
            {

                try
                {
                    if (!_serviceRepository.IsServiceExist(entity))
                    {
                        using (var uow = session.UnitOfWork())
                        { _serviceRepository.SaveOrUpdate(entity, uow); }


                        if (files != null && files.Count() > 0)
                        {
                            DeleteFiles(entity.id);
                            var directory = Server.MapPath("~/Content/uploads/services");
                            if (!Directory.Exists(directory))
                                Directory.CreateDirectory(directory);
                            foreach (var file in files)
                            {
                                string fileName = Path.GetFileName(string.Format("{0}{1}", DateTime.Now.Ticks.GetHashCode().ToString("x"), Path.GetExtension(file.FileName)));
                                file.SaveAs(directory + "/" + fileName);
                                session.Insert(new ServiceImage
                                {
                                    original_name = file.FileName,
                                    path = "/Content/uploads/services/" + fileName,
                                    service_id = entity.id
                                });
                            }

                        }
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
                var editItem = _serviceRepository.GetKey(id, session);

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
        [HttpGet]
        public ActionResult DeleteService(int id)
        {
            using (var session = GetSession())
            {
                try
                {
                    DeleteFiles(id);
                    session.Query("Delete from services where id=" + id);
                    return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete service fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }
        private void DeleteFiles(int serviceId)
        {
            using (var session = GetSession())
            {
                var existImages = session.Find<ServiceImage>(stm => stm.Where($"{nameof(ServiceImage.service_id)}={serviceId}"));
                if (existImages.Count() > 0)
                {
                    foreach (var image in existImages)
                    {
                        var file = Server.MapPath(image.path);
                        if (System.IO.File.Exists(file))
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                }
                session.Query("Delete from service_images where service_id=" + serviceId);
            }
        }

    }
}