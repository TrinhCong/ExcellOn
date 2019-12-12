using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcellOn.Models
{
    [Table("user_services", Schema = "dbo")]
    public class UserService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string user_id { get; set; }
        public int service_id { get; set; }
        public DateTime? registered_date { get; set; }
        public DateTime? expired_date { get; set; }
        public DateTime? finish_pay_date { get; set; }
        public bool is_paid { get; set; }
        public bool confirmed { get; set; }

        [NotMapped]
        public virtual CategoryService category { get; set; }
    }

     
}