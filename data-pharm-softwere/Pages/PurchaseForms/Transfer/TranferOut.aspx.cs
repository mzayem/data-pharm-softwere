using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.PurchaseForms.Transfer
{
    public partial class TranferOut : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadVoucherNumber();
                Session["PurchaseDetails"] = new List<PurchaseLineItem>();
                LoadVendors();
                LoadTaxType();
                LoadAllAvailableBatches();
                LoadProduct();
                ClearUI();
                txtAdvTaxRate.Text = "0.00";
            }
        }

        private void SetDefaultFooterValues()
        {
            // Footer default values
            txtAdditionalCharges.Text = "0.00";
            lblGross.Text = "0.00";
            lblDiscount.Text = "0.00";
            lblAdvTaxAmount.Text = "0.00";
            lblNetAmount.Text = "0.00";
            ddlAdvTaxType.SelectedValue = "Net";
            ddlGstType.SelectedValue = "Net";
        }

        private void LoadVoucherNumber()
        {
            try
            {
                var settings = _context.Settings.FirstOrDefault();
                if (settings == null)
                {
                    lblMessage.Text = "Settings not configured.";
                    lblMessage.CssClass = "alert alert-warning mt-3";
                    return;
                }

                string prefix = settings.TransferOutHead;

                var lastVoucher = _context.Purchases
                    .Where(p => p.VoucherType == VoucherType.TOR && p.VoucherNumber.Contains("-"))
                    .OrderByDescending(p => p.PurchaseId)
                    .Select(p => p.VoucherNumber)
                    .FirstOrDefault();

                int lastNumber = 0;

                if (!string.IsNullOrEmpty(lastVoucher))
                {
                    var numPart = lastVoucher.Split('-').Last();
                    int.TryParse(numPart, out lastNumber);
                }

                string nextVoucher = prefix + "-" + (lastNumber + 1).ToString("D3");
                txtVoucherNo.Text = nextVoucher;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error fetching Voucher No: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        protected void txtVendorCode_TextChanged(object sender, EventArgs e)
        {
            var vendor = _context.Vendors
                        .Include(v => v.Account)
                        .FirstOrDefault(v => v.AccountId.ToString() == txtVendorCode.Text.Trim());

            if (vendor == null)
            {
                ddlVendor.SelectedIndex = 0;
                txtAdvTaxRate.Text = "0.00";
                lblMessage.Text = "Account not found.";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            if (!vendor.Account.AccountType.Equals("DISTRIBUTOR", StringComparison.OrdinalIgnoreCase))
            {
                ddlVendor.SelectedIndex = 0;
                txtAdvTaxRate.Text = "0.00";
                lblMessage.Text = "Account is not a Distributor.";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            // Valid distributor
            ddlVendor.SelectedValue = vendor.AccountId.ToString();
            txtAdvTaxRate.Text = vendor.AdvTaxRate.ToString("0.#");
            TotalValuesChanged(sender, e);
            lblMessage.Text = "";
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlVendor.SelectedValue))
            {
                var vendor = _context.Vendors
                    .Include(v => v.Account)
                    .FirstOrDefault(v => v.AccountId.ToString() == ddlVendor.SelectedValue &&
                                         v.Account.AccountType.Equals("DISTRIBUTOR", StringComparison.OrdinalIgnoreCase));

                txtVendorCode.Text = ddlVendor.SelectedValue;
                txtAdvTaxRate.Text = vendor.AdvTaxRate.ToString("0.#");
                TotalValuesChanged(sender, e);
            }
            else
            {
                txtVendorCode.Text = "";
                txtAdvTaxRate.Text = "0.00";
            }
        }

        private void LoadVendors()
        {
            try
            {
                var vendors = _context.Vendors
                    .Include(v => v.Account)
                    .Where(v => v.Account.AccountType.Equals("DISTRIBUTOR", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(v => v.Account.AccountName)
                    .Select(v => new
                    {
                        v.AccountId,
                        AccountName = v.Account.AccountName,
                        v.AdvTaxRate
                    })
                    .ToList();

                ddlVendor.Items.Clear();
                ddlVendor.DataSource = vendors;
                ddlVendor.DataTextField = "AccountName";
                ddlVendor.DataValueField = "AccountId";
                ddlVendor.DataBind();
                txtAdvTaxRate.Text = "0";

                ddlVendor.Items.Insert(0, new ListItem("-- Select Distributor --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading Distributor: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        private void LoadProduct()
        {
            try
            {
                var products = _context.Products
               .ToList();

                ddlProduct.DataSource = products;
                ddlProduct.DataTextField = "Name";
                ddlProduct.DataValueField = "ProductID";
                ddlProduct.DataBind();
                ddlProduct.Items.Insert(0, new ListItem("-- Select Product --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading products: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        private void LoadTaxType()
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
                .Where(b => !addedBatchNos.Contains(b.BatchNo.Trim().ToLower())
                            && b.ExpiryDate >= DateTime.Today)
                .OrderBy(b => b.ExpiryDate)
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

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlProduct.SelectedValue))
            {
                txtProductId.Text = ddlProduct.SelectedValue;
            }
            else
            {
                txtProductId.Text = "";
                ddlBatch.Items.Clear();
                ddlBatch.Items.Insert(0, new ListItem("-- Select Batch --", ""));
            }
            txtProductId_TextChanged(sender, e);
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
                .Where(b => !addedBatchNos.Contains(b.BatchNo.Trim().ToLower())
                            && b.ExpiryDate >= DateTime.Today)
                .OrderBy(b => b.ExpiryDate)
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

            if (ddlProduct.Items.FindByValue(product.ProductID.ToString()) != null)
            {
                ddlProduct.ClearSelection();
                ddlProduct.SelectedValue = product.ProductID.ToString();
            }

            // Load batches for product (not in purchase)
            LoadBatches(product.ProductID, addedBatchNos);
        }

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            int qty;
            if (!int.TryParse(txtQty.Text, out qty))
            {
                lblMessage.Text = "Invalid quantity!";
                lblMessage.CssClass = "alert alert-warning";
                txtQty.Text = "0";
                return;
            }

            // Ensure BatchNo is selected
            if (string.IsNullOrEmpty(ddlBatch.SelectedValue))
            {
                lblMessage.Text = "Please select a batch first!";
                lblMessage.CssClass = "alert alert-warning";
            }

            string selectedBatch = ddlBatch.SelectedValue;

            var batch = _context.BatchesStock
                .Include("Product")
                .FirstOrDefault(b => b.BatchNo.Trim().Equals(selectedBatch, StringComparison.OrdinalIgnoreCase));

            if (batch == null)
            {
                lblMessage.Text = "Invalid batch selected!";
                lblMessage.CssClass = "alert alert-danger";
                txtQty.Text = "0";
                return;
            }

            // Validate against AvailableQty
            if (qty > batch.AvailableQty)
            {
                lblMessage.Text = "Available Quantity exceeds!";
                lblMessage.CssClass = "alert alert-warning";

                qty = batch.AvailableQty; // cap at max allowed
                txtQty.Text = qty.ToString();
            }

            KeepFocus(sender as Control);
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

                if (ddlProduct.Items.FindByValue(batch.ProductID.ToString()) != null)
                {
                    ddlProduct.ClearSelection();
                    ddlProduct.SelectedValue = batch.ProductID.ToString();
                }

                var addedBatchNos = (Session["PurchaseDetails"] as List<PurchaseLineItem>)
                    ?.Select(i => i.BatchNo.Trim().ToLower())
                    .ToList() ?? new List<string>();

                if (batch.AvailableQty <= 0)
                {
                    lblMessage.Text = "Batch has no Available quantity. Cannot proceed!";
                    lblMessage.CssClass = "alert alert-warning";
                    return;
                }
                LoadBatches(batch.ProductID, addedBatchNos, selectedBatch);
            }
        }

        protected void txtBatchNo_TextChanged(object sender, EventArgs e)
        {
            string batchNo = txtBatchNo.Text.Trim();
            if (string.IsNullOrEmpty(batchNo))
                return;

            int productId;

            var batch = _context.BatchesStock
                .FirstOrDefault(b => b.BatchNo.Trim().Equals(batchNo, StringComparison.OrdinalIgnoreCase));

            if (batch == null)
            {
                lblMessage.Text = "Batch not found. Enter a valid batch number.";

                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            if (batch.AvailableQty <= 0)
            {
                lblMessage.Text = "Batch has no quantity. Cannot proceed!";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            // If batch found, set IDs
            hiddenProductId.Value = batch.ProductID.ToString();
            txtProductId.Text = batch.ProductID.ToString();
            productId = batch.ProductID;

            if (ddlProduct.Items.FindByValue(productId.ToString()) != null)
            {
                ddlProduct.ClearSelection();
                ddlProduct.SelectedValue = productId.ToString();
            }

            // Continue loading batches
            var addedBatchNos = (Session["PurchaseDetails"] as List<PurchaseLineItem>)
                ?.Select(i => i.BatchNo.Trim().ToLower())
                .ToList() ?? new List<string>();

            LoadBatches(productId, addedBatchNos, batchNo);
        }

        private void LoadBatches(long? productId, List<string> excludeBatchNos, string selectedBatch = null)
        {
            ddlBatch.Items.Clear();

            var query = _context.BatchesStock
         .Where(b => !excludeBatchNos.Contains(b.BatchNo.Trim().ToLower())
                     && b.ExpiryDate >= DateTime.Today);

            if (productId.HasValue)
            {
                query = query.Where(b => b.ProductID == productId.Value);
            }

            var batches = query
            .OrderBy(b => b.ExpiryDate)
            .Select(b => new { b.BatchNo, b.ExpiryDate })
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
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public string BatchNo { get; set; }
            public DateTime ExpiryDate { get; set; }
            public int Qty { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal DiscountPercent { get; set; }
            public decimal GSTPercent { get; set; }
            public string GstType { get; set; }
            public int BonusQty { get; set; }
            public int AvailableQty { get; set; }
            public bool IsManualBonus { get; set; }
            public decimal GrossAmount => Qty * UnitPrice;
            public decimal DiscountAmount => Math.Round(GrossAmount * (DiscountPercent / 100m), 2);
            public decimal TaxBase => GstType == "Net" ? (GrossAmount - DiscountAmount) : GrossAmount;
            public decimal GSTAmount => Math.Round(TaxBase * (GSTPercent / 100m), 2);
            public decimal NetAmount => Math.Round(TaxBase + GSTAmount, 2);
            public decimal TotalAmount => NetAmount;
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

            int parsedQty;
            if (!int.TryParse(txtQty.Text, out parsedQty) || parsedQty <= 0)
            {
                lblMessage.Text = "Please enter some quantity.";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            if (parsedQty > batchStock.AvailableQty)
            {
                lblMessage.Text = "Available Quantity exceeds!";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            var list = Session["PurchaseDetails"] as List<PurchaseLineItem> ?? new List<PurchaseLineItem>();

            var item = new PurchaseLineItem
            {
                BatchStockID = batchStock.BatchStockID,
                ProductId = batchStock.ProductID.ToString(),
                ProductName = batchStock.Product?.Name,
                BatchNo = batchStock.BatchNo,
                ExpiryDate = batchStock.ExpiryDate,
                Qty = parsedQty,
                UnitPrice = batchStock.DP,

                DiscountPercent = batchStock.Product?.PurchaseDiscount ?? 0,
                GSTPercent = batchStock.Product?.ReqGST ?? 0,
                GstType = ddlGstType.SelectedValue,
                BonusQty = CalculateBonusQty(batchStock.ProductID, parsedQty)
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
            LoadProduct();
        }

        // --- Helpers ---
        private List<PurchaseLineItem> GetPurchaseList()
        {
            var list = Session["PurchaseDetails"] as List<PurchaseLineItem>;
            if (list == null)
            {
                list = new List<PurchaseLineItem>();
                Session["PurchaseDetails"] = list;
            }
            return list;
        }

        private void BindGrid()
        {
            var list = GetPurchaseList();
            gvPurchaseDetails.DataSource = list;
            gvPurchaseDetails.DataBind();
        }

        protected void gvPurchaseDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Example: add serial number dynamically
                Label lblSrNo = e.Row.FindControl("lblSrNo") as Label;
                if (lblSrNo != null)
                {
                    lblSrNo.Text = (e.Row.RowIndex + 1).ToString();
                }

                // Force Qty column into edit mode automatically
                if (gvPurchaseDetails.EditIndex == e.Row.RowIndex)
                {
                    TextBox txtQty = e.Row.FindControl("txtEditQty") as TextBox;
                    if (txtQty != null)
                    {
                        txtQty.Focus();
                    }
                }
            }
        }

        protected void Qty_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;

            int id = Convert.ToInt32(gvPurchaseDetails.DataKeys[row.RowIndex].Value);
            int qty = int.TryParse(txt.Text, out int parsed) ? parsed : 0;

            var list = GetPurchaseList();
            var item = list.FirstOrDefault(x => x.BatchStockID == id);
            if (item != null)
            {
                if (qty > item.AvailableQty)
                {
                    lblMessage.Text = "Available Quantity exceeds!.";
                    lblMessage.CssClass = "alert alert-danger";
                    qty = item.AvailableQty;
                    txt.Text = qty.ToString();
                }

                item.Qty = qty;
                if (!item.IsManualBonus)
                {
                    if (int.TryParse(item.ProductId, out int prodId))
                        item.BonusQty = CalculateBonusQty(prodId, qty);
                    else
                        item.BonusQty = 0;
                }
            }

            Session["PurchaseDetails"] = list;
            BindGrid();
            KeepFocus(sender as Control);
            UpdateTotals(list);
        }

        protected void BonusQty_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;

            int id = Convert.ToInt32(gvPurchaseDetails.DataKeys[row.RowIndex].Value);
            int bonusQty = int.TryParse(txt.Text, out int parsed) ? parsed : 0;

            var list = GetPurchaseList();
            var item = list.FirstOrDefault(x => x.BatchStockID == id);
            if (item != null)
            {
                item.BonusQty = bonusQty;
                item.IsManualBonus = true;
            }

            Session["PurchaseDetails"] = list;
            BindGrid();
            KeepFocus(sender as Control);
        }

        private int CalculateBonusQty(int productId, int totalQty)
        {
            if (totalQty <= 0) return 0;

            // load active slabs for the product, largest MinQty first
            var slabs = _context.ProductBonuses
                .Where(b => b.ProductID == productId && b.IsActive)
                .OrderByDescending(b => b.MinQty)
                .Select(b => new { b.MinQty, b.BonusItems })
                .ToList();

            if (!slabs.Any()) return 0;

            int remaining = totalQty;
            int totalBonus = 0;

            foreach (var slab in slabs)
            {
                if (remaining < slab.MinQty) continue;

                int count = remaining / slab.MinQty;
                totalBonus += count * slab.BonusItems;

                remaining -= count * slab.MinQty;

                if (remaining == 0) break;
            }

            return totalBonus;
        }

        protected void gvPurchaseDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var list = GetPurchaseList();

            int id = Convert.ToInt32(gvPurchaseDetails.DataKeys[e.RowIndex].Value);
            var item = list.FirstOrDefault(x => x.BatchStockID == id);
            if (item != null)
            {
                list.Remove(item);
            }

            Session["PurchaseDetails"] = list;

            BindGrid();
            UpdateTotals(list);
            LoadAllAvailableBatches();
        }

        private void UpdateTotals(List<PurchaseLineItem> list)
        {
            decimal gross = list.Sum(i => i.GrossAmount);
            decimal discount = list.Sum(i => i.DiscountAmount);
            decimal advTaxRate = decimal.TryParse(txtAdvTaxRate.Text, out var a) ? a : 0;
            decimal additional = decimal.TryParse(txtAdditionalCharges.Text, out var add) ? add : 0;

            decimal advTax = ddlAdvTaxType.SelectedValue == "Net"
            ? (gross - discount) * advTaxRate / 100
            : gross * advTaxRate / (100 + advTaxRate);

            decimal net = list.Sum(i => i.NetAmount) + advTax + additional;

            lblGross.Text = gross.ToString("N2");
            lblDiscount.Text = discount.ToString("N2");
            lblAdvTaxAmount.Text = advTax.ToString("N2");
            lblNetAmount.Text = net.ToString("N2");
        }

        private void KeepFocus(Control control)
        {
            if (control != null)
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "setFocus",
                    $"setTimeout(function(){{document.getElementById('{control.ClientID}').focus();}}, 0);",
                    true
                );
            }
        }

        protected void TotalValuesChanged(object sender, EventArgs e)
        {
            var list = GetPurchaseList();
            foreach (var item in list)
            {
                item.GstType = ddlGstType.SelectedValue;
            }

            Session["PurchaseDetails"] = list;

            BindGrid();
            UpdateTotals(list);

            KeepFocus(sender as Control);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var list = Session["PurchaseDetails"] as List<PurchaseLineItem>;
            if (list == null || !list.Any())
            {
                lblMessage.Text = "No Items added.";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }
            try
            {
                decimal ParseDecimal(string input)
                {
                    decimal.TryParse(input,
                        NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture,
                        out var result);
                    return result;
                }

                var purchase = new Models.Purchase
                {
                    VoucherNumber = txtVoucherNo.Text,
                    PurchaseDate = DateTime.TryParse(txtPurchaseDate.Text, out var d) ? d : DateTime.Now,
                    PoNumber = txtPoNumber.Text,
                    Reference = txtReference.Text,
                    AdvTaxOn = (TaxBaseType)Enum.Parse(typeof(TaxBaseType), ddlAdvTaxType.SelectedValue),
                    GSTType = (TaxBaseType)Enum.Parse(typeof(TaxBaseType), ddlGstType.SelectedValue),
                    VoucherType = VoucherType.POR,
                    AdvTaxRate = ParseDecimal(txtAdvTaxRate.Text),
                    AdvTaxAmount = ParseDecimal(lblAdvTaxAmount.Text),
                    AdditionalCharges = ParseDecimal(txtAdditionalCharges.Text),
                    DiscountedAmount = ParseDecimal(lblDiscount.Text),
                    GrossAmount = ParseDecimal(lblGross.Text),
                    NetAmount = ParseDecimal(lblNetAmount.Text),
                    VendorId = int.TryParse(ddlVendor.SelectedValue, out var v) ? v : 0,
                    CreatedBy = "system",
                    CreatedAt = DateTime.Now,
                    Posted = false
                };

                foreach (var item in list)
                {
                    var detail = new PurchaseDetail
                    {
                        BatchStockID = item.BatchStockID,
                        NetAmount = item.NetAmount,
                        CreatedAt = DateTime.Now
                    };

                    purchase.PurchaseDetails.Add(detail);

                    var batch = _context.BatchesStock.FirstOrDefault(b => b.BatchStockID == item.BatchStockID);
                    if (batch != null)
                    {
                        batch.AvailableQty -= item.Qty;
                        batch.BonusQty -= item.BonusQty;
                        batch.UpdatedAt = DateTime.Now;
                        batch.UpdatedBy = "system";
                    }
                }

                _context.Purchases.Add(purchase);
                _context.SaveChanges();

                var vendor = _context.Accounts.FirstOrDefault(a => a.AccountId == purchase.VendorId);
                var stockInHandAccount = _context.Settings
                    .Select(s => s.StockInHandAccountNo)
                    .FirstOrDefault();

                int stockInHandAccountId;
                if (!int.TryParse(stockInHandAccount, out stockInHandAccountId))
                {
                    lblMessage.Text = "Invalid Stock In Hand account number in settings.";
                    lblMessage.CssClass = "alert alert-danger mt-3";
                    return;
                }

                var stockInHand = _context.Accounts.FirstOrDefault(a => a.AccountId == stockInHandAccountId);

                string[] parts = purchase.VoucherNumber.Split('-');
                int voucherNo = 0;

                if (parts.Length > 1)
                {
                    int.TryParse(parts[parts.Length - 1], out voucherNo);
                }

                if (vendor != null)
                {
                    var vendorEntry = new Models.Data
                    {
                        Vr = voucherNo,
                        VrDate = DateTime.Now,
                        AccountId = vendor.AccountId,
                        AccountTitle = vendor.AccountName,
                        Dr = purchase.NetAmount,
                        Cr = 0,
                        Type = vendor.AccountType,
                        Status = "unconfirmed",
                        Creator = "system",
                        Vtype = purchase.VoucherType.ToString(),
                        Remarks = $"TOR {purchase.PurchaseId}, {purchase.PurchaseDate:yyyy-MM-dd}, {purchase.PoNumber}, {purchase.Reference}"
                    };

                    var stockEntry = new Models.Data
                    {
                        Vr = voucherNo,
                        VrDate = DateTime.Now,
                        AccountId = stockInHand.AccountId,
                        AccountTitle = stockInHand.AccountName,
                        Cr = purchase.NetAmount,
                        Dr = 0,
                        Type = stockInHand.AccountType,
                        Status = "unconfirmed",
                        Creator = "system",
                        Vtype = "TORS",
                        Remarks = $"TORS {purchase.PurchaseId}, {purchase.PurchaseDate:yyyy-MM-dd}, {purchase.PoNumber}, {purchase.Reference}"
                    };

                    _context.Data.Add(vendorEntry);
                    _context.Data.Add(stockEntry);
                    _context.SaveChanges();
                }

                lblMessage.Text = "Tranfer out Initiated successfully.";
                lblMessage.CssClass = "alert alert-success mt-3";
                ClearUI();
            }
            catch (DbEntityValidationException dbEx)
            {
                // Detailed EF validation error dump
                var errors = new System.Text.StringBuilder();
                foreach (var eve in dbEx.EntityValidationErrors)
                {
                    errors.AppendLine($"Entity: {eve.Entry.Entity.GetType().Name}, State: {eve.Entry.State}");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errors.AppendLine($"-- Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                    }
                }

                lblMessage.Text = "Entity Validation Error:<br/><pre>" + errors.ToString() + "</pre>";
                lblMessage.CssClass = "alert alert-danger";
            }
            catch (Exception ex)
            {
                string debugInfo = $@"
                Exception: {ex.Message}
                StackTrace: {ex.StackTrace}

                --- Form Values ---
                PurchaseDate: {txtPurchaseDate.Text}
                PoNumber: {txtPoNumber.Text}
                Reference: {txtReference.Text}
                AdvTaxType: {ddlAdvTaxType.SelectedValue}
                GstType: {ddlGstType.SelectedValue}
                VendorId: {ddlVendor.SelectedValue}

                --- Labels ---
                lblGross: {lblGross.Text}
                lblDiscount: {lblDiscount.Text}
                lblAdvTaxAmount: {lblAdvTaxAmount.Text}
                lblNetAmount: {lblNetAmount.Text}

                --- Textboxes ---
                txtAdvTaxRate: {txtAdvTaxRate.Text}
                txtAdditionalCharges: {txtAdditionalCharges.Text}";

                lblMessage.Text = "Unexpected Error:<br/><pre>" + debugInfo + "</pre>";
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
            txtPurchaseDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

            gvPurchaseDetails.DataSource = null;
            gvPurchaseDetails.DataBind();
            Session["PurchaseDetails"] = new List<PurchaseLineItem>();

            SetDefaultFooterValues();
        }
    }
}