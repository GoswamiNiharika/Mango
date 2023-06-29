using Mango.Web.Resources;
using System.Reflection.Metadata;

namespace Mango.Web.Utility
{
    public static class StaticDetails
    {
        public static string CouponAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string ProductAPIBase { get; set; }

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
