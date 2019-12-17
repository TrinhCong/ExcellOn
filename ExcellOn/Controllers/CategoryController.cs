using ExcellOn.Models;
using ExcellOn.Repositories;
using ExcellOn.Repositories.Sessions;
using Smooth.IoC.UnitOfWork.Interfaces;
using System.Web.Mvc;
using PagedList;
using ExcellOn.ViewModels;
using System;

namespace ExcellOn.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly CategoryRepository<CategoryService> _categoryServiceRepository;
        private readonly CategoryRepository<CategoryDepartment> _categoryDepartmentRepository;

        public CategoryController(
                                    IDbFactory dbFactory,
                                    CategoryRepository<CategoryService> categoryServiceRepository,
                                    CategoryRepository<CategoryDepartment> categoryDepartmentRepository
                                ) : base(dbFactory)
        {
            _categoryServiceRepository = categoryServiceRepository;
            _categoryDepartmentRepository = categoryDepartmentRepository;
        }

        public ActionResult Index(string Search, int? page)

        {

            using (var session = _dbFactory.Create<IAppSession>())
            {
                return View();
            }
        }

        public ActionResult GetAllDepartmentCategories()
        {
            var items = _categoryServiceRepository.GetAllDepartmentCategories();
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category newItem)
        {

            return View(newItem);
        }
        public ActionResult Get(int id)
        {
            return View();
        }
        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(Category category)
        {


            return View();
        }
        public ActionResult Delete(int id)
        {
            return View();
        }

        public ActionResult DeleteDepartment(int id)
        {
            using (var session = GetSession())
            {
                try
                {
                    _categoryDepartmentRepository.DeleteKey(id, session);
                    return Json(new ResponseInfo(true, "Delete successfully!"), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete user fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }

        [HttpPost]
        public ActionResult CreateOrUpdateDepartment(CategoryDepartment entity)
        {
            using (var session = GetSession())
            {
                using (var uow = session.UnitOfWork())
                {
                    try
                    {
                        if (!_categoryDepartmentRepository.IsDepartmentExist(entity))
                        {
                            _categoryDepartmentRepository.SaveOrUpdate(entity, uow);
                            return Json(new ResponseInfo(true, "Update Department successfully!"), JsonRequestBehavior.AllowGet);
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

    }
}