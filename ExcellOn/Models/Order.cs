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

        [NotMapped]
        public virtual User user { get; set; }

    }

     
}