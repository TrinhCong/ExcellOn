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
    public class AdminController : BaseController
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly CategoryRepository<CategoryProduct> _categoryProductRepository;
        private readonly CategoryRepository<CategoryService> _categoryServiceRepository;
        private readonly ServiceRepository _serviceRepository;
        private readonly DepartmentRepository _departmentRepository;
        public AdminController(
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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User entity)
        {
            var employee = _employeeRepository.Login(new Employee { user_name = entity.user_name, password = entity.password });
            var customer = _customerRepository.Login(new Customer { user_name = entity.user_name, password = entity.password });
            if (employee != null)
            {
                setEmployeeSession(employee);
                return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
            }
            else if (customer != null)                     
            {
                setCustomerSession(customer);
                return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
            }
            return Json(new ResponseInfo(false, "Wrong user name or password!"), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CustomerDetail(int customerId)
        {
            var customer = _customerRepository.GetCustomerById(customerId);
            return View(customer);
        }
        [HttpGet]
        public ActionResult EmployeeDetail(int employeeId)
        {
            var customer = _employeeRepository.GetEmployeeById(employeeId);
            return View(customer);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer entity)
        {
            using (var session = GetSession())
            {
                if (_customerRepository.IsExist(entity))
                {
                    return Json(new ResponseInfo(false, "Dupplicated user name! Please try enter another!"), JsonRequestBehavior.AllowGet);
                }
                var customer = _customerRepository.Register(entity);
                if (customer != null)
                {
                    setCustomerSession(customer);
                    return Json(new ResponseInfo(true, "Register successfully!"), JsonRequestBehavior.AllowGet);
                }
                return Json(new ResponseInfo(false, "Register fail!"), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Logout()
        {
            clearCustomerSession();
            clearEmployeeSession();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Employee()
        {
            return View();
        }
        public ActionResult Customer()
        {
            return View();
        }
        public ActionResult Department()
        {
            return View();
        }
        public ActionResult ServiceCategory()
        {
            return View();
        }
        public ActionResult Service()
        {
            ViewBag.Categories = _categoryServiceRepository.GetAllServiceCategories();
            ViewBag.Services = _serviceRepository.GetItems();
            return View();
        }
        public ActionResult ProductCategory()
        {
            return View();
        }
        public ActionResult Product()
        {
            
            ViewBag.Categories = _categoryProductRepository.GetAllProductCategories();
            return View();
        }
        public ActionResult UnAuthorized()
        {
            return View();
        }

        public ActionResult ServiceOrderNoCare()
        {
            return View();
        }

        public ActionResult ServiceOrderCare()
        {
            return View();
        }

    }
}