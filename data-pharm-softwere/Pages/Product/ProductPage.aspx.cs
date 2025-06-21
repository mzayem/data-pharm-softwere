using data_pharm_softwere.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebListItem = System.Web.UI.WebControls.ListItem;
using PdfListItem = iTextSharp.text.ListItem;

namespace data_pharm_softwere.Pages.Product
{
    public partial class ProductPage : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadVendors();
                LoadGroups();
                LoadSubGroups();
                LoadProducts();
            }
        }

        private void LoadProducts(string search = "")
        {
            var query = _context.Products
                .Select(p => new
                {
                    p.ProductID,
                    p.Name,
                    p.ProductCode,
                    p.Type,
                    p.PackingType,
                    SubGroupID = p.SubGroup.SubGroupID,
                    GroupID = p.SubGroup.Group.GroupID,
                    VendorID = p.SubGroup.Group.Vendor.VendorID,
                    SubGroupName = p.SubGroup.Name,
                    GroupName = p.SubGroup.Group.Name,
                    VendorName = p.SubGroup.Group.Vendor.Name,
                    p.CreatedAt
                });

            // Search text filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.ProductCode.Contains(search) ||
                    p.Type.ToString().Contains(search) ||
                    p.PackingType.ToString().Contains(search) ||
                    p.SubGroupName.Contains(search) ||
                    p.GroupName.Contains(search) ||
                    p.VendorName.Contains(search)
                );
            }

            // Vendor filter
            if (!string.IsNullOrEmpty(ddlVendor.SelectedValue))
            {
                int vendorId = int.Parse(ddlVendor.SelectedValue);
                query = query.Where(p => p.VendorID == vendorId);
            }

            // Group filter
            if (!string.IsNullOrEmpty(ddlGroup.SelectedValue))
            {
                int groupId = int.Parse(ddlGroup.SelectedValue);
                query = query.Where(p => p.GroupID == groupId);
            }

            // SubGroup filter
            if (!string.IsNullOrEmpty(ddlSubGroup.SelectedValue))
            {
                int subGroupId = int.Parse(ddlSubGroup.SelectedValue);
                query = query.Where(p => p.SubGroupID == subGroupId);
            }

            gvProducts.DataSource = query.OrderByDescending(p => p.CreatedAt).ToList();
            gvProducts.DataBind();
        }

        private void LoadVendors()
        {
            var vendors = _context.Vendors
                .OrderBy(v => v.Name)
                .ToList();

            ddlVendor.DataSource = vendors;
            ddlVendor.DataTextField = "Name";
            ddlVendor.DataValueField = "VendorID";
            ddlVendor.DataBind();
            ddlVendor.Items.Insert(0, new WebListItem("All Vendors", ""));
        }

        private void LoadGroups()
        {
            int vendorId;
            var allGroups = _context.Groups.AsQueryable();

            if (!string.IsNullOrEmpty(ddlVendor.SelectedValue) && int.TryParse(ddlVendor.SelectedValue, out vendorId))
            {
                allGroups = allGroups.Where(g => g.VendorID == vendorId);
            }

            var groups = allGroups
                .OrderBy(g => g.Name)
                .ToList();

            ddlGroup.DataSource = groups;
            ddlGroup.DataTextField = "Name";
            ddlGroup.DataValueField = "GroupID";
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, new WebListItem("All Groups", ""));
        }

        private void LoadSubGroups()
        {
            int groupId;
            var allSubGroups = _context.SubGroups.AsQueryable();

            if (!string.IsNullOrEmpty(ddlGroup.SelectedValue) && int.TryParse(ddlGroup.SelectedValue, out groupId))
            {
                allSubGroups = allSubGroups.Where(sg => sg.GroupID == groupId);
            }

            var subGroups = allSubGroups
                .OrderBy(sg => sg.Name)
                .ToList();

            ddlSubGroup.DataSource = subGroups;
            ddlSubGroup.DataTextField = "Name";
            ddlSubGroup.DataValueField = "SubGroupID";
            ddlSubGroup.DataBind();
            ddlSubGroup.Items.Insert(0, new WebListItem("All Sub Groups", ""));
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();
            ViewState["ProductSearch"] = search;
            LoadProducts(search);
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroups(); // optionally reload groups based on vendor
            LoadProducts(txtSearch.Text.Trim());
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSubGroups(); // optionally reload subgroups based on group
            LoadProducts(txtSearch.Text.Trim());
        }

        protected void ddlSubGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts(txtSearch.Text.Trim());
        }

        protected void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            HiddenField hf = (HiddenField)row.FindControl("hfProductID");

            if (int.TryParse(hf.Value, out int productId))
            {
                string action = ddl.SelectedValue;

                if (action == "Edit")
                {
                    Response.Redirect($"/product/edit?id={productId}");
                }
                else if (action == "Delete")
                {
                    var product = _context.Products.Find(productId);
                    if (product != null)
                    {
                        _context.Products.Remove(product);
                        _context.SaveChanges();
                        LoadProducts(txtSearch.Text.Trim());
                    }
                }
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string search = ViewState["ProductSearch"]?.ToString() ?? "";

            var query = _context.Products.AsQueryable();

            // Include navigation properties
            query = query
                .Include("SubGroup.Group.Vendor");

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.ProductCode.Contains(search) ||
                    p.Type.ToString().Contains(search) ||
                    p.PackingType.ToString().Contains(search) ||
                    p.SubGroup.Name.Contains(search) ||
                    p.SubGroup.Group.Name.Contains(search) ||
                    p.SubGroup.Group.Vendor.Name.Contains(search)
                );
            }

            // Vendor filter
            if (!string.IsNullOrEmpty(ddlVendor.SelectedValue))
            {
                int vendorId = int.Parse(ddlVendor.SelectedValue);
                query = query.Where(p => p.SubGroup.Group.Vendor.VendorID == vendorId);
            }

            string vendorText = string.IsNullOrEmpty(ddlVendor.SelectedValue)
                ? "All Vendors"
                : ddlVendor.SelectedItem.Text;

            // Group filter
            if (!string.IsNullOrEmpty(ddlGroup.SelectedValue))
            {
                int groupId = int.Parse(ddlGroup.SelectedValue);
                query = query.Where(p => p.SubGroup.Group.GroupID == groupId);
            }

            // SubGroup filter
            if (!string.IsNullOrEmpty(ddlSubGroup.SelectedValue))
            {
                int subGroupId = int.Parse(ddlSubGroup.SelectedValue);
                query = query.Where(p => p.SubGroupID == subGroupId);
            }

            var products = query.OrderBy(p => p.Name).ToList();

            // Build CSV
            var sb = new System.Text.StringBuilder();

            // Header row
            sb.AppendLine("ProductID,Name,ProductCode,HSCode,PackingType,ProductType,PackingSize,CartonSize,UOM,PurchaseDiscount,ReqGST,UnReqGST,IsAdvTaxExempted,IsGSTExempted,Vendor,Group,SubGroup,CreatedAt,UpdatedAt");

            // Data rows
            foreach (var p in products)
            {
                sb.AppendLine(string.Join(",", new string[]
                {
            p.ProductID.ToString(),
            EscapeCsv(p.Name),
            EscapeCsv(p.ProductCode),
            p.HSCode.ToString(),
            p.PackingType.ToString(),
            p.Type.ToString(),
            EscapeCsv(p.PackingSize),
            EscapeCsv(p.CartonSize),
            EscapeCsv(p.Uom),
            p.PurchaseDiscount.ToString("0.##"),
            p.ReqGST.ToString(),
            p.UnReqGST.ToString(),
            p.IsAdvTaxExempted ? "Yes" : "No",
            p.IsGSTExempted ? "Yes" : "No",
            EscapeCsv(p.SubGroup?.Group?.Vendor?.Name ?? "-"),
            EscapeCsv(p.SubGroup?.Group?.Name ?? "-"),
            EscapeCsv(p.SubGroup?.Name ?? "-"),
            "=\"" + p.CreatedAt.ToString("dd MMMM, yyyy") + "\"",
            "=\"" + (p.UpdatedAt?.ToString("dd MMMM, yyyy") ?? "-") + "\""
                }));
            }

            // Send to client
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", $"attachment;filename={vendorText}_Products_{DateTime.Now:dd_MMMM_yyyy}.csv");
            Response.ContentType = "text/csv";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        private string EscapeCsv(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "\"\"";

            value = value.Replace("\"", "\"\""); // Escape double quotes
            return $"\"{value}\""; // Wrap in quotes
        }

        //pdf export
        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            string search = ViewState["ProductSearch"]?.ToString() ?? "";

            var query = _context.Products
                .Include("SubGroup.Group.Vendor")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.ProductCode.Contains(search) ||
                    p.Type.ToString().Contains(search) ||
                    p.PackingType.ToString().Contains(search) ||
                    p.SubGroup.Name.Contains(search) ||
                    p.SubGroup.Group.Name.Contains(search) ||
                    p.SubGroup.Group.Vendor.Name.Contains(search)
                );
            }

            if (!string.IsNullOrEmpty(ddlVendor.SelectedValue))
            {
                int vendorId = int.Parse(ddlVendor.SelectedValue);
                query = query.Where(p => p.SubGroup.Group.Vendor.VendorID == vendorId);
            }

            if (!string.IsNullOrEmpty(ddlGroup.SelectedValue))
            {
                int groupId = int.Parse(ddlGroup.SelectedValue);
                query = query.Where(p => p.SubGroup.Group.GroupID == groupId);
            }

            if (!string.IsNullOrEmpty(ddlSubGroup.SelectedValue))
            {
                int subGroupId = int.Parse(ddlSubGroup.SelectedValue);
                query = query.Where(p => p.SubGroupID == subGroupId);
            }

            var products = query.OrderBy(p => p.Name).ToList();

            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 20f, 10f);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                var titleFont = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD);
                var headerFont = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD);
                var bodyFont = FontFactory.GetFont("Arial", 8);

                // Title
                pdfDoc.Add(new Paragraph("Data Pharma - Full Product Report", titleFont));
                pdfDoc.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), bodyFont));
                pdfDoc.Add(new Paragraph(" "));

                // Determine filter summary
                string vendorText = string.IsNullOrEmpty(ddlVendor.SelectedValue)
                    ? "All Vendors"
                    : ddlVendor.SelectedItem.Text;

                string groupText = string.IsNullOrEmpty(ddlGroup.SelectedValue)
                    ? "All Groups"
                    : ddlGroup.SelectedItem.Text;

                string subGroupText = string.IsNullOrEmpty(ddlSubGroup.SelectedValue)
                    ? "All Sub Groups"
                    : ddlSubGroup.SelectedItem.Text;

                string filterSummary = $"This report includes data for: {vendorText}, under {groupText}, with {subGroupText}.";

                pdfDoc.Add(new Paragraph(filterSummary, bodyFont));
                pdfDoc.Add(new Paragraph(" "));

                // Table
                PdfPTable table = new PdfPTable(17)
                {
                    WidthPercentage = 100,
                    HeaderRows = 1
                };

                string[] headers = {
                "Code", "Name", "Prod. #", "HS #", "Packing", "Type", "Pack Size",
                "Carton", "UOM", "Pur. Disc", "Req GST", "UnReq GST", "Adv Tax Ex?", "GST Ex?", "Vendor",
                "Group", "SubGrp.",
            };

                foreach (string header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, headerFont))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY
                    };
                    table.AddCell(cell);
                }

                foreach (var p in products)
                {
                    table.AddCell(new Phrase(p.ProductID.ToString(), bodyFont));
                    table.AddCell(new Phrase(p.Name, bodyFont));
                    table.AddCell(new Phrase(p.ProductCode, bodyFont));
                    table.AddCell(new Phrase(p.HSCode.ToString(), bodyFont));
                    table.AddCell(new Phrase(p.PackingType.ToString(), bodyFont));
                    table.AddCell(new Phrase(p.Type.ToString(), bodyFont));
                    table.AddCell(new Phrase(p.PackingSize, bodyFont));
                    table.AddCell(new Phrase(p.CartonSize, bodyFont));
                    table.AddCell(new Phrase(p.Uom, bodyFont));
                    table.AddCell(new Phrase(p.PurchaseDiscount.ToString("0.##") + "%", bodyFont));
                    table.AddCell(new Phrase(p.ReqGST.ToString() + "%", bodyFont));
                    table.AddCell(new Phrase(p.UnReqGST.ToString() + "%", bodyFont));
                    table.AddCell(new Phrase(p.IsAdvTaxExempted ? "Yes" : "No", bodyFont));
                    table.AddCell(new Phrase(p.IsGSTExempted ? "Yes" : "No", bodyFont));
                    table.AddCell(new Phrase(p.SubGroup?.Group?.Vendor?.Name ?? "-", bodyFont));
                    table.AddCell(new Phrase(p.SubGroup?.Group?.Name ?? "-", bodyFont));
                    table.AddCell(new Phrase(p.SubGroup?.Name ?? "-", bodyFont));
                }

                pdfDoc.Add(table);
                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename={vendorText}_Products_Report_{DateTime.Now:dd_MMMM_yyyy}.pdf");
                Response.BinaryWrite(memoryStream.ToArray());
                Response.End();
            }
        }

        protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string id = DataBinder.Eval(e.Row.DataItem, "ProductID").ToString();
                e.Row.Attributes["onclick"] = $"window.location='/product/edit?id={id}'";
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

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
    }
}