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


        public ActionResult GetAll()
        {
            var items = _departmentRepository.GetAll();
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllView()
        {
            var items = _departmentRepository.GetAllView();
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);
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

        public ActionResult Delete(int id)
        {
            using (var session = GetSession())
            {
                try
                {
                    _departmentRepository.DeleteKey(id, session);
                    return Json(new ResponseInfo(true, "Delete successfully!"), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete user fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }

    }
}