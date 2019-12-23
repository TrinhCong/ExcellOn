using ExcellOn.Enums;
using ExcellOn.Models;
using ExcellOn.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExcellOn.Attributes
{
    public class CustomAuthorizeAttribute:AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Customer customer = (Customer)HttpContext.Current.Session["Customer"];
            Employee employee = (Employee)HttpContext.Current.Session["Employee"];
            if (customer != null)
            {
                var roles = Roles.Split(',');
                if (roles.Contains(EnumRoleName.CUSTOMER))
                {
                    return true;
                }
            }
            else if(employee!=null)
            {
                var roles = Roles.Split(',');
                if (roles.Contains(employee.role.name))
                {
                    return true;
                }
            }
            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new JsonResult { JsonRequestBehavior=JsonRequestBehavior.AllowGet,Data=new ResponseInfo( authorized: false,success:false,message:"You do not have role to emplement this feature!",data:new object[] { }) };
        }
    }
}