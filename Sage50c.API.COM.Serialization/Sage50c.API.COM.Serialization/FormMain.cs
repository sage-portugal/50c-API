using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sage50c.API.COM.Serialization
{
    public partial class FormMain : Form
    {
        public FormMain() {
            InitializeComponent();
            
            APIEngine.APIStarted += (sender, e) => {
                btnCloseCompany.Enabled = true;
                btnOpenCompany.Enabled = false;
            };

            APIEngine.APIStopped += (sender, e) => {
                btnCloseCompany.Enabled = false;
                btnOpenCompany.Enabled = true;
            };
        }


        private void btnOpenCompany_Click(object sender, EventArgs e) {
            try {
                if (APIEngine.APIInitialized) {
                    MessageBox.Show("API Is already initialized. Close if first", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                APIEngine.Initialize(cmbAppCode.Text, txtCompanyID.Text, true);
            }
            catch( Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCloseCompany_Click(object sender, EventArgs e) {
            if (! APIEngine.APIInitialized) {
                MessageBox.Show("API is not initialized", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                APIEngine.Terminate();
            }
        }

        private void btnTransSerialize_Click(object sender, EventArgs e) {
            try {
                txtJSONBox.Clear();

                var transId = new S50cSys18.TransactionID();
                transId.FromString(txtTransactionID.Text);

                var trans = APIEngine.DSOCache.ItemTransactionProvider.GetItemTransaction(S50cSys18.DocumentTypeEnum.dcTypeSale, transId.TransSerial, transId.TransDocument, transId.TransDocNumber);
                if(trans != null) {
                    var serializer = ShopConnection.JsonCOMSerialization.JsonCOMSerializer.GetDefaultSerializer();
                    var jsonToken = JToken.FromObject(trans, serializer);
                    if (jsonToken != null) {
                        txtJSONBox.Text = jsonToken.ToString(); ;
                    }
                }
                else {
                    MessageBox.Show($"Transaction not found: {transId.ToString()}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnTransDeserialize_Click(object sender, EventArgs e) {
            try {
                var serializer = ShopConnection.JsonCOMSerialization.JsonCOMSerializer.GetDefaultSerializer();
                var jsonToken = JToken.Parse(txtJSONBox.Text);
                var trans = jsonToken.ToObject<S50cBO18.ItemTransaction>(serializer);
                txtJSONBox.Text = $"Transacion {trans.TransactionID.ToString()} deserialized";
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
