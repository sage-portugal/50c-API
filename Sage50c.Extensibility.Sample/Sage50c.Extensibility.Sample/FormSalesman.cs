using S50cBL22;
using S50cBO22;
using S50cSys22;
using Sage50c.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sage50c.ExtenderSample22 {
    public partial class FormSalesman : Form, IChildPanel {
        public FormSalesman() {
            InitializeComponent();
            ApplyGridStyle();
        }

        public string Description {
            get {
                return "Ver vendas";
            }
        }

        /// <summary>
        /// OBRIGATÓRIO: hWnd da janela
        /// </summary>
        public int Handler {
            get {
                return this.Handle.ToInt32();
            }
        }

        public stdole.StdPicture Picture {
            get {
                //return null;
                return (stdole.StdPicture)ImageConverter.GetIPictureDispFromImage(Icon.ToBitmap());
            }
        }

        public string Title {
            get {
                return "Vendas";
            }
        }

        public bool BeforeCancel() {
            return true;
        }

        public bool BeforeOk() {
            return true;
        }

        public bool CheckIDValue(string value) {
            return true;
        }

        public bool OnMenuItem(string MenuItemID) {
            return true;
        }


        /// <summary>
        /// Cor de fundo
        /// </summary>
        /// <param name="value"></param>
        public void SetBackcolor(int value) {
            this.BackColor = System.Drawing.Color.FromArgb(value);
        }

        public void SetFocus() {
            try {
                this.SetFocus();
            }
            catch { }
        }

        public void SetSize(float Width, float Height) {
            try {
                // Traduzir para pixels
                var width = (int)Microsoft.VisualBasic.Compatibility.VB6.Support.TwipsToPixelsX(Width);
                var height = (int)Microsoft.VisualBasic.Compatibility.VB6.Support.TwipsToPixelsY(Height);

                //this.SetBounds(0, 0, (int)(Width/16), (int)(Height/16));
                this.SetBounds(0, 0, width, height);
                this.Visible = true;
                //this.BringToFront();
            }
            catch { }

        }


        public void Validate(ExtendedEventResult result) {
            result.Success = true;
            result.ResultMessage = "Validate Event: Not done yet, but I'm returning TRUE so you can continue.";
        }
        public void Save() {
            MessageBox.Show("Save event: save done", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FormSalesman_Load(object sender, EventArgs e) {

        }

        public void UpdateNumSales(double salesmanId) {
            dataGridSales.Rows.Clear();
            var listOfSales = GetSalesList(salesmanId);
            var sales = listOfSales.Count;
            txtNumSales.Text = sales.ToString();
            double totalValue = 0;
            foreach(var sale in  listOfSales) {
                dataGridSales.Rows.Add(sale.CreateDate.ToShortDateString(), sale.TotalAmount, sale.PartyOrganizationName);
                totalValue+=sale.TotalAmount;
            }
            dataGridSales.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            txtTotalValue.Text = totalValue.ToString();
        }

        private List<ItemTransaction> GetSalesList(double SalesmanID) {
            var mainProvider = APIEngine.DataManager.MainProvider;

            var list = new List<ItemTransaction>();

            string query = "SELECT SaleTransaction.CreateDate, TotalAmount, Customer.OrganizationName FROM (SaleTransaction INNER JOIN Customer ON Customer.CustomerID = SaleTransaction.PartyID)  " +
                           "WHERE SaleTransaction.SalesmanID = " + SalesmanID;

            ADODB.Recordset recordset = mainProvider.Execute(query);

            while(!recordset.EOF) {
                var bsoTransaction = new BSOItemTransaction();
                bsoTransaction.Transaction.CreateDate = (DateTime)recordset.Fields["CreateDate"].Value;
                bsoTransaction.Transaction.TotalAmount = (double)recordset.Fields["TotalAmount"].Value;
                var orgName = (string)recordset.Fields["OrganizationName"].Value;
                if (!string.IsNullOrEmpty(orgName)) {
                    bsoTransaction.PartyOrganizationName = orgName;
                }
                list.Add(bsoTransaction.Transaction);
                bsoTransaction = null;
                recordset.MoveNext();
            }
            recordset.Close();
            recordset = null;

            return list;
        }

        private void ApplyGridStyle() {
            dataGridSales.BackgroundColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.WindowBackColor);
            dataGridSales.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.AppHeaderBackColor);
            dataGridSales.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.TextNoFocus);
            dataGridSales.GridColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.LightGray);

            dataGridSales.RowsDefaultCellStyle.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.WindowBackColor);
            dataGridSales.RowsDefaultCellStyle.SelectionBackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.WindowBackColor);
            dataGridSales.RowsDefaultCellStyle.SelectionForeColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.TextColor);
            dataGridSales.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.TabBackColor);

            dataGridSales.Rows.Clear();
            dataGridSales.ClearSelection();
           
        }
    }
}
