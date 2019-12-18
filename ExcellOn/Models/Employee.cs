using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcellOn.Models
{
    [Table("employees", Schema = "dbo")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        [ForeignKey(nameof(department))]
        public int department_id { get; set; }
        [ForeignKey(nameof(service))]
        public int service_id { get; set; }
        public string description { get; set; }

        [NotMapped]
        public virtual Department department { get; set; }
        [NotMapped]
        public virtual Service service { get; set; }
    }

     
}