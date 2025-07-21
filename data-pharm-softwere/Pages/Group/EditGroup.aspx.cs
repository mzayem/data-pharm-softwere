using System;
using System.Linq;
using System.Web.UI.WebControls;
using data_pharm_softwere.Data;
using data_pharm_softwere.Models;

namespace data_pharm_softwere.Pages.Group
{
    public partial class EditGroup : System.Web.UI.Page
    {
        private DataPharmaContext _context = new DataPharmaContext();
        private int GroupId
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
                LoadDivisions();
                LoadGroup();
            }
        }

        private void LoadDivisions()
        {
            var divisions = _context.Divisions.ToList();
            ddlDivision.DataSource = divisions;
            ddlDivision.DataTextField = "Name";
            ddlDivision.DataValueField = "DivisionID";
            ddlDivision.DataBind();
            ddlDivision.Items.Insert(0, new ListItem("-- Select Division --", ""));
        }

        private void LoadGroup()
        {
            var group = _context.Groups.FirstOrDefault(v => v.GroupID == GroupId);
            if (group == null)
            {
                Response.Redirect("/group");
                return;
            }

            txtName.Text = group.Name;
            ddlDivision.SelectedValue = group.DivisionID.ToString();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var group = _context.Groups.FirstOrDefault(v => v.GroupID == GroupId);
                if (group == null)
                {
                    Response.Redirect("/group");
                    return;
                }

                group.Name = txtName.Text.Trim();
                group.DivisionID = int.Parse(ddlDivision.SelectedValue);

                _context.SaveChanges();
                Response.Redirect("/group");
            }
        }
    }
}