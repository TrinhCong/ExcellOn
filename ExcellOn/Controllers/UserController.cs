using System;
using System.Collections.Generic;
using System.IO;
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
            return Json(new ResponseInfo(success: true, data: _userRepository.GetItems()), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(User entity)
        {
            using (var session = GetSession())
            {
                using (var uow = session.UnitOfWork())
                {
                    try
                    {
                        string fileName = Path.GetFileName(string.Format("{0}{1}", DateTime.Now.Ticks.GetHashCode().ToString("x"), Path.GetExtension(entity.avatar.FileName)));
                        entity.avatar_path = Path.Combine(Server.MapPath("~/Content/uploads/avatars"), fileName);
                        entity.avatar.SaveAs(entity.avatar_path);
                        entity.avatar_path = fileName;
                        entity.hash_password = _userRepository.EncryptPassword(entity.password);
                        if (!_userRepository.IsExist(entity))
                        {
                            _userRepository.SaveOrUpdate(entity, uow);
                            return Json(new ResponseInfo(true, "Update user successfully!"));
                        }
                        else
                            return Json(new ResponseInfo(false, "Dupplicate user name!"));

                    }
                    catch (Exception e)
                    {
                        return Json(new ResponseInfo(false, "Update user fail!"));
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
                    _userRepository.DeleteKey(id, session);
                    return Json(new ResponseInfo(true));
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete user fail!"));
                }
            }

        }
    }
}