using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExcellOn.Enums
{
    public static class EnumRole
    {
        public const int SA = 1;
        public const int EMPLOYEE = 2;
        public const int CUSTOMER = 3;
    }
    public static class EnumRoleName
    {
        public const string SA = "sa";
        public const string EMPLOYEE = "employee";
        public const string CUSTOMER = "customer";
    }

    public static class EnumOrderStatus
    {
        public const int UNRESOLVED = 1;
        public const int CONFIRMED = 2;
        public const int SUCCESS = 3;
        public const int CANCELLED = 4;
    }
}