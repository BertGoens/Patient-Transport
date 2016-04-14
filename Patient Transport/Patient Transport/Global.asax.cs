using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using Patient_Transport.Models.Application;

namespace Patient_Transport {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Start Singleton (Config)
            var pt = Models.Application.PT.Instance;
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e) {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null) {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializer = new JavaScriptSerializer();

                var serializeModel = serializer.Deserialize<PatientTransportPrincipalSerializeModel>(authTicket.UserData);

                var newUser = new PatientTransportPrincipal(authTicket.Name);
                newUser.Guid = serializeModel.Guid;
                newUser.EmployeeId = serializeModel.EmployeeId;
                newUser.Roles = serializeModel.Roles;
                newUser.FirstName = serializeModel.FirstName;
                newUser.LastName = serializeModel.LastName;

                HttpContext.Current.User = newUser;
            }
        }
    }
}