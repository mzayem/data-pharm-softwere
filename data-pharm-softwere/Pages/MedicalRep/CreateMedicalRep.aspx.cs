using data_pharm_softwere.Data;

using data_pharm_softwere.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.MedicalRep
{
    public partial class CreateMedicalRep : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;
                ViewState["AssignedSubGroupIDs"] = new List<int>();
                LoadSubGroups();
                BindAssignedSubGroups();
            }
        }

        private void LoadSubGroups()
        {
            var subGroups = _context.SubGroups.OrderBy(sg => sg.Name).ToList();
            ddlSubGroup.DataSource = subGroups;
            ddlSubGroup.DataTextField = "Name";
            ddlSubGroup.DataValueField = "SubGroupID";
            ddlSubGroup.DataBind();
            ddlSubGroup.Items.Insert(0, new ListItem("-- Assign SubGroups --", ""));
        }

        private void BindAssignedSubGroups()
        {
            var assignedSubGroupIDs = ViewState["AssignedSubGroupIDs"] as List<int> ?? new List<int>();

            var subGroups = _context.SubGroups
                .Where(sg => assignedSubGroupIDs.Contains(sg.SubGroupID))
                .Select(sg => new { sg.SubGroupID, sg.Name })
                .ToList();

            rptAssignedSubGroups.DataSource = subGroups;
            rptAssignedSubGroups.DataBind();
        }

        protected void ddlSubGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlSubGroup.SelectedValue, out int selectedSubGroupId))
            {
                var assignedSubGroupIDs = ViewState["AssignedSubGroupIDs"] as List<int> ?? new List<int>();
                if (!assignedSubGroupIDs.Contains(selectedSubGroupId))
                {
                    assignedSubGroupIDs.Add(selectedSubGroupId);
                    ViewState["AssignedSubGroupIDs"] = assignedSubGroupIDs;
                    BindAssignedSubGroups();
                }
                ddlSubGroup.SelectedIndex = 0;
            }
        }

        protected void btnRemoveSubGroup_Click(object sender, EventArgs e)
        {
            var btn = sender as LinkButton;
            if (int.TryParse(btn.CommandArgument, out int subGroupId))
            {
                var assignedSubGroupIDs = ViewState["AssignedSubGroupIDs"] as List<int> ?? new List<int>();
                assignedSubGroupIDs.Remove(subGroupId);
                ViewState["AssignedSubGroupIDs"] = assignedSubGroupIDs;
                BindAssignedSubGroups();
            }
        }

        private void LoadCustomerTypes()
        {
            ddlMedicalRepType.DataSource = Enum.GetValues(typeof(CustomerType));
            ddlMedicalRepType.DataBind();
            ddlMedicalRepType.Items.Insert(0, new ListItem("-- Select Part Type --", ""));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var medicalRep = new Models.MedicalRep
                {
                    Name = txtName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Contact = txtContact.Text.Trim(),
                    Type = (RepType)Enum.Parse(typeof(RepType), ddlMedicalRepType.SelectedValue),
                    CreatedAt = DateTime.Now
                };

                var assignedSubGroupIDs = ViewState["AssignedSubGroupIDs"] as List<int> ?? new List<int>();
                medicalRep.MedicalRepSubGroups = assignedSubGroupIDs
                    .Select(subGroupId => new MedicalRepSubGroup
                    {
                        SubGroupID = subGroupId,
                        CreatedAt = DateTime.Now
                    })
                    .ToList();

                _context.MedicalReps.Add(medicalRep);
                _context.SaveChanges();

                lblMessage.CssClass = "text-success fw-semibold";
                lblMessage.Text = "Medical Rep saved successfully.";
                ClearForm();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.CssClass = "text-danger fw-semibold";
            }
        }

        private void ClearForm()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtContact.Text = string.Empty;
            ddlMedicalRepType.SelectedIndex = 0;
            ViewState["AssignedSubGroupIDs"] = new List<int>();
            BindAssignedSubGroups();
        }
    }
}