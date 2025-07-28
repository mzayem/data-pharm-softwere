using data_pharm_softwere.Data;
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
                return int.TryParse(Request.QueryString["id"], out id) ? id : 0;
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
            txtName.Text = string.Empty;

            if (!int.TryParse(txtID.Text.Trim(), out newAccountId))
            {
                ShowMessage("Invalid Account ID.", "danger");
                return;
            }

            if (!FetchAccount(newAccountId, out var validAccount)) return;

            // Check if vendor already exists
            var vendor = _context.Vendors.FirstOrDefault(v => v.AccountId == newAccountId);
            if (vendor == null)
            {
                Response.Redirect($"/vendor/create?id={newAccountId}", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // If vendor exists, reload this page with new ID
            Response.Redirect($"/vendor/edit?id={newAccountId}", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private bool FetchAccount(int accountId, out Account validAccount)
        {
            validAccount = null;

            using (var db = new DataPharmaContext())
            {
                var account = db.Accounts.FirstOrDefault(a => a.AccountId == accountId);

                if (account == null)
                {
                    Response.Redirect($"/vendor/create?id={accountId}", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return false;
                }

                if (!account.AccountType?.Equals("VENDORS", StringComparison.OrdinalIgnoreCase) ?? true)
                {
                    Response.Redirect($"/vendor/create?id={accountId}", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return false;
                }

                if (!account.Status?.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase) ?? true)
                {
                    Response.Redirect($"/vendor/create?id={accountId}", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return false;
                }

                txtName.Text = account.AccountName;
                validAccount = account;
                return true;
            }
        }

        private void ShowMessage(string message, string cssType)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "alert alert-" + cssType;
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

            // Validate account
            if (!FetchAccount(AccountId, out var validAccount))
            {
                return;
            }

            // Check vendor existence
            var vendor = _context.Vendors.FirstOrDefault(v => v.AccountId == AccountId);
            if (vendor == null)
            {
                Response.Redirect($"/vendor/create?id={AccountId}", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Update vendor
            vendor.Email = txtEmail.Text;
            vendor.Contact = txtContact.Text;
            vendor.Address = txtAddress.Text;
            vendor.Town = txtTown.Text;
            vendor.City = txtCity.Text;
            vendor.LicenceNo = txtLicenceNo.Text;

            if (DateTime.TryParse(txtExpiryDate.Text, out DateTime expiryDate))
            {
                vendor.ExpiryDate = expiryDate;
            }

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