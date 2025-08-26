using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Linq;
using System.Security.Principal;
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