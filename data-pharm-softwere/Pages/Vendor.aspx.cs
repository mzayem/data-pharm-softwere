using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Web.UI;

namespace data_pharm_softwere.Pages
{
    public partial class Vendor : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;
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
                        var vendor = new Models.Vendor
                        {
                            Name = txtName.Text.Trim(),
                            Email = txtEmail.Text.Trim(),
                            Address = txtAddress.Text.Trim(),
                            Town = txtTown.Text.Trim(),
                            City = txtCity.Text.Trim(),
                            LicenceNo = txtLicenceNo.Text.Trim(),
                            ExpiryDate = DateTime.TryParse(txtExpiryDate.Text, out var expDate) ? expDate : DateTime.Now,
                            SRACode = txtSRACode.Text.Trim(),
                            GstNo = txtGstNo.Text.Trim(),
                            NtnNo = txtNtnNo.Text.Trim(),
                            CompanyCode = txtCompanyCode.Text.Trim(),
                            MaxDiscountAllowed = decimal.TryParse(txtMaxDiscount.Text, out var discount) ? discount : 0,
                            Remarks = txtRemarks.Text.Trim(),
                            CreatedAt = DateTime.Now
                        };

                        db.Vendors.Add(vendor);
                        db.SaveChanges();

                        lblMessage.CssClass = "text-success fw-semibold";
                        lblMessage.Text = "Vendor saved successfully!";
                        ClearForm();
                    }
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
            txtEmail.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtTown.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtLicenceNo.Text = string.Empty;
            txtExpiryDate.Text = string.Empty;
            txtSRACode.Text = string.Empty;
            txtGstNo.Text = string.Empty;
            txtNtnNo.Text = string.Empty;
            txtCompanyCode.Text = string.Empty;
            txtMaxDiscount.Text = string.Empty;
            txtRemarks.Text = string.Empty;
        }
    }
}