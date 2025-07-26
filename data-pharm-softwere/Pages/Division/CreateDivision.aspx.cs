using data_pharm_softwere.Data;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;

namespace data_pharm_softwere.Pages.Division
{
    public partial class CreateDivision : System.Web.UI.Page
    {
        private DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;
                LoadVendors();
            }
        }

        private void LoadVendors()
        {
            try
            {
                var vendors = _context.Vendors
               .Include(v => v.Account)
               .OrderBy(v => v.Account.AccountName)
               .Select(v => new
               {
                   v.AccountId,
                   AccountName = v.Account.AccountName
               })
               .ToList();

                ddlVendor.DataSource = vendors;
                ddlVendor.DataTextField = "AccountName";
                ddlVendor.DataValueField = "AccountId";
                ddlVendor.DataBind();
                ddlVendor.Items.Insert(0, new ListItem("-- Select Vendor --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading vendors: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var division = new Models.Division
                    {
                        Name = txtName.Text.Trim(),
                        AccountId = int.Parse(ddlVendor.SelectedValue),
                        CreatedAt = DateTime.Now
                    };

                    _context.Divisions.Add(division);
                    _context.SaveChanges();

                    lblMessage.CssClass = "alert alert-success mt-3";
                    lblMessage.Text = "Division saved successfully.";
                    ClearForm();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "alert alert-danger mt-3";
                }
            }
        }

        private void ClearForm()
        {
            txtName.Text = string.Empty;
            ddlVendor.SelectedIndex = 0;
        }
    }
}