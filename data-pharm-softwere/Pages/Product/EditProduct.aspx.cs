using System;
using System.Linq;
using System.Web.UI.WebControls;
using data_pharm_softwere.Data;
using data_pharm_softwere.Models;

namespace data_pharm_softwere.Pages.Product
{
    public partial class EditProduct : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        private int ProductId => int.TryParse(Request.QueryString["id"], out int id) ? id : 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;

                LoadVendors();
                LoadPackingTypes();
                LoadProductTypes();

                if (ProductId > 0)
                {
                    LoadProduct();
                }
                else
                {
                    Response.Redirect("/product/create");
                }
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

        private void LoadGroupsByVendor(int? vendorId = null)
        {
            try
            {
                var groups = vendorId.HasValue
                    ? _context.Groups.Where(g => g.VendorID == vendorId).ToList()
                    : _context.Groups.ToList();

                ddlGroup.DataSource = groups;
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

        private void LoadSubGroupsByGroup(int? groupId = null)
        {
            try
            {
                var subGroups = groupId.HasValue
                    ? _context.SubGroups.Where(sg => sg.GroupID == groupId).ToList()
                    : _context.SubGroups.ToList();

                ddlSubGroup.DataSource = subGroups;
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

        private void LoadPackingTypes()
        {
            ddlPackingType.Items.Insert(0, new ListItem("Select", ""));
        }

        private void LoadProductTypes()
        {
            ddlType.Items.Insert(0, new ListItem("Select", ""));
        }

        private void LoadProduct()
        {
            try
            {
                var product = _context.Products.Find(ProductId);

                if (product == null)
                {
                    Response.Redirect("/product/create/");
                    return;
                }

                lblProductId.Text = product.ProductID.ToString();
                txtName.Text = product.Name;
                txtProductCode.Text = product.ProductCode;
                txtHSCode.Text = product.HSCode.ToString();
                txtPackingSize.Text = product.PackingSize;
                txtCartonSize.Text = product.CartonSize;
                txtUom.Text = product.Uom;
                txtPurchaseDiscount.Text = product.PurchaseDiscount.ToString();
                txtReqGST.Text = product.ReqGST.ToString();
                txtUnReqGST.Text = product.UnReqGST.ToString();
                ddlPackingType.SelectedValue = product.PackingType.ToString();
                ddlType.SelectedValue = product.Type.ToString();
                chkAdvTaxExempted.Checked = product.IsAdvTaxExempted;
                chkGSTExempted.Checked = product.IsGSTExempted;

                // Load Vendor > Group > SubGroup chain properly
                var subGroup = _context.SubGroups.Find(product.SubGroupID);
                if (subGroup != null)
                {
                    var group = _context.Groups.Find(subGroup.GroupID);
                    if (group != null)
                    {
                        ddlVendor.SelectedValue = group.VendorID.ToString();
                        LoadGroupsByVendor(group.VendorID);

                        ddlGroup.SelectedValue = group.GroupID.ToString();
                        LoadSubGroupsByGroup(group.GroupID);

                        ddlSubGroup.SelectedValue = subGroup.SubGroupID.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading product: " + ex.Message;
                lblMessage.CssClass = "text-danger fw-semibold";
            }
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlVendor.SelectedValue, out int vendorId))
            {
                LoadGroupsByVendor(vendorId);
                ddlGroup.ClearSelection();
                ddlSubGroup.ClearSelection();
            }
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlGroup.SelectedValue, out int groupId))
            {
                LoadSubGroupsByGroup(groupId);
                ddlSubGroup.ClearSelection();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var product = _context.Products.Find(ProductId);

                if (product == null)
                {
                    lblMessage.Text = "Product not found.";
                    lblMessage.CssClass = "text-danger fw-semibold";
                    return;
                }

                product.Name = txtName.Text.Trim();
                product.ProductCode = txtProductCode.Text.Trim();
                product.HSCode = int.Parse(txtHSCode.Text);
                product.PackingSize = txtPackingSize.Text.Trim();
                product.CartonSize = txtCartonSize.Text.Trim();
                product.Uom = txtUom.Text.Trim();
                product.PurchaseDiscount = decimal.TryParse(txtPurchaseDiscount.Text, out var discount) ? discount : 0;
                product.ReqGST = int.Parse(txtReqGST.Text);
                product.UnReqGST = int.Parse(txtUnReqGST.Text);

                product.PackingType = (PackingType)Enum.Parse(typeof(PackingType), ddlPackingType.SelectedValue);
                product.Type = (ProductType)Enum.Parse(typeof(ProductType), ddlType.SelectedValue);

                product.SubGroupID = int.Parse(ddlSubGroup.SelectedValue);

                product.IsAdvTaxExempted = chkAdvTaxExempted.Checked;
                product.IsGSTExempted = chkGSTExempted.Checked;

                product.UpdatedAt = DateTime.Now;

                _context.SaveChanges();

                Response.Redirect("/product");
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error updating product: " + ex.Message;
                lblMessage.CssClass = "text-danger fw-semibold";
            }
        }
    }
}