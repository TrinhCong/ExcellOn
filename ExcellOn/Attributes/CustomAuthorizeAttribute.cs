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
            User user = (User)HttpContext.Current.Session["User"];
            if (user != null)
            {
                var roles = Roles.Split(',');
                if (roles.Contains(user.role.name))
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