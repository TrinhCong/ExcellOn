using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcellOn.Models
{
    [Table("users", Schema = "dbo")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string user_name { get; set; }
        public string hash_password { get; set; }
        [ForeignKey(nameof(role))]
        public int role_id { get; set; }
        public string display_name { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public int gender { get; set; }
        public string avatar_path { get; set; }
        public DateTime? date_of_birth { get; set; }

        [NotMapped]
        public string dob => date_of_birth != null ? ((DateTime)date_of_birth).ToString("dd/MM/yyyy") : string.Empty;

        [NotMapped]
        public string password { get; set; }
        [NotMapped]
        public string re_password { get; set; }
        [NotMapped]
        public virtual UserRole role { get; set; }
        [NotMapped]
        public HttpPostedFileBase avatar { get; set; }
    }


}