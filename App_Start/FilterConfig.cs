using System.Web;
using System.Web.Mvc;

namespace ZeroHunger_Mid_
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
