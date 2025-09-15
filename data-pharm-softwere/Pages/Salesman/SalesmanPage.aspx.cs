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

        //Excel Export
        private string EscapeCsv(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            if (input.Contains(",") || input.Contains("\"") || input.Contains("\n"))
            {
                input = input.Replace("\"", "\"\"");
                return $"\"{input}\"";
            }

            return input;
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            var search = txtSearch.Text.Trim();
            int.TryParse(ddlTown.SelectedValue, out int townId);

            var query = _context.Salesmen
                .Include(s => s.SalesmanTowns.Select(st => st.Town))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.Name.Contains(search) ||
                    s.SalesmanID.ToString().Contains(search) ||
                    s.Contact.Contains(search));
            }

            if (townId > 0)
            {
                query = query.Where(s => s.SalesmanTowns.Any(st => st.TownID == townId));
            }

            var salesmen = query.OrderBy(s => s.SalesmanID).ToList();

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("ID,Name,Email,Contact,Towns,Percentages,Types,Created At");

            foreach (var s in salesmen)
            {
                var townNames = string.Join(" | ", s.SalesmanTowns
                    .Select(st => st.Town?.Name)
                    .Where(t => !string.IsNullOrEmpty(t)));
                var percentages = string.Join(" | ", s.SalesmanTowns
            .Select(st => st.Percentage.ToString("0.##") + " %"));

                var types = string.Join(" | ", s.SalesmanTowns
                    .Select(st => st.AssignmentType.ToString()));

                sb.AppendLine(string.Join(",", new string[] {
            s.SalesmanID.ToString("D4"),
            EscapeCsv(s.Name),
            EscapeCsv(s.Email),
            EscapeCsv(s.Contact),
            EscapeCsv(townNames),
            EscapeCsv(percentages),
            EscapeCsv(types),
            "=\"" + s.CreatedAt.ToString("yyyy-MM-dd HH:mm") + "\""
        }));
            }

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", $"attachment;filename=Salesman_Report_{DateTime.Now:yyyyMMdd}.csv");
            Response.ContentType = "text/csv";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        //PDF Export
        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 10f);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                var titleFont = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD);
                var subTitleFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
                var labelFont = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD);
                var bodyFont = FontFactory.GetFont("Arial", 8);

                pdfDoc.Add(new Paragraph("Data Pharma - Salesman Report", titleFont));
                pdfDoc.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), bodyFont));
                string searchText = string.IsNullOrWhiteSpace(txtSearch.Text) ? "All Salesmen" : $"Search: \"{txtSearch.Text}\"";
                string townText = string.IsNullOrEmpty(ddlTown.SelectedValue) || ddlTown.SelectedValue == "0"
                    ? "All Towns"
                    : ddlTown.SelectedItem.Text;
                string filterSummary = $"This report includes data for {searchText}, filtered by {townText}.";
                pdfDoc.Add(new Paragraph(filterSummary, bodyFont));
                pdfDoc.Add(new Paragraph(" "));

                var search = txtSearch.Text.Trim();
                int.TryParse(ddlTown.SelectedValue, out int townId);

                var query = _context.Salesmen
                    .Include(s => s.SalesmanTowns.Select(st => st.Town))
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(s =>
                        s.Name.Contains(search) ||
                        s.SalesmanID.ToString().Contains(search) ||
                        s.Contact.Contains(search));
                }

                if (townId > 0)
                {
                    query = query.Where(s => s.SalesmanTowns.Any(st => st.TownID == townId));
                }

                var salesmen = query.OrderBy(s => s.SalesmanID).ToList();

                foreach (var s in salesmen)
                {
                    pdfDoc.Add(new Paragraph($"Name: {s.Name}", subTitleFont));

                    Paragraph detailsLine = new Paragraph();
                    detailsLine.Add(new Chunk("ID: ", labelFont));
                    detailsLine.Add(new Chunk(s.SalesmanID.ToString(), bodyFont));
                    detailsLine.Add(new Chunk("   Email: ", labelFont));
                    detailsLine.Add(new Chunk(string.IsNullOrEmpty(s.Email) ? "-" : s.Email, bodyFont));
                    detailsLine.Add(new Chunk("   Contact: ", labelFont));
                    detailsLine.Add(new Chunk(string.IsNullOrEmpty(s.Contact) ? "-" : s.Contact, bodyFont));

                    pdfDoc.Add(detailsLine);
                    pdfDoc.Add(new Paragraph(" ")); // spacing

                    PdfPTable table = new PdfPTable(3) { WidthPercentage = 100, SpacingAfter = 15f };
                    table.SetWidths(new float[] { 3f, 2f, 2f });

                    table.DefaultCell.Border = Rectangle.NO_BORDER;

                    string[] headers = { "Town", "Percentage", "Type" };

                    foreach (var h in headers)
                    {
                        PdfPCell headerCell = new PdfPCell(new Phrase(h, labelFont))
                        {
                            BackgroundColor = BaseColor.WHITE,
                            Padding = 5,
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            Border = Rectangle.NO_BORDER
                        };

                        // Solid border for header row
                        headerCell.BorderWidthBottom = 1f;
                        headerCell.BorderColorBottom = BaseColor.BLACK;

                        table.AddCell(headerCell);
                    }

                    // Town rows
                    foreach (var st in s.SalesmanTowns)
                    {
                        table.AddCell(CreateWrappedCell(st.Town.Name, bodyFont));
                        table.AddCell(CreateWrappedCell(st.Percentage.ToString("0.##") + " %", bodyFont));
                        table.AddCell(CreateWrappedCell(st.AssignmentType.ToString(), bodyFont));
                    }

                    table.CompleteRow();
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 15f;
                    table.TableEvent = new TableBoxBorder();
                    pdfDoc.Add(table);
                }
                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename=Salesman_Report_{DateTime.Now:dd_MMMM_yyyy}.pdf");
                Response.BinaryWrite(memoryStream.ToArray());
                Response.End();
            }
        }

        private PdfPCell CreateWrappedCell(string text, Font font)
        {
            var cell = new PdfPCell(new Phrase(text, font))
            {
                NoWrap = false,
                Padding = 5,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_TOP,
                Border = Rectangle.NO_BORDER
            };

            cell.CellEvent = new DottedCellBorder();

            return cell;
        }

        public class DottedCellBorder : IPdfPCellEvent
        {
            public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
            {
                PdfContentByte cb = canvases[PdfPTable.LINECANVAS];
                cb.SetLineDash(1f, 2f);
                cb.SetLineWidth(0.5f);
                cb.SetColorStroke(new BaseColor(200, 200, 200));
                cb.MoveTo(position.Left, position.Bottom);
                cb.LineTo(position.Right, position.Bottom);
                cb.Stroke();
                cb.SetLineDash(1f);
            }
        }

        public class TableBoxBorder : IPdfPTableEvent
        {
            public void TableLayout(PdfPTable table, float[][] widths, float[] heights,
                                    int headerRows, int rowStart, PdfContentByte[] canvases)
            {
                Rectangle rect = new Rectangle(widths[0][0], heights[heights.Length - 1],
                                               widths[0][widths[0].Length - 1], heights[0]);

                PdfContentByte cb = canvases[PdfPTable.LINECANVAS];
                cb.SetLineWidth(1f);
                cb.SetColorStroke(BaseColor.BLACK);
                cb.Rectangle(rect.Left, rect.Bottom, rect.Width, rect.Height);
                cb.Stroke();
            }
        }

        //Import functions

        //Sample file
        private void ImportInfoControl_DownloadRequested(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=Salesman_sample.csv");

            Response.Write("Name,Email,Contact,Towns,Percentages,Types\r\n");
            Response.Write("John Doe,john@email.com,03001234567,TownA|TownB|TownC,50|30|20,Booker|Supplier|Driver\r\n");

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
                    int colPercentages = headers.IndexOf("Percentages");
                    int colTypes = headers.IndexOf("Types");

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
                            string percentageRaw = SafeGet(fields, colPercentages);
                            string typeRaw = SafeGet(fields, colTypes);

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
                                           .Select(t => t.Trim()).ToList();

                            var percentages = (percentageRaw ?? "").Split(new[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(p => p.Trim()).ToList();

                            var types = (typeRaw ?? "").Split(new[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(t => t.Trim()).ToList();

                            for (int i = 0; i < townNames.Count; i++)
                            {
                                var townName = townNames[i];
                                var town = MatchTown(townName, out string suggestion);
                                if (town == null)
                                {
                                    string msg = $"Town '{townName}' not found.";
                                    if (!string.IsNullOrEmpty(suggestion))
                                        msg += $" Did you mean '{suggestion}'?";
                                    throw new Exception(msg);
                                }

                                // Try to parse percentage (default 0)
                                decimal percentage = 0;
                                if (i < percentages.Count && decimal.TryParse(percentages[i].Replace("%", "").Trim(), out var parsedPct))
                                    percentage = parsedPct;

                                // Try to parse type (default Primary)
                                Models.AssignmentType type = Models.AssignmentType.Booker;
                                if (i < types.Count && Enum.TryParse(types[i], true, out Models.AssignmentType parsedType))
                                    type = parsedType;

                                bool alreadyAssigned = existing.SalesmanTowns.Any(st => st.TownID == town.TownID);
                                if (!alreadyAssigned)
                                {
                                    existing.SalesmanTowns.Add(new Models.SalesmanTown
                                    {
                                        TownID = town.TownID,
                                        Percentage = percentage,
                                        AssignmentType = type,
                                        AssignedOn = DateTime.Now
                                    });
                                }
                                else
                                {
                                    var st = existing.SalesmanTowns.First(x => x.TownID == town.TownID);
                                    st.Percentage = percentage;
                                    st.AssignmentType = type;
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