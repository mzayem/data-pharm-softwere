using data_pharm_softwere.Data;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Town
{
    public partial class CreateTown : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCityRoutes();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var town = new Models.Town
                    {
                        CityRouteID = int.Parse(ddlCityRoute.SelectedValue),
                        Name = txtName.Text.Trim(),
                        CreatedAt = DateTime.Now,
                    };

                    _context.Towns.Add(town);
                    _context.SaveChanges();

                    lblMessage.Text = "Town saved successfully.";
                    lblMessage.CssClass = "alert alert-success mt-3";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "alert alert-danger mt-3";
                }
                finally
                {
                    txtName.Text = "";
                    ddlCityRoute.ClearSelection();
                }
            }
        }
    }
}