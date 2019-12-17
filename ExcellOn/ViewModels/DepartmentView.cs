using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExcellOn.ViewModels
{
    public class DepartmentView
    {
        public int id { get; set; }
        public string name { get; set; }
        public int cat_id { get; set; }
        public string catName { get; set; }
        public string description { get; set; }
    }
}