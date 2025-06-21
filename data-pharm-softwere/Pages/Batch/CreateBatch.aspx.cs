using data_pharm_softwere.Data;
using System;
using System.Linq;

namespace data_pharm_softwere.Pages.Batch
{
    public partial class CreateBatch : System.Web.UI.Page
    {
        private readonly DataPharmaContext _context = new DataPharmaContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            var products = _context.Products.OrderBy(p => p.Name).ToList();
            ddlProduct.DataSource = products;
            ddlProduct.DataTextField = "Name";
            ddlProduct.DataValueField = "ProductID";
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Product --", ""));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var batch = new Models.Batch
                {
                    ProductID = string.IsNullOrEmpty(ddlProduct.SelectedValue) ? (int?)null : int.Parse(ddlProduct.SelectedValue),
                    BatchNo = int.Parse(txtBatchNo.Text),
                    MFGDate = DateTime.Parse(txtMFGDate.Text),
                    ExpiryDate = DateTime.Parse(txtExpiryDate.Text),
                    DP = decimal.Parse(txtDP.Text),
                    TP = decimal.Parse(txtTP.Text),
                    MRP = decimal.Parse(txtMRP.Text),
                    CartonQty = int.Parse(txtCartonQty.Text),
                    CartonPrice = decimal.Parse(txtCartonPrice.Text),
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin",
                };

                _context.Batches.Add(batch);
                _context.SaveChanges();

                Response.Redirect("/batch");
            }
        }
    }
}