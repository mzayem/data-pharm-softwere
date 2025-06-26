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
                LoadPartyTypes();
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

        private void LoadPartyTypes()
        {
            ddlPartyType.DataSource = Enum.GetValues(typeof(PartyType));
            ddlPartyType.DataBind();
            ddlPartyType.Items.Insert(0, new ListItem("-- Select Part Type --", ""));
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
                    var customer = new Models.Customer
                    {
                        Name = txtName.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Contact = txtContact.Text.Trim(),
                        CNIC = txtCNIC.Text.Trim(),
                        Address = txtAddress.Text.Trim(),
                        TownID = int.Parse(ddlTown.SelectedValue),
                        LicenceNo = txtLicenceNo.Text.Trim(),
                        NtnNo = txtNtnNo.Text.Trim(),
                        PartyType = (PartyType)Enum.Parse(typeof(PartyType), ddlPartyType.SelectedValue),
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
                    lblMessage.CssClass = "text-success fw-semibold";

                    // Optional: clear form or redirect
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-semibold";
                }
            }
        }
    }
}