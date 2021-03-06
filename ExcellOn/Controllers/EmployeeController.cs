﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Dapper.FastCrud;
using ExcellOn.Attributes;
using ExcellOn.Enums;
using ExcellOn.Helpers;
using ExcellOn.Models;
using ExcellOn.Repositories;
using ExcellOn.Repositories.Sessions;
using ExcellOn.ViewModels;
using Smooth.IoC.UnitOfWork.Interfaces;

namespace ExcellOn.Controllers
{
    //[CustomAuthorize(Roles = EnumRoleName.SA)]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(
                                IDbFactory dbFactory,
                                EmployeeRepository employeeRepository
                                ) : base(dbFactory)
        {
            _employeeRepository = employeeRepository;
        }
        public ActionResult GetAll()
        {
            var employee = (Employee)Session["Employee"];
            string condition = null;
            if (employee != null)
            {
                condition = $"{Sql.Table<Employee>()}.{nameof(Employee.id)}<>{employee.id}";
            }
            var items = _employeeRepository.GetItems(condition);
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(Employee entity)
        {
            using (var session = GetSession())
            {
                using (var uow = session.UnitOfWork())
                {
                    try
                    {
                        if (entity.avatar != null)
                        {
                            string fileName = Path.GetFileName(string.Format("{0}{1}", DateTime.Now.Ticks.GetHashCode().ToString("x"), Path.GetExtension(entity.avatar.FileName)));
                            entity.avatar_path = Path.Combine(Server.MapPath("~/Content/uploads/avatars"), fileName);
                            entity.avatar.SaveAs(entity.avatar_path);
                            entity.avatar_path = fileName;
                        }
                        entity.hash_password = AuthHelper.EncryptPassword(entity.password);
                        if (!_employeeRepository.IsExist(entity))
                        {
                            _employeeRepository.SaveOrUpdate(entity, uow);
                            return Json(new ResponseInfo(true, "Update employee successfully!"), JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new ResponseInfo(false, "Dupplicate employee name!"), JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        return Json(new ResponseInfo(false, "Update employee fail!"), JsonRequestBehavior.AllowGet);
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
                    _employeeRepository.DeleteKey(id, session);
                    return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete employee fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }
    }
}