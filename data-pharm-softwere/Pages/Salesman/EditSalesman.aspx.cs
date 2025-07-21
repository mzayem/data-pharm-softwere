using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Salesman
{
    public partial class EditSalesman : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        private int SalesmanID
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
            if (SalesmanID == 0)
            {
                Response.Redirect("/salesman");
            }
            if (!IsPostBack)
            {
                ViewState["AssignedTownIDs"] = new List<int>();
                LoadSalesman();
                LoadTowns();
                LoadAssignedTownIDs();
                BindAssignedTowns();
            }
        }

        private void LoadAssignedTownIDs()
        {
            var assignedTownIDs = _context.SalesmanTowns
                .Where(st => st.SalesmanID == SalesmanID)
                .Select(st => st.TownID)
                .ToList();

            ViewState["AssignedTownIDs"] = assignedTownIDs;
        }

        private void LoadSalesman()
        {
            var salesman = _context.Salesmen.FirstOrDefault(s => s.SalesmanID == SalesmanID);
            if (salesman == null)
            {
                Response.Redirect("/salesman");
                return;
            }
            txtName.Text = salesman.Name;
            txtEmail.Text = salesman.Email;
            txtContact.Text = salesman.Contact;
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
            if (!Page.IsValid) return;

            var salesman = _context.Salesmen.FirstOrDefault(s => s.SalesmanID == SalesmanID);
            if (salesman == null)
            {
                Response.Redirect("/salesman");
                return;
            }

            try
            {
                salesman.Name = txtName.Text.Trim();
                salesman.Email = txtEmail.Text.Trim();
                salesman.Contact = txtContact.Text.Trim();

                var assignedTownIDs = ViewState["AssignedTownIDs"] as List<int> ?? new List<int>();

                var existingTownIDs = _context.SalesmanTowns
                    .Where(st => st.SalesmanID == SalesmanID)
                    .Select(st => st.TownID)
                    .ToList();

                var townsToAdd = assignedTownIDs.Except(existingTownIDs).ToList();
                var townsToRemove = existingTownIDs.Except(assignedTownIDs).ToList();

                var townsToRemoveEntities = _context.SalesmanTowns
                    .Where(st => st.SalesmanID == SalesmanID && townsToRemove.Contains(st.TownID))
                    .ToList();
                _context.SalesmanTowns.RemoveRange(townsToRemoveEntities);

                foreach (var townId in townsToAdd)
                {
                    _context.SalesmanTowns.Add(new SalesmanTown
                    {
                        SalesmanID = SalesmanID,
                        TownID = townId,
                        AssignedOn = DateTime.Now
                    });
                }

                _context.SaveChanges();

                lblMessage.Text = "Salesman updated successfully.";
                lblMessage.CssClass = "alert alert-success mt-3";
                Response.Redirect("/salesman");
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }
    }
}