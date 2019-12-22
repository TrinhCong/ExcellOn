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
        public int customer_id { get; set; }
        public int service_id { get; set; }
        public int registered_date { get; set; }
        public int expirated_date { get; set; }
        public int finished_pay_date { get; set; }
        public int is_paid { get; set; }
        public string message { get; set; }
        public int employee_id { get; set; }
    }

     
}