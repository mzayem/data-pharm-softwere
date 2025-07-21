using data_pharm_softwere.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using ListItem = System.Web.UI.WebControls.ListItem;

namespace data_pharm_softwere.Pages.Salesman
{
    public partial class SalesmanPage : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTowns();
                LoadSalesman();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ImportInfoControl.DownloadRequested += ImportInfoControl_DownloadRequested;
        }

        private void LoadTowns()
        {
            var towns = _context.Towns.OrderBy(t => t.Name).ToList();
            ddlTown.DataSource = towns;
            ddlTown.DataTextField = "Name";
            ddlTown.DataValueField = "TownID";
            ddlTown.DataBind();
            ddlTown.Items.Insert(0, new ListItem("-- Select Town --", ""));
        }

        private void LoadSalesman(string search = "")
        {
            var query = _context.Salesmen.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.Name.Contains(search) ||
                    s.SalesmanID.ToString().Contains(search) ||
                    s.Contact.Contains(search));
            }

            if (int.TryParse(ddlTown.SelectedValue, out int townId) && townId > 0)
            {
                query = query.Where(s => s.SalesmanTowns.Any(st => st.TownID == townId));
            }

            var result = query.OrderBy(s => s.SalesmanID).ToList();

            gvSalesmen.DataSource = result;
            gvSalesmen.DataBind();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadSalesman(txtSearch.Text.Trim());
        }

        protected void ddlTown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSalesman(txtSearch.Text.Trim());
        }

        protected string GetLimitedTownNames(int salesmanID, int max = 3)
        {
            var townNames = _context.SalesmanTowns
                .Where(st => st.SalesmanID == salesmanID)
                .Select(st => st.Town.Name)
                .ToList();

            return string.Join(", ", townNames.Take(max));
        }

        protected string GetTownOverflowText(int salesmanID, int max = 3)
        {
            var count = _context.SalesmanTowns.Count(st => st.SalesmanID == salesmanID);
            return count > max ? $"+{count - max} more" : string.Empty;
        }

        protected void gvSalesmen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var salesman = (Models.Salesman)e.Row.DataItem;

                string id = salesman.SalesmanID.ToString();
                e.Row.Attributes["onclick"] = $"window.location='/salesman/edit?id={id}'";
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

        protected void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            HiddenField hf = (HiddenField)row.FindControl("hfSalesmanId");

            if (int.TryParse(hf.Value, out int salesmanId))
            {
                string action = ddl.SelectedValue;

                if (action == "Edit")
                {
                    Response.Redirect($"/salesman/edit?id={salesmanId}");
                }
                else if (action == "Delete")
                {
                    var salesman = _context.Salesmen
                        .Include(s => s.SalesmanTowns)
                        .FirstOrDefault(s => s.SalesmanID == salesmanId);

                    if (salesman != null)
                    {
                        // Remove related towns first
                        var relatedTowns = _context.SalesmanTowns.Where(st => st.SalesmanID == salesmanId);
                        _context.SalesmanTowns.RemoveRange(relatedTowns);

                        // Then remove the salesman
                        _context.Salesmen.Remove(salesman);
                        _context.SaveChanges();

                        LoadSalesman(txtSearch.Text.Trim());
                    }
                }
            }
        }

        protected void gvSalesmen_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        //Import functions

        //Sample file
        private void ImportInfoControl_DownloadRequested(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=Salesman_sample.csv");

            Response.Write("Name,Email,Contact,Towns\r\n");
            Response.Write("John Doe,john@email.com,03001234567,TownA|TownB|TownC\r\n");

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
                using (var reader = new StreamReader(fuCSV.FileContent))
                {
                    string headerLine = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(headerLine))
                    {
                        lblImportStatus.Text = "CSV file is empty.";
                        lblImportStatus.CssClass = "alert alert-danger d-block";
                        return;
                    }

                    var headers = headerLine.Split(',').Select(h => h.Trim()).ToList();

                    int colName = headers.IndexOf("Name");
                    int colEmail = headers.IndexOf("Email");
                    int colContact = headers.IndexOf("Contact");
                    int colTowns = headers.IndexOf("Towns");

                    if (colName == -1 || colContact == -1 || colTowns == -1)
                    {
                        lblImportStatus.Text = "Missing required columns: Name, Contact, or Towns.";
                        lblImportStatus.CssClass = "alert alert-danger d-block";
                        return;
                    }

                    int insertCount = 0, updateCount = 0, lineNo = 1;
                    var errorMessages = new List<string>();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        lineNo++;
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var fields = line.Split(',');
                        try
                        {
                            string name = SafeGet(fields, colName);
                            string contact = SafeGet(fields, colContact);
                            string email = SafeGet(fields, colEmail);
                            string townRaw = SafeGet(fields, colTowns);

                            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(contact) || string.IsNullOrWhiteSpace(townRaw))
                                throw new Exception("Name, Contact, and Towns are required.");

                            var existing = _context.Salesmen.FirstOrDefault(s => s.Name == name && s.Contact == contact);

                            if (existing == null)
                            {
                                existing = new Models.Salesman
                                {
                                    Name = name,
                                    Contact = contact,
                                    Email = email,
                                    CreatedAt = DateTime.Now,
                                    SalesmanTowns = new List<Models.SalesmanTown>()
                                };
                                _context.Salesmen.Add(existing);
                                insertCount++;
                            }
                            else
                            {
                                existing.Email = email;
                                updateCount++;
                            }

                            var townNames = townRaw.Split(new[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries)
                       .Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).Distinct();

                            foreach (var townName in townNames)
                            {
                                var town = MatchTown(townName, out string suggestion);
                                if (town == null)
                                {
                                    string msg = $"Town '{townName}' not found.";
                                    if (!string.IsNullOrEmpty(suggestion))
                                        msg += $" Did you mean '{suggestion}'?";
                                    throw new Exception(msg);
                                }

                                bool alreadyAssigned = existing.SalesmanTowns.Any(st => st.TownID == town.TownID);
                                if (!alreadyAssigned)
                                {
                                    existing.SalesmanTowns.Add(new Models.SalesmanTown
                                    {
                                        TownID = town.TownID,
                                        AssignedOn = DateTime.Now
                                    });
                                }
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
                        lblImportStatus.Text = $"Imported: {insertCount} added, {updateCount} updated.<br><strong>Errors:</strong><br>{string.Join("<br>", errorMessages.Take(10))}" +
                                               (errorMessages.Count > 10 ? "<br>...and more." : "");
                        lblImportStatus.CssClass = "alert alert-danger d-block";
                    }
                    else
                    {
                        lblImportStatus.Text = $"Import successful: {insertCount} added, {updateCount} updated.";
                        lblImportStatus.CssClass = "alert alert-success mt-3 d-block";
                    }

                    LoadSalesman(txtSearch.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                lblImportStatus.Text = $"Import failed: {ex.Message}";
                lblImportStatus.CssClass = "alert alert-danger mt-3 d-block";
            }
        }

        private string SafeGet(string[] arr, int index)
        {
            return index >= 0 && index < arr.Length ? arr[index].Trim() : null;
        }

        private Models.Town MatchTown(string inputTown, out string suggestion)
        {
            suggestion = null;

            if (string.IsNullOrWhiteSpace(inputTown)) return null;

            string normalizedInput = Regex.Replace(inputTown.ToLowerInvariant(), @"[\s\-_]", "");

            var towns = _context.Towns.ToList();
            foreach (var town in towns)
            {
                string normalizedDbTown = Regex.Replace(town.Name.ToLowerInvariant(), @"[\s\-_]", "");
                if (normalizedDbTown == normalizedInput)
                    return town;
            }

            // Fuzzy match with suggestion
            int minDistance = int.MaxValue;
            Models.Town closest = null;

            foreach (var town in towns)
            {
                string dbTownNormalized = Regex.Replace(town.Name.ToLowerInvariant(), @"[\s\-_]", "");
                int distance = GetLevenshteinDistance(normalizedInput, dbTownNormalized);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = town;
                }
            }

            if (minDistance <= 2 && closest != null)
            {
                suggestion = closest.Name;
                return null; // Still return null to trigger the error with suggestion
            }

            return null;
        }

        private int GetLevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s)) return t?.Length ?? 0;
            if (string.IsNullOrEmpty(t)) return s.Length;

            var d = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++) d[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[s.Length, t.Length];
        }

    }
}