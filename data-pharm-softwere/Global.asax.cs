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

            //vendor routes
            routes.MapPageRoute(
                "Create Vendor",
                "vendor/create",
                "~/pages/Vendor/CreateVendor.aspx"
            );
            routes.MapPageRoute(
                "Edit Vendor",
                "vendor/edit",
                "~/pages/Vendor/EditVendor.aspx"
            );
            routes.MapPageRoute(
                "Vendor Management",
                "vendor",
                "~/pages/Vendor/VendorPage.aspx"
            );

            //group routes
            routes.MapPageRoute(
                "Create Group",
                "group/create",
                "~/pages/Group/CreateGroup.aspx"
            );
            routes.MapPageRoute(
                " Edit Group",
                "group/edit",
                "~/pages/Group/EditGroup.aspx"
            );
            routes.MapPageRoute(
                "Group Management",
                "group",
                "~/pages/Group/GroupPage.aspx"
            );

            //subgroup routes
            routes.MapPageRoute(
                "Create SubGroup",
                "subgroup/create",
                "~/pages/SubGroup/CreateSubGroup.aspx"
            );
            routes.MapPageRoute(
                "Edit SubGroup",
                "subgroup/edit",
                "~/pages/SubGroup/EditSubGroup.aspx"
            );
            routes.MapPageRoute(
                "SubGroup Management",
                "subgroup",
                "~/pages/SubGroup/SubGroupPage.aspx"
            );

            //product routes
            routes.MapPageRoute(
                "Product Create",
                "product/create",
                "~/pages/Product/CreateProduct.aspx"
            );
            routes.MapPageRoute(
                "Product Edit",
                "product/edit",
                "~/pages/Product/EditProduct.aspx"
            );
            routes.MapPageRoute(
                "Product Management",
                "product",
                "~/pages/Product/ProductPage.aspx"
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