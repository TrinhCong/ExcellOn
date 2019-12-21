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
    public class EmployeeController : BaseController
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeController(
                                IDbFactory dbFactory,
                                EmployeeRepository employeeRepository

                                ) : base(dbFactory)
        {
            _employeeRepository = employeeRepository;

        }
        public ActionResult GetAllEmployee()
        {
            var items = _employeeRepository.GetAllEmployee();
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);

        }
        public ActionResult DeleteEmployee(int id)
        {
            using (var session = GetSession())
            {
                try
                {
                    session.Query("Delete from employees where id=" + id);
                    return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete employees fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }


        [HttpPost]
        public ActionResult CreateOrUpdateEmployee(Employee entity)
        {
            using (var session = GetSession())
            {
                using (var uow = session.UnitOfWork())
                {
                    try
                    {
                        if (!_employeeRepository.IsEmployeeExist(entity))
                        {
                            _employeeRepository.SaveOrUpdate(entity, uow);
                            return Json(new ResponseInfo(true, "Update Employee successfully!"), JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new ResponseInfo(false, "Dupplicate name!"), JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        return Json(new ResponseInfo(false, "Update Employee fail!"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }
    }
}