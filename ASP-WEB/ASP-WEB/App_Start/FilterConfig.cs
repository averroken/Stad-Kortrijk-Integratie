using System.Web;
using System.Web.Mvc;

namespace ASP_WEB
{
#pragma warning disable 1591
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
