﻿namespace Mango.Web.Utlities
{
    public class SD
    {
        public static string CouponAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string Token = "JWTToken"; 


        public enum ApiType
        {
            GET,
            POST, 
            PUT,
            DELETE

        }
    }
}
