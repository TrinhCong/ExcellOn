using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcellOn.Models
{
    [Table("order_details", Schema = "dbo")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [ForeignKey(nameof(order))]
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public double discount { get; set; }
        [NotMapped]
        public virtual Order order { get; set; }

    }

     
}