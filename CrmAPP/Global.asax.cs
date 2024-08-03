using CrmAPP.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace CrmAPP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Erzwingen Sie die Authentifizierung für alle Seiten außer der Login-Seite
            // -Login sayfas? d???nda tüm sayfalarda kimlik do?rulamas?n? zorunlu k?l
            //GlobalFilters.Filters.Add(new AuthorizeAttribute());

            GlobalFilters.Filters.Add(new System.Web.Mvc.AuthorizeAttribute());

            // API’yi kaydet
            GlobalConfiguration.Configure(WebRestApiConfig.Register);
         

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
