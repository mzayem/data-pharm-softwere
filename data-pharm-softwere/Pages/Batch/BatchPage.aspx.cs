using data_pharm_softwere.Data;

using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Batch
{
    public partial class BatchPage : System.Web.UI.Page
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
                LoadBatches();
            }
        }

        private void LoadVendors()
        {
            var vendors = _context.Vendors.OrderBy(v => v.Name).ToList();
            ddlVendor.DataSource = vendors;
            ddlVendor.DataTextField = "Name";
            ddlVendor.DataValueField = "VendorID";
            ddlVendor.DataBind();
            ddlVendor.Items.Insert(0, new ListItem("All Vendors", ""));
        }

        private void LoadGroups()
        {
            int vendorId;
            var groups = _context.Groups.AsQueryable();

            if (int.TryParse(ddlVendor.SelectedValue, out vendorId))
            {
                groups = groups.Where(g => g.VendorID == vendorId);
            }

            ddlGroup.DataSource = groups.OrderBy(g => g.Name).ToList();
            ddlGroup.DataTextField = "Name";
            ddlGroup.DataValueField = "GroupID";
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, new ListItem("All Groups", ""));
        }

        private void LoadSubGroups()
        {
            int groupId;
            var subGroups = _context.SubGroups.AsQueryable();

            if (int.TryParse(ddlGroup.SelectedValue, out groupId))
            {
                subGroups = subGroups.Where(sg => sg.GroupID == groupId);
            }

            ddlSubGroup.DataSource = subGroups.OrderBy(sg => sg.Name).ToList();
            ddlSubGroup.DataTextField = "Name";
            ddlSubGroup.DataValueField = "SubGroupID";
            ddlSubGroup.DataBind();
            ddlSubGroup.Items.Insert(0, new ListItem("All SubGroups", ""));
        }

        private void LoadProducts()
        {
            int subGroupId;
            var products = _context.Products.AsQueryable();

            if (int.TryParse(ddlSubGroup.SelectedValue, out subGroupId))
            {
                products = products.Where(p => p.SubGroupID == subGroupId);
            }

            ddlProduct.DataSource = products.OrderBy(p => p.Name).ToList();
            ddlProduct.DataTextField = "Name";
            ddlProduct.DataValueField = "ProductID";
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, new ListItem("All Products", ""));
        }

        private void LoadBatches(string search = "")
        {
            var query = _context.Batches.Select(b => new
            {
                b.BatchID,
                b.BatchNo,
                ProductName = b.Product.Name,
                b.MFGDate,
                b.ExpiryDate,
                b.MRP,
                b.TP,
                b.DP,
                ProductID = b.Product.ProductID,
                GroupID = b.Product.SubGroup.Group.GroupID,
                VendorID = b.Product.SubGroup.Group.Vendor.VendorID,
                SubGroupID = b.Product.SubGroup.SubGroupID
            });

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b =>
                    b.ProductName.Contains(search));
            }

            if (int.TryParse(ddlVendor.SelectedValue, out int vendorId))
                query = query.Where(b => b.VendorID == vendorId);

            if (int.TryParse(ddlGroup.SelectedValue, out int groupId))
                query = query.Where(b => b.GroupID == groupId);

            if (int.TryParse(ddlSubGroup.SelectedValue, out int subGroupId))
                query = query.Where(b => b.SubGroupID == subGroupId);

            if (int.TryParse(ddlProduct.SelectedValue, out int productId))
                query = query.Where(b => b.ProductID == productId);

            // Custom sort: first ExpiryDate ascending, then BatchNo numeric ascending
            var sortedList = query
                .ToList() // bring into memory to use natural sort
                .OrderBy(b => b.ExpiryDate)
                .ThenBy(b => b.BatchNo)
                .ToList();

            gvBatches.DataSource = sortedList;
            gvBatches.DataBind();
        }


        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadBatches(txtSearch.Text.Trim());
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroups();
            LoadSubGroups();
            LoadProductDropdown();
            LoadBatches(txtSearch.Text.Trim());
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSubGroups();
            LoadProducts();
            LoadBatches(txtSearch.Text.Trim());
        }

        protected void ddlSubGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
            LoadBatches(txtSearch.Text.Trim());
        }

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBatches(txtSearch.Text.Trim());
        }

        // Use this for dropdown
        private void LoadProductDropdown()
        {
            int subGroupId;
            var products = _context.Products.AsQueryable();

            if (int.TryParse(ddlSubGroup.SelectedValue, out subGroupId))
            {
                products = products.Where(p => p.SubGroupID == subGroupId);
            }

            ddlProduct.DataSource = products.OrderBy(p => p.Name).ToList();
            ddlProduct.DataTextField = "Name";
            ddlProduct.DataValueField = "ProductID";
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, new ListItem("All Products", ""));
        }

        protected void gvBatches_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Optional: handle edit/delete here
        }

        protected void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            HiddenField hf = (HiddenField)row.FindControl("hfBatchID");

            if (int.TryParse(hf.Value, out int batchId))
            {
                string action = ddl.SelectedValue;

                if (action == "Edit")
                {
                    Response.Redirect($"/batch/edit?id={batchId}");
                }
                else if (action == "Delete")
                {
                    var batch = _context.Batches.Find(batchId);
                    if (batch != null)
                    {
                        _context.Batches.Remove(batch);
                        _context.SaveChanges();
                        LoadBatches();
                    }
                }
            }
        }

        protected void gvBatches_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int batchId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "BatchID"));
                e.Row.Attributes["style"] = "cursor:pointer";

                e.Row.Attributes["onclick"] = $"window.location='/batch/edit?id={batchId}'";
            }

            
        }
    }
}