using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Salesman
{
    public partial class CreateSalesman : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;
                ViewState["AssignedTownIDs"] = new List<int>();
                LoadTowns();
                BindAssignedTowns();
            }
        }

        private void LoadTowns()
        {
            var towns = _context.Towns.OrderBy(t => t.Name).ToList();
            ddlTown.DataSource = towns;
            ddlTown.DataTextField = "Name";
            ddlTown.DataValueField = "TownID";
            ddlTown.DataBind();
            ddlTown.Items.Insert(0, new ListItem("-- Assign Towns --", ""));
        }

        private void BindAssignedTowns()
        {
            var assignedTownIDs = ViewState["AssignedTownIDs"] as List<int> ?? new List<int>();

            var towns = _context.Towns
                .Where(t => assignedTownIDs.Contains(t.TownID))
                .Select(t => new { t.TownID, t.Name })
                .ToList();

            rptAssignedTowns.DataSource = towns;
            rptAssignedTowns.DataBind();
        }

        protected void ddlTown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlTown.SelectedValue, out int selectedTownId))
            {
                var assignedTownIDs = ViewState["AssignedTownIDs"] as List<int> ?? new List<int>();

                if (!assignedTownIDs.Contains(selectedTownId))
                {
                    assignedTownIDs.Add(selectedTownId);
                    ViewState["AssignedTownIDs"] = assignedTownIDs;
                    BindAssignedTowns();
                }

                ddlTown.SelectedIndex = 0;
            }
        }

        protected void btnRemoveTown_Click(object sender, EventArgs e)
        {
            var btn = sender as LinkButton;
            if (int.TryParse(btn.CommandArgument, out int townId))
            {
                var assignedTownIDs = ViewState["AssignedTownIDs"] as List<int> ?? new List<int>();
                assignedTownIDs.Remove(townId);
                ViewState["AssignedTownIDs"] = assignedTownIDs;
                BindAssignedTowns();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var salesman = new Models.Salesman
                    {
                        Name = txtName.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Contact = txtContact.Text.Trim(),
                        CreatedAt = DateTime.Now
                    };

                    var assignedTownIDs = ViewState["AssignedTownIDs"] as List<int> ?? new List<int>();
                    foreach (var townId in assignedTownIDs)
                    {
                        salesman.SalesmanTowns.Add(new SalesmanTown
                        {
                            TownID = townId,
                            AssignedOn = DateTime.Now
                        });
                    }

                    _context.Salesmen.Add(salesman);
                    _context.SaveChanges();

                    lblMessage.Text = "Salesman saved successfully.";
                    lblMessage.CssClass = "alert alert-success mt-3";

                    ViewState["AssignedTownIDs"] = new List<int>();
                    BindAssignedTowns();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "alert alert-danger mt-3";
                }
                finally
                {
                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtContact.Text = "";
                }
            }
        }
    }
}