using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using S50cBL22;
using S50cBO22;
using S50cDL22;
using S50cPrint22;
using S50cSys22;
using S50cUtil22;

namespace Sage50c.API.Sample {
    public partial class fApi : Form {
        /// <summary>
        /// Motor de dados para os artigos.
        /// NOTA: Api tem de estar inicializada antes de usar!
        /// </summary>
        private DSOItem itemProvider { get { return APIEngine.DSOCache.ItemProvider; } }
        /// <summary>
        /// Parâmetros do sistema
        /// </summary>
        private SystemSettings systemSettings { get { return APIEngine.SystemSettings; } }
        /// <summary>
        /// Cache dos motores de acesso a dados mais comuns
        /// </summary>
        private DSOFactory dsoCache { get { return APIEngine.DSOCache; } }
        //
        /// <summary>
        /// Inidica que houve um erro na transação e não foi gravada
        /// </summary>
        private bool transactionError = false;
        /// <summary>
        /// Motor das transações de documentos de compra e venda
        /// </summary>
        private BSOItemTransaction bsoItemTransaction = null;
        /// <summary>
        /// Motor das transações de documentos de stock
        /// </summary>
        private BSOStockTransaction bsoStockTransaction = null;
        /// <summary>
        /// Motor das transações de recibos e pagamentos
        /// </summary>
        AccountTransactionManager accountTransManager = null;
        /// <summary>
        /// Printing MANAGER
        /// </summary>
        private PrintingManager printingManager { get { return APIEngine.PrintingManager; } }
        /// <summary>
        /// Motor dos armazens
        /// </summary>
        private DSOWarehouse dSOWarehouse = new DSOWarehouse();

        public fApi() {

            InitializeComponent();

            FormatColorGrid();
            FormatSizeGrid();

            chkAPIDebugMode.Checked = Properties.Settings.Default.DebugMode;
            txtCompanyId.Text = Properties.Settings.Default.CompanyId;
            cmbAPI.SelectedItem = Properties.Settings.Default.API;

            APIEngine.APIStarted += S50cAPIEngine_APIStarted;
            APIEngine.APIStopped += S50cAPIEngine_APIStopped;
        }

        #region Eventos da RTLAPI

        void S50cAPIEngine_APIStopped(object sender, EventArgs e) {
            accountTransManager = null;
            bsoItemTransaction = null;
            bsoStockTransaction = null;

            tabEntities.Enabled = false;
            btnStopAPI.Enabled = false;
            btnStartAPI.Enabled = true;

            btnInsert.Enabled = false;
            btnUpdate.Enabled = false;
            btnRemove.Enabled = false;
            btnGet.Enabled = false;
            btnClear.Enabled = false;

            gbShareCost_1.Enabled = false;
            gbShareCost_2.Enabled = false;
            //
            cmbAPI.Enabled = true;

            this.Cursor = Cursors.Default;
        }

        void S50cAPIEngine_APIStarted(object sender, EventArgs e) {

            gbShareCost_1.Enabled = APIEngine.SystemSettings.SpecialConfigs.UpdateItemCostWithFreightAmount;
            gbShareCost_2.Enabled = APIEngine.SystemSettings.SpecialConfigs.UpdateItemCostWithFreightAmount;

            tabEntities.Enabled = true;

            btnStopAPI.Enabled = true;
            btnStartAPI.Enabled = false;
            cmbAPI.Enabled = false;

            btnInsert.Enabled = true;
            btnUpdate.Enabled = true;
            btnRemove.Enabled = true;
            btnGet.Enabled = true;
            btnClear.Enabled = true;
            btnPrint.Enabled = true;



            chkPrintPreview.Enabled = true;
            //
            btnAccoutTransPrint.Enabled = true;
            chkAccoutTransPrintPreview.Enabled = true;
            //
            //Inicialiizar o motor do documentos de venda
            bsoItemTransaction = new BSOItemTransaction();
            bsoItemTransaction.UserPermissions = systemSettings.User;
            //Eventos
            bsoItemTransaction.WarningItemStock += BsoItemTransaction_WarningItemStock;
            //
            //Inicializar o motor dos documentos de stock
            bsoStockTransaction = new BSOStockTransaction();
            bsoStockTransaction.UserPermissions = systemSettings.User;
            //
            // Inicilizar o motor dos recibos e pagamentos
            accountTransManager = new AccountTransactionManager();
            //
            // Load combos
            // Customer -- Load combos data and clear
            ItemClear(true);
            CustomerClear();
            SupplierClear();
            TransactionClear();
            AccountTransactionClear();

            //txtTransDoc.Text = "FAC";
            //txtTransSerial.Text = "1";
            //txtTransDocNumber.Text = "2";
            //chkPrintPreview.Checked = false;

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Mensagem de aviso de falta de stock, tal como está na 50c
        /// </summary>
        /// <param name="MsgID"></param>
        /// <param name="objItemTransactionDetail"></param>
        private void BsoItemTransaction_WarningItemStock(TransactionWarningsEnum MsgID, ItemTransactionDetail objItemTransactionDetail) {
            double dblStockQuantity = 0;
            double dblReorderPointQuantity = 0;
            string strMessage = string.Empty;

            switch (MsgID) {
                case TransactionWarningsEnum.tweItemColorSizeStockNotHavePhysical:
                case TransactionWarningsEnum.tweItemStockNotHavePhysical:
                    if (objItemTransactionDetail.PackQuantity == 0) {
                        dblStockQuantity = objItemTransactionDetail.QntyPhysicalBalanceCount;
                    }
                    else {
                        dblStockQuantity = objItemTransactionDetail.QntyPhysicalBalanceCount / objItemTransactionDetail.PackQuantity;
                    }
                    strMessage = APIEngine.gLng.GS((int)MsgID, new object[]{
                                                             objItemTransactionDetail.WarehouseID.ToString().Trim(),
                                                             dblStockQuantity,
                                                             objItemTransactionDetail.UnitOfSaleID,
                                                             objItemTransactionDetail.ItemID,
                                                             objItemTransactionDetail.Size.Description,
                                                             objItemTransactionDetail.Color.Description}
                                                     );

                    break;

                case TransactionWarningsEnum.tweItemReorderPoint:
                case TransactionWarningsEnum.tweItemColorSizeReorderPoint:
                    if (objItemTransactionDetail.PackQuantity == 0) {
                        dblStockQuantity = objItemTransactionDetail.QntyWrPhysicalBalanceCount;
                        dblReorderPointQuantity = objItemTransactionDetail.QntyReorderPoint;
                    }
                    else {
                        dblStockQuantity = objItemTransactionDetail.QntyWrPhysicalBalanceCount / objItemTransactionDetail.PackQuantity;
                        dblReorderPointQuantity = objItemTransactionDetail.QntyReorderPoint / objItemTransactionDetail.PackQuantity;
                    }
                    strMessage = APIEngine.gLng.GS((int)MsgID, new object[]{
                                                             objItemTransactionDetail.WarehouseID.ToString(),
                                                             dblStockQuantity.ToString(),
                                                             objItemTransactionDetail.UnitOfSaleID,
                                                             objItemTransactionDetail.ItemID,
                                                             objItemTransactionDetail.Size.Description,
                                                             objItemTransactionDetail.Color.Description,
                                                             dblReorderPointQuantity.ToString()}
                                );
                    break;

                default:
                    if (objItemTransactionDetail.PackQuantity == 0) {
                        dblStockQuantity = objItemTransactionDetail.QntyAvailableBalanceCount;
                    }
                    else {
                        dblStockQuantity = objItemTransactionDetail.QntyAvailableBalanceCount / objItemTransactionDetail.PackQuantity;
                    }
                    strMessage = APIEngine.gLng.GS((int)MsgID, new object[]{
                                                             objItemTransactionDetail.WarehouseID.ToString(),
                                                             dblStockQuantity.ToString(),
                                                             objItemTransactionDetail.UnitOfSaleID,
                                                             objItemTransactionDetail.ItemID,
                                                             objItemTransactionDetail.Size.Description,
                                                             objItemTransactionDetail.Color.Description}
                                                      );
                    break;
            }
            if (!string.IsNullOrEmpty(strMessage)) {
                MessageBox.Show(strMessage, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void accountTransManager_FunctionExecuted(string FunctionName, string FunctionParam) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Mensagens de AVISO da API
        /// Vamos mostrar só as mensagens
        /// </summary>
        /// <param name="Message"></param>
        void S50cAPIEngine_WarningMessage(string Message) {
            //Indicar um erro na transação de forma a cancelá-la
            transactionError = true;
            //
            MessageBox.Show(Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// Mensagens de erro da API
        /// Neste caso vamos lançar uma exeção que será apanhada no botão pressionado neste exemplo, de forma a informar o utilizador que falhou.
        /// </summary>
        /// <param name="Number">Número do erro </param>
        /// <param name="Source">O método que gerou o erro</param>
        /// <param name="Description">A descrição do erro</param>
        void S50cAPIEngine_WarningError(int Number, string Source, string Description) {
            //Indicar um erro na transação de forma a cancelá-la
            transactionError = true;
            //
            string msg = string.Format("Erro: {0}{1}Fonte: {2}{1}{3}", Number, Environment.NewLine, Source, Description);
            MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Mensagens genéricas de AVISO/ERRO/INFO da API
        /// Vamos mostrar só as mensagens
        /// É necessário devolver um valor no Args.Result
        /// </summary>
        /// <param name="Message"></param>
        private void S50cAPIEngine_Message(APIEngine.MessageEventArgs Args) {
            Args.Result = MessageBox.Show(Args.Prompt, Args.Title, Args.Buttons, Args.Icon);
        }

        #endregion

        #region User interface

        /// <summary>
        /// Inicialização da API
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartAPI_Click(object sender, EventArgs e) {
            try {
                this.Cursor = Cursors.WaitCursor;

                APIEngine.WarningError += S50cAPIEngine_WarningError;
                APIEngine.WarningMessage += S50cAPIEngine_WarningMessage;
                APIEngine.Message += S50cAPIEngine_Message;

                APIEngine.Initialize(cmbAPI.SelectedItem.ToString(), txtCompanyId.Text, chkAPIDebugMode.Checked);
            }
            catch (Exception ex) {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void fApi_FormClosed(object sender, FormClosedEventArgs e) {
            // Guardar a empresa de testes
            Properties.Settings.Default.DebugMode = chkAPIDebugMode.Checked;
            Properties.Settings.Default.CompanyId = txtCompanyId.Text;
            Properties.Settings.Default.API = cmbAPI.SelectedItem.ToString();
            Properties.Settings.Default.Save();
            //
            // Terminar a API e sair
            APIEngine.Terminate();
            Application.Exit();
        }

        private void btnCloseAPI_Click(object sender, EventArgs e) {
            if (APIEngine.APIInitialized) {
                APIEngine.Terminate();
            }
            Application.Exit();
        }

        #endregion

        #region ITEM

        /// <summary>
        /// Insere um novo artigo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsert_Click(object sender, EventArgs e) {
            try {
                transactionError = false;
                TransactionID transId = null;

                switch (tabEntities.SelectedIndex) {
                    case 0: ItemInsert(); break;
                    case 1: CustomerUpdate((double)numCustomerId.Value, true); break;
                    case 2: SupplierUpdate(double.Parse(txtSupplierId.Text), true); break;
                    case 3: transId = TransactionInsert(false); break;
                    case 4: transId = AccountTransactionUpdate(true); break;

                    case 5: UnitOfMeasureUpdate(txtUnitOfMeasureId.Text, true); break;
                }
                if (!transactionError) {
                    string msg = null;
                    if (transId != null) {
                        msg = string.Format("Registo inserido: {0}", transId.ToString());
                    }
                    else {
                        msg = "Registo inserido.";
                    }

                    MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Remove um artigo da BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e) {
            try {
                if (DialogResult.Yes == MessageBox.Show("Anular este registo da base de dados?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    TransactionID transId = null;
                    transactionError = false;

                    switch (tabEntities.SelectedIndex) {
                        case 0: ItemRemove(); break;                                        //Artigos
                        case 1: CustomerRemove((double)numCustomerId.Value); break;         //Clientes
                        case 2: SupplierRemove(double.Parse(txtSupplierId.Text)); break;  //Fornecedores
                        case 3: transId = TransactionRemove(); break;                                 //Compras e Vendas
                        case 4: transId = AccountTransactionRemove(); break;                          //Pagamentos e recebimentos

                        case 5: UnitOfMeasureRemove(txtUnitOfMeasureId.Text); break;        //Unidades de medida
                    }

                    if (!transactionError) {
                        string msg = null;
                        if (transId != null) {
                            msg = string.Format("Registo anulado: {0}", transId.ToString());
                        }
                        else {
                            msg = "Registo anulado.";
                        }
                        MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Altera um artigo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAlterar_Click(object sender, EventArgs e) {
            try {

                TransactionID transId = null;
                transactionError = false;

                switch (tabEntities.SelectedIndex) {
                    case 0: ItemUpdate(txtItemId.Text); break;
                    case 1: CustomerUpdate((double)numCustomerId.Value, false); break;
                    case 2: SupplierUpdate(double.Parse(txtSupplierId.Text), false); break;
                    case 3: transId = TransactionEdit(false); break;
                    case 4: transId = AccountTransactionUpdate(false); break;

                    case 5: UnitOfMeasureUpdate(txtUnitOfMeasureId.Text, false); break;
                }

                if (!transactionError) {
                    string msg = null;
                    if (transId != null) {
                        msg = string.Format("Registo alterado: {0}", transId.ToString());
                    }
                    else {
                        msg = "Registo alterado.";
                    }
                    MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void btnItemLoad_Click(object sender, EventArgs e) {
            try {
                switch (tabEntities.SelectedIndex) {
                    case 0: ItemGet(txtItemId.Text.Trim()); break;
                    case 1: CustomerGet((double)numCustomerId.Value); break;
                    case 2: SupplierGet(double.Parse(txtSupplierId.Text)); break;
                    case 3: TransactionGet(false); break;
                    case 4: AccountTransactionGet(); break;

                    case 5: UnitOfMeasureGet(txtUnitOfMeasureId.Text); break;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Cria um artigo novo
        /// * = Campos obrigatórios
        /// </summary>
        private void ItemInsert() {
            string itemId = txtItemId.Text.Trim();
            if (string.IsNullOrEmpty(itemId)) {
                MessageBox.Show("O código do artigo está vazio!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else {
                if (dsoCache.ItemProvider.ItemExist(itemId)) {
                    throw new Exception(string.Format("O artigo [{0}] já existe.", itemId));
                }

                var newItem = new Item();
                var dsoPriceLine = new DSOPriceLine();
                //*
                newItem.ItemID = itemId;
                newItem.Description = txtItemDescription.Text;
                newItem.ShortDescription = txtItemShortDescription.Text;
                newItem.Comments = txtItemComments.Text;
                // IVA/Imposto por omissão do sistema
                newItem.TaxableGroupID = systemSettings.SystemInfo.ItemDefaultsSettings.DefaultTaxableGroupID;
                //
                newItem.SupplierID = APIEngine.DSOCache.SupplierProvider.GetFirstSupplierEx();
                //
                //Inicializar as linhas de preço do artigo
                newItem.InitPriceList(dsoPriceLine.GetPriceLineRS());
                // Preço do artigo (linha de preço=1)
                Price myPrice = newItem.SalePrice[1, 0, string.Empty, 0, APIEngine.SystemSettings.SystemInfo.ItemDefaultsSettings.ItemDefaultUnit];
                //
                // Definir o preços (neste caso, com imposto (IVA) incluido)
                myPrice.TaxIncludedPrice = (double)numItemPriceTaxIncluded.Value;
                // Obter preço unitário sem impostos
                myPrice.UnitPrice = APIEngine.DSOCache.TaxesProvider.GetItemNetPrice(
                                                    myPrice.TaxIncludedPrice,
                                                    newItem.TaxableGroupID,
                                                    systemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID,
                                                    systemSettings.SystemInfo.TaxRegionID);
                //
                // *Familia: Obter a primeira disponivel
                double familyId = APIEngine.DSOCache.FamilyProvider.GetFirstLeafFamilyID();
                newItem.Family = APIEngine.DSOCache.FamilyProvider.GetFamily(familyId);

                //// Descomentar para criar COR e adicionar ao artigo
                //// Criar nova côr na base de dados.
                //var newColorId = dsoCache.ColorProvider.GetNewID();
                //var colorCode = System.Drawing.Color.Blue.B << 32 + System.Drawing.Color.Blue.G << 16 + System.Drawing.Color.Blue.R;
                //var newColor = new S50cBO22.Color() {
                //    ColorCode = colorCode,
                //    ColorID = newColorId,
                //    Description = "Cor " + newColorId.ToString()
                //};
                //dsoCache.ColorProvider.Save(newColor, newColor.ColorID, true);
                ////
                //// Adicionar ao artigo
                //var newItemColor = new ItemColor() {
                //    ColorID = newColor.ColorID,
                //    ColorName = newColor.Description,
                //    ColorCode = (int)newColor.ColorCode,
                //    //ColorKey = NÃO USAR
                //};

                // Adicionar cores ao artigo

                // Definir as cores do artigo
                AddColorsToItem(newItem);

                //Definir os tamanhos do artigo
                AddSizesToItem(newItem);

                //newItem.Colors.Add(newItemColor);

                //// Descomentar para criar um novo tamanho e adicionar ao artigo
                //// Criar um tamanho nov
                //var newSizeID = dsoCache.SizeProvider.GetNewID();
                //var newSize = new S50cBO22.Size() {
                //    Description = "Size " + newSizeID.ToString(),
                //    SizeID = newSizeID,
                //    //SizeKey = NÃO USAR
                //};
                //dsoCache.SizeProvider.Save(newSize, newSize.SizeID, true);
                //var newItemSize = new ItemSize() {
                //    SizeID = newSize.SizeID,
                //    SizeName = newSize.Description,
                //    Quantity = 1,
                //    Units = 1
                //};
                //newItem.Sizes.Add(newItemSize);
                ////
                //// Adicionar um preço ao tamanho
                //myPrice = newItem.SalePrice[1, newSizeID, string.Empty, 0, APIEngine.SystemSettings.SystemInfo.ItemDefaultUnit];
                //// Para ser diferente, vamos colocar este preço com mais 10%
                //myPrice.TaxIncludedPrice = (double)numItemPriceTaxIncluded.Value * 1.10;
                //myPrice.UnitPrice = S50cAPIEngine.DSOCache.TaxesProvider.GetItemNetPrice(
                //                                    myPrice.TaxIncludedPrice,
                //                                    newItem.TaxableGroupID,
                //                                    systemSettings.SystemInfo.DefaultCountryID,
                //                                    systemSettings.SystemInfo.TaxRegionID);
                ////NOTA: A linha seguinte só é necessário se for um novo preço. Se já existe, não adicionar o preço à coleção. Neste exemplo criamos um tamanho novo por isso o preço também é novo
                //newItem.SalePrice.Add(myPrice);
                //
                // Gravar
                dsoCache.ItemProvider.Save(newItem, newItem.ItemID, true);
            }
        }

        /// <summary>
        /// Elimina um Artigo
        /// </summary>
        /// <param name="itemId"></param>
        private void ItemRemove() {
            string itemId = txtItemId.Text.Trim();
            itemProvider.Delete(itemId);
            //
            ItemClear(false);
        }

        /// <summary>
        /// Altera um Artigo
        /// </summary>
        /// <param name="itemId"></param>
        private void ItemUpdate(string itemId) {
            var myItem = APIEngine.DSOCache.ItemProvider.GetItem(itemId, systemSettings.BaseCurrency);
            if (myItem != null) {
                myItem.Description = txtItemDescription.Text;
                myItem.ShortDescription = txtItemShortDescription.Text;
                myItem.Comments = txtItemComments.Text;
                //
                // Preços - PVP1
                Price myPrice = myItem.SalePrice[1, 0, string.Empty, 0, myItem.UnitOfSaleID];
                // Definir o preço (neste caso, com imposto (IVA) incluido)
                myPrice.TaxIncludedPrice = (double)numItemPriceTaxIncluded.Value;
                // Obter preço unitário sem impostos
                myPrice.UnitPrice = APIEngine.DSOCache.TaxesProvider.GetItemNetPrice(
                                                    myPrice.TaxIncludedPrice,
                                                    myItem.TaxableGroupID,
                                                    systemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID,
                                                    systemSettings.SystemInfo.TaxRegionID);

                // Definir as cores do artigo
                AddColorsToItem(myItem);

                //Definir os tamanhos do artigo
                AddSizesToItem(myItem);

                // Guardar as alterações
                APIEngine.DSOCache.ItemProvider.Save(myItem, myItem.ItemID, false);
            }
            else {
                throw new Exception(string.Format("Artigo [{0}] não encontrado.", itemId));
            }
        }

        /// <summary>
        /// Ler e apresenta a informação de um artigo
        /// </summary>
        private void ItemGet(string itemId) {
            if (string.IsNullOrEmpty(itemId)) {
                throw new Exception("O código do artigo está vazio!");
            }
            else {
                ItemClear(false);
                //Ler o artigo da BD na moeda base
                var item = itemProvider.GetItem(itemId, systemSettings.BaseCurrency);

                if (item != null) {
                    txtItemId.Text = item.ItemID;
                    txtItemDescription.Text = item.Description;
                    txtItemShortDescription.Text = item.ShortDescription;
                    numItemPriceTaxIncluded.Value = (decimal)item.SalePrice[1, 0, string.Empty, 0, item.UnitOfSaleID].TaxIncludedPrice;
                    txtItemComments.Text = item.Comments;

                    foreach (ItemColor value in item.Colors) {
                        var newRowIndex = dgvColor.Rows.Add();
                        var newRow = dgvColor.Rows[newRowIndex];

                        newRow.Cells[0].Value = value.ColorID;
                        newRow.Cells[1].Style.BackColor = ColorTranslator.FromOle((int)value.ColorCode);
                        newRow.Cells[2].Value = value.ColorName;
                    }

                    foreach (ItemSize value in item.Sizes) {
                        var newRowIndex = dgvSize.Rows.Add();
                        var newRow = dgvSize.Rows[newRowIndex];

                        newRow.Cells[0].Value = value.SizeID;
                        newRow.Cells[1].Value = value.SizeName;
                    }
                }
                else {
                    throw new Exception(string.Format("O Artigo {0} não foi encontrado!", itemId));
                }
            }
        }

        /// <summary>
        /// Limpar o form
        /// </summary>
        private void ItemClear(bool clearItemId) {
            //Limpar
            if (clearItemId) {
                txtItemId.Text = string.Empty;
            }

            dgvColor.Rows.Clear();
            dgvSize.Rows.Clear();

            txtItemDescription.Text = string.Empty;
            txtItemShortDescription.Text = string.Empty;
            numItemPriceTaxIncluded.Value = 0;
            txtItemComments.Text = string.Empty;
        }
        #endregion

        #region CUSTOMER

        /// <summary>
        /// Gravar (inserir ou alterar) um cliente
        /// </summary>
        /// <param name="customerId"></param>
        private void CustomerUpdate(double customerId, bool isNew) {
            S50cBO22.Customer myCustomer = null;

            //Ler da BD se não for novo
            myCustomer = dsoCache.CustomerProvider.GetCustomer(customerId);
            if (myCustomer == null && !isNew) {
                throw new Exception(string.Format("O cliente [{0}] não existe.", customerId));
            }
            else if (myCustomer != null && isNew) {
                throw new Exception(string.Format("O cliente [{0}] já existe.", customerId));
            }

            if (myCustomer == null) {
                // Cliente NOVO
                // Obter um novo Id
                myCustomer = new S50cBO22.Customer();
                myCustomer.CustomerID = (double)numCustomerId.Value;
                // Colocar credito por limite de valor para não bloquear o cliente
                myCustomer.LimitType = CustomerLimitType.ltValue;
            }

            myCustomer.OrganizationName = txtCustomerName.Text;
            myCustomer.FederalTaxId = txtCustomerTaxId.Text;
            myCustomer.Comments = txtCustomerComments.Text;
            //
            if (cmbCustomerTax.SelectedItem != null) {
                var entityFiscalStatus = (EntityFiscalStatus)cmbCustomerTax.SelectedItem;
                myCustomer.EntityFiscalStatusID = entityFiscalStatus.EntityFiscalStatusID;
            }
            myCustomer.SalesmanId = (int)numCustomerSalesmanId.Value;
            if (cmbCustomerCurrency.SelectedValue != null) {
                myCustomer.CurrencyID = (string)cmbCustomerCurrency.SelectedValue;
            }
            myCustomer.ZoneID = (short)numCustomerZoneId.Value;
            if (cmbCustomerCountry.SelectedItem != null) {
                myCustomer.CountryID = ((CountryCode)cmbCustomerCountry.SelectedItem).CountryID;
            }
            //
            // Outros campos obrigatórios
            myCustomer.CarrierID = dsoCache.CarrierProvider.GetFirstCarrierID();
            myCustomer.TenderID = dsoCache.TenderProvider.GetFirstTenderCash();
            myCustomer.CurrencyID = cmbCustomerCurrency.Text;

            // Se a zone estiver vazia, considerar a primeira zona nacional
            if (myCustomer.ZoneID == 0) {
                myCustomer.ZoneID = dsoCache.ZoneProvider.FindZone(ZoneTypeEnum.ztNational);
            }
            // Se o modo de pagamento estiver vazio, obter o primeiro disponivel
            if (myCustomer.PaymentID == 0) {
                myCustomer.PaymentID = dsoCache.PaymentProvider.GetFirstID();
            }
            // Se o vendedor não existir, utilizar o primeiro disponivel
            if (!dsoCache.SalesmanProvider.SalesmanExists(myCustomer.SalesmanId)) {
                myCustomer.SalesmanId = (int)dsoCache.SalesmanProvider.GetFirstSalesmanID();
            }
            // Se o pais não existir, rectificar
            if (!dsoCache.CountryProvider.CountryExists(myCustomer.CountryID)) {
                myCustomer.CountryID = systemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID;
            }
            // Se a moeda não existir, guar a moeda base
            if (!dsoCache.CurrencyProvider.CurrencyExists(myCustomer.CurrencyID)) {
                myCustomer.CurrencyID = systemSettings.BaseCurrency.CurrencyID;
            }

            // Gravar. Se for novo NewRec = true;
            dsoCache.CustomerProvider.Save(myCustomer, myCustomer.CustomerID, isNew);
            //
            CustomerClear();
        }

        /// <summary>
        /// Ler um cliente da base de dados e apresentá-lo no ecran
        /// </summary>
        /// <param name="customerId"></param>
        private void CustomerGet(double customerId) {
            CustomerClear();
            var customer = dsoCache.CustomerProvider.GetCustomer(customerId);
            if (customer != null) {
                numCustomerId.Value = (decimal)customerId;
                numCustomerSalesmanId.Value = customer.SalesmanId;
                numCustomerZoneId.Value = customer.ZoneID;

                cmbCustomerCountry.SelectedItem = cmbCustomerCountry.Items.Cast<CountryCode>().FirstOrDefault(x => x.CountryID == customer.CountryID);
                cmbCustomerCurrency.SelectedItem = cmbCustomerCurrency.Items.Cast<CurrencyDefinition>().FirstOrDefault(x => x.CurrencyID == customer.CurrencyID);
                cmbCustomerTax.SelectedItem = cmbCustomerTax.Items.Cast<EntityFiscalStatus>().FirstOrDefault(x => x.EntityFiscalStatusID == customer.EntityFiscalStatusID);

                txtCustomerComments.Text = customer.Comments;
                txtCustomerName.Text = customer.OrganizationName;
                txtCustomerTaxId.Text = customer.FederalTaxId;
            }
            else {
                //O cliente não existe!
                throw new Exception(string.Format("O Cliente {0} não foi encontrado!", customerId));
            }
        }

        /// <summary>
        /// Apagar um cliente
        /// </summary>
        /// <param name="customerId"></param>
        private void CustomerRemove(double customerId) {
            dsoCache.CustomerProvider.Delete(customerId);
            CustomerClear();
        }

        private void CustomerClear() {
            // Obter um novo ID (para um novo cliente)
            numCustomerId.Value = (decimal)dsoCache.CustomerProvider.GetNewID();
            //
            txtCustomerComments.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtCustomerTaxId.Text = string.Empty;
            numCustomerSalesmanId.Value = 0;

            UIUtils.FillCountryCombo(cmbCustomerCountry);
            var country = cmbCustomerCountry.Items.Cast<CountryCode>()
                                            .FirstOrDefault(x => x.CountryID.Equals(systemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID, StringComparison.CurrentCultureIgnoreCase));
            cmbCustomerCountry.SelectedItem = country;
            //
            UIUtils.FillCurrencyCombo(cmbCustomerCurrency);
            var currency = cmbCustomerCurrency.Items.Cast<CurrencyDefinition>()
                                              .FirstOrDefault(x => x.CurrencyID.Equals(systemSettings.BaseCurrency.CurrencyID, StringComparison.CurrentCultureIgnoreCase));
            cmbCustomerCurrency.SelectedItem = currency;
            //
            UIUtils.FillEntityFiscalStatusCombo(cmbCustomerTax);
            cmbCustomerTax.SelectedItem = cmbCustomerTax.Items.Cast<EntityFiscalStatus>().FirstOrDefault(x => x.EntityFiscalStatusID == APIEngine.SystemSettings.SystemInfo.PartySettings.SystemFiscalStatusID);
            if (cmbCustomerTax.SelectedItem == null && cmbCustomerTax.Items.Count > 0) {
                cmbCustomerTax.SelectedIndex = 0;
            }
        }

        #endregion

        #region SUPPLIER

        private void SupplierGet(double supplierId) {
            var supplier = dsoCache.SupplierProvider.GetSupplier(supplierId);
            if (supplier != null) {
                txtSupplierComments.Text = supplier.Comments;
                txtSupplierCountry.Text = supplier.CountryID;
                txtSupplierCurrency.Text = supplier.CurrencyID;
                txtSupplierId.Text = supplier.SupplierID.ToString();
                txtSupplierName.Text = supplier.OrganizationName;
                txtSupplierTaxId.Text = supplier.FederalTaxId;

                cmbSupplierTax.SelectedItem = cmbSupplierTax.Items.Cast<EntityFiscalStatus>().FirstOrDefault(x => x.EntityFiscalStatusID == supplier.EntityFiscalStatusID);
                txtSupplierZone.Text = supplier.ZoneID.ToString();
            }
            else {
                SupplierClear();
            }
        }

        private void SupplierUpdate(double supplierId, bool isNew) {
            S50cBO22.Supplier supplier = null;

            if (isNew && dsoCache.SupplierProvider.SupplierExists(supplierId)) {
                throw new Exception(string.Format("O fornecedor [{0}] já existe.", supplierId));
            }
            if (!isNew) {
                supplier = dsoCache.SupplierProvider.GetSupplier(supplierId);
                if (supplier == null && !isNew) {
                    throw new Exception(string.Format("O fornecedor [{0}] não existe.", supplierId));
                }
            }
            //
            if (supplier == null) {
                // Como o fornecedor não existe na base de dados, vamos criar um novo
                supplier = new S50cBO22.Supplier();
                supplier.SupplierID = supplierId;
            }
            supplier.Comments = txtSupplierComments.Text;
            supplier.CountryID = txtSupplierCountry.Text;
            supplier.CurrencyID = txtSupplierCurrency.Text;
            supplier.OrganizationName = txtSupplierName.Text;
            supplier.FederalTaxId = txtSupplierTaxId.Text;

            if (cmbSupplierTax.SelectedIndex >= 0) {
                var entityFiscalStatus = (EntityFiscalStatus)cmbSupplierTax.SelectedItem;
                supplier.EntityFiscalStatusID = entityFiscalStatus.EntityFiscalStatusID;
            }

            supplier.ZoneID = short.Parse(txtSupplierZone.Text);
            //
            //  A forma de pagamento é obrigatória. Vamos usar a primeira disponivel.
            supplier.PaymentID = dsoCache.PaymentProvider.GetFirstID();
            //  O meio de pagamento é obrigatório. VAmos usar o primeiro disponivel em numerário.
            supplier.TenderID = dsoCache.TenderProvider.GetFirstTenderCash();

            dsoCache.SupplierProvider.Save(supplier, supplier.SupplierID, isNew);

            SupplierClear();
        }

        private void SupplierRemove(double supplierId) {
            dsoCache.SupplierProvider.Delete(supplierId);
            SupplierClear();
        }

        private void SupplierClear() {
            txtSupplierComments.Text = string.Empty;
            txtSupplierCountry.Text = systemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID;
            txtSupplierCurrency.Text = systemSettings.BaseCurrency.CurrencyID;
            txtSupplierId.Text = dsoCache.SupplierProvider.GetNewID().ToString();
            txtSupplierName.Text = string.Empty;
            txtSupplierTaxId.Text = "0";
            txtSupplierZone.Text = dsoCache.ZoneProvider.GetFirstID().ToString();

            UIUtils.FillEntityFiscalStatusCombo(cmbSupplierTax);
            cmbSupplierTax.SelectedItem = cmbSupplierTax.Items.Cast<EntityFiscalStatus>().FirstOrDefault(x => x.EntityFiscalStatusID == APIEngine.SystemSettings.SystemInfo.PartySettings.SystemFiscalStatusID);
            if (cmbSupplierTax.SelectedItem == null && cmbSupplierTax.Items.Count > 0) {
                cmbSupplierTax.SelectedIndex = 0;
            }
        }

        #endregion

        #region Unit of measure

        private void UnitOfMeasureGet(string unitOfMeasureId) {
            UnitOfMeasureClear();
            var unit = dsoCache.UnitOfMeasureProvider.GetUnitOfMeasure(unitOfMeasureId);
            if (unit != null) {
                txtUnitOfMeasureId.Text = unitOfMeasureId;
                txtUnitOfMeasureName.Text = unit.Description;
            }

        }

        private void UnitOfMeasureUpdate(string unitOfMeasureId, bool isNew) {
            UnitOfMeasure myUnit = null;
            if (!isNew) {
                myUnit = dsoCache.UnitOfMeasureProvider.GetUnitOfMeasure(unitOfMeasureId);
            }
            if (myUnit == null && !isNew) {
                throw new Exception(string.Format("A unidade de medida [{0}] não existe.", unitOfMeasureId));
            }
            if (myUnit == null) {
                myUnit = new UnitOfMeasure();
                myUnit.UnitOfMeasureID = unitOfMeasureId;
            }
            myUnit.Description = txtUnitOfMeasureName.Text;
            dsoCache.UnitOfMeasureProvider.Save(myUnit, myUnit.UnitOfMeasureID, isNew);

            UnitOfMeasureClear();
        }


        private void UnitOfMeasureRemove(string unitOfMeasureId) {
            dsoCache.UnitOfMeasureProvider.Delete(unitOfMeasureId);
            UnitOfMeasureClear();
        }

        private void UnitOfMeasureClear() {
            // Obter o próximo ID
            txtUnitOfMeasureId.Text = string.Empty;
            txtUnitOfMeasureName.Text = string.Empty;
        }

        #endregion

        #region Buy/Sale TRANSACTION

        private TransactionID TransactionRemove() {
            TransactionID transId = null;
            string transDoc = txtTransDoc.Text;
            string transSerial = txtTransSerial.Text;
            double transDocNumber = 0;
            double.TryParse(txtTransDocNumber.Text, out transDocNumber);
            bool result = false;

            var transType = ItemTransactionHelper.TransGetType(transDoc);

            if (rbTransBuySell.Checked) {
                if (transType != DocumentTypeEnum.dcTypeSale && transType != DocumentTypeEnum.dcTypePurchase) {
                    throw new Exception("O documento indicado não é um documento de compra ou venda.");
                }
                if (bsoItemTransaction.LoadItemTransaction(transType, transSerial, transDoc, transDocNumber)) {
                    // O motivo de anulação deve ser sempre preenchido.
                    // Se for obrigatório, o documento não é anulado sem que esteja preenchido
                    bsoItemTransaction.Transaction.VoidMotive = "Anulado por: " + Application.ProductName;
                    //
                    result = bsoItemTransaction.DeleteItemTransaction(false);
                    if (result) {
                        transId = bsoItemTransaction.Transaction.TransactionID;
                    }
                    else {
                        throw new Exception(string.Format("Não foi possível anular o documento {0} {1}/{2}", transDoc, transSerial, transDocNumber));
                    }
                }
                else {
                    throw new Exception(string.Format("Não foi possível carregar o documento {0} {1}/{2}.", transDoc, transSerial, transDocNumber));
                }
            }
            else {
                if (transType != DocumentTypeEnum.dcTypeStock) {
                    throw new Exception("O documento indicado não é um documento de stock.");
                }
                var loaded = bsoStockTransaction.LoadStockTransaction(transType, transSerial, transDoc, transDocNumber);
                if (loaded) {
                    // O motivo de anulação deve ser sempre preenchido.
                    // Se for obrigatório, o documento não é anulado sem que esteja preenchido
                    bsoStockTransaction.Transaction.VoidMotive = "Anulado por: " + Application.ProductName;
                    //
                    result = bsoStockTransaction.DeleteStockTransaction();
                    if (result) {
                        transId = new TransactionID();
                        transId.TransSerial = transSerial;
                        transId.TransDocument = transDoc;
                        transId.TransDocNumber = transDocNumber;
                    }
                    else {
                        throw new Exception(string.Format("Não foi possível anular o documento {0} {1}/{2}", transDoc, transSerial, transDocNumber));
                    }
                }
                else {
                    throw new Exception("O documento indicado não existe.");
                }
            }
            return transId;
        }

        /// <summary>
        /// Inserir ou Actualizar uma transação na base dados
        /// </summary>
        /// <returns></returns>
        private TransactionID TransactionInsert(bool suspendTransaction) {
            string transDoc = txtTransDoc.Text;
            string transSerial = txtTransSerial.Text;
            double transDocNumber = 0;
            double.TryParse(txtTransDocNumber.Text, out transDocNumber);

            TransactionID result = null;
            if (rbTransBuySell.Checked) {
                result = TransactionUpdate(transSerial, transDoc, transDocNumber, true, suspendTransaction);
            }
            else {
                result = TransactionStockUpdate(transSerial, transDoc, transDocNumber, true);
            }

            return result;
        }

        private TransactionID TransactionEdit(bool suspendedTransaction) {
            string transDoc = txtTransDoc.Text;
            string transSerial = txtTransSerial.Text;
            double transDocNumber = 0;
            double.TryParse(txtTransDocNumber.Text, out transDocNumber);

            TransactionID result = null;
            if (rbTransBuySell.Checked) {
                result = TransactionUpdate(transSerial, transDoc, transDocNumber, false, suspendedTransaction);
            }
            else {
                result = TransactionStockUpdate(transSerial, transDoc, transDocNumber, false);
            }
            return result;
        }

        /// <summary>
        /// Insere ou altera uma transação (compra/venda)
        /// </summary>
        /// <param name="transSerial">Série</param>
        /// <param name="transDoc">Documento</param>
        /// <param name="transDocNumber">Número do documento</param>
        /// <param name="newTransaction">true: Nova transação (inserir); false: transação existente (alterar)</param>
        /// <returns>TransactionId da transação inserida/alterada</returns>
        /// 
        private TransactionID TransactionUpdate(string transSerial, string transDoc, double transDocNumber, bool newTransaction, bool suspendTransaction) {

            TransactionID insertedTrans = null;
            transactionError = false;

            try {
                BSOItemTransactionDetail BSOItemTransDetail = null;

                //'-------------------------------------------------------------------------
                //' DOCUMENT HEADER and initialization
                //'-------------------------------------------------------------------------
                //'*** Total source document amount. Save to verify at the end if an adjustment is necessary
                //'OriginalDocTotalAmount = 10
                //'
                // Documento
                if (!systemSettings.WorkstationInfo.Document.IsInCollection(transDoc)) {
                    throw new Exception("O documento não se encontra preenchido ou não existe");
                }
                Document doc = systemSettings.WorkstationInfo.Document[transDoc];
                // Série
                if (!systemSettings.DocumentSeries.IsInCollection(transSerial)) {
                    throw new Exception("A série não se encontra preenchida ou não existe");
                }
                DocumentsSeries series = systemSettings.DocumentSeries[transSerial];
                //if (series.SeriesType != SeriesTypeEnum.SeriesExternal) {
                //    throw new Exception("Para lançamentos de documentos externos à aplicação apenas são permitidas séries externas.");
                //}
                //
                var transType = ItemTransactionHelper.TransGetType(transDoc);
                if (transType != DocumentTypeEnum.dcTypeSale && transType != DocumentTypeEnum.dcTypePurchase) {
                    throw new Exception(string.Format("O documento indicado [{0}] não é um documento de venda/compra", transDoc));
                }
                //
                if (!newTransaction && !suspendTransaction) {
                    //Exemplo: Verificar se uma transação existe:
                    if (!dsoCache.ItemTransactionProvider.ItemTransactionExists(transSerial, transDoc, transDocNumber)) {
                        throw new Exception(string.Format("O documento {0} {1}/{2} não existe para ser alterado. Deve criar um novo.", transDoc, transSerial, transDocNumber));
                    }
                }
                //
                // Motor do documento
                bsoItemTransaction.TransactionType = transType;
                // Motor dos detalhes (linhas)
                BSOItemTransDetail = new BSOItemTransactionDetail();
                BSOItemTransDetail.TransactionType = transType;
                // Utilizador e permissões
                BSOItemTransDetail.UserPermissions = systemSettings.User;
                BSOItemTransDetail.PermissionsType = FrontOfficePermissionEnum.foPermByUser;
                //
                bsoItemTransaction.BSOItemTransactionDetail = BSOItemTransDetail;
                BSOItemTransDetail = null;
                //
                // Terceiro
                double partyId = 0;
                double.TryParse(txtTransPartyId.Text, out partyId);
                //
                //Inicializar uma transação
                bsoItemTransaction.Transaction = new ItemTransaction();
                if (newTransaction) {
                    bsoItemTransaction.InitNewTransaction(transDoc, transSerial);
                    if (transDocNumber > 0) {
                        // Tentar numeração indicada
                        bsoItemTransaction.Transaction.TransDocNumber = transDocNumber;
                    }
                }
                else {
                    if (suspendTransaction) {
                        //NOTA:
                        // transDocNumber=número da transação suspensa. Não número final
                        if (!bsoItemTransaction.LoadSuspendedTransaction(transSerial, transDoc, transDocNumber)) {
                            throw new Exception(string.Format("O documento {0} {1}/{2} não existe para ser alterado. Deve criar um novo.", transDoc, transSerial, transDocNumber));
                        }
                    }
                    else {
                        bsoItemTransaction.LoadItemTransaction(transType, transSerial, transDoc, transDocNumber);
                    }
                }
                bsoItemTransaction.UserPermissions = systemSettings.User;

                ItemTransaction trans = bsoItemTransaction.Transaction;
                if (trans == null) {
                    if (newTransaction) {
                        throw new Exception(string.Format("Não foi possível inicializar o documento [{0}] da série [{1}]", transDoc, transSerial));
                    }
                    else {
                        throw new Exception(string.Format("Não foi possível carregar o documento [{0}] da série [{1}] número [{2}]", transDoc, transSerial, transDocNumber));
                    }
                }
                //
                // Limpar todas as linhas
                int i = 1;
                while (trans.Details.Count > 0) {
                    trans.Details.Remove(ref i);
                }
                //
                //// Definir o terceiro (cliente ou fornecedor)
                bsoItemTransaction.PartyID = partyId;
                //
                //To use an EXISTING party address:
                //bsoItemTransaction.PartyAddressByKey = 33 // Specify Address ID; Address Id 33 must exists
                //bsoItemTransaction.PartyAddressID=33      // Specify the Index of: trans.Party.PartyInfo.AddressList; Index 33 must exist
                //
                // To manually specify an address:
                //bsoItemTransaction.PartyFederalTaxID = "123456789";
                //bsoItemTransaction.PartyAddressLine1 = "Rua 1";
                //bsoItemTransaction.PartyPostalCode = "4000 Porto";
                //                
                //
                //Descomentar para indicar uma referência externa ao documento:
                //trans.ContractReferenceNumber = ExternalDocId;
                //
                //Set Create date and deliverydate
                var createDate = DateTime.Today;
                var createTime = DateTime.Today;
                DateTime.TryParse(txtTransDate.Text, out createDate);
                DateTime.TryParse(txtTransTime.Text, out createTime);

                trans.CreateDate = createDate;
                trans.CreateTime = createTime;

                trans.ActualDeliveryDate = createDate;

                //
                // Definir se o imposto é incluido
                trans.TransactionTaxIncluded = chkTransTaxIncluded.Checked;
                //
                // Definir o pagamento. Neste caso optou-se por utilizar o primeiro pagamento disponivel na base de dados

                short PaymentId = 0;
                short.TryParse(txtPaymentID.Text, out PaymentId);
                if (PaymentId == 0) {
                    PaymentId = dsoCache.PaymentProvider.GetFirstID();
                }
                trans.Payment = dsoCache.PaymentProvider.GetPayment(PaymentId);

                trans.ATCUD = txtAtcud.Text;
                trans.QRCode = txtQrCode.Text;
                //
                //*** Locais de carga e descarga
                //// Descomentar o seguinte para carregar um local de descarga "livre"
                //var placeId = S50cAPIEngine.DSOCache.LoadUnloadPlaceProvider.FindForAddressType(LoadUnloadAddressTypes.luatFree);
                //if( placeId == 0) {
                //    // Não existe nenhum local de carga/descar com endereços livres, por isso vamos criar um
                //    var freePlace = new LoadUnloadPlace() {
                //        AddressType = LoadUnloadAddressTypes.luatFree,
                //        Description = "Livre",
                //        LoadUnloadPlaceID = S50cAPIEngine.DSOCache.LoadUnloadPlaceProvider.GetNewID()
                //    };
                //    S50cAPIEngine.DSOCache.LoadUnloadPlaceProvider.Save(freePlace, freePlace.LoadUnloadPlaceID, true);
                //    placeId = freePlace.LoadUnloadPlaceID;
                //}
                ////
                //// Vamos definir o local de descarga
                //bsoItemTransaction.UnloadPlaceID = placeId;
                //bsoItemTransaction.Transaction.UnloadPlaceAddress.AddressLine1 = "Edifício Olympus II ";
                //bsoItemTransaction.Transaction.UnloadPlaceAddress.AddressLine2 = "Avenida D. Afonso Henriques, 1462 - 2º";
                //bsoItemTransaction.Transaction.UnloadPlaceAddress.PostalCode = "4450-013 Matosinhos";
                ////// Para local de carga, usar
                ////bsoItemTransaction.LoadPlaceID = placeId;
                ////bsoItemTransaction.Transaction.LoadPlaceAddress.AddressLine1 = "Edifício Olympus II ";
                ////bsoItemTransaction.Transaction.LoadPlaceAddress.AddressLine2 = "Avenida D. Afonso Henriques, 1462 - 2º";
                ////bsoItemTransaction.Transaction.LoadPlaceAddress.PostalCode = "4450-013 Matosinhos";
                //
                //Modo de Pagamento
                //se não preencheu nem tem cliente sugere o primeiro TenderID
                short tenderID = 0;
                short.TryParse(txtTenderID.Text, out tenderID);
                if (partyId == 0) {
                    if (tenderID == 0) {
                        tenderID = dsoCache.TenderProvider.GetFirstID();
                    }
                    trans.Tender.TenderID = tenderID;
                }

                //
                //ID da  Session
                short sessionID = APIEngine.SystemSettings.TillSession.SessionID;

                trans.WorkstationStamp.SessionID = sessionID;
                //

                if (string.IsNullOrEmpty(txtTransCurrency.Text)) {
                    trans.BaseCurrency = systemSettings.BaseCurrency;
                }
                else {
                    CurrencyDefinition currency = new CurrencyDefinition();
                    currency = dsoCache.CurrencyProvider.GetCurrency(txtTransCurrency.Text);
                    if (currency == null) {
                        throw new Exception(string.Format("A moeda[{0}] não existe.", txtTransCurrency.Text));
                    }
                    else {
                        trans.BaseCurrency = dsoCache.CurrencyProvider.GetCurrency(txtTransCurrency.Text);
                    }
                }
                //
                // Comentários / Observações
                trans.Comments = "Gerado por " + Application.ProductName;
                //
                //-------------------------------------------------------------------------
                // DOCUMENT DETAILS
                //-------------------------------------------------------------------------
                string itemId = txtItemId.Text;
                //
                //Adicionar a primeira linha ao documento
                double qty = 0; double.TryParse(txtTransQuantityL1.Text, out qty);
                double unitPrice = 0; double.TryParse(txtTransUnitPriceL1.Text, out unitPrice);
                double taxPercent = 0; double.TryParse(txtTransTaxRateL1.Text, out taxPercent);
                short wareHouseId = 0; short.TryParse(txtTransWarehouseL1.Text, out wareHouseId);
                Item item = TransGetCreateItem(txtTransItemL1.Text, string.Empty, string.Empty, txtTransUnL1.Text, string.Empty, 1, false, false, 0, 0, taxPercent);
                //Alterar Eco taxa aqui
                //item.ItemTax = 1.3;
                //item.ItemTax2 = 0;
                //item.ItemTax3 = 0;
                short colorId = 0;
                short.TryParse(txtTransColor1.Text, out colorId);
                short sizeId = 0;
                short.TryParse(txtTransSize1.Text, out sizeId);
                string serialNumber = txtTransPropValueL1.Text;
                var currentDate = DateTime.Today;
                //
                TransAddDetail(trans, item, qty, txtTransUnL1.Text, unitPrice, taxPercent, wareHouseId, colorId, sizeId, lblTransPropNameL1.Text, serialNumber);
                //
                //Adicionar a segunda linha ao documento
                if (!string.IsNullOrEmpty(txtTransItemL2.Text)) {
                    qty = 0; double.TryParse(txtTransQuantityL2.Text, out qty);
                    unitPrice = 0; double.TryParse(txtTransUnitPriceL2.Text, out unitPrice);
                    taxPercent = 0; double.TryParse(txtTransTaxRateL2.Text, out taxPercent);
                    wareHouseId = 0; short.TryParse(txtTransWarehouseL2.Text, out wareHouseId);
                    item = TransGetCreateItem(txtTransItemL2.Text, string.Empty, string.Empty, txtTransUnL2.Text, string.Empty, 1, false, false, 0, 0, taxPercent);
                    //Alterar Eco taxa aqui
                    //item.ItemTax = 1.3;
                    //item.ItemTax2 = 0;
                    //item.ItemTax3 = 0;
                    colorId = 0;
                    short.TryParse(txtTransColor1.Text, out colorId);
                    sizeId = 0;
                    short.TryParse(txtTransSize1.Text, out sizeId);
                    serialNumber = txtTransPropValueL2.Text;
                    currentDate = DateTime.Today;
                    //
                    TransAddDetail(trans, item, qty, txtTransUnL2.Text, unitPrice, taxPercent, wareHouseId, colorId, sizeId, lblTransPropNameL2.Text, serialNumber);
                }
                //*** Descomentar a linha seguinte para definir automaticamente as origens (conversão de um documento)
                //bsoItemTransaction.FillTransactionOrigins()

                // Desconto Global -- Atribuir só no fim do documento depois de adicionadas todas as linhas
                double globalDiscount = 0;
                double.TryParse(txtTransGlobalDiscount.Text, out globalDiscount);
                bsoItemTransaction.PaymentDiscountPercent1 = globalDiscount;

                //Calcular todo o documento
                bsoItemTransaction.Calculate(true, true);
                //
                //*** Descomentar o seguinte para ajustar o total do documento (arredondamentos)
                //double OriginalDocTotalAmount = 9999; //Ajustar para o valor do documento original
                //if( OriginalDocTotalAmount != 0 ){
                //    double TotalDiff = OriginalDocTotalAmount - bsoItemTransaction.Transaction.TotalAmount;
                //    if( TotalDiff != 0 ){
                //        bsoItemTransaction.Transaction.TotalAdjustmentAmount = TotalDiff;
                //        bsoItemTransaction.Transaction.TotalAmount = bsoItemTransaction.Transaction.TotalAmount + TotalDiff;
                //        bsoItemTransaction.Transaction.TotalTransactionAmount = bsoItemTransaction.Transaction.TotalTransactionAmount + TotalDiff;
                //    }
                //}
                //
                //// Exemplo de pagamento por cheque...
                //// Gerar a linha do pagamento
                //var tenderCheck = dsoCache.TenderProvider.GetFirstTenderType(TenderTypeEnum.tndCheck);
                //if ( tenderCheck != null ) {
                //    // Preencher o cheque
                //    TenderCheck tCheck = new TenderCheck() {
                //        BankID = "AAA",                             // Código do banco! TempItemTransaction decimal existir
                //        CheckAmount = trans.TotalTransactionAmount, // Pagar na totalidade
                //        CheckDeferredDate = trans.CreateDate,       // Data do cheque
                //        CheckSequenceNumber = "987654321",          // Númerom do cheque
                //        TillID = trans.Till.TillID,                 // Caixa
                //        Guid = Guid.NewGuid().ToString()            // Guid identificador do registo
                //    };
                //    // Preencher a linha do pagamento
                //    var tenderLine = new TenderLineItem();
                //    tenderLine.Amount = trans.TotalTransactionAmount;
                //    tenderLine.CreateDate = trans.CreateDate;
                //    tenderLine.PartyID = trans.PartyID;
                //    tenderLine.PartyTypeCode = trans.PartyTypeCode;
                //    tenderLine.Tender = tenderCheck;
                //    tenderLine.TenderCheck = tCheck;

                //    trans.TenderLineItem.Add(tenderLine);
                //}
                //

                //// Exemplo para registar a origem nas Notas de crédito e Notas de débito:
                //if (doc.Nature.NatureID == TransactionNatureEnum.Sale_CreditNote || doc.Nature.NatureID == TransactionNatureEnum.Sale_DebitNote) {
                //    var originTransId = new TransactionID();
                //    originTransId.Init("1","FAC",1);
                //    trans.OriginatingON = originTransId.ToString();
                //}

                // Definir a assinatura de um sistema externo
                if (series.SeriesType == SeriesTypeEnum.SeriesExternal) {
                    if (!SetExternalSignature(trans)) {
                        MessageBox.Show("A assinatura não foi definida. Vão ser usados valores por omissão", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                if (suspendTransaction) {
                    insertedTrans = bsoItemTransaction.SuspendCurrentTransaction();
                }
                else {
                    //Exemplo da Repartição de Custos
                    //INICIO
                    if (APIEngine.SystemSettings.SpecialConfigs.UpdateItemCostWithFreightAmount) {

                        bsoItemTransaction.Transaction.BuyShareOtherCostList = null;
                        SimpleDocument objDocument;
                        SimpleDocumentList objDocumentList = new SimpleDocumentList();
                        SimpleItemDetail objDocumentDetailsList;

                        double Convert_Double = 0;


                        //Begin manual Share amount 
                        if (txtShareTransDocNumber_R1.Text.Length > 0) {
                            objDocument = new SimpleDocument();
                            objDocument.TransSerial = txtShareTransSerial_R1.Text;
                            objDocument.TransDocument = txtShareTransDocument_R1.Text;
                            double.TryParse(txtShareTransDocNumber_R1.Text, out Convert_Double);
                            objDocument.TransDocNumber = Convert_Double;
                            double.TryParse(txtShareAmount_R1.Text, out Convert_Double);
                            objDocument.TotalTransactionAmount = Convert_Double;
                            objDocument.CurrencyID = txtTransCurrency.Text;
                            objDocument.CurrencyExchange = 1;
                            objDocument.CurrencyFactor = 1;


                            //ADD Line 1
                            if (txtAmout_R1_L1.Text.Length > 0) {
                                objDocumentDetailsList = new SimpleItemDetail();
                                objDocumentDetailsList.DestinationTransSerial = txtShareTransSerial_R1.Text;
                                objDocumentDetailsList.DestinationTransDocument = txtShareTransDocument_R1.Text;
                                double.TryParse(txtShareTransDocNumber_R1.Text, out Convert_Double);
                                objDocumentDetailsList.DestinationTransDocNumber = Convert_Double;
                                objDocumentDetailsList.DestinationLineItemID = 1;
                                objDocumentDetailsList.ItemID = LblL1.Text;
                                double.TryParse(txtAmout_R1_L1.Text, out Convert_Double);
                                objDocumentDetailsList.UnitPrice = Convert_Double;
                                objDocumentDetailsList.Quantity = 1;
                                //Line KEY
                                objDocumentDetailsList.ItemSearchKey = objDocumentDetailsList.DestinationTransSerial + "|" + objDocumentDetailsList.DestinationTransDocument + "|" + objDocumentDetailsList.DestinationTransDocNumber.ToString() + "|" + Convert.ToString(objDocumentDetailsList.DestinationLineItemID) + "|" + objDocumentDetailsList.ItemID + "|" + objDocumentDetailsList.Color.ColorID + "|" + objDocumentDetailsList.Size.SizeID;
                                //Add Line 1 to document detail
                                objDocument.Details.Add(objDocumentDetailsList);
                            }

                            //ADD Line 2
                            if (txtAmout_R1_L2.Text.Length > 0) {
                                objDocumentDetailsList = new SimpleItemDetail();
                                objDocumentDetailsList.DestinationTransSerial = txtShareTransSerial_R1.Text;
                                objDocumentDetailsList.DestinationTransDocument = txtShareTransDocument_R1.Text;
                                double.TryParse(txtShareTransDocNumber_R1.Text, out Convert_Double);
                                objDocumentDetailsList.DestinationTransDocNumber = Convert_Double;
                                objDocumentDetailsList.DestinationLineItemID = 2;
                                objDocumentDetailsList.ItemID = LblL2.Text;
                                double.TryParse(txtAmout_R1_L2.Text, out Convert_Double);
                                objDocumentDetailsList.UnitPrice = Convert_Double;
                                objDocumentDetailsList.Quantity = 1;
                                //Line KEY
                                objDocumentDetailsList.ItemSearchKey = objDocumentDetailsList.DestinationTransSerial + "|" + objDocumentDetailsList.DestinationTransDocument + "|" + objDocumentDetailsList.DestinationTransDocNumber.ToString() + "|" + Convert.ToString(objDocumentDetailsList.DestinationLineItemID) + "|" + objDocumentDetailsList.ItemID + "|" + objDocumentDetailsList.Color.ColorID + "|" + objDocumentDetailsList.Size.SizeID;
                                //Add Line 2 to document detail
                                objDocument.Details.Add(objDocumentDetailsList);

                                //Add Document to list of Documento to Share amount 
                            }

                            objDocumentList.Add(objDocument);

                        }
                        //End manual Share amount 

                        //Begin Automatic Share amount 
                        //if it does not have details, divide the value in proportion to the value of the line
                        if (txtShareTransDocNumber_R2.Text.Length > 0) {
                            objDocument = new SimpleDocument();
                            objDocument.TransSerial = txtShareTransSerial_R2.Text;
                            objDocument.TransDocument = txtShareTransDocument_R2.Text;
                            double.TryParse(txtShareTransDocNumber_R2.Text, out Convert_Double);
                            objDocument.TransDocNumber = Convert_Double;
                            double.TryParse(txtShareAmount_R2.Text, out Convert_Double);
                            objDocument.TotalTransactionAmount = Convert_Double;
                            objDocument.CurrencyID = txtTransCurrency.Text;

                            //Add Document to list of Documento to Share amount 
                            objDocumentList.Add(objDocument);

                        }
                        //End Automatic Share amount 

                        // Add Shares amount cost to  Transaction , BuyShareOtherCostList 
                        bsoItemTransaction.Transaction.BuyShareOtherCostList = objDocumentList;

                    }
                    //FIM

                    // Abrir automaticamente o caixa, se estiver fechar
                    bsoItemTransaction.EnsureOpenTill(bsoItemTransaction.Transaction.Till.TillID);
                    //
                    bsoItemTransaction.SaveDocument(false, false);
                    //
                    if (!transactionError) {
                        insertedTrans = bsoItemTransaction.Transaction.TransactionID;
                    }
                }
                //
                BSOItemTransDetail = null;
            }
            catch (Exception ex) {
                throw ex;
            }
            finally {
                //Unsubscribe from event
                bsoItemTransaction.TenderIDChanged -= bsoItemTransaction_TenderIDChanged;
            }

            TransactionPrint2(bsoItemTransaction.Transaction.TransSerial, bsoItemTransaction.Transaction.TransDocument, bsoItemTransaction.Transaction.TransDocNumber);

            return insertedTrans;
        }

        void bsoItemTransaction_TenderIDChanged(ref short value) {
            MessageBox.Show("bsoItemTransaction_TenderIDChanged");
        }

        /// <summary>
        /// Obtém ou cria um artigo novo e devolve-o
        /// </summary>
        /// <param name="itemId">Código do artigo</param>
        /// <param name="EANBarcode">Código de barras do artigo para a criação pode ser vazio.</param>
        /// <param name="itemDescription">Descrição do artigo para a criação. Pode ser vazio.</param>
        /// <param name="unitId">Unidade para a criação pode ser vazio.</param>
        /// <param name="packUnitId">Unidade de grupo (pack) para a crição. Se fornecida, deve também ser indicado o fator (unitsPerPack)</param>
        /// <param name="unitsPerPack">Factor de agrupamento ou número de unidades por pack. Se não fornecido, indicar 1</param>
        /// <param name="isKg">Indica se é uma unidade de peso (Kg)</param>
        /// <param name="isPack">Indica se é um pack</param>
        /// <param name="supplierId">Identificado do fornecedor para a criação. Obrigatório para criar um artigo novo.</param>
        /// <param name="unitCostPrice">Custo por unidade do artigo</param>
        /// <param name="itemTaxPercent">Taxa de imposto do artigo</param>
        /// <returns></returns>
        private Item TransGetCreateItem(string itemId, string EANBarcode, string itemDescription,
                                   string unitId, string packUnitId, int unitsPerPack,
                                   bool isKg, bool isPack,
                                   double supplierId, double unitCostPrice,
                                   double itemTaxPercent) {
            //Descomentar para fazer a pesquisa por código de barras, código do artigo e código do fornecedor
            //string strItemID = dsoCache.ItemProvider.ItemSearch(itemId, 0, 0);
            //if( string.IsNullOrEmpty(strItemID) ) {
            //    //Search by supplier code
            //    object objSupplierId = supplierId;
            //    strItemID = dsoCache.ItemProvider.GetItemBySuplierReorderID(itemId, ref objSupplierId );
            //}
            //
            // senão, pesquisar o artigo por referência
            Item oItem = dsoCache.ItemProvider.GetItem(itemId, systemSettings.BaseCurrency);
            //
            // Se o artigo não existir devolver uma exceção
            if (oItem == null) {
                throw new Exception(string.Format("O Artigo[{0}] não existe.", itemId));
            }
            //
            // OU
            // Descomentar o seguinte para criar automaticamente um artigo novo
            //// Unidades de volume e fator
            ////
            //bool bSaveItem = false;
            ////
            ////Conversion processing
            //if (!isKg) {
            //    bool bHasPosIdentity = false;
            //    foreach (POSIdentity posIdentity in oItem.POSIdentity) {
            //        if (string.Compare(posIdentity.UnitOfMeasure, unitId, true) == 0) {
            //            bHasPosIdentity = true;
            //            break;
            //        }
            //    }
            //    //
            //    //POS Identity por unidade (Sem código de barras)
            //    if (!bHasPosIdentity && isPack) {
            //        POSIdentity posIdentity = new POSIdentity();
            //        posIdentity.UnitOfMeasure = unitId;
            //        posIdentity.Quantity = unitsPerPack;
            //        posIdentity.CurrencyID = systemSettings.BaseCurrency.CurrencyID;
            //        posIdentity.CurrencyExchange = systemSettings.BaseCurrency.BuyExchange;
            //        posIdentity.CurrencyFactor = systemSettings.BaseCurrency.EuroConversionRate;
            //        posIdentity.Description = oItem.Description;    //Or "Product custom description"
            //        oItem.POSIdentity.Add(posIdentity);
            //        posIdentity = null;
            //        bSaveItem = true;
            //    }
            //    //
            //    if (string.IsNullOrEmpty(oItem.BarCode)) {
            //        // código de barras EAN do artigo
            //        // Descomentar as linhas seguintes para atribuir um código de barras ao artigo:
            //        //oItem.BarCode = "12345678980123";
            //        //bSaveItem = true;
            //    }
            //    else if (!string.IsNullOrEmpty(EANBarcode)) {
            //        //Add new barcode, if not on present item
            //        if (!oItem.POSIdentity.IsInCollection(EANBarcode, oItem.UnitOfSaleID) && string.Compare(oItem.BarCode, EANBarcode, true) != 0) {
            //            POSIdentity oPOSIdentity = new POSIdentity();
            //            oPOSIdentity.UnitOfMeasure = oItem.UnitOfSaleID;
            //            oPOSIdentity.Quantity = 1;
            //            oPOSIdentity.CurrencyID = systemSettings.BaseCurrency.CurrencyID;
            //            oPOSIdentity.CurrencyExchange = systemSettings.BaseCurrency.BuyExchange;
            //            oPOSIdentity.CurrencyFactor = systemSettings.BaseCurrency.EuroConversionRate;
            //            oPOSIdentity.Description = itemDescription;    //Or custom description
            //            oPOSIdentity.POSItemID = EANBarcode;
            //            oItem.POSIdentity.Add(oPOSIdentity);
            //            oPOSIdentity = null;
            //            bSaveItem = true;
            //        }
            //    }
            //}
            ////
            //bool bHasItemSupplier = false;
            //foreach (ItemSupplier itemSupplier in oItem.SupplierList) {
            //    if (itemSupplier.SupplierID == supplierId && string.Compare(itemSupplier.ReorderID, itemId, true) == 0) {
            //        bHasItemSupplier = true;
            //        break;
            //    }
            //}
            ////
            //if (!bHasItemSupplier) {
            //    ItemSupplier itemSupplier = new ItemSupplier();
            //    itemSupplier.SupplierID = supplierId;
            //    itemSupplier.SupplierName = dsoCache.SupplierProvider.GetSupplierName(supplierId);
            //    //
            //    itemSupplier.UnitOfMeasure = unitId;
            //    itemSupplier.ReorderID = itemId;     //Supplier Reference
            //    itemSupplier.CurrencyID = systemSettings.BaseCurrency.CurrencyID;
            //    itemSupplier.CurrencyExchange = systemSettings.BaseCurrency.BuyExchange;
            //    itemSupplier.CurrencyFactor = systemSettings.BaseCurrency.EuroConversionRate;
            //    //Definir aqui o preço de custo:
            //    itemSupplier.CostPrice = unitCostPrice;

            //    oItem.SupplierList.Add(itemSupplier);
            //    itemSupplier = null;

            //    bSaveItem = true;
            //}
            ////Definir a taxa de imposto
            //short TaxGroupId = dsoCache.TaxesProvider.GetTaxableGroupIDFromTaxRate(itemTaxPercent, systemSettings.SystemInfo.DefaultCountryID, systemSettings.SystemInfo.TaxRegionID);
            //if (oItem.TaxableGroupID != TaxGroupId) {
            //    oItem.TaxableGroupID = TaxGroupId;
            //    bSaveItem = true;
            //}
            ////
            ////Gravar o artigo
            //if (bSaveItem)
            //    dsoCache.ItemProvider.Save(oItem, oItem.ItemID, false);

            return oItem;
        }

        /// <summary>
        /// Adiciona um detalhe (linha) à transação
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="itemId"></param>
        /// <param name="qty"></param>
        /// <param name="unitOfMeasureId"></param>
        /// <param name="unitPrice"></param>
        /// <param name="taxPercent"></param>
        /// <param name="whareHouseId"></param>
        private void TransAddDetail(ItemTransaction trans, Item item, double qty, string unitOfMeasureId, double unitPrice, double taxPercent, short whareHouseId,
                                     short colorId, short sizeId,
                                     string serialNumberPropId, string serialNumberPropValue) {


            var doc = systemSettings.WorkstationInfo.Document[trans.TransDocument];

            ItemTransactionDetail transDetail = new ItemTransactionDetail();

            //Moeda dos detalhes de  documento
            if (string.IsNullOrEmpty(txtTransCurrency.Text)) {
                transDetail.BaseCurrency = systemSettings.BaseCurrency;
            }
            else {
                CurrencyDefinition currency = new CurrencyDefinition();
                currency = dsoCache.CurrencyProvider.GetCurrency(txtTransCurrency.Text);
                if (currency == null) {
                    throw new Exception(string.Format("A moeda[{0}] não existe.", txtTransCurrency.Text));
                }
                else {
                    transDetail.BaseCurrency = dsoCache.CurrencyProvider.GetCurrency(txtTransCurrency.Text);
                }
            }
            //

            transDetail.ItemID = item.ItemID;
            transDetail.CreateDate = trans.CreateDate;
            transDetail.CreateTime = trans.CreateTime;
            transDetail.ActualDeliveryDate = trans.CreateDate;
            //Utilizar a descrição do artigo, ou uma descrição personalizada
            transDetail.Description = item.Description;
            // definir a quantidade
            transDetail.Quantity = qty;
            // Preço unitário. NOTA: Ver a diferença se o documento for com impostos incluidos!
            if (trans.TransactionTaxIncluded) {
                transDetail.TaxIncludedPrice = unitPrice;
            }
            else {
                transDetail.UnitPrice = unitPrice;
            }
            // Definir a lista de unidades
            transDetail.UnitList = item.UnitList;
            // Definir a unidade de venda/compra
            transDetail.SetUnitOfSaleID(unitOfMeasureId);
            //Definir os impostos
            short TaxGroupId = 0;
            if (taxPercent == 0 && item.TaxableGroupID != 0) {
                //se não preencher a taxa, carrega o imposto do artigo
                TaxGroupId = item.TaxableGroupID;
            }
            else {
                // Carrega o imposto pela ZONA
                // IMPORTANTE OSS: A transação já deve ter neste ponto a ZONA correta carregada
                TaxGroupId = bsoItemTransaction.BSOTaxes.GetTaxableGroupIDFromTaxRate(taxPercent, trans.Zone.CountryID, trans.Zone.TaxRegionID);
            }
            transDetail.TaxableGroupID = TaxGroupId;
            //*** Uncomment for discout
            //transDetail.DiscountPercent = 10
            //
            // Se o Armazém não existir, utilizar o default que se encontra no documento.
            if (dsoCache.WarehouseProvider.WarehouseExists(whareHouseId)) {
                transDetail.WarehouseID = whareHouseId;
            }
            else {
                transDetail.WarehouseID = doc.Defaults.Warehouse;
            }
            // Identificador da linha
            transDetail.LineItemID = trans.Details.Count + 1;
            //
            //*** Uncomment to provide line totals
            //.TotalGrossAmount =        'Line Gross amount
            //.TotalNetAmount =          'Net Gross amount
            //
            //Definir o último preço de compra
            if (doc.TransDocType == DocumentTypeEnum.dcTypePurchase) {
                transDetail.ItemExtraInfo.ItemLastCostTaxIncludedPrice = item.SalePrice[0, transDetail.Size.SizeID, string.Empty, 0, item.UnitOfSaleID].TaxIncludedPrice;
                transDetail.ItemExtraInfo.ItemLastCostUnitPrice = item.SalePrice[0, transDetail.Size.SizeID, string.Empty, 0, item.UnitOfSaleID].UnitPrice;
            }
            // Cores e tamanhos
            if (systemSettings.SystemInfo.UseColorSizeItems && chkTransModuleSizeColor.Checked) {
                // Cores
                if (item.Colors.Count > 0) {
                    ItemColor color = null;
                    if (colorId > 0 && item.Colors.IsInCollection(colorId)) {
                        color = item.Colors[ref colorId];
                    }
                    if (color == null) {
                        throw new Exception(string.Format("A cor indicada [{0}] não existe.", colorId));
                    }
                    transDetail.Color.ColorID = colorId;
                    transDetail.Color.Description = color.ColorName;
                    transDetail.Color.ColorKey = color.ColorKey;
                    transDetail.Color.ColorCode = color.ColorCode;
                }
                //Tamanhos
                if (item.Sizes.Count > 0 && chkTransModuleSizeColor.Checked) {
                    ItemSize size = null;
                    if (sizeId > 0 && item.Sizes.IsInCollection(sizeId)) {
                        size = item.Sizes[sizeId];
                    }
                    if (size == null) {
                        throw new Exception(string.Format("O tamanho indicado [{0}] não existe.", sizeId));
                    }
                    transDetail.Size.Description = size.SizeName;
                    transDetail.Size.SizeID = size.SizeID;
                    transDetail.Size.SizeKey = size.SizeKey;
                }
            }
            //
            // Propriedades (números de série e lotes)
            // ATENÇÃO: As regras de verificação das propriedades não estão implementadas na API. Deve ser a aplicação a fazer todas as validações necessárias
            //          Como por exemplo a movimentação duplicada de números de série
            // Verificar se estão ativadas no sistema e se foram marcadas no documento
            if (systemSettings.SystemInfo.UsePropertyItems && chkTransModuleProps.Checked) {
                // O Artigo tem propriedades ?
                if (item.PropertyEnabled) {
                    // NOTA: Para o exemplo atual apenas queremos uma propriedade definida no artigo com o ID1 = "NS ou "LOT"
                    //       Para outras propriedades e combinações, o código deve ser alterado em conformidade.
                    if (item.PropertyID1.Equals("NS", StringComparison.CurrentCultureIgnoreCase) || item.PropertyID1.Equals("LOT", StringComparison.CurrentCultureIgnoreCase)) {
                        transDetail.ItemProperties.ResetValues();
                        transDetail.ItemProperties.PropertyID1 = item.PropertyID1;
                        transDetail.ItemProperties.PropertyID2 = item.PropertyID2;
                        transDetail.ItemProperties.PropertyID3 = item.PropertyID3;
                        transDetail.ItemProperties.ControlMode = item.PropertyControlMode;
                        transDetail.ItemProperties.ControlType = item.PropertyControlType;
                        transDetail.ItemProperties.UseExpirationDate = item.PropertyUseExpirationDate;
                        transDetail.ItemProperties.UseProductionDate = item.PropertyUseProductionDate;
                        transDetail.ItemProperties.ExpirationDateControl = item.PropertyExpirationDateControl;
                        transDetail.ItemProperties.MaximumQuantity = item.PropertyMaximumQuantity;
                        transDetail.ItemProperties.UsePriceOnProp1 = item.UsePriceOnProp1;
                        transDetail.ItemProperties.UsePriceOnProp2 = item.UsePriceOnProp2;
                        transDetail.ItemProperties.UsePriceOnProp3 = item.UsePriceOnProp3;
                        //
                        transDetail.ItemProperties.PropertyValue1 = serialNumberPropValue;
                    }
                    else {
                        throw new Exception(string.Format("O Artigo indicado [{0}] não possui a propriedade indicada.", item.ItemID));
                    }
                    //}
                }
                else {
                    throw new Exception(string.Format("O Artigo indicado [{0}] não possui propriedades.", item.ItemID));
                }
            }

            transDetail.Graduation = item.Graduation;
            transDetail.ItemTax = item.ItemTax;
            transDetail.ItemTax2 = item.ItemTax2;
            transDetail.ItemTax3 = item.ItemTax3;
            //.WeightUnitOfMeasure = item.WeightUnitOfMeasure;
            //.WeightMeasure = item.WeightMeasure;

            transDetail.ItemType = item.ItemType;

            if (item.ItemType == ItemTypeEnum.itmService || item.ItemType == ItemTypeEnum.itmInterestRate || item.ItemType == ItemTypeEnum.itmOtherProductOrService) {
                transDetail.RetentionTax = item.WithholdingTaxRate;
            }

            item = null;
            //
            trans.Details.Add(transDetail);
        }

        /// <summary>
        /// Carrega um documento da base de dados e apresenta-o no ecran
        /// </summary>
        private void TransactionGet(bool suspendedTransaction) {
            Document doc = null;
            string transDoc = txtTransDoc.Text;
            string transSerial = txtTransSerial.Text;
            double transDocNumber = 0;
            double.TryParse(txtTransDocNumber.Text, out transDocNumber);

            // trans pode ser SaleTransaction ou BuyTransaction
            // dynamic permite utilizar as propriedades como num 'object' do VB6, sem que o compilador valide propriedades e métodos no momento da compilação
            dynamic trans = null;

            if (systemSettings.WorkstationInfo.Document.IsInCollection(transDoc)) {
                doc = systemSettings.WorkstationInfo.Document[transDoc];
            }
            if (doc == null) {
                throw new Exception(string.Format(" O documento [{0}] não existe.", transDoc));
            }

            if (suspendedTransaction) {
                if (bsoItemTransaction.LoadSuspendedTransaction(transSerial, transDoc, transDocNumber)) {
                    trans = bsoItemTransaction.Transaction;
                }
                else {
                    MessageBox.Show(string.Format("Não foi encontrada a transação em preparação: {0} {1}/{2}", transDoc, transSerial, transDocNumber),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else {
                switch (doc.TransDocType) {
                    case DocumentTypeEnum.dcTypeSale:
                    case DocumentTypeEnum.dcTypePurchase:
                        if (!bsoItemTransaction.LoadItemTransaction(doc.TransDocType, transSerial, transDoc, transDocNumber)) {
                            throw new Exception(string.Format("Não foi possível ler o documento [{0} {1}/{2}]", transDoc, transSerial, transDocNumber));
                        }
                        trans = bsoItemTransaction.Transaction;
                        rbTransBuySell.Checked = true;
                        break;

                    case DocumentTypeEnum.dcTypeStock:
                        if (!bsoStockTransaction.LoadStockTransaction(doc.TransDocType, transSerial, transDoc, transDocNumber)) {
                            throw new Exception(string.Format("Não foi possível ler o documento [{0} {1}/{2}]", transDoc, transSerial, transDocNumber));
                        }
                        trans = bsoStockTransaction.Transaction;

                        if (doc.StockBehavior == StockBehaviorEnum.sbStockCompose) {
                            rbTransStockCompose.Checked = true;
                        }
                        else {
                            if (doc.StockBehavior == StockBehaviorEnum.sbStockDecompose) {
                                rbTransStockDecompose.Checked = true;
                            }
                            else {
                                rbTransStock.Checked = true;
                            }
                        }
                        break;

                    default:
                        throw new Exception(string.Format(" O documento [{0}] é de um tipo não suportado por este exemplo: {1}.", transDoc, doc.TransDocType));
                }
            }

            var Transaction = new GenericTransaction(trans);
            TransactionShow(Transaction);
        }

        private void TransactionClear() {
            RepClear();
            chkTransModuleProps.Checked = false;
            chkTransModuleSizeColor.Checked = false;

            PartyTypeEnum partyType = cmbTransPartyType.SelectedIndex == 0 ? PartyTypeEnum.ptSupplier : PartyTypeEnum.ptCustomer;
            txtTransColor1.Text = string.Empty;
            txtTransCurrency.Text = systemSettings.BaseCurrency.CurrencyID;
            txtTransDate.Text = DateTime.Today.ToShortDateString();
            txtTransTime.Text = DateTime.Now.ToShortTimeString();
            string docId = string.Empty;

            if (rbTransBuySell.Checked) {
                if (partyType == PartyTypeEnum.ptCustomer) {
                    docId = systemSettings.WorkstationInfo.DefaultTransDocument;
                    cmbTransPartyType.SelectedIndex = 1;
                }
                else {
                    cmbTransPartyType.SelectedIndex = 0;
                    var docs = systemSettings.WorkstationInfo.Document.FindByNature(TransactionNatureEnum.Purchase_YourInvoice);
                    if (docs.Count > 0) {
                        docId = docs.get_ItemByIndex(1).DocumentID;
                    }
                }
                if (!string.IsNullOrEmpty(docId)) {
                    var doc = systemSettings.WorkstationInfo.Document[docId];
                    chkTransTaxIncluded.Checked = doc.TaxIncludedPrice;
                }
            }
            else {
                TransactionNatureEnum StockTransactionNatureId = 0;
                if (rbTransStock.Checked) {
                    StockTransactionNatureId = TransactionNatureEnum.Stock_Release;
                }
                else {
                    if (rbTransStockCompose.Checked) {
                        StockTransactionNatureId = TransactionNatureEnum.Stock_ProductionCompose;
                    }
                    else {
                        if (rbTransStockDecompose.Checked) {
                            StockTransactionNatureId = TransactionNatureEnum.Stock_ProductionDecompose;
                        }
                    }
                }
                var docs = systemSettings.WorkstationInfo.Document.FindByNature(StockTransactionNatureId);
                if (docs.Count > 0) {
                    var doc = docs.get_ItemByIndex(1);
                    docId = doc.DocumentID;
                    chkTransTaxIncluded.Checked = doc.TaxIncludedPrice;

                    if (doc.StockBehavior == StockBehaviorEnum.sbStockCompose || doc.StockBehavior == StockBehaviorEnum.sbStockDecompose) {
                        dataGridItemLines.Rows.Clear();
                    }
                }
                // partyType = Nenhum
                cmbTransPartyType.SelectedIndex = 2;
            }
            //
            txtTransDoc.Text = docId;
            txtTransDocNumber.Text = "0";
            txtTransGlobalDiscount.Text = "0";
            txtTransPartyId.Text = string.Empty;
            txtPaymentID.Text = "0";
            txtTenderID.Text = "0";
            //
            // Obter a primeira série EXTERNA
            var externalSeries = systemSettings.DocumentSeries
                                               .OfType<DocumentsSeries>()
                                               .FirstOrDefault(x => x.SeriesType == SeriesTypeEnum.SeriesExternal);
            if (externalSeries != null) {
                txtTransSerial.Text = externalSeries.Series;
            }
            //
            TransClearL1();
            TransClearL2();
            //
            tabBuySaleTransaction.BackgroundImage = null;
        }

        private void TransClearL1() {
            txtTransItemL1.Text = string.Empty;
            txtTransFactorL1.Text = "1";
            txtTransQuantityL1.Text = string.Empty;
            txtTransTaxRateL1.Text = string.Empty;
            txtTransUnitPriceL1.Text = string.Empty;
            txtTransUnL1.Text = systemSettings.SystemInfo.ItemDefaultsSettings.ItemDefaultUnit;
            txtTransWarehouseL1.Text = string.Empty;

            //Propriedades: NS
            TransClearNS1();

            //Size and colors
            TransClearSize1();
            TransClearColor1();
        }

        private void TransClearL2() {
            txtTransFactorL2.Text = "1";
            txtTransItemL2.Text = string.Empty;
            txtTransQuantityL2.Text = string.Empty;
            txtTransUnL2.Text = systemSettings.SystemInfo.ItemDefaultsSettings.ItemDefaultUnit;
            txtTransTaxRateL2.Text = string.Empty;
            txtTransUnitPriceL2.Text = string.Empty;
            txtTransWarehouseL2.Text = string.Empty;

            //Propriedades: NS
            TransClearNS2();
        }

        private void TransClearNS1() {
            txtTransPropValueL1.Text = string.Empty;
        }

        private void TransClearNS2() {
            txtTransPropValueL2.Text = string.Empty;
        }

        private void TransClearSize1() {
            txtTransSize1.Text = string.Empty;
        }

        private void TransClearColor1() {
            txtTransColor1.Text = string.Empty;
        }

        private void btnTransClearL1_Click(object sender, EventArgs e) {
            TransClearL1();
        }

        private void btnTransClearNSL1_Click(object sender, EventArgs e) {
            TransClearNS1();
        }

        private void btnTransClearNSL2_Click(object sender, EventArgs e) {
            TransClearNS2();
        }

        private void btnTransClearSz_Click(object sender, EventArgs e) {
            TransClearSize1();
        }

        private void btnTransClearColor1_Click(object sender, EventArgs e) {
            TransClearColor1();
        }

        private void btnTransClearL2_Click(object sender, EventArgs e) {
            TransClearL2();
        }

        private short GetWeekOfYear(DateTime currentDate) {
            short lotRetWeek = (short)System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(currentDate,
                                                                                                             System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule,
                                                                                                            System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
            if (lotRetWeek < 52) {
                lotRetWeek++;
            }
            return lotRetWeek;
        }

        #endregion

        #region Stock

        internal TransactionID TransactionStockUpdate(string transSerial, string transDocument, double transDocNumber, bool isNew) {
            bool blnSaved = false;
            TransactionID resultTransId = null;

            if (!APIEngine.SystemSettings.WorkstationInfo.Document.IsInCollection(transDocument)) {
                throw new Exception(string.Format("O documento [{0}] não existe ou não se encontra preenchido.", transDocument));
            }

            DocumentsSeries transSeries = null;
            if (APIEngine.SystemSettings.DocumentSeries.IsInCollection(transSerial)) {
                transSeries = APIEngine.SystemSettings.DocumentSeries[transSerial];
                if (transSeries.SeriesType != SeriesTypeEnum.SeriesExternal) {
                    throw new Exception("Apenas são permitidas séries externas.");
                }
            }
            if (transSeries == null) {
                throw new Exception("A série indicada não existe");
            }
            //
            var transType = ItemTransactionHelper.TransGetType(transDocument);
            if (transType != DocumentTypeEnum.dcTypeStock) {
                throw new Exception(string.Format("O documento indicado [{0}] não é um documento de stock", transDocument));
            }

            var objDSOStockTransaction = new DSOStockTransaction();

            //var DocTransStatus = TransStatusEnum.stNormal;
            blnSaved = false;

            bsoStockTransaction.PermissionsType = FrontOfficePermissionEnum.foPermByUser;
            if (isNew) {
                bsoStockTransaction.InitNewTransaction(transDocument, transSerial);
                if (transDocNumber > 0) {
                    bsoStockTransaction.Transaction.TransDocNumber = transDocNumber;
                }
            }
            else {
                var loadResult = bsoStockTransaction.LoadStockTransaction(transType, transSerial, transDocument, transDocNumber);
                if (!loadResult) {
                    throw new Exception(string.Format("Não foi possível carregar o documento {0} {1}/{2}.", transDocument, transSerial, transDocNumber));
                }
            }
            var bsoCommonTransaction = bsoStockTransaction.BSOCommonTransaction;

            //Taxes included?
            bool transTaxIncluded = chkTransTaxIncluded.Checked;
            bsoStockTransaction.TransactionTaxIncluded = transTaxIncluded;
            bsoCommonTransaction.TransactionTaxIncluded = transTaxIncluded;
            //
            bsoCommonTransaction.TransactionType = DocumentTypeEnum.dcTypeStock;
            bsoStockTransaction.Transaction.TransDocType = DocumentTypeEnum.dcTypeStock;
            //
            DateTime createDate = txtTransDate.Text.ToDateTime(DateTime.Now);
            bsoStockTransaction.createDate = createDate;
            //bsoStockTransaction.CheckCreateDate = createDate;
            bsoStockTransaction.CreateTime = new DateTime(DateTime.Now.TimeOfDay.Ticks);
            bsoStockTransaction.ActualDeliveryDate = createDate;
            //
            // Descomentar a linha seguiinte para indicar uma referência livre
            //bsoStockTransaction.ContractReferenceNumber = "External REF"

            //Party RELATED INFO (can be ignored)
            PartyTypeEnum partyType = ItemTransactionHelper.TransGetPartyType(cmbTransPartyType.SelectedIndex);
            bsoStockTransaction.PartyType = (short)partyType;
            double partyId = txtTransPartyId.Text.ToDouble();
            if (bsoStockTransaction.CheckPartyID(partyId)) {
                bsoStockTransaction.PartyID = partyId;
            }
            //TODO: Verify
            //bsoCommonTransaction.CountryID = APIEngine.SystemSettings.SystemInfo.DefaultCountryID;
            //bsoCommonTransaction.TaxRegionID = APIEngine.SystemSettings.SystemInfo.TaxRegionID;
            bsoCommonTransaction.EntityFiscalStatusID = bsoStockTransaction.Transaction.PartyFiscalStatus;
            //------------> ZONA
            //------------> MOEDA
            var currency = APIEngine.DSOCache.CurrencyProvider.GetCurrency(txtTransCurrency.Text);
            if (currency == null) {
                currency = APIEngine.SystemSettings.BaseCurrency;
            }
            bsoStockTransaction.BaseCurrency = currency.CurrencyID;
            bsoStockTransaction.BaseCurrencyExchange = currency.BuyExchange;

            // Observações
            // Modificar para acrescentar ou retirar observações livres
            bsoStockTransaction.Transaction.Comments = "Gerado por: " + Application.ProductName;

            var transStock = bsoStockTransaction.Transaction;

            //-------------------------------------------------------------
            // *** DETALHES
            //-------------------------------------------------------------
            // Remover todas as linhas (caso da alteração)
            int i = 1;
            while (transStock.Details.Count > 0) {
                transStock.Details.Remove(ref i);
            }
            StockQtyRuleEnum StockQtyRule = StockQtyRuleEnum.stkQtyNone;
            if (bsoStockTransaction.Transaction.TransStockBehavior == StockBehaviorEnum.sbStockCompose) {
                StockQtyRule = StockQtyRuleEnum.stkQtyReceipt;
            }
            else {
                if (bsoStockTransaction.Transaction.TransStockBehavior == StockBehaviorEnum.sbStockDecompose) {
                    StockQtyRule = StockQtyRuleEnum.stkQtyOutgoing;
                }
            }
            //
            //Lista de armazens
            var warehouseList = dSOWarehouse.GetWarehouseList();
            //
            //Linha 1
            string itemId = txtTransItemL1.Text;
            if (!string.IsNullOrEmpty(itemId)) {
                if (itemProvider.ItemExist(itemId)) {
                    short wareHouseId = txtTransWarehouseL1.Text.ToShort();
                    if (warehouseList.IsInCollection(wareHouseId)) {
                        string unitOfMovId = txtTransUnL1.Text;
                        double taxRate = txtTransTaxRateL1.Text.ToDouble();
                        double qty = txtTransQuantityL1.Text.ToDouble();
                        double unitPrice = txtTransUnitPriceL1.Text.ToDouble();
                        TransStockAddDetail(wareHouseId, itemId, unitOfMovId, taxRate, qty, unitPrice, StockQtyRule);

                        if (bsoStockTransaction.Transaction.TransStockBehavior == StockBehaviorEnum.sbStockCompose || bsoStockTransaction.Transaction.TransStockBehavior == StockBehaviorEnum.sbStockDecompose) {
                            var itemDetails = GetItemComponentList(1);
                            if (itemDetails != null) {

                                foreach (ItemTransactionDetail value in itemDetails) {
                                    TransStockAddDetail(wareHouseId, value.ItemID, value.UnitOfSaleID, taxRate, value.Quantity, value.UnitPrice, value.PhysicalQtyRule);
                                }
                            }
                        }
                    }
                    else {
                        throw new Exception("O armazém indicado não existe");
                    }
                }
                else {
                    throw new Exception(string.Format("O Artigo [{0}] não foi entrado.", itemId));
                }
            }
            //
            // Linha 2
            itemId = txtTransItemL2.Text.Trim();
            if (!string.IsNullOrEmpty(itemId)) {
                if (itemProvider.ItemExist(itemId)) {
                    short wareHouseId = txtTransWarehouseL2.Text.ToShort();
                    if (warehouseList.IsInCollection(wareHouseId)) {
                        string unitOfMovId = txtTransUnL2.Text;
                        double taxRate = txtTransTaxRateL2.Text.ToDouble();
                        double qty = txtTransQuantityL2.Text.ToDouble();
                        double unitPrice = txtTransUnitPriceL2.Text.ToDouble();
                        TransStockAddDetail(wareHouseId, itemId, unitOfMovId, taxRate, qty, unitPrice, StockQtyRule);

                        if (bsoStockTransaction.Transaction.TransStockBehavior == StockBehaviorEnum.sbStockCompose || bsoStockTransaction.Transaction.TransStockBehavior == StockBehaviorEnum.sbStockDecompose) {
                            var itemDetails = GetItemComponentList(2);
                            if (itemDetails != null) {

                                foreach (ItemTransactionDetail value in itemDetails) {
                                    TransStockAddDetail(wareHouseId, value.ItemID, value.UnitOfSaleID, taxRate, value.Quantity, value.UnitPrice, value.PhysicalQtyRule);
                                }
                            }
                        }
                    }
                    else {
                        throw new Exception("O armazém indicado não existe");
                    }
                }
                else {
                    throw new Exception("O artigo indicado não existe");
                }
            }
            //
            if (bsoStockTransaction.Transaction.Details.Count == 0) {
                throw new Exception("O documento não tem linhas.");
            }
            //
            //*** SAVE
            if (!blnSaved) {
                if (bsoStockTransaction.Transaction.Details.Count > 0) {
                    // Colocar a false para não imprimir.
                    // A Impressão não é atualmente suportada em .NET
                    bool printDoc = false;

                    //CalculateOutgoingQuantities (Documentos de Fabrico, Composição e Decomposição determinar o preço da materia prima entrada)
                    bsoStockTransaction.Calculate(true, true, true);
                    bsoStockTransaction.SaveDocumentEx(true, ref printDoc);

                    resultTransId = new TransactionID();
                    resultTransId.TransSerial = transStock.TransSerial;
                    resultTransId.TransDocument = transStock.TransDocument;
                    resultTransId.TransDocNumber = transStock.TransDocNumber;
                }
                else {
                    throw new Exception("O documento não tem linhas");
                }

                ////Documento anulado -- Descomentar
                //if (DocTransStatus == TransStatusEnum.stVoid) {
                //    if (bsoStockTransaction.LoadStockTransaction(DocumentTypeEnum.dcTypeStock, transSerial, transDocument, transDocNumber)) {
                //        objDSOStockTransaction.Delete(transStock);
                //        blnSaved = true;
                //    }
                //    else
                //        bsoStockTransaction.Transaction.TransStatus = TransStatusEnum.stVoid;
                //}
            }
            bsoCommonTransaction = null;
            objDSOStockTransaction = null;

            return resultTransId;
        }

        internal void TransStockAddDetail(short warehouseId, string itemId, string unitOfSaleId, double itemTaxRate, double Quantity, double unitPrice, StockQtyRuleEnum StockQtyRule) {
            StockTransaction stockTrans = bsoStockTransaction.Transaction;
            BSOItemTransactionDetail BSOItemTransDetail = null;
            // Motor dos detalhes (linhas)
            BSOItemTransDetail = new BSOItemTransactionDetail();
            BSOItemTransDetail.TransactionType = stockTrans.TransDocType;
            // Utilizador e permissões
            BSOItemTransDetail.UserPermissions = systemSettings.User;
            BSOItemTransDetail.PermissionsType = FrontOfficePermissionEnum.foPermByUser;
            //
            bsoStockTransaction.BSOStockTransactionDetail = BSOItemTransDetail;
            BSOItemTransDetail = null;

            double lngLineItemID = stockTrans.Details.Count + 1;

            bool blnCanAddDetail = true;
            var transDetail = new ItemTransactionDetail();
            transDetail.BaseCurrency = stockTrans.BaseCurrency;
            transDetail.CreateDate = stockTrans.CreateDate;
            transDetail.ActualDeliveryDate = stockTrans.ActualDeliveryDate;
            transDetail.PartyTypeCode = stockTrans.PartyTypeCode;
            transDetail.PartyID = stockTrans.PartyID;
            //
            //*** WAREHOUSE
            if (warehouseId > 0)
                if (APIEngine.DSOCache.WarehouseProvider.WarehouseExists(warehouseId)) {
                    transDetail.WarehouseID = warehouseId;
                }
                else {
                    transDetail.WarehouseID = warehouseId;
                }
            else {
                transDetail.WarehouseID = warehouseId;
            }
            //
            transDetail.WarehouseOutgoing = transDetail.WarehouseID;
            transDetail.WarehouseReceipt = transDetail.WarehouseID;
            transDetail.PhysicalQtyRule = StockQtyRule;

            ////***STOCK TRANSFER ONLY -- uncomment to set
            //if (systemSettings.WorkstationInfo.Document[stockTrans.TransDocument].StockBehavior == StockBehaviorEnum.sbStockTransfer)
            //    transDetail.WarehouseReceipt = bsoStockTrans.WarehouseReceipt;

            ////    *** DESTINATION WAREHOUSE -- uncomment to set
            //short warehouseReceiptId=2;
            //if( dsoCache.WarehouseProvider.WarehouseExists(warehouseReceiptId) ){
            //    transDetail.WarehouseReceipt = warehouseReceiptId;

            //    if( transDetail.ComponentList !=null ){
            //        foreach( ItemTransactionDetail transDetailSave in transDetail.ComponentList )
            //            transDetailSave.WarehouseID = warehouseReceiptId;
            //    }
            //}

            ////*** SOURCE WAREHOUSE -- uncomment to set
            //short warehouseOutgoingId = 1;
            //if( dsoCache.WarehouseProvider.WarehouseExists(warehouseOutgoingId) ){
            //    transDetail.WarehouseOutgoing = warehouseOutgoingId;

            //    if( systemSettings.WorkstationInfo.Document[stockTrans.TransDocument].StockBehavior == StockBehaviorEnum.sbStockTransfer ){
            //        if( warehouseOutgoingId != transDetail.WarehouseID )
            //            transDetail.WarehouseID = warehouseReceiptId;
            //    }

            //    if( transDetail.ComponentList!=null){
            //        foreach( ItemTransactionDetail transDetailSave in transDetail.ComponentList )
            //            transDetailSave.WarehouseID = warehouseOutgoingId;
            //    }
            //}

            //LineItemId
            transDetail.LineItemID = lngLineItemID;
            //
            //-----> INFORMAÇÕES DO PRODUTO
            var item = APIEngine.DSOCache.ItemProvider.GetItemForTransactionDetail(itemId, transDetail.BaseCurrency);

            if (item != null) {
                transDetail.ItemID = item.ItemID;
                transDetail.Description = item.Description;
                transDetail.TaxableGroupID = item.TaxableGroupID;
                transDetail.ItemType = item.ItemType;
                transDetail.FamilyID = item.Family.FamilyID;
                transDetail.UnitList = item.UnitList.Clone();

                transDetail.WeightUnitOfMeasure = item.WeightUnitOfMeasure;
                transDetail.WeightMeasure = item.WeightMeasure;
                transDetail.Graduation = item.Graduation;
                transDetail.ItemTax = item.ItemTax;
                transDetail.ItemTax2 = item.ItemTax2;
                transDetail.ItemTax3 = item.ItemTax3;
                transDetail.ItemExtraInfo.ItemQuantityCalcFormula = item.ItemQuantityCalcFormula;

                if (item.UnitList.IsInCollection(unitOfSaleId)) {
                    transDetail.UnitOfSaleID = unitOfSaleId;
                }
                else {
                    transDetail.UnitOfSaleID = item.GetDefaultUnitForTransaction(DocumentTypeEnum.dcTypeStock);
                }

                //*** PROPERTIES -- Uncomment to use
                //if(item.PropertyEnabled){
                //    transDetail.ItemProperties.PropertyID1 = item.PropertyID1;
                //    transDetail.ItemProperties.PropertyID2 = item.PropertyID2;
                //    transDetail.ItemProperties.PropertyID3 = item.PropertyID3;
                //    transDetail.ItemProperties.UsePriceOnProp1 = item.UsePriceOnProp1;
                //    transDetail.ItemProperties.UsePriceOnProp2 = item.UsePriceOnProp2;
                //    transDetail.ItemProperties.UsePriceOnProp3 = item.UsePriceOnProp3;
                //    transDetail.ItemProperties.ControlType = item.PropertyControlType;
                //    transDetail.ItemProperties.ControlMode = item.PropertyControlMode;
                //    transDetail.ItemProperties.UseExpirationDate = item.PropertyUseExpirationDate;
                //    transDetail.ItemProperties.UseProductionDate = item.PropertyUseProductionDate;
                //    transDetail.ItemProperties.ExpirationDateControl = item.PropertyExpirationDateControl;
                //    transDetail.ItemProperties.MaximumQuantity = item.PropertyMaximumQuantity;
                //    transDetail.ItemProperties.ResetValues();

                //    transDetail.ItemProperties.PropertyValue1 = ... value 1
                //    transDetail.ItemProperties.PropertyValue1_Key2 = ... key 2
                //    transDetail.ItemProperties.PropertyValue1_Key3 = ... key 3
                //    transDetail.ItemProperties.PropertyValue2 = ... value 2
                //    transDetail.ItemProperties.PropertyValue2_Key2 = ... key 2
                //    transDetail.ItemProperties.PropertyValue2_Key3 = ... key 3
                //    transDetail.ItemProperties.PropertyValue3 = ... value 3
                //    transDetail.ItemProperties.PropertyValue3_Key2 = ... key 2
                //    transDetail.ItemProperties.PropertyValue3_Key3 = ... key 3
                //}
            }
            else if (itemId == "=") {
                //*** COMMENT LINE
                item = new Item();
                //ItemId: //=// represents comment line
                item.ItemID = "=";
                item.Description = "Só descrição";
                item.ItemType = ItemTypeEnum.itmComments;
                item.UnitOfSaleID = APIEngine.SystemSettings.SystemInfo.ItemDefaultsSettings.ItemDefaultUnit;
                item.AlternativeUnitOfStock = APIEngine.SystemSettings.SystemInfo.ItemDefaultsSettings.ItemDefaultUnit;
                item.DefaultStockUnit = APIEngine.SystemSettings.SystemInfo.ItemDefaultsSettings.ItemDefaultUnit;
                item.DefaultBuyUnit = APIEngine.SystemSettings.SystemInfo.ItemDefaultsSettings.ItemDefaultUnit;
                item.DefaultSellingUnit = APIEngine.SystemSettings.SystemInfo.ItemDefaultsSettings.ItemDefaultUnit;
                item.TaxableGroupID = APIEngine.SystemSettings.SystemInfo.ItemDefaultsSettings.DefaultTaxableGroupID;
                item.CurrencyID = APIEngine.SystemSettings.BaseCurrency.CurrencyID;
                item.CurrencyExchange = APIEngine.SystemSettings.BaseCurrency.SaleExchange;
                item.CurrencyFactor = APIEngine.SystemSettings.BaseCurrency.EuroConversionRate;
            }
            else {
                throw new Exception(string.Format("O Artigo [{0}] não foi entrado.", itemId));
            }

            //-----> Taxa de IVA
            transDetail.TaxableGroupID = APIEngine.DSOCache.TaxesProvider.GetTaxableGroupIDFromTaxRate(itemTaxRate,
                                                                                                           APIEngine.SystemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID,
                                                                                                           APIEngine.SystemSettings.SystemInfo.TaxRegionID);

            //-----> Cores e Tamanhos. Uncomment to SET
            //short ColorId = 3;
            //short SizeId = 4;
            //if( item != null ){
            //    if( item.Colors.Count > 0 && item.Sizes.Count > 0 ){
            //        var color = dsoCache.ColorProvider.GetColor(ColorId);
            //        if( color !=null )
            //            transDetail.Color = color;

            //        var size = dsoCache.SizeProvider.GetSize(SizeId);
            //        if( size != null )
            //            transDetail.Size = size;

            //        if( transDetail.Color.ColorID == 0 ){
            //            foreach( ItemColor itemColor in item.Colors ){
            //                color = dsoCache.ColorProvider.GetColor(itemColor.ColorID);
            //                if( color !=null )
            //                    transDetail.Color = color;
            //                break;
            //            }
            //        }

            //        if( transDetail.Size.SizeID == 0){
            //            foreach( ItemSize itemSize in item.Sizes ){
            //                size = dsoCache.SizeProvider.GetSize(itemSize.SizeId);
            //                if( size !=null )
            //                    transDetail.Size = size;
            //                break;
            //            }
            //        }
            //    }
            //}
            //
            transDetail.SetUnitOfSaleID(transDetail.UnitOfSaleID);

            //Formulas
            double Quantity1 = 0;
            double Quantity2 = 0;
            double Quantity3 = 0;
            double Quantity4 = 0;

            ////*** Packs -- Uncomment to set
            //double packQuantity=10;
            //if( transDetail.UnitConversion != 0 && packQuantity != 0)
            //    transDetail.PackQuantity = packQuantity;

            bool blnHaveSetUnits = false;
            ////*** Units -- uncomment to set
            //double units = 10;
            //if( units != 0){
            //    transDetail.SetUnits(units);
            //    blnHaveSetUnits = true;
            //}

            transDetail.Quantity1 = Quantity1;
            transDetail.Quantity2 = Quantity2;
            transDetail.Quantity3 = Quantity3;
            transDetail.Quantity4 = Quantity4;
            if (!blnHaveSetUnits) {
                if (!string.IsNullOrEmpty(transDetail.ItemExtraInfo.ItemQuantityCalcFormula) && APIEngine.SystemSettings.SystemInfo.UseUnitWithFormulaItems) {
                    transDetail.SetQuantity(StockHelper.CalculateQuantity(transDetail.ItemExtraInfo.ItemQuantityCalcFormula, transDetail, true));
                }
                else {
                    transDetail.SetQuantity(StockHelper.CalculateQuantity(null, transDetail, true));
                }
            }
            //    
            if (!blnHaveSetUnits) {
                transDetail.SetQuantity(Quantity);
            }
            transDetail.Description = item.Description;     // OR "Custom description"
            transDetail.Comments = "Observações de linha: Gerada por" + Application.ProductName;

            //*** UnitPrice
            if (bsoStockTransaction.TransactionTaxIncluded) {
                transDetail.TaxIncludedPrice = unitPrice;
            }
            else {
                transDetail.UnitPrice = unitPrice;
            }
            //
            // Descomentar para indicar desconto na linha
            //transDetail.DiscountPercent = 10;
            //
            ////Desconto cumulativo - Descomentar para indicar
            //transDetail.CumulativeDiscountPercent1 = 1
            //transDetail.CumulativeDiscountPercent2 = 2
            //transDetail.CumulativeDiscountPercent3 = 3

            S50cUtil22.MathFunctions mathUtil = new MathFunctions();

            if (transDetail.DiscountPercent == 0 && (transDetail.CumulativeDiscountPercent1 != 0 || transDetail.CumulativeDiscountPercent2 != 0 || transDetail.CumulativeDiscountPercent3 != 0)) {
                transDetail.DiscountPercent = mathUtil.GetCumulativeDiscount(transDetail.CumulativeDiscountPercent1, transDetail.CumulativeDiscountPercent2, transDetail.CumulativeDiscountPercent3);
            }

            if (transDetail.DiscountPercent != 0 && (transDetail.CumulativeDiscountPercent1 == 0 && transDetail.CumulativeDiscountPercent2 == 0 && transDetail.CumulativeDiscountPercent3 == 0)) {
                transDetail.CumulativeDiscountPercent1 = transDetail.DiscountPercent;
            }

            ////*** Kit ITEMS -- Uncomment to use
            //if( item != null ){
            //    if( item.ItemType == ItemTypeEnum.itmKit){
            //        transDetail.ComponentList = bsoStockTrans.BSOCommonTransaction.GetComponentList(transDetail, item.ItemCollection, transDetail.Quantity, item.NeededComponents, item.UseComponentPrices, "PCUP", 0);
            //    }
            //}
            //transDetail.ItemExtraInfo.DoNotGroup = true;

            //*** PROPERTIES
            if (transDetail.ItemProperties.HasPropertyValues) {
                APIEngine.DSOCache.ItemPropertyProvider.GetItemPropertyStock(transDetail.ItemID, transDetail.WarehouseID, transDetail.ItemProperties);
            }

            //*** Delivery time -- Uncomment to set
            //transDetail.RequiredDeliveryDateTime = DateTime.Now.AddDays(10);  // Hoje + 10 dias

            if (blnCanAddDetail) {
                bool calculate = true;
                bsoStockTransaction.AddDetail(transDetail, ref calculate);
            }
            item = null;
        }

        #endregion

        private void btnClear_Click(object sender, EventArgs e) {
            switch (tabEntities.SelectedIndex) {
                case 0: ItemClear(false); break;
                case 1: CustomerClear(); break;
                case 2: SupplierClear(); break;
                case 3: TransactionClear(); break;
                case 4: AccountTransactionClear(); break;

                case 5: UnitOfMeasureClear(); break;
            }
        }

        private void chkTransModuleProps_CheckedChanged(object sender, EventArgs e) {
            pnlTransModuleProp.Enabled = chkTransModuleProps.Checked;
        }

        private void chkTransModuleSizeColor_CheckedChanged(object sender, EventArgs e) {
            pnlTransModuleSizeColor.Enabled = chkTransModuleSizeColor.Checked;
        }

        private void tabEntities_SelectedIndexChanged(object sender, EventArgs e) {
            //TODO: Perguntar ao jorge
        }

        #region Account documents

        private TransactionID AccountTransactionRemove() {
            TransactionID transId = null;
            string transSerial = txtAccountTransSerial.Text;
            string transDoc = txtAccountTransDoc.Text;
            double transDocNumber = 0;
            double.TryParse(txtAccountTransDocNumber.Text, out transDocNumber);
            //
            // Obter a transação (recibo ou pagamento)
            var result = accountTransManager.LoadTransaction(transSerial, transDoc, transDocNumber);
            if (!result) {
                throw new Exception(string.Format(" O documento {0} {1}/{2} não existe ou não é possível carregá-lo.", transDoc, transSerial, transDocNumber));
            }
            //
            //Colocar o motivo de isenção: obrigatóriedade depende da definição do documento
            accountTransManager.Transaction.VoidMotive = "Anulado por " + Application.ProductName;
            // Anular o documento
            if (accountTransManager.DeleteDocument()) {
                transId = accountTransManager.Transaction.TransactionID;
            }
            else {
                throw new Exception(string.Format("Não foi possível anular o documento {0} {1}/{2}.", transDoc, transSerial, transDocNumber));
            }

            return transId;
        }

        private void AccountTransAddDetail(AccountTransactionManager accountTransMan, AccountUsedEnum accountUsed, string accountTypeId,
                                            string docId, string docSeries, double docNumber, short transInstallment, double paymentValue) {
            // Linhas
            if (paymentValue > 0) {
                AccountTransaction accountTrans = accountTransMan.Transaction;

                if (systemSettings.WorkstationInfo.Document.IsInCollection(docId)) {
                    // Obter o pendente. PAra efeito de exemplo consideramos que não há prestações (installmentId=0)
                    var ledger = accountTransMan.LedgerAccounts.OfType<LedgerAccount>().FirstOrDefault(x => x.TransDocument == docId && x.TransSerial == docSeries && x.TransDocNumber == docNumber && x.TransInstallmentID == transInstallment);
                    if (ledger != null) {
                        if (paymentValue > ledger.TotalPendingAmount) {
                            throw new Exception(string.Format("O valor a pagar é superior ao valor em divida no documento: {0} {1}/{2}", docId, docSeries, docNumber));
                        }
                        AccountTransactionDetail detail = accountTrans.Details.Find(docSeries, docId, docNumber, transInstallment);
                        if (detail == null) {
                            detail = new AccountTransactionDetail();
                        }
                        // Lançar o pagamento correcto, acertando também a retenção.
                        accountTransMan.SetPaymentValue(ledger.Guid, paymentValue);
                        //
                        // Copiar o pendente para o pagamento
                        detail.AccountTypeID = ledger.PartyAccountTypeID;
                        detail.BaseCurrency = accountTrans.BaseCurrency;
                        detail.DocContractReference = ledger.ContractReferenceNumber;
                        detail.DocCreateDate = ledger.CreateDate;
                        detail.DocCurrency = ledger.BaseCurrency;
                        detail.DocDeferredPaymentDate = ledger.DeferredPaymentDate;
                        detail.DocID = ledger.TransDocument;
                        detail.DocInstallmentID = ledger.TransInstallmentID;
                        detail.DocNumber = ledger.TransDocNumber;
                        detail.DocSerial = ledger.TransSerial;
                        //detail.ExchangeDifference = 
                        detail.LedgerGUID = ledger.Guid;
                        detail.PartyID = accountTrans.Entity.PartyID;
                        detail.PartyTypeCode = (short)accountTrans.Entity.PartyType;
                        detail.RetentionOriginalAmount = ledger.RetentionTotalAmount;
                        detail.RetentionPayedAmount = ledger.RetentionPayedAmount;
                        detail.RetentionPendingAmount = ledger.RetentionPendingAmount - ledger.RetentionPayedAmount;
                        //detail.TaxValues
                        detail.TotalDiscountAmount = ledger.DiscountValue;
                        detail.TotalDiscountPercent = ledger.DiscountPercent;
                        detail.TotalOriginalAmount = ledger.TotalAmount;
                        detail.TotalPayedAmount = ledger.PaymentValue;
                        detail.TotalPendingAmount = ledger.TotalPendingAmount - ledger.PaymentValue;
                        //
                        detail.TransDocNumber = accountTrans.TransDocNumber;
                        detail.TransDocument = accountTrans.TransDocument;
                        detail.TransSerial = accountTrans.TransSerial;
                        //
                        detail.CashAccountingSchemeType = ledger.CashAccountingSchemeType;
                        //
                        accountTrans.Details.Add(ref detail);
                    }
                }
            }
        }

        /// <summary>
        /// Insere ou altera um pagamento ou recibo
        /// 
        /// </summary>
        /// <param name="newDoc"></param>
        private TransactionID AccountTransactionUpdate(bool newDoc) {
            const string ACCOUNT_TYPE = "CC";                   //Como exemplo, sóvamos utilizar a carteira de contas correntes
            string transSerial = txtAccountTransSerial.Text.ToUpper();
            string transDoc = txtAccountTransDoc.Text.ToUpper();
            double transDocNumber = 0;
            double.TryParse(txtAccountTransDocNumber.Text, out transDocNumber);
            double partyId = 0;
            double.TryParse(txtAccountTransPartyId.Text, out partyId);
            TransactionID result = null;
            //
            AccountUsedEnum accountUsed = AccountUsedEnum.auNone;
            if (cmbRecPeg.SelectedIndex == 0) {
                accountUsed = AccountUsedEnum.auCustomerLedgerAccount;
                accountTransManager.InitManager(accountUsed);
            }
            else {
                accountUsed = AccountUsedEnum.auSupplierLedgerAccount;
                accountTransManager.InitManager(accountUsed);
            }
            if (newDoc) {
                accountTransManager.InitNewTransaction(transSerial, transDoc, transDocNumber);
                accountTransManager.SetPartyID(partyId);
            }
            else {
                accountTransManager.LoadTransaction(transSerial, transDoc, transDocNumber);
            }
            var accountTrans = accountTransManager.Transaction;
            if (accountTrans == null) {
                throw new Exception(string.Format("Não foi possível iniciar/carregar o documento {0} {1}/{2}", transDoc, transSerial, transDocNumber));
            }
            //Obter a conta corrente do cliente
            accountTrans.LedgerAccounts = dsoCache.LedgerAccountProvider.GetLedgerAccountList(accountUsed, ACCOUNT_TYPE, partyId, accountTrans.BaseCurrency);
            if (accountTrans.LedgerAccounts.Count == 0) {
                throw new Exception(string.Format("A entidade [{0}] não tem pendentes na carteira [{1}].", partyId, ACCOUNT_TYPE));
            }
            //
            // Remover todos os pagamentos da ledger account (se o recibo estiver a ser alterado)
            int i = 1;
            while (accountTrans.Details.Count > 0) {
                accountTrans.Details.Remove(i);
            }
            accountTransManager.SetAccountID(ACCOUNT_TYPE); // Conta corrente
            accountTransManager.SetBaseCurrencyID(txtAccountTransDocCurrency.Text);
            DateTime createDate = DateTime.Today;
            DateTime.TryParse(txtAccountTransDocDate.Text, out createDate);
            accountTransManager.SetCreateDate(createDate);
            //
            // Linhas
            // Linha 1
            string docId = txtAccountTransDocL1.Text;
            string docSeries = txtAccountTransSeriesL1.Text;
            double docNumber = 0;
            double.TryParse(txtAccountTransDocNumberL1.Text, out docNumber);
            double paymentValue = 0;
            double.TryParse(txtAccountTransDocValueL1.Text, out paymentValue);
            if (paymentValue > 0) {
                AccountTransAddDetail(accountTransManager, accountUsed, ACCOUNT_TYPE, docId, docSeries, docNumber, 0, paymentValue);
            }
            // Linha 2
            docId = txtAccountTransDocL2.Text;
            docSeries = txtAccountTransSeriesL2.Text;
            docNumber = 0;
            double.TryParse(txtAccountTransDocNumberL2.Text, out docNumber);
            paymentValue = 0;
            double.TryParse(txtAccountTransDocValueL2.Text, out paymentValue);
            if (paymentValue > 0) {
                AccountTransAddDetail(accountTransManager, accountUsed, ACCOUNT_TYPE, docId, docSeries, docNumber, 0, paymentValue);
            }
            //
            // Não continuar se o documento não tiver linhas
            if (accountTrans.Details.Count == 0) {
                throw new Exception("O documento não tem linhas.");
            }
            //
            accountTrans.TenderLineItems = AccountTransGetTenderLineItems(accountTransManager);
            //
            // Abrir automaticamente o caixa, se estiver fechar
            accountTransManager.EnsureOpenTill(accountTrans.Till.TillID);
            //
            // Gravar
            if (!accountTransManager.SaveDocument(false)) {
                throw new Exception("A gravação do recibo falhou!");
            }
            else {
                result = accountTransManager.Transaction.TransactionID;
            }

            return result;
        }

        /// <summary>
        /// Preencher os meios de pagamentos utilizados no recebimento/pagamento
        /// </summary>
        TenderLineItemList AccountTransGetTenderLineItems(AccountTransactionManager accountTransManager) {
            //
            var accountTrans = accountTransManager.Transaction;
            var TenderLines = new TenderLineItemList();

            // Tender -- modo(s) de pagamento(s)
            short tenderId = dsoCache.TenderProvider.GetFirstTenderCash();
            short.TryParse(txtAccountTransPaymentId.Text, out tenderId);
            var tender = dsoCache.TenderProvider.GetTender(tenderId);
            // Add tender line
            var tenderLine = new TenderLineItem();
            tenderLine.Tender = tender;
            tenderLine.Amount = accountTrans.TotalAmount;
            // Caixa. dever ser a caixa aberta do sistema. Para simplificar colocou-se a default do sistema
            tenderLine.TillId = systemSettings.WorkstationInfo.DefaultMainTillID;
            tenderLine.TenderCurrency = accountTrans.BaseCurrency;
            tenderLine.PartyTypeCode = accountTrans.PartyTypeCode;
            tenderLine.PartyID = accountTrans.Entity.PartyID;
            tenderLine.CreateDate = DateTime.Today;
            //
            // Por uma questão de simplificação, neste exemplo apenas se vai considerar um pagamento de um só cheque.
            if (tender.TenderType == TenderTypeEnum.tndCheck) {
                TenderCheck tenderCheck = null;
                if (tenderLine.TenderCheck == null) {
                    tenderLine.TenderCheck = new TenderCheck();
                }
                tenderCheck = tenderLine.TenderCheck;
                tenderCheck.CheckAmount = tenderLine.Amount;
                tenderCheck.CheckDeferredDate = tenderLine.CreateDate;
                tenderCheck.TillId = tenderLine.TillId;

                tenderLine.TenderCheck = tenderCheck;
                var formCheck = new FormTenderCheck();
                if (formCheck.FillTenderCheck(tenderCheck) == System.Windows.Forms.DialogResult.Cancel) {
                    throw new Exception("É necessário preencher os dados do cheque.");
                }
            }
            TenderLines.Add(tenderLine);

            return TenderLines;
        }

        /// <summary>
        /// Lê e mostra no ecran um recibo ou pagamento
        /// </summary>
        private void AccountTransactionGet() {
            string accountTransSerial = txtAccountTransSerial.Text;
            string accountTransDoc = txtAccountTransDoc.Text;
            double accountTransDocNumber = 0;
            double.TryParse(txtAccountTransDocNumber.Text, out accountTransDocNumber);

            AccountTransactionClear();
            var transLoaded = accountTransManager.LoadTransaction(accountTransSerial, accountTransDoc, accountTransDocNumber);
            if (!transLoaded) {
                throw new Exception(string.Format("Não foi possível carregar o documento {0} {1}/{2}.", accountTransDoc, accountTransSerial, accountTransDocNumber));
            }
            var accountTrans = accountTransManager.Transaction;
            //
            txtAccountTransDoc.Text = accountTrans.TransDocument;
            txtAccountTransDocCurrency.Text = accountTrans.BaseCurrency.CurrencyID;
            txtAccountTransDocNumber.Text = accountTrans.TransDocNumber.ToString();
            txtAccountTransPartyId.Text = accountTrans.Entity.PartyID.ToString();
            txtAccountTransDocDate.Text = accountTrans.CreateDate.ToShortDateString();
            //txtAccountTransPaymentId.Text = accountTrans.;
            txtAccountTransSerial.Text = accountTrans.TransSerial;
            //
            if (accountTrans.TenderLineItems.Count > 0) {
                var tenderLine = accountTrans.TenderLineItems[1];
                txtAccountTransPaymentId.Text = tenderLine.Tender.TenderID.ToString();
            }

            // Line 1
            if (accountTrans.Details.Count > 0) {
                int i = 1;
                var detail = accountTrans.Details[ref i];
                txtAccountTransDocL1.Text = detail.DocID;
                txtAccountTransDocNumberL1.Text = detail.DocNumber.ToString();
                txtAccountTransDocValueL1.Text = detail.TotalPayedAmount.ToString();
                txtAccountTransSeriesL1.Text = detail.DocSerial;
                //
                // Line 2
                if (accountTrans.Details.Count > 1) {
                    i = 2;
                    detail = accountTrans.Details[ref i];
                    txtAccountTransDocL2.Text = detail.DocID;
                    txtAccountTransDocNumberL2.Text = detail.DocNumber.ToString();
                    txtAccountTransDocValueL2.Text = detail.TotalPayedAmount.ToString();
                    txtAccountTransSeriesL2.Text = detail.DocSerial;
                }
            }
            if (accountTrans.TransStatus == TransStatusEnum.stVoid) {
                tabAccount.BackgroundImage = Properties.Resources.stamp_Void;
            }
            else {
                tabAccount.BackgroundImage = null;
            }

            accountTrans = null;
        }


        /// <summary>
        /// Obtêm o documento por omissão para receibo ou pagamento
        /// </summary>
        /// <returns>O primeiro documetno encontrado para o tipo descrito</returns>
        private Document AccountTransGetDocument() {
            Document accountDoc = null;
            if (cmbRecPeg.SelectedIndex == 0) {
                // Primeiro documento disponivel para recebimento
                accountDoc = systemSettings.WorkstationInfo.Document.OfType<Document>().FirstOrDefault(x => x.TransDocType == DocumentTypeEnum.dcTypeAccount && x.UpdateTenderReport && x.AccountBehavior == AccountBehaviorEnum.abAccountSettlement && x.SignTenderReport == "+");
            }
            else {
                // Primeiro documento disponivel para pagamento
                accountDoc = systemSettings.WorkstationInfo.Document.OfType<Document>().FirstOrDefault(x => x.TransDocType == DocumentTypeEnum.dcTypeAccount && x.UpdateTenderReport && x.AccountBehavior == AccountBehaviorEnum.abAccountSettlement && x.SignTenderReport == "-");
            }
            return accountDoc;
        }

        /// <summary>
        /// Limpa a transação (recibo ou pagamento) do ecran e preenche alguns valores por omissão
        /// </summary>
        private void AccountTransactionClear() {
            if (cmbRecPeg.SelectedIndex < 0) {
                cmbRecPeg.SelectedIndex = 0;
            }

            var accountDoc = AccountTransGetDocument();
            if (accountDoc != null) {
                txtAccountTransDoc.Text = accountDoc.DocumentID;
            }
            else {
                txtAccountTransDoc.Text = string.Empty;
            }
            var externalSeries = systemSettings.DocumentSeries.OfType<DocumentsSeries>().FirstOrDefault(x => x.SeriesType == SeriesTypeEnum.SeriesExternal);
            if (externalSeries != null) {
                txtAccountTransSerial.Text = externalSeries.Series;
            }
            else {
                txtAccountTransSerial.Text = string.Empty;
            }
            txtAccountTransDocNumber.Text = "0";
            txtAccountTransDocCurrency.Text = systemSettings.BaseCurrency.CurrencyID;
            txtAccountTransPartyId.Text = string.Empty;
            txtAccountTransDocDate.Text = DateTime.Today.ToShortDateString();
            var tender = dsoCache.TenderProvider.GetFirstMoneyTender(TenderUseEnum.tndUsedOnBoth);
            if (tender != null) {
                txtAccountTransPaymentId.Text = tender.TenderID.ToString();
            }
            else {
                txtAccountTransPaymentId.Text = string.Empty;
            }
            txtAccountTransPaymentId.Text = dsoCache.PaymentProvider.GetFirstID().ToString();
            //
            AccountTransClearL1();
            AccountTransClearL2();
            RepClear();
            //
            tabAccount.BackgroundImage = null;
        }

        private void AccountTransClearL1() {
            txtAccountTransSeriesL1.Text = string.Empty;
            txtAccountTransDocL1.Text = string.Empty;
            txtAccountTransDocNumberL1.Text = "0";
            txtAccountTransDocValueL1.Text = "0";
        }

        private void AccountTransClearL2() {
            txtAccountTransSeriesL2.Text = string.Empty;
            txtAccountTransDocL2.Text = string.Empty;
            txtAccountTransDocNumberL2.Text = "0";
            txtAccountTransDocValueL2.Text = "0";
        }

        private void btnAccountClearL1_Click(object sender, EventArgs e) {
            AccountTransClearL1();
        }

        private void btnAccountClearL2_Click(object sender, EventArgs e) {
            AccountTransClearL2();
        }

        private void cmbRecPeg_SelectedIndexChanged(object sender, EventArgs e) {
            switch (cmbRecPeg.SelectedIndex) {
                case 0:
                    lblAccountPartyId.Text = "Cliente";
                    tabAccount.Text = "Recibo";
                    break;

                case 1:
                    lblAccountPartyId.Text = "Fornecedor";
                    tabAccount.Text = "Pagamento";
                    break;
            }
            var accountDoc = AccountTransGetDocument();
            if (accountDoc != null)
                txtAccountTransDoc.Text = accountDoc.DocumentID;
            else
                txtAccountTransDoc.Text = string.Empty;
        }

        #endregion

        private void rbTransStock_CheckedChanged(object sender, EventArgs e) {
            tabTransModules.Visible = false;
            lblTransModules.Visible = false;
            txtTransGlobalDiscount.Enabled = false;
            dataGridItemLines.Visible = false;
            btnRefreshGridLines.Visible = false;

            if (rbTransStock.Checked) {
                TransactionClear();
            }
        }

        private void rbTransBuySell_CheckedChanged(object sender, EventArgs e) {
            tabTransModules.Visible = true;
            lblTransModules.Visible = true;
            txtTransGlobalDiscount.Enabled = true;
            dataGridItemLines.Visible = false;
            btnRefreshGridLines.Visible = false;

            if (rbTransBuySell.Checked) {
                TransactionClear();
            }
        }

        private void cmbTransPartyType_SelectedIndexChanged(object sender, EventArgs e) {
            TransactionClear();
        }

        private void btnPrint_Click(object sender, EventArgs e) {
            double transDocNumber = 0;
            double.TryParse(txtTransDocNumber.Text, out transDocNumber);

            try {
                // Mostrar no ecran
                TransactionGet(false);
                //
                if (optPrintOptions0.Checked) {
                    //Imprimir com as regras default da 50c e caixa de diálogo
                    TransactionPrint(txtTransSerial.Text, txtTransDoc.Text, transDocNumber, chkPrintPreview.Checked);
                }
                else {
                    // Impressão customizada, exportação para PDF, ...
                    TransactionPrint2(txtTransSerial.Text, txtTransDoc.Text, transDocNumber);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Impressão normal via caixa de diálogo e regras da 50c
        /// </summary>
        /// <param name="transSerial"></param>
        /// <param name="transDoc"></param>
        /// <param name="transDocNumber"></param>
        /// <param name="printPreview"></param>
        private void TransactionPrint(string transSerial, string transDoc, double transDocNumber, bool printPreview) {
            if (printPreview) {
                bsoItemTransaction.PrintTransaction(transSerial, transDoc, transDocNumber, PrintJobEnum.jobPreview, 1);
            }
            else {
                bsoItemTransaction.PrintTransaction(transSerial, transDoc, transDocNumber, PrintJobEnum.jobPrint, 1);
            }
        }

        private void tabItem_Click(object sender, EventArgs e) {
            //TODO: Perguntar ao Jorge
        }

        #region QuickSearch

        private void btnItemBrow_Click(object sender, EventArgs e) {
            var item = QuickSearchHelper.ItemFind();
            if (!string.IsNullOrEmpty(item)) {
                ItemGet(item);
            }
        }

        private void btnCustomerBrow_Click(object sender, EventArgs e) {
            var customerId = QuickSearchHelper.CustomerFind();
            if (customerId > 0) {
                numCustomerId.Value = (decimal)customerId;
                CustomerGet(customerId);
            }
        }


        private void btnSupplierBrow_Click(object sender, EventArgs e) {
            var supplierId = QuickSearchHelper.SupplierFind();
            if (supplierId > 0) {
                SupplierGet(supplierId);
            }
        }

        #endregion

        private void btnTransGetPrep_Click(object sender, EventArgs e) {
            try {
                TransactionGet(true);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnSavePrep_Click(object sender, EventArgs e) {
            TransactionID result = null;
            try {
                if (bsoItemTransaction.Transaction.TempTransIndex != 0) {
                    // Atualizar
                    result = TransactionEdit(true);
                }
                else {
                    result = TransactionInsert(true);
                }
                if (result != null) {
                    TransactionClear();
                    MessageBox.Show(string.Format("Colocado em preparação: {0}", result.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else {
                    MessageBox.Show(string.Format("Não foi possível colocar em preparação: {0} {1}/{2}",
                                                   txtTransSerial.Text, txtTransDoc.Text, txtTransDocNumber.Text),
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        static bool tempTransactionIndexIsFinding = false;
        private double TempTransactionIndexFind(DocumentTypeEnum transDocType) {
            double result = 0;

            //show data for view with id=0: the title is fetched by the
            //quick search viewer.
            try {
                if (!tempTransactionIndexIsFinding) {
                    tempTransactionIndexIsFinding = true;
                    var quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_TempTransaction, false);

                    if (!systemSettings.SystemInfo.CanRestoreTempTranOnAll) {
                        quickSearch.ExtraWhereClause = "[Terminal]=" + systemSettings.WorkstationInfo.WorkstationID.ToString() + " AND [TransDocType]= " + ((int)transDocType).ToString();
                    }
                    else {
                        quickSearch.ExtraWhereClause = "[TransDocType]= " + ((int)transDocType).ToString();
                    }

                    if (quickSearch.SelectValue()) {
                        result = quickSearch.ValueSelectedDouble();
                    }
                    else {
                        result = -1;
                    }

                    tempTransactionIndexIsFinding = false;
                }
            }
            catch (Exception ex) {
                tempTransactionIndexIsFinding = false;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return result;
        }

        private void btnTransactionFinalize_Click(object sender, EventArgs e) {
            try {
                string transDoc = txtTransDoc.Text;
                string transSerial = txtTransSerial.Text;
                double transdocNumber = 0;

                if (double.TryParse(txtTransDocNumber.Text, out transdocNumber)) {
                    if (bsoItemTransaction.FinalizeSuspendedTransaction(transSerial, transDoc, transdocNumber)) {
                        MessageBox.Show(string.Format("Documento finalizado: {0}", bsoItemTransaction.Transaction.TransactionID.ToString()),
                                         Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else {
                        MessageBox.Show(string.Format("Não foi possível finalizar o documento suspenso: {0} {1}/{2}.", transDoc, transSerial, transdocNumber),
                                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else {
                    MessageBox.Show(string.Format("O número do documento ({0}) não é válido.", txtTransDocNumber.Text),
                                     Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnAccoutTransPrint_Click(object sender, EventArgs e) {
            try {
                // Carregar o documento
                AccountTransactionGet();

                // Pré-visualizar ou Imprimir
                if (chkAccoutTransPrintPreview.Checked) {
                    accountTransManager.ExecuteFunction("PREVIEW", string.Empty);
                }
                else {
                    accountTransManager.ExecuteFunction("PRINT", string.Empty);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void TransactionPrint2(string transSerial, string transDoc, double transDocNumber) {
            clsLArrayObject objListPrintSettings;
            PrintSettings oPrintSettings = null;
            Document oDocument = null;
            PlaceHolders oPlaceHolders = new PlaceHolders();

            btnPrint.Enabled = false;

            oPlaceHolders = new PlaceHolders();

            try {
                oDocument = systemSettings.WorkstationInfo.Document[transDoc];

                // Preencher as opções default
                var defaultPrintSettings = new PrintSettings() {
                    AskForPrinter = false,
                    UseIssuingOutput = false,
                    PrintAction = chkPrintPreview.Checked ? PrintActionEnum.prnActPreview : PrintActionEnum.prnActPrint
                };
                if (optPrintOptions1.Checked) {
                    // Exportar para PDF
                    defaultPrintSettings.PrintAction = PrintActionEnum.prnActExportToFile;
                    defaultPrintSettings.ExportFileType = ExportFileTypeEnum.filePDF;
                    defaultPrintSettings.ExportFileFolder = oPlaceHolders.GetPlaceHolderPath(systemSettings.WorkstationInfo.PDFDestinationFolder);
                }
                //Obter configurações de impressão na configuração de postos
                objListPrintSettings = printingManager.GetTransactionPrintSettings(oDocument, transSerial, ref defaultPrintSettings);
                //
                if (objListPrintSettings.getCount() > 0) {
                    // Neste exemplo, vamos escolher a primeira configuração
                    // Se houverem mais configuradas, deve-se alterar para a pretendida
                    oPrintSettings = (PrintSettings)objListPrintSettings.item[0];
                    // Imprimir...
                    // Retorna falso em caso de erro
                    if (chkPrintPreview.Checked) {
                        bsoItemTransaction.PrintTransaction(transSerial, transDoc, transDocNumber, PrintJobEnum.jobPreview, oPrintSettings.PrintCopies);
                    }
                    else {
                        bsoItemTransaction.PrintTransaction
                            (transSerial, transDoc, transDocNumber,
                            PrintJobEnum.jobPrint, oPrintSettings.PrintCopies,
                            oPrintSettings);
                    }
                }
                MessageBox.Show("Concluido.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally {
                btnPrint.Enabled = true;
                oDocument = null;
                oPlaceHolders = null;
            }
        }

        //private void cmbItemColor_SelectedIndexChanged(object sender, EventArgs e) {
        //    cmbItemSize.ResetText();
        //    dataGridView1.Columns.Clear();
        //    dataGridView1.RowHeadersVisible = false;
        //    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        //    dataGridView1.DataSource = GetGridDataColor();
        //    dataGridView1.Columns[0].Visible = false;
        //    dataGridView1.AutoSize = true;
        //    dataGridView1.Refresh();
        //}

        //private DataTable GetGridDataColor() {
        //    var mainProvider = APIEngine.DataManager.MainProvider;
        //    ItemColor color = (ItemColor)cmbItemColor.SelectedItem;
        //    string query = "SELECT Stock.ItemID, Stock.ColorID, Stock.SizeID, ItemSize.SequenceNumber, Stock.WarehouseID, Stock.PhysicalQty, Size.Description AS SizeDescription" +
        //                   " FROM ((Stock " +
        //                   " inner join Size on Stock.SizeID = Size.SizeID) " +
        //                   " inner join ItemSize on Stock.SizeID = ItemSize.SizeID AND Stock.ItemID = ItemSize.ItemID) " +
        //                   " WHERE Stock.ItemID = '" + mainProvider.SQLFormatter.SQLString(txtItemId.Text) + "' AND " +
        //                   " ColorID = " + mainProvider.SQLFormatter.SQLNumber(color.ColorID) +
        //                   " ORDER BY SequenceNumber, WarehouseID";

        //    object recsAffected = new object();
        //    ADODB.Recordset rs = mainProvider.Execute(query);

        //    DataTable dt = new DataTable();

        //    var keyCol = dt.Columns.Add("SizeId", typeof(int));
        //    keyCol.ColumnName = "Tamanho";
        //    dt.PrimaryKey = new DataColumn[] { keyCol };

        //    var colSizeDesc = dt.Columns.Add("Desc.", typeof(string));

        //    var warehouseList = dsoCache.WarehouseProvider.GetWarehouseList();
        //    foreach (Warehouse ware in warehouseList) {
        //        dt.Columns.Add(ware.WarehouseID.ToString(), typeof(double));
        //    }

        //    while (!rs.EOF) {
        //        var sizeId = (int)rs.Fields["SizeId"].Value;
        //        var warehouseId = (int)rs.Fields["WarehouseId"].Value;
        //        DataColumn col = dt.Columns[warehouseId.ToString()];

        //        var row = dt.Rows.Find(sizeId);
        //        if (row == null) {
        //            row = dt.NewRow();
        //            row[keyCol] = sizeId;
        //            row[colSizeDesc] = rs.Fields["SizeDescription"].Value.ToString();
        //            dt.Rows.Add(row);
        //        }
        //        row[col] = Math.Round((double)rs.Fields["PhysicalQty"].Value, 5);

        //        rs.MoveNext();
        //    }
        //    rs.Close();
        //    rs = null;

        //    return dt;
        //}

        //private void cmbItemSize_SelectedIndexChanged(object sender, EventArgs e) {
        //    cmbItemColor.ResetText();
        //    dataGridView1.Columns.Clear();
        //    dataGridView1.RowHeadersVisible = false;
        //    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        //    dataGridView1.DataSource = GetGridDataSize();
        //    dataGridView1.Columns[0].Visible = false;
        //    dataGridView1.AutoSize = true;
        //    dataGridView1.Refresh();
        //}

        //private DataTable GetGridDataSize() {
        //    var mainProvider = APIEngine.DataManager.MainProvider;
        //    ItemSize size = (ItemSize)cmbItemSize.SelectedItem;
        //    string query = "SELECT Stock.ItemID, Stock.ColorID, Stock.SizeID, ItemColor.SequenceNumber, Stock.WarehouseID, Stock.PhysicalQty, Color.Description AS ColorDescription" +
        //                   " FROM ((Stock " +
        //                   " inner join Color on Stock.ColorID = Color.ColorID) " +
        //                   " inner join ItemColor on Stock.ColorID = ItemColor.ColorID AND Stock.ItemID = ItemColor.ItemID) " +
        //                   " WHERE Stock.ItemID = '" + mainProvider.SQLFormatter.SQLString(txtItemId.Text) + "' AND " +
        //                   " Stock.SizeID = " + mainProvider.SQLFormatter.SQLNumber(size.SizeID) +
        //                   " ORDER BY SequenceNumber, WarehouseID";



        //    var rs = mainProvider.Execute(query);

        //    DataTable dt = new DataTable();

        //    var keyCol = dt.Columns.Add("ColorId", typeof(int));
        //    keyCol.ColumnName = "Cor";
        //    dt.PrimaryKey = new DataColumn[] { keyCol };

        //    var colColorDesc = dt.Columns.Add("Desc.", typeof(string));

        //    var warehouseList = dsoCache.WarehouseProvider.GetWarehouseList();
        //    foreach (Warehouse ware in warehouseList) {
        //        dt.Columns.Add(ware.WarehouseID.ToString(), typeof(double));
        //    }

        //    while (!rs.EOF) {
        //        var colorId = (int)rs.Fields["ColorId"].Value;
        //        var warehouseId = (int)rs.Fields["WarehouseId"].Value;
        //        DataColumn col = dt.Columns[warehouseId.ToString()];

        //        var row = dt.Rows.Find(colorId);
        //        if (row == null) {
        //            row = dt.NewRow();
        //            row[keyCol] = colorId;
        //            row[colColorDesc] = rs.Fields["ColorDescription"].Value.ToString();
        //            dt.Rows.Add(row);
        //        }
        //        row[col] = Math.Round((double)rs.Fields["PhysicalQty"].Value, 5);

        //        rs.MoveNext();
        //    }
        //    rs.Close();
        //    rs = null;

        //    return dt;
        //}

        private ItemTransactionDetailList GetItemComponentList(int LineID) {
            var itemDetails = new ItemTransactionDetailList();
            string itemID = string.Empty;
            double Quantity = 0;
            string UnitOfSaleID = string.Empty;
            double UnitPrice = 0;
            short WarehouseID = 0;

            CurrencyDefinition currency = new CurrencyDefinition();

            var doc = systemSettings.WorkstationInfo.Document[txtTransDoc.Text];
            ItemTransactionDetail itemComponent = new ItemTransactionDetail();
            ItemTransactionDetail StockTransactionDetail = new ItemTransactionDetail();

            if (string.IsNullOrEmpty(txtTransCurrency.Text)) {
                currency = systemSettings.BaseCurrency;
            }
            else {

                currency = dsoCache.CurrencyProvider.GetCurrency(txtTransCurrency.Text);
                if (currency == null) {
                    currency = systemSettings.BaseCurrency;
                }
            }

            switch (LineID) {
                case 1:
                    itemID = txtTransItemL1.Text.Trim();
                    Quantity = txtTransQuantityL1.Text.ToDouble();
                    UnitOfSaleID = txtTransUnL1.Text;
                    UnitPrice = txtTransUnitPriceL1.Text.ToDouble();
                    WarehouseID = txtTransWarehouseL1.Text.ToShort();

                    break;
                case 2:
                    itemID = txtTransItemL2.Text.Trim();
                    Quantity = txtTransQuantityL2.Text.ToDouble();
                    UnitOfSaleID = txtTransUnL2.Text;
                    UnitPrice = txtTransUnitPriceL2.Text.ToDouble();
                    WarehouseID = txtTransWarehouseL2.Text.ToShort();

                    break;
            }

            var item = itemProvider.GetItem(itemID, currency, false);
            if (item != null) {
                if (item.ItemType == ItemTypeEnum.itmManufactured) {
                    if (doc.StockBehavior == StockBehaviorEnum.sbStockCompose || doc.StockBehavior == StockBehaviorEnum.sbStockDecompose) {
                        string strPriceTag = doc.DefaultPrice.Substring(0, 3);// "PVP"
                        short intSalePriceLevel = doc.DefaultPrice.Substring(3, 1).ToShort();

                        bsoStockTransaction.BSOCommonTransaction.TransactionType = DocumentTypeEnum.dcTypeStock;

                        StockTransactionDetail.ItemID = itemID;
                        StockTransactionDetail.Quantity = Quantity;
                        StockTransactionDetail.UnitOfSaleID = UnitOfSaleID;
                        StockTransactionDetail.UnitPrice = UnitPrice;
                        StockTransactionDetail.WarehouseOutgoing = WarehouseID;
                        StockTransactionDetail.WarehouseReceipt = WarehouseID;
                        StockTransactionDetail.WarehouseID = WarehouseID;


                        if (doc.StockBehavior == StockBehaviorEnum.sbStockCompose) {
                            bsoStockTransaction.BSOCommonTransaction.TransactionStockBehavior = StockBehaviorEnum.sbStockCompose;
                            bsoStockTransaction.BSOCommonTransaction.TransactionPhysicalQtyRule = StockQtyRuleEnum.stkQtyReceipt;
                            StockTransactionDetail.PhysicalQtyRule = StockQtyRuleEnum.stkQtyReceipt;
                        }
                        else {
                            if (doc.StockBehavior == StockBehaviorEnum.sbStockDecompose) {
                                bsoStockTransaction.BSOCommonTransaction.TransactionStockBehavior = StockBehaviorEnum.sbStockDecompose;
                                bsoStockTransaction.BSOCommonTransaction.TransactionPhysicalQtyRule = StockQtyRuleEnum.stkQtyOutgoing;
                                StockTransactionDetail.PhysicalQtyRule = StockQtyRuleEnum.stkQtyOutgoing;
                            }
                        }

                        itemDetails = bsoStockTransaction.BSOCommonTransaction.GetComponentList(StockTransactionDetail, item.ItemCollection, StockTransactionDetail.Quantity, StockTransactionDetail.ItemExtraInfo.NeededComponents, StockTransactionDetail.ItemExtraInfo.UseComponentPrices, StockTransactionDetail.ItemExtraInfo.UseComponentPriceLineID, strPriceTag, intSalePriceLevel, 0);

                    }
                }
            }
            return itemDetails;
        }

        private void addComponentListToGrid(ItemTransactionDetailList ComponentList) {
            if (ComponentList != null) {
                foreach (ItemTransactionDetail value in ComponentList) {
                    var rowIndex = this.dataGridItemLines.Rows.Add(txtTransWarehouseL1.Text.ToDouble(),
                                                value.ItemID, value.UnitPrice,
                                                value.Quantity,
                                                value.UnitOfSaleID);
                    dataGridItemLines.Rows[rowIndex].Tag = value;
                }
            }
        }

        private void rbTransStockCompose_CheckedChanged(object sender, EventArgs e) {
            tabTransModules.Visible = false;
            lblTransModules.Visible = false;
            txtTransGlobalDiscount.Enabled = false;
            dataGridItemLines.Visible = true;
            btnRefreshGridLines.Visible = true;
            dataGridItemLines.Rows.Clear();

            if (rbTransStockCompose.Checked) {
                TransactionClear();
            }
        }

        private void rbTransStockDecompose_CheckedChanged(object sender, EventArgs e) {
            tabTransModules.Visible = false;
            lblTransModules.Visible = false;
            txtTransGlobalDiscount.Enabled = false;
            dataGridItemLines.Visible = true;
            btnRefreshGridLines.Visible = true;
            dataGridItemLines.Rows.Clear();

            if (rbTransStockDecompose.Checked) {
                TransactionClear();
            }
        }

        private void btnRefreshGridLines_Click(object sender, EventArgs e) {
            string transDoc = txtTransDoc.Text;

            if (!systemSettings.WorkstationInfo.Document.IsInCollection(transDoc)) {
                throw new Exception("O documento não se encontra preenchido ou não existe");
            }
            Document doc = systemSettings.WorkstationInfo.Document[transDoc];

            if (doc.TransDocType != DocumentTypeEnum.dcTypeStock) {
                throw new Exception(string.Format("O documento indicado não é um documento de stock", transDoc));
            }

            if (doc.StockBehavior == StockBehaviorEnum.sbStockCompose || doc.StockBehavior == StockBehaviorEnum.sbStockDecompose) {
                dataGridItemLines.Rows.Clear();

                var itemDetails = GetItemComponentList(1);
                addComponentListToGrid(itemDetails);

                itemDetails = GetItemComponentList(2);
                addComponentListToGrid(itemDetails);
            }
            else {
                throw new Exception(string.Format("O documento indicado não é um documento de fabricação/transformação", transDoc));
            }
        }

        private double getLineNumberTotxtTransItemL(int TransItemL, DocumentTypeEnum TransDocType, StockBehaviorEnum StockBehavior, ItemTransactionDetailList itemDetails) {
            double result = 0;
            int foundlines = 0;

            if (itemDetails != null) {
                if (TransDocType == DocumentTypeEnum.dcTypeStock) {
                    if (StockBehavior == StockBehaviorEnum.sbStockCompose) {
                        foreach (ItemTransactionDetail value in itemDetails) {
                            if (value.PhysicalQtyRule == StockQtyRuleEnum.stkQtyReceipt) {
                                ++foundlines;
                                if (foundlines == TransItemL) {
                                    result = value.LineItemID;
                                    break;
                                }
                            }
                        }
                    }
                    else {
                        if (StockBehavior == StockBehaviorEnum.sbStockDecompose) {
                            foreach (ItemTransactionDetail value in itemDetails) {
                                if (value.PhysicalQtyRule == StockQtyRuleEnum.stkQtyOutgoing) {
                                    ++foundlines;
                                    if (foundlines == TransItemL) {
                                        result = value.LineItemID;
                                        break;
                                    }
                                }
                            }
                        }
                        else {
                            result = TransItemL;
                        }
                    }
                }
                else {
                    result = TransItemL;
                }
            }
            else {
                result = 0;
            }

            return result;
        }

        private void fillComponentListGrid(StockBehaviorEnum StockBehavior, ItemTransactionDetailList itemDetails) {
            dataGridItemLines.Rows.Clear();

            if (StockBehavior == StockBehaviorEnum.sbStockCompose) {
                foreach (ItemTransactionDetail value in itemDetails) {
                    if (value.PhysicalQtyRule == StockQtyRuleEnum.stkQtyOutgoing) {
                        var rowIndex = this.dataGridItemLines.Rows.Add(value.WarehouseID,
                                                    value.ItemID, value.UnitPrice,
                                                    value.Quantity,
                                                    value.UnitOfSaleID);
                        dataGridItemLines.Rows[rowIndex].Tag = value;
                    }
                }
            }
            else {
                if (StockBehavior == StockBehaviorEnum.sbStockDecompose) {
                    foreach (ItemTransactionDetail value in itemDetails) {
                        if (value.PhysicalQtyRule == StockQtyRuleEnum.stkQtyReceipt) {
                            var rowIndex = this.dataGridItemLines.Rows.Add(value.WarehouseID,
                                                        value.ItemID, value.UnitPrice,
                                                        value.Quantity,
                                                        value.UnitOfSaleID);
                            dataGridItemLines.Rows[rowIndex].Tag = value;
                        }
                    }
                }
            }
        }

        private void btnExternalSignature_Click(object sender, EventArgs e) {
            MessageBox.Show("NOTA: Só é possível definir a assinatura sem séries externas.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

            using (var frm = new FormExternalSignature()) {
                frm.Signature = bsoItemTransaction.Transaction.Signature;
                frm.SignatureVersion = bsoItemTransaction.Transaction.SignatureControl;
                frm.SoftwareCertificateNumber = bsoItemTransaction.Transaction.SoftwareCertificateNumber;
                frm.ShowDialog(this);
            };
        }

        private bool SetExternalSignature(ItemTransaction trans) {
            var result = true;
            using (var formSig = new FormExternalSignature()) {
                if (trans != null) {
                    if (trans.TransBehavior == TransBehaviorEnum.BehAlwaysNewDocument) {
                        // Inserir assinatura aqui
                        formSig.Signature = "Exemplo de assinatura externa";
                        // Versão (atualmente=1)
                        formSig.SignatureVersion = 1;
                        // Número de certificação do software
                        formSig.SoftwareCertificateNumber = 999;
                    }
                    else {
                        // Inserir assinatura aqui
                        formSig.Signature = trans.Signature;
                        // Versão (atualmente=1)
                        formSig.SignatureVersion = trans.SignatureControl;
                        // Número de certificação do software
                        formSig.SoftwareCertificateNumber = trans.SoftwareCertificateNumber;
                    }

                    if (formSig.ShowDialog() == DialogResult.OK) {
                        var sigTransaction = (ISignableTransaction)trans;
                        APIEngine.SystemSettings.SignatureLoader.SetSignature(sigTransaction, formSig.Signature, formSig.SignatureVersion, formSig.SoftwareCertificateNumber);
                    }
                    else {
                        result = false;
                    }
                }
            }
            return result;
        }

        private void txtItemId_Click(object sender, EventArgs e) {
            APIEngine.CoreGlobals.ShowKeyPadInContext(txtItemId, "Text", VBA.VbCallType.VbLet);
        }

        private void btnClearRep1_Click(object sender, EventArgs e) {
            RepClear();
        }

        private void RepClear() {
            //Limpar
            txtShareTransDocument_R1.Text = string.Empty;
            txtShareTransDocument_R2.Text = string.Empty;
            txtShareTransSerial_R1.Text = string.Empty;
            txtShareTransSerial_R2.Text = string.Empty;
            txtShareTransDocNumber_R1.Text = string.Empty;
            txtShareTransDocNumber_R2.Text = string.Empty;
            txtShareAmount_R1.Text = string.Empty;
            txtShareAmount_R2.Text = string.Empty;
            txtAmout_R1_L1.Text = string.Empty;
            txtAmout_R1_L2.Text = string.Empty;
            LblL1.Text = string.Empty;
            LblL2.Text = string.Empty;
        }

        private void Fill_ShareDetails(string ShareTransSerial, string ShareTransDocument, string ShareTransDocumentNumber) {

            double TransDocNumber = 0;
            double.TryParse(txtShareTransDocNumber_R1.Text, out TransDocNumber);
            ItemTransaction objTempItemTransaction = new ItemTransaction();
            DSOItemTransaction objDSOItemTransaction = new DSOItemTransaction();

            objTempItemTransaction = objDSOItemTransaction.GetItemTransaction(DocumentTypeEnum.dcTypePurchase, ShareTransSerial, ShareTransDocument, TransDocNumber);
            LblL1.Text = string.Empty;
            LblL2.Text = string.Empty;

            if (objTempItemTransaction != null) {
                foreach (ItemTransactionDetail objTempItemTransactionDetail in objTempItemTransaction.Details) {

                    switch ((int)objTempItemTransactionDetail.LineItemID) {
                        case 1:
                            LblL1.Text = objTempItemTransactionDetail.ItemID;
                            break;

                        case 2:
                            LblL2.Text = objTempItemTransactionDetail.ItemID;
                            break;
                    }
                }
            }
            else {
                txtShareAmount_R1.Text = string.Empty;
                txtAmout_R1_L1.Text = string.Empty;
                txtAmout_R1_L2.Text = string.Empty;
                LblL1.Text = string.Empty;
                LblL2.Text = string.Empty;
            }
        }

        private void txtShareTransDocument_R1_LostFocus(object sender, EventArgs e) {
            Fill_ShareDetails(txtShareTransSerial_R1.Text, txtShareTransDocument_R1.Text, txtShareTransDocNumber_R1.Text);
        }

        private void txtShareTransSerial_R1_LostFocus(object sender, EventArgs e) {
            Fill_ShareDetails(txtShareTransSerial_R1.Text, txtShareTransDocument_R1.Text, txtShareTransDocNumber_R1.Text);
        }

        private void txtShareTransDocNumber_R1_LostFocus(object sender, EventArgs e) {
            Fill_ShareDetails(txtShareTransSerial_R1.Text, txtShareTransDocument_R1.Text, txtShareTransDocNumber_R1.Text);
        }

        private void btnTransactionRestoreTemp_Click(object sender, EventArgs e) {
            try {
                //Documento, série destino
                bsoItemTransaction.InitNewTransaction(txtTransDoc.Text, txtTransSerial.Text);
                //Nr. do documento temporário a importar.
                bsoItemTransaction.RestoreTempTransaction(DocumentTypeEnum.dcTypeSale, txtTransDocNumber.Text.ToInt());
                if (bsoItemTransaction.Transaction.Details.Count > 0) {

                    var Transaction = new GenericTransaction(bsoItemTransaction.Transaction);
                    TransactionShow(Transaction);

                    if (DialogResult.Yes ==
                        MessageBox.Show("Guardar o documento temporário recuperado?", Application.ProductName,
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                        //Após importar, se pretender, será gravado automaticamente o documento.
                        if (bsoItemTransaction.Transaction.Tender.TenderID == 0) {
                            // Set the first TenderId, just in case...
                            bsoItemTransaction.TenderID = APIEngine.DSOCache.TenderProvider.GetFirstID();
                        }
                        bsoItemTransaction.SaveDocument(false, false);
                        MessageBox.Show($"Documento gravado: {bsoItemTransaction.Transaction.TransactionID.ToString()}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TransactionClear();
                    }
                }
                else {
                    MessageBox.Show($"O temporário indicado '{txtTransDocNumber.Text}' não existe ou não existem mais temporários.",
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void TransactionShow(GenericTransaction trans) {
            if (trans != null) {
                var doc = APIEngine.SystemSettings.WorkstationInfo.Document[trans.TransDocument];

                TransactionClear();
                //txtTransColor1.Text = 
                txtTransCurrency.Text = trans.BaseCurrency.CurrencyID;
                txtTransDate.Text = trans.CreateDate.ToShortDateString();
                txtTransDoc.Text = trans.TransDocument;

                chkTransTaxIncluded.Checked = trans.TransactionTaxIncluded;

                txtTransDocNumber.Text = trans.TransDocNumber.ToString();

                txtPaymentID.Text = trans.PaymentID;
                txtTenderID.Text = trans.TenderID;

                //ItemTransaction i; i.PaymentDiscountPercent

                txtTransGlobalDiscount.Text = trans.PaymentDiscountPercent;
                txtTransGlobalDiscount.Enabled = trans.TransGlobalDiscountEnabled;

                txtTransPartyId.Text = trans.PartyID.ToString();
                txtTransSerial.Text = trans.TransSerial;

                //Linha 1
                if (trans.Details.Count > 0) {
                    int lineNumber = (int)getLineNumberTotxtTransItemL(1, (DocumentTypeEnum)doc.TransDocType, (StockBehaviorEnum)doc.StockBehavior, trans.Details);

                    if (lineNumber != 0) {
                        var transDetail = trans.Details[lineNumber];

                        txtTransFactorL1.Text = transDetail.QuantityFactor.ToString();
                        txtTransItemL1.Text = transDetail.ItemID;
                        txtTransQuantityL1.Text = transDetail.Quantity.ToString();
                        if (transDetail.TaxList.Count > 0) {
                            txtTransTaxRateL1.Text = transDetail.TaxList[1].TaxRate.ToString();
                        }
                        if (trans.TransactionTaxIncluded) {
                            txtTransUnitPriceL1.Text = transDetail.TaxIncludedPrice.ToString();
                        }
                        else {
                            txtTransUnitPriceL1.Text = transDetail.UnitPrice.ToString();
                        }
                        txtTransUnL1.Text = transDetail.UnitOfSaleID;
                        txtTransWarehouseL1.Text = transDetail.WarehouseID.ToString();
                        // Cores e Tamanhos - Só na linha 1 
                        if (transDetail.Color.ColorID > 0) {
                            txtTransColor1.Text = transDetail.Color.ColorID.ToString();
                            chkTransModuleSizeColor.Checked = true;
                        }
                        if (transDetail.Size.SizeID > 0) {
                            txtTransSize1.Text = transDetail.Size.SizeID.ToString();
                            chkTransModuleSizeColor.Checked = true;
                        }
                        // Propriedades: Números de série
                        if (transDetail.ItemProperties.HasPropertyValues) {
                            lblTransPropNameL1.Text = transDetail.ItemProperties.PropertyID1;
                            txtTransPropValueL1.Text = transDetail.ItemProperties.PropertyValue1;
                            // Também é possível utilizar as restantes 3 propriedades. Para isso necessitariamos de outra forma de apresentar os dados (com mais controlos, p.ex.)
                            //lblTransPropNameL1_2.Text = transDetail.ItemProperties.PropertyID2;
                            //txtTransPropValueL1_2.Text = transDetail.ItemProperties.PropertyValue2;
                            //lblTransPropNameL1_3.Text = transDetail.ItemProperties.PropertyID3;
                            //txtTransPropValueL1_3.Text = transDetail.ItemProperties.PropertyValue3;

                            chkTransModuleProps.Checked = true;
                        }
                    }

                    // Linha 2 - Não tem cores e tamanhos 
                    if (trans.Details.Count > 1) {
                        int line = (int)getLineNumberTotxtTransItemL(2, (DocumentTypeEnum)doc.TransDocType, (StockBehaviorEnum)doc.StockBehavior, trans.Details);

                        if (line != 0) {
                            var transDetail = trans.Details[line];
                            txtTransFactorL2.Text = transDetail.QuantityFactor.ToString();
                            txtTransItemL2.Text = transDetail.ItemID;
                            txtTransQuantityL2.Text = transDetail.Quantity.ToString();
                            if (transDetail.TaxList.Count > 0) {
                                txtTransTaxRateL2.Text = transDetail.TaxList[1].TaxRate.ToString();
                            }
                            if (trans.TransactionTaxIncluded) {
                                txtTransUnitPriceL2.Text = transDetail.TaxIncludedPrice.ToString();
                            }
                            else {
                                txtTransUnitPriceL2.Text = transDetail.UnitPrice.ToString();
                            }
                            txtTransUnL2.Text = transDetail.UnitOfSaleID;
                            txtTransWarehouseL2.Text = transDetail.WarehouseID.ToString();
                            // Propriedades: Números de série
                            if (transDetail.ItemProperties.HasPropertyValues) {
                                lblTransPropNameL2.Text = transDetail.ItemProperties.PropertyID1;
                                txtTransPropValueL2.Text = transDetail.ItemProperties.PropertyValue1;

                                chkTransModuleProps.Checked = true;
                            }
                        }
                    }

                    if (bsoItemTransaction.Transaction.BuyShareOtherCostList.Count > 0) {

                        SimpleDocumentList objDocumentList = new SimpleDocumentList();
                        objDocumentList = bsoItemTransaction.Transaction.BuyShareOtherCostList;

                        ItemTransaction objTempItemTransaction = new ItemTransaction();
                        DSOItemTransaction objDSOItemTransaction = new DSOItemTransaction();

                        string sDetailKey;
                        foreach (SimpleDocument objDocument in objDocumentList) {

                            if (objDocument.Details.Count > 0) {

                                objTempItemTransaction = objDSOItemTransaction.GetItemTransaction(DocumentTypeEnum.dcTypePurchase, objDocument.TransID.TransSerial, objDocument.TransID.TransDocument, objDocument.TransID.TransDocNumber);

                                txtShareTransSerial_R1.Text = objDocument.TransID.TransSerial;
                                txtShareTransDocument_R1.Text = objDocument.TransID.TransDocument;
                                txtShareTransDocNumber_R1.Text = objDocument.TransID.TransDocNumber.ToString();
                                txtShareAmount_R1.Text = objDocument.TotalTransactionAmount.ToString();

                                foreach (ItemTransactionDetail objTempItemTransactionDetail in objTempItemTransaction.Details) {

                                    sDetailKey = objDocument.TransID.TransSerial + "|" + objDocument.TransID.TransDocument + "|" + objDocument.TransID.TransDocNumber.ToString() + "|" + objTempItemTransactionDetail.LineItemID.ToString() + "|" + objTempItemTransactionDetail.ItemID + "|" + objTempItemTransactionDetail.Color.ColorID + "|" + objTempItemTransactionDetail.Size.SizeID;

                                    switch ((int)objTempItemTransactionDetail.LineItemID) {
                                        case 1:
                                            LblL1.Text = objDocument.Details.ItemByIndex[1].ItemID;
                                            txtAmout_R1_L1.Text = objDocument.Details.ItemByIndex[1].UnitPrice.ToString();
                                            break;

                                        case 2:
                                            LblL2.Text = objDocument.Details.ItemByIndex[2].ItemID;
                                            txtAmout_R1_L2.Text = objDocument.Details.ItemByIndex[2].UnitPrice.ToString();
                                            break;
                                    }
                                }
                            }
                            else {
                                txtShareTransSerial_R2.Text = objDocument.TransID.TransSerial;
                                txtShareTransDocument_R2.Text = objDocument.TransID.TransDocument;
                                txtShareTransDocNumber_R2.Text = objDocument.TransID.TransDocNumber.ToString();
                                txtShareAmount_R2.Text = objDocument.TotalTransactionAmount.ToString();
                            }
                        }
                    }
                }

                //Fabricação/Transformação - restantes linhas
                if ((int)doc.StockBehavior == (int)StockBehaviorEnum.sbStockCompose || (int)doc.StockBehavior == (int)StockBehaviorEnum.sbStockDecompose) {
                    fillComponentListGrid((StockBehaviorEnum)doc.StockBehavior, trans.Details);
                }

                // O Documento está anulado ?
                if ((int)trans.TransStatus == (int)TransStatusEnum.stVoid) {
                    tabBuySaleTransaction.BackgroundImage = Properties.Resources.stamp_Void;
                }
                else {
                    tabBuySaleTransaction.BackgroundImage = null;
                }
            }
            else {
                MessageBox.Show("A transação indicada não existe.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnTest_Click(object sender, EventArgs e) {
            bsoItemTransaction.Transaction.Taxes.Remove("IVA", 1, TaxItemTypeEnum.txitmProduct);
        }

        private void lblTransDocNumber_Click(object sender, EventArgs e) {
            try {
                var transSerial = string.IsNullOrEmpty(txtTransSerial.Text) ? APIEngine.SystemSettings.WorkstationInfo.DefaultTransSerial : txtTransSerial.Text;
                var transDocument = string.IsNullOrEmpty(txtTransDoc.Text) ? APIEngine.SystemSettings.WorkstationInfo.DefaultTransDocument : txtTransDoc.Text;
                double transdocNumber = 0;
                if (rbTransBuySell.Checked) {
                    transdocNumber = QuickSearchHelper.ItemTransactionFind(transSerial, transDocument);
                }
                else {
                    transdocNumber = QuickSearchHelper.StockTransactionFind(transSerial, transDocument);
                }
                if (transdocNumber > 0) {
                    txtTransDoc.Text = transDocument;
                    txtTransSerial.Text = transSerial;
                    txtTransDocNumber.Text = transdocNumber.ToString();
                    // Load the document
                    TransactionGet(false);
                    txtTransDocNumber.Focus();
                }
            }
            catch (Exception ex) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
        }

        private void lblAccountTransDocNumber_Click(object sender, EventArgs e) {
            try {
                var transSerial = string.IsNullOrEmpty(txtAccountTransSerial.Text) ? APIEngine.SystemSettings.WorkstationInfo.DefaultTransSerial : txtAccountTransSerial.Text;
                string transDocument = txtAccountTransDoc.Text;
                if (string.IsNullOrEmpty(transDocument)) {
                    if (cmbRecPeg.SelectedIndex == 0) {
                        // Recebimentos
                        transDocument = APIEngine.SystemSettings.WorkstationInfo.Document.FindByNature(TransactionNatureEnum.Account_Customer)
                                                                                         .OfType<Document>()
                                                                                         .FirstOrDefault(d => !d.Inactive)
                                                                                         .DocumentID;
                    }
                    else {
                        // Pagamentos
                        transDocument = APIEngine.SystemSettings.WorkstationInfo.Document.FindByNature(TransactionNatureEnum.Account_Supplier)
                                                                                         .OfType<Document>()
                                                                                         .FirstOrDefault(d => !d.Inactive)
                                                                                         .DocumentID;
                    }
                }
                //
                var transdocNumber = QuickSearchHelper.AccountTransactionFind(transSerial, transDocument);
                if (transdocNumber > 0) {
                    txtAccountTransDoc.Text = transDocument;
                    txtAccountTransSerial.Text = transSerial;
                    txtAccountTransDocNumber.Text = transdocNumber.ToString();
                    txtAccountTransDocNumber.Focus();
                    // Load the document
                    AccountTransactionGet();
                }
            }
            catch (Exception ex) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
        }

        private void btnAddColor_Click(object sender, EventArgs e) {

            var colorID = QuickSearchHelper.ColorFind();
            if (colorID > 0) {
                var colorToAdd = APIEngine.DSOCache.ColorProvider.GetColor((short)colorID);

                var isDuplicate = false;
                foreach (DataGridViewRow colorRow in dgvColor.Rows) {
                    var colorRowID = (short)colorRow.Cells[0].Value;

                    if (colorRowID == colorToAdd.ColorID) {
                        APIEngine.CoreGlobals.MsgBoxFrontOffice("Não é possível adicionar a mesma cor mais do que uma vez.", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                        isDuplicate = true;
                        break;
                    }
                }

                if (!isDuplicate) {
                    var newRowIndex = dgvColor.Rows.Add();
                    var newRow = dgvColor.Rows[newRowIndex];

                    newRow.Cells[0].Value = colorToAdd.ColorID;
                    newRow.Cells[1].Style.BackColor = ColorTranslator.FromOle((int)colorToAdd.ColorCode);
                    newRow.Cells[2].Value = colorToAdd.Description;
                }
            }
        }

        private void btnRemoveColor_Click(object sender, EventArgs e) {

            if (dgvColor.CurrentCell != null) {
                dgvColor.Rows.RemoveAt(dgvColor.CurrentCell.RowIndex);
            }
            else {
                APIEngine.CoreGlobals.MsgBoxFrontOffice("Selecione uma cor para remover.", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
            }
        }

        private void btnAddSize_Click(object sender, EventArgs e) {

            var size = QuickSearchHelper.SizeFind();
            if (size > 0) {
                var sizeToAdd = APIEngine.DSOCache.SizeProvider.GetSize((short)size);

                var isDuplicate = false;
                foreach (DataGridViewRow sizeRow in dgvSize.Rows) {
                    var sizeId = (short)sizeRow.Cells[0].Value;

                    if (sizeId == sizeToAdd.SizeID) {
                        APIEngine.CoreGlobals.MsgBoxFrontOffice("Não é possível adicionar o mesmo tamanho mais do que uma vez.", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                        isDuplicate = true;
                        break;
                    }
                }

                if (!isDuplicate) {
                    var newRowIndex = dgvSize.Rows.Add();
                    var newRow = dgvSize.Rows[newRowIndex];

                    newRow.Cells[0].Value = sizeToAdd.SizeID;
                    newRow.Cells[1].Value = sizeToAdd.Description;
                }
            }
        }

        private void btnRemoveSize_Click(object sender, EventArgs e) {

            if (dgvSize.CurrentCell != null) {
                dgvSize.Rows.RemoveAt(dgvSize.CurrentCell.RowIndex);
            }
            else {
                APIEngine.CoreGlobals.MsgBoxFrontOffice("Selecione um tamanho para remover.", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
            }
        }

        private void FormatColorGrid() {

            var ColorID = new DataGridViewTextBoxColumn {
                HeaderText = "Cód.",
                Name = "ColorID",
                ReadOnly = true,
                Width = 50
            };

            var ColorUI = new DataGridViewTextBoxColumn {
                HeaderText = "Cor",
                Name = "ColorUI",
                ReadOnly = true,
                Width = 50
            };

            var ColorDescription = new DataGridViewTextBoxColumn {
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                HeaderText = "Descrição",
                Name = "Description",
                ReadOnly = true
            };

            ApplyGridStyle(dgvColor, new DataGridViewColumn[] {
                ColorID,
                ColorUI,
                ColorDescription
            });
        }

        private void FormatSizeGrid() {

            var SizeID = new DataGridViewTextBoxColumn {
                HeaderText = "Cód.",
                Name = "SizeID",
                ReadOnly = true,
                Width = 50
            };

            var SizeDescription = new DataGridViewTextBoxColumn {
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                HeaderText = "Tamanho",
                Name = "Description",
                ReadOnly = true
            };

            ApplyGridStyle(dgvSize, new DataGridViewColumn[] {
                SizeID,
                SizeDescription
            });
        }

        private void ApplyGridStyle(DataGridView dgv, DataGridViewColumn[] columns) {

            dgv.BackgroundColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.WindowBackColor);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.AppHeaderBackColor);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.TextNoFocus);
            dgv.GridColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.LightGray);

            dgv.RowsDefaultCellStyle.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.WindowBackColor);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.TabBackColor);

            dgv.Columns.Clear();
            dgv.Rows.Clear();
            dgv.Columns.AddRange(columns);
        }

        private void AddColorsToItem(Item item) {

            // Limpar as cores anteriores
            item.Colors.Clear();
            // Adicionar as cores atualizadas
            foreach (DataGridViewRow colorRow in dgvColor.Rows) {
                var colorID = (short)colorRow.Cells[0].Value;
                var color = APIEngine.DSOCache.ColorProvider.GetColor(colorID);

                var newItemColor = new ItemColor() {
                    ColorID = color.ColorID,
                    ColorName = color.Description,
                    ColorCode = (int)color.ColorCode,
                };

                item.Colors.Add(newItemColor);
            }
        }

        private void btnCreateColor_Click(object sender, EventArgs e) {
            fColor colorForm = new fColor();
            colorForm.Show();
        }

        private void AddSizesToItem(Item item) {

            item.Sizes.Clear();
            foreach (DataGridViewRow sizeRow in dgvSize.Rows) {
                var sizeID = (short)(sizeRow.Cells[0].Value);
                var size = APIEngine.DSOCache.SizeProvider.GetSize(sizeID);

                var newItemSize = new ItemSize() {
                    SizeID = size.SizeID,
                    SizeName = size.Description,
                    Quantity = 1,
                    Units = 1,
                };

                item.Sizes.Add(newItemSize);
            }
        }

        private void btnCreateSize_Click(object sender, EventArgs e) {
            FormSizes formSizes = new FormSizes();
            formSizes.ShowDialog();
        }
    }
}
