using System;
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
    //[CustomAuthorize(Roles =EnumRoleName.SA)]
    public class CustomerController : BaseController
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerController(
                                IDbFactory dbFactory,
                                CustomerRepository customerRepository
                                ) : base(dbFactory)
        {
            _customerRepository = customerRepository;
        }
        public ActionResult GetAll()
        {
            var customer = (Customer)Session["Customer"];
            string condition = null;
            if (customer != null)
            {
                condition = $"{Sql.Table<Customer>()}.{nameof(Models.Customer.id)}<>{customer.id}";
            }
            var items = _customerRepository.GetItems(condition);
            return Json(new ResponseInfo(success: true, data: items), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(Customer entity)
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
                        if (!_customerRepository.IsExist(entity))
                        {
                            _customerRepository.SaveOrUpdate(entity, uow);
                            return Json(new ResponseInfo(true, "Update customer successfully!"),JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new ResponseInfo(false, "Dupplicate customer name!"), JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception e)
                    {
                        return Json(new ResponseInfo(false, "Update customer fail!"), JsonRequestBehavior.AllowGet);
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
                    _customerRepository.DeleteKey(id, session);
                    return Json(new ResponseInfo(true), JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new ResponseInfo(false, "Delete customer fail!"), JsonRequestBehavior.AllowGet);
                }
            }

        }
    }
}