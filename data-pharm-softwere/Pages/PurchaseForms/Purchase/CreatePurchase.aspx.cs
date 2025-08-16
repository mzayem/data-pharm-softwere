using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.PurchaseForms.Purchase
{
    public partial class CreatePurchase : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadInvoiceNumber();
                txtAdvTaxRate.Text = "0";
                ddlAdvTaxType.SelectedValue = "Net";

                lblGross.Text = "0.00";
                lblDiscount.Text = "0.00";
                lblAdvTaxAmount.Text = "0.00";
                lblNetAmount.Text = "0.00";
                Session["PurchaseDetails"] = new List<PurchaseLineItem>();
                LoadVendors();
                LoadAdvTaxType();
                LoadAllAvailableBatches();
                ClearUI();
            }
        }

        private void LoadInvoiceNumber()
        {
            try
            {
                var lastPurchase = _context.Purchases.OrderByDescending(p => p.PurchaseId).FirstOrDefault();

                int nextInvoice = (lastPurchase?.PurchaseId ?? 1) + 1;
                txtInvoiceNo.Text = nextInvoice.ToString();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error fetching Product ID: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        protected void txtVendorCode_TextChanged(object sender, EventArgs e)
        {
            var vendor = _context.Vendors.Include(v => v.Account)
                            .FirstOrDefault(v => v.AccountId.ToString() == txtVendorCode.Text.Trim());

            if (vendor != null)
            {
                ddlVendor.SelectedValue = vendor.AccountId.ToString();
                lblMessage.Text = "";
            }
            else
            {
                ddlVendor.SelectedIndex = 0;
                lblMessage.Text = "Vendor code not found.";
                lblMessage.CssClass = "alert alert-warning";
            }
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlVendor.SelectedValue))
            {
                txtVendorCode.Text = ddlVendor.SelectedValue;
            }
            else
            {
                txtVendorCode.Text = "";
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

        private void LoadAdvTaxType()
        {
            ddlAdvTaxType.Items.Clear();
            ddlAdvTaxType.Items.Add(new ListItem("Net", "Net"));
            ddlAdvTaxType.Items.Add(new ListItem("Gross", "Gross"));
            ddlAdvTaxType.SelectedIndex = 0;
            ddlGstType.Items.Clear();
            ddlGstType.Items.Add(new ListItem("Net", "Net"));
            ddlGstType.Items.Add(new ListItem("Gross", "Gross"));
            ddlGstType.SelectedIndex = 0;
        }

        private void LoadAllAvailableBatches()
        {
            var addedBatchNos = (Session["PurchaseDetails"] as List<PurchaseLineItem>)
                                ?.Select(i => i.BatchNo.Trim().ToLower())
                                .ToList() ?? new List<string>();

            var allBatches = _context.BatchesStock
                .Where(b => !addedBatchNos.Contains(b.BatchNo.Trim().ToLower()))
                .OrderByDescending(b => b.BatchNo)
                .Select(b => new { b.BatchNo })
                .ToList();

            ddlBatch.DataSource = allBatches;
            ddlBatch.DataTextField = "BatchNo";
            ddlBatch.DataValueField = "BatchNo";
            ddlBatch.DataBind();
            ddlBatch.Items.Insert(0, new ListItem("-- Select Batch --", ""));
        }

        protected void txtChargesChanged_TextChanged(object sender, EventArgs e)
        {
            var list = Session["PurchaseDetails"] as List<PurchaseLineItem> ?? new List<PurchaseLineItem>();
            UpdateTotals(list);
        }

        protected void txtProductId_TextChanged(object sender, EventArgs e)
        {
            ddlBatch.Items.Clear();
            hiddenProductId.Value = string.Empty;

            var addedBatchNos = (Session["PurchaseDetails"] as List<PurchaseLineItem>)
            ?.Select(i => i.BatchNo.Trim().ToLower())
            .ToList() ?? new List<string>();

            if (string.IsNullOrWhiteSpace(txtProductId.Text))
            {
                // Load ALL batches not in purchase
                var allBatches = _context.BatchesStock
                    .Where(b => !addedBatchNos.Contains(b.BatchNo.Trim().ToLower()))
                    .OrderByDescending(b => b.BatchNo)
                    .Select(b => new { b.BatchNo })
                    .ToList();

                ddlBatch.DataSource = allBatches;
                ddlBatch.DataTextField = "BatchNo";
                ddlBatch.DataValueField = "BatchNo";
                ddlBatch.DataBind();
                ddlBatch.Items.Insert(0, new ListItem("-- Select Batch --", ""));
                return;
            }

            // If product code is entered → get product
            if (!long.TryParse(txtProductId.Text.Trim(), out long productId))
            {
                lblMessage.Text = "Invalid product code.";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            var product = _context.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                lblMessage.Text = "Product code not found.";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            // Store for later use
            hiddenProductId.Value = product.ProductID.ToString();

            // Load batches for product (not in purchase)
            LoadBatches(product.ProductID, addedBatchNos);
        }

        protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlBatch.SelectedValue))
                return;

            string selectedBatch = ddlBatch.SelectedValue;
            txtBatchNo.Text = selectedBatch;

            var batch = _context.BatchesStock.Include("Product")
                .FirstOrDefault(b => b.BatchNo.Trim().Equals(selectedBatch, StringComparison.OrdinalIgnoreCase));

            if (batch != null)
            {
                hiddenProductId.Value = batch.ProductID.ToString();
                txtProductId.Text = batch.ProductID.ToString();

                var addedBatchNos = (Session["PurchaseDetails"] as List<PurchaseLineItem>)
                    ?.Select(i => i.BatchNo.Trim().ToLower())
                    .ToList() ?? new List<string>();

                LoadBatches(batch.ProductID, addedBatchNos, selectedBatch);
            }
        }

        protected void txtBatchNo_TextChanged(object sender, EventArgs e)
        {
            string batchNo = txtBatchNo.Text.Trim();
            if (string.IsNullOrEmpty(batchNo))
                return;

            int productId;

            // Try to find the batch regardless of hiddenProductId
            var batch = _context.BatchesStock
                .FirstOrDefault(b => b.BatchNo.Trim().Equals(batchNo, StringComparison.OrdinalIgnoreCase));

            if (batch == null)
            {
                // Pass current product ID if available
                string productValue = !string.IsNullOrEmpty(txtProductId.Text) ? txtProductId.Text : null;

                BatchModalControl.SetBatchNo(batchNo, productValue);
                string script = $@"
                    $('#batchModal').modal('show');
                ";
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "ShowBatchModal",
                    script,
                    true
                );

                return;
            }

            // If batch found, set IDs
            hiddenProductId.Value = batch.ProductID.ToString();
            txtProductId.Text = batch.ProductID.ToString();
            productId = batch.ProductID;

            // Continue loading batches
            var addedBatchNos = (Session["PurchaseDetails"] as List<PurchaseLineItem>)
                ?.Select(i => i.BatchNo.Trim().ToLower())
                .ToList() ?? new List<string>();

            LoadBatches(productId, addedBatchNos, batchNo);
        }

        private void LoadBatches(long productId, List<string> excludeBatchNos, string selectedBatch = null)
        {
            ddlBatch.Items.Clear();

            var batches = _context.BatchesStock
                .Where(b => b.ProductID == productId &&
                            !excludeBatchNos.Contains(b.BatchNo.Trim().ToLower()))
                .OrderByDescending(b => b.BatchNo)
                .Select(b => new { b.BatchNo })
                .ToList();

            if (batches.Any())
            {
                ddlBatch.DataSource = batches;
                ddlBatch.DataTextField = "BatchNo";
                ddlBatch.DataValueField = "BatchNo";
                ddlBatch.DataBind();
                ddlBatch.Items.Insert(0, new ListItem("-- Type or Select Batch --", ""));

                // Restore selection if provided
                if (!string.IsNullOrEmpty(selectedBatch) &&
                    ddlBatch.Items.FindByValue(selectedBatch) != null)
                {
                    ddlBatch.SelectedValue = selectedBatch;
                }
            }
            else
            {
                ddlBatch.Items.Insert(0, new ListItem("-- No batch found --", ""));
                txtBatchNo.Attributes["placeholder"] = "Enter new batch No";
            }
        }

        public class PurchaseLineItem
        {
            public int BatchStockID { get; set; }
            public string BatchNo { get; set; }
            public string ProductName { get; set; }
            public DateTime ExpiryDate { get; set; }
            public int CartonQty { get; set; }
            public decimal CartonPrice { get; set; }
            public decimal GSTPercent { get; set; }
            public decimal GSTAmount => CartonQty * CartonPrice * GSTPercent / 100;
            public decimal TotalAmount => (CartonQty * CartonPrice) + GSTAmount;
        }

        protected void btnAddBatch_Click(object sender, EventArgs e)
        {
            string batchNo = txtBatchNo.Text.Trim();
            if (string.IsNullOrEmpty(batchNo))
            {
                lblMessage.Text = "Please enter a batch number.";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            var productId = int.Parse(hiddenProductId.Value);
            var batchStock = _context.BatchesStock
                .Include("Product")
                .FirstOrDefault(b => b.ProductID == productId &&
                                     b.BatchNo.Trim().Equals(batchNo, StringComparison.OrdinalIgnoreCase) &&
                                     b.ExpiryDate >= DateTime.Today &&
                                     !b.PurchaseDetails.Any());

            if (batchStock == null)
            {
                lblMessage.Text = "Batch Stock not found.";
                lblMessage.CssClass = "alert alert-danger";
                return;
            }

            var list = Session["PurchaseDetails"] as List<PurchaseLineItem> ?? new List<PurchaseLineItem>();

            var item = new PurchaseLineItem
            {
                BatchStockID = batchStock.BatchStockID,
                BatchNo = batchStock.BatchNo,
                ProductName = batchStock.Product?.Name,
                ExpiryDate = batchStock.ExpiryDate,
                CartonQty = 0,
                CartonPrice = batchStock.CartonDp,
                GSTPercent = batchStock.Product?.ReqGST ?? 0
            };

            list.Add(item);
            Session["PurchaseDetails"] = list;

            gvPurchaseDetails.DataSource = list;
            gvPurchaseDetails.DataBind();
            UpdateTotals(list);

            txtProductId.Text = "";
            txtBatchNo.Text = "";
            ddlBatch.Items.Clear();
            LoadAllAvailableBatches();
        }

        private void UpdateTotals(List<PurchaseLineItem> list)
        {
            decimal gross = list.Sum(i => i.TotalAmount);
            decimal discount = decimal.TryParse(txtDiscountedAmount.Text, out var d) ? d : 0;
            decimal advTaxRate = decimal.TryParse(txtAdvTaxRate.Text, out var a) ? a : 0;
            decimal additional = decimal.TryParse(txtAdditionalCharges.Text, out var add) ? add : 0;

            string taxType = ddlAdvTaxType.SelectedValue;
            decimal advTax = taxType == "Net"
                ? (gross - discount) * advTaxRate / 100
                : gross * advTaxRate / (100 + advTaxRate);

            decimal net = gross - discount + advTax + additional;

            lblGross.Text = gross.ToString("N2");
            lblDiscount.Text = discount.ToString("N2");
            lblAdvTaxAmount.Text = advTax.ToString("N2");
            lblNetAmount.Text = net.ToString("N2");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var list = Session["PurchaseDetails"] as List<PurchaseLineItem>;
            if (list == null || !list.Any())
            {
                lblMessage.Text = "No batches added.";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            try
            {
                decimal gross = list.Sum(i => i.TotalAmount);
                decimal discount = decimal.Parse(txtDiscountedAmount.Text);
                decimal advTaxRate = decimal.Parse(txtAdvTaxRate.Text);
                decimal additional = decimal.Parse(txtAdditionalCharges.Text);
                string taxType = ddlAdvTaxType.SelectedValue;

                decimal advTax = taxType == "Net"
                    ? (gross - discount) * advTaxRate / 100
                    : gross * advTaxRate / (100 + advTaxRate);

                decimal net = gross - discount + advTax + additional;

                var purchase = new Models.Purchase
                {
                    PurchaseDate = DateTime.Parse(txtPurchaseDate.Text),
                    PoNumber = txtPoNumber.Text,
                    Reference = txtReference.Text,
                    PurchaseType = PurchaseType.Purchase,
                    AdvTaxRate = advTaxRate,
                    AdditionalCharges = additional,
                    DiscountedAmount = discount,
                    GrossAmount = gross,
                    AdvTaxAmount = advTax,
                    NetAmount = net,
                    VendorId = int.Parse(ddlVendor.SelectedValue),
                    CreatedBy = "Admin",
                    CreatedAt = DateTime.Now,
                    PurchaseDetails = list.Select(i => new PurchaseDetail
                    {
                        BatchStockID = i.BatchStockID,
                        CreatedAt = DateTime.Now
                    }).ToList()
                };

                _context.Purchases.Add(purchase);
                _context.SaveChanges();

                lblMessage.Text = "Purchase saved successfully.";
                lblMessage.CssClass = "alert alert-success mt-3";

                ClearUI();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
            }
        }

        protected void gvBatches_RowCommand(object sender, GridViewRowEventArgs e)
        {
        }

        private void ClearUI()
        {
            txtPoNumber.Text = "";
            txtBatchNo.Text = "";
            txtReference.Text = "";
            txtAdvTaxRate.Text = "";
            txtAdditionalCharges.Text = "";
            txtDiscountedAmount.Text = "";
            txtPurchaseDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            lblGross.Text = lblDiscount.Text = lblAdvTaxAmount.Text = lblNetAmount.Text = "";
            gvPurchaseDetails.DataSource = null;
            gvPurchaseDetails.DataBind();
            Session["PurchaseDetails"] = new List<PurchaseLineItem>();
        }
    }
}