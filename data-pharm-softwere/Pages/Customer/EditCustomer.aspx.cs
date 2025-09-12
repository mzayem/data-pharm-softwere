using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Customer
{
    public partial class EditCustomer : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        private int CustomerId
        {
            get
            {
                int id;
                if (int.TryParse(Request.QueryString["id"], out id))
                {
                    return id;
                }
                return 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCityRoutes();
                LoadTowns();
                LoadCustomerTypes();
                if (CustomerId > 0)
                {
                    LoadCustomer();
                }
                else
                {
                    Response.Redirect("/customer/create/");
                }
            }
        }

        private void LoadCityRoutes()
        {
            ddlCityRoute.DataSource = _context.CityRoutes.OrderBy(r => r.Name).ToList();
            ddlCityRoute.DataTextField = "Name";
            ddlCityRoute.DataValueField = "CityRouteID";
            ddlCityRoute.DataBind();
            ddlCityRoute.Items.Insert(0, new ListItem("-- All Routes --", ""));
        }

        private void LoadTowns(int? cityRouteId = null)
        {
            var townsQuery = _context.Towns.AsQueryable();

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

        private void LoadCustomerTypes()
        {
            ddlCustomerType.DataSource = Enum.GetValues(typeof(CustomerType));
            ddlCustomerType.DataBind();
            ddlCustomerType.Items.Insert(0, new ListItem("-- Select Part Type --", ""));
        }

        protected void ddlCityRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlCityRoute.SelectedValue, out int cityRouteId))
            {
                LoadTowns(cityRouteId);
            }
            else
            {
                LoadTowns(); // Show all
            }
        }

        protected void btnFetchAccount_Click(object sender, EventArgs e)
        {
            int newAccountId;
            txtName.Text = string.Empty;

            if (!int.TryParse(txtID.Text.Trim(), out newAccountId))
            {
                ShowMessage("Invalid Account ID.", "danger");
                return;
            }

            if (!FetchAccount(newAccountId, out var validAccount)) return;

            // Check if customer already exists
            var customer = _context.Customers.FirstOrDefault(v => v.AccountId == newAccountId);
            if (customer == null)
            {
                Response.Redirect($"/customer/create?id={newAccountId}", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // If customer exists, reload this page with new ID
            Response.Redirect($"/customer/edit?id={newAccountId}", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private bool FetchAccount(int accountId, out Account validAccount)
        {
            validAccount = null;

            using (var db = new DataPharmaContext())
            {
                var account = db.Accounts.FirstOrDefault(a => a.AccountId == accountId);

                if (account == null)
                {
                    Response.Redirect($"/customer/create?id={accountId}", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return false;
                }

                if (!account.AccountType?.Equals("CUSTOMERS", StringComparison.OrdinalIgnoreCase) ?? true)
                {
                    Response.Redirect($"/customer/create?id={accountId}", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return false;
                }

                if (!account.Status?.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase) ?? true)
                {
                    Response.Redirect($"/customer/create?id={accountId}", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return false;
                }

                txtName.Text = account.AccountName;
                validAccount = account;
                return true;
            }
        }

        protected void LoadCustomer()
        {
            try
            {
                var customer = _context.Customers
                    .Include("Town.CityRoute")
                    .FirstOrDefault(c => c.AccountId == CustomerId);

                if (customer == null)
                {
                    Response.Redirect("/customer/create/");
                    return;
                }

                txtEmail.Text = customer.Email;
                txtContact.Text = customer.Contact;
                txtCNIC.Text = customer.CNIC;
                txtAddress.Text = customer.Address;

                // Load CityRoute
                if (customer.Town?.CityRouteID != null)
                {
                    ddlCityRoute.SelectedValue = customer.Town.CityRouteID.ToString();
                    LoadTowns(customer.Town.CityRouteID);
                }

                ddlTown.SelectedValue = customer.TownID.ToString();
                txtLicenceNo.Text = customer.LicenceNo;
                txtExpiryDate.Text = customer.ExpiryDate.ToString("yyyy-MM-dd");
                txtNtnNo.Text = customer.NtnNo;
                ddlCustomerType.SelectedValue = customer.CustomerType.ToString();

                chkNorcoticsSaleAllowed.Checked = customer.NorcoticsSaleAllowed;
                chkInActive.Checked = customer.InActive;
                chkAdvTaxExempted.Checked = customer.IsAdvTaxExempted;
                chkFbrInActiveGST.Checked = customer.FbrInActiveGST;
                chkFBRInActiveTax236H.Checked = customer.FBRInActiveTax236H;
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading customer: " + ex.Message, "danger");
            }
        }

        public TextBox TxtAddress
        {
            get => txtAddress;
            set => txtAddress = value;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var customer = _context.Customers.FirstOrDefault(c => c.AccountId == CustomerId);
                if (customer == null)
                {
                    Response.Redirect("/customer/create/");
                    return;
                }
                try
                {
                    customer.Email = txtEmail.Text.Trim();
                    customer.Contact = txtContact.Text.Trim();
                    customer.CNIC = txtCNIC.Text.Trim();
                    customer.Address = txtAddress.Text.Trim();
                    customer.TownID = int.Parse(ddlTown.SelectedValue);
                    customer.LicenceNo = txtLicenceNo.Text.Trim();
                    customer.ExpiryDate = DateTime.Parse(txtExpiryDate.Text.Trim());
                    customer.NtnNo = txtNtnNo.Text.Trim();
                    customer.CustomerType = (CustomerType)Enum.Parse(typeof(CustomerType), ddlCustomerType.SelectedValue);
                    customer.NorcoticsSaleAllowed = chkNorcoticsSaleAllowed.Checked;
                    customer.InActive = chkInActive.Checked;
                    customer.IsAdvTaxExempted = chkAdvTaxExempted.Checked;
                    customer.FbrInActiveGST = chkFbrInActiveGST.Checked;
                    customer.FBRInActiveTax236H = chkFBRInActiveTax236H.Checked;

                    _context.SaveChanges();

                    lblMessage.Text = "Customer saved successfully!";
                    lblMessage.CssClass = "text-success fw-semibold";

                    Response.Redirect("/customer");
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-semibold";
                }
            }
        }

        private void ShowMessage(string message, string cssType)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "alert alert-" + cssType;
        }
    }
}