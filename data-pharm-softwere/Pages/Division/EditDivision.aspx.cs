using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Division
{
    public partial class EditDivision : System.Web.UI.Page
    {
        private DataPharmaContext _context = new DataPharmaContext();

        private int DivisionId
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
            if (DivisionId == 0)
            {
                Response.Redirect("/division");
                return;
            }

            if (!IsPostBack)
            {
                LoadVendors();
                LoadDivision();
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

        private void LoadDivision()
        {
            var division = _context.Divisions.FirstOrDefault(v => v.DivisionID == DivisionId);
            if (division == null)
            {
                Response.Redirect("/division");
                return;
            }

            txtName.Text = division.Name;
            ddlVendor.SelectedValue = division.VendorID.ToString();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var division = _context.Divisions.FirstOrDefault(v => v.DivisionID == DivisionId);
                if (division == null)
                {
                    Response.Redirect("/division");
                    return;
                }

                division.Name = txtName.Text.Trim();
                division.VendorID = int.Parse(ddlVendor.SelectedValue);

                _context.SaveChanges();
                Response.Redirect("/division");
            }
        }
    }
}