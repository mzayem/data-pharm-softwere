using data_pharm_softwere.Data;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.SubGroup
{
    public partial class SubGroupPage : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSubGroups();
            }
        }

        private void LoadSubGroups(string search = "")
        {
            var query = _context.SubGroups.Select(sg => new
            {
                sg.SubGroupID,
                sg.Name,
                GroupName = sg.Group.Name,
                VendorName = sg.Group.Vendor.Name,
                sg.CreatedAt
            });

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(sg =>
                    sg.Name.Contains(search) ||
                    sg.GroupName.Contains(search) ||
                    sg.VendorName.Contains(search)
                );
            }

            gvSubGroups.DataSource = query.OrderByDescending(sg => sg.CreatedAt).ToList();
            gvSubGroups.DataBind();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();
            ViewState["SubGroupSearch"] = search;
            LoadSubGroups(search);
        }

        protected void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            HiddenField hf = (HiddenField)row.FindControl("hfSubGroupID");

            int subGroupId = int.Parse(hf.Value);
            string action = ddl.SelectedValue;

            if (action == "Edit")
            {
                Response.Redirect($"/subgroup/edit?id={subGroupId}");
            }
            else if (action == "Delete")
            {
                var subGroup = _context.SubGroups.Find(subGroupId);
                if (subGroup != null)
                {
                    _context.SubGroups.Remove(subGroup);
                    _context.SaveChanges();
                    LoadSubGroups(txtSearch.Text.Trim());
                }
            }
        }

        protected void gvSubGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string id = DataBinder.Eval(e.Row.DataItem, "SubGroupID").ToString();
                e.Row.Attributes["onclick"] = $"window.location='/subgroup/edit?id={id}'";
                e.Row.Style["cursor"] = "pointer";

                foreach (TableCell cell in e.Row.Cells)
                {
                    foreach (Control control in cell.Controls)
                    {
                        if (control is DropDownList ddl)
                            ddl.Attributes["onclick"] = "event.stopPropagation();";
                    }
                }
            }
        }

        protected void gvSubGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Not used because dropdown handles commands
        }
    }
}