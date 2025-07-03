using System;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;
using data_pharm_softwere.Data;
using data_pharm_softwere.Models;

namespace data_pharm_softwere.Pages.SubGroup
{
    public partial class CreateSubGroup : System.Web.UI.Page
    {
        private DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;
                LoadVendors();
                LoadGroupsByVendor(); // Load all groups initially
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

        private void LoadGroupsByVendor(int? vendorId = null)
        {
            try
            {
                var groupsQuery = _context.Groups.Include("Division").AsQueryable();

                if (vendorId.HasValue && vendorId.Value > 0)
                {
                    groupsQuery = groupsQuery.Where(g => g.Division.VendorID == vendorId.Value);
                }

                var groups = groupsQuery.ToList();

                ddlGroup.DataSource = groups;
                ddlGroup.DataTextField = "Name";
                ddlGroup.DataValueField = "GroupID";
                ddlGroup.DataBind();

                ddlGroup.Items.Insert(0, new ListItem("-- Select Group --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading groups: " + ex.Message;
                lblMessage.CssClass = "text-danger fw-semibold";
            }
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlVendor.SelectedValue, out int vendorId) && vendorId > 0)
            {
                LoadGroupsByVendor(vendorId);
            }
            else
            {
                LoadGroupsByVendor(); // Load all groups if no vendor selected
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var subGroup = new Models.SubGroup
                    {
                        Name = txtName.Text.Trim(),
                        GroupID = int.Parse(ddlGroup.SelectedValue),
                        CreatedAt = DateTime.Now
                    };

                    _context.SubGroups.Add(subGroup);
                    _context.SaveChanges();

                    lblMessage.CssClass = "text-success fw-semibold";
                    lblMessage.Text = "Sub Group saved successfully.";
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
            LoadGroupsByVendor(); // Reset group list
        }
    }
}