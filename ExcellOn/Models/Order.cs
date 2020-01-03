using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcellOn.Models
{
    [Table("orders", Schema = "dbo")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [ForeignKey(nameof(user))]
        public int user_id { get; set; }
        public DateTime order_date { get; set; }
        public DateTime required_date { get; set; }
        public DateTime? shipped_date { get; set; }
        public double freight { get; set; }
        public string ship_address { get; set; }
        public string message { get; set; }
        public int is_cancelled { get; set; }
        public DateTime? cancel_date { get; set; }
        [ForeignKey(nameof(pay_type))]
        public int pay_type_id { get; set; }
        [ForeignKey(nameof(employee))]
        public int? employee_id { get; set; }
        [NotMapped]
        public virtual Customer user { get; set; }
        [NotMapped]
        public virtual Employee employee { get; set; }
        [NotMapped]
        public virtual CategoryPayType pay_type { get; set; }

    }

     
}