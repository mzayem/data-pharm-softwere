using System;
using System.Linq;
using System.Web.UI.WebControls;
using data_pharm_softwere.Data;
using data_pharm_softwere.Models;

namespace data_pharm_softwere.Pages.Group
{
    public partial class EditGroup : System.Web.UI.Page
    {
        private DataPharmaContext _context = new DataPharmaContext();
        private int GroupId => Convert.ToInt32(Request.QueryString["id"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadVendors();
                LoadGroup();
            }
        }

        private void LoadVendors()
        {
            var vendors = _context.Vendors.ToList();
            ddlVendor.DataSource = vendors;
            ddlVendor.DataTextField = "Name";
            ddlVendor.DataValueField = "VendorID";
            ddlVendor.DataBind();
            ddlVendor.Items.Insert(0, new ListItem("-- Select Vendor --", ""));
        }

        private void LoadGroup()
        {
            var group = _context.Groups.FirstOrDefault(v => v.GroupID == GroupId);
            if (group == null)
            {
                Response.Redirect("/group");
                return;
            }

            txtName.Text = group.Name;
            ddlVendor.SelectedValue = group.VendorID.ToString();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                var group = _context.Groups.FirstOrDefault(v => v.GroupID == GroupId);
                if (group == null)
                {
                    Response.Redirect("/group");
                    return;
                }

                group.Name = txtName.Text.Trim();
                group.VendorID = int.Parse(ddlVendor.SelectedValue);

                _context.SaveChanges();
                Response.Redirect("/group");
            }
        }
    }
}