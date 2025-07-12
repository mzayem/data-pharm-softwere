using data_pharm_softwere.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.SubGroup
{
    public partial class SubGroupPage : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSubGroups();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ImportInfoControl.DownloadRequested += ImportInfoControl_DownloadRequested;
        }

        private void LoadSubGroups(string search = "")
        {
            if (!_context.SubGroups.Any())
            {
                Response.Redirect("/subgroup/create");
                return;
            }
            var query = _context.SubGroups.Select(sg => new
            {
                sg.SubGroupID,
                sg.Name,
                GroupName = sg.Group.Name,
                VendorName = sg.Group.Division.Vendor.Name,
                sg.CreatedAt
            });

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(sg =>
                    sg.Name.Contains(search) ||
                    sg.GroupName.Contains(search) ||
                    sg.VendorName.Contains(search)
                );
            }

            gvSubGroups.DataSource = query.OrderByDescending(sg => sg.CreatedAt).ToList();
            gvSubGroups.DataBind();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();
            ViewState["SubGroupSearch"] = search;
            LoadSubGroups(search);
        }

        protected void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            HiddenField hf = (HiddenField)row.FindControl("hfSubGroupID");

            int subGroupId = int.Parse(hf.Value);
            string action = ddl.SelectedValue;

            if (action == "Edit")
            {
                Response.Redirect($"/subgroup/edit?id={subGroupId}");
            }
            else if (action == "Delete")
            {
                var subGroup = _context.SubGroups.Find(subGroupId);
                if (subGroup != null)
                {
                    _context.SubGroups.Remove(subGroup);
                    _context.SaveChanges();
                    LoadSubGroups(txtSearch.Text.Trim());
                }
            }
        }

        protected void gvSubGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string id = DataBinder.Eval(e.Row.DataItem, "SubGroupID").ToString();
                e.Row.Attributes["onclick"] = $"window.location='/subgroup/edit?id={id}'";
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

        protected void gvSubGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Not used because dropdown handles commands
        }

        //Import System
        //Sample file
        private void ImportInfoControl_DownloadRequested(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=subgroup_sample.csv");

            Response.Write("Name,GroupID\r\n");
            Response.Write("SubGroup A,1\r\n");
            Response.Write("SubGroup B,2\r\n");

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
                    int colGroupID = headers.IndexOf("GroupID");

                    if (colName == -1 || colGroupID == -1)
                    {
                        lblImportStatus.Text = "Missing required columns in CSV.";
                        lblImportStatus.CssClass = "alert alert-danger d-block";
                        return;
                    }

                    int insertCount = 0;
                    int skipCount = 0;
                    int lineNo = 1;
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
                            string rawGroupID = values[colGroupID].Trim();

                            if (string.IsNullOrWhiteSpace(rawName))
                                throw new Exception("SubGroup Name is required.");

                            if (!int.TryParse(rawGroupID, out int groupId))
                                throw new Exception($"Invalid GroupID '{rawGroupID}'.");

                            if (!_context.Groups.Any(g => g.GroupID == groupId))
                                throw new Exception($"GroupID '{groupId}' not found in database.");

                            var existing = _context.SubGroups
                                .FirstOrDefault(sg => sg.Name == rawName && sg.GroupID == groupId);

                            if (existing != null)
                            {
                                // Already exists – skip or update
                                skipCount++;
                                continue;
                            }

                            var subGroup = new Models.SubGroup
                            {
                                Name = rawName,
                                GroupID = groupId,
                                CreatedAt = DateTime.Now
                            };

                            _context.SubGroups.Add(subGroup);
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

                    LoadSubGroups(txtSearch.Text.Trim());
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