using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcellOn.Models
{
    [Table("employee_services", Schema = "dbo")]
    public class EmployeeService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [ForeignKey(nameof(service))]
        public int service_id { get; set; }
        public DateTime updated_date { get; set; }
        [NotMapped]
        public virtual Service service { get; set; }
    }
}