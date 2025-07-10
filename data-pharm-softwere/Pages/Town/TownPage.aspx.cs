using data_pharm_softwere.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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

        protected void Page_Init(object sender, EventArgs e)
        {
            ImportInfoControl.DownloadRequested += ImportInfoControl_DownloadRequested;
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
                string id = DataBinder.Eval(e.Row.DataItem, "TownID").ToString();
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

        //Import functions

        //Sample file
        private void ImportInfoControl_DownloadRequested(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=town_sample.csv");

            Response.Write("Name,CityRouteID\r\n");
            Response.Write("Town1,1\r\n");
            Response.Write("Town2,2\r\n");

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
                    int colCityRouteID = headers.IndexOf("CityRouteID");

                    if (colName == -1 || colCityRouteID == -1)
                    {
                        lblImportStatus.Text = "Missing required columns in CSV.";
                        lblImportStatus.CssClass = "alert alert-danger d-block";
                        return;
                    }

                    int insertCount = 0;
                    int skipCount = 0;
                    int lineNo = 1;
                    var errorMessages = new List<string>();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        lineNo++;

                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var values = line.Split(',');

                        try
                        {
                            string rawName = values[colName].Trim();
                            string rawCityRouteID = values[colCityRouteID].Trim();

                            if (string.IsNullOrWhiteSpace(rawName))
                                throw new Exception("Town name is required.");

                            if (!int.TryParse(rawCityRouteID, out int cityRouteId))
                                throw new Exception($"Invalid CityRouteID '{rawCityRouteID}'.");

                            if (!_context.CityRoutes.Any(c => c.CityRouteID == cityRouteId))
                                throw new Exception($"CityRouteID '{cityRouteId}' not found.");

                            var existing = _context.Towns
                                .FirstOrDefault(t => t.Name == rawName && t.CityRouteID == cityRouteId);

                            if (existing != null)
                            {
                                // Already exists – skip or update logic
                                skipCount++;
                                continue;
                            }

                            var town = new Models.Town
                            {
                                Name = rawName,
                                CityRouteID = cityRouteId,
                                CreatedAt = DateTime.Now
                            };

                            _context.Towns.Add(town);
                            insertCount++;
                        }
                        catch (Exception ex)
                        {
                            errorMessages.Add($"Line {lineNo}: {ex.Message}");
                        }
                    }

                    _context.SaveChanges();

                    lblImportStatus.Text = $"Import completed: {insertCount} added, {skipCount} skipped.";
                    lblImportStatus.CssClass = "alert alert-success mt-3 d-block";

                    if (errorMessages.Any())
                    {
                        lblImportStatus.Text += "<br><b>Errors:</b><br>" +
                            string.Join("<br>", errorMessages.Take(10)) +
                            (errorMessages.Count > 10 ? "<br>...and more." : "");
                    }

                    LoadTown(txtSearch.Text.Trim());
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