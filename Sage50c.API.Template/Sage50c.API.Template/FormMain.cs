using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sage50c.API.Sample {
    public partial class FormMain : Form {
        public FormMain() {
            InitializeComponent();
        }

        private void btnStartAPI_Click(object sender, EventArgs e) {
            try {
                APIEngine.Initialize((string)cmbAPI.SelectedItem, txtCompanyId.Text, chkAPIDebugMode.Checked);
            }
            catch( Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void FormMain_Load(object sender, EventArgs e) {
            this.Text = Application.ProductName;

            txtCompanyId.Text = Properties.Settings.Default.CompanyId;
            cmbAPI.SelectedItem = Properties.Settings.Default.API;

            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            dtSAFTStart.Value = new DateTime(currentYear, currentMonth, 1);
            dtSAFTEnd.Value = new DateTime(currentYear, currentMonth, DateTime.DaysInMonth( currentYear, currentMonth));

            APIEngine.APIStarted += APIEngine_APIStarted;
            APIEngine.APIStopped += APIEngine_APIStopped;
        }

        private void APIEngine_APIStopped(object sender, EventArgs e) {
            btnStartAPI.Enabled = true;
            btnTerminateAPI.Enabled = false;
            cmbAPI.Enabled = true;
        }

        private void APIEngine_APIStarted(object sender, EventArgs e) {
            btnStartAPI.Enabled = false;
            btnTerminateAPI.Enabled = true;
            cmbAPI.Enabled = false;
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e) {
            Properties.Settings.Default.CompanyId = txtCompanyId.Text;
            Properties.Settings.Default.API = (string)cmbAPI.SelectedItem;
            Properties.Settings.Default.Save();
        }

        private void brnmSAFTExport_Click(object sender, EventArgs e) {
            progressSAFT.Maximum = 100;
            progressSAFT.Minimum = 0;

            var saftFile = "C:\\Temp\\Test.xml";
            var factory = new S50cSAFTX22.SAFTExportFactory();
            factory.Version = "1.04";
            
            factory.RequestReplaceFile += Factory_RequestReplaceFile;
            factory.StatusProgressValues += Factory_StatusProgressValues;
            factory.SaftType = S50cSAFTX22.SaftTypeEnum.SaftTypeInvoice;
            factory.InitialDate = dtSAFTStart.Value;
            factory.FinalDate = dtSAFTEnd.Value;
            var saftExporter = factory.GetSAFTExporter();

            saftExporter.HWnd = this.Handle.ToInt32();
            saftExporter.AuditFileName = System.IO.Path.GetFileName(saftFile);

            saftExporter.Export(saftFile);

            string saftMessages = string.Empty;
            if ( saftExporter.ValidateSAFT("C:\\Temp\\Test.xml", ref saftMessages)) {
                MessageBox.Show($"SAFT Exportado com sucesso para: {saftFile}", Application.ProductName,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                if (!string.IsNullOrEmpty(saftMessages)) {
                    MessageBox.Show(saftMessages, Application.ProductName,
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
        }

        private void Factory_StatusProgressValues(string TextID, int Current, int Total) {
            lblSAFTProgress.Text = TextID;
            progressSAFT.Maximum = Total;
            progressSAFT.Value = Current;
        }

        private void Factory_RequestReplaceFile(string File, ref bool Cancel) {
            Cancel = (DialogResult.No == MessageBox.Show($"O ficheiro {File} já existe.{Environment.NewLine}Deseja substituí-lo?", 
                                                        Application.ProductName,
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question));
        }

        private void btnTerminateAPI_Click(object sender, EventArgs e) {
            APIEngine.Terminate();
        }
    }
}
