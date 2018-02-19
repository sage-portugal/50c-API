using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sage.S50c.API.Sample {
    public partial class FormExternalSignature : Form {
        public string Signature { get; set; }
        public short SignatureVersion { get; set; }
        public int SoftwareCertificateNumber { get; set; }

        public FormExternalSignature() {
            InitializeComponent();

            SignatureVersion = 0;
            Signature = string.Empty;
            SoftwareCertificateNumber = 9999;
        }

        private void btnOK_Click(object sender, EventArgs e) {
            short shortTemp = 0;
            if (short.TryParse(txtSignatureVersion.Text, out shortTemp)) {
                SignatureVersion = shortTemp;
            }
            else {
                SignatureVersion = 0;
            }
            SignatureVersion = shortTemp;
            //
            int intTemp = 0;
            if (int.TryParse(txtSoftwareCertNumber.Text, out intTemp)) {
                SoftwareCertificateNumber = intTemp;
            }
            else {
                SoftwareCertificateNumber = 0;
            }
            //
            Signature = txtSignature.Text;
            //
            DialogResult = DialogResult.OK;
        }

        private void FormExternalSignature_Load(object sender, EventArgs e) {
            txtSignature.Text = Signature;
            txtSignatureVersion.Text = SignatureVersion.ToString();
            txtSoftwareCertNumber.Text = SoftwareCertificateNumber.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
        }
    }
}
