using Newtonsoft.Json.Linq;
using S50cBO22;
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
                btnItemSerialize.Enabled = true;
                btnItemDeserialize.Enabled = true;
                btnCustomerSerialize.Enabled = true;
                btnCustomerDeserialize.Enabled = true;
                btnItemTransSerialize.Enabled = true;
                btnItemTransDeSerialize.Enabled = true;
            };

            APIEngine.APIStopped += (sender, e) => {
                btnCloseCompany.Enabled = false;
                btnOpenCompany.Enabled = true;
                btnItemSerialize.Enabled = false;
                btnItemDeserialize.Enabled = false;
                btnCustomerSerialize.Enabled = false;
                btnCustomerDeserialize.Enabled = false;
                btnItemTransSerialize.Enabled = false;
                btnItemTransDeSerialize.Enabled = false;
            };

            cmbAppCode.SelectedIndex = 0;
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

        private void btnItemDeserialize_Click(object sender, EventArgs e) {
            try {
                var serializer = ShopConnection.JsonCOMSerialization.JsonCOMSerializer.GetDefaultSerializer();
                var jsonToken = JToken.Parse(txtJSONBox.Text);
                var item = jsonToken.ToObject<S50cBO22.Item>(serializer);
                txtJSONBox.Text = $"Item '{item.ItemID}' '{item.Description}' deserialized";
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void btnItemSerialize_Click(object sender, EventArgs e) {
            try {
                txtJSONBox.Clear();

                var item = APIEngine.DSOCache.ItemProvider.GetItem(txtItemID.Text, APIEngine.SystemSettings.BaseCurrency);
                if (item != null) {
                    var serializer = ShopConnection.JsonCOMSerialization.JsonCOMSerializer.GetDefaultSerializer();
                    var jsonToken = JToken.FromObject(item, serializer);
                    if (jsonToken != null) {
                        txtJSONBox.Text = jsonToken.ToString(); ;
                    }
                }
                else {
                    MessageBox.Show($"Item not found: {txtItemID.Text}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCustomerSerialize_Click(object sender, EventArgs e) {
            try {
                txtJSONBox.Clear();
                var customerId = 0.0;
                if (double.TryParse(txtCustomerID.Text, out customerId)) {
                    var customer = APIEngine.DSOCache.CustomerProvider.GetCustomer( customerId );
                    if (customer != null) {
                        var serializer = ShopConnection.JsonCOMSerialization.JsonCOMSerializer.GetDefaultSerializer();
                        var jsonToken = JToken.FromObject(customer, serializer);
                        if (jsonToken != null) {
                            txtJSONBox.Text = jsonToken.ToString(); ;
                        }
                    }
                    else {
                        MessageBox.Show($"Customer not found: {txtItemID.Text}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else {
                    MessageBox.Show($"Invalid customer: {txtItemID.Text}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void btnTransDeserialize_Click(object sender, EventArgs e) {
            try {
                var serializer = ShopConnection.JsonCOMSerialization.JsonCOMSerializer.GetDefaultSerializer();
                var jsonToken = JToken.Parse(txtJSONBox.Text);
                var trans = jsonToken.ToObject<S50cBO22.ItemTransaction>(serializer);
                txtJSONBox.Text = $"Transacion {trans.TransactionID.ToString()} deserialized";
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCustomerDeserialize_Click(object sender, EventArgs e) {
            try {
                var serializer = ShopConnection.JsonCOMSerialization.JsonCOMSerializer.GetDefaultSerializer();
                var jsonToken = JToken.Parse(txtJSONBox.Text);
                var customer = jsonToken.ToObject<S50cBO22.Customer> (serializer);
                txtJSONBox.Text = $"Customer '{customer.CustomerID}' '{customer.OrganizationName}' deserialized";
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        /// <summary>
        /// Sample transaction Deserialization
        /// </summary>
        private void DeserializeTransaction() {
            var serializer = ShopConnection.JsonCOMSerialization.JsonCOMSerializer.GetDefaultSerializer();
            var jsonToken = JToken.Parse(txtJSONBox.Text);
            var item = jsonToken.ToObject<S50cBO22.ItemTransaction>(serializer);
            txtJSONBox.Text = $"Transaction '{item.TransactionID}' deserialized";
        }

    
        /// <summary>
        /// Sample transaction serialization
        /// </summary>
        private void SerializeTransaction() {
            var item = APIEngine.DSOCache.ItemTransactionProvider.GetItemTransaction(S50cSys22.DocumentTypeEnum.dcTypeSale, "1", "FAC", 1);
            if (item != null) {
                var serializer = ShopConnection.JsonCOMSerialization.JsonCOMSerializer.GetDefaultSerializer();
                var jsonToken = JToken.FromObject(item, serializer);
                if (jsonToken != null) {
                    txtJSONBox.Text = jsonToken.ToString(); ;
                }
            }
        }

        private void btnItemTransSerialize_Click(object sender, EventArgs e) {
            SerializeTransaction();
        }

        private void btnItemTransDeSerialize_Click(object sender, EventArgs e) {
            DeserializeTransaction();

        }
    }
}
