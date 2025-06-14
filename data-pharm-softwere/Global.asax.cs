using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;

namespace data_pharm_softwere
{
    public class Global : HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
            ScriptResourceDefinition jQueryDefinition = new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery-3.7.1.min.js",
                DebugPath = "~/Scripts/jquery-3.7.1.js",
                CdnPath = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.7.1.min.js",
                CdnDebugPath = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.7.1.js",
                CdnSupportsSecureConnection = true
            };

            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", jQueryDefinition);
            RegisterRoutes(RouteTable.Routes);
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute(
                "Home",
                "",
                "~/Pages/Main.aspx"
            );
            routes.MapPageRoute(
                "Vendor Create",
                "vendor/create",
                "~/pages/Vendor/CreateVendor.aspx"
            );
            routes.MapPageRoute(
                "Vendor Edit",
                "vendor/edit",
                "~/pages/Vendor/EditVendor.aspx"
            );
            routes.MapPageRoute(
                "Vendor Management",
                "vendor",
                "~/pages/Vendor/VendorPage.aspx"
            );
        }

        // Default event hooks
        private void Session_Start(object sender, EventArgs e)
        { }

        private void Application_BeginRequest(object sender, EventArgs e)
        { }

        private void Application_AuthenticateRequest(object sender, EventArgs e)
        { }

        private void Application_Error(object sender, EventArgs e)
        { }

        private void Session_End(object sender, EventArgs e)
        { }

        private void Application_End(object sender, EventArgs e)
        { }
    }
}