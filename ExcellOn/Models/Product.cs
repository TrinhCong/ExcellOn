using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcellOn.Models
{
    [Table("products", Schema = "dbo")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        [ForeignKey(nameof(category))]
        public int cat_id { get; set; }
        public string quantity_per_unit { get; set; }
        public int unit_price { get; set; }
        public int units_in_stock { get; set; }
        public string description { get; set; }
        public List<Category> ListCategory { get; set; }

        [NotMapped]
        public virtual CategoryProduct category { get; set; }
    }


}