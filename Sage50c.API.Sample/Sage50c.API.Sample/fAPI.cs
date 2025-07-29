using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using S50cBL22;
using S50cBO22;
using S50cDL22;
using S50cPrint22;
using S50cSys22;
using S50cUtil22;
using Sage50c.API.Sample.Controllers;
using Sage50c.API.Sample.Helpers;

namespace Sage50c.API.Sample {
    public partial class fApi : Form {

        /// <summary>
        /// Parâmetros do sistema
        /// </summary>
        private SystemSettings systemSettings { get { return APIEngine.SystemSettings; } }
        /// <summary>
        /// Cache dos motores de acesso a dados mais comuns
        /// </summary>
        private DSOFactory dsoCache { get { return APIEngine.DSOCache; } }
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

        private ItemController _itemController = null;
        private CustomerController _customerController = null;
        private SupplierController _supplierController = null;
        private ItemTransactionController _itemTransactionController = null;
        private StockTransactionController _stockTransactionController = null;
        private UnitOfMeasureController _unitOfMeasureController = null;
        private AccountTransactionController _accountTransactionController = null;

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
            BInsertViaJSON.Enabled = false;
            BReadCurrentDocumentJSON.Enabled = false;
            btnUpdate.Enabled = false;
            btnRemove.Enabled = false;
            btnGet.Enabled = false;
            btnClear.Enabled = false;

            gbShareCost_1.Enabled = false;
            gbShareCost_2.Enabled = false;

            cmbAPI.Enabled = true;

            Cursor = Cursors.Default;
        }

        void S50cAPIEngine_APIStarted(object sender, EventArgs e) {

            gbShareCost_1.Enabled = APIEngine.SystemSettings.SpecialConfigs.UpdateItemCostWithFreightAmount;
            gbShareCost_2.Enabled = APIEngine.SystemSettings.SpecialConfigs.UpdateItemCostWithFreightAmount;

            tabEntities.Enabled = true;

            btnStopAPI.Enabled = true;
            btnStartAPI.Enabled = false;
            cmbAPI.Enabled = false;

            btnInsert.Enabled = true;
            BInsertViaJSON.Enabled = true;
            BReadCurrentDocumentJSON.Enabled = true;
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

            // Initialize controllers
            _itemController = new ItemController();
            _customerController = new CustomerController();
            _supplierController = new SupplierController();
            _itemTransactionController = new ItemTransactionController();
            _stockTransactionController = new StockTransactionController();
            _unitOfMeasureController = new UnitOfMeasureController();
            _accountTransactionController = new AccountTransactionController();

            // Load combos
            ItemClear(true);
            CustomerClearUI();
            SupplierClearUI();
            TransactionClearUI();
            AccountTransactionClear();
            SAFTClear();

            ApplyStyles();

            Cursor = Cursors.Default;
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
                APIEngine.CoreGlobals.MsgBoxFrontOffice(strMessage, VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
            }
        }

        /// <summary>
        /// Displays exclamation warning messages from the API
        /// </summary>
        void S50cAPIEngine_WarningMessage(string Message) {

            // Flag the error in the transaction in order to cancel it
            transactionError = true;

            APIEngine.CoreGlobals.MsgBoxFrontOffice(Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
        }

        /// <summary>
        /// Displays critical error messages from the API
        /// </summary>
        void S50cAPIEngine_WarningError(int Number, string Source, string Description) {

            // Flag the error in the transaction in order to cancel it
            transactionError = true;

            string msg = $"Erro: {Number}{Environment.NewLine}Fonte: {Source}{Environment.NewLine}{Description}";
            APIEngine.CoreGlobals.MsgBoxFrontOffice(msg, VBA.VbMsgBoxStyle.vbCritical, Application.ProductName);
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
        /// API initialization
        /// </summary>
        private void btnStartAPI_Click(object sender, EventArgs e) {
            try {
                Cursor = Cursors.WaitCursor;

                APIEngine.WarningError += S50cAPIEngine_WarningError;
                APIEngine.WarningMessage += S50cAPIEngine_WarningMessage;
                APIEngine.Message += S50cAPIEngine_Message;

                APIEngine.Initialize(cmbAPI.SelectedItem.ToString(), txtCompanyId.Text, chkAPIDebugMode.Checked, txtMachineId.Text);
            }
            catch (Exception ex) {
                Cursor = Cursors.Default;

                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
        }

        private void fApi_FormClosed(object sender, FormClosedEventArgs e) {
            // Save settings
            Properties.Settings.Default.DebugMode = chkAPIDebugMode.Checked;
            Properties.Settings.Default.CompanyId = txtCompanyId.Text;
            Properties.Settings.Default.API = cmbAPI.SelectedItem.ToString();
            Properties.Settings.Default.Save();

            if (APIEngine.APIInitialized) {
                APIEngine.Terminate();
            }
            Application.Exit();
        }

        private void btnStopAPI_Click(object sender, EventArgs e) {
            if (APIEngine.APIInitialized) {
                APIEngine.Terminate();
            }
            Application.Exit();
        }

        #endregion

        /// <summary>
        /// Creates data
        /// </summary>
        private void btnInsert_Click(object sender, EventArgs e) {
            try {
                transactionError = false;
                TransactionID transId = null;

                switch (tabEntities.SelectedIndex) {
                    case 0: ItemInsert(); break;
                    case 1: CustomerInsert(); break;
                    case 2: SupplierInsert(); break;
                    case 3: transId = TransactionInsert(false); break;
                    case 4: transId = AccountTransactionInsert(); break;
                    case 5: UnitOfMeasureInsert(); break;
                }
                if (!transactionError) {
                    string msg = null;
                    if (transId != null) {
                        msg = $"Registo inserido: {transId.ToString()}";
                    }
                    else {
                        msg = "Registo inserido.";
                    }

                    APIEngine.CoreGlobals.MsgBoxFrontOffice(msg, VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                }
            }
            catch (Exception ex) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
        }

        /// <summary>
        /// Loads data
        /// </summary>
        private void btnItemLoad_Click(object sender, EventArgs e) {
            try {
                switch (tabEntities.SelectedIndex) {
                    case 0: ItemGet(txtItemID.Text.Trim()); break;
                    case 1: CustomerGet((double)numCustomerId.Value); break;
                    case 2: SupplierGet(double.Parse(txtSupplierId.Text)); break;
                    case 3: TransactionGet(false); break;
                    case 4: AccountTransactionGet(); break;
                    case 5: UnitOfMeasureGet(txtUnitOfMeasureId.Text); break;
                }
            }
            catch (Exception ex) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
        }

        /// <summary>
        /// Update data
        /// </summary>
        private void btnAlterar_Click(object sender, EventArgs e) {
            try {
                TransactionID transId = null;
                transactionError = false;

                switch (tabEntities.SelectedIndex) {
                    case 0: ItemUpdate(); break;
                    case 1: CustomerUpdate(); break;
                    case 2: SupplierUpdate(); break;
                    case 3: transId = TransactionUpdate(false); break;
                    case 4: transId = AccountTransactionUpdate(); break;
                    case 5: UnitOfMeasureUpdate(); break;
                }

                if (!transactionError) {
                    string msg = null;
                    if (transId != null) {
                        msg = $"Registo alterado: {transId.ToString()}";
                    }
                    else {
                        msg = "Registo alterado.";
                    }

                    APIEngine.CoreGlobals.MsgBoxFrontOffice(msg, VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                }
            }
            catch (Exception ex) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
        }

        /// <summary>
        /// Removes data
        /// </summary>
        private void btnRemove_Click(object sender, EventArgs e) {
            try {
                if (VBA.VbMsgBoxResult.vbYes == APIEngine.CoreGlobals.MsgBoxFrontOffice("Confirma a anulação deste registo?", VBA.VbMsgBoxStyle.vbQuestion | VBA.VbMsgBoxStyle.vbYesNo, Application.ProductName)) {
                    TransactionID transId = null;
                    transactionError = false;

                    switch (tabEntities.SelectedIndex) {
                        case 0: ItemRemove(); break;
                        case 1: CustomerRemove(); break;
                        case 2: SupplierRemove(); break;
                        case 3: transId = TransactionRemove(); break;
                        case 4: transId = AccountTransactionRemove(); break;
                        case 5: UnitOfMeasureRemove(); break;
                    }

                    if (!transactionError) {
                        string msg = null;
                        if (transId != null) {
                            msg = $"Registo anulado: {transId.ToString()}";
                        }
                        else {
                            msg = "Registo anulado.";
                        }
                        APIEngine.CoreGlobals.MsgBoxFrontOffice(msg, VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                    }
                }
            }
            catch (Exception ex) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
        }

        /// <summary>
        /// Clears the UI
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e) {
            switch (tabEntities.SelectedIndex) {
                case 0: ItemClear(false); break;
                case 1: CustomerClearUI(); break;
                case 2: SupplierClearUI(); break;
                case 3: TransactionClearUI(); break;
                case 4: AccountTransactionClear(); break;
                case 5: UnitOfMeasureClear(); break;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e) {

            double transDocNumber = txtTransDocNumber.Text.ToDouble();

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
                    TransactionPrintWithConfig(txtTransSerial.Text, txtTransDoc.Text, transDocNumber);
                }
            }
            catch (Exception ex) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
        }

        #region ITEM

        /// <summary>
        /// Loads an item with the quicksearch result
        /// </summary>
        private void btnItemBrow_Click(object sender, EventArgs e) {

            var itemID = QuickSearchHelper.ItemFind();
            if (!string.IsNullOrEmpty(itemID)) {
                ItemGet(itemID);
            }
        }

        /// <summary>
        /// Fills the item with data from the UI
        /// </summary>
        private void ItemFill(bool bIsNew) {

            if (bIsNew) {
                _itemController.Create();
                _itemController.Item.ItemID = txtItemID.Text;
            }
            else if (_itemController.Item == null) {
                throw new Exception("Carregue um artigo antes de fazer alterações.");
            }

            _itemController.Item.Description = txtItemDescription.Text;
            _itemController.Item.ShortDescription = txtItemShortDescription.Text;
            _itemController.Item.Comments = txtItemComments.Text;

            var dsoPriceLine = new DSOPriceLine();
            // Initialize the price lines
            _itemController.Item.InitPriceList(dsoPriceLine.GetPriceLineRS());
            // Price of the item (1st line)
            Price myPrice = _itemController.Item.SalePrice[1, 0, string.Empty, 0, APIEngine.SystemSettings.SystemInfo.ItemDefaultsSettings.ItemDefaultUnit];
            // Set the price (tax included)
            myPrice.TaxIncludedPrice = (double)numItemPriceTaxIncluded.Value;
            // Get the unit price without taxes
            myPrice.UnitPrice = APIEngine.DSOCache.TaxesProvider.GetItemNetPrice(
                myPrice.TaxIncludedPrice,
                _itemController.Item.TaxableGroupID,
                systemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID,
                systemSettings.SystemInfo.TaxRegionID);


            // Clear the previous colors
            _itemController.ClearItemColors();
            // Add the new colors
            foreach (DataGridViewRow colorRow in dgvColor.Rows) {
                var colorID = (short)colorRow.Cells[0].Value;
                _itemController.AddColor(colorID);
            }


            // Clear the previous sizes
            _itemController.ClearItemSizes();
            // Add the new sizes
            foreach (DataGridViewRow sizeRow in dgvSize.Rows) {
                var sizeID = (short)sizeRow.Cells[0].Value;
                _itemController.AddSize(sizeID);
            }
        }

        /// <summary>
        /// Creates a new item
        /// </summary>
        void ItemInsert() {

            ItemFill(true);
            _itemController.Save();
        }

        /// <summary>
        /// Loads an item
        /// </summary>
        void ItemGet(string ItemID) {

            ItemClear(false);
            _itemController.Load(ItemID);

            var item = _itemController.Item;
            if (item != null) {
                txtItemID.Text = item.ItemID;
                txtItemDescription.Text = item.Description;
                txtItemShortDescription.Text = item.ShortDescription;
                numItemPriceTaxIncluded.Value = (decimal)item.SalePrice[1, 0, string.Empty, 0, item.UnitOfSaleID].TaxIncludedPrice;
                txtItemComments.Text = item.Comments;

                foreach (ItemColor value in item.Colors) {
                    var newRowIndex = dgvColor.Rows.Add();
                    var newRow = dgvColor.Rows[newRowIndex];

                    newRow.Cells[0].Value = value.ColorID;
                    newRow.Cells[1].Style.BackColor = ColorTranslator.FromOle(value.ColorCode);
                    newRow.Cells[2].Value = value.ColorName;
                }

                foreach (ItemSize value in item.Sizes) {
                    var newRowIndex = dgvSize.Rows.Add();
                    var newRow = dgvSize.Rows[newRowIndex];

                    newRow.Cells[0].Value = value.SizeID;
                    newRow.Cells[1].Value = value.SizeName;
                }
            }
        }

        /// <summary>
        /// Updates an item
        /// </summary>
        void ItemUpdate() {

            ItemFill(false);
            _itemController.Save();
        }

        /// <summary>
        /// Removes an item
        /// </summary>
        void ItemRemove() {

            _itemController.Remove(txtItemID.Text.Trim());
            ItemClear(false);
        }

        /// <summary>
        /// Clears the UI
        /// </summary>
        private void ItemClear(bool bClearItemID) {

            if (bClearItemID) {
                txtItemID.Text = string.Empty;
            }

            dgvColor.Rows.Clear();
            dgvSize.Rows.Clear();

            txtItemDescription.Text = string.Empty;
            txtItemShortDescription.Text = string.Empty;
            numItemPriceTaxIncluded.Value = 0;
            txtItemComments.Text = string.Empty;
        }

        private void btnAddColor_Click(object sender, EventArgs e) {

            var colorID = QuickSearchHelper.ColorFind();
            if (colorID > 0) {

                var isDuplicate = false;
                foreach (DataGridViewRow colorRow in dgvColor.Rows) {

                    var colorRowID = (short)colorRow.Cells[0].Value;
                    if (colorRowID == colorID) {
                        APIEngine.CoreGlobals.MsgBoxFrontOffice("Não é possível adicionar a mesma cor mais do que uma vez.", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                        isDuplicate = true;
                        break;
                    }
                }

                if (!isDuplicate) {
                    var colorToAdd = APIEngine.DSOCache.ColorProvider.GetColor((short)colorID);

                    var newRowIndex = dgvColor.Rows.Add();
                    var newRow = dgvColor.Rows[newRowIndex];

                    newRow.Cells[0].Value = colorToAdd.ColorID;
                    newRow.Cells[1].Style.BackColor = ColorTranslator.FromOle((int)colorToAdd.ColorCode);
                    newRow.Cells[2].Value = colorToAdd.Description;
                }
            }
        }

        private void btnAddSize_Click(object sender, EventArgs e) {

            var sizeID = QuickSearchHelper.SizeFind();
            if (sizeID > 0) {

                var isDuplicate = false;
                foreach (DataGridViewRow sizeRow in dgvSize.Rows) {
                    var sizeId = (short)sizeRow.Cells[0].Value;

                    if (sizeId == sizeID) {
                        APIEngine.CoreGlobals.MsgBoxFrontOffice("Não é possível adicionar o mesmo tamanho mais do que uma vez.", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                        isDuplicate = true;
                        break;
                    }
                }

                if (!isDuplicate) {
                    var sizeToAdd = APIEngine.DSOCache.SizeProvider.GetSize((short)sizeID);

                    var newRowIndex = dgvSize.Rows.Add();
                    var newRow = dgvSize.Rows[newRowIndex];

                    newRow.Cells[0].Value = sizeToAdd.SizeID;
                    newRow.Cells[1].Value = sizeToAdd.Description;
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

            dgv.Rows.Clear();
            dgv.Columns.Clear();
            dgv.Columns.AddRange(columns);
        }

        private void btnCreateColor_Click(object sender, EventArgs e) {

            fColor colorForm = new fColor();
            colorForm.ShowDialog();
        }

        private void btnCreateSize_Click(object sender, EventArgs e) {

            FormSizes formSizes = new FormSizes();
            formSizes.ShowDialog();
        }

        #endregion

        #region CUSTOMER

        private Customer CustomerInsert() {
            var customer = _customerController.Create();
            CustomerFill();
            _customerController.Save();
            CustomerClearUI();
            return customer;
        }

        private bool CustomerRemove() {
            var result = false;
            CustomerFill();
            result = _customerController.Remove();
            CustomerClearUI();
            return result;
        }

        private bool CustomerUpdate() {
            var result = false;
            CustomerFill();
            result = _customerController.Save();
            CustomerClearUI();
            return result;
        }

        private void CustomerGet(double customerId) {
            var customer = _customerController.Load(customerId);
            //Update form
            CustomerUpdateUI(customer);
        }

        private void CustomerFill() {
            if (_customerController.Customer == null) {
                throw new Exception("Carregue um cliente antes de fazer alterações.");
            }
            else {
                _customerController.Customer.CustomerID = (double)numCustomerId.Value;
                _customerController.Customer.OrganizationName = txtCustomerName.Text;
                _customerController.Customer.FederalTaxId = txtCustomerTaxId.Text;
                _customerController.Customer.EntityFiscalStatusID = ((EntityFiscalStatus)cmbCustomerTax.SelectedItem).EntityFiscalStatusID;
                _customerController.Customer.SalesmanId = (int)numCustomerSalesmanId.Value;
                _customerController.Customer.ZoneID = (short)numCustomerZoneId.Value;
                _customerController.Customer.CountryID = ((CountryCode)cmbCustomerCountry.SelectedItem).CountryID;
                _customerController.Customer.Comments = txtCustomerComments.Text;
            }
        }

        private void CustomerClearUI() {
            // Get new id
            numCustomerId.Value = (decimal)dsoCache.CustomerProvider.GetNewID();
            //
            txtCustomerComments.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtCustomerTaxId.Text = string.Empty;
            numCustomerSalesmanId.Value = 0;
            numCustomerZoneId.Value = 0;

            UIUtils.FillCountryCombo(cmbCustomerCountry);
            var country = cmbCustomerCountry.Items.Cast<CountryCode>()
                                            .FirstOrDefault(x => x.CountryID.Equals(systemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID, StringComparison.CurrentCultureIgnoreCase));
            cmbCustomerCountry.SelectedItem = country;
            //
            UIUtils.FillEntityFiscalStatusCombo(cmbCustomerTax);
            cmbCustomerTax.SelectedItem = cmbCustomerTax.Items.Cast<EntityFiscalStatus>().FirstOrDefault(x => x.EntityFiscalStatusID == APIEngine.SystemSettings.SystemInfo.PartySettings.SystemFiscalStatusID);
            if (cmbCustomerTax.SelectedItem == null && cmbCustomerTax.Items.Count > 0) {
                cmbCustomerTax.SelectedIndex = 0;
            }
        }

        private void CustomerUpdateUI(Customer customer) {
            if (customer != null) {
                numCustomerId.Value = (decimal)customer.CustomerID;
                numCustomerSalesmanId.Value = customer.SalesmanId;
                numCustomerZoneId.Value = customer.ZoneID;

                cmbCustomerCountry.SelectedItem = cmbCustomerCountry.Items.Cast<CountryCode>().FirstOrDefault(x => x.CountryID == customer.CountryID);
                cmbCustomerTax.SelectedItem = cmbCustomerTax.Items.Cast<EntityFiscalStatus>().FirstOrDefault(x => x.EntityFiscalStatusID == customer.EntityFiscalStatusID);

                txtCustomerComments.Text = customer.Comments;
                txtCustomerName.Text = customer.OrganizationName;
                txtCustomerTaxId.Text = customer.FederalTaxId;
            }
            else {
                //O cliente não existe!
                CustomerClearUI();
                throw new Exception($"O Cliente [{numCustomerId.Value}] não foi encontrado!");
            }
        }

        #endregion

        #region SUPPLIER

        private Supplier SupplierInsert() {
            var supplier = _supplierController.Create();
            SupplierFill();
            _supplierController.Save();
            SupplierClearUI();
            return supplier;
        }

        private bool SupplierRemove() {
            var result = false;
            SupplierFill();
            result = _supplierController.Remove();
            SupplierClearUI();
            return result;
        }

        private bool SupplierUpdate() {
            var result = false;
            SupplierFill();
            result = _supplierController.Save();
            SupplierClearUI();
            return result;
        }

        private void SupplierGet(double supplierId) {
            var supplier = _supplierController.Load(supplierId);
            //Update form
            SupplierUpdateUI(supplier);
        }

        private void SupplierFill() {
            if (_supplierController.Supplier == null) {
                throw new Exception("Carregue um fornecedor antes de fazer alterações.");
            }
            else {
                _supplierController.Supplier.SupplierID = txtSupplierId.Text.ToDouble();
                _supplierController.Supplier.OrganizationName = txtSupplierName.Text;
                _supplierController.Supplier.EntityFiscalStatusID = ((EntityFiscalStatus)cmbSupplierTax.SelectedItem).EntityFiscalStatusID;
                _supplierController.Supplier.FederalTaxId = txtSupplierTaxId.Text;
                _supplierController.Supplier.ZoneID = txtSupplierZone.Text.ToShort();
                _supplierController.Supplier.CountryID = ((CountryCode)cmbCustomerCountry.SelectedItem).CountryID;
                _supplierController.Supplier.Comments = txtSupplierComments.Text;
            }
        }

        private void SupplierClearUI() {
            //Get new id 
            txtSupplierId.Text = dsoCache.SupplierProvider.GetNewID().ToString();
            //
            txtSupplierName.Text = string.Empty;

            UIUtils.FillEntityFiscalStatusCombo(cmbSupplierTax);
            cmbSupplierTax.SelectedItem = cmbSupplierTax.Items.Cast<EntityFiscalStatus>().FirstOrDefault(x => x.EntityFiscalStatusID == APIEngine.SystemSettings.SystemInfo.PartySettings.SystemFiscalStatusID);
            if (cmbSupplierTax.SelectedItem == null && cmbSupplierTax.Items.Count > 0) {
                cmbSupplierTax.SelectedIndex = 0;
            }

            txtSupplierTaxId.Text = string.Empty;
            txtSupplierZone.Text = dsoCache.ZoneProvider.GetFirstID().ToString();

            UIUtils.FillCountryCombo(cmbSupplierCountry);
            var country = cmbCustomerCountry.Items.Cast<CountryCode>()
                                            .FirstOrDefault(x => x.CountryID.Equals(systemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID, StringComparison.CurrentCultureIgnoreCase));

            cmbCustomerCountry.SelectedItem = country;
            txtSupplierComments.Text = string.Empty;
        }

        private void SupplierUpdateUI(Supplier supplier) {
            if (supplier != null) {
                txtSupplierId.Text = supplier.SupplierID.ToString();
                txtSupplierName.Text = supplier.OrganizationName;
                cmbSupplierTax.SelectedItem = cmbSupplierTax.Items.Cast<EntityFiscalStatus>().FirstOrDefault(x => x.EntityFiscalStatusID == supplier.EntityFiscalStatusID);
                txtSupplierTaxId.Text = supplier.FederalTaxId;
                txtSupplierZone.Text = supplier.ZoneID.ToString();
                cmbSupplierCountry.SelectedItem = cmbSupplierCountry.Items.Cast<CountryCode>().FirstOrDefault(x => x.CountryID == supplier.CountryID);
                txtSupplierComments.Text = supplier.Comments;
            }
            else {
                SupplierClearUI();
                throw new Exception($"O Fornecedor [{txtSupplierId.Text}] não foi encontrado!"); ;
            }
        }

        #endregion

        #region TRANSACTION

        private TransactionID TransactionInsert(bool suspendTransaction) {

            TransactionID transactionID;

            if (rbTransBuySell.Checked) {
                _itemTransactionController.Create(txtTransDoc.Text, txtTransSerial.Text);
                transactionID = ItemTransactionUpdate(suspendTransaction);
            }
            else {
                _stockTransactionController.Create(txtTransDoc.Text, txtTransSerial.Text);
                transactionID = TransactionStockUpdate();
            }

            return transactionID;
        }

        private TransactionID TransactionInsertViaJSON(bool suspendTransaction, string json)
        {

            TransactionID transactionID;

            if (rbTransBuySell.Checked)
            {
                _itemTransactionController.CreateViaJSON(json);
                transactionID = ItemTransactionUpdate(suspendTransaction, false);
            }
            else
            {
                _stockTransactionController.Create(txtTransDoc.Text, txtTransSerial.Text);
                transactionID = TransactionStockUpdate();
            }

            return transactionID;
        }

        private string ReadCurrentTransactionJSON()
        {
            return _itemTransactionController.ReadCurrentTransactionJSON();
        }

        private TransactionID TransactionRemove() {

            TransactionID transactionID;

            if (rbTransBuySell.Checked) {
                TransactionFill();
                transactionID = _itemTransactionController.Remove();
                TransactionClearUI();
            }
            else {
                TransactionStockFill();
                transactionID = _stockTransactionController.Remove();
                TransactionClearUI();
            }

            return transactionID;
        }

        private TransactionID TransactionUpdate(bool suspendedTransaction) {

            TransactionID transactionID;

            if (rbTransBuySell.Checked) {
                transactionID = ItemTransactionUpdate(suspendedTransaction);
            }
            else {
                transactionID = TransactionStockUpdate();
            }

            TransactionClearUI();
            return transactionID;
        }

        private void TransactionGet(bool suspendedTransaction) {

            if (rbTransBuySell.Checked) {
                var trans = _itemTransactionController.Load(suspendedTransaction, txtTransDoc.Text, txtTransSerial.Text, txtTransDocNumber.Text.ToShort());
                var Transaction = new GenericTransaction(trans);
                TransactionShow(Transaction);
            }
            else {
                var trans = _stockTransactionController.Load(txtTransDoc.Text, txtTransSerial.Text, txtTransDocNumber.Text.ToShort());
                var Transaction = new GenericTransaction(trans);
                TransactionShow(Transaction);
            }
        }

        private void TransactionClearUI() {
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

                groupBox3.Enabled = true;
                groupBox3.Visible = true;

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
                lblTenderID.Enabled = true;
                txtTenderID.Enabled = true;
                lblPaymentID.Enabled = true;
                txtPaymentID.Enabled = true;
            }
            else {

                groupBox3.Enabled = false;
                groupBox3.Visible = false;

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
                lblTenderID.Enabled = false;
                txtTenderID.Enabled = false;
                lblPaymentID.Enabled = false;
                txtPaymentID.Enabled = false;
            }
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

        /// <summary>
        /// Impressão normal via caixa de diálogo e regras da 50c
        /// </summary>
        private void TransactionPrint(string transSerial, string transDoc, double transDocNumber, bool printPreview) {
            if (printPreview) {
                bsoItemTransaction.PrintTransaction(transSerial, transDoc, transDocNumber, PrintJobEnum.jobPreview, 1);
            }
            else {
                bsoItemTransaction.PrintTransaction(transSerial, transDoc, transDocNumber, PrintJobEnum.jobPrint, 1);
            }
        }

        private void TransactionPrintWithConfig(string transSerial, string transDoc, double transDocNumber) {
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
                    bsoItemTransaction.UserPermissions = systemSettings.User;
                    bsoItemTransaction.PermissionsType = FrontOfficePermissionEnum.foPermByUser;

                    if (chkPrintPreview.Checked) {
                        bsoItemTransaction.PrintTransaction(transSerial, transDoc, transDocNumber, PrintJobEnum.jobPreview, 1);
                    }
                    else {
                        bsoItemTransaction.PrintTransaction(transSerial, transDoc, transDocNumber, PrintJobEnum.jobPrint, 1, oPrintSettings);
                    }
                }
                APIEngine.CoreGlobals.MsgBoxFrontOffice("Concluido.", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
            }
            catch (Exception ex) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
            finally {
                btnPrint.Enabled = true;
                oDocument = null;
                oPlaceHolders = null;
            }
        }

        #endregion

        #region BUY/SALE TRANSACTION

        private void TransactionFill(bool fillTransDocNumber = true) {
            if (_itemTransactionController.Transaction == null) {
                throw new Exception("Carregue uma transação antes de fazer alterações.");
            }
            else {
                _itemTransactionController.SetPartyID(txtTransPartyId.Text.ToShort());
                _itemTransactionController.Transaction.TransDocument = txtTransDoc.Text.ToUpper();
                if (fillTransDocNumber)
                    _itemTransactionController.Transaction.TransDocNumber = txtTransDocNumber.Text.ToDouble();
                _itemTransactionController.Transaction.BaseCurrency.CurrencyID = txtTransCurrency.Text;
                _itemTransactionController.Transaction.CreateDate = txtTransDate.Text.ToDateTime().Date;
                _itemTransactionController.Transaction.CreateTime = txtTransTime.Text.ToTime();
                _itemTransactionController.Transaction.ActualDeliveryDate = txtTransDate.Text.ToDateTime().Date;
                _itemTransactionController.Transaction.ATCUD = txtAtcud.Text;
                _itemTransactionController.Transaction.QRCode = txtQrCode.Text;
                _itemTransactionController.Transaction.TransSerial = txtTransSerial.Text.ToUpper();
                _itemTransactionController.Transaction.Tender.TenderID = txtTenderID.Text.ToShort();
                //_itemTransactionController.Transaction.Payment.PaymentID = txtPaymentID.Text.ToShort();
                
                if (APIEngine.DSOCache.PaymentProvider.PaymentExists(txtPaymentID.Text.ToShort())) {
                    _itemTransactionController.Transaction.Payment = APIEngine.DSOCache.PaymentProvider.GetPayment(txtPaymentID.Text.ToShort());
                }
                else {
                    if (txtPaymentID.Text.ToShort() == 0) {
                        _itemTransactionController.Transaction.Payment.PaymentID = 0;
                    }
                    else {
                        _itemTransactionController.Transaction.Payment = null;
                    }
                    
                }
                
                _itemTransactionController.Transaction.Comments = "Gerado por " + Application.ProductName;
                _itemTransactionController.Transaction.WorkstationStamp.SessionID = systemSettings.TillSession.SessionID;
                _itemTransactionController.Transaction.TransactionTaxIncluded = chkTransTaxIncluded.Checked;
                _itemTransactionController.SetUserPermissions();

            }
        }

        private ItemTransactionDetail TransactionDetailFill() {
            if (dsoCache.ItemProvider.ItemExist(txtTransItemL1.Text)) {
                ItemTransactionDetail details = new ItemTransactionDetail();
                details.ItemID = txtTransItemL1.Text;
                details.Quantity = txtTransQuantityL1.Text.ToDouble();
                if (_itemTransactionController.Transaction.TransactionTaxIncluded) {
                    details.TaxIncludedPrice = txtTransUnitPriceL1.Text.ToDouble();
                }
                else {
                    details.UnitPrice = txtTransUnitPriceL1.Text.ToDouble();
                }
                details.WarehouseID = txtTransWarehouseL1.Text.ToShort();
                details.UnitOfSaleID = txtTransUnL1.Text;
                if (systemSettings.SystemInfo.UseColorSizeItems && chkTransModuleSizeColor.Checked) {
                    details.Color.ColorID = txtTransColor1.Text.ToShort();
                    details.Size.SizeID = txtTransSize1.Text.ToShort();
                }
                if (systemSettings.SystemInfo.UsePropertyItems && chkTransModuleProps.Checked) {
                    details.ItemProperties.ResetValues();
                    details.ItemProperties.PropertyValue1 = txtTransPropValueL1.Text;
                    //details.ItemProperties.PropertyValue2 = txtTransPropValueL2.Text;
                }
                return details;
            }
            else {
                throw new Exception($"O artigo [{txtTransItemL1.Text}] não foi encontrado");
            }
        }

        private ItemTransactionDetail TransactionDetailFillL2() {
            if (dsoCache.ItemProvider.ItemExist(txtTransItemL2.Text)) {
                ItemTransactionDetail details = new ItemTransactionDetail();
                details.ItemID = txtTransItemL2.Text;
                details.Quantity = txtTransQuantityL2.Text.ToDouble();
                if (_itemTransactionController.Transaction.TransactionTaxIncluded) {
                    details.TaxIncludedPrice = txtTransUnitPriceL2.Text.ToDouble();
                }
                else {
                    details.UnitPrice = txtTransUnitPriceL2.Text.ToDouble();
                }
                details.WarehouseID = txtTransWarehouseL2.Text.ToShort();
                details.UnitOfSaleID = txtTransUnL2.Text;
                if (systemSettings.SystemInfo.UseColorSizeItems && chkTransModuleSizeColor.Checked) {
                    details.Color.ColorID = txtTransColor1.Text.ToShort();
                    details.Size.SizeID = txtTransSize1.Text.ToShort();
                }
                if (systemSettings.SystemInfo.UsePropertyItems && chkTransModuleProps.Checked) {
                    //details.ItemProperties.PropertyValue1 = txtTransPropValueL1.Text;
                    details.ItemProperties.PropertyValue1 = txtTransPropValueL2.Text;
                }
                return details;
            }
            else {
                throw new Exception($"O artigo [{txtTransItemL2.Text}] não foi encontrado");
            }
        }

        private SimpleDocumentList TransactionFillCostShare() {
            SimpleDocument simpleDocument;
            SimpleDocumentList simpleDocumentList = new SimpleDocumentList();
            SimpleItemDetail simpleItemDetail;
            if (txtShareTransDocNumber_R1.Text.Length > 0) {

                simpleDocument = new SimpleDocument();
                simpleDocument.TransSerial = txtShareTransSerial_R1.Text;
                simpleDocument.TransDocument = txtShareTransDocument_R1.Text;
                simpleDocument.TransDocNumber = txtShareTransDocNumber_R1.Text.ToDouble();
                simpleDocument.TotalTransactionAmount = txtShareAmount_R1.Text.ToDouble();
                simpleDocument.CurrencyID = txtTransCurrency.Text;
                simpleDocument.CurrencyExchange = 1;
                simpleDocument.CurrencyFactor = 1;

                //ADD Line 1
                if (txtAmout_R1_L1.Text.Length > 0) {
                    simpleItemDetail = new SimpleItemDetail();
                    simpleItemDetail.DestinationTransSerial = txtShareTransSerial_R1.Text;
                    simpleItemDetail.DestinationTransDocument = txtShareTransDocument_R1.Text;
                    simpleItemDetail.DestinationTransDocNumber = txtShareTransDocNumber_R1.Text.ToDouble();
                    simpleItemDetail.DestinationLineItemID = 1;
                    simpleItemDetail.ItemID = LblL1.Text;
                    simpleItemDetail.UnitPrice = txtAmout_R1_L1.Text.ToDouble();
                    simpleItemDetail.Quantity = 1;
                    //Line KEY
                    simpleItemDetail.ItemSearchKey = simpleItemDetail.DestinationTransSerial + "|" + simpleItemDetail.DestinationTransDocument + "|" + simpleItemDetail.DestinationTransDocNumber.ToString() + "|" + Convert.ToString(simpleItemDetail.DestinationLineItemID) + "|" + simpleItemDetail.ItemID + "|" + simpleItemDetail.Color.ColorID + "|" + simpleItemDetail.Size.SizeID;
                    //Add Line 1 to document detail
                    simpleDocument.Details.Add(simpleItemDetail);
                }

                if (txtAmout_R1_L2.Text.Length > 0) {
                    simpleItemDetail = new SimpleItemDetail();
                    simpleItemDetail.DestinationTransSerial = txtShareTransSerial_R1.Text;
                    simpleItemDetail.DestinationTransDocument = txtShareTransDocument_R1.Text;
                    simpleItemDetail.DestinationTransDocNumber = txtShareTransDocNumber_R1.Text.ToDouble();
                    simpleItemDetail.DestinationLineItemID = 2;
                    simpleItemDetail.ItemID = LblL2.Text;
                    simpleItemDetail.UnitPrice = txtAmout_R1_L2.Text.ToDouble();
                    simpleItemDetail.Quantity = 1;
                    //Line KEY
                    simpleItemDetail.ItemSearchKey = simpleItemDetail.DestinationTransSerial + "|" + simpleItemDetail.DestinationTransDocument + "|" + simpleItemDetail.DestinationTransDocNumber.ToString() + "|" + Convert.ToString(simpleItemDetail.DestinationLineItemID) + "|" + simpleItemDetail.ItemID + "|" + simpleItemDetail.Color.ColorID + "|" + simpleItemDetail.Size.SizeID;
                    //Add Line 2 to document detail
                    simpleDocument.Details.Add(simpleItemDetail);

                }
                simpleDocumentList.Add(simpleDocument);
            }
            else {
                if (txtShareTransDocNumber_R2.Text.Length > 0) {
                    simpleDocument = new SimpleDocument();
                    simpleDocument.TransSerial = txtShareTransSerial_R2.Text;
                    simpleDocument.TransDocument = txtShareTransDocument_R2.Text;
                    simpleDocument.TransDocNumber = txtShareTransDocNumber_R2.Text.ToDouble();
                    simpleDocument.TotalTransactionAmount = txtShareAmount_R2.Text.ToDouble();
                    simpleDocument.CurrencyID = txtTransCurrency.Text;

                    //Add Document to list of Documento to Share amount 
                    simpleDocumentList.Add(simpleDocument);
                }
            }

            return simpleDocumentList;
        }

        private TransactionID ItemTransactionUpdate(bool suspended, bool fillTransDocNumber = true) {
            transactionError = false;
            TransactionFill(fillTransDocNumber);

            //Clear lines
            int i = 1;
            while (_itemTransactionController.Transaction.Details.Count > 0) {
                _itemTransactionController.Transaction.Details.Remove(ref i);
            }

            _itemTransactionController.SetUserPermissions();

            if (string.IsNullOrEmpty(txtTransItemL1.Text)) {
                throw new Exception("Não pode criar uma transação vazia!");
            }
            var detail = TransactionDetailFill();
            if (detail != null) {
                _itemTransactionController.AddDetail(txtTransTaxRateL1.Text.ToDouble(), detail);
            }
            //If exist 2 items add second
            if (!string.IsNullOrEmpty(txtTransItemL2.Text)) {
                detail = TransactionDetailFillL2();
                if (detail != null) {
                    _itemTransactionController.AddDetail(txtTransTaxRateL2.Text.ToDouble(), detail);
                }
            }

            _itemTransactionController.SetPaymentDiscountPercent(txtTransGlobalDiscount.Text.ToDouble());
            _itemTransactionController.Calculate();

            var series = systemSettings.DocumentSeries[_itemTransactionController.Transaction.TransSerial];
            if (series.SeriesType == SeriesTypeEnum.SeriesExternal) {
                if (!SetExternalSignature(_itemTransactionController.Transaction)) {
                    APIEngine.CoreGlobals.MsgBoxFrontOffice("A assinatura não foi definida. Vão ser usados valores por omissão", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                }
            }

            TransactionID transactionID = null;
            if (suspended) {
                transactionID = _itemTransactionController.SuspendTransaction();   
            }
            if(!suspended) {
                //Exemplo da Repartição de Custos
                if (systemSettings.SpecialConfigs.UpdateItemCostWithFreightAmount) {
                    var simpleDocumentList = TransactionFillCostShare();
                    _itemTransactionController.CreateCostShare(simpleDocumentList);
                }
                
                //Exemplo de recalculo de prestações
                _itemTransactionController.RecalculateInstallments();

                _itemTransactionController.Save(suspended);
                transactionID = _itemTransactionController.Transaction.TransactionID;
            }
            TransactionPrintWithConfig(_itemTransactionController.Transaction.TransSerial, _itemTransactionController.Transaction.TransDocument, _itemTransactionController.Transaction.TransDocNumber);

            if (APIEngine.CoreGlobals.MsgBoxFrontOffice("Clean UI?", VBA.VbMsgBoxStyle.vbYesNo, Application.ProductName) == VBA.VbMsgBoxResult.vbYes)
                TransactionClearUI();
            return transactionID;
        }

        #endregion

        #region STOCK TRANSACTION

        private void TransactionStockFill() {
            if (_stockTransactionController.StockTransaction == null) {
                throw new Exception("Carregue uma transação antes de fazer alterações.");
            }
            else {
                _stockTransactionController.SetPermissions();
                _stockTransactionController.StockTransaction.TransDocument = txtTransDoc.Text.ToUpper();
                _stockTransactionController.StockTransaction.TransSerial = txtTransSerial.Text.ToUpper();
                _stockTransactionController.StockTransaction.TransDocNumber = txtTransDocNumber.Text.ToShort();
                _stockTransactionController.StockTransaction.TransactionTaxIncluded = chkTransTaxIncluded.Checked;
                _stockTransactionController.StockTransaction.CreateDate = txtTransDate.Text.ToDateTime(DateTime.Now);
                _stockTransactionController.StockTransaction.CreateTime = new DateTime(DateTime.Now.TimeOfDay.Ticks);
                _stockTransactionController.StockTransaction.ActualDeliveryDate = txtTransDate.Text.ToDateTime(DateTime.Now);
                _stockTransactionController.SetPartyType(cmbTransPartyType.SelectedIndex);
                _stockTransactionController.SetBaseCurrency(txtTransCurrency.Text);
                _stockTransactionController.StockTransaction.Comments = "Gerado por: " + Application.ProductName;
                _stockTransactionController.StockTransaction.BaseCurrency.CurrencyID = txtTransCurrency.Text;
            }
        }

        private ItemTransactionDetail TransactionStockDetailsFill() {
            if (dsoCache.ItemProvider.ItemExist(txtTransItemL1.Text)) {
                ItemTransactionDetail details = new ItemTransactionDetail();
                details.ItemID = txtTransItemL1.Text;
                if (dsoCache.WarehouseProvider.WarehouseExists(txtTransWarehouseL1.Text.ToShort())) {
                    details.WarehouseID = txtTransWarehouseL1.Text.ToShort();
                    details.UnitOfSaleID = txtTransUnL1.Text;
                    details.UnitPrice = txtTransUnitPriceL1.Text.ToDouble();
                    details.Quantity = txtTransQuantityL1.Text.ToDouble();
                    return details;
                }
                else {
                    throw new Exception($"O Armazém indicado [{txtTransWarehouseL1.Text.ToDouble()}] não existe.");
                }
            }
            else {
                throw new Exception($"O Artigo [{txtTransItemL1.Text}] não foi encontrado.");
            }
        }

        private ItemTransactionDetail TransactionStockDetailsFillL2() {

            if (dsoCache.ItemProvider.ItemExist(txtTransItemL2.Text)) {
                ItemTransactionDetail details = new ItemTransactionDetail();
                details.ItemID = txtTransItemL2.Text;
                if (dsoCache.WarehouseProvider.WarehouseExists(txtTransWarehouseL2.Text.ToShort())) {
                    details.WarehouseID = txtTransWarehouseL2.Text.ToShort();
                    details.UnitOfSaleID = txtTransUnL2.Text;
                    details.UnitPrice = txtTransUnitPriceL2.Text.ToDouble();
                    details.Quantity = txtTransQuantityL2.Text.ToDouble();
                    return details;
                }
                else {
                    throw new Exception($"O Armazém indicado [{txtTransWarehouseL1.Text.ToDouble()}] não existe.");
                }
            }
            else {
                throw new Exception($"O Artigo [{txtTransItemL2.Text}] não foi encontrado.");
            }
        }

        private TransactionID TransactionStockUpdate() {
            transactionError = false;
            TransactionStockFill();

            // Remover todas as linhas (caso da alteração)
            int i = 1;
            while (_stockTransactionController.StockTransaction.Details.Count > 0) {
                _stockTransactionController.StockTransaction.Details.Remove(ref i);
            }

            StockQtyRuleEnum StockQtyRule = StockQtyRuleEnum.stkQtyNone;
            if (_stockTransactionController.StockTransaction.TransStockBehavior == StockBehaviorEnum.sbStockCompose) {
                StockQtyRule = StockQtyRuleEnum.stkQtyReceipt;
            }
            else {
                if (_stockTransactionController.StockTransaction.TransStockBehavior == StockBehaviorEnum.sbStockDecompose) {
                    StockQtyRule = StockQtyRuleEnum.stkQtyOutgoing;
                }
            }

            _stockTransactionController.SetPermissions();

            if (!string.IsNullOrEmpty(txtTransItemL1.Text)) {
                //Add details
                var detail = TransactionStockDetailsFill();
                _stockTransactionController.AddDetailStock(txtTransTaxRateL1.Text.ToDouble(), StockQtyRule, detail);

                if (bsoStockTransaction.Transaction.TransStockBehavior == StockBehaviorEnum.sbStockCompose || bsoStockTransaction.Transaction.TransStockBehavior == StockBehaviorEnum.sbStockDecompose) {
                    var itemDetails = GetItemComponentList(1);
                    if (itemDetails != null) {
                        foreach (ItemTransactionDetail value in itemDetails) {
                            _stockTransactionController.AddDetailStock(txtTransTaxRateL1.Text.ToDouble(), value.PhysicalQtyRule, value);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(txtTransItemL2.Text)) {
                //Add details
                var detail = TransactionStockDetailsFillL2();
                _stockTransactionController.AddDetailStock(txtTransTaxRateL1.Text.ToDouble(), StockQtyRule, detail);

                if (_stockTransactionController.StockTransaction.TransStockBehavior == StockBehaviorEnum.sbStockCompose || bsoStockTransaction.Transaction.TransStockBehavior == StockBehaviorEnum.sbStockDecompose) {
                    var itemDetails = GetItemComponentList(1);
                    if (itemDetails != null) {
                        foreach (ItemTransactionDetail value in itemDetails) {
                            _stockTransactionController.AddDetailStock(txtTransTaxRateL1.Text.ToDouble(), value.PhysicalQtyRule, value);
                            //TransStockAddDetail(taxRate, value.PhysicalQtyRule);
                        }
                    }
                }
            }

            _stockTransactionController.Save();
            TransactionClearUI();
            return _stockTransactionController.StockTransaction.TransactionID;
        }

        #endregion

        #region ACCOUNT TRANSACTION

        /// <summary>
        /// Collects the data from the UI
        /// </summary>
        private void AccountTransFill(bool isNew) {

            const string ACCOUNT_ID = "CC";

            string transSerial = txtAccountTransSerial.Text.ToUpper();
            string transDoc = txtAccountTransDoc.Text.ToUpper();
            double transDocNumber = txtAccountTransDocNumber.Text.ToDouble();
            double partyId = txtAccountTransPartyId.Text.ToDouble();

            DateTime createDate = txtAccountTransDocDate.Text.ToDateTime().Date;

            if (isNew) {

                var accountUsed = cmbRecPeg.SelectedIndex == 0 ? AccountUsedEnum.auCustomerLedgerAccount : AccountUsedEnum.auSupplierLedgerAccount;

                _accountTransactionController.Create(accountUsed, transSerial, transDoc, transDocNumber);
                _accountTransactionController.SetPartyID(partyId);
            }
            else {
                _accountTransactionController.Load(transSerial, transDoc, transDocNumber);
            }

            _accountTransactionController.SetLedgerAccount(ACCOUNT_ID, partyId);

            // Remove all the transaction details
            _accountTransactionController.ClearTransactionDetails();

            _accountTransactionController.SetAccountID(ACCOUNT_ID);
            _accountTransactionController.SetBaseCurrencyID(txtAccountTransDocCurrency.Text);
            _accountTransactionController.SetCreateDate(createDate);

            // Transaction detail line 1
            string docId = txtAccountTransDocL1.Text;
            string docSeries = txtAccountTransSeriesL1.Text;
            double docNumber = txtAccountTransDocNumberL1.Text.ToDouble();
            double paymentValue = txtAccountTransDocValueL1.Text.ToDouble();
            if (paymentValue > 0) {
                _accountTransactionController.AddDetail(docId, docSeries, docNumber, 0, paymentValue);
            }

            // Transaction detail line 2
            docId = txtAccountTransDocL2.Text;
            docSeries = txtAccountTransSeriesL2.Text;
            docNumber = txtAccountTransDocNumberL2.Text.ToDouble();
            paymentValue = txtAccountTransDocValueL2.Text.ToDouble();
            if (paymentValue > 0) {
                _accountTransactionController.AddDetail(docId, docSeries, docNumber, 0, paymentValue);
            }

            var transaction = _accountTransactionController.AccountTransaction;

            // Abort if the document doesn't have any lines
            if (transaction.Details.Count == 0) {
                throw new Exception("O documento não tem linhas.");
            }

            transaction.TenderLineItems = _accountTransactionController.GetTenderLineItems(txtAccountTransPaymentId.Text);

            // Ensure the till is open
            _accountTransactionController.EnsureOpenTill();
        }

        /// <summary>
        /// Creates a new account transaction
        /// </summary>
        private TransactionID AccountTransactionInsert() {

            AccountTransFill(true);
            var transactionID = _accountTransactionController.Save();
            return transactionID;
        }

        /// <summary>
        /// Loads an account transaction
        /// </summary>
        private void AccountTransactionGet() {

            string transSerial = txtAccountTransSerial.Text.ToUpper();
            string transDoc = txtAccountTransDoc.Text.ToUpper();
            double transDocNumber = txtAccountTransDocNumber.Text.ToDouble();

            // Clear the UI
            AccountTransactionClear();

            // Load the transaction
            _accountTransactionController.Load(transSerial, transDoc, transDocNumber);

            var accountTrans = _accountTransactionController.AccountTransaction;
            txtAccountTransDoc.Text = accountTrans.TransDocument;
            txtAccountTransDocCurrency.Text = accountTrans.BaseCurrency.CurrencyID;
            txtAccountTransDocNumber.Text = accountTrans.TransDocNumber.ToString();
            txtAccountTransPartyId.Text = accountTrans.Entity.PartyID.ToString();
            txtAccountTransDocDate.Text = accountTrans.CreateDate.ToShortDateString();
            txtAccountTransSerial.Text = accountTrans.TransSerial;

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
        }

        /// <summary>
        /// Updates an account transaction
        /// </summary>
        private TransactionID AccountTransactionUpdate() {

            AccountTransFill(false);
            var transactionID = _accountTransactionController.Save();
            return transactionID;
        }

        /// <summary>
        /// Removes an account transaction
        /// </summary>
        private TransactionID AccountTransactionRemove() {

            string transSerial = txtAccountTransSerial.Text.ToUpper();
            string transDoc = txtAccountTransDoc.Text.ToUpper();
            double transDocNumber = txtAccountTransDocNumber.Text.ToDouble();

            _accountTransactionController.Load(transSerial, transDoc, transDocNumber);
            var transactionID = _accountTransactionController.Remove(transSerial, transDoc, transDocNumber, Application.ProductName);
            return transactionID;
        }

        /// <summary>
        /// Clears the UI
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

            AccountTransClearL1();
            AccountTransClearL2();

            tabAccount.BackgroundImage = null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        private void AccountTransClearL1() {
            txtAccountTransSeriesL1.Text = string.Empty;
            txtAccountTransDocL1.Text = string.Empty;
            txtAccountTransDocNumberL1.Text = "0";
            txtAccountTransDocValueL1.Text = "0";
        }

        /// <summary>
        /// TODO
        /// </summary>
        private void AccountTransClearL2() {
            txtAccountTransSeriesL2.Text = string.Empty;
            txtAccountTransDocL2.Text = string.Empty;
            txtAccountTransDocNumberL2.Text = "0";
            txtAccountTransDocValueL2.Text = "0";
        }

        /// <summary>
        /// Obtain the first available document of a given type
        /// </summary>
        private Document AccountTransGetDocument() {

            Document accountDoc = null;

            if (cmbRecPeg.SelectedIndex == 0) {
                // Get the first avaialable receipt document
                accountDoc = systemSettings.WorkstationInfo.Document.OfType<Document>().FirstOrDefault(x => x.TransDocType == DocumentTypeEnum.dcTypeAccount && x.UpdateTenderReport && x.AccountBehavior == AccountBehaviorEnum.abAccountSettlement && x.SignTenderReport == "+");
            }
            else {
                // Get the first avaialable payment document
                accountDoc = systemSettings.WorkstationInfo.Document.OfType<Document>().FirstOrDefault(x => x.TransDocType == DocumentTypeEnum.dcTypeAccount && x.UpdateTenderReport && x.AccountBehavior == AccountBehaviorEnum.abAccountSettlement && x.SignTenderReport == "-");
            }

            return accountDoc;
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
            if (accountDoc != null) {
                txtAccountTransDoc.Text = accountDoc.DocumentID;
            }
            else {
                txtAccountTransDoc.Text = string.Empty;
            }
        }

        #endregion

        #region UNIT OF MEASURE

        /// <summary>
        /// Loads a unit of measure with the quicksearch result
        /// </summary>
        private void btnUnitOfMeasureBrow_Click(object sender, EventArgs e) {

            var unitOfMeasureID = QuickSearchHelper.UnitOfMeasureFind();
            if (!string.IsNullOrEmpty(unitOfMeasureID)) {
                UnitOfMeasureGet(unitOfMeasureID);
            }
        }

        /// <summary>
        /// Fills the unit of measure with data from the UI
        /// </summary>
        private void UnitOfMeasureFill(bool isNew) {

            if (isNew) {
                _unitOfMeasureController.Create();
                _unitOfMeasureController.SetUnitOfMeasureID(txtUnitOfMeasureId.Text);
            }
            else if (_unitOfMeasureController.UnitOfMeasure == null) {
                throw new Exception("Carregue uma unidade de medição antes de fazer alterações.");
            }
            _unitOfMeasureController.SetDescription(txtUnitOfMeasureName.Text);
        }

        /// <summary>
        /// Creates a new unit of measure
        /// </summary>
        private void UnitOfMeasureInsert() {

            UnitOfMeasureFill(true);
            _unitOfMeasureController.Save();
        }

        /// <summary>
        /// Loads a unit of measure
        /// </summary>
        private void UnitOfMeasureGet(string unitOfMeasureID) {

            UnitOfMeasureClear();
            _unitOfMeasureController.Load(unitOfMeasureID);

            var unitOfMeasure = _unitOfMeasureController.UnitOfMeasure;
            if (unitOfMeasure != null) {
                txtUnitOfMeasureId.Text = unitOfMeasure.UnitOfMeasureID;
                txtUnitOfMeasureName.Text = unitOfMeasure.Description;
            }
        }

        /// <summary>
        /// Updates a unit of measure
        /// </summary>
        private void UnitOfMeasureUpdate() {

            UnitOfMeasureFill(false);
            _unitOfMeasureController.Save();
        }

        /// <summary>
        /// Removes a unit of measure
        /// </summary>
        private void UnitOfMeasureRemove() {

            _unitOfMeasureController.Remove(txtUnitOfMeasureId.Text.Trim());
            UnitOfMeasureClear();
        }

        /// <summary>
        /// Clears the UI
        /// </summary>
        private void UnitOfMeasureClear() {

            txtUnitOfMeasureId.Text = string.Empty;
            txtUnitOfMeasureName.Text = string.Empty;
        }

        #endregion

        #region SAF-T

        private void cmbSAFTMonth_SelectedIndexChanged(object sender, EventArgs e) {

            nudSAFTStartDay.Value = 1;
            nudSAFTEndDay.Value = DateTime.DaysInMonth((int)nudSAFTYear.Value, ((Month)cmbSAFTMonth.SelectedItem).Value);
        }

        private void nudSAFTYear_ValueChanged(object sender, EventArgs e) {

            nudSAFTStartDay.Value = 1;
            nudSAFTEndDay.Value = DateTime.DaysInMonth((int)nudSAFTYear.Value, ((Month)cmbSAFTMonth.SelectedItem).Value);
        }

        private void nudSAFTStartDay_ValueChanged(object sender, EventArgs e) {

            int daysInMonth = DateTime.DaysInMonth((int)nudSAFTYear.Value, ((Month)cmbSAFTMonth.SelectedItem).Value);
            if (nudSAFTStartDay.Value < 1) {
                nudSAFTStartDay.Value = daysInMonth;
            }
            else if (nudSAFTStartDay.Value > daysInMonth) {
                nudSAFTStartDay.Value = 1;
            }
        }

        private void nudSAFTEndDay_ValueChanged(object sender, EventArgs e) {

            int daysInMonth = DateTime.DaysInMonth((int)nudSAFTYear.Value, ((Month)cmbSAFTMonth.SelectedItem).Value);
            if (nudSAFTEndDay.Value < 1) {
                nudSAFTEndDay.Value = daysInMonth;
            }
            else if (nudSAFTEndDay.Value > daysInMonth) {
                nudSAFTEndDay.Value = 1;
            }
        }

        /// <summary>
        /// Exports a global SAF-T
        /// </summary>
        private void btnSAFTExport0_Click(object sender, EventArgs e) {

            var fileName = $"Global-{APIEngine.SystemSettings.Company.CompanyID}-{dtpStart.Value.Date:yyyyMMdd}-{dtpEnd.Value.Date:yyyyMMdd}-{new Random().Next(1, 1001)}.xml";
            var filePath = Path.Combine(txtSAFTPath0.Text, fileName);
            SAFTHelper.ExportSAFT(dtpStart.Value, dtpEnd.Value, filePath, false);
        }

        /// <summary>
        /// Exports a simplified SAF-T
        /// </summary>
        private void btnSAFTExport1_Click(object sender, EventArgs e) {

            var year = (int)nudSAFTYear.Value;
            var month = ((Month)cmbSAFTMonth.SelectedItem).Value;

            DateTime startDate = new DateTime(year, month, (int)nudSAFTStartDay.Value);
            DateTime endDate = new DateTime(year, month, (int)nudSAFTEndDay.Value);

            var fileName = $"Simplified-{APIEngine.SystemSettings.Company.CompanyID}-{startDate.Date:yyyyMMdd}-{endDate.Date:yyyyMMdd}-{new Random().Next(1, 1001)}.xml";
            var filePath = Path.Combine(txtSAFTPath1.Text, fileName);
            SAFTHelper.ExportSAFT(startDate, endDate, filePath, true);
        }

        private void SAFTClear() {

            UIUtils.FillMonthCombo(cmbSAFTMonth);

            var dateToday = DateTime.Today.AddMonths(-1);
            var startDate = dateToday.FirstDayOfMonth();
            var endDate = dateToday.LastDayOfLastMonth();

            dtpStart.Value = startDate;
            dtpEnd.Value = endDate;

            cmbSAFTMonth.SelectedIndex = startDate.Month - 1;
            nudSAFTYear.Value = startDate.Year;

            var saftPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                APIEngine.SystemSettings.Application.LongName,
                APIEngine.SystemSettings.Company.CompanyID,
                "SAFT");

            if (!Directory.Exists(saftPath)) {
                Directory.CreateDirectory(saftPath);
            }

            txtSAFTPath0.Text = saftPath;
            txtSAFTPath1.Text = saftPath;

            txtSAFTPath0.SelectionStart = txtSAFTPath0.TextLength;
            txtSAFTPath0.ScrollToCaret();

            txtSAFTPath1.SelectionStart = txtSAFTPath1.TextLength;
            txtSAFTPath1.ScrollToCaret();
        }

        private void ApplyStyles() {

            btnSAFTExport0.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.PrimaryBackColor);
            btnSAFTExport0.FlatAppearance.BorderColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBorderColor);
            btnSAFTExport0.ForeColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.PrimaryForeColor);

            btnSAFTExport1.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.PrimaryBackColor);
            btnSAFTExport1.FlatAppearance.BorderColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBorderColor);
            btnSAFTExport1.ForeColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.PrimaryForeColor);
        }

        #endregion

        private void chkTransModuleProps_CheckedChanged(object sender, EventArgs e) {
            pnlTransModuleProp.Enabled = chkTransModuleProps.Checked;
        }

        private void chkTransModuleSizeColor_CheckedChanged(object sender, EventArgs e) {
            pnlTransModuleSizeColor.Enabled = chkTransModuleSizeColor.Checked;
        }

        private void rbTransStock_CheckedChanged(object sender, EventArgs e) {
            tabTransModules.Visible = false;
            lblTransModules.Visible = false;
            txtTransGlobalDiscount.Enabled = false;
            dataGridItemLines.Visible = false;
            btnRefreshGridLines.Visible = false;

            if (rbTransStock.Checked) {
                TransactionClearUI();
            }
        }

        private void rbTransBuySell_CheckedChanged(object sender, EventArgs e) {
            tabTransModules.Visible = true;
            lblTransModules.Visible = true;
            txtTransGlobalDiscount.Enabled = true;
            dataGridItemLines.Visible = false;
            btnRefreshGridLines.Visible = false;

            if (rbTransBuySell.Checked) {
                TransactionClearUI();
            }
        }

        private void cmbTransPartyType_SelectedIndexChanged(object sender, EventArgs e) {
            TransactionClearUI();
        }

        #region QuickSearch

        private void btnSearchSalesman_Click(object sender, EventArgs e) {
            var SalesmanId = QuickSearchHelper.SalesmanFind();
            if (SalesmanId > 0) {
                numCustomerSalesmanId.Value = (long)SalesmanId;
            }
        }

        private void btnSearchZone_Click(object sender, EventArgs e) {
            var ZoneId = QuickSearchHelper.ZoneFind();
            if (ZoneId > 0) {
                numCustomerZoneId.Value = (short)ZoneId;
            }
        }

        private void btnSearchZoneSupplier_Click(object sender, EventArgs e) {
            var zoneId = QuickSearchHelper.ZoneFind();
            if (zoneId > 0) {
                txtSupplierZone.Text = zoneId.ToString();
            }
        }

        private void btnCustomerBrow_Click(object sender, EventArgs e) {
            var customerId = QuickSearchHelper.CustomerFind();
            if (customerId > 0) {
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
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
        }

        private void btnSavePrep_Click(object sender, EventArgs e) {
            TransactionID result = null;
            try {
                if (_itemTransactionController.Transaction.TempTransIndex != 0) {
                    // Atualizar
                    result = TransactionUpdate(true);
                }
                else {
                    result = TransactionInsert(true);
                }
                //var docNum = _itemTransactionController.SuspendTransaction();
                if (result != null) {
                    //TransactionClearUI();
                    APIEngine.CoreGlobals.MsgBoxFrontOffice($"Colocado em preparação: {result.TransDocument} {result.TransSerial}/{result.TransDocNumber} ", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                }
                else {
                    APIEngine.CoreGlobals.MsgBoxFrontOffice($"Não foi possível colocar em preparação: {txtTransDoc.Text} {txtTransSerial.Text}/{txtTransDocNumber.Text}", VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
                }

            }
            catch (Exception ex) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
        }

        private void btnTransactionFinalize_Click(object sender, EventArgs e) {
            try {
                string transDoc = txtTransDoc.Text;
                string transSerial = txtTransSerial.Text;
                double transdocNumber = txtTransDocNumber.Text.ToDouble();

                if (transdocNumber > 0) {
                    if (_itemTransactionController.FinalizeTransaction(transSerial, transDoc, transdocNumber)) {
                        APIEngine.CoreGlobals.MsgBoxFrontOffice($"Documento finalizado: {transDoc} {transSerial}/{transdocNumber}.", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                    }
                    else {
                        APIEngine.CoreGlobals.MsgBoxFrontOffice($"Não foi possível finalizar o documento suspenso: {transDoc} {transSerial}/{transdocNumber}.", VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
                    }
                }
                else {
                    APIEngine.CoreGlobals.MsgBoxFrontOffice($"O número do documento [{transdocNumber}] não é válido.", VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
                }
            }
            catch (Exception ex) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
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
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
        }

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

            var item = dsoCache.ItemProvider.GetItem(itemID, currency, false);
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
                TransactionClearUI();
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
                TransactionClearUI();
            }
        }

        private void btnRefreshGridLines_Click(object sender, EventArgs e) {
            string transDoc = txtTransDoc.Text;

            if (!systemSettings.WorkstationInfo.Document.IsInCollection(transDoc)) {
                throw new Exception("O documento não se encontra preenchido ou não existe");
            }
            Document doc = systemSettings.WorkstationInfo.Document[transDoc];

            if (rbTransStock.Checked && doc.TransDocType != DocumentTypeEnum.dcTypeStock) {
                throw new Exception($"O documento indicado [{transDoc}] não é um documento de stock");
            }
            else if (rbTransBuySell.Checked && (doc.TransDocType != DocumentTypeEnum.dcTypePurchase || doc.TransDocType != DocumentTypeEnum.dcTypeSale)) {
                throw new Exception($"O documento indicado [{transDoc}] não é um documento de Compra/Venda");
            }

            if (doc.StockBehavior == StockBehaviorEnum.sbStockCompose || doc.StockBehavior == StockBehaviorEnum.sbStockDecompose) {
                dataGridItemLines.Rows.Clear();

                var itemDetails = GetItemComponentList(1);
                addComponentListToGrid(itemDetails);

                itemDetails = GetItemComponentList(2);
                addComponentListToGrid(itemDetails);
            }
            else {
                throw new Exception($"O documento indicado [{transDoc}] não é um documento de fabricação/transformação");
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
            APIEngine.CoreGlobals.MsgBoxFrontOffice("NOTA: Só é possível definir a assinatura sem séries externas.", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);

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
            APIEngine.CoreGlobals.ShowKeyPadInContext(txtItemID, "Text", VBA.VbCallType.VbLet);
        }

        private void btnClearRep1_Click(object sender, EventArgs e) {
            RepClear();
        }

        private void RepClear() {

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

            double TransDocNumber = txtShareTransDocNumber_R1.Text.ToDouble();
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

                    if (VBA.VbMsgBoxResult.vbYes ==
                        APIEngine.CoreGlobals.MsgBoxFrontOffice("Guardar o documento temporário recuperado?",
                                        VBA.VbMsgBoxStyle.vbQuestion | VBA.VbMsgBoxStyle.vbYesNo, Application.ProductName)) {
                        //Após importar, se pretender, será gravado automaticamente o documento.
                        if (bsoItemTransaction.Transaction.Tender.TenderID == 0) {
                            // Set the first TenderId, just in case...
                            bsoItemTransaction.TenderID = APIEngine.DSOCache.TenderProvider.GetFirstID();
                        }
                        bsoItemTransaction.SaveDocument(false, false);
                        APIEngine.CoreGlobals.MsgBoxFrontOffice($"Documento gravado: {bsoItemTransaction.Transaction.TransactionID.ToString()}", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                        TransactionClearUI();
                    }
                }
                else {
                    APIEngine.CoreGlobals.MsgBoxFrontOffice($"O temporário indicado '{txtTransDocNumber.Text}' não existe ou não existem mais temporários.", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                }
            }
            catch (Exception ex) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
            }
        }

        private void TransactionShow(GenericTransaction trans) {
            if (trans != null) {
                var doc = APIEngine.SystemSettings.WorkstationInfo.Document[trans.TransDocument];

                TransactionClearUI();

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

                    if (doc.TransDocType == DocumentTypeEnum.dcTypePurchase) {
                        if (trans.BuyShareOtherCostList.Count > 0) {

                            SimpleDocumentList objDocumentList = new SimpleDocumentList();
                            objDocumentList = trans.BuyShareOtherCostList;

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
                }

                //Fabricação/Transformação - restantes linhas
                if ((int)doc.StockBehavior == (int)StockBehaviorEnum.sbStockCompose || (int)doc.StockBehavior == (int)StockBehaviorEnum.sbStockDecompose) {
                    fillComponentListGrid(doc.StockBehavior, trans.Details);
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
                APIEngine.CoreGlobals.MsgBoxFrontOffice("A transação indicada não existe.", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
            }
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
                    //TransactionGet(false);
                    txtTransDocNumber.Focus();
                    TransactionGet(false);
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

        /// <summary>
        /// Disables UI buttons on tabs that don't need them
        /// </summary>
        private void tabEntities_SelectedIndexChanged(object sender, EventArgs e) {

            int tabIndex = tabEntities.SelectedIndex;

            switch (tabIndex) {
                case 6: // SAF-T tab
                    btnInsert.Enabled = false;
                    BInsertViaJSON.Enabled = false;
                    BReadCurrentDocumentJSON.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnRemove.Enabled = false;
                    btnClear.Enabled = false;
                    btnGet.Enabled = false;
                    break;
                default:
                    btnInsert.Enabled = true;
                    BInsertViaJSON.Enabled = true;
                    BReadCurrentDocumentJSON.Enabled = true;
                    btnUpdate.Enabled = true;
                    btnRemove.Enabled = true;
                    btnClear.Enabled = true;
                    btnGet.Enabled = true;
                    break;
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e) {

        }

        private void lblTenderID_Click(object sender, EventArgs e) {
            try {
                var tenderID = QuickSearchHelper.TenderFind();
                if(tenderID > 0) {
                    txtTenderID.Text = tenderID.ToString();
                }

            } catch (Exception ex) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
        }

        private void lblPaymentID_Click(object sender, EventArgs e) {
            try {
                var paymentID = QuickSearchHelper.PaymentFind();
                if (paymentID > 0) {
                    txtPaymentID.Text = paymentID.ToString();
                }

            }
            catch (Exception ex) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
        }

        private void BInsertViaJSON_Click(object sender, EventArgs e)
        {
            try
            {
                var jSONFormResult = DialogResult.None;
                var jSONFormResultString = string.Empty;

                using (var jSONForm = new JSONForm())
                {
                    jSONFormResult = jSONForm.ShowDialog();
                    jSONFormResultString = jSONForm.JSON;
                }

                if (jSONFormResult != DialogResult.OK)
                    return;

                transactionError = false;
                TransactionID transId = null;
                
                transId = TransactionInsertViaJSON(false, jSONFormResultString);

                if (!transactionError)
                {
                    string msg = null;
                    if (transId != null)
                    {
                        msg = $"Registo inserido: {transId.ToString()}";
                    }
                    else
                    {
                        msg = "Registo inserido.";
                    }

                    APIEngine.CoreGlobals.MsgBoxFrontOffice(msg, VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                }
            }
            catch (Exception ex)
            {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
        }
    }
}