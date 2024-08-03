using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;

namespace CrmAPP.App_Start
{
    public class WebRestApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Attribute routing
            config.MapHttpAttributeRoutes();
            config.EnableCors(new EnableCorsAttribute("http://localhost:3000", "*", "*"));
            // (İstersen klasik route da ekleyebilirsin)
            // config.Routes.MapHttpRoute(
            //   name: "DefaultApi",
            //   routeTemplate: "api/{controller}/{id}",
            //   defaults: new { id = RouteParameter.Optional }
            // );
            // XML'i kapat
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            // JSON ayarları
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}