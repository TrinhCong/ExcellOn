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
        public DepartmentController(
                                IDbFactory dbFactory,
                                DepartmentRepository departmentRepository
                                ) : base(dbFactory)
        {
            _departmentRepository = departmentRepository;
        }
       
    }
}