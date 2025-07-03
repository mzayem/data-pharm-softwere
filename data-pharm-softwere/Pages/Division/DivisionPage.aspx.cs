using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Division
{
    public partial class DivisionPage : System.Web.UI.Page
    {
        private DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDivisions();
            }
        }

        private void LoadDivisions(string search = "")
        {
            if (!_context.Divisions.Any())
            {
                Response.Redirect("/division/create");
                return;
            }
            var query = _context.Divisions.Select(g => new
            {
                g.DivisionID,
                g.Name,
                VendorName = g.Vendor.Name,
                g.CreatedAt
            });

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(g =>
                    g.Name.Contains(search) ||
                    g.VendorName.Contains(search)
                );
            }

            gvDivisions.DataSource = query.OrderByDescending(g => g.CreatedAt).ToList();
            gvDivisions.DataBind();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();
            ViewState["DivisionSearch"] = search;
            LoadDivisions(search);
        }

        protected void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            HiddenField hf = (HiddenField)row.FindControl("hfDivisionID");

            int divisionId = int.Parse(hf.Value);
            string action = ddl.SelectedValue;

            if (action == "Edit")
            {
                Response.Redirect($"/division/edit?id={divisionId}");
            }
            else if (action == "Delete")
            {
                var division = _context.Divisions.Find(divisionId);
                if (division != null)
                {
                    _context.Divisions.Remove(division);
                    _context.SaveChanges();
                    LoadDivisions(txtSearch.Text.Trim());
                }
            }
        }

        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            string search = ViewState["DivisionSearch"]?.ToString() ?? "";
            var divisions = _context.Divisions
                .Where(g => string.IsNullOrEmpty(search) || g.Name.Contains(search) || g.Vendor.Name.Contains(search))
                .Select(g => new
                {
                    g.DivisionID,
                    g.Name,
                    VendorName = g.Vendor.Name,
                    g.CreatedAt
                })
                .OrderBy(g => g.Name)
                .ToList();

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 10f);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                pdfDoc.Add(new Paragraph("Division Report", FontFactory.GetFont("Arial", 14, Font.BOLD)));
                pdfDoc.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                pdfDoc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(4) { WidthPercentage = 100 };
                string[] headers = { "ID", "Division Name", "Vendor", "Created At" };

                foreach (string header in headers)
                {
                    var cell = new PdfPCell(new Phrase(header, FontFactory.GetFont("Arial", 9, Font.BOLD)))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY
                    };
                    table.AddCell(cell);
                }

                foreach (var g in divisions)
                {
                    table.AddCell(g.DivisionID.ToString());
                    table.AddCell(g.Name);
                    table.AddCell(g.VendorName);
                    table.AddCell(g.CreatedAt.ToString("yyyy-MM-dd"));
                }

                pdfDoc.Add(table);
                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename=Division_Report_{DateTime.Now:yyyyMMddHHmmss}.pdf");
                Response.BinaryWrite(memoryStream.ToArray());
                Response.End();
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string search = ViewState["DivisionSearch"]?.ToString() ?? "";
            var divisions = _context.Divisions
                .Where(g => string.IsNullOrEmpty(search) || g.Name.Contains(search) || g.Vendor.Name.Contains(search))
                .Select(g => new
                {
                    g.DivisionID,
                    g.Name,
                    VendorName = g.Vendor.Name,
                    g.CreatedAt
                })
                .OrderBy(g => g.Name)
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("DivisionID,Division Name,Vendor,Created At");

            foreach (var g in divisions)
            {
                sb.AppendLine($"{g.DivisionID},{EscapeCsv(g.Name)},{EscapeCsv(g.VendorName)},{g.CreatedAt:yyyy-MM-dd}");
            }

            Response.Clear();
            Response.AddHeader("content-disposition", $"attachment;filename=Division_Report_{DateTime.Now:yyyyMMddHHmmss}.csv");
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

        protected void gvDivisions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string divisionId = DataBinder.Eval(e.Row.DataItem, "DivisionID").ToString();
                e.Row.Attributes["onclick"] = $"window.location='/division/edit?id={divisionId}'";
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

        protected void gvDivisions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Optional: Not used because dropdown handles actions
        }
    }
}