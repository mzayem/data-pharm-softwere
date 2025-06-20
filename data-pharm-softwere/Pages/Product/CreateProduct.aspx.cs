using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Product
{
    public partial class CreateProduct : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;
                LoadVendors();
                LoadGroups();
                LoadSubGroups();
                LoadDropdowns();
                ShowNextProductId();

                txtPurchaseDiscount.Attributes["step"] = "0.1";
                txtPurchaseDiscount.Attributes["min"] = "0";
                txtPurchaseDiscount.Attributes["max"] = "100";

                txtReqGST.Attributes["step"] = "0.1";
                txtReqGST.Attributes["min"] = "0";
                txtReqGST.Attributes["max"] = "100";

                txtUnReqGST.Attributes["step"] = "0.1";
                txtUnReqGST.Attributes["min"] = "0";
                txtUnReqGST.Attributes["max"] = "100";
            }
        }

        private void ShowNextProductId()
        {
            try
            {
                int nextId = (_context.Products.Any() ? _context.Products.Max(p => p.ProductID) : 0) + 1;
                lblProductId.Text = nextId.ToString(); // For TextBox use .Text
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error fetching Product ID: " + ex.Message;
                lblMessage.CssClass = "text-danger fw-semibold";
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

        private void LoadGroups(int? vendorId = null)
        {
            try
            {
                var query = _context.Groups.AsQueryable();
                if (vendorId.HasValue)
                    query = query.Where(g => g.VendorID == vendorId.Value);

                ddlGroup.DataSource = query.ToList();
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

        private void LoadSubGroups(int? vendorId = null, int? groupId = null)
        {
            try
            {
                var query = _context.SubGroups.AsQueryable();
                if (vendorId.HasValue)
                    query = query.Where(sg => sg.Group.VendorID == vendorId.Value);

                if (groupId.HasValue)
                    query = query.Where(sg => sg.GroupID == groupId.Value);

                ddlSubGroup.DataSource = query.ToList();
                ddlSubGroup.DataTextField = "Name";
                ddlSubGroup.DataValueField = "SubGroupID";
                ddlSubGroup.DataBind();
                ddlSubGroup.Items.Insert(0, new ListItem("-- Select SubGroup --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading subgroups: " + ex.Message;
                lblMessage.CssClass = "text-danger fw-semibold";
            }
        }

        private void LoadDropdowns()
        {
            ddlType.DataSource = Enum.GetNames(typeof(ProductType));
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem("-- Select Type --", ""));

            ddlPackingType.DataSource = Enum.GetNames(typeof(PackingType));
            ddlPackingType.DataBind();
            ddlPackingType.Items.Insert(0, new ListItem("-- Select PackingType --", ""));
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlVendor.SelectedValue, out int vendorId))
            {
                LoadGroups(vendorId);
                LoadSubGroups(vendorId); // only by vendor
            }
            else
            {
                LoadGroups();
                LoadSubGroups();
            }
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            int.TryParse(ddlVendor.SelectedValue, out int vendorId);
            if (int.TryParse(ddlGroup.SelectedValue, out int groupId))
            {
                LoadSubGroups(vendorId > 0 ? (int?)vendorId : null
                    , groupId);
            }
            else
            {
                LoadSubGroups(vendorId > 0 ? (int?)vendorId : null);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var product = new Models.Product
                    {
                        Name = txtName.Text.Trim(),
                        ProductCode = txtProductCode.Text.Trim(),
                        PackingSize = txtPackingSize.Text.Trim(),
                        CartonSize = txtCartonSize.Text.Trim(),
                        PurchaseDiscount = decimal.TryParse(txtPurchaseDiscount.Text, out var disc) ? disc : 0,
                        Type = (ProductType)Enum.Parse(typeof(ProductType), ddlType.SelectedValue),
                        PackingType = (PackingType)Enum.Parse(typeof(PackingType), ddlPackingType.SelectedValue),
                        Uom = txtUom.Text.Trim(),
                        HSCode = int.Parse(txtHSCode.Text),
                        ReqGST = int.Parse(txtReqGST.Text),
                        UnReqGST = int.Parse(txtUnReqGST.Text),
                        IsAdvTaxExempted = chkAdvTaxExempted.Checked,
                        IsGSTExempted = chkGSTExempted.Checked,
                        SubGroupID = int.Parse(ddlSubGroup.SelectedValue),
                        CreatedAt = DateTime.Now
                    };

                    _context.Products.Add(product);
                    _context.SaveChanges();

                    lblMessage.Text = "Product saved successfully!";
                    lblMessage.CssClass = "text-success fw-semibold";
                    ClearForm();
                    ShowNextProductId();
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
            txtName.Text = "";
            txtProductCode.Text = "";
            txtPackingSize.Text = "";
            txtCartonSize.Text = "";
            txtPurchaseDiscount.Text = "";
            txtHSCode.Text = "";
            txtReqGST.Text = "";
            txtUnReqGST.Text = "";
            ddlVendor.SelectedIndex = 0;
            ddlGroup.SelectedIndex = 0;
            ddlSubGroup.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            txtUom.Text = "";
            chkAdvTaxExempted.Checked = false;
            chkGSTExempted.Checked = false;
        }
    }
}