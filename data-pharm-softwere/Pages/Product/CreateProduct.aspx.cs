using data_pharm_softwere.Data;
using data_pharm_softwere.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Product
{
    public partial class CreateProduct : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;
                LoadVendors();
                LoadGroups();
                LoadSubGroups();
                LoadDivisions();
                LoadDropdowns();
                ShowNextProductId();

                txtPurchaseDiscount.Attributes["step"] = "0.1";
                txtPurchaseDiscount.Attributes["min"] = "0";
                txtPurchaseDiscount.Attributes["max"] = "100";

                txtReqGST.Attributes["step"] = "0.1";
                txtReqGST.Attributes["min"] = "0";
                txtReqGST.Attributes["max"] = "100";

                txtUnReqGST.Attributes["step"] = "0.1";
                txtUnReqGST.Attributes["min"] = "0";
                txtUnReqGST.Attributes["max"] = "100";

                txtDist1.Attributes["step"] = "0.1";
                txtDist1.Attributes["min"] = "0";
                txtDist1.Attributes["max"] = "100";
                txtDist1.Text = "0";

                txtDist2.Attributes["step"] = "0.1";
                txtDist2.Attributes["min"] = "0";
                txtDist2.Attributes["max"] = "100";
                txtDist2.Text = "0";
            }
        }

        private void ShowNextProductId()
        {
            try
            {
                var lastProduct = _context.Products
                    .OrderByDescending(p => p.ProductID)
                    .FirstOrDefault();

                int nextProductId = (lastProduct?.ProductID ?? 101000) + 1;
                lblProductId.Text = nextProductId.ToString();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error fetching Product ID: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        private void LoadVendors()
        {
            try
            {
                var vendors = _context.Vendors
               .Include(v => v.Account)
               .OrderBy(v => v.Account.AccountName)
               .Select(v => new
               {
                   v.AccountId,
                   AccountName = v.Account.AccountName
               })
               .ToList();

                ddlVendor.DataSource = vendors;
                ddlVendor.DataTextField = "AccountName";
                ddlVendor.DataValueField = "AccountId";
                ddlVendor.DataBind();
                ddlVendor.Items.Insert(0, new ListItem("-- Select Vendor --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading vendors: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        private void LoadDivisions()
        {
            try
            {
                var divisions = _context.Divisions
                    .OrderBy(d => d.Name)
                    .ToList();
                ddlDivision.DataSource = divisions;
                ddlDivision.DataTextField = "Name";
                ddlDivision.DataValueField = "DivisionID";
                ddlDivision.DataBind();
                ddlDivision.Items.Insert(0, new ListItem("-- Select Division --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading divisions: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        private void LoadGroups(int? vendorId = null)
        {
            try
            {
                var query = _context.Groups.Include(g => g.Division).AsQueryable();
                if (vendorId.HasValue)
                    query = query.Where(g => g.Division.AccountId == vendorId.Value);

                ddlGroup.DataSource = query.ToList();
                ddlGroup.DataTextField = "Name";
                ddlGroup.DataValueField = "GroupID";
                ddlGroup.DataBind();
                ddlGroup.Items.Insert(0, new ListItem("-- Select Group --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading groups: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        private void LoadSubGroups(int? vendorId = null, int? groupId = null)
        {
            try
            {
                var query = _context.SubGroups.Include(sg => sg.Group.Division).AsQueryable();
                if (vendorId.HasValue)
                    query = query.Where(sg => sg.Group.Division.AccountId == vendorId.Value);

                if (groupId.HasValue)
                    query = query.Where(sg => sg.GroupID == groupId.Value);

                ddlSubGroup.DataSource = query.ToList();
                ddlSubGroup.DataTextField = "Name";
                ddlSubGroup.DataValueField = "SubGroupID";
                ddlSubGroup.DataBind();
                ddlSubGroup.Items.Insert(0, new ListItem("-- Select SubGroup --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading subgroups: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger mt-3";
            }
        }

        private void LoadDropdowns()
        {
            ddlType.DataSource = Enum.GetNames(typeof(ProductType));
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem("-- Select Type --", ""));

            ddlPackingType.DataSource = Enum.GetNames(typeof(PackingType));
            ddlPackingType.DataBind();
            ddlPackingType.Items.Insert(0, new ListItem("-- Select PackingType --", ""));

            ddlBonusType.DataSource = Enum.GetNames(typeof(BonusType));
            ddlBonusType.DataBind();

            if (ddlBonusType.Items.FindByText("NoBonus") != null)
            {
                ddlBonusType.SelectedValue = "NoBonus";
            }
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlVendor.SelectedValue, out int vendorId))
            {
                LoadGroups(vendorId);
                LoadSubGroups(vendorId);
            }
            else
            {
                LoadGroups();
                LoadSubGroups();
            }
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            int.TryParse(ddlVendor.SelectedValue, out int vendorId);
            if (int.TryParse(ddlGroup.SelectedValue, out int groupId))
            {
                LoadSubGroups(vendorId > 0 ? (int?)vendorId : null
                    , groupId);
            }
            else
            {
                LoadSubGroups(vendorId > 0 ? (int?)vendorId : null);
            }
        }

        // Temporary storage for bonuses (before product save)
        private List<ProductBonus> Bonuses
        {
            get { return (List<ProductBonus>)Session["Bonuses"] ?? new List<ProductBonus>(); }
            set { Session["Bonuses"] = value; }
        }

        protected void btnAddBonus_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtBonusMinQty.Text, out int minQty) || minQty <= 0)
                {
                    lblBonuses.Text = "Enter valid Min Qty.";
                    lblBonuses.CssClass = "alert alert-warning";
                    return;
                }

                if (!int.TryParse(txtBonusItems.Text, out int bonusItems) || bonusItems < 0)
                {
                    lblBonuses.Text = "Enter valid Bonus Items.";
                    lblBonuses.CssClass = "alert alert-warning";
                    return;
                }

                var bonus = new ProductBonus
                {
                    MinQty = minQty,
                    BonusItems = bonusItems,
                    IsActive = bool.Parse(ddlBonusStatus.SelectedValue),
                    AssignedOn = DateTime.Now
                };

                var list = Bonuses;
                list.Add(bonus);
                Bonuses = list;

                BindBonuses();

                // clear input
                txtBonusMinQty.Text = "";
                txtBonusItems.Text = "";
                ddlBonusStatus.SelectedValue = "true";
            }
            catch (Exception ex)
            {
                lblBonuses.Text = "Error adding bonus: " + ex.Message;
                lblBonuses.CssClass = "alert alert-danger";
            }
        }

        private void BindBonuses()
        {
            rptBonuses.DataSource = Bonuses;
            rptBonuses.DataBind();
        }

        protected void rptBonuses_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                var list = Bonuses;
                int index = Convert.ToInt32(e.CommandArgument);
                if (index >= 0 && index < list.Count)
                {
                    list.RemoveAt(index);
                }
                Bonuses = list;
                BindBonuses();
            }
        }

        protected void rptBonuses_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var bonus = (ProductBonus)e.Item.DataItem;

                var ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");
                if (ddlStatus != null)
                {
                    ddlStatus.SelectedValue = bonus.IsActive.ToString().ToLower();
                }
            }
        }

        protected void BonusFieldChanged(object sender, EventArgs e)
        {
            var list = Bonuses;

            // Find which row triggered the event
            RepeaterItem item = (RepeaterItem)((Control)sender).NamingContainer;
            int index = item.ItemIndex;

            if (index >= 0 && index < list.Count)
            {
                var txtMinQty = (TextBox)item.FindControl("txtMinQty");
                var txtBonusItems = (TextBox)item.FindControl("txtBonusItemsRow");
                var ddlStatus = (DropDownList)item.FindControl("ddlStatus");

                if (int.TryParse(txtMinQty.Text, out int minQty))
                    list[index].MinQty = minQty;

                if (int.TryParse(txtBonusItems.Text, out int bonusItems))
                    list[index].BonusItems = bonusItems;

                list[index].IsActive = ddlStatus.SelectedValue == "true";

                KeepFocus((Control)sender);
            }

            Bonuses = list;
        }

        private void KeepFocus(Control control)
        {
            if (control != null)
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "setFocus",
                    $"setTimeout(function(){{document.getElementById('{control.ClientID}').focus();}}, 0);",
                    true
                );
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Validate PackingType
                    PackingType packingType;
                    if (!Enum.TryParse(ddlPackingType.SelectedValue, out packingType))
                    {
                        lblMessage.Text = "Please select a valid Packing Type.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    // Validate ProductType
                    ProductType productType;
                    if (!Enum.TryParse(ddlType.SelectedValue, out productType))
                    {
                        lblMessage.Text = "Please select a valid Product Type.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    // Validate BonusType
                    BonusType bonusType;
                    if (!Enum.TryParse(ddlBonusType.SelectedValue, out bonusType))
                    {
                        lblMessage.Text = "Please select a valid Bonus Type.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    // Validate IsDiscounted (bool)
                    bool isDiscounted;
                    if (!bool.TryParse(ddlIsDiscounted.SelectedValue, out isDiscounted))
                    {
                        lblMessage.Text = "Please select if product is discounted or not.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    // Validate HSCode
                    if (!int.TryParse(txtHSCode.Text, out int hsCode))
                    {
                        lblMessage.Text = "Invalid HS Code format.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    // Validate CartonSize
                    if (!int.TryParse(txtCartonSize.Text, out int cartonSize))
                    {
                        lblMessage.Text = "Invalid Carton Size format.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    // Validate SubGroup
                    if (!int.TryParse(ddlSubGroup.SelectedValue, out int subGroupId))
                    {
                        lblMessage.Text = "Please select a valid SubGroup.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    // Validate Division
                    if (!int.TryParse(ddlDivision.SelectedValue, out int divisionId))
                    {
                        lblMessage.Text = "Please select a valid Division.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    // Validate Product Code
                    if (!long.TryParse(txtProductCode.Text.Trim(), out var productCode))
                    {
                        lblMessage.Text = "Invalid Product Code";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    // Dist1 / Dist2 (0–100)
                    if (!decimal.TryParse(txtDist1.Text, out var dist1) || dist1 < 0 || dist1 > 100)
                    {
                        lblMessage.Text = "Discount1 must be a number between 0 and 100.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }
                    if (!decimal.TryParse(txtDist2.Text, out var dist2) || dist2 < 0 || dist2 > 100)
                    {
                        lblMessage.Text = "Discount2 must be a number between 0 and 100.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                        return;
                    }

                    int nextProductId = (_context.Products.Max(p => (int?)p.ProductID) ?? 101000) + 1;

                    if (_context.Products.Any(p => p.ProductCode == productCode))
                    {
                        lblMessage.Text = "A product with this code already exists.";
                        lblMessage.CssClass = "alert alert-warning mt-3";
                        return;
                    }

                    // Assign values
                    var product = new Models.Product
                    {
                        ProductID = nextProductId,
                        Type = productType,
                        PackingType = packingType,
                        BonusType = bonusType,
                        IsDiscounted = isDiscounted,
                        Dist1 = dist1,
                        Dist2 = dist2,
                        Name = txtName.Text.Trim(),
                        ProductCode = productCode,
                        HSCode = hsCode,
                        PackingSize = txtPackingSize.Text.Trim(),
                        CartonSize = cartonSize,
                        Uom = txtUom.Text.Trim(),
                        PurchaseDiscount = decimal.TryParse(txtPurchaseDiscount.Text, out var disc1) ? disc1 : 0,
                        ReqGST = decimal.TryParse(txtReqGST.Text, out var disc2) ? disc2 : 0,
                        UnReqGST = decimal.TryParse(txtUnReqGST.Text, out var disc3) ? disc3 : 0,
                        IsAdvTaxExempted = chkAdvTaxExempted.Checked,
                        IsGSTExempted = chkGSTExempted.Checked,
                        SubGroupID = subGroupId,
                        DivisionID = divisionId,
                        CreatedAt = DateTime.Now
                    };

                    _context.Products.Add(product);
                    _context.SaveChanges();

                    foreach (var bonus in Bonuses)
                    {
                        bonus.ProductID = product.ProductID;
                        _context.ProductBonuses.Add(bonus);
                    }
                    _context.SaveChanges();

                    // Clear Bonuses after save
                    Bonuses = new List<ProductBonus>();

                    lblMessage.Text = "Product saved successfully!";
                    lblMessage.CssClass = "alert alert-success mt-3";
                    ClearForm();
                    ShowNextProductId();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "alert alert-danger mt-3";
                }
            }
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtProductCode.Text = "";
            txtPackingSize.Text = "";
            txtCartonSize.Text = "";
            txtPurchaseDiscount.Text = "";
            txtHSCode.Text = "";
            txtReqGST.Text = "";
            txtUnReqGST.Text = "";
            ddlVendor.SelectedIndex = 0;
            ddlGroup.SelectedIndex = 0;
            ddlSubGroup.SelectedIndex = 0;
            ddlDivision.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            txtUom.Text = "";
            chkAdvTaxExempted.Checked = false;
            chkGSTExempted.Checked = false;
        }
    }
}