using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Customer
{
    public partial class CreateCustomer : System.Web.UI.Page
    {
        private int AccountId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) ? id : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (AccountId > 0)
                {
                    txtID.Text = AccountId.ToString();
                    FetchAccount(AccountId, out _);
                }

                lblMessage.Text = string.Empty;
                LoadCityRoutes();
                LoadTowns();
                LoadCustomerTypes();
            }
        }

        private void LoadCityRoutes()
        {
            using (var db = new DataPharmaContext())
            {
                if (Cache["CityRoutes"] == null)
                {
                    Cache["CityRoutes"] = db.CityRoutes
                        .OrderBy(r => r.Name)
                        .Select(r => new { r.CityRouteID, r.Name })
                        .ToList();
                }
            }

            ddlCityRoute.DataSource = Cache["CityRoutes"];
            ddlCityRoute.DataTextField = "Name";
            ddlCityRoute.DataValueField = "CityRouteID";
            ddlCityRoute.DataBind();
            ddlCityRoute.Items.Insert(0, new ListItem("-- All Routes --", ""));
        }

        private void LoadTowns(int? cityRouteId = null)
        {
            using (var db = new DataPharmaContext())
            {
                var townsQuery = db.Towns.AsQueryable();

                if (cityRouteId.HasValue && cityRouteId > 0)
                {
                    townsQuery = townsQuery.Where(t => t.CityRouteID == cityRouteId);
                }

                ddlTown.DataSource = townsQuery.OrderBy(t => t.Name).ToList();
                ddlTown.DataTextField = "Name";
                ddlTown.DataValueField = "TownID";
                ddlTown.DataBind();
                ddlTown.Items.Insert(0, new ListItem("-- Select Town --", ""));
            }
        }

        private void LoadCustomerTypes()
        {
            ddlCustomerType.DataSource = Enum.GetValues(typeof(CustomerType));
            ddlCustomerType.DataBind();
            ddlCustomerType.Items.Insert(0, new ListItem("-- Select Customer Type --", ""));
        }

        protected void ddlCityRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlCityRoute.SelectedValue, out int cityRouteId))
            {
                LoadTowns(cityRouteId);
            }
            else
            {
                LoadTowns();
            }
        }

        protected void btnFetchAccount_Click(object sender, EventArgs e)
        {
            int accountId;
            if (!int.TryParse(txtID.Text.Trim(), out accountId))
            {
                ShowMessage("Invalid Account ID.", "danger");
                return;
            }

            txtName.Text = string.Empty;
            FetchAccount(accountId, out _);
        }

        private bool FetchAccount(int accountId, out Account validAccount)
        {
            validAccount = null;

            using (var db = new DataPharmaContext())
            {
                var account = db.Accounts.FirstOrDefault(a => a.AccountId == accountId);

                if (account == null)
                {
                    ShowMessage("Account not found.", "danger");
                    return false;
                }

                if (!string.Equals(account.AccountType, "CUSTOMERS", StringComparison.OrdinalIgnoreCase))
                {
                    ShowMessage("This Account is not of type 'Customer'.", "warning");
                    return false;
                }

                if (!account.Status?.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase) ?? true)
                {
                    ShowMessage("This Account is Deactivated!", "danger");
                    return false;
                }

                if (db.Customers.Any(c => c.AccountId == accountId))
                {
                    Response.Redirect($"/customer/edit?id={accountId}", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return false;
                }

                txtName.Text = account.AccountName;
                ShowMessage("Account is valid. Please fill customer details.", "success");
                validAccount = account;
                return true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (!int.TryParse(txtID.Text.Trim(), out int accountId))
            {
                ShowMessage("Invalid Account ID. Cannot save customer.", "danger");
                return;
            }

            if (!FetchAccount(accountId, out var account))
                return;

            try
            {
                using (var db = new DataPharmaContext())
                {
                    if (db.Customers.Any(c => c.AccountId == accountId))
                    {
                        ShowMessage("Customer already exists for this Account ID.", "danger");
                        return;
                    }

                    string cnic = txtCNIC.Text.Trim();
                    if (db.Customers.Any(c => c.CNIC == cnic))
                    {
                        ShowMessage("A customer with the same CNIC already exists.", "warning");
                        return;
                    }

                    var customer = new Models.Customer
                    {
                        AccountId = accountId,
                        Email = txtEmail.Text.Trim(),
                        Contact = txtContact.Text.Trim(),
                        CNIC = cnic,
                        Address = txtAddress.Text.Trim(),
                        TownID = int.Parse(ddlTown.SelectedValue),
                        LicenceNo = txtLicenceNo.Text.Trim(),
                        ExpiryDate = DateTime.Parse(txtExpiryDate.Text.Trim()),
                        NtnNo = txtNtnNo.Text.Trim(),
                        CustomerType = (CustomerType)Enum.Parse(typeof(CustomerType), ddlCustomerType.SelectedValue),
                        NorcoticsSaleAllowed = chkNorcoticsSaleAllowed.Checked,
                        InActive = chkInActive.Checked,
                        IsAdvTaxExempted = chkAdvTaxExempted.Checked,
                        FbrInActiveGST = chkFbrInActiveGST.Checked,
                        FBRInActiveTax236H = chkFBRInActiveTax236H.Checked,
                        CreatedAt = DateTime.Now
                    };

                    db.Customers.Add(customer);
                    db.SaveChanges();

                    ShowMessage("Customer saved successfully!", "success");
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.ToString(), "danger");
            }
        }

        private void ShowMessage(string message, string cssType)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "alert alert-" + cssType;
        }

        private void ClearForm()
        {
            txtID.Text = string.Empty;
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtContact.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtCNIC.Text = string.Empty;
            txtLicenceNo.Text = string.Empty;
            txtExpiryDate.Text = string.Empty;
            txtNtnNo.Text = string.Empty;

            if (ddlTown.Items.Count > 0) ddlTown.SelectedIndex = 0;
            if (ddlCustomerType.Items.Count > 0) ddlCustomerType.SelectedIndex = 0;
            if (ddlCityRoute.Items.Count > 0) ddlCityRoute.SelectedIndex = 0;

            chkNorcoticsSaleAllowed.Checked = false;
            chkInActive.Checked = false;
            chkAdvTaxExempted.Checked = false;
            chkFbrInActiveGST.Checked = false;
            chkFBRInActiveTax236H.Checked = false;
        }
    }
}