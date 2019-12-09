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
        private readonly IUserRepository _userRepository;
        public AdminController(
                                IDbFactory dbFactory,
                                UserRepository userRepository
                                ) : base(dbFactory)
        {
            _userRepository = userRepository;
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
           var user= _userRepository.Login(entity); 
            if (user != null)
            {
                setUserSession(user);
                return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
            }
            return Json(new ResponseInfo(false, "Sai tài khoản hoặc mật khẩu!"), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Detail(int userId)
        {
           var user= _userRepository.GetUserById(userId);
            return View(user);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User entity)
        {
            using(var session = GetSession())
            {
                if (_userRepository.IsExist(entity))
                {
                    return Json(new ResponseInfo(false, "Tên đăng nhập này đã tồn tại! Vui lòng thử một tên khác!"), JsonRequestBehavior.AllowGet);
                }
                var user = _userRepository.Register(entity);
                if (user!= null)
                {
                    setUserSession(user);
                    return Json( new ResponseInfo(true,"Đăng kí thành công!"),JsonRequestBehavior.AllowGet);
                }
                return Json(new ResponseInfo(false, "Đăng kí thất bại!"), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Logout()
        {
            clearUserSession();
            return RedirectToAction("Index","Home");
        }
        
        private void setUserSession(User entity)
        {
            Session["User"] = entity;
        }
        private void clearUserSession()
        {
            Session["User"] = null;
        }

    }
}