using data_pharm_softwere.Data;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace data_pharm_softwere.Pages.Batch.Controls
{
    public partial class BatchModal : System.Web.UI.UserControl
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
            var products = _context.Products.ToList();
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
                var batch = new Models.BatchStock
                {
                    ProductID = int.Parse(ddlProduct.SelectedValue),
                    BatchNo = txtBatchNo.Text,
                    MFGDate = DateTime.Parse(txtMFGDate.Text),
                    ExpiryDate = DateTime.Parse(txtExpiryDate.Text),
                    DP = decimal.Parse(txtDP.Text),
                    TP = decimal.Parse(txtTP.Text),
                    MRP = decimal.Parse(txtMRP.Text),
                    CartonUnits = int.Parse(txtCartonQty.Text),
                    CartonDp = decimal.Parse(txtCartonPrice.Text),
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin",
                };

                _context.BatchesStock.Add(batch);
                _context.SaveChanges();

                Response.Redirect("/batch");
            }
        }
    }
}