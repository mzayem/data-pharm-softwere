using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Linq;
using System.Web.UI;

namespace data_pharm_softwere.Pages.Vendor
{
    public partial class CreateVendor : System.Web.UI.Page
    {
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
                    FetchAccount(AccountId, out _);
                }
            }
            else
            {
                lblMessage.Text = string.Empty;
            }
        }

        protected void btnFetchAccount_Click(object sender, EventArgs e)
        {
            int accountId;
            if (!int.TryParse(txtID.Text.Trim(), out accountId))
            {
                ShowMessage("Invalid Account ID.", "danger");
                return;
            }

            txtName.Text = string.Empty;
            FetchAccount(accountId, out _);
        }

        private bool FetchAccount(int accountId, out Account validAccount)
        {
            validAccount = null;

            using (var db = new DataPharmaContext())
            {
                var account = db.Accounts.FirstOrDefault(a => a.AccountId == accountId);

                if (account == null)
                {
                    ShowMessage("Account not found.", "danger");
                    return false;
                }

                if (!account.AccountType?.Equals("VENDORS", StringComparison.OrdinalIgnoreCase) ?? true)
                {
                    ShowMessage("This Account is not of type 'Vendor'.", "warning");
                    return false;
                }

                if (!account.Status?.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase) ?? true)
                {
                    ShowMessage("This Account is Deactivated!", "danger");
                    return false;
                }

                if (db.Vendors != null && db.Vendors.Any(v => v.AccountId == accountId))
                {
                    // Redirect to Edit
                    Response.Redirect($"/vendor/edit?id={accountId}", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return false;
                }

                txtName.Text = account.AccountName;
                ShowMessage("Account is valid. Please fill vendor details.", "success");
                validAccount = account;
                return true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (!int.TryParse(txtID.Text.Trim(), out int accountId))
            {
                ShowMessage("Invalid Account ID. Cannot save vendor.", "danger");
                return;
            }

            if (!FetchAccount(accountId, out var account))
                return;

            try
            {
                using (var db = new DataPharmaContext())
                {
                    if (db.Vendors.Any(v => v.AccountId == accountId))
                    {
                        ShowMessage("Vendor already exists for this Account ID.", "danger");
                        return;
                    }

                    var vendor = new Models.Vendor
                    {
                        AccountId = accountId,
                        Email = txtEmail.Text.Trim(),
                        Address = txtAddress.Text.Trim(),
                        Contact = txtContact.Text.Trim(),
                        Town = txtTown.Text.Trim(),
                        City = txtCity.Text.Trim(),
                        LicenceNo = txtLicenceNo.Text.Trim(),
                        ExpiryDate = DateTime.TryParse(txtExpiryDate.Text, out var expDate) ? expDate : DateTime.Now,
                        SRACode = txtSRACode.Text.Trim(),
                        GstNo = txtGstNo.Text.Trim(),
                        NtnNo = txtNtnNo.Text.Trim(),
                        CompanyCode = txtCompanyCode.Text.Trim(),
                        Remarks = txtRemarks.Text.Trim(),
                        CreatedAt = DateTime.Now
                    };

                    db.Vendors.Add(vendor);
                    db.SaveChanges();

                    ShowMessage("Vendor saved successfully!", "success");
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        private void ShowMessage(string message, string cssType)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "alert alert-" + cssType;
        }

        private void ClearForm()
        {
            txtID.Text = string.Empty;
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtContact.Text = string.Empty;
            txtTown.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtLicenceNo.Text = string.Empty;
            txtExpiryDate.Text = string.Empty;
            txtSRACode.Text = string.Empty;
            txtGstNo.Text = string.Empty;
            txtNtnNo.Text = string.Empty;
            txtCompanyCode.Text = string.Empty;
            txtRemarks.Text = string.Empty;
        }
    }
}