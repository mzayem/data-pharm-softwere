using data_pharm_softwere.Data;
using System;

namespace data_pharm_softwere.Pages.CityRoute
{
    public partial class EditCityRoute : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        private int CityRouteID
        {
            get
            {
                int id;
                if (int.TryParse(Request.QueryString["id"], out id))
                {
                    return id;
                }
                return 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;

                if (CityRouteID > 0)
                {
                    LoadCityRoute();
                }
                else
                {
                    Response.Redirect("/city-route/create/");
                }
            }
        }

        private void LoadCityRoute()
        {
            try
            {
                var cityRoute = _context.CityRoutes.Find(CityRouteID);

                if (cityRoute == null)
                {
                    Response.Redirect("/city-route/create/");
                    return;
                }

                txtName.Text = cityRoute.Name;
                
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading subgroup: " + ex.Message;
                lblMessage.CssClass = "text-danger fw-semibold";
            }
        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var cityRoute = _context.CityRoutes.Find(CityRouteID);

                    if (cityRoute == null)
                    {
                        lblMessage.Text = "Route not found.";
                        lblMessage.CssClass = "text-danger fw-semibold";
                        return;
                    }

                    cityRoute.Name = txtName.Text.Trim();

                    _context.SaveChanges();

                    Response.Redirect("/city-route");
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error updating Route: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-semibold";
                }
            }
        }
    }
}