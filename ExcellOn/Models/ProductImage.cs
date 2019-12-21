using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcellOn.Models
{
    [Table("product_images", Schema = "dbo")]
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string path { get; set; }
        public string original_name { get; set; }
        [ForeignKey(nameof(product))]
        public int product_id { get; set; }
        [NotMapped]
        public virtual Product product { get; set; }
    }
     
}