using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcellOn.Models
{
    [Table("departments", Schema = "dbo")]
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        [ForeignKey(nameof(category))]
        public int cat_id { get; set; }
        public string description { get; set; }

        [NotMapped]
        public virtual CategoryDepartment category { get; set; }
    }

     
}