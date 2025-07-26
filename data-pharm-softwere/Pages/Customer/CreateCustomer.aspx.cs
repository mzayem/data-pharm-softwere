using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Customer
{
    public partial class CreateCustomer : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCityRoutes();
                LoadTowns();
                LoadCustomerTypes();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string name = txtName.Text.Trim();
                    string cnic = txtCNIC.Text.Trim();

                    bool duplicateExists = _context.Customers
                        .Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                               && c.CNIC == cnic);

                    if (duplicateExists)
                    {
                        lblMessage.Text = "A customer with the same Name and CNIC already exists.";
                        lblMessage.CssClass = "alert alert-warning";
                        return;
                    }

                    var customer = new Models.Customer
                    {
                        Name = txtName.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Contact = txtContact.Text.Trim(),
                        CNIC = txtCNIC.Text.Trim(),
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

                    _context.Customers.Add(customer);
                    _context.SaveChanges();

                    lblMessage.Text = "Customer saved successfully!";
                    lblMessage.CssClass = "alert alert-success";

                    // Optional: clear form or redirect
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