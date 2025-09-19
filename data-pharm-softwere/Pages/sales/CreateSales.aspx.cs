using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System.Linq;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Sales
{
    public partial class CreateSales : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadVoucherNumber();
                LoadTaxBillType();
                LoadProduct();
                LoadCustomer();
                LoadSalesmen();
                ClearUI();
            }
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

                string prefix = settings.SalesHead;

                var lastVoucher = _context.Sales
                    .Where(p => p.VoucherType == VoucherType.SIR && p.VoucherNumber.Contains("-"))
                    .OrderByDescending(p => p.SalesId)
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

        private void LoadTaxBillType()
        {
            ddlAdvTaxType.Items.Clear();
            ddlAdvTaxType.Items.Add(new ListItem("Net", "Net"));
            ddlAdvTaxType.Items.Add(new ListItem("Gross", "Gross"));
            ddlAdvTaxType.SelectedIndex = 0;
            ddlGstType.Items.Clear();
            ddlGstType.Items.Add(new ListItem("Net", "Net"));
            ddlGstType.Items.Add(new ListItem("Gross", "Gross"));
            ddlGstType.SelectedIndex = 0;
            ddlBillType.Items.Clear();
            ddlBillType.Items.Add(new ListItem("Normal", "Normal"));
            ddlBillType.Items.Add(new ListItem("Availability", "Availability"));
            ddlBillType.SelectedIndex = 0;
        }

        protected void txtCustomerCode_TextChanged(object sender, EventArgs e)
        {
            var customer = _context.Customers.Include(v => v.Account)
                            .FirstOrDefault(v => v.AccountId.ToString() == txtCustomerCode.Text.Trim());

            if (customer != null)
            {
                ddlCustomer.SelectedValue = customer.AccountId.ToString();
                LoadTown(customer);
                //txtAdvTaxRate.Text = customer.AdvTaxRate.ToString("0.#");
                //TotalValuesChanged(sender, e);
                lblMessage.Text = "";
            }
            else
            {
                ddlCustomer.SelectedIndex = 0;
                //txtAdvTaxRate.Text = "0.00";
                ClearTown();
                lblMessage.Text = "Customer code not found.";
                lblMessage.CssClass = "alert alert-warning";
            }
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlCustomer.SelectedValue))
            {
                var customer = _context.Customers.Include(v => v.Account)
                            .FirstOrDefault(v => v.AccountId.ToString() == ddlCustomer.SelectedValue);

                txtCustomerCode.Text = ddlCustomer.SelectedValue;
                LoadTown(customer);
                //txtAdvTaxRate.Text = customer.AdvTaxRate.ToString("0.#");
                //TotalValuesChanged(sender, e);
            }
            else
            {
                txtCustomerCode.Text = "";
                //txtAdvTaxRate.Text = "0.00";
                ClearTown();
            }
        }

        private void LoadCustomer()
        {
            try
            {
                var customers = _context.Customers
                   .Include(v => v.Account)
                   .OrderBy(v => v.Account.AccountName)
                   .Select(v => new
                   {
                       v.AccountId,
                       AccountName = v.Account.AccountName,
                   })
                   .ToList();

                ddlCustomer.Items.Clear();
                ddlCustomer.DataSource = customers;
                ddlCustomer.DataTextField = "AccountName";
                ddlCustomer.DataValueField = "AccountId";
                ddlCustomer.DataBind();
                //txtAdvTaxRate.Text = "0";

                ddlCustomer.Items.Insert(0, new ListItem("-- Select Customer --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading customers: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        private void LoadProduct()
        {
            //try
            //{
            //    var products = _context.Products
            //   .ToList();

            //    ddlProduct.DataSource = products;
            //    ddlProduct.DataTextField = "Name";
            //    ddlProduct.DataValueField = "ProductID";
            //    ddlProduct.DataBind();
            //    ddlProduct.Items.Insert(0, new ListItem("-- Select Product --", ""));
            //}
            //catch (Exception ex)
            //{
            //    lblMessage.Text = "Error loading products: " + ex.Message;
            //    lblMessage.CssClass = "alert alert-danger mt-3";
            //}
        }

        private void LoadSalesmen()
        {
            try
            {
                // Booker
                var bookers = _context.SalesmanTowns
                    .Where(st => st.AssignmentType == AssignmentType.Booker)
                    .Select(st => st.Salesman)
                    .Distinct()
                    .OrderBy(s => s.Name)
                    .ToList();

                ddlSalesmanBooker.DataSource = bookers;
                ddlSalesmanBooker.DataTextField = "Name";
                ddlSalesmanBooker.DataValueField = "SalesmanID";
                ddlSalesmanBooker.DataBind();
                ddlSalesmanBooker.Items.Insert(0, new ListItem("-- Select Booker --", ""));

                // Supplier
                var suppliers = _context.SalesmanTowns
                    .Where(st => st.AssignmentType == AssignmentType.Supplier)
                    .Select(st => st.Salesman)
                    .Distinct()
                    .OrderBy(s => s.Name)
                    .ToList();

                ddlSalesmanSupplier.DataSource = suppliers;
                ddlSalesmanSupplier.DataTextField = "Name";
                ddlSalesmanSupplier.DataValueField = "SalesmanID";
                ddlSalesmanSupplier.DataBind();
                ddlSalesmanSupplier.Items.Insert(0, new ListItem("-- Select Supplier --", ""));

                // Driver
                var drivers = _context.SalesmanTowns
                    .Where(st => st.AssignmentType == AssignmentType.Driver)
                    .Select(st => st.Salesman)
                    .Distinct()
                    .OrderBy(s => s.Name)
                    .ToList();

                ddlSalesmanDriver.DataSource = drivers;
                ddlSalesmanDriver.DataTextField = "Name";
                ddlSalesmanDriver.DataValueField = "SalesmanID";
                ddlSalesmanDriver.DataBind();
                ddlSalesmanDriver.Items.Insert(0, new ListItem("-- Select Driver --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading salesmen: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        private void LoadTown(Models.Customer customer)
        {
            var town = _context.Towns.FirstOrDefault(t => t.TownID == customer.TownID);
            txtTown.Text = town != null ? town.Name : string.Empty;

            LoadSalesmen();
            PreselectSalesmenByTown(customer.TownID);
        }

        private void PreselectSalesmenByTown(int townId)
        {
            try
            {
                // Booker
                var bookerId = _context.SalesmanTowns
                    .Where(st => st.AssignmentType == AssignmentType.Booker && st.TownID == townId)
                    .Select(st => st.SalesmanID)
                    .FirstOrDefault();

                if (bookerId != 0)
                    ddlSalesmanBooker.SelectedValue = bookerId.ToString();

                // Supplier
                var supplierId = _context.SalesmanTowns
                    .Where(st => st.AssignmentType == AssignmentType.Supplier && st.TownID == townId)
                    .Select(st => st.SalesmanID)
                    .FirstOrDefault();

                if (supplierId != 0)
                    ddlSalesmanSupplier.SelectedValue = supplierId.ToString();

                // Driver
                var driverId = _context.SalesmanTowns
                    .Where(st => st.AssignmentType == AssignmentType.Driver && st.TownID == townId)
                    .Select(st => st.SalesmanID)
                    .FirstOrDefault();

                if (driverId != 0)
                    ddlSalesmanDriver.SelectedValue = driverId.ToString();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: pre-selecting salesmen by town: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        protected void Load_ACBalance(object sender, EventArgs e)
        {
            try
            {
                if (int.TryParse(txtCustomerCode.Text, out int customerId))
                {
                    var result = _context.Data
                        .Where(a => a.AccountId == customerId)
                        .GroupBy(a => a.AccountId)
                        .Select(g => new
                        {
                            TotalDr = g.Sum(x => (decimal?)(x.Dr ?? 0)) ?? 0,
                            TotalCr = g.Sum(x => (decimal?)(x.Cr ?? 0)) ?? 0
                        })
                        .FirstOrDefault();

                    if (result != null)
                    {
                        decimal netBalance = result.TotalDr - result.TotalCr;
                        lblACBal.Text = $"{netBalance:N0}";
                        lblACBal.CssClass = "badge bg-success text-white rounded-pill fs-6 px-3 py-2 shadow-sm";
                    }
                    else
                    {
                        lblACBal.Text = "No Transactions";
                    }

                   btnACBal.Text = "<i class='bi bi-arrow-clockwise me-1'></i> Refresh";
                }
                else
                {
                    lblMessage.Text = "Please enter/select a valid Customer.";
                    lblMessage.CssClass = "alert alert-danger mt-3";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error calculating balance: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        protected void TotalValuesChanged(object sender, EventArgs e)
        {
        }

        private void ClearUI()
        {
            txtTown.Text = "";
            lblACBal.Text = "A/C Bal";
            //txtBatchNo.Text = "";
            //txtReference.Text = "";
            //txtAdvTaxRate.Text = "";
            //txtAdditionalCharges.Text = "";
            txtPurchaseDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

            //gvPurchaseDetails.DataSource = null;
            //gvPurchaseDetails.DataBind();
            //Session["PurchaseDetails"] = new List<PurchaseLineItem>();

            //SetDefaultFooterValues();
            ClearTown();
        }

        private void ClearTown()
        {
            txtTown.Text = "";
            ddlSalesmanBooker.SelectedIndex = 0;
            ddlSalesmanSupplier.SelectedIndex = 0;
            ddlSalesmanDriver.SelectedIndex = 0;
        }
    }
}