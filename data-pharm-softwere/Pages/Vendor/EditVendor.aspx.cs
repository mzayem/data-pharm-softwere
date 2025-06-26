using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Linq;

namespace data_pharm_softwere.Pages.Vendor
{
    public partial class EditVendor : System.Web.UI.Page
    {
        private DataPharmaContext _context = new DataPharmaContext();
        private int VendorId => Convert.ToInt32(Request.QueryString["id"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (VendorId > 0)
                {
                    LoadVendor();
                }
                else
                {
                    Response.Redirect("/vendor/create/");
                }
            }
        }

        private void LoadVendor()
        {
            var vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == VendorId);
            if (vendor == null)
            {
                Response.Redirect("/vendor/create/");
                return;
            }

            txtName.Text = vendor.Name;
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
            txtDiscount.Text = vendor.MaxDiscountAllowed.ToString();
            txtRemarks.Text = vendor.Remarks;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var vendor = _context.Vendors.FirstOrDefault(v => v.VendorID == VendorId);
            if (vendor == null)
            {
                Response.Redirect("/vendor/create/");
                return;
            }

            vendor.Name = txtName.Text;
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
            vendor.MaxDiscountAllowed = decimal.TryParse(txtDiscount.Text, out decimal discount) ? discount : 0;
            vendor.Remarks = txtRemarks.Text;

            _context.SaveChanges();
            Response.Redirect("/vendor");
        }
    }
}