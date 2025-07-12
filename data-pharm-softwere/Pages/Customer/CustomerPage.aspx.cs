using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ListItem = System.Web.UI.WebControls.ListItem;

namespace data_pharm_softwere.Pages.Customer
{
    public partial class CustomerPage : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCityRoutes();
                LoadTowns();
                LoadPartyTypes();
                LoadCustomers();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ImportInfoControl.DownloadRequested += ImportInfoControl_DownloadRequested;
        }

        private void LoadCityRoutes()
        {
            ddlCityRoute.DataSource = _context.CityRoutes.OrderBy(r => r.Name).ToList();
            ddlCityRoute.DataTextField = "Name";
            ddlCityRoute.DataValueField = "CityRouteID";
            ddlCityRoute.DataBind();
            ddlCityRoute.Items.Insert(0, new ListItem("-- All Routes --", ""));
        }

        private void LoadTowns(int? cityRouteId = null)
        {
            var towns = _context.Towns.AsQueryable();

            if (cityRouteId.HasValue && cityRouteId > 0)
            {
                towns = towns.Where(t => t.CityRouteID == cityRouteId);
            }

            ddlTown.DataSource = towns.OrderBy(t => t.Name).ToList();
            ddlTown.DataTextField = "Name";
            ddlTown.DataValueField = "TownID";
            ddlTown.DataBind();
            ddlTown.Items.Insert(0, new ListItem("-- Select Town --", ""));
        }

        private void LoadPartyTypes()
        {
            ddlPartyType.DataSource = Enum.GetValues(typeof(PartyType));
            ddlPartyType.DataBind();
            ddlPartyType.Items.Insert(0, new ListItem("-- Select Part Type --", ""));
        }

        private void LoadCustomers(string search = "")
        {
            if (!_context.Customers.Any())
            {
                Response.Redirect("/customer/create");
                return;
            }
            var query = _context.Customers
                .Select(c => new
                {
                    c.CustomerId,
                    c.Name,
                    c.Contact,
                    c.CNIC,
                    TownName = c.Town.Name,
                    CityRouteName = c.Town.CityRoute.Name,
                    c.PartyType,
                    c.NtnNo,
                    c.InActive,
                    c.CreatedAt
                });

            // Apply search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    c.Name.Contains(search) ||
                    c.CustomerId.ToString().Contains(search) ||
                    c.NtnNo.Contains(search) ||
                    c.CNIC.Replace("-", "").Contains(search.Replace("-", "")));
            }

            // Filter: City Route
            if (int.TryParse(ddlCityRoute.SelectedValue, out int routeId) && routeId > 0)
            {
                query = query.Where(c =>
                    c.CityRouteName != null && c.CityRouteName == ddlCityRoute.SelectedItem.Text);
            }

            // Filter: Town
            if (int.TryParse(ddlTown.SelectedValue, out int townId) && townId > 0)
            {
                query = query.Where(c => c.TownName != null && c.TownName == ddlTown.SelectedItem.Text);
            }

            // Filter: Party Type
            if (Enum.TryParse(ddlPartyType.SelectedValue, out PartyType partyType))
            {
                query = query.Where(c => c.PartyType == partyType);
            }

            // Filter: Status
            string status = ddlStatus.SelectedValue;
            if (!string.IsNullOrEmpty(status))
            {
                if (status == "Active")
                {
                    query = query.Where(c => c.InActive == false);
                }
                else if (status == "Dead")
                {
                    query = query.Where(c => c.InActive == true);
                }
            }
            var result = query.OrderBy(c => c.CustomerId).ToList();

            gvCustomers.DataSource = result;
            gvCustomers.DataBind();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomers(txtSearch.Text.Trim());
        }

        protected void ddlCityRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTowns(int.TryParse(ddlCityRoute.SelectedValue, out int routeId) ? routeId : (int?)null);
            LoadCustomers(txtSearch.Text.Trim());
        }

        protected void ddlTown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCustomers(txtSearch.Text.Trim());
        }

        protected void ddlPartyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCustomers(txtSearch.Text.Trim());
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCustomers(txtSearch.Text.Trim());
        }

        protected void gvCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void gvCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string id = DataBinder.Eval(e.Row.DataItem, "CustomerID").ToString();
                e.Row.Attributes["onclick"] = $"window.location='/customer/edit?id={id}'";
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
            HiddenField hf = (HiddenField)row.FindControl("hfCustomerId");

            if (int.TryParse(hf.Value, out int customerId))
            {
                string action = ddl.SelectedValue;

                if (action == "Edit")
                {
                    Response.Redirect($"/customer/edit?id={customerId}");
                }
                else if (action == "Delete")
                {
                    var customer = _context.Customers.Find(customerId);
                    if (customer != null)
                    {
                        _context.Customers.Remove(customer);
                        _context.SaveChanges();
                        LoadCustomers(txtSearch.Text.Trim());
                    }
                }
            }
        }

        //Export Helpers

        private List<dynamic> GetFilteredCustomerList(string search = "")
        {
            var query = _context.Customers
                .Select(c => new
                {
                    c.CustomerId,
                    c.Name,
                    c.Email,
                    c.Contact,
                    c.CNIC,
                    c.Address,
                    TownName = c.Town.Name,
                    CityRouteName = c.Town.CityRoute.Name,
                    c.LicenceNo,
                    c.PartyType,
                    c.NtnNo,
                    c.NorcoticsSaleAllowed,
                    c.InActive,
                    c.IsAdvTaxExempted,
                    c.FbrInActiveGST,
                    c.FBRInActiveTax236H,
                    c.CreatedAt
                });

            // Apply search
            if (!string.IsNullOrWhiteSpace(search))
            {
                string normalizedSearch = search.Replace("-", "");
                query = query.Where(c =>
                    c.Name.Contains(search) ||
                    c.CustomerId.ToString().Contains(search) ||
                    c.NtnNo.Contains(search) ||
                    c.CNIC.Replace("-", "").Contains(normalizedSearch));
            }

            // CityRoute Filter
            if (int.TryParse(ddlCityRoute.SelectedValue, out int routeId) && routeId > 0)
            {
                query = query.Where(c => c.CityRouteName == ddlCityRoute.SelectedItem.Text);
            }

            // Town Filter
            if (int.TryParse(ddlTown.SelectedValue, out int townId) && townId > 0)
            {
                query = query.Where(c => c.TownName == ddlTown.SelectedItem.Text);
            }

            // PartyType Filter
            if (Enum.TryParse(ddlPartyType.SelectedValue, out PartyType partyType))
            {
                query = query.Where(c => c.PartyType == partyType);
            }

            // Status Filter
            string status = ddlStatus.SelectedValue;
            if (!string.IsNullOrEmpty(status))
            {
                if (status == "Active")
                {
                    query = query.Where(c => c.InActive == false);
                }
                else if (status == "Dead")
                {
                    query = query.Where(c => c.InActive == true);
                }
            }

            // Materialize the result from DB
            var result = query.OrderBy(c => c.CustomerId).ToList();

            // Now convert to export-ready dynamic object
            return result
                .Select(c => new
                {
                    c.CustomerId,
                    c.Name,
                    c.Email,
                    c.Contact,
                    c.CNIC,
                    c.Address,
                    c.CityRouteName,
                    c.TownName,
                    c.LicenceNo,
                    PartyType = c.PartyType.ToString(),
                    c.NtnNo,
                    NorcoticsSaleAllowed = c.NorcoticsSaleAllowed ? "Yes" : "No",
                    InActive = c.InActive ? "Yes" : "No",
                    IsAdvTaxExempted = c.IsAdvTaxExempted ? "Yes" : "No",
                    FbrInActiveGST = c.FbrInActiveGST ? "Yes" : "No",
                    FBRInActiveTax236H = c.FBRInActiveTax236H ? "Yes" : "No",
                    CreatedAt = c.CreatedAt.ToString("yyyy-MM-dd")
                })
                .Cast<dynamic>()
                .ToList();
        }

        private string EscapeCsv(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "\"\"";
            value = value.Replace("\"", "\"\"");
            return $"\"{value}\"";
        }

        //Excel Export

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            var customers = GetFilteredCustomerList(txtSearch.Text.Trim());

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Code,Name,Email,Contact,CNIC,Address,Route,Town,Licence No,Party Type,NTN,Norcotics Allowed,Inactive,AdvTax Exempt,GST Inactive,Tax236H Inactive,Created At");

            foreach (var c in customers)
            {
                sb.AppendLine(string.Join(",", new string[] {
                    c.CustomerId.ToString("D4"),
                    EscapeCsv(c.Name),
                    EscapeCsv(c.Email),
                    EscapeCsv(c.Contact),
                    EscapeCsv(c.CNIC),
                    EscapeCsv(c.Address),
                    EscapeCsv(c.CityRouteName),
                    EscapeCsv(c.TownName),
                    EscapeCsv(c.LicenceNo),
                    c.PartyType.ToString(),
                    EscapeCsv(c.NtnNo),
                    c.NorcoticsSaleAllowed,
                    c.InActive,
                    c.IsAdvTaxExempted,
                    c.FbrInActiveGST,
                    c.FBRInActiveTax236H,
                    "=\"" + c.CreatedAt + "\""
                }));
            }

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", $"attachment;filename=Customer_Report_{DateTime.Now:yyyyMMdd}.csv");
            Response.ContentType = "text/csv";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        //PDF Export
        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();
            var customers = GetFilteredCustomerList(search);

            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 20f, 10f);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                var titleFont = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD);
                var headerFont = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD);
                var bodyFont = FontFactory.GetFont("Arial", 8);

                pdfDoc.Add(new Paragraph("Data Pharma - Customer Report", titleFont));
                pdfDoc.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), bodyFont));
                pdfDoc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(16)
                {
                    WidthPercentage = 100,
                    HeaderRows = 1
                };

                string[] headers = {
                    "Code", "Name", "Email", "Contact", "CNIC", "Address", "Route", "Town",
                    "Licence No", "Party Type", "NTN", "Norcotics", "Dead", "AdvTax Ex",
                    "GST Inactive", "236H Inactive"
                };

                foreach (string h in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(h, headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY };
                    table.AddCell(cell);
                }

                foreach (var c in customers)
                {
                    table.AddCell(new Phrase(c.CustomerId.ToString("D4"), bodyFont));
                    table.AddCell(new Phrase(c.Name, bodyFont));
                    table.AddCell(new Phrase(c.Email, bodyFont));
                    table.AddCell(new Phrase(c.Contact, bodyFont));
                    table.AddCell(new Phrase(c.CNIC, bodyFont));
                    table.AddCell(new Phrase(c.Address, bodyFont));
                    table.AddCell(new Phrase(c.CityRouteName, bodyFont));
                    table.AddCell(new Phrase(c.TownName, bodyFont));
                    table.AddCell(new Phrase(c.LicenceNo, bodyFont));
                    table.AddCell(new Phrase(c.PartyType.ToString(), bodyFont));
                    table.AddCell(new Phrase(c.NtnNo, bodyFont));
                    table.AddCell(new Phrase(c.NorcoticsSaleAllowed, bodyFont));
                    table.AddCell(new Phrase(c.InActive, bodyFont));
                    table.AddCell(new Phrase(c.IsAdvTaxExempted, bodyFont));
                    table.AddCell(new Phrase(c.FbrInActiveGST, bodyFont));
                    table.AddCell(new Phrase(c.FBRInActiveTax236H, bodyFont));
                }

                pdfDoc.Add(table);
                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename=Customer_Report_{DateTime.Now:dd_MMMM_yyyy}.pdf");
                Response.BinaryWrite(memoryStream.ToArray());
                Response.End();
            }
        }

        //Import System

        //sample data
        private void ImportInfoControl_DownloadRequested(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=customer_sample.csv");

            Response.Write("Name,Email,Contact,CNIC,Address,TownID,LicenceNo,PartyType,NtnNo,NorcoticsSaleAllowed,InActive,IsAdvTaxExempted,FbrInActiveGST,FBRInActiveTax236H\r\n");
            Response.Write("Customer 1,ali@example.com,03001234567,31234-1234567-1,123 Street,2,LN-1023,Pharmacy,1234567-8,false,yes,no,no,yes\r\n");

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

                    // Required
                    int colName = headers.IndexOf("Name");
                    int colContact = headers.IndexOf("Contact");
                    int colCNIC = headers.IndexOf("CNIC");
                    int colAddress = headers.IndexOf("Address");
                    int colTownID = headers.IndexOf("TownID");
                    int colPartyType = headers.IndexOf("PartyType");
                    int colNtnNo = headers.IndexOf("NtnNo");
                    int colEmail = headers.IndexOf("Email");
                    int colLicenceNo = headers.IndexOf("LicenceNo");
                    int colNorcoticsSaleAllowed = headers.IndexOf("NorcoticsSaleAllowed");
                    int colInActive = headers.IndexOf("InActive");
                    int colIsAdvTaxExempted = headers.IndexOf("IsAdvTaxExempted");
                    int colFbrInActiveGST = headers.IndexOf("FbrInActiveGST");
                    int colFBRInActiveTax236H = headers.IndexOf("FBRInActiveTax236H");

                    if (colName == -1 || colContact == -1 || colCNIC == -1 || colAddress == -1 || colTownID == -1 || colPartyType == -1 || colNtnNo == -1)
                    {
                        lblImportStatus.Text = "Missing required columns.";
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
                            string cnic = SafeGet(fields, colCNIC);
                            string address = SafeGet(fields, colAddress);
                            string rawTownID = SafeGet(fields, colTownID);
                            string rawPartyType = SafeGet(fields, colPartyType);
                            string ntnNo = SafeGet(fields, colNtnNo);

                            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(contact) ||
                                string.IsNullOrWhiteSpace(cnic) || string.IsNullOrWhiteSpace(address) ||
                                string.IsNullOrWhiteSpace(rawTownID) || string.IsNullOrWhiteSpace(rawPartyType) ||
                                string.IsNullOrWhiteSpace(ntnNo))
                                throw new Exception("One or more required fields are missing.");

                            if (!int.TryParse(rawTownID, out int townId) || !_context.Towns.Any(t => t.TownID == townId))
                                throw new Exception("Invalid or non-existing TownID.");

                            PartyType partyType = ParsePartyType(rawPartyType);

                            // Check if customer exists
                            var existing = _context.Customers
                                .FirstOrDefault(c => c.Name == name && c.CNIC == cnic && c.TownID == townId);

                            if (existing != null)
                            {
                                existing.Contact = contact;
                                existing.Address = address;
                                existing.NtnNo = ntnNo;
                                existing.PartyType = partyType;

                                if (colEmail != -1) existing.Email = SafeGet(fields, colEmail);
                                if (colLicenceNo != -1) existing.LicenceNo = SafeGet(fields, colLicenceNo);
                                if (colNorcoticsSaleAllowed != -1) existing.NorcoticsSaleAllowed = ParseBool(SafeGet(fields, colNorcoticsSaleAllowed));
                                if (colInActive != -1) existing.InActive = ParseBool(SafeGet(fields, colInActive));
                                if (colIsAdvTaxExempted != -1) existing.IsAdvTaxExempted = ParseBool(SafeGet(fields, colIsAdvTaxExempted));
                                if (colFbrInActiveGST != -1) existing.FbrInActiveGST = ParseBool(SafeGet(fields, colFbrInActiveGST));
                                if (colFBRInActiveTax236H != -1) existing.FBRInActiveTax236H = ParseBool(SafeGet(fields, colFBRInActiveTax236H));

                                updateCount++;
                            }
                            else
                            {
                                var newCustomer = new Models.Customer
                                {
                                    Name = name,
                                    Contact = contact,
                                    CNIC = cnic,
                                    Address = address,
                                    TownID = townId,
                                    PartyType = partyType,
                                    NtnNo = ntnNo,
                                    Email = colEmail != -1 ? SafeGet(fields, colEmail) : null,
                                    LicenceNo = colLicenceNo != -1 ? SafeGet(fields, colLicenceNo) : null,
                                    NorcoticsSaleAllowed = colNorcoticsSaleAllowed != -1 && ParseBool(SafeGet(fields, colNorcoticsSaleAllowed)),
                                    InActive = colInActive != -1 && ParseBool(SafeGet(fields, colInActive)),
                                    IsAdvTaxExempted = colIsAdvTaxExempted != -1 && ParseBool(SafeGet(fields, colIsAdvTaxExempted)),
                                    FbrInActiveGST = colFbrInActiveGST != -1 && ParseBool(SafeGet(fields, colFbrInActiveGST)),
                                    FBRInActiveTax236H = colFBRInActiveTax236H != -1 && ParseBool(SafeGet(fields, colFBRInActiveTax236H)),
                                    CreatedAt = DateTime.Now
                                };

                                _context.Customers.Add(newCustomer);
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
                        lblImportStatus.Text = $"Import completed: {insertCount} added, {updateCount} updated." +
                            "<br><b>Errors:</b><br>" +
                            string.Join("<br>", errorMessages.Take(10)) +
                            (errorMessages.Count > 10 ? "<br>...and more." : "");
                        lblImportStatus.CssClass = "alert alert-danger d-block"; 
                    }
                    else
                    {
                        lblImportStatus.Text = $"Import completed: {insertCount} added, {updateCount} updated.";
                        lblImportStatus.CssClass = "alert alert-success mt-3 d-block";
                    }

                    LoadCustomers(txtSearch.Text.Trim());
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

        private bool ParseBool(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            input = input.Trim().ToLower();
            return input == "yes" || input == "true" || input == "1";
        }

        private PartyType ParsePartyType(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("PartyType value is required.");

            // Normalize: lowercase, remove spaces, slashes, hyphens
            string cleaned = Regex.Replace(input, @"[\s\-_\/]", "", RegexOptions.Compiled).ToLowerInvariant();

            foreach (PartyType type in Enum.GetValues(typeof(PartyType)))
            {
                string normalized = Regex.Replace(type.ToString(), @"[\s\-_\/]", "", RegexOptions.Compiled).ToLowerInvariant();
                if (normalized == cleaned)
                    return type;
            }

            var validOptions = string.Join(", ", Enum.GetNames(typeof(PartyType)));
            throw new ArgumentException($"Invalid PartyType: '{input}'. Valid values are: {validOptions}");
        }
    }
}