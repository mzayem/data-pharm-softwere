using data_pharm_softwere.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Vendor
{
    public partial class VendorPage : System.Web.UI.Page
    {
        private DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadVendors();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ImportInfoControl.DownloadRequested += ImportInfoControl_DownloadRequested;
        }

        private void LoadVendors(string search = "")
        {
            var query = _context.Vendors.AsQueryable();

            if (!_context.Vendors.Any())
            {
                Response.Redirect("/vendor/create");
                return;
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(v =>
                    v.VendorID.ToString().Contains(search) ||
                    v.Name.Contains(search) ||
                    v.City.Contains(search) ||
                    v.CompanyCode.Contains(search) ||
                    v.Email.Contains(search) ||
                    v.Contact.Contains(search) ||
                    v.GstNo.Contains(search) ||
                    v.SRACode.Contains(search)
                );
            }

            gvVendors.DataSource = query.OrderByDescending(v => v.CreatedAt).ToList();
            gvVendors.DataBind();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();
            ViewState["VendorSearch"] = search;
            LoadVendors(search);
        }

        protected void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            HiddenField hf = (HiddenField)row.FindControl("hfVendorID");

            int vendorId = int.Parse(hf.Value);
            string action = ddl.SelectedValue;

            if (action == "Edit")
            {
                Response.Redirect($"/vendor/edit?id={vendorId}");
            }
            else if (action == "Delete")
            {
                var vendor = _context.Vendors.Find(vendorId);
                if (vendor != null)
                {
                    _context.Vendors.Remove(vendor);
                    _context.SaveChanges();
                    LoadVendors(txtSearch.Text.Trim());
                }
            }
        }

        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            string search = ViewState["VendorSearch"]?.ToString() ?? "";
            var query = _context.Vendors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(v =>
                    v.VendorID.ToString().Contains(search) ||
                    v.Name.Contains(search) ||
                    v.City.Contains(search) ||
                    v.CompanyCode.Contains(search) ||
                    v.Email.Contains(search) ||
                    v.Contact.Contains(search) ||
                    v.GstNo.Contains(search) ||
                    v.SRACode.Contains(search)
                );
            }

            var vendors = query.OrderBy(v => v.Name).ToList();

            // Use landscape orientation for more width
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 20f, 10f);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                // Fonts
                iTextSharp.text.Font titleFont = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font headerFont = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font bodyFont = FontFactory.GetFont("Arial", 8);

                // Title
                pdfDoc.Add(new Paragraph("Data Pharma - Full Vendor Report", titleFont));
                pdfDoc.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), bodyFont));
                pdfDoc.Add(new Paragraph(" "));

                // Table with all columns
                PdfPTable table = new PdfPTable(16)
                {
                    WidthPercentage = 100,
                    HeaderRows = 1
                };

                string[] headers = {
            "ID", "Name", "Email", "Contact", "Address", "Town", "City", "License No",
            "Expiry", "SRA Code", "GST No", "NTN No", "Company Code", "Max Discount", "Remarks", "Created"
                };

                foreach (string header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, headerFont));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                }

                foreach (var v in vendors)
                {
                    table.AddCell(new Phrase(v.VendorID.ToString(), bodyFont));
                    table.AddCell(new Phrase(v.Name, bodyFont));
                    table.AddCell(new Phrase(v.Email ?? "-", bodyFont));
                    table.AddCell(new Phrase(v.Contact ?? "-", bodyFont));
                    table.AddCell(new Phrase(v.Address, bodyFont));
                    table.AddCell(new Phrase(v.Town, bodyFont));
                    table.AddCell(new Phrase(v.City, bodyFont));
                    table.AddCell(new Phrase(v.LicenceNo ?? "-", bodyFont));
                    table.AddCell(new Phrase(v.ExpiryDate.ToString("yyyy-MM-dd"), bodyFont));
                    table.AddCell(new Phrase(v.SRACode, bodyFont));
                    table.AddCell(new Phrase(v.GstNo, bodyFont));
                    table.AddCell(new Phrase(v.NtnNo, bodyFont));
                    table.AddCell(new Phrase(v.CompanyCode, bodyFont));
                    table.AddCell(new Phrase($"{v.MaxDiscountAllowed}%", bodyFont));
                    table.AddCell(new Phrase(v.Remarks ?? "-", bodyFont));
                    table.AddCell(new Phrase(v.CreatedAt.ToString("yyyy-MM-dd"), bodyFont));
                }

                pdfDoc.Add(table);
                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename=Vendor_Report_{DateTime.Now:yyyyMMddHHmmss}.pdf");
                Response.BinaryWrite(memoryStream.ToArray());
                Response.End();
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string search = ViewState["VendorSearch"]?.ToString() ?? "";
            var query = _context.Vendors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(v =>
                    v.VendorID.ToString().Contains(search) ||
                    v.Name.Contains(search) ||
                    v.City.Contains(search) ||
                    v.CompanyCode.Contains(search) ||
                    v.Email.Contains(search) ||
                    v.Contact.Contains(search) ||
                    v.GstNo.Contains(search) ||
                    v.SRACode.Contains(search)
                );
            }

            var vendors = query.OrderBy(v => v.Name).ToList();

            // Build CSV content
            var sb = new System.Text.StringBuilder();

            // Header
            sb.AppendLine("VendorID,Name,Email,Contact,Address,Town,City,LicenceNo,Expiry,SRACode,GstNo,NtnNo,CompanyCode,MaxDiscount,Remarks,Created");

            // Rows
            foreach (var v in vendors)
            {
                sb.AppendLine(string.Join(",",
                    v.VendorID,
                    EscapeCsv(v.Name),
                    EscapeCsv(v.Email ?? "-"),
                    EscapeCsv(v.Contact ?? "-"),
                    EscapeCsv(v.Address),
                    EscapeCsv(v.Town),
                    EscapeCsv(v.City),
                    EscapeCsv(v.LicenceNo ?? "-"),
                    v.ExpiryDate.ToString("yyyy-MM-dd"),
                    EscapeCsv(v.SRACode),
                    EscapeCsv(v.GstNo),
                    EscapeCsv(v.NtnNo),
                    EscapeCsv(v.CompanyCode),
                    $"{v.MaxDiscountAllowed}%",
                    EscapeCsv(v.Remarks ?? "-"),
                    v.CreatedAt.ToString("yyyy-MM-dd")
                ));
            }

            // Export as CSV
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", $"attachment;filename=Vendor_Report_{DateTime.Now:yyyyMMddHHmmss}.csv");
            Response.ContentType = "text/csv";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Charset = "";

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        // Helper method to escape commas and quotes in CSV
        private string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "\"\"";

            value = value.Replace("\"", "\"\""); // escape quotes
            return $"\"{value}\"";
        }

        protected void gvVendors_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Get the vendor ID from the data item
                string vendorId = DataBinder.Eval(e.Row.DataItem, "VendorID").ToString();

                // Add the onclick event to the row
                e.Row.Attributes["onclick"] = $"window.location='/vendor/edit?id={vendorId}'";
                e.Row.Style["cursor"] = "pointer";

                // Prevent dropdown click from triggering the row click
                foreach (TableCell cell in e.Row.Cells)
                {
                    foreach (Control control in cell.Controls)
                    {
                        if (control is DropDownList ddl)
                        {
                            ddl.Attributes["onclick"] = "event.stopPropagation();";
                        }
                    }
                }
            }
        }

        protected void gvVendors_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Not used in this case — dropdown handles action.
        }

        //Import functions

        //Sample file
        private void ImportInfoControl_DownloadRequested(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=vendor_sample.csv");

            Response.Write("Name,Email,Contact,Address,Town,City,LicenceNo,ExpiryDate,SRACode,GstNo,NtnNo,CompanyCode,MaxDiscountAllowed,Remarks\r\n");
            Response.Write("ABC Pharma,abc@email.com,03001234567,Main Street,Town1,City1,LIC123,2025-12-31,SRA001,GST123,NTN789,CODE123,15,Trusted vendor\r\n");

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
                    if (headerLine == null)
                    {
                        lblImportStatus.Text = "CSV file is empty.";
                        lblImportStatus.CssClass = "alert alert-danger d-block";
                        return;
                    }

                    var headers = headerLine.Split(',').Select(h => h.Trim()).ToList();

                    int colName = headers.IndexOf("Name");
                    int colCompanyCode = headers.IndexOf("CompanyCode");
                    int colEmail = headers.IndexOf("Email");
                    int colContact = headers.IndexOf("Contact");
                    int colAddress = headers.IndexOf("Address");
                    int colTown = headers.IndexOf("Town");
                    int colCity = headers.IndexOf("City");
                    int colLicenceNo = headers.IndexOf("LicenceNo");
                    int colExpiry = headers.IndexOf("ExpiryDate");
                    int colSraCode = headers.IndexOf("SRACode");
                    int colGstNo = headers.IndexOf("GstNo");
                    int colNtnNo = headers.IndexOf("NtnNo");
                    int colMaxDiscount = headers.IndexOf("MaxDiscountAllowed");
                    int colRemarks = headers.IndexOf("Remarks");

                    if (colName == -1 || colCompanyCode == -1)
                    {
                        lblImportStatus.Text = "Missing required columns: 'Name' and 'CompanyCode'.";
                        lblImportStatus.CssClass = "alert alert-danger d-block";
                        return;
                    }

                    int insertCount = 0;
                    int updateCount = 0;
                    var errorMessages = new List<string>();
                    int lineNo = 1;

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        lineNo++;
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        try
                        {
                            var fields = line.Split(',');

                            string rawName = fields[colName].Trim();
                            string rawCompanyCode = fields[colCompanyCode].Trim();

                            if (string.IsNullOrWhiteSpace(rawName))
                                throw new Exception("Name is required.");
                            if (string.IsNullOrWhiteSpace(rawCompanyCode))
                                throw new Exception("CompanyCode is required.");

                            var existingVendor = _context.Vendors
                                .FirstOrDefault(v => v.Name == rawName && v.CompanyCode == rawCompanyCode);

                            if (existingVendor != null)
                            {
                                // Only update provided columns
                                if (colEmail != -1) existingVendor.Email = SafeGet(fields, colEmail);
                                if (colContact != -1) existingVendor.Contact = SafeGet(fields, colContact);
                                if (colAddress != -1) existingVendor.Address = SafeGet(fields, colAddress);
                                if (colTown != -1) existingVendor.Town = SafeGet(fields, colTown);
                                if (colCity != -1) existingVendor.City = SafeGet(fields, colCity);
                                if (colLicenceNo != -1) existingVendor.LicenceNo = SafeGet(fields, colLicenceNo);
                                if (colExpiry != -1 && DateTime.TryParse(SafeGet(fields, colExpiry), out DateTime expiry))
                                    existingVendor.ExpiryDate = expiry;
                                if (colSraCode != -1) existingVendor.SRACode = SafeGet(fields, colSraCode);
                                if (colGstNo != -1) existingVendor.GstNo = SafeGet(fields, colGstNo);
                                if (colNtnNo != -1) existingVendor.NtnNo = SafeGet(fields, colNtnNo);
                                if (colMaxDiscount != -1 && decimal.TryParse(SafeGet(fields, colMaxDiscount), out decimal maxDisc))
                                    existingVendor.MaxDiscountAllowed = maxDisc;
                                if (colRemarks != -1) existingVendor.Remarks = SafeGet(fields, colRemarks);

                                updateCount++;
                                continue;
                            }

                            var newVendor = new Models.Vendor
                            {
                                Name = rawName,
                                CompanyCode = rawCompanyCode,
                                Email = SafeGet(fields, colEmail),
                                Contact = SafeGet(fields, colContact),
                                Address = SafeGet(fields, colAddress),
                                Town = SafeGet(fields, colTown),
                                City = SafeGet(fields, colCity),
                                LicenceNo = SafeGet(fields, colLicenceNo),
                                ExpiryDate = (colExpiry != -1 && DateTime.TryParse(SafeGet(fields, colExpiry), out DateTime expiryDate))
                            ? expiryDate : DateTime.Now.AddYears(1),
                                SRACode = SafeGet(fields, colSraCode),
                                GstNo = SafeGet(fields, colGstNo),
                                NtnNo = SafeGet(fields, colNtnNo),
                                MaxDiscountAllowed = (colMaxDiscount != -1 && decimal.TryParse(SafeGet(fields, colMaxDiscount), out decimal disc))
                            ? disc : 0,
                                Remarks = SafeGet(fields, colRemarks),
                                CreatedAt = DateTime.Now
                            };

                            _context.Vendors.Add(newVendor);
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

                    LoadVendors(ViewState["VendorSearch"]?.ToString() ?? "");
                }
            }
            catch (Exception ex)
            {
                lblImportStatus.Text = $"Import failed: {ex.Message}";
                lblImportStatus.CssClass = "alert alert-danger d-block";
            }
        }

        private string SafeGet(string[] arr, int index)
        {
            return index >= 0 && index < arr.Length ? arr[index].Trim() : null;
        }
    }
}