using data_pharm_softwere.Data;
using Org.BouncyCastle.Tls.Crypto;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.CityRoute
{
    public partial class CityRoutePage : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCityRoute();
            }
        }

        private void LoadCityRoute(string search = "")
        {
            var query = _context.CityRoutes.Select(b => new
            {
                b.CityRouteID,
                b.Name,
                b.CreatedAt,
            });

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b =>
                    b.Name.Contains(search));
            }

            var sortedList = query
                .ToList()
                .OrderBy(b => b.CityRouteID)
                .ToList();

            gvBatches.DataSource = sortedList;
            gvBatches.DataBind();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCityRoute(txtSearch.Text.Trim());
        }

        protected void gvBatches_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            HiddenField hf = (HiddenField)row.FindControl("hfCityRouteId");

            if (int.TryParse(hf.Value, out int cityRouteId))
            {
                string action = ddl.SelectedValue;

                if (action == "Edit")
                {
                    Response.Redirect($"/city-route/edit?id={cityRouteId}");
                }
                else if (action == "Delete")
                {
                    var cityRoute = _context.CityRoutes.Find(cityRouteId);
                    if (cityRoute != null)
                    {
                        _context.CityRoutes.Remove(cityRoute);
                        _context.SaveChanges();
                        LoadCityRoute(txtSearch.Text.Trim());
                    }
                }
            }
        }

        protected void gvBatches_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string id = DataBinder.Eval(e.Row.DataItem, "CityRouteID").ToString(); // fix casing
                e.Row.Attributes["onclick"] = $"window.location='/city-route/edit?id={id}'";
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