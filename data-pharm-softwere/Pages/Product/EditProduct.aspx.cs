using System;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;
using data_pharm_softwere.Data;
using data_pharm_softwere.Models;

namespace data_pharm_softwere.Pages.Product
{
    public partial class EditProduct : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        private int ProductId
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
                lblMessage.Text = string.Empty;
                LoadVendors();
                LoadGroups();
                LoadSubGroups();
                LoadDropdowns();

                if (ProductId > 0)
                {
                    LoadProduct();
                }
                else
                {
                    Response.Redirect("/product/create");
                }

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
                ShowError("Error loading vendors: " + ex.Message);
            }
        }

        private void LoadGroups(int? vendorId = null)
        {
            try
            {
                var query = _context.Groups.Include("Division").AsQueryable();
                if (vendorId.HasValue)
                    query = query.Where(g => g.Division.VendorID == vendorId.Value);

                ddlGroup.DataSource = query.ToList();
                ddlGroup.DataTextField = "Name";
                ddlGroup.DataValueField = "GroupID";
                ddlGroup.DataBind();
                ddlGroup.Items.Insert(0, new ListItem("-- Select Group --", ""));
            }
            catch (Exception ex)
            {
                ShowError("Error loading groups: " + ex.Message);
            }
        }

        private void LoadSubGroups(int? vendorId = null, int? groupId = null)
        {
            try
            {
                var query = _context.SubGroups
                    .Include("Group.Division")
                    .AsQueryable();

                if (vendorId.HasValue)
                    query = query.Where(sg => sg.Group.Division.VendorID == vendorId.Value);

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
                ShowError("Error loading subgroups: " + ex.Message);
            }
        }

        private void LoadDropdowns()
        {
            ddlPackingType.DataSource = Enum.GetNames(typeof(PackingType));
            ddlPackingType.DataBind();
            ddlPackingType.Items.Insert(0, new ListItem("-- Select PackingType --", ""));

            ddlType.DataSource = Enum.GetNames(typeof(ProductType));
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem("-- Select Type --", ""));
        }

        private void LoadProduct()
        {
            try
            {
                var product = _context.Products.Find(ProductId);
                if (product == null)
                {
                    Response.Redirect("/product/create");
                    return;
                }

                lblProductId.Text = product.ProductID.ToString();
                txtName.Text = product.Name;
                txtProductCode.Text = product.ProductCode.ToString();
                txtHSCode.Text = product.HSCode.ToString();
                txtPackingSize.Text = product.PackingSize;
                txtCartonSize.Text = product.CartonSize.ToString();
                txtUom.Text = product.Uom;
                txtPurchaseDiscount.Text = product.PurchaseDiscount.ToString();
                txtReqGST.Text = product.ReqGST.ToString();
                txtUnReqGST.Text = product.UnReqGST.ToString();
                ddlPackingType.SelectedValue = product.PackingType.ToString();
                ddlType.SelectedValue = product.Type.ToString();
                chkAdvTaxExempted.Checked = product.IsAdvTaxExempted;
                chkGSTExempted.Checked = product.IsGSTExempted;

                var subGroup = _context.SubGroups.Find(product.SubGroupID);
                if (subGroup != null)
                {
                    var group = _context.Groups.Find(subGroup.GroupID);
                    if (group != null)
                    {
                        ddlVendor.SelectedValue = group.Division.VendorID.ToString();
                        LoadGroups(group.Division.VendorID);
                        ddlGroup.SelectedValue = group.GroupID.ToString();
                        LoadSubGroups(group.Division.VendorID, group.GroupID);
                        ddlSubGroup.SelectedValue = subGroup.SubGroupID.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Error loading product: " + ex.Message);
            }
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

            ddlGroup.SelectedIndex = 0;
            ddlSubGroup.SelectedIndex = 0;
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            int.TryParse(ddlVendor.SelectedValue, out int vendorId);
            if (int.TryParse(ddlGroup.SelectedValue, out int groupId))
            {
                LoadSubGroups(vendorId > 0 ? (int?)vendorId : null, groupId);
            }
            else
            {
                LoadSubGroups(vendorId > 0 ? (int?)vendorId : null);
            }

            ddlSubGroup.SelectedIndex = 0;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var product = _context.Products.Find(ProductId);
                if (product == null)
                {
                    ShowError("Product not found.");
                    return;
                }

                if (!int.TryParse(txtHSCode.Text, out int hsCode))
                {
                    ShowError("Invalid HS Code format.");
                    return;
                }

                if (!int.TryParse(txtCartonSize.Text, out int cartonSize))
                {
                    ShowError("Invalid Carton Size format.");
                    return;
                }

                if (!decimal.TryParse(txtReqGST.Text, out decimal reqGst) ||
                    !decimal.TryParse(txtUnReqGST.Text, out decimal unreqGst))
                {
                    ShowError("Invalid GST format.");
                    return;
                }

                if (!Enum.TryParse(ddlPackingType.SelectedValue, out PackingType packingType))
                {
                    ShowError("Please select a valid Packing Type.");
                    return;
                }

                if (!Enum.TryParse(ddlType.SelectedValue, out ProductType productType))
                {
                    ShowError("Please select a valid Product Type.");
                    return;
                }

                if (!int.TryParse(ddlSubGroup.SelectedValue, out int subGroupId))
                {
                    ShowError("Please select a valid SubGroup.");
                    return;
                }

                if (!long.TryParse(txtProductCode.Text.Trim(), out var productCode))
                {
                    lblMessage.Text = "Invalid Product Code";
                    lblMessage.CssClass = "alert alert-danger mt-3";
                    return;
                }

                product.Name = txtName.Text.Trim();
                product.ProductCode = productCode;
                product.HSCode = hsCode;
                product.PackingSize = txtPackingSize.Text.Trim();
                product.CartonSize = cartonSize;
                product.Uom = txtUom.Text.Trim();
                product.PurchaseDiscount = decimal.TryParse(txtPurchaseDiscount.Text, out var discount) ? discount : 0;
                product.ReqGST = reqGst;
                product.UnReqGST = unreqGst;
                product.PackingType = packingType;
                product.Type = productType;
                product.SubGroupID = subGroupId;
                product.IsAdvTaxExempted = chkAdvTaxExempted.Checked;
                product.IsGSTExempted = chkGSTExempted.Checked;
                product.UpdatedAt = DateTime.Now;

                _context.SaveChanges();

                Response.Redirect("/product");
            }
            catch (Exception ex)
            {
                ShowError("Error updating product: " + ex.Message);
            }
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "text-danger fw-semibold";
        }
    }
}