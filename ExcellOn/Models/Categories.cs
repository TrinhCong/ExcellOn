using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcellOn.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
    [Table("cat_products", Schema = "dbo")]
    public class CategoryProduct : Category
    {

    }
    [Table("cat_services", Schema = "dbo")]
    public class CategoryService : Category
    {

    }
    [Table("roles", Schema = "dbo")]
    public class UserRole : Category
    {

    }

}