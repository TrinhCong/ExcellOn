using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExcellOn.ViewModels
{
    public class ResponseInfo
    {
        public string message { get; set; }
        public bool success { get; set; }
        public object data { get; set; }
        public ResponseInfo(bool success=false, string message="",object data=null)
        {
            this.message = message;
            this.success = success;
            this.data = data;
        }
    }
}