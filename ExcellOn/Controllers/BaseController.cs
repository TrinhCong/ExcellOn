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
    }
}