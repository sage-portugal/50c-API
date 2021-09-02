using S50cBO22;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sage50c.API.Sample {
    public partial class FormTenderCheck : Form {
        private TenderCheck tenderCheck = null;

        public FormTenderCheck() {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        public DialogResult FillTenderCheck(TenderCheck tCheck) {
            tenderCheck = tCheck;

            FillBanks();

            txtCheckNumber.Text = tenderCheck.CheckSequenceNumber;
            cmbBank.Text = tenderCheck.BankID;
            dtCheckDate.Value = tenderCheck.CheckDeferredDate;
            txtCheckValue.Text = tenderCheck.CheckAmount.ToString("F2");

            this.StartPosition = FormStartPosition.CenterParent;
            return this.ShowDialog();
        }

        private void FillObject() {
            tenderCheck.CheckSequenceNumber = txtCheckNumber.Text;
            tenderCheck.BankID = cmbBank.Text;
            tenderCheck.CheckDeferredDate = dtCheckDate.Value;
            double checkValue = 0;
            if (double.TryParse(txtCheckValue.Text, out checkValue)) {
                tenderCheck.CheckAmount = checkValue;
            }
            else {
                tenderCheck.CheckAmount = 0;
            }
        }

        private void FillBanks() {
            var dsoBankAccountCodes = new S50cDL22.DSOBankAccountCodes();

            cmbBank.Items.Clear();
            cmbBank.DisplayMember = "BankId";
            cmbBank.ValueMember = "Name";

            var rsBanks = dsoBankAccountCodes.GetBankAccountCodesRS();
            while (!rsBanks.EOF) {
                cmbBank.Items.Add(new { BankId = rsBanks.Fields["QuickSearchBankName"].Value, Name = rsBanks.Fields["BankName"].Value });
                rsBanks.MoveNext();
            };
            rsBanks.Close();
            rsBanks = null;

            dsoBankAccountCodes = null;
        }

        private void btnOK_Click(object sender, EventArgs e) {
            FillObject();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
