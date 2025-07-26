using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Batch
{
    public partial class CreateBatch : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadVendors();
                LoadGroups();
                LoadSubGroups();
                LoadProducts();
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
            var query = _context.SubGroups.Include("Group.Division").AsQueryable();

            if (vendorId.HasValue && vendorId.Value > 0)
            {
                query = query.Where(sg => sg.Group.Division.AccountId == vendorId.Value);
            }

            // Filter by Group if valid
            if (groupId.HasValue && groupId.Value > 0)
            {
                query = query.Where(sg => sg.GroupID == groupId.Value);
            }

            ddlSubGroup.DataSource = query.OrderBy(sg => sg.Name).ToList();
            ddlSubGroup.DataTextField = "Name";
            ddlSubGroup.DataValueField = "SubGroupID";
            ddlSubGroup.DataBind();
            ddlSubGroup.Items.Insert(0, new ListItem("-- Select SubGroup --", ""));
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int.TryParse(ddlVendor.SelectedValue, out int vendorId);
            LoadGroups(vendorId);
            LoadSubGroups(vendorId);
            LoadProducts(); // filter products
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            int.TryParse(ddlVendor.SelectedValue, out int vendorId);
            int.TryParse(ddlGroup.SelectedValue, out int groupId);
            LoadSubGroups(vendorId > 0 ? (int?)vendorId : null, groupId);
            LoadProducts(); // filter products
        }

        protected void ddlSubGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts(); // filtered by SubGroup
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

            var products = query
                .Select(p => new
                {
                    p.ProductID,
                    DisplayName = p.Name,
                    p.CartonSize
                })
                .ToList();

            ddlProduct.DataSource = products;
            ddlProduct.DataTextField = "DisplayName";
            ddlProduct.DataValueField = "ProductID";
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, new ListItem("-- Select Product --", ""));

            foreach (ListItem item in ddlProduct.Items)
            {
                int productId;
                if (int.TryParse(item.Value, out productId))
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
            CalculateCartonPrice();
        }

        protected void txtDP_TextChanged(object sender, EventArgs e)
        {
            CalculateCartonPrice();
        }

        private void CalculateCartonPrice()
        {
            if (int.TryParse(ddlProduct.SelectedValue, out int productId))
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductID == productId);
                if (product != null && decimal.TryParse(txtDP.Text, out decimal dp))
                {
                    txtCartonPrice.Text = (dp * product.CartonSize).ToString("0.00");
                }
                else
                {
                    txtCartonPrice.Text = "";
                }
            }
            else
            {
                txtCartonPrice.Text = "";
            }
        }

        protected void txtBatchNo_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtBatchNo.Text.Trim(), out int batchNo))
            {
                var existingBatch = _context.Batches.FirstOrDefault(b => b.BatchNo == batchNo);
                if (existingBatch != null)
                {
                    // Redirect to edit page with ID
                    Response.Redirect($"/batch/edit?id={existingBatch.BatchID}");
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    if (int.TryParse(txtBatchNo.Text.Trim(), out int batchNo))
                    {
                        var existingBatch = _context.Batches.FirstOrDefault(b => b.BatchNo == batchNo);
                        if (existingBatch != null)
                        {
                            // Redirect if batch already exists
                            Response.Redirect($"/batch/edit?id={existingBatch.BatchID}");
                            return;
                        }
                    }

                    var batch = new Models.Batch
                    {
                        ProductID = string.IsNullOrEmpty(ddlProduct.SelectedValue)
                            ? (int?)null
                            : int.Parse(ddlProduct.SelectedValue),
                        BatchNo = int.Parse(txtBatchNo.Text),
                        MFGDate = DateTime.Parse(txtMFGDate.Text),
                        ExpiryDate = DateTime.Parse(txtExpiryDate.Text),
                        DP = decimal.Parse(txtDP.Text),
                        TP = decimal.Parse(txtTP.Text),
                        MRP = decimal.Parse(txtMRP.Text),
                        CartonQty = int.Parse(txtCartonQty.Text),
                        CartonPrice = decimal.Parse(txtCartonPrice.Text),
                        CreatedAt = DateTime.Now,
                        CreatedBy = "Admin",
                    };

                    _context.Batches.Add(batch);
                    _context.SaveChanges();

                    lblMessage.Text = "Batch saved successfully.";
                    lblMessage.CssClass = "alert alert-success mt-3";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "alert alert-danger mt-3";
                }
                finally
                {
                    ddlVendor.ClearSelection();
                    ddlGroup.ClearSelection();
                    ddlSubGroup.ClearSelection();
                    ddlProduct.ClearSelection();

                    txtBatchNo.Text = "";
                    txtMFGDate.Text = "";
                    txtExpiryDate.Text = "";
                    txtDP.Text = "";
                    txtTP.Text = "";
                    txtMRP.Text = "";
                    txtCartonQty.Text = "";
                    txtCartonPrice.Text = "";
                }
            }
        }
    }
}