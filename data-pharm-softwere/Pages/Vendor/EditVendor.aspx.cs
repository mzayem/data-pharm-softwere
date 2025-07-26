using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Linq;
using System.Data.Entity;

namespace data_pharm_softwere.Pages.Vendor
{
    public partial class EditVendor : System.Web.UI.Page
    {
        private DataPharmaContext _context = new DataPharmaContext();

        private int AccountId
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
            if (!IsPostBack)
            {
                if (AccountId > 0)
                {
                    txtID.Text = AccountId.ToString();
                    LoadVendor();
                }
                else
                {
                    Response.Redirect("/vendor/create/");
                }
            }
        }

        protected void btnFetchAccount_Click(object sender, EventArgs e)
        {
            int newAccountId;
            if (!int.TryParse(txtID.Text.Trim(), out newAccountId))
            {
                lblMessage.CssClass = "text-danger fw-semibold";
                lblMessage.Text = "Invalid Account ID.";
                return;
            }

            var account = _context.Accounts.FirstOrDefault(a => a.AccountId == newAccountId);
            if (account == null)
            {
                lblMessage.CssClass = "text-danger fw-semibold";
                lblMessage.Text = "Account not found.";
                return;
            }

            if (!string.Equals(account.AccountType, "VENDORS", StringComparison.OrdinalIgnoreCase))
            {
                lblMessage.CssClass = "text-danger fw-semibold";
                lblMessage.Text = "This account is not of type 'Vendor'.";
                return;
            }

            if (!string.Equals(account.Status, "ACTIVE", StringComparison.OrdinalIgnoreCase))
            {
                lblMessage.CssClass = "text-danger fw-semibold";
                lblMessage.Text = "This account is not active.";
                return;
            }

            // Check if vendor already exists
            var vendor = _context.Vendors.FirstOrDefault(v => v.AccountId == newAccountId);
            if (vendor == null)
            {
                // Redirect to Create page with this accountId
                Response.Redirect($"/vendor/create?id={newAccountId}", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // If vendor exists, refresh the page with new ID to reload it
            Response.Redirect($"/vendor/edit?id={newAccountId}", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void LoadVendor()
        {
            var vendor = _context.Vendors.Include(v => v.Account).FirstOrDefault(v => v.AccountId == AccountId);
            if (vendor == null)
            {
                Response.Redirect("/vendor/create/");
                return;
            }
            txtName.Text = vendor.Account.AccountName;
            txtEmail.Text = vendor.Email;
            txtContact.Text = vendor.Contact;
            txtAddress.Text = vendor.Address;
            txtTown.Text = vendor.Town;
            txtCity.Text = vendor.City;
            txtLicenceNo.Text = vendor.LicenceNo;
            txtExpiryDate.Text = vendor.ExpiryDate.ToString("yyyy-MM-dd");
            txtSraCode.Text = vendor.SRACode;
            txtGstNo.Text = vendor.GstNo;
            txtNtnNo.Text = vendor.NtnNo;
            txtCompanyCode.Text = vendor.CompanyCode;
            txtRemarks.Text = vendor.Remarks;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var vendor = _context.Vendors.FirstOrDefault(v => v.AccountId == AccountId);
            if (vendor == null)
            {
                Response.Redirect("/vendor/create/");
                return;
            }

            vendor.Email = txtEmail.Text;
            vendor.Contact = txtContact.Text;
            vendor.Address = txtAddress.Text;
            vendor.Town = txtTown.Text;
            vendor.City = txtCity.Text;
            vendor.LicenceNo = txtLicenceNo.Text;
            vendor.ExpiryDate = DateTime.Parse(txtExpiryDate.Text);
            vendor.SRACode = txtSraCode.Text;
            vendor.GstNo = txtGstNo.Text;
            vendor.NtnNo = txtNtnNo.Text;
            vendor.CompanyCode = txtCompanyCode.Text;
            vendor.Remarks = txtRemarks.Text;

            _context.SaveChanges();
            Response.Redirect("/vendor");
        }
    }
}