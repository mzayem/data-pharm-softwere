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
using System.Web.UI;
using System.Web.UI.WebControls;
using WebListItem = System.Web.UI.WebControls.ListItem;

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

        protected void Page_Init(object sender, EventArgs e)
        {
            ImportInfoControl.DownloadRequested += ImportInfoControl_DownloadRequested;
        }

        private void LoadProducts(string search = "")
        {
            if (!_context.Products.Any())
            {
                Response.Redirect("/product/create");
                return;
            }

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
                    VendorID = p.SubGroup.Group.Division.Vendor.VendorID,
                    SubGroupName = p.SubGroup.Name,
                    GroupName = p.SubGroup.Group.Name,
                    VendorName = p.SubGroup.Group.Division.Vendor.Name,
                    p.CreatedAt
                });

            // Search text filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.ProductID.ToString().Contains(search) ||
                    p.Name.Contains(search) ||
                    p.ProductCode.ToString().Contains(search) ||
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
            var allGroups = _context.Groups.Include("Division").AsQueryable();

            if (!string.IsNullOrEmpty(ddlVendor.SelectedValue) && int.TryParse(ddlVendor.SelectedValue, out vendorId))
            {
                allGroups = allGroups.Where(g => g.Division.VendorID == vendorId);
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
                    p.ProductID.ToString().Contains(search) ||
                    p.Name.Contains(search) ||
                    p.ProductCode.ToString().Contains(search) ||
                    p.Type.ToString().Contains(search) ||
                    p.PackingType.ToString().Contains(search) ||
                    p.SubGroup.Name.Contains(search) ||
                    p.SubGroup.Group.Name.Contains(search) ||
                    p.SubGroup.Group.Division.Vendor.Name.Contains(search)
                );
            }

            // Vendor filter
            if (!string.IsNullOrEmpty(ddlVendor.SelectedValue))
            {
                int vendorId = int.Parse(ddlVendor.SelectedValue);
                query = query.Where(p => p.SubGroup.Group.Division.Vendor.VendorID == vendorId);
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
            p.ProductCode.ToString(),
            p.HSCode.ToString(),
            p.PackingType.ToString(),
            p.Type.ToString(),
            EscapeCsv(p.PackingSize),
            EscapeCsv(p.CartonSize.ToString()),
            EscapeCsv(p.Uom),
            p.PurchaseDiscount.ToString("0.##"),
            p.ReqGST.ToString(),
            p.UnReqGST.ToString(),
            p.IsAdvTaxExempted ? "Yes" : "No",
            p.IsGSTExempted ? "Yes" : "No",
            EscapeCsv(p.SubGroup?.Group?.Division?.Vendor?.Name ?? "-"),
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
                    p.ProductID.ToString().Contains(search) ||
                    p.Name.Contains(search) ||
                    p.ProductCode.ToString().Contains(search) ||
                    p.Type.ToString().Contains(search) ||
                    p.PackingType.ToString().Contains(search) ||
                    p.SubGroup.Name.Contains(search) ||
                    p.SubGroup.Group.Name.Contains(search) ||
                    p.SubGroup.Group.Division.Vendor.Name.Contains(search)
                );
            }

            if (!string.IsNullOrEmpty(ddlVendor.SelectedValue))
            {
                int vendorId = int.Parse(ddlVendor.SelectedValue);
                query = query.Where(p => p.SubGroup.Group.Division.Vendor.VendorID == vendorId);
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
                    table.AddCell(new Phrase(p.ProductCode.ToString(), bodyFont));
                    table.AddCell(new Phrase(p.HSCode.ToString(), bodyFont));
                    table.AddCell(new Phrase(p.PackingType.ToString(), bodyFont));
                    table.AddCell(new Phrase(p.Type.ToString(), bodyFont));
                    table.AddCell(new Phrase(p.PackingSize, bodyFont));
                    table.AddCell(new Phrase(p.CartonSize.ToString(), bodyFont));
                    table.AddCell(new Phrase(p.Uom, bodyFont));
                    table.AddCell(new Phrase(p.PurchaseDiscount.ToString("0.##") + "%", bodyFont));
                    table.AddCell(new Phrase(p.ReqGST.ToString() + "%", bodyFont));
                    table.AddCell(new Phrase(p.UnReqGST.ToString() + "%", bodyFont));
                    table.AddCell(new Phrase(p.IsAdvTaxExempted ? "Yes" : "No", bodyFont));
                    table.AddCell(new Phrase(p.IsGSTExempted ? "Yes" : "No", bodyFont));
                    table.AddCell(new Phrase(p.SubGroup?.Group?.Division?.Vendor?.Name ?? "-", bodyFont));
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
                        {
                            ddl.Attributes["onclick"] = "event.stopPropagation();";
                            ddl.Attributes["onmousedown"] = "event.stopPropagation();";
                            ddl.Attributes["onmouseup"] = "event.stopPropagation();";
                        }
                    }
                }
            }
        }

        //Import System

        //sample data
        private void ImportInfoControl_DownloadRequested(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=product_sample.csv");

            Response.Write("Name,SubGroupID,ProductCode,HSCode,PackingType,ProductType,PackingSize,CartonSize,UOM,PurchaseDiscount,ReqGST,UnReqGST,IsAdvTaxExempted,IsGSTExempted\r\n");
            Response.Write("Aspirin,2,1234,5678,Syrup,Medicine,500mg,20,Box,5,17,0,Yes,No\r\n");

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

                    // Required fields
                    int colName = headers.IndexOf("Name");
                    int colSubGroupID = headers.IndexOf("SubGroupID");
                    int colProductCode = headers.IndexOf("ProductCode");
                    int colHSCode = headers.IndexOf("HSCode");
                    int colPackingType = headers.IndexOf("PackingType");
                    int colProductType = headers.IndexOf("ProductType");
                    int colPackingSize = headers.IndexOf("PackingSize");
                    int colCartonSize = headers.IndexOf("CartonSize");
                    int colUOM = headers.IndexOf("UOM");
                    int colPurchaseDiscount = headers.IndexOf("PurchaseDiscount");
                    int colReqGST = headers.IndexOf("ReqGST");
                    int colUnReqGST = headers.IndexOf("UnReqGST");
                    int colIsAdvTaxExempted = headers.IndexOf("IsAdvTaxExempted");
                    int colIsGSTExempted = headers.IndexOf("IsGSTExempted");

                    if (colName == -1 || colSubGroupID == -1)
                    {
                        lblImportStatus.Text = "Missing required columns: Name and SubGroupID.";
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

                        var fields = line.Split(',');

                        try
                        {
                            string rawName = SafeGet(fields, colName);
                            string rawSubGroupID = SafeGet(fields, colSubGroupID);

                            if (string.IsNullOrWhiteSpace(rawName))
                                throw new Exception("Name is required.");

                            if (!int.TryParse(rawSubGroupID, out int subGroupId))
                                throw new Exception("SubGroupID is invalid.");

                            if (!_context.SubGroups.Any(sg => sg.SubGroupID == subGroupId))
                                throw new Exception($"SubGroupID '{subGroupId}' does not exist.");

                            var existing = _context.Products
                                .FirstOrDefault(p => p.Name == rawName && p.SubGroupID == subGroupId);

                            if (existing != null)
                            {
                                if (colProductCode != -1 && long.TryParse(SafeGet(fields, colProductCode), out long code)) existing.ProductCode = code;
                                if (colHSCode != -1 && int.TryParse(SafeGet(fields, colHSCode), out int hs)) existing.HSCode = hs;
                                if (colPackingType != -1)
                                    existing.PackingType = (PackingType)ParseProductEnum(SafeGet(fields, colPackingType), typeof(PackingType));
                                if (colProductType != -1)
                                    existing.Type = (ProductType)ParseProductEnum(SafeGet(fields, colProductType), typeof(ProductType));
                                if (colPackingSize != -1) existing.PackingSize = SafeGet(fields, colPackingSize);
                                if (colCartonSize != -1 && int.TryParse(SafeGet(fields, colCartonSize), out int carton)) existing.CartonSize = carton;
                                if (colUOM != -1) existing.Uom = SafeGet(fields, colUOM);
                                if (colPurchaseDiscount != -1 && decimal.TryParse(SafeGet(fields, colPurchaseDiscount), out decimal disc)) existing.PurchaseDiscount = disc;
                                if (colReqGST != -1 && decimal.TryParse(SafeGet(fields, colReqGST), out decimal reqgst)) existing.ReqGST = reqgst;
                                if (colUnReqGST != -1 && decimal.TryParse(SafeGet(fields, colUnReqGST), out decimal unreqgst)) existing.UnReqGST = unreqgst;
                                if (colIsAdvTaxExempted != -1) existing.IsAdvTaxExempted = ParseBool(SafeGet(fields, colIsAdvTaxExempted));
                                if (colIsGSTExempted != -1) existing.IsGSTExempted = ParseBool(SafeGet(fields, colIsGSTExempted));

                                existing.UpdatedAt = DateTime.Now;
                                updateCount++;
                            }
                            else
                            {
                                var newProduct = new Models.Product
                                {
                                    Name = rawName,
                                    SubGroupID = subGroupId,
                                    CreatedAt = DateTime.Now,
                                    ProductCode = (colProductCode != -1 && long.TryParse(SafeGet(fields, colProductCode), out long code)) ? code : 0,
                                    HSCode = (colHSCode != -1 && int.TryParse(SafeGet(fields, colHSCode), out int hs)) ? hs : 0,
                                    PackingType = (colPackingType != -1) ? (PackingType)ParseProductEnum(SafeGet(fields, colPackingType), typeof(PackingType)) : PackingType.Tablet,
                                    Type = (colProductType != -1) ? (ProductType)ParseProductEnum(SafeGet(fields, colProductType), typeof(ProductType)) : ProductType.Medicine,
                                    PackingSize = colPackingSize != -1 ? SafeGet(fields, colPackingSize) : "-",
                                    CartonSize = (colCartonSize != -1 && int.TryParse(SafeGet(fields, colCartonSize), out int carton)) ? carton : 0,
                                    Uom = colUOM != -1 ? SafeGet(fields, colUOM) : "-",
                                    PurchaseDiscount = (colPurchaseDiscount != -1 && decimal.TryParse(SafeGet(fields, colPurchaseDiscount), out decimal disc)) ? disc : 0,
                                    ReqGST = (colReqGST != -1 && decimal.TryParse(SafeGet(fields, colReqGST), out decimal reqgst)) ? reqgst : 0,
                                    UnReqGST = (colUnReqGST != -1 && decimal.TryParse(SafeGet(fields, colUnReqGST), out decimal unreqgst)) ? unreqgst : 0,
                                    IsAdvTaxExempted = (colIsAdvTaxExempted != -1) && ParseBool(SafeGet(fields, colIsAdvTaxExempted)),
                                    IsGSTExempted = (colIsGSTExempted != -1) && ParseBool(SafeGet(fields, colIsGSTExempted))
                                };

                                _context.Products.Add(newProduct);
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

                    LoadProducts(txtSearch.Text.Trim());
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

        private object ParseProductEnum(string input, Type enumType)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Enum value is required.");

            if (enumType != typeof(PackingType) && enumType != typeof(ProductType))
                throw new ArgumentException("Only PackingType or ProductType are supported.");

            // Normalize input: remove spaces, slashes, dashes, underscores
            string cleanedInput = Regex.Replace(input, @"[\s\-_\/]", "", RegexOptions.Compiled).ToLowerInvariant();

            foreach (var value in Enum.GetValues(enumType))
            {
                string normalizedEnum = Regex.Replace(value.ToString(), @"[\s\-_\/]", "", RegexOptions.Compiled).ToLowerInvariant();
                if (cleanedInput == normalizedEnum)
                    return value;
            }

            throw new ArgumentException($"Invalid {enumType.Name}: '{input}'");
        }
    }
}