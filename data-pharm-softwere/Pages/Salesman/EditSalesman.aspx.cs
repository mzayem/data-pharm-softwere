using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
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
                LoadSalesman();
                LoadTowns();
                LoadAssignedTowns();
                BindAssignedTowns();
            }
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

        private List<AssignedTownViewModel> AssignedTowns
        {
            get => ViewState["AssignedTowns"] as List<AssignedTownViewModel> ?? new List<AssignedTownViewModel>();
            set => ViewState["AssignedTowns"] = value;
        }

        private void LoadAssignedTowns()
        {
            var towns = _context.SalesmanTowns
                .Where(st => st.SalesmanID == SalesmanID)
                .Select(st => new AssignedTownViewModel
                {
                    TownID = st.TownID,
                    TownName = st.Town.Name,
                    AssignmentType = st.AssignmentType,
                    Percentage = (int)st.Percentage
                })
                .ToList();

            AssignedTowns = towns;
        }

        private void BindAssignedTowns()
        {
            rptAssignedTowns.DataSource = AssignedTowns;
            rptAssignedTowns.DataBind();
        }

        protected void ddlTown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlTown.SelectedValue, out int selectedTownId))
            {
                var list = AssignedTowns;

                var existingAssignments = list.Where(x => x.TownID == selectedTownId).ToList();

                AssignmentType newType;

                if (!existingAssignments.Any(x => x.AssignmentType == AssignmentType.Booker))
                {
                    newType = AssignmentType.Booker;
                }
                else if (!existingAssignments.Any(x => x.AssignmentType == AssignmentType.Supplier))
                {
                    newType = AssignmentType.Supplier;
                }
                else if (!existingAssignments.Any(x => x.AssignmentType == AssignmentType.Driver))
                {
                    newType = AssignmentType.Driver;
                }
                else
                {
                    lblMessage.Text = "This town already has Booker, Supplier, and Driver assigned.";
                    lblMessage.CssClass = "alert alert-warning";
                    ddlTown.SelectedIndex = 0;
                    return;
                }

                list.Add(new AssignedTownViewModel
                {
                    TownID = selectedTownId,
                    TownName = ddlTown.SelectedItem.Text,
                    AssignmentType = newType,
                    Percentage = 0
                });

                AssignedTowns = list;
                BindAssignedTowns();
                ddlTown.SelectedIndex = 0;
            }
        }

        protected void rptAssignedTowns_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Remove" && int.TryParse(e.CommandArgument.ToString(), out int index))
            {
                if (index >= 0 && index < AssignedTowns.Count)
                {
                    AssignedTowns.RemoveAt(index);
                    AssignedTowns = AssignedTowns;
                    BindAssignedTowns();
                }
            }
        }

        protected void rptAssignedTowns_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var item = (AssignedTownViewModel)e.Item.DataItem;

                var ddl = (DropDownList)e.Item.FindControl("ddlAssignmentType");
                if (ddl != null)
                {
                    ddl.DataSource = Enum.GetValues(typeof(AssignmentType));
                    ddl.DataBind();
                    ddl.SelectedValue = item.AssignmentType.ToString();
                }
            }
        }

        protected void BonusFieldChanged(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptAssignedTowns.Items)
            {
                var ddlType = item.FindControl("ddlAssignmentType") as DropDownList;
                var txtPercentage = item.FindControl("txtPercentage") as TextBox;
                var hfTownID = item.FindControl("hfTownID") as HiddenField;

                if (int.TryParse(hfTownID.Value, out int townId))
                {
                    var town = AssignedTowns.FirstOrDefault(x => x.TownID == townId);
                    if (town != null)
                    {
                        if (Enum.TryParse<AssignmentType>(ddlType.SelectedValue, out var parsedType))
                            town.AssignmentType = parsedType;

                        if (decimal.TryParse(txtPercentage.Text, out var percentage))
                            town.Percentage = percentage;
                    }
                }
            }

            AssignedTowns = AssignedTowns;
            KeepFocus((Control)sender);
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

                // Remove all old towns
                var oldTowns = _context.SalesmanTowns.Where(st => st.SalesmanID == SalesmanID).ToList();
                _context.SalesmanTowns.RemoveRange(oldTowns);

                // Add updated towns
                foreach (var townVm in AssignedTowns)
                {
                    _context.SalesmanTowns.Add(new SalesmanTown
                    {
                        SalesmanID = SalesmanID,
                        TownID = townVm.TownID,
                        AssignmentType = townVm.AssignmentType,
                        Percentage = townVm.Percentage,
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