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
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        public UserController(
                                IDbFactory dbFactory,
                                UserRepository userRepository
                                ) : base(dbFactory)
        {
            _userRepository = userRepository;
        }
       
        public ActionResult GetAll()
        {
            return Json(new ResponseInfo(success: true, data: _userRepository.GetItems()),JsonRequestBehavior.AllowGet);
        }
    }
}