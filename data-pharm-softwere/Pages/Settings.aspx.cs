using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages
{
    public partial class Settings : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();
        private Setting currentSetting;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAccounts();
                LoadSettings();
            }
        }

        private void BindAccounts()
        {
            var accounts = _context.Accounts
                .Where(a => a.AccountType == "STOCK IN HAND")
                .Select(a => new
                {
                    a.AccountId,
                    DisplayName = a.AccountId + " - " + a.AccountName
                })
                .ToList();

            ddlStockAccounts.DataSource = accounts;
            ddlStockAccounts.DataTextField = "DisplayName";
            ddlStockAccounts.DataValueField = "AccountId";
            ddlStockAccounts.DataBind();

            ddlStockAccounts.Items.Insert(0, new ListItem("-- Select Account --", ""));
        }

        private void LoadSettings()
        {
            currentSetting = _context.Settings.FirstOrDefault();

            if (currentSetting != null)
            {
                txtCompanyName.Text = currentSetting.CompanyName;
                txtCurrency.Text = currentSetting.DefaultCurrency;
                txtAddress.Text = currentSetting.Address;
                txtStockInHand.Text = currentSetting.StockInHandAccountNo;
                txtPurchaseHead.Text = (!string.IsNullOrEmpty(currentSetting.PurchaseHead) && currentSetting.PurchaseHead != "P")
                    ? currentSetting.PurchaseHead
                    : string.Empty;
                txtPurchaseReturnHead.Text = (!string.IsNullOrEmpty(currentSetting.PurchaseReturnHead) && currentSetting.PurchaseReturnHead != "PR")
                    ? currentSetting.PurchaseReturnHead
                    : string.Empty;
                txtTransferInHead.Text = (!string.IsNullOrEmpty(currentSetting.TransferInHead) && currentSetting.TransferInHead != "TI")
                    ? currentSetting.TransferInHead
                    : string.Empty;
                txtTransferOutHead.Text = (!string.IsNullOrEmpty(currentSetting.TransferOutHead) && currentSetting.TransferOutHead != "TO")
                    ? currentSetting.TransferOutHead
                    : string.Empty;
                txtSalesHead.Text = (!string.IsNullOrEmpty(currentSetting.SalesHead) && currentSetting.SalesHead != "S")
                    ? currentSetting.SalesHead
                    : string.Empty;
                txtSalesReturnHead.Text = (!string.IsNullOrEmpty(currentSetting.SalesReturnHead) && currentSetting.SalesReturnHead != "SR")
                    ? currentSetting.SalesReturnHead
                    : string.Empty;

                if (!string.IsNullOrEmpty(currentSetting.StockInHandAccountNo))
                {
                    var item = ddlStockAccounts.Items.FindByValue(currentSetting.StockInHandAccountNo);
                    if (item != null)
                        ddlStockAccounts.SelectedValue = currentSetting.StockInHandAccountNo;
                }
            }
        }

        protected void txtStockInHand_TextChanged(object sender, EventArgs e)
        {
            string enteredValue = txtStockInHand.Text.Trim();

            var account = _context.Accounts
                .FirstOrDefault(a => a.AccountType == "STOCK IN HAND" &&
                                    (a.AccountId.ToString() == enteredValue ||
                                     a.AccountName.Contains(enteredValue)));

            if (account != null)
            {
                ddlStockAccounts.SelectedValue = account.AccountId.ToString();
            }
            else
            {
                ddlStockAccounts.SelectedIndex = 0;
                lblMessage.Text = "Invalid Stock Account";
                lblMessage.CssClass = "alert alert-danger";
            }
        }

        protected void ddlStockAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlStockAccounts.SelectedValue))
            {
                txtStockInHand.Text = ddlStockAccounts.SelectedValue;
            }
            else
            {
                txtStockInHand.Text = "";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string enteredValue = txtStockInHand.Text.Trim();

                    var account = _context.Accounts
                        .FirstOrDefault(a => a.AccountType == "STOCK IN HAND" &&
                                            (a.AccountId.ToString() == enteredValue ||
                                             a.AccountName.Contains(enteredValue)));

                    if (account == null)
                    {
                        lblMessage.Text = "Invalid Stock In Hand Account.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    currentSetting = _context.Settings.FirstOrDefault();

                    if (currentSetting == null)
                    {
                        currentSetting = new Setting
                        {
                            CompanyName = txtCompanyName.Text.Trim(),
                            DefaultCurrency = txtCurrency.Text.Trim(),
                            Address = txtAddress.Text.Trim(),
                            StockInHandAccountNo = txtStockInHand.Text.Trim(),
                            PurchaseHead = string.IsNullOrEmpty(txtPurchaseHead.Text.Trim()) ? "P" : txtPurchaseHead.Text.Trim(),
                            PurchaseReturnHead = string.IsNullOrEmpty(txtPurchaseReturnHead.Text.Trim()) ? "PR" : txtPurchaseReturnHead.Text.Trim(),
                            TransferInHead = string.IsNullOrEmpty(txtTransferInHead.Text.Trim()) ? "TI" : txtTransferInHead.Text.Trim(),
                            TransferOutHead = string.IsNullOrEmpty(txtTransferOutHead.Text.Trim()) ? "TO" : txtTransferOutHead.Text.Trim(),
                            SalesHead = string.IsNullOrEmpty(txtSalesHead.Text.Trim()) ? "S" : txtSalesHead.Text.Trim(),
                            SalesReturnHead = string.IsNullOrEmpty(txtSalesReturnHead.Text.Trim()) ? "SR" : txtSalesReturnHead.Text.Trim(),
                            CreatedAt = DateTime.Now
                        };

                        _context.Settings.Add(currentSetting);
                    }
                    else
                    {
                        currentSetting.CompanyName = txtCompanyName.Text.Trim();
                        currentSetting.DefaultCurrency = txtCurrency.Text.Trim();
                        currentSetting.Address = txtAddress.Text.Trim();
                        currentSetting.StockInHandAccountNo = txtStockInHand.Text.Trim();
                        currentSetting.PurchaseHead = string.IsNullOrEmpty(txtPurchaseHead.Text.Trim()) ? "P" : txtPurchaseHead.Text.Trim();
                        currentSetting.PurchaseReturnHead = string.IsNullOrEmpty(txtPurchaseReturnHead.Text.Trim()) ? "PR" : txtPurchaseReturnHead.Text.Trim();
                        currentSetting.TransferInHead = string.IsNullOrEmpty(txtTransferInHead.Text.Trim()) ? "TI" : txtTransferInHead.Text.Trim();
                        currentSetting.TransferOutHead = string.IsNullOrEmpty(txtTransferOutHead.Text.Trim()) ? "TO" : txtTransferOutHead.Text.Trim();
                        currentSetting.SalesHead = string.IsNullOrEmpty(txtSalesHead.Text.Trim()) ? "S" : txtSalesHead.Text.Trim();
                        currentSetting.SalesReturnHead = string.IsNullOrEmpty(txtSalesReturnHead.Text.Trim()) ? "SR" : txtSalesReturnHead.Text.Trim();
                    }

                    _context.SaveChanges();
                    lblMessage.Text = "Settings updated successfully!";
                    lblMessage.CssClass = "alert alert-success";
                    Response.Redirect("/");
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "alert alert-danger";
                }
            }
        }
    }
}