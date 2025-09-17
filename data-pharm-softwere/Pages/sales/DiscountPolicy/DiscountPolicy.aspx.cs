using data_pharm_softwere.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.sales.DiscountPolicy
{
    public partial class DiscountPolicy : System.Web.UI.Page
    {
        private DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCustomers();
                LoadProduct();

                if (int.TryParse(Request.QueryString["id"], out int accountId))
                {
                    var item = ddlCustomer.Items.FindByValue(accountId.ToString());
                    if (item == null)
                    {
                        var cust = _context.Customers
                                           .Include(c => c.Account)
                                           .Include(c => c.Town)
                                           .FirstOrDefault(c => c.AccountId == accountId);

                        if (cust != null)
                        {
                            ddlCustomer.Items.Add(new ListItem(cust.Account.AccountName, cust.AccountId.ToString()));
                        }
                    }

                    if (ddlCustomer.Items.FindByValue(accountId.ToString()) != null)
                    {
                        ddlCustomer.ClearSelection();
                        ddlCustomer.SelectedValue = accountId.ToString();
                        txtCustomerCode.Text = accountId.ToString();
                        ShowCustomerTown(accountId);
                        LoadPolicies(accountId);
                    }
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ImportInfoControl.DownloadRequested += ImportInfoControl_DownloadRequested;
        }

        private void LoadCustomers()
        {
            try
            {
                var vendors = _context.Customers
                   .Include(c => c.Account)
                   .OrderBy(c => c.Account.AccountName)
                   .Select(c => new
                   {
                       c.AccountId,
                       AccountName = c.Account.AccountName,
                   })
                   .ToList();

                ddlCustomer.Items.Clear();
                ddlCustomer.DataSource = vendors;
                ddlCustomer.DataTextField = "AccountName";
                ddlCustomer.DataValueField = "AccountId";
                ddlCustomer.DataBind();

                ddlCustomer.Items.Insert(0, new ListItem("-- Select Customer --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading vendors: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlCustomer.SelectedValue, out int accountId))
            {
                txtCustomerCode.Text = accountId.ToString();
                ShowCustomerTown(accountId);
                LoadPolicies(accountId);
            }
            else
            {
                txtCustomerCode.Text = "";
                txtTown.Text = "";
                rptBonuses.DataSource = null;
                rptBonuses.DataBind();
            }
            upAllControls.Update();
        }

        protected void txtCustomerCode_TextChanged(object sender, EventArgs e)
        {
            txtTown.Text = "";
            lblMessage.Text = "";

            if (!int.TryParse(txtCustomerCode.Text.Trim(), out int accountId))
            {
                lblMessage.Text = "Invalid customer code.";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            // If dropdown already contains this account id, just select it
            var item = ddlCustomer.Items.FindByValue(accountId.ToString());
            if (item != null)
            {
                ddlCustomer.ClearSelection();
                item.Selected = true;
                ShowCustomerTown(accountId);
                LoadPolicies(accountId);
                return;
            }

            // otherwise try to load the customer from DB and add to dropdown
            var customer = _context.Customers
                                   .Include(c => c.Account)
                                   .Include(c => c.Town)
                                   .FirstOrDefault(c => c.AccountId == accountId);

            if (customer != null)
            {
                var listItem = new ListItem(customer.Account?.AccountName ?? $"Account #{customer.AccountId}", customer.AccountId.ToString());
                ddlCustomer.Items.Add(listItem);
                ddlCustomer.ClearSelection();
                listItem.Selected = true;

                ShowCustomerTown(customer);
                LoadPolicies(accountId);
            }
            else
            {
                lblMessage.Text = "Customer not found.";
                lblMessage.CssClass = "alert alert-warning";

                // clear any previous policy listing
                rptBonuses.DataSource = null;
                rptBonuses.DataBind();
            }

            upAllControls.Update();
        }

        private void ShowCustomerTown(int accountId)
        {
            var customer = _context.Customers
                                   .Include(c => c.Town)
                                   .FirstOrDefault(c => c.AccountId == accountId);

            if (customer != null)
            {
                txtTown.Text = GetTownName(customer);
            }
            else
            {
                txtTown.Text = "";
            }
        }

        private void ShowCustomerTown(Models.Customer customer)
        {
            if (customer == null)
            {
                txtTown.Text = "";
                return;
            }

            txtTown.Text = GetTownName(customer);
        }

        private string GetTownName(Models.Customer customer)
        {
            // prefer navigation property if present
            if (customer.Town != null)
                return customer.Town.Name ?? "";

            // otherwise try to find via TownID if present
            if (customer.TownID > 0)
            {
                var town = _context.Towns.Find(customer.TownID);
                return town?.Name ?? "";
            }

            return "";
        }

        private void LoadProduct()
        {
            try
            {
                var products = _context.Products.ToList();

                ddlProduct.DataSource = products;
                ddlProduct.DataTextField = "Name";
                ddlProduct.DataValueField = "ProductID";
                ddlProduct.DataBind();
                ddlProduct.Items.Insert(0, new ListItem("-- All Product --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading products: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        private void LoadPolicies(int accountId)
        {
            var policies = _context.DiscountPolicies
                .Include(d => d.Product)
                .Where(d => d.CustomerAccountId == accountId)
                .ToList();

            Policies = policies;
            BindPolicies();
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
            }
            txtProductId_TextChanged(sender, e);
        }

        protected void txtProductId_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductId.Text))
                return;

            if (!int.TryParse(txtProductId.Text.Trim(), out int productId))
            {
                lblMessage.Text = "Invalid product code.";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            var product = _context.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                lblMessage.Text = "Product not found.";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            if (ddlProduct.Items.FindByValue(product.ProductID.ToString()) != null)
            {
                ddlProduct.ClearSelection();
                ddlProduct.SelectedValue = product.ProductID.ToString();
            }
        }

        private List<Models.DiscountPolicy> Policies
        {
            get { return Session["Policies"] as List<Models.DiscountPolicy> ?? new List<Models.DiscountPolicy>(); }
            set { Session["Policies"] = value; }
        }

        private void BindPolicies()
        {
            rptBonuses.DataSource = Policies;
            rptBonuses.DataBind();
        }

        protected void BonusFieldChanged(object sender, EventArgs e)
        {
            var list = Policies;
            RepeaterItem item = (RepeaterItem)((Control)sender).NamingContainer;
            int index = item.ItemIndex;

            if (index >= 0 && index < list.Count)
            {
                var policy = list[index];

                var txtFlatDiscount = (TextBox)item.FindControl("txtFlatDiscount");
                var txtFlatExpiry = (TextBox)item.FindControl("txtFlatExpiry");
                var txtCreditDiscount = (TextBox)item.FindControl("txtCreditDiscount");
                var txtCreditExpiry = (TextBox)item.FindControl("txtCreditExpiry");

                if (decimal.TryParse(txtFlatDiscount.Text, out var f))
                    policy.FlatDiscount = f;

                if (DateTime.TryParse(txtFlatExpiry.Text, out var fe))
                    policy.FlatDiscountExpiry = fe;

                if (decimal.TryParse(txtCreditDiscount.Text, out var c))
                    policy.CreditDiscount = c;

                if (DateTime.TryParse(txtCreditExpiry.Text, out var ce))
                    policy.CreditDiscountExpiry = ce;

                KeepFocus(sender as Control);
            }

            Policies = list;
            btnSave.Text = "Save Discount";
            upAllControls.Update();
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

        protected void rptBonuses_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                if (!int.TryParse(e.CommandArgument?.ToString(), out int index))
                {
                    lblMessage.Text = "Unable to determine item index.";
                    lblMessage.CssClass = "alert alert-warning";
                    return;
                }

                var list = Policies;
                if (index >= 0 && index < list.Count)
                {
                    var policy = list[index];

                    if (policy.DiscountPolicyID > 0)
                    {
                        try
                        {
                            using (var db = new DataPharmaContext())
                            {
                                var dbPolicy = db.DiscountPolicies.Find(policy.DiscountPolicyID);
                                if (dbPolicy != null)
                                {
                                    db.DiscountPolicies.Remove(dbPolicy);
                                    db.SaveChanges();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Warning: removed from list but failed to remove from DB: " + ex.Message;
                            lblMessage.CssClass = "alert alert-warning";
                        }
                    }

                    list.RemoveAt(index);
                    Policies = list;

                    btnSave.Text = "Save Discount";
                    BindPolicies();
                    upAllControls.Update();

                    lblMessage.Text = "Item removed successfully.";
                    lblMessage.CssClass = "alert alert-success";
                }
            }
        }

        protected void rptBonuses_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Register all textboxes
                var txtFlatDiscount = (TextBox)e.Item.FindControl("txtFlatDiscount");
                var txtFlatExpiry = (TextBox)e.Item.FindControl("txtFlatExpiry");
                var txtCreditDiscount = (TextBox)e.Item.FindControl("txtCreditDiscount");
                var txtCreditExpiry = (TextBox)e.Item.FindControl("txtCreditExpiry");

                var sm = ScriptManager.GetCurrent(Page);

                if (txtFlatDiscount != null) sm.RegisterAsyncPostBackControl(txtFlatDiscount);
                if (txtFlatExpiry != null) sm.RegisterAsyncPostBackControl(txtFlatExpiry);
                if (txtCreditDiscount != null) sm.RegisterAsyncPostBackControl(txtCreditDiscount);
                if (txtCreditExpiry != null) sm.RegisterAsyncPostBackControl(txtCreditExpiry);

                var btnRemove = (LinkButton)e.Item.FindControl("btnRemove");
                if (btnRemove != null)
                {
                    ScriptManager.GetCurrent(Page).RegisterAsyncPostBackControl(btnRemove);
                }
            }

            // Empty placeholder logic
            if (e.Item.ItemType == ListItemType.Footer)
            {
                var phEmpty = e.Item.FindControl("phEmpty") as PlaceHolder;
                if (phEmpty != null)
                    phEmpty.Visible = rptBonuses.Items.Count == 0;
            }
        }

        protected void btnAddDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlCustomer.SelectedValue))
                {
                    lblMessage.Text = "Please select a customer first.";
                    lblMessage.CssClass = "alert alert-warning";
                    return;
                }

                // parse input values safely
                decimal flatDiscount = string.IsNullOrWhiteSpace(txtFlatDiscountAdd.Text) ? 0 : Convert.ToDecimal(txtFlatDiscountAdd.Text);
                DateTime? flatExpiry = string.IsNullOrWhiteSpace(txtFlatExpiryDateAdd.Text) ? (DateTime?)null : Convert.ToDateTime(txtFlatExpiryDateAdd.Text);

                decimal creditDiscount = string.IsNullOrWhiteSpace(txtCreditDiscountAdd.Text) ? 0 : Convert.ToDecimal(txtCreditDiscountAdd.Text);
                DateTime? creditExpiry = string.IsNullOrWhiteSpace(txtCreditExpiryDateAdd.Text) ? (DateTime?)null : Convert.ToDateTime(txtCreditExpiryDateAdd.Text);

                // validation rules
                if (flatDiscount <= 0 && creditDiscount <= 0)
                {
                    lblMessage.Text = "You must enter at least one discount value.";
                    lblMessage.CssClass = "alert alert-warning";
                    return;
                }
                if (flatDiscount > 0 && flatExpiry == null)
                {
                    lblMessage.Text = "Flat discount requires an expiry date.";
                    lblMessage.CssClass = "alert alert-warning";
                    return;
                }
                if (creditDiscount > 0 && creditExpiry == null)
                {
                    lblMessage.Text = "Credit discount requires an expiry date.";
                    lblMessage.CssClass = "alert alert-warning";
                    return;
                }

                var discounts = Policies;
                int accountId = Convert.ToInt32(ddlCustomer.SelectedValue);

                using (var db = new DataPharmaContext())
                {
                    if (string.IsNullOrEmpty(ddlProduct.SelectedValue))
                    {
                        // ALL PRODUCTS
                        var allProducts = db.Products.ToList();
                        foreach (var product in allProducts)
                        {
                            if (discounts.Any(d => d.ProductID == product.ProductID && d.CustomerAccountId == accountId))
                                continue;

                            discounts.Add(new Models.DiscountPolicy
                            {
                                ProductID = product.ProductID,
                                Product = product,
                                CustomerAccountId = accountId,
                                FlatDiscount = flatDiscount,
                                FlatDiscountExpiry = flatExpiry,
                                CreditDiscount = creditDiscount,
                                CreditDiscountExpiry = creditExpiry
                            });
                        }
                    }
                    else
                    {
                        // specific product
                        int productId;
                        if (!int.TryParse(ddlProduct.SelectedValue, out productId))
                        {
                            lblMessage.Text = "Invalid Product ID.";
                            lblMessage.CssClass = "alert alert-warning";
                            return;
                        }

                        var product = db.Products.FirstOrDefault(p => p.ProductID == productId);
                        if (product == null)
                        {
                            lblMessage.Text = "Product not found.";
                            lblMessage.CssClass = "alert alert-warning";
                            return;
                        }

                        if (discounts.Any(d => d.ProductID == productId && d.CustomerAccountId == accountId))
                        {
                            lblMessage.Text = "This product is already in the list.";
                            lblMessage.CssClass = "alert alert-warning";
                            return;
                        }

                        discounts.Add(new Models.DiscountPolicy
                        {
                            ProductID = product.ProductID,
                            Product = product,
                            CustomerAccountId = accountId,
                            FlatDiscount = flatDiscount,
                            FlatDiscountExpiry = flatExpiry,
                            CreditDiscount = creditDiscount,
                            CreditDiscountExpiry = creditExpiry
                        });
                    }
                }

                Policies = discounts;
                BindPolicies();
                btnSave.Text = "Save Discount";
                upAllControls.Update();

                // reset inputs
                txtProductId.Text = "";
                ddlProduct.ClearSelection();
                txtFlatDiscountAdd.Text = "";
                txtFlatExpiryDateAdd.Text = "";
                txtCreditDiscountAdd.Text = "";
                txtCreditExpiryDateAdd.Text = "";

                lblMessage.Text = "Discount(s) added successfully!";
                lblMessage.CssClass = "alert alert-success mt-3";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(ddlCustomer.SelectedValue, out int accountId))
            {
                lblMessage.Text = "Please select a customer first.";
                lblMessage.CssClass = "alert alert-warning";
                return;
            }

            var sessionPolicies = Policies;

            // 🔹 Validate policies
            foreach (var policy in sessionPolicies.Where(p => p.CustomerAccountId == accountId))
            {
                if (policy.FlatDiscount <= 0 && policy.CreditDiscount <= 0)
                {
                    lblMessage.Text = $"Product ID {policy.ProductID}: At least one discount is required.";
                    lblMessage.CssClass = "alert alert-warning";
                    return;
                }
                if (policy.FlatDiscount > 0 && policy.FlatDiscountExpiry == null)
                {
                    lblMessage.Text = $"Product ID {policy.ProductID}: Flat discount requires an expiry date.";
                    lblMessage.CssClass = "alert alert-warning";
                    return;
                }
                if (policy.CreditDiscount > 0 && policy.CreditDiscountExpiry == null)
                {
                    lblMessage.Text = $"Product ID {policy.ProductID}: Credit discount requires an expiry date.";
                    lblMessage.CssClass = "alert alert-warning";
                    return;
                }
            }

            btnSave.Text = "Saving...";
            upAllControls.Update();

            using (var db = new DataPharmaContext())
            {
                // 🔹 Load all existing policies once
                var existingPolicies = db.DiscountPolicies
                                         .Where(d => d.CustomerAccountId == accountId)
                                         .ToList();

                var existingDict = existingPolicies.ToDictionary(d => d.ProductID, d => d);

                var toUpdate = new List<Models.DiscountPolicy>();
                var toInsert = new List<Models.DiscountPolicy>();

                foreach (var sessPolicy in sessionPolicies.Where(p => p.CustomerAccountId == accountId))
                {
                    if (existingDict.TryGetValue(sessPolicy.ProductID, out var existing))
                    {
                        bool hasChanges =
                            existing.FlatDiscount != sessPolicy.FlatDiscount ||
                            existing.FlatDiscountExpiry != sessPolicy.FlatDiscountExpiry ||
                            existing.CreditDiscount != sessPolicy.CreditDiscount ||
                            existing.CreditDiscountExpiry != sessPolicy.CreditDiscountExpiry;

                        if (hasChanges)
                        {
                            existing.FlatDiscount = sessPolicy.FlatDiscount;
                            existing.FlatDiscountExpiry = sessPolicy.FlatDiscountExpiry;
                            existing.CreditDiscount = sessPolicy.CreditDiscount;
                            existing.CreditDiscountExpiry = sessPolicy.CreditDiscountExpiry;

                            toUpdate.Add(existing);
                        }
                    }
                    else
                    {
                        toInsert.Add(new Models.DiscountPolicy
                        {
                            ProductID = sessPolicy.ProductID,
                            CustomerAccountId = accountId,
                            FlatDiscount = sessPolicy.FlatDiscount,
                            FlatDiscountExpiry = sessPolicy.FlatDiscountExpiry,
                            CreditDiscount = sessPolicy.CreditDiscount,
                            CreditDiscountExpiry = sessPolicy.CreditDiscountExpiry
                        });
                    }
                }

                var sessionProductIds = sessionPolicies.Where(p => p.CustomerAccountId == accountId)
                                                       .Select(p => p.ProductID)
                                                       .ToHashSet();

                var toDelete = existingPolicies
                               .Where(dbp => !sessionProductIds.Contains(dbp.ProductID))
                               .ToList();

                // 🔹 Process updates
                foreach (var upd in toUpdate)
                {
                    db.Entry(upd).State = EntityState.Modified;
                }

                // 🔹 Process inserts
                foreach (var ins in toInsert)
                {
                    db.DiscountPolicies.Add(ins);
                }

                // 🔹 Process deletes
                foreach (var del in toDelete)
                {
                    db.DiscountPolicies.Remove(del);
                }

                db.SaveChanges();
            }

            LoadPolicies(accountId);

            btnSave.Text = "Saved!";
            lblMessage.Text = "Discounts saved successfully.";
            lblMessage.CssClass = "alert alert-success mt-3";
            upAllControls.Update();
        }

        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(ddlCustomer.SelectedValue, out int accountId))
            {
                lblMessage.Text = "Please select a customer first.";
                lblMessage.CssClass = "alert alert-warning";
                upAllControls.Update();
                return;
            }

            using (var db = new DataPharmaContext())
            {
                btnDeleteAll.Text = "Deleting!";
                upAllControls.Update();
                var policies = db.DiscountPolicies
                                 .Where(d => d.CustomerAccountId == accountId)
                                 .ToList();

                if (!policies.Any())
                {
                    lblMessage.Text = "No discount policies found for this customer.";
                    lblMessage.CssClass = "alert alert-info";
                    upAllControls.Update();
                    return;
                }

                foreach (var policy in policies)
                {
                    db.DiscountPolicies.Remove(policy);
                }

                db.SaveChanges();
            }

            LoadPolicies(accountId);

            btnDeleteAll.Text = "Delete All Discount";
            lblMessage.Text = "All discount policies deleted successfully.";
            lblMessage.CssClass = "alert alert-success mt-3";
            upAllControls.Update();
        }

        //Import functions

        //Sample file
        private void ImportInfoControl_DownloadRequested(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=discountpolicy_sample.csv");

            // CSV header (all columns, expiry always present)
            Response.Write("CustomerId,ProductID,FlatDiscount,FlatDiscountExpiry,CreditDiscount,CreditDiscountExpiry\r\n");

            // Sample rows (expiry columns kept but can be blank)
            Response.Write("101,5001,10,2025-12-31,0,\r\n");
            Response.Write("101,5002,0,,5,2026-01-15\r\n");

            Response.End();
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (!fuCSV.HasFile || !fuCSV.FileName.EndsWith(".csv"))
            {
                lblMessage.Text = "Please upload a valid CSV file.";
                lblMessage.CssClass = "alert alert-danger d-block";
                return;
            }

            try
            {
                using (var reader = new System.IO.StreamReader(fuCSV.FileContent))
                {
                    string headerLine = reader.ReadLine();
                    if (headerLine == null)
                    {
                        lblMessage.Text = "CSV file is empty.";
                        lblMessage.CssClass = "alert alert-danger d-block";
                        return;
                    }

                    var headers = headerLine.Split(',').Select(h => h.Trim()).ToList();

                    int colCustomerId = headers.IndexOf("CustomerId");
                    int colProductId = headers.IndexOf("ProductID");
                    int colFlat = headers.IndexOf("FlatDiscount");
                    int colFlatExp = headers.IndexOf("FlatDiscountExpiry");
                    int colCredit = headers.IndexOf("CreditDiscount");
                    int colCreditExp = headers.IndexOf("CreditDiscountExpiry");

                    if (colCustomerId == -1 || colProductId == -1 || colFlat == -1 || colFlatExp == -1 ||
                        colCredit == -1 || colCreditExp == -1)
                    {
                        lblMessage.Text = "Missing required columns in CSV.";
                        lblMessage.CssClass = "alert alert-danger d-block";
                        return;
                    }

                    int lineNo = 1;
                    int insertCount = 0;
                    int updateCount = 0;
                    var errorMessages = new List<string>();

                    var processed = new HashSet<(int customerId, int productId)>();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        lineNo++;
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var values = line.Split(',');

                        try
                        {
                            string rawCustomerId = values[colCustomerId].Trim();
                            string rawProductId = values[colProductId].Trim();
                            string rawFlat = values[colFlat].Trim();
                            string rawFlatExp = values[colFlatExp].Trim();
                            string rawCredit = values[colCredit].Trim();
                            string rawCreditExp = values[colCreditExp].Trim();

                            // 🔹 Parse IDs
                            if (!int.TryParse(rawCustomerId, out int customerId))
                                throw new Exception($"Invalid CustomerId '{rawCustomerId}'");

                            if (!int.TryParse(rawProductId, out int productId))
                                throw new Exception($"Invalid ProductID '{rawProductId}'");

                            if (!processed.Add((customerId, productId)))
                            {
                                // Already processed → skip
                                continue;
                            }

                            if (!_context.Products.Any(p => p.ProductID == productId))
                                throw new Exception($"ProductID '{productId}' not found in DB.");

                            // 🔹 Parse decimals
                            if (!decimal.TryParse(rawFlat, out decimal flatDiscount))
                                throw new Exception($"Invalid FlatDiscount '{rawFlat}'");

                            if (!decimal.TryParse(rawCredit, out decimal creditDiscount))
                                throw new Exception($"Invalid CreditDiscount '{rawCredit}'");

                            // 🔹 Parse expiries (must exist, even if blank → force check)
                            DateTime? flatExp = null, creditExp = null;

                            if (!string.IsNullOrWhiteSpace(rawFlatExp))
                            {
                                if (DateTime.TryParse(rawFlatExp, out DateTime parsedFlatExp))
                                    flatExp = parsedFlatExp;
                                else
                                    throw new Exception($"Invalid FlatDiscountExpiry '{rawFlatExp}'");
                            }
                            else if (flatDiscount > 0)
                            {
                                throw new Exception($"FlatDiscountExpiry is required when FlatDiscount > 0");
                            }

                            if (!string.IsNullOrWhiteSpace(rawCreditExp))
                            {
                                if (DateTime.TryParse(rawCreditExp, out DateTime parsedCreditExp))
                                    creditExp = parsedCreditExp;
                                else
                                    throw new Exception($"Invalid CreditDiscountExpiry '{rawCreditExp}'");
                            }
                            else if (creditDiscount > 0)
                            {
                                throw new Exception($"CreditDiscountExpiry is required when CreditDiscount > 0");
                            }

                            // 🔹 Lookup existing policy
                            var existingPolicy = _context.DiscountPolicies
                                .FirstOrDefault(p => p.CustomerAccountId == customerId && p.ProductID == productId);

                            if (existingPolicy != null)
                            {
                                // Update
                                existingPolicy.FlatDiscount = flatDiscount;
                                existingPolicy.FlatDiscountExpiry = flatExp;
                                existingPolicy.CreditDiscount = creditDiscount;
                                existingPolicy.CreditDiscountExpiry = creditExp;
                                existingPolicy.UpdatedAt = DateTime.Now;

                                updateCount++;
                            }
                            else
                            {
                                // Insert
                                var policy = new Models.DiscountPolicy
                                {
                                    CustomerAccountId = customerId,
                                    ProductID = productId,
                                    FlatDiscount = flatDiscount,
                                    FlatDiscountExpiry = flatExp,
                                    CreditDiscount = creditDiscount,
                                    CreditDiscountExpiry = creditExp,
                                };

                                _context.DiscountPolicies.Add(policy);
                                insertCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            errorMessages.Add($"Line {lineNo}: {ex.Message}");
                        }
                    }

                    _context.SaveChanges();

                    if (errorMessages.Any())
                    {
                        lblMessage.Text = $"Import completed: {insertCount} added, {updateCount} updated." +
                            "<br><b>Errors:</b><br>" +
                            string.Join("<br>", errorMessages.Take(10)) +
                            (errorMessages.Count > 10 ? "<br>...and more." : "");
                        lblMessage.CssClass = "alert alert-danger d-block";
                    }
                    else
                    {
                        lblMessage.Text = $"Import completed: {insertCount} added, {updateCount} updated.";
                        lblMessage.CssClass = "alert alert-success mt-3 d-block";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Import failed: {ex.Message}";
                lblMessage.CssClass = "alert alert-danger mt-3 d-block";
            }
        }
    }
}