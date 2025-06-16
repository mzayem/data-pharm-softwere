using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
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

        private void LoadGroups(string search = "")
        {
            var query = _context.Groups.Select(g => new
            {
                g.GroupID,
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
                .Where(g => string.IsNullOrEmpty(search) || g.Name.Contains(search) || g.Vendor.Name.Contains(search))
                .Select(g => new
                {
                    g.GroupID,
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

                pdfDoc.Add(new Paragraph("Group Report", FontFactory.GetFont("Arial", 14, Font.BOLD)));
                pdfDoc.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                pdfDoc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(4) { WidthPercentage = 100 };
                string[] headers = { "ID", "Group Name", "Vendor", "Created At" };

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
                    table.AddCell(g.VendorName);
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
                .Where(g => string.IsNullOrEmpty(search) || g.Name.Contains(search) || g.Vendor.Name.Contains(search))
                .Select(g => new
                {
                    g.GroupID,
                    g.Name,
                    VendorName = g.Vendor.Name,
                    g.CreatedAt
                })
                .OrderBy(g => g.Name)
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("GroupID,Group Name,Vendor,Created At");

            foreach (var g in groups)
            {
                sb.AppendLine($"{g.GroupID},{EscapeCsv(g.Name)},{EscapeCsv(g.VendorName)},{g.CreatedAt:yyyy-MM-dd}");
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
    }
}
