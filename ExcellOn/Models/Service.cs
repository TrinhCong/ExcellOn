using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcellOn.Models
{
    [Table("services", Schema = "dbo")]
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        [ForeignKey(nameof(category))]
        public int cat_id { get; set; }
        public int price { get; set; }
        public double hours { get; set; }
        public string description { get; set; }

        [NotMapped]
        public virtual CategoryService category { get; set; }
    }

     
}