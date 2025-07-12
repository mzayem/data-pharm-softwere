using data_pharm_softwere.Data;
using Org.BouncyCastle.Tls.Crypto;
using System;
using System.Collections.Generic;
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

        protected void Page_Init(object sender, EventArgs e)
        {
            ImportInfoControl.DownloadRequested += ImportInfoControl_DownloadRequested;
        }

        private void LoadCityRoute(string search = "")
        {
            if (!_context.CityRoutes.Any())
            {
                Response.Redirect("/city-route/create");
                return;
            }
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

        //Import functions

        //Sample file
        private void ImportInfoControl_DownloadRequested(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=cityRoute_sample.csv");

            Response.Write("Name\r\n");
            Response.Write("Route1\r\n");
            Response.Write("Route2\r\n");

            Response.End();
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (!fuCSV.HasFile || !fuCSV.FileName.EndsWith(".csv"))
            {
                lblImportStatus.Text = "Please upload a valid CSV file.";
                lblImportStatus.CssClass = "alert alert-danger d-block";
                return;
            }

            try
            {
                using (var reader = new System.IO.StreamReader(fuCSV.FileContent))
                {
                    string headerLine = reader.ReadLine();
                    if (headerLine == null)
                    {
                        lblImportStatus.Text = "CSV file is empty.";
                        lblImportStatus.CssClass = "alert alert-danger d-block";
                        return;
                    }

                    var headers = headerLine.Split(',').Select(h => h.Trim()).ToList();
                    int colName = headers.IndexOf("Name");

                    if (colName == -1)
                    {
                        lblImportStatus.Text = "Missing 'Name' column in CSV.";
                        lblImportStatus.CssClass = "alert alert-danger d-block";
                        return;
                    }

                    int lineNo = 1;
                    int insertCount = 0;
                    int skipCount = 0;
                    var errorMessages = new List<string>();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        lineNo++;

                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var values = line.Split(',');

                        try
                        {
                            string name = values[colName].Trim();

                            if (string.IsNullOrWhiteSpace(name))
                                throw new Exception("Route Name cannot be empty.");

                            var existing = _context.CityRoutes.FirstOrDefault(r => r.Name == name);

                            if (existing != null)
                            {
                                skipCount++;
                            }
                            else
                            {
                                var route = new Models.CityRoute
                                {
                                    Name = name,
                                    CreatedAt = DateTime.Now
                                };

                                _context.CityRoutes.Add(route);
                                insertCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            errorMessages.Add($"Line {lineNo}: {ex.Message}");
                        }
                    }

                    _context.SaveChanges();

                    if (errorMessages.Any())
                    {
                        lblImportStatus.Text = $"Import completed: {insertCount} added, {skipCount} duplicates skipped." +
                            "<br><b>Errors:</b><br>" +
                            string.Join("<br>", errorMessages.Take(10)) +
                            (errorMessages.Count > 10 ? "<br>...and more." : "");
                        lblImportStatus.CssClass = "alert alert-danger d-block";
                    }
                    else
                    {
                        lblImportStatus.Text = $"Import completed: {insertCount} added, {skipCount} duplicates skipped.";
                        lblImportStatus.CssClass = "alert alert-success mt-3 d-block";
                    }

                    LoadCityRoute();
                }
            }
            catch (Exception ex)
            {
                lblImportStatus.Text = $"Import failed: {ex.Message}";
                lblImportStatus.CssClass = "alert alert-danger mt-3 d-block";
            }
        }
    }
}