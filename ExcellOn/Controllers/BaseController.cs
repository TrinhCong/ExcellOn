using ExcellOn.Models;
using ExcellOn.Repositories.Sessions;
using Smooth.IoC.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExcellOn.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IDbFactory _dbFactory;

        public BaseController(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IAppSession GetSession()
        {
            return _dbFactory.Create<IAppSession>();
        }

        public void setCustomerSession(Customer entity)
        {
            clearEmployeeSession();
            Session["Customer"] = entity;
        }

        public void clearCustomerSession()
        {
            Session["Customer"] = null;
        }
        public void setEmployeeSession(Employee entity)
        {
            clearCustomerSession();
            Session["Employee"] = entity;
        }
        public void clearEmployeeSession()
        {
            Session["Employee"] = null;
        }
        public Employee _employee => Session["Employee"] == null ? null : (Employee)Session["Employee"];
        public Customer customer => Session["Customer"] == null ? null : (Customer)Session["Customer"];
    }
}