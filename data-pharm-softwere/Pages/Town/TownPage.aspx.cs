using data_pharm_softwere.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Town
{
    public partial class TownPage : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTown();
                LoadCityRoutes();
            }
        }

        private void LoadCityRoutes()
        {
            var routes = _context.CityRoutes
                .OrderBy(c => c.Name)
                .ToList();

            ddlCityRoute.DataSource = routes;
            ddlCityRoute.DataTextField = "Name";
            ddlCityRoute.DataValueField = "CityRouteID";
            ddlCityRoute.DataBind();
            ddlCityRoute.Items.Insert(0, new ListItem("All Routes", ""));
        }

        private void LoadTown(string search = "")
        {
            var allTowns = _context.Towns.Include("CityRoute");

            if (!_context.Towns.Any())
            {
                Response.Redirect("/town/create");
                return;
            }

            var query = allTowns.Select(b => new
            {
                b.TownID,
                b.Name,
                b.CityRouteID,
                CityRouteName = b.CityRoute.Name,
                b.CreatedAt
            });

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b =>
                    b.TownID.ToString().Contains(search) ||
                    b.Name.Contains(search) ||
                    b.CityRouteName.Contains(search)
                );
            }

            if (!string.IsNullOrEmpty(ddlCityRoute.SelectedValue))
            {
                int cityRouteId = int.Parse(ddlCityRoute.SelectedValue);
                query = query.Where(t => t.CityRouteID == cityRouteId);
            }

            var sortedList = query
                .OrderBy(b => b.TownID)
                .ToList();

            gvBatches.DataSource = sortedList;
            gvBatches.DataBind();
        }


        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadTown(txtSearch.Text.Trim());
        }

        protected void ddlCityRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTown(txtSearch.Text.Trim());
        }

        protected void gvBatches_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            HiddenField hf = (HiddenField)row.FindControl("hfTownID");

            if (int.TryParse(hf.Value, out int TownId))
            {
                string action = ddl.SelectedValue;

                if (action == "Edit")
                {
                    Response.Redirect($"/town/edit?id={TownId}");
                }
                else if (action == "Delete")
                {
                    var town = _context.Towns.Find(TownId);
                    if (town != null)
                    {
                        _context.Towns.Remove(town);
                        _context.SaveChanges();
                        LoadTown(txtSearch.Text.Trim());
                    }
                }
            }
        }

        protected void gvBatches_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string id = DataBinder.Eval(e.Row.DataItem, "TownID").ToString(); // fix casing
                e.Row.Attributes["onclick"] = $"window.location='/town/edit?id={id}'";
                e.Row.Style["cursor"] = "pointer";

                foreach (TableCell cell in e.Row.Cells)
                {
                    foreach (Control control in cell.Controls)
                    {
                        if (control is DropDownList ddl)
                        {
                            ddl.Attributes["onclick"] = "event.stopPropagation();";
                            ddl.Attributes["onmousedown"] = "event.stopPropagation();";
                            ddl.Attributes["onmouseup"] = "event.stopPropagation();";
                        }
                    }
                }
            }
        }
    }
}