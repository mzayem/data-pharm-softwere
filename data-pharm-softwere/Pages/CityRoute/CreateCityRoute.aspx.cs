using System;
using data_pharm_softwere.Data;

namespace data_pharm_softwere.Pages.CityRoute
{
    public partial class CreateCityRoute : System.Web.UI.Page
    {
        private DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var cityRoute = new Models.CityRoute
                    {
                        Name = txtName.Text.Trim(),
                        CreatedAt = DateTime.Now
                    };

                    _context.CityRoutes.Add(cityRoute);
                    _context.SaveChanges();

                    lblMessage.CssClass = "text-success fw-semibold";
                    lblMessage.Text = "Route saved successfully.";
                    ClearForm();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-semibold";
                }
            }
        }

        private void ClearForm()
        {
            txtName.Text = string.Empty;
        }
    }
}