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
       
    }
}