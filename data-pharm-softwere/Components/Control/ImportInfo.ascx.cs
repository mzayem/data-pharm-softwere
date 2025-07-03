using System;
using System.Web.UI;

namespace data_pharm_softwere.Components.Control
{
    public partial class ImportInfo : System.Web.UI.UserControl
    {
        public string ImportContext { get; set; }

        public event EventHandler DownloadRequested;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadImportGuide();
            }
        }

        private void LoadImportGuide()
        {
            if (string.IsNullOrEmpty(ImportContext)) return;

            switch (ImportContext.ToLower())
            {
                case "batch":
                    litHeader.Text = "Batch Import Guide";
                    litBody.Text = "Upload a CSV file with the following columns:<br><ul>" +
                                   "<li><b>BatchNo</b></li>" +
                                   "<li><b>MFGDate</b> (yyyy-MM-dd)</li>" +
                                   "<li><b>ExpiryDate</b> (yyyy-MM-dd)</li>" +
                                   "<li><b>DP</b></li>" +
                                   "<li><b>TP</b></li>" +
                                   "<li><b>MRP</b></li>" +
                                   "<li><b>CartonQty</b></li></ul>";
                    break;

                case "product":
                    litHeader.Text = "Product Import Guide";
                    litBody.Text = "Upload a CSV file with the following columns:<br><ul>" +
                                   "<li><b>ProductName</b></li>" +
                                   "<li><b>ProductCode</b></li>" +
                                   "<li><b>HSCode</b></li>" +
                                   "<li><b>PackingSize</b></li>" +
                                   "<li><b>CartonSize</b></li></ul>";
                    break;

                default:
                    litHeader.Text = "Import Guide";
                    litBody.Text = "Please upload a valid CSV file.";
                    break;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DownloadRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
