using data_pharm_softwere.Data;
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
                    FetchAccount(AccountId);
                }
                lblMessage.Text = string.Empty;
            }
        }

        protected void btnFetchAccount_Click(object sender, EventArgs e)
        {
            int accountId;
            if (!int.TryParse(txtID.Text.Trim(), out accountId))
            {
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Text = "Invalid Account ID.";
                return;
            }

            FetchAccount(accountId);
        }

        private void FetchAccount(int accountId)
        {
            using (var db = new DataPharmaContext())
            {
                var account = db.Accounts.FirstOrDefault(a => a.AccountId == accountId);

                if (account == null)
                {
                    lblMessage.CssClass = "alert alert-danger";
                    lblMessage.Text = "Account not found.";
                    return;
                }

                if (!account.AccountType?.Equals("VENDORS", StringComparison.OrdinalIgnoreCase) ?? true)
                {
                    lblMessage.CssClass = "alert alert-warning";
                    lblMessage.Text = "This Account is not of type 'Vendor'.";
                    return;
                }

                if (!account.Status?.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase) ?? true)
                {
                    lblMessage.CssClass = "alert alert-danger";
                    lblMessage.Text = "This Account is Deactivated!";
                    return;
                }

                var vendor = db.Vendors.FirstOrDefault(v => v.AccountId == accountId);
                if (vendor != null)
                {
                    // Redirect to Edit page
                    Response.Redirect($"/vendor/edit?id={vendor.AccountId}", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                // No vendor yet, show account name to proceed
                txtName.Text = account.AccountName;
                lblMessage.CssClass = "alert alert-success";
                lblMessage.Text = "Account is valid. Please fill vendor details.";
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    using (var db = new DataPharmaContext())
                    {
                        int accountId;
                        if (!int.TryParse(txtID.Text.Trim(), out accountId))
                        {
                            lblMessage.CssClass = "alert alert-danger";
                            lblMessage.Text = "Invalid Account ID. Cannot save vendor.";
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

                        if (db.Vendors.Any(v => v.AccountId == accountId))
                        {
                            lblMessage.Text = "Vendor already exists for this Account ID.";
                            lblMessage.CssClass = "alert alert-danger";
                            return;
                        }

                        db.Vendors.Add(vendor);
                        db.SaveChanges();

                        lblMessage.CssClass = "alert alert-success";
                        lblMessage.Text = "Vendor saved successfully!";
                        ClearForm();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "alert alert-danger";
                }
            }
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