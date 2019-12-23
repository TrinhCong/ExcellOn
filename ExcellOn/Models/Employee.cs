using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcellOn.Models
{
    [Table("employees", Schema = "dbo")]
    public class Employee:User
    {
        [ForeignKey(nameof(role))]
        public int role_id { get; set; }

        [ForeignKey(nameof(department))]
        public int department_id { get; set; }


        [NotMapped]
        public virtual Department department { get; set; }
        [NotMapped]
        public virtual UserRole role { get; set; }
    }


}