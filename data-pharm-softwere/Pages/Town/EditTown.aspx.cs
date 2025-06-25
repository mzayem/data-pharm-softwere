using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using Org.BouncyCastle.Tls.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.WebRequestMethods;

namespace data_pharm_softwere.Pages.Town
{
    public partial class EditTown : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();
        private int TownId => int.TryParse(Request.QueryString["id"], out int id) ? id : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TownId == 0)
            {
                Response.Redirect("/town/create");
                return;
            }

            if (!IsPostBack)
            {
                LoadCityRoutes();
                LoadTown();
            }
        }

        private void LoadCityRoutes()
        {
            var cityRoutes = _context.CityRoutes.OrderBy(v => v.Name).ToList();
            ddlCityRoute.DataSource = cityRoutes;
            ddlCityRoute.DataTextField = "Name";
            ddlCityRoute.DataValueField = "CityRouteID";
            ddlCityRoute.DataBind();
            ddlCityRoute.Items.Insert(0, new ListItem("-- Select Routes --", ""));
        }

        protected void ddlCityRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            int.TryParse(ddlCityRoute.SelectedValue, out int vendorId);
        }

        private void LoadTown()
        {
            var town = _context.Towns.FirstOrDefault(b => b.TownID == TownId);
            if (town == null)
            {
                Response.Redirect("/town/");
                return;
            }

            ddlCityRoute.SelectedValue = town.CityRouteID.ToString();
            txtName.Text = town.Name;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var town = _context.Towns.FirstOrDefault(b => b.TownID == TownId);
            if (town == null)
            {
                Response.Redirect("/town/create");
                return;
            }

            try
            {
                town.CityRouteID = int.Parse(ddlCityRoute.SelectedValue);
                town.Name = txtName.Text.Trim();

                _context.SaveChanges();

                Response.Redirect("/town");
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }
    }
}