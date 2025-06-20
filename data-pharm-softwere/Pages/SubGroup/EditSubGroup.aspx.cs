using System;
using System.Linq;
using System.Web.UI.WebControls;
using data_pharm_softwere.Data;
using data_pharm_softwere.Models;

namespace data_pharm_softwere.Pages.SubGroup
{
    public partial class EditSubGroup : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        private int SubGroupId => int.TryParse(Request.QueryString["id"], out int id) ? id : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;

                LoadVendors();

                if (SubGroupId > 0)
                {
                    LoadSubGroup();
                }
                else
                {
                    Response.Redirect("/subgroup/create/");
                }
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
                var groupsQuery = _context.Groups.AsQueryable();

                if (vendorId.HasValue && vendorId.Value > 0)
                {
                    groupsQuery = groupsQuery.Where(g => g.VendorID == vendorId.Value);
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

        private void LoadSubGroup()
        {
            try
            {
                var subGroup = _context.SubGroups.Find(SubGroupId);

                if (subGroup == null)
                {
                    Response.Redirect("/subgroup/create/");
                    return;
                }

                txtName.Text = subGroup.Name;

                // Load groups for this vendor first
                var group = _context.Groups.FirstOrDefault(g => g.GroupID == subGroup.GroupID);
                if (group != null)
                {
                    ddlVendor.SelectedValue = group.VendorID.ToString();
                    LoadGroupsByVendor(group.VendorID);
                    ddlGroup.SelectedValue = subGroup.GroupID.ToString();
                }
                else
                {
                    LoadGroupsByVendor();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading subgroup: " + ex.Message;
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
                LoadGroupsByVendor();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var subGroup = _context.SubGroups.Find(SubGroupId);

                    if (subGroup == null)
                    {
                        lblMessage.Text = "SubGroup not found.";
                        lblMessage.CssClass = "text-danger fw-semibold";
                        return;
                    }

                    subGroup.Name = txtName.Text.Trim();
                    subGroup.GroupID = int.Parse(ddlGroup.SelectedValue);

                    _context.SaveChanges();

                    Response.Redirect("/subgroup");
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error updating SubGroup: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-semibold";
                }
            }
        }
    }
}