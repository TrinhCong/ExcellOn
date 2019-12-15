using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Dapper.FastCrud;
using ExcellOn.Enums;
using ExcellOn.Models;
using ExcellOn.Repositories;
using ExcellOn.Repositories.Sessions;
using ExcellOn.ViewModels;
using Smooth.IoC.UnitOfWork.Interfaces;

namespace ExcellOn.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly CategoryRepository<CategoryDepartment> _categoryRepository;
        public DepartmentController(
                                IDbFactory dbFactory,
                                DepartmentRepository departmentRepository,
                                CategoryRepository<CategoryDepartment> categoryRepository
                                ) : base(dbFactory)
        {
            _departmentRepository = departmentRepository;
            _categoryRepository = categoryRepository;
        }


        public ActionResult GetAllCategories()
        {
            var items = _categoryRepository.GetAllDepartmentCategories();
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CreateOrUpdateCategory(CategoryDepartment entity)
        {
            using (var session = GetSession())
            {
                using (var uow = session.UnitOfWork())
                {
                    try
                    {
                        if (!_categoryRepository.IsCategoryDepartmentExist(entity))
                        {
                            _categoryRepository.SaveOrUpdate(entity, uow);
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

    }
}