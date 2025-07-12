using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Group
{
    public partial class GroupPage : System.Web.UI.Page
    {
        private DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGroups();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ImportInfoControl.DownloadRequested += ImportInfoControl_DownloadRequested;
        }

        private void LoadGroups(string search = "")
        {
            if (!_context.Groups.Any())
            {
                Response.Redirect("/group/create");
                return;
            }
            var query = _context.Groups.Select(g => new
            {
                g.GroupID,
                g.Name,
                DivisionName = g.Division.Name,
                g.CreatedAt
            });

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(g =>
                    g.Name.Contains(search) ||
                    g.DivisionName.Contains(search)
                );
            }

            gvGroups.DataSource = query.OrderByDescending(g => g.CreatedAt).ToList();
            gvGroups.DataBind();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();
            ViewState["GroupSearch"] = search;
            LoadGroups(search);
        }

        protected void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            HiddenField hf = (HiddenField)row.FindControl("hfGroupID");

            int groupId = int.Parse(hf.Value);
            string action = ddl.SelectedValue;

            if (action == "Edit")
            {
                Response.Redirect($"/group/edit?id={groupId}");
            }
            else if (action == "Delete")
            {
                var group = _context.Groups.Find(groupId);
                if (group != null)
                {
                    _context.Groups.Remove(group);
                    _context.SaveChanges();
                    LoadGroups(txtSearch.Text.Trim());
                }
            }
        }

        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            string search = ViewState["GroupSearch"]?.ToString() ?? "";
            var groups = _context.Groups
                .Where(g => string.IsNullOrEmpty(search) || g.Name.Contains(search) || g.Division.Name.Contains(search))
                .Select(g => new
                {
                    g.GroupID,
                    g.Name,
                    DivisionName = g.Division.Name,
                    g.CreatedAt
                })
                .OrderBy(g => g.Name)
                .ToList();

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 10f);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                pdfDoc.Add(new Paragraph("Group Report", FontFactory.GetFont("Arial", 14, Font.BOLD)));
                pdfDoc.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                pdfDoc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(4) { WidthPercentage = 100 };
                string[] headers = { "ID", "Group Name", "Division", "Created At" };

                foreach (string header in headers)
                {
                    var cell = new PdfPCell(new Phrase(header, FontFactory.GetFont("Arial", 9, Font.BOLD)))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY
                    };
                    table.AddCell(cell);
                }

                foreach (var g in groups)
                {
                    table.AddCell(g.GroupID.ToString());
                    table.AddCell(g.Name);
                    table.AddCell(g.DivisionName);
                    table.AddCell(g.CreatedAt.ToString("yyyy-MM-dd"));
                }

                pdfDoc.Add(table);
                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename=Group_Report_{DateTime.Now:yyyyMMddHHmmss}.pdf");
                Response.BinaryWrite(memoryStream.ToArray());
                Response.End();
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string search = ViewState["GroupSearch"]?.ToString() ?? "";
            var groups = _context.Groups
                .Where(g => string.IsNullOrEmpty(search) || g.Name.Contains(search) || g.Division.Name.Contains(search))
                .Select(g => new
                {
                    g.GroupID,
                    g.Name,
                    DivisionName = g.Division.Name,
                    g.CreatedAt
                })
                .OrderBy(g => g.Name)
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("GroupID,Group Name,Division,Created At");

            foreach (var g in groups)
            {
                sb.AppendLine($"{g.GroupID},{EscapeCsv(g.Name)},{EscapeCsv(g.DivisionName)},{g.CreatedAt:yyyy-MM-dd}");
            }

            Response.Clear();
            Response.AddHeader("content-disposition", $"attachment;filename=Group_Report_{DateTime.Now:yyyyMMddHHmmss}.csv");
            Response.ContentType = "text/csv";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        private string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "\"\"";
            value = value.Replace("\"", "\"\"");
            return $"\"{value}\"";
        }

        protected void gvGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string groupId = DataBinder.Eval(e.Row.DataItem, "GroupID").ToString();
                e.Row.Attributes["onclick"] = $"window.location='/group/edit?id={groupId}'";
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

        protected void gvGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Optional: Not used because dropdown handles actions
        }

        //Import functions

        //Sample file
        private void ImportInfoControl_DownloadRequested(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=group_sample.csv");

            Response.Write("Name,DivisionID\r\n");
            Response.Write("Group A,1\r\n");
            Response.Write("Group B,2\r\n");

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
                    int colDivisionID = headers.IndexOf("DivisionID");

                    if (colName == -1 || colDivisionID == -1)
                    {
                        lblImportStatus.Text = "Missing required columns in CSV.";
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
                            string rawName = values[colName].Trim();
                            string rawDivisionID = values[colDivisionID].Trim();

                            if (string.IsNullOrWhiteSpace(rawName))
                                throw new Exception("Name is required.");

                            if (!int.TryParse(rawDivisionID, out int divisionId))
                                throw new Exception($"Invalid DivisionID '{rawDivisionID}'");

                            if (!_context.Divisions.Any(d => d.DivisionID == divisionId))
                                throw new Exception($"DivisionID '{divisionId}' not found in DB.");

                            var existing = _context.Groups
                                .FirstOrDefault(g => g.Name == rawName && g.DivisionID == divisionId);

                            if (existing != null)
                            {
                                // Optionally update — for now we skip duplicates
                                skipCount++;
                                continue;
                            }

                            var group = new Models.Group
                            {
                                Name = rawName,
                                DivisionID = divisionId,
                                CreatedAt = DateTime.Now
                            };

                            _context.Groups.Add(group);
                            insertCount++;
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

                    LoadGroups(); // Make sure you have this method to reload data
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