using data_pharm_softwere.Data;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Batch.Controls
{
    public partial class BatchModal : System.Web.UI.UserControl
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts(string selectedProduct = null)
        {
            var products = _context.Products.ToList();
            ddlProduct.DataSource = products;
            ddlProduct.DataTextField = "Name";
            ddlProduct.DataValueField = "ProductID";
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Product --", ""));

            if (!string.IsNullOrEmpty(selectedProduct) && ddlProduct.Items.FindByValue(selectedProduct) != null)
            {
                ddlProduct.SelectedValue = selectedProduct;
            }
        }

        public void SetBatchNo(string batchNo, string productIdOrName = null)
        {
            txtBatchNo.Text = batchNo;

            if (ddlProduct.Items.Count <= 1)
            {
                LoadProducts();
            }
            if (!string.IsNullOrEmpty(productIdOrName))
            {
                string selectedValue = null;

                // If it's numeric, assume ProductID
                if (int.TryParse(productIdOrName, out var pid))
                {
                    selectedValue = pid.ToString();
                }
                else
                {
                    // Resolve by Name -> get ID
                    var product = _context.Products
                        .FirstOrDefault(p => p.Name.Equals(productIdOrName, StringComparison.OrdinalIgnoreCase));

                    if (product != null)
                    {
                        selectedValue = product.ProductID.ToString();
                    }
                }

                if (!string.IsNullOrEmpty(selectedValue) && ddlProduct.Items.FindByValue(selectedValue) != null)
                {
                    ddlProduct.ClearSelection();
                    ddlProduct.SelectedValue = selectedValue;
                }
            }
            updBatchModal.Update();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var batch = new Models.BatchStock
                {
                    ProductID = int.Parse(ddlProduct.SelectedValue),
                    BatchNo = txtBatchNo.Text,
                    MFGDate = DateTime.Parse(txtMFGDate.Text),
                    ExpiryDate = DateTime.Parse(txtExpiryDate.Text),
                    DP = decimal.Parse(txtDP.Text),
                    TP = decimal.Parse(txtTP.Text),
                    MRP = decimal.Parse(txtMRP.Text),
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin",
                };

                _context.BatchesStock.Add(batch);
                _context.SaveChanges();

                Response.Redirect("/batch");
            }
        }
    }
}