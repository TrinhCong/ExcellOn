using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper.FastCrud;
using ExcellOn.Models;
using ExcellOn.Repositories;
using ExcellOn.Repositories.Sessions;
using ExcellOn.ViewModels;
using Smooth.IoC.UnitOfWork.Interfaces;

namespace ExcellOn.Controllers
{
    public class StatisticController : BaseController
    {
        private readonly IOrderRepository _orderRepository;
        public StatisticController(
                                IDbFactory dbFactory,
                                OrderRepository orderRepository
                                ) : base(dbFactory)
        {
            _orderRepository = orderRepository;
        }

        public ActionResult UnresolvedProductOrders()
        {

            return View();
        }

        public JsonResult GetUnresolvedProductOrders()
        {
            var condtion = $"{Sql.Table<Order>()}.{nameof(Order.is_cancelled)}<>1 AND {Sql.Table<Order>()}.{nameof(Order.employee_id)} is null";
            return Json(new ResponseInfo(success: true, data: _orderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SucccessProductOrders()
        {
            return View();
        }

        public JsonResult GetSucccessProductOrders()
        {
            var condtion = $"{Sql.Table<Order>()}.{nameof(Order.shipped_date)} is not null AND {Sql.Table<Order>()}.{nameof(Order.employee_id)}={_employee.id}";
            return Json(new ResponseInfo(success: true, data: _orderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmedProductOrders()
        {
            return View();
        }

        public JsonResult GetConfirmedProductOrders()
        {
            var condtion = $"{Sql.Table<Order>()}.{nameof(Order.is_cancelled)}<>1 AND {Sql.Table<Order>()}.{nameof(Order.employee_id)} ={_employee.id} AND {Sql.Table<Order>()}.{nameof(Order.shipped_date)} is null ";
            return Json(new ResponseInfo(success: true, data: _orderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelledProductOrders()
        {
            return View();
        }

        public JsonResult GetCancelledProductOrders()
        {
            var condtion = $"{Sql.Table<Order>()}.{nameof(Order.is_cancelled)}=1 AND {Sql.Table<Order>()}.{nameof(Order.employee_id)}={_employee.id}";
            return Json(new ResponseInfo(success: true, data: _orderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }

    }
}