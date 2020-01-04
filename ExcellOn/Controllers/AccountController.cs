using ExcellOn.Models;
using ExcellOn.Repositories;
using ExcellOn.ViewModels;
using Smooth.IoC.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExcellOn.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly CategoryRepository<CategoryProduct> _categoryProductRepository;
        private readonly CategoryRepository<CategoryService> _categoryServiceRepository;
        private readonly ServiceRepository _serviceRepository;
        private readonly DepartmentRepository _departmentRepository;
        public AccountController(
                                IDbFactory dbFactory,
                                CustomerRepository customerRepository,
                                CategoryRepository<CategoryProduct> categoryProductRepository,
                                CategoryRepository<CategoryService> categoryServiceRepository,
                                ServiceRepository serviceRepository,
                                DepartmentRepository departmentRepository,
                                EmployeeRepository employeeRepository
                                ) : base(dbFactory)
        {
            _customerRepository = customerRepository;
            _categoryProductRepository = categoryProductRepository;
            _categoryServiceRepository = categoryServiceRepository;
            _departmentRepository = departmentRepository;
            _serviceRepository = serviceRepository;
            _employeeRepository = employeeRepository;
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User entity)
        {
            var customer = _customerRepository.Login(new Customer { user_name = entity.user_name, password = entity.password });
            if (customer != null)
            {
                setCustomerSession(customer);
                if (Session["ReturnUrl"] != null)
                {
                    var path = Session["ReturnUrl"].ToString();
                    return Json(new ResponseInfo(true, data: path), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new ResponseInfo(false, "Wrong user name or password!"), JsonRequestBehavior.AllowGet);
        }
    }
}