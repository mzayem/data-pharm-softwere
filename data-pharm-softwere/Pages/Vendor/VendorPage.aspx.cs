using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using iTextSharp.text;
using iTextSharp.text.pdf;

using data_pharm_softwere.Data;
using data_pharm_softwere.Models;

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
    }
}