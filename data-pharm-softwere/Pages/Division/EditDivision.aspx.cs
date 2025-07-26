using data_pharm_softwere.Data;
using System;
using System.Linq;
using System.Data.Entity;
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

        private void LoadDivision()
        {
            var division = _context.Divisions.FirstOrDefault(v => v.DivisionID == DivisionId);
            if (division == null)
            {
                Response.Redirect("/division");
                return;
            }

            txtName.Text = division.Name;
            ddlVendor.SelectedValue = division.AccountId.ToString();
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
                division.AccountId = int.Parse(ddlVendor.SelectedValue);

                _context.SaveChanges();
                Response.Redirect("/division");
            }
        }
    }
}