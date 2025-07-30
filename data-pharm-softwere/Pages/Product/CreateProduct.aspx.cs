using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.Entity;

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
                LoadDivisions();
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
                var lastProduct = _context.Products
                    .OrderByDescending(p => p.ProductID)
                    .FirstOrDefault();

                long nextProductId = (lastProduct?.ProductID ?? 101000) + 1;
                lblProductId.Text = nextProductId.ToString();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error fetching Product ID: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
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

        private void LoadDivisions()
        {
            try
            {
                var divisions = _context.Divisions
                    .OrderBy(d => d.Name)
                    .ToList();
                ddlDivision.DataSource = divisions;
                ddlDivision.DataTextField = "Name";
                ddlDivision.DataValueField = "DivisionID";
                ddlDivision.DataBind();
                ddlDivision.Items.Insert(0, new ListItem("-- Select Division --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading divisions: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        private void LoadGroups(int? vendorId = null)
        {
            try
            {
                var query = _context.Groups.Include(g => g.Division).AsQueryable();
                if (vendorId.HasValue)
                    query = query.Where(g => g.Division.AccountId == vendorId.Value);

                ddlGroup.DataSource = query.ToList();
                ddlGroup.DataTextField = "Name";
                ddlGroup.DataValueField = "GroupID";
                ddlGroup.DataBind();
                ddlGroup.Items.Insert(0, new ListItem("-- Select Group --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading groups: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        private void LoadSubGroups(int? vendorId = null, int? groupId = null)
        {
            try
            {
                var query = _context.SubGroups.Include(sg => sg.Group.Division).AsQueryable();
                if (vendorId.HasValue)
                    query = query.Where(sg => sg.Group.Division.AccountId == vendorId.Value);

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
                lblMessage.CssClass = "alert alert-danger mt-3";
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
                LoadSubGroups(vendorId);
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
                {// Validate PackingType
                    PackingType packingType;
                    if (!Enum.TryParse(ddlPackingType.SelectedValue, out packingType))
                    {
                        lblMessage.Text = "Please select a valid Packing Type.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    // Validate ProductType
                    ProductType productType;
                    if (!Enum.TryParse(ddlType.SelectedValue, out productType))
                    {
                        lblMessage.Text = "Please select a valid Product Type.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    // Validate HSCode
                    if (!int.TryParse(txtHSCode.Text, out int hsCode))
                    {
                        lblMessage.Text = "Invalid HS Code format.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    // Validate CartonSize
                    if (!int.TryParse(txtCartonSize.Text, out int cartonSize))
                    {
                        lblMessage.Text = "Invalid Carton Size format.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    // Validate SubGroup
                    if (!int.TryParse(ddlSubGroup.SelectedValue, out int subGroupId))
                    {
                        lblMessage.Text = "Please select a valid SubGroup.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }
                    if (!int.TryParse(ddlDivision.SelectedValue, out int divisionId))
                    {
                        lblMessage.Text = "Please select a valid Division.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    if (!long.TryParse(txtProductCode.Text.Trim(), out var productCode))
                    {
                        lblMessage.Text = "Invalid Product Code";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    // Assign values
                    var product = new Models.Product
                    {
                        Type = productType,
                        PackingType = packingType,
                        Name = txtName.Text.Trim(),
                        ProductCode = productCode,

                        HSCode = hsCode,
                        PackingSize = txtPackingSize.Text.Trim(),
                        CartonSize = cartonSize,
                        Uom = txtUom.Text.Trim(),
                        PurchaseDiscount = decimal.TryParse(txtPurchaseDiscount.Text, out var disc1) ? disc1 : 0,
                        ReqGST = decimal.TryParse(txtReqGST.Text, out var disc2) ? disc2 : 0,
                        UnReqGST = decimal.TryParse(txtUnReqGST.Text, out var disc3) ? disc3 : 0,
                        IsAdvTaxExempted = chkAdvTaxExempted.Checked,
                        IsGSTExempted = chkGSTExempted.Checked,
                        SubGroupID = subGroupId,
                        DivisionID = divisionId,
                        CreatedAt = DateTime.Now
                    };

                    _context.Products.Add(product);
                    _context.SaveChanges();

                    lblMessage.Text = "Product saved successfully!";
                    lblMessage.CssClass = "alert alert-success mt-3";
                    ClearForm();
                    ShowNextProductId();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "alert alert-danger mt-3";
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
            ddlDivision.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            txtUom.Text = "";
            chkAdvTaxExempted.Checked = false;
            chkGSTExempted.Checked = false;
        }
    }
}