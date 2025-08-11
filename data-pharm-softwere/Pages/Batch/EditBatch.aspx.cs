using data_pharm_softwere.Data;
using System.Data.Entity;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Batch
{
    public partial class EditBatch : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        private int BatchId
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
            //if (BatchId == 0)
            //{
            //    Response.Redirect("/batch/create");
            //    return;
            //}

            if (!IsPostBack)
            {
                LoadVendors();
                LoadGroups();
                LoadSubGroups();
                LoadProducts();
                LoadBatch();
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

        private void LoadGroups(int? vendorId = null)
        {
            var query = _context.Groups.Include("Division").AsQueryable();
            if (vendorId.HasValue && vendorId.Value > 0)
            {
                query = query.Where(g => g.Division.AccountId == vendorId.Value);
            }
            ddlGroup.DataSource = query.OrderBy(g => g.Name).ToList();
            ddlGroup.DataTextField = "Name";
            ddlGroup.DataValueField = "GroupID";
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, new ListItem("-- Select Group --", ""));
        }

        private void LoadSubGroups(int? vendorId = null, int? groupId = null)
        {
            var query = _context.SubGroups
                .Include("Group.Division")
                .AsQueryable();

            if (vendorId.HasValue)
                query = query.Where(sg => sg.Group.Division.AccountId == vendorId.Value);
            if (groupId.HasValue)
                query = query.Where(sg => sg.GroupID == groupId.Value);

            ddlSubGroup.DataSource = query.OrderBy(sg => sg.Name).ToList();
            ddlSubGroup.DataTextField = "Name";
            ddlSubGroup.DataValueField = "SubGroupID";
            ddlSubGroup.DataBind();
            ddlSubGroup.Items.Insert(0, new ListItem("-- Select SubGroup --", ""));
        }

        private void LoadProducts()
        {
            var query = _context.Products.AsQueryable();
            if (int.TryParse(ddlSubGroup.SelectedValue, out int subGroupId))
            {
                query = query.Where(p => p.SubGroupID == subGroupId);
            }
            else if (int.TryParse(ddlGroup.SelectedValue, out int groupId))
            {
                query = query.Where(p => p.SubGroup.GroupID == groupId);
            }
            else if (int.TryParse(ddlVendor.SelectedValue, out int vendorId))
            {
                query = query.Where(p => p.SubGroup.Group.Division.AccountId == vendorId);
            }

            var products = query.Select(p => new
            {
                p.ProductID,
                DisplayName = p.Name,
                p.CartonSize
            }).ToList();

            ddlProduct.DataSource = products;
            ddlProduct.DataTextField = "DisplayName";
            ddlProduct.DataValueField = "ProductID";
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, new ListItem("-- Select Product --", ""));

            foreach (ListItem item in ddlProduct.Items)
            {
                if (int.TryParse(item.Value, out int productId))
                {
                    var product = _context.Products.FirstOrDefault(p => p.ProductID == productId);
                    if (product != null)
                    {
                        item.Attributes["data-cartonsize"] = product.CartonSize.ToString();
                    }
                }
            }
        }

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCartonPrice();
        }

        protected void txtDP_TextChanged(object sender, EventArgs e)
        {
            UpdateCartonPrice();
        }

        private void UpdateCartonPrice()
        {
            if (ddlProduct.SelectedValue != "" && decimal.TryParse(txtDP.Text.Trim(), out decimal dp))
            {
                int productId = int.Parse(ddlProduct.SelectedValue);

                using (var db = new DataPharmaContext())
                {
                    var product = db.Products.FirstOrDefault(p => p.ProductID == productId);

                    if (product != null && product.CartonSize > 0)
                    {
                        decimal cartonPrice = dp * product.CartonSize;
                        txtCartonPrice.Text = cartonPrice.ToString("0.00");
                    }
                    else
                    {
                        txtCartonPrice.Text = "";
                    }
                }
            }
            else
            {
                txtCartonPrice.Text = "";
            }
        }

        private void LoadBatch()
        {
            var batch = _context.BatchesStock.FirstOrDefault(b => b.BatchStockID == BatchId);
            if (batch == null)
            {
                Response.Redirect("/batch/create");
                return;
            }

            var product = _context.Products.FirstOrDefault(p => p.ProductID == batch.ProductID);
            var subGroup = product?.SubGroup;
            var group = subGroup?.Group;
            var vendor = group?.Division?.Vendor;

            if (vendor != null) ddlVendor.SelectedValue = vendor.AccountId.ToString();
            LoadGroups(vendor?.AccountId);
            if (group != null) ddlGroup.SelectedValue = group.GroupID.ToString();
            LoadSubGroups(vendor?.AccountId, group?.GroupID);
            if (subGroup != null) ddlSubGroup.SelectedValue = subGroup.SubGroupID.ToString();
            LoadProducts();
            ddlProduct.SelectedValue = product?.ProductID.ToString();

            txtBatchNo.Text = batch.BatchNo.ToString();
            txtMFGDate.Text = batch.MFGDate.ToString("yyyy-MM-dd");
            txtExpiryDate.Text = batch.ExpiryDate.ToString("yyyy-MM-dd");
            txtDP.Text = batch.DP.ToString("0.##");
            txtTP.Text = batch.TP.ToString("0.##");
            txtMRP.Text = batch.MRP.ToString("0.##");
            txtCartonQty.Text = batch.CartonUnits.ToString();
            txtCartonPrice.Text = batch.CartonDp.ToString("0.##");
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int.TryParse(ddlVendor.SelectedValue, out int vendorId);
            LoadGroups(vendorId);
            LoadSubGroups(vendorId);
            LoadProducts();
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            int.TryParse(ddlVendor.SelectedValue, out int vendorId);
            int.TryParse(ddlGroup.SelectedValue, out int groupId);
            LoadSubGroups(vendorId, groupId);
            LoadProducts();
        }

        protected void ddlSubGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var batch = _context.BatchesStock.FirstOrDefault(b => b.BatchStockID == BatchId);
            if (batch == null)
            {
                Response.Redirect("/batch/create");
                return;
            }

            try
            {
                batch.ProductID = int.Parse(ddlProduct.SelectedValue);
                batch.BatchNo = txtBatchNo.Text.Trim();
                batch.MFGDate = DateTime.Parse(txtMFGDate.Text);
                batch.ExpiryDate = DateTime.Parse(txtExpiryDate.Text);
                batch.DP = decimal.Parse(txtDP.Text);
                batch.TP = decimal.Parse(txtTP.Text);
                batch.MRP = decimal.Parse(txtMRP.Text);
                batch.CartonUnits = int.Parse(txtCartonQty.Text);
                batch.CartonDp = decimal.Parse(txtCartonPrice.Text);
                batch.UpdatedAt = DateTime.Now;
                batch.UpdatedBy = "Admin";

                _context.SaveChanges();

                Response.Redirect("/batch");
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }
    }
}