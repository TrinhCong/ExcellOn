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
using Dapper;
using ExcellOn.ViewModels;

namespace ExcellOn.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly DepartmentRepository _departmentRepository;
        private readonly CategoryRepository<CategoryDepartment> _categoryDepartmentRepository;
        public DepartmentController(
                                IDbFactory dbFactory,
                                DepartmentRepository departmentRepository,
                                CategoryRepository<CategoryDepartment> categoryDepartmentRepository
                                ) : base(dbFactory)
        {
            _departmentRepository = departmentRepository;
            _categoryDepartmentRepository = categoryDepartmentRepository;
        }


        public ActionResult GetAllDepartmentCategories()
        {
            var items = _categoryDepartmentRepository.GetAllDepartmentCategories();
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteDepartmentCategory(int id)
        {
            using (var session = GetSession())
            {
                try
                {
                    session.Query("Delete from cat_departments where id=" + id);
                    return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete Department Categories fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }


        [HttpPost]
        public ActionResult CreateOrUpdateDepartmentCategory(CategoryDepartment entity)
        {
            using (var session = GetSession())
            {
                using (var uow = session.UnitOfWork())
                {
                    try
                    {
                        if (!_categoryDepartmentRepository.IsCategoryDepartmentExist(entity))
                        {
                            _categoryDepartmentRepository.SaveOrUpdate(entity, uow);
                            return Json(new ResponseInfo(true, "Update department category successfully!"), JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new ResponseInfo(false, "Dupplicate name!"), JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        return Json(new ResponseInfo(false, "Update department category fail!"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }
        //============== Department===================
        public ActionResult GetAllDepartment()
        {
            var items = _departmentRepository.GetAllDepartment();
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);

        }
        public ActionResult DeleteDepartment(int id)
        {
            using (var session = GetSession())
            {
                try
                {
                    session.Query("Delete from departments where id=" + id);
                    return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete Department fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }


        [HttpPost]
        public ActionResult CreateOrUpdate(Department entity)
        {
            using (var session = GetSession())
            {
                using (var uow = session.UnitOfWork())
                {
                    try
                    {
                        if (!_departmentRepository.IsDepartmentExist(entity))
                        {
                            _departmentRepository.SaveOrUpdate(entity, uow);
                            return Json(new ResponseInfo(true, "Update department category successfully!"), JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new ResponseInfo(false, "Dupplicate name!"), JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        return Json(new ResponseInfo(false, "Update department category fail!"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        // GET: Department

        public ActionResult Index()
        {
            return View("Index");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Department department)
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
                var editItem = this._departmentRepository.GetKey(id, session);

                if (editItem != null)
                {
                    return View(editItem);
                }
                else
                    return HttpNotFound();

            }

        }
        [HttpPost]
        public ActionResult Edit(Department department)
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
                    _departmentRepository.DeleteKey(id, session);
                    return Json(new ResponseInfo(true, "Deleted complete!"), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Deleted fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }

    }
}