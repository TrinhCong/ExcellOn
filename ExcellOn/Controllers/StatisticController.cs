using System;
using System.Collections.Generic;
using System.Linq;
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
    public class StatisticController : BaseController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IServiceOrderRepository _serviceOrderRepository;
        public StatisticController(
                                IDbFactory dbFactory,
                                OrderRepository orderRepository,
                                ServiceOrderRepository serviceOrderRepository
                                ) : base(dbFactory)
        {
            _orderRepository = orderRepository;
            _serviceOrderRepository = serviceOrderRepository;
        }

        public ActionResult UnresolvedProductOrders()
        {

            return View();
        }

        public JsonResult GetUnresolvedProductOrders()
        {
            var condtion = $"{Sql.Table<Order>()}.{nameof(Order.status)}={EnumOrderStatus.UNRESOLVED}";
            return Json(new ResponseInfo(success: true, data: _orderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SucccessProductOrders()
        {
            return View();
        }

        public JsonResult GetSucccessProductOrders()
        {
            var condtion = $"{Sql.Table<Order>()}.{nameof(Order.status)}={EnumOrderStatus.SUCCESS}";
            return Json(new ResponseInfo(success: true, data: _orderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmedProductOrders()
        {
            return View();
        }

        public JsonResult GetConfirmedProductOrders()
        {
            var condtion = $"{Sql.Table<Order>()}.{nameof(Order.status)}={EnumOrderStatus.CONFIRMED}";
            return Json(new ResponseInfo(success: true, data: _orderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelledProductOrders()
        {
            return View();
        }

        public JsonResult GetCancelledProductOrders()
        {
            var condtion = $"{Sql.Table<Order>()}.{nameof(Order.status)}={EnumOrderStatus.CANCELLED}";
            return Json(new ResponseInfo(success: true, data: _orderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }



        public ActionResult UnresolvedServiceOrders()
        {

            return View();
        }

        public JsonResult GetUnresolvedServiceOrders()
        {
            var condtion = $"{Sql.Table<ServiceOrder>()}.{nameof(ServiceOrder.status)}={EnumOrderStatus.UNRESOLVED}";
            return Json(new ResponseInfo(success: true, data: _serviceOrderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SucccessServiceOrders()
        {
            return View();
        }

        public JsonResult GetSucccessServiceOrders()
        {
            var condtion = $"{Sql.Table<ServiceOrder>()}.{nameof(ServiceOrder.status)}={EnumOrderStatus.SUCCESS}";
            return Json(new ResponseInfo(success: true, data: _serviceOrderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmedServiceOrders()
        {
            return View();
        }

        public JsonResult GetConfirmedServiceOrders()
        {
            var condtion = $"{Sql.Table<ServiceOrder>()}.{nameof(ServiceOrder.status)}={EnumOrderStatus.CONFIRMED}";
            return Json(new ResponseInfo(success: true, data: _serviceOrderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelledServiceOrders()
        {
            return View();
        }

        public JsonResult GetCancelledServiceOrders()
        {
            var condtion = $"{Sql.Table<ServiceOrder>()}.{nameof(ServiceOrder.status)}={EnumOrderStatus.CANCELLED}";
            return Json(new ResponseInfo(success: true, data: _serviceOrderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }
    }
}