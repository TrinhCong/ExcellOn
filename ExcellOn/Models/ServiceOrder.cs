using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcellOn.Models
{
    [Table("service_orders", Schema = "dbo")]
    public class ServiceOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [ForeignKey(nameof(customer))]
        public int customer_id { get; set; }
        [ForeignKey(nameof(service))]
        public int service_id { get; set; }
        public DateTime registered_date { get; set; }
        public DateTime expired_date { get; set; }
        public DateTime finished_pay_date { get; set; }
        public int is_paid { get; set; }
        public string message { get; set; }
        [ForeignKey(nameof(employee))]
        public int employee_id { get; set; }

        [NotMapped]
        public virtual Customer customer { get; set; }

        [NotMapped]
        public virtual Service service { get; set; }

        [NotMapped]
        public virtual Employee employee { get; set; }
    }

     
}