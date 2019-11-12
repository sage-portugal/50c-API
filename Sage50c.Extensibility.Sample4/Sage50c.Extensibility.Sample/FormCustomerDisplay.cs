using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sage50c.ExtenderSample {
    public partial class FormCustomerDisplay : System.Windows.Forms.Form {
        public Boolean UseCustomerDisplay { get; set; }
        public Boolean UseAdsViewer { get; set; }

        public string DisplayLine1 { get; set; }
        public double DisplayValue { get; set; }

        public FormCustomerDisplay() {
            InitializeComponent();
        }
        public DialogResult ShowDisplayMessage() {

            return this.ShowDialog();
        }

        private void btnOK_Click(object sender, EventArgs e) {
            UseCustomerDisplay = chkCustomerDisplay.Checked;
            UseAdsViewer = chkAdsViewer.Checked;

            DisplayLine1 = txtDisplayLine1.Text;
            DisplayValue = (double) numValue.Value;


            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void FormCustomerDisplay_Load(object sender, EventArgs e) {
            txtDisplayLine1.Text = DisplayLine1;
            numValue.Value = (decimal) DisplayValue;

            chkCustomerDisplay.Enabled = UseCustomerDisplay;
            chkAdsViewer.Enabled = UseAdsViewer;

            chkCustomerDisplay.Checked = UseCustomerDisplay;
            chkAdsViewer.Checked = UseAdsViewer;

        }
    }
}
