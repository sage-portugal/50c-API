using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using S50cSys18;
using S50cUtil18;
using S50cBO18;
using S50cBL18;
using S50cDL18;

namespace APIDocumentos {
    public partial class frmDocumentos : Form {
        private S50cDL18.DSOItem itemProvider { get { return S50cAPIEngine.DSOCache.ItemProvider; } }
        private S50cSys18.SystemSettings systemSettings { get { return S50cAPIEngine.SystemSettings; } }
        private S50cDL18.DSOFactory dsoCache { get { return S50cAPIEngine.DSOCache; } }
        private bool transactionError = false;



        public frmDocumentos() {
            InitializeComponent();
        }


        void S50cAPIEngine_WarningMessage(string Message) {
            //Indicar um erro na transação de forma a cancelá-la
            transactionError = true;
            //
            MessageBox.Show(Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        /// Mensagens de erro da API
        /// Neste caso vamos lançar uma exeção que será apanhada no botão pressionado neste exemplo, de forma a informar o utilizador que falhou.
        void S50cAPIEngine_WarningError(int Number, string Source, string Description) {
            //Indicar um erro na transação de forma a cancelá-la
            transactionError = true;
            //
            string msg = string.Format("Erro: {0}{1}Fonte: {2}{1}{3}", Number, Environment.NewLine, Source, Description);
            MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void Form1_Load(object sender, EventArgs e) {
            if (cmbApplicationCode.Items.Count > 0) {
                cmbApplicationCode.SelectedIndex = 0;
            }
        }



        private void btnCriar_Click(object sender, EventArgs e) {
            TransactionID transId = null;

            double transDocNumber = 0;
            double.TryParse(txtTransDocNumber.Text, out transDocNumber);
            transId = TransactionUpdate(txtTransSerial.Text, txtTransDoc.Text, transDocNumber);

            if (transId != null) {
                MessageBox.Show(string.Format("Registo inserido: {0}", transId.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        private TransactionID TransactionUpdate(string transSerial, string transDoc, double transDocNumber) {
            TransactionID insertedTrans = null;

            try {
                transactionError = false;

                // Motor do documento
                BSOItemTransaction bsoItemTransaction = null;
                bsoItemTransaction = new BSOItemTransaction();
                bsoItemTransaction.UserPermissions = systemSettings.User;
                bsoItemTransaction.TransactionType = DocumentTypeEnum.dcTypeSale; //Tipo de documento para cabeçalho --> vendas
                // Motor dos detalhes (linhas)
                BSOItemTransactionDetail bsoItemTransDetail = null;
                bsoItemTransDetail = new BSOItemTransactionDetail(); //**
                bsoItemTransDetail.TransactionType = DocumentTypeEnum.dcTypeSale; //Tipo de documento para linhas --> vendas
                // Utilizador e permissões
                bsoItemTransDetail.UserPermissions = systemSettings.User;
                bsoItemTransDetail.PermissionsType = FrontOfficePermissionEnum.foPermByUser;

                bsoItemTransaction.BSOItemTransactionDetail = bsoItemTransDetail; //Adicionar os detalhes ao documento
                bsoItemTransDetail = null;
                //Inicializar uma transação

                bsoItemTransaction.Transaction = new ItemTransaction();

                bsoItemTransaction.InitNewTransaction(transDoc, transSerial);
                if (transDocNumber > 0)
                    bsoItemTransaction.Transaction.TransDocNumber = transDocNumber;

                //Terceiro
                double partyId = 0;
                double.TryParse(txtTransPartyId.Text, out partyId);
                bsoItemTransaction.PartyID = partyId;

                ItemTransaction trans = bsoItemTransaction.Transaction;
                if (trans == null) {
                    throw new Exception(string.Format("Não foi possivel inicializar o documento [{0}] da série [{1}]", transDoc, transSerial));

                }
                //
                // Limpar todas as linhas
                int i = 1;
                while (trans.Details.Count > 0) {
                    trans.Details.Remove(ref i);
                }

                //Set Create date and deliverydate
                var createDate = DateTime.Today;
                DateTime.TryParse(txtTransDate.Text, out createDate);
                trans.CreateDate = createDate;
                trans.ActualDeliveryDate = createDate;
                //
                // Definir se o imposto é incluido
                trans.TransactionTaxIncluded = chkTransTaxIncluded.Checked;
                //
                // Definir o pagamento. Neste caso optou-se por utilizar o primeiro pagamento disponivel na base de dados
                short PaymentId = dsoCache.PaymentProvider.GetFirstID();
                trans.Payment = dsoCache.PaymentProvider.GetPayment(PaymentId);
                //
                // Comentários / Observações
                trans.Comments = "Gerado por " + Application.ProductName;
                // desconto global
                double globalDiscount = 0;
                double.TryParse(txtTransGlobalDiscount.Text, out globalDiscount);
                trans.PaymentCumulativeDiscountPercent1 = globalDiscount;
                //
                //-------------------------------------------------------------------------
                // DOCUMENT DETAILS
                //-------------------------------------------------------------------------

                //
                //Adicionar a primeira linha ao documento
                double qty = 0; double.TryParse(txtTransQuantityL1.Text, out qty);
                double unitPrice = 0; double.TryParse(txtTransUnitPriceL1.Text, out unitPrice);
                double taxPercent = 0; double.TryParse(txtTransTaxRateL1.Text, out taxPercent);
                short wareHouseId = 0; short.TryParse(txtTransWarehouseL1.Text, out wareHouseId);
                Item item = dsoCache.ItemProvider.GetItem(txtTransItemL1.Text, systemSettings.BaseCurrency);

                TransAddDetail(trans, item, qty, txtTransUnL1.Text, unitPrice, taxPercent, wareHouseId);//, colorId, sizeId, "", serialNumber, lotId, lotDescription, lotExpDate, 0, lotRetYear, lotEditionId);
                //


                if (chkDocExtraField1.Checked == true || chkDocExtraField2.Checked == true) {
                    var ExtraFiedsDocument = new ExtraFieldList();

                    if (chkDocExtraField1.Checked == true) {
                        ExtraField extraField1 = new ExtraField();

                        extraField1.ExtraFieldID = 1; //nº do campo extra
                        extraField1.TextAnswer = txtNomeMotorista.Text;
                        int idadeMotorista = 0;
                        int.TryParse(txtIdadeMotorista.Text, out idadeMotorista);
                        extraField1.NumberAnswer = idadeMotorista;
                        extraField1.TransDocNumber = transDocNumber;
                        extraField1.TransSerial = transSerial;
                        extraField1.TransDocument = transDoc;
                        ExtraFiedsDocument.Add(extraField1);
                    }

                    if (chkDocExtraField2.Checked == true) {
                        ExtraField extraField2 = new ExtraField();

                        extraField2.ExtraFieldID = 2; //nº do campo extra
                        extraField2.TextAnswer = txtEstadoMaterial.Text;
                        extraField2.BooleanAnswer = chkGarantia.Checked;
                        extraField2.TransDocNumber = transDocNumber;
                        extraField2.TransSerial = transSerial;
                        extraField2.TransDocument = transDoc;
                        ExtraFiedsDocument.Add(extraField2);
                    }

                    bsoItemTransaction.ExtraFields = ExtraFiedsDocument;
                }

                bsoItemTransaction.Calculate(true);

                bsoItemTransaction.SaveDocument(false, false);

                if (!transactionError) {
                    insertedTrans = bsoItemTransaction.Transaction.TransactionID;
                }

                bsoItemTransDetail = null;

            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            return insertedTrans;
        }


        private void TransAddDetail(ItemTransaction trans, Item item, double qty, string unitOfMeasureId, double unitPrice, double taxPercent, short whareHouseId) {
            ItemTransactionDetail transDetail = new ItemTransactionDetail();
            transDetail.BaseCurrency = systemSettings.BaseCurrency;
            transDetail.ItemID = item.ItemID;
            transDetail.CreateDate = trans.CreateDate;
            transDetail.CreateTime = trans.CreateTime;
            transDetail.ActualDeliveryDate = trans.CreateDate;
            //Utilizar a descrição do artigo, ou uma descrição personalizada
            transDetail.Description = item.Description;
            // definir a quantidade
            transDetail.Quantity = qty;
            // Preço unitário. NOTA: Ver a diferença se o documento for com impostos incluidos!
            if (trans.TransactionTaxIncluded)
                transDetail.TaxIncludedPrice = unitPrice;
            else
                transDetail.UnitPrice = unitPrice;
            // Definir a lista de unidades
            transDetail.UnitList = item.UnitList;
            // Definir a unidade de venda/compra
            transDetail.SetUnitOfSaleID(unitOfMeasureId);
            //Definir os impostos
            short TaxGroupId = dsoCache.TaxesProvider.GetTaxableGroupIDFromTaxRate(taxPercent, systemSettings.SystemInfo.DefaultCountryID, systemSettings.SystemInfo.TaxRegionID);
            transDetail.TaxableGroupID = TaxGroupId;
            //armazém
            transDetail.WarehouseID = whareHouseId;
            // Identificador da linha
            transDetail.LineItemID = trans.Details.Count + 1;
            item = null;
            trans.Details.Add(transDetail);
        }

        private void btnTransClearL1_Click(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            Form2 verprint = new Form2();
            verprint.ShowDialog();

        }

        private void frmDocumentos_FormClosed(object sender, FormClosedEventArgs e) {
            S50cAPIEngine.Terminate();
            Application.Exit();
        }

        private void btnAbrirEmpresa_Click(object sender, EventArgs e) {
            try {
                //this.Cursor = Cursors.WaitCursor;
                S50cAPIEngine.WarningError += S50cAPIEngine_WarningError;
                S50cAPIEngine.WarningMessage += S50cAPIEngine_WarningMessage;

                var productCode = cmbApplicationCode.Text;
                var companyId = txtEmpresaNome.Text.Trim();
                //Inicia a API na empresa com o nome "Empresa", tal como existe na Área de Sistema
                S50cAPIEngine.Initialize(productCode, companyId, false);
                txtTransDate.Text = DateTime.Today.ToShortDateString();
                btnCriar.Enabled = true;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

    }
}
