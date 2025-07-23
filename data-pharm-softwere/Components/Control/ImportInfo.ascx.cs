using System;
using System.Web.UI;

namespace data_pharm_softwere.Components.Control
{
    public partial class ImportInfo : System.Web.UI.UserControl
    {
        public string ImportContext
        {
            get => ViewState["ImportContext"]?.ToString()?.ToLower();
            set => ViewState["ImportContext"] = value?.ToLower();
        }

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
                case "vendor":
                    litHeader.Text = "Vendor Import Guide";
                    litBody.Text = "Upload a CSV file with the following columns:<br><ul>" +
                                   "<li><b>Name</b> (required)</li>" +
                                   "<li><b>SRACode</b> (required)</li>" +
                                   "<li><b>GstNo</b> (required)</li>" +
                                   "<li><b>NtnNo</b> (required)</li>" +
                                   "<li><b>CompanyCode</b> (required)</li>" +
                                   "<li><b>MaxDiscountAllowed</b> (0 - 100)</li>" +
                                   "<li><b>Email</b></li>" +
                                   "<li><b>Contact</b></li>" +
                                   "<li><b>Address</b> (required)</li>" +
                                   "<li><b>Town</b> (required)</li>" +
                                   "<li><b>City</b> (required)</li>" +
                                   "<li><b>LicenceNo</b></li>" +
                                   "<li><b>ExpiryDate</b> (required, format: yyyy-MM-dd)</li>" +
                                   "<li><b>Remarks</b></li>" +
                                   "</ul>";
                    break;

                case "division":
                    litHeader.Text = "Division Import Guide";
                    litBody.Text = "Upload a CSV file with the following columns:<br><ul>" +
                                   "<li><b>Name</b> (required)</li>" +
                                   "<li><b>VendorID</b> (required, must exist in the Vendors table)</li>" +
                                   "</ul>" +
                                   "Ensure that the VendorID you provide is valid and already exists in the system.";
                    break;

                case "group":
                    litHeader.Text = "Group Import Guide";
                    litBody.Text = "Upload a CSV file with the following columns:<br><ul>" +
                                   "<li><b>Name</b> (required)</li>" +
                                   "<li><b>DivisionID</b> (required, must exist in the Divisions table)</li>" +
                                   "</ul>" +
                                   "Ensure that the DivisionID you provide is valid and already exists in the system.";
                    break;

                case "subgroup":
                    litHeader.Text = "SubGroup Import Guide";
                    litBody.Text = "Upload a CSV file with the following columns:<br><ul>" +
                                   "<li><b>Name</b> (required)</li>" +
                                   "<li><b>GroupID</b> (required, must exist in the Groups table)</li>" +
                                   "</ul>" +
                                   "Ensure that the GroupID you provide is valid and already exists in the system.";
                    break;

                case "product":
                    litHeader.Text = "Product Import Guide";
                    litBody.Text = "Upload a CSV file with the following columns:<br><ul>" +
                                   "<li><b>Name</b> (required)</li>" +
                                   "<li><b>ProductCode</b> (required, must be unique, numeric)</li>" +
                                   "<li><b>HSCode</b> (required, numeric)</li>" +
                                   "<li><b>PackingSize</b> (required)</li>" +
                                   "<li><b>CartonSize</b> (required, numeric)</li>" +
                                   "<li><b>Uom</b> (required)</li>" +
                                   "<li><b>PurchaseDiscount</b> (required, %, numeric 0–100)</li>" +
                                   "<li><b>ReqGST</b> (required, %, numeric 0–100)</li>" +
                                   "<li><b>UnReqGST</b> (required, %, numeric 0–100)</li>" +
                                   "<li><b>IsAdvTaxExempted</b> (true/false)</li>" +
                                   "<li><b>IsGSTExempted</b> (true/false)</li>" +
                                   "<li><b>PackingType</b> (required: Tablet, Capsule, Syrup, Injection, Cream)</li>" +
                                   "<li><b>Type</b> (required: Medicine, Neutra, NonWare, Narcotics, Cosmetic, Consumer)</li>" +
                                   "<li><b>SubGroupID</b> (required, must exist in the SubGroups table)</li>" +
                                   "</ul>" +
                                   "Make sure that the <b>ProductCode</b> is unique and all dropdown/enumeration values are spelled correctly.";
                    break;

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

                case "cityroute":
                    litHeader.Text = "City Route Import Guide";
                    litBody.Text = "Upload a CSV file with the following column:<br><ul>" +
                                   "<li><b>Name</b> (required)</li>" +
                                   "</ul>" +
                                   "Each row should contain a unique route name.";
                    break;

                case "town":
                    litHeader.Text = "Town Import Guide";
                    litBody.Text = "Upload a CSV file with the following columns:<br><ul>" +
                                   "<li><b>Name</b> (required)</li>" +
                                   "<li><b>CityRouteID</b> (required) — must reference a valid CityRoute</li>" +
                                   "</ul>" +
                                   "Make sure <b>CityRouteID</b> corresponds to an existing City Route in the system.";
                    break;

                case "customer":
                    litHeader.Text = "Customer Import Guide";
                    litBody.Text = "Upload a CSV file with the following columns:<br><ul>" +
                                   "<li><b>Name</b> (required)</li>" +
                                   "<li><b>Email</b> (optional)</li>" +
                                   "<li><b>Contact</b> (required)</li>" +
                                   "<li><b>CNIC</b> (required)</li>" +
                                   "<li><b>Address</b> (required)</li>" +
                                   "<li><b>TownID</b> (required) — must reference an existing Town</li>" +
                                   "<li><b>LicenceNo</b> (optional)</li>" +
                                   "<li><b>ExpiryDate</b> (required, format: yyyy-MM-dd)</li>" +
                                   "<li><b>PartyType</b> (required) — one of: Doctor, Pharmacy, RetailerWholesaler, Dispenser, MedicalStore, Distributor, Institute</li>" +
                                   "<li><b>NtnNo</b> (required)</li>" +
                                   "<li><b>NorcoticsSaleAllowed</b> (true/false)</li>" +
                                   "<li><b>InActive</b> (true/false)</li>" +
                                   "<li><b>IsAdvTaxExempted</b> (true/false)</li>" +
                                   "<li><b>FbrInActiveGST</b> (true/false)</li>" +
                                   "<li><b>FBRInActiveTax236H</b> (true/false)</li>" +
                                   "</ul>" +
                                   "Ensure PartyType matches exactly (case-sensitive). Boolean fields must be 'true' or 'false'.";
                    break;

                case "salesman":
                    litHeader.Text = "Salesman Import Guide";
                    litBody.Text = "Upload a CSV file with the following columns:<br><ul>" +
                                   "<li><b>Name</b> (required)</li>" +
                                   "<li><b>Email</b> (optional)</li>" +
                                   "<li><b>Contact</b> (required)</li>" +
                                   "<li><b>Towns</b> (required) — pipe-separated list (e.g., TownA|TownB). Town names are matched using fuzzy spelling correction.</li>" +
                                   "</ul>" +
                                   "<p><b>Note:</b> If a town name is misspelled but close to an existing town in the system, it will still be matched using fuzzy logic.</p>";
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