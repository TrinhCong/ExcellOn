using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExcellOn.Enums;

namespace ExcellOn.Models
{
    [Table("customers", Schema = "dbo")]
    public class Customer:User
    {

    }
}