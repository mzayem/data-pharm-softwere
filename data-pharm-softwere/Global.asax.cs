using System;
using System.Web;
using System.Web.Routing;
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

            //division routes
            routes.MapPageRoute(
                "Create Division",
                "division/create",
                "~/pages/Division/CreateDivision.aspx"
            );
            routes.MapPageRoute(
                " Edit Division",
                "division/edit",
                "~/pages/Division/EditDivision.aspx"
            );
            routes.MapPageRoute(
                "Division Management",
                "division",
                "~/pages/Division/DivisionPage.aspx"
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

            //Batch routes
            routes.MapPageRoute(
                "Batch Create",
                "batch/create",
                "~/pages/Batch/CreateBatch.aspx"
            );
            routes.MapPageRoute(
                "Batch Edit",
                "batch/edit",
                "~/pages/Batch/EditBatch.aspx"
            );
            routes.MapPageRoute(
                "Batch Management",
                "batch",
                "~/pages/Batch/BatchPage.aspx"
            );

            //CityRoute routes
            routes.MapPageRoute(
                "CityRoute Create",
                "city-route/create",
                "~/pages/CityRoute/CreateCityRoute.aspx"
            );
            routes.MapPageRoute(
                "CityRoute Edit",
                "city-route/edit",
                "~/pages/CityRoute/EditCityRoute.aspx"
            );
            routes.MapPageRoute(
                "CityRoute Management",
                "city-route",
                "~/pages/CityRoute/CityRoutePage.aspx"
            );

            //Town routes
            routes.MapPageRoute(
                "Town Create",
                "town/create",
                "~/pages/Town/CreateTown.aspx"
            );
            routes.MapPageRoute(
                "Town Edit",
                "town/edit",
                "~/pages/Town/EditTown.aspx"
            );
            routes.MapPageRoute(
                "Town Management",
                "town",
                "~/pages/Town/TownPage.aspx"
            );

            //Customer routes
            routes.MapPageRoute(
                "Customer Create",
                "customer/create",
                "~/pages/Customer/CreateCustomer.aspx"
            );
            routes.MapPageRoute(
                "Customer Edit",
                "customer/edit",
                "~/pages/Customer/EditCustomer.aspx"
            );
            routes.MapPageRoute(
                "Customer Management",
                "customer",
                "~/pages/Customer/CustomerPage.aspx"
            );

            //Salesman routes
            routes.MapPageRoute(
                "Salesman Create",
                "salesman/create",
                "~/pages/Salesman/CreateSalesman.aspx"
            );
            routes.MapPageRoute(
                "Salesman Edit",
                "salesman/edit",
                "~/pages/Salesman/EditSalesman.aspx"
            );
            routes.MapPageRoute(
                "Salesman Management",
                "salesman",
                "~/pages/Salesman/SalesmanPage.aspx"
            );

            //MedicalRep routes
            routes.MapPageRoute(
                "MedicalRep  Create",
                "medicalRep/create",
                "~/pages/MedicalRep/CreateMedicalRep.aspx"
            );
            routes.MapPageRoute(
                "MedicalRep Edit",
                "medicalRep/edit",
                "~/pages/MedicalRep/EditMedicalRep.aspx"
            );
            routes.MapPageRoute(
                "MedicalRep Management",
                "medicalRep",
                "~/pages/MedicalRep/MedicalRepPage.aspx"
            );

            //Purchase routes
            routes.MapPageRoute(
                "Purchase Create",
                "purchase/create",
                "~/pages/PurchaseForms/Purchase/CreatePurchase.aspx"
            );
            routes.MapPageRoute(
                "Purchase Edit",
                "purchase/edit",
                "~/pages/PurchaseForms/Purchase/EditPurchase.aspx"
            );
            routes.MapPageRoute(
                "Purchases Management",
                "purchase",
                "~/pages/PurchaseForms/Purchase/PurchasePage.aspx"
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