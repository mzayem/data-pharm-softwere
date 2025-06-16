using System;
using System.Linq;
using System.Web.UI.WebControls;
using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System.Data.Entity;

namespace data_pharm_softwere.Pages.Group
{
    public partial class CreateGroup : System.Web.UI.Page
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
                var vendors = _context.Vendors.ToList();
                ddlVendor.DataSource = vendors;
                ddlVendor.DataTextField = "Name";
                ddlVendor.DataValueField = "VendorID";
                ddlVendor.DataBind();

                ddlVendor.Items.Insert(0, new ListItem("-- Select Vendor --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading vendors: " + ex.Message;
                lblMessage.CssClass = "text-danger fw-semibold";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var group = new Models.Group
                    {
                        Name = txtName.Text.Trim(),
                        VendorID = int.Parse(ddlVendor.SelectedValue),
                        CreatedAt = DateTime.Now
                    };

                    _context.Groups.Add(group);
                    _context.SaveChanges();

                    lblMessage.CssClass = "text-success fw-semibold";
                    lblMessage.Text = "Group saved successfully.";
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
            ddlVendor.SelectedIndex = 0;
        }
    }
}