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



        public ActionResult UnresolvedServiceServiceOrders()
        {

            return View();
        }

        public JsonResult GetUnresolvedServiceServiceOrders()
        {
            var condtion = $"{Sql.Table<ServiceOrder>()}.{nameof(ServiceOrder.status)}={EnumOrderStatus.UNRESOLVED}";
            return Json(new ResponseInfo(success: true, data: _serviceOrderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SucccessServiceServiceOrders()
        {
            return View();
        }

        public JsonResult GetSucccessServiceServiceOrders()
        {
            var condtion = $"{Sql.Table<ServiceOrder>()}.{nameof(ServiceOrder.status)}={EnumOrderStatus.SUCCESS}";
            return Json(new ResponseInfo(success: true, data: _serviceOrderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmedServiceServiceOrders()
        {
            return View();
        }

        public JsonResult GetConfirmedServiceServiceOrders()
        {
            var condtion = $"{Sql.Table<ServiceOrder>()}.{nameof(ServiceOrder.status)}={EnumOrderStatus.CONFIRMED}";
            return Json(new ResponseInfo(success: true, data: _serviceOrderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelledServiceServiceOrders()
        {
            return View();
        }

        public JsonResult GetCancelledServiceServiceOrders()
        {
            var condtion = $"{Sql.Table<ServiceOrder>()}.{nameof(ServiceOrder.status)}={EnumOrderStatus.CANCELLED}";
            return Json(new ResponseInfo(success: true, data: _serviceOrderRepository.GetItems(condtion)), JsonRequestBehavior.AllowGet);
        }
    }
}