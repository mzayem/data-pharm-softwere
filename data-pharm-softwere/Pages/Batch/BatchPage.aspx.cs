using data_pharm_softwere.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Batch
{
    public partial class BatchPage : System.Web.UI.Page
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
                LoadBatches();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ImportInfoControl.DownloadRequested += ImportInfoControl_DownloadRequested;
        }

        private void LoadVendors()
        {
            var vendors = _context.Vendors.OrderBy(v => v.Name).ToList();
            ddlVendor.DataSource = vendors;
            ddlVendor.DataTextField = "Name";
            ddlVendor.DataValueField = "VendorID";
            ddlVendor.DataBind();
            ddlVendor.Items.Insert(0, new ListItem("All Vendors", ""));
        }

        private void LoadGroups()
        {
            int vendorId;
            var groups = _context.Groups.Include("Division").AsQueryable();

            if (int.TryParse(ddlVendor.SelectedValue, out vendorId))
            {
                groups = groups.Where(g => g.Division.VendorID == vendorId);
            }

            ddlGroup.DataSource = groups.OrderBy(g => g.Name).ToList();
            ddlGroup.DataTextField = "Name";
            ddlGroup.DataValueField = "GroupID";
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, new ListItem("All Groups", ""));
        }

        private void LoadSubGroups()
        {
            int groupId;
            var subGroups = _context.SubGroups.AsQueryable();

            if (int.TryParse(ddlGroup.SelectedValue, out groupId))
            {
                subGroups = subGroups.Where(sg => sg.GroupID == groupId);
            }

            ddlSubGroup.DataSource = subGroups.OrderBy(sg => sg.Name).ToList();
            ddlSubGroup.DataTextField = "Name";
            ddlSubGroup.DataValueField = "SubGroupID";
            ddlSubGroup.DataBind();
            ddlSubGroup.Items.Insert(0, new ListItem("All SubGroups", ""));
        }

        private void LoadProducts()
        {
            int subGroupId;
            var products = _context.Products.AsQueryable();

            if (int.TryParse(ddlSubGroup.SelectedValue, out subGroupId))
            {
                products = products.Where(p => p.SubGroupID == subGroupId);
            }

            ddlProduct.DataSource = products.OrderBy(p => p.Name).ToList();
            ddlProduct.DataTextField = "Name";
            ddlProduct.DataValueField = "ProductID";
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, new ListItem("All Products", ""));
        }

        private void LoadBatches(string search = "")
        {
            if (!_context.Batches.Any())
            {
                Response.Redirect("/batch/create");
                return;
            }
            var query = _context.Batches
                .Where(b => b.Product != null &&
                            b.Product.SubGroup != null &&
                            b.Product.SubGroup.Group != null &&
                            b.Product.SubGroup.Group.Division != null &&
                            b.Product.SubGroup.Group.Division.Vendor != null)
                .Select(b => new
                {
                    b.BatchID,
                    b.BatchNo,
                    ProductName = b.Product.Name,
                    b.MFGDate,
                    b.ExpiryDate,
                    b.MRP,
                    b.TP,
                    b.DP,
                    ProductID = (int?)b.Product.ProductID,
                    GroupID = (int?)b.Product.SubGroup.Group.GroupID,
                    VendorID = (int?)b.Product.SubGroup.Group.Division.Vendor.VendorID,
                    SubGroupID = (int?)b.Product.SubGroup.SubGroupID
                });

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b =>
                    b.BatchNo.ToString().Contains(search) ||
                    b.ProductName.Contains(search));
            }

            if (int.TryParse(ddlVendor.SelectedValue, out int vendorId))
                query = query.Where(b => b.VendorID == vendorId);

            if (int.TryParse(ddlGroup.SelectedValue, out int groupId))
                query = query.Where(b => b.GroupID == groupId);

            if (int.TryParse(ddlSubGroup.SelectedValue, out int subGroupId))
                query = query.Where(b => b.SubGroupID == subGroupId);

            if (int.TryParse(ddlProduct.SelectedValue, out int productId))
                query = query.Where(b => b.ProductID == productId);

            // Custom sort: first ExpiryDate ascending, then BatchNo numeric ascending
            var sortedList = query
                .ToList() // bring into memory to use natural sort
                .OrderBy(b => b.ExpiryDate)
                .ThenBy(b => b.BatchNo)
                .ToList();

            gvBatches.DataSource = sortedList;
            gvBatches.DataBind();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadBatches(txtSearch.Text.Trim());
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroups();
            LoadSubGroups();
            LoadProductDropdown();
            LoadBatches(txtSearch.Text.Trim());
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSubGroups();
            LoadProducts();
            LoadBatches(txtSearch.Text.Trim());
        }

        protected void ddlSubGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
            LoadBatches(txtSearch.Text.Trim());
        }

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBatches(txtSearch.Text.Trim());
        }

        // Use this for dropdown
        private void LoadProductDropdown()
        {
            int subGroupId;
            var products = _context.Products.AsQueryable();

            if (int.TryParse(ddlSubGroup.SelectedValue, out subGroupId))
            {
                products = products.Where(p => p.SubGroupID == subGroupId);
            }

            ddlProduct.DataSource = products.OrderBy(p => p.Name).ToList();
            ddlProduct.DataTextField = "Name";
            ddlProduct.DataValueField = "ProductID";
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, new ListItem("All Products", ""));
        }

        protected void gvBatches_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Optional: handle edit/delete here
        }

        protected void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            HiddenField hf = (HiddenField)row.FindControl("hfBatchID");

            if (int.TryParse(hf.Value, out int batchId))
            {
                string action = ddl.SelectedValue;

                if (action == "Edit")
                {
                    Response.Redirect($"/batch/edit?id={batchId}");
                }
                else if (action == "Delete")
                {
                    var batch = _context.Batches.Find(batchId);
                    if (batch != null)
                    {
                        _context.Batches.Remove(batch);
                        _context.SaveChanges();
                        LoadBatches();
                    }
                }
            }
        }

        protected void gvBatches_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int batchId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "BatchID"));
                e.Row.Attributes["style"] = "cursor:pointer";
                e.Row.Attributes["onclick"] = $"window.location='/batch/edit?id={batchId}'";

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

        //Import functions

        //Sample file
        private void ImportInfoControl_DownloadRequested(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=batch_sample.csv");

            Response.Write("ProductID,BatchNo,MFGDate,ExpiryDate,DP,TP,MRP,CartonQty\r\n");
            Response.Write("1,1001,2024-01-01,2025-12-31,12.5,14.0,16.5,50\r\n");
            Response.Write("2,1002,2024-02-01,2026-01-01,11.0,13.0,15.0,40\r\n");

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

                    int colProductID = headers.IndexOf("ProductID");
                    int colBatchNo = headers.IndexOf("BatchNo");
                    int colMFGDate = headers.IndexOf("MFGDate");
                    int colExpiryDate = headers.IndexOf("ExpiryDate");
                    int colDP = headers.IndexOf("DP");
                    int colTP = headers.IndexOf("TP");
                    int colMRP = headers.IndexOf("MRP");
                    int colCartonQty = headers.IndexOf("CartonQty");

                    if (colProductID == -1 || colBatchNo == -1 || colMFGDate == -1 || colExpiryDate == -1 ||
                        colDP == -1 || colTP == -1 || colMRP == -1 || colCartonQty == -1)
                    {
                        lblImportStatus.Text = "Missing required columns in CSV.";
                        lblImportStatus.CssClass = "alert alert-danger d-block";
                        return;
                    }

                    int lineNo = 1;
                    int insertCount = 0;
                    int updateCount = 0;
                    var errorMessages = new List<string>();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        lineNo++;

                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var values = line.Split(',');

                        try
                        {
                            string rawProductID = values[colProductID].Trim();
                            string rawBatchNo = values[colBatchNo].Trim();
                            string rawMFG = values[colMFGDate].Trim();
                            string rawEXP = values[colExpiryDate].Trim();
                            string rawDP = values[colDP].Trim();
                            string rawTP = values[colTP].Trim();
                            string rawMRP = values[colMRP].Trim();
                            string rawQty = values[colCartonQty].Trim();

                            if (!int.TryParse(rawProductID, out int productId))
                                throw new Exception($"Invalid ProductID '{rawProductID}'");

                            if (!_context.Products.Any(p => p.ProductID == productId))
                                throw new Exception($"ProductID '{productId}' not found in DB.");

                            if (!int.TryParse(rawBatchNo, out int batchNo))
                                throw new Exception($"Invalid BatchNo '{rawBatchNo}'");

                            if (!DateTime.TryParse(rawMFG, out DateTime mfgDate))
                                throw new Exception($"Invalid MFGDate '{rawMFG}'");

                            if (!DateTime.TryParse(rawEXP, out DateTime expDate))
                                throw new Exception($"Invalid ExpiryDate '{rawEXP}'");

                            if (!decimal.TryParse(rawDP, out decimal dp))
                                throw new Exception($"Invalid DP '{rawDP}'");

                            if (!decimal.TryParse(rawTP, out decimal tp))
                                throw new Exception($"Invalid TP '{rawTP}'");

                            if (!decimal.TryParse(rawMRP, out decimal mrp))
                                throw new Exception($"Invalid MRP '{rawMRP}'");

                            if (!int.TryParse(rawQty, out int qty))
                                throw new Exception($"Invalid CartonQty '{rawQty}'");

                            var existingBatch = _context.Batches.FirstOrDefault(b => b.BatchNo == batchNo);

                            if (existingBatch != null)
                            {
                                // Update existing
                                existingBatch.ProductID = productId;
                                existingBatch.MFGDate = mfgDate;
                                existingBatch.ExpiryDate = expDate;
                                existingBatch.DP = dp;
                                existingBatch.TP = tp;
                                existingBatch.MRP = mrp;
                                existingBatch.CartonQty = qty;
                                existingBatch.CartonPrice = tp * qty;
                                existingBatch.UpdatedBy = string.IsNullOrWhiteSpace(User.Identity?.Name) ? "System" : User.Identity.Name;
                                existingBatch.UpdatedAt = DateTime.Now;

                                updateCount++;
                            }
                            else
                            {
                                var batch = new Models.Batch
                                {
                                    BatchNo = batchNo,
                                    ProductID = productId,
                                    MFGDate = mfgDate,
                                    ExpiryDate = expDate,
                                    DP = dp,
                                    TP = tp,
                                    MRP = mrp,
                                    CartonQty = qty,
                                    CartonPrice = tp * qty,
                                    CreatedBy = string.IsNullOrWhiteSpace(User.Identity?.Name) ? "System" : User.Identity.Name,
                                    CreatedAt = DateTime.Now
                                };

                                _context.Batches.Add(batch);
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

                    LoadBatches();
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