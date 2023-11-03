using DAO;
using S50cBL22;
using S50cBO22;
using S50cDL22;
using S50cPrint22;
using S50cSys22;
using S50cUtil22;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sage50c.API.Sample.Controllers {
    internal class BuySaleTransactionController {

        private SystemSettings _systemSettings { get { return APIEngine.SystemSettings; } }

        private DSOFactory _dsoCache { get { return APIEngine.DSOCache; } }

        private BSOItemTransaction _bsoItemTransaction = null;

        private PrintingManager _printingManager { get { return APIEngine.PrintingManager; } }

        public BSOItemTransaction BsoItemTransaction { get { return _bsoItemTransaction; } }

        private EditState _editState = EditState.None;

        public ItemTransaction Create(string transDoc, string transSerial) {
            Initialize(transDoc);
            _bsoItemTransaction.InitNewTransaction(transDoc, transSerial);

            FillSuggestedValues();
            _editState = EditState.New;

            return _bsoItemTransaction.Transaction;
        }

        public void Initialize(string transDoc) {
            //ResetDetails();
            _bsoItemTransaction = new BSOItemTransaction() {
                TransactionType = ItemTransactionHelper.TransGetType(transDoc),
            };

            _bsoItemTransaction.BSOItemTransactionDetail = new BSOItemTransactionDetail() {
                UserPermissions = _systemSettings.User,
                PermissionsType = FrontOfficePermissionEnum.foPermByUser,
                TransactionType = ItemTransactionHelper.TransGetType(transDoc),
            };

            //_itemTransactionDetail = _bsoItemTransaction.BSOItemTransactionDetail.TransactionDetail;
            _bsoItemTransaction.Transaction = new ItemTransaction();
        }

        public ItemTransaction Load(bool suspended, string transDoc, string transSerial, double transDocNum) {
            S50cSys22.Document doc = null;
            dynamic trans = null;
            _bsoItemTransaction = new BSOItemTransaction();
            _editState = EditState.Editing;
            if (_systemSettings.WorkstationInfo.Document.IsInCollection(transDoc)) {
                doc = _systemSettings.WorkstationInfo.Document[transDoc];
            }
            else {
                throw new Exception($"O documento [{transDoc}] não existe");
            }

            if (suspended) {
                if (_bsoItemTransaction.LoadSuspendedTransaction(transSerial, transDoc, transDocNum)) {
                    trans = _bsoItemTransaction.Transaction;
                    return trans;
                }
                else {
                    throw new Exception($"O documento {transDoc} {transSerial}/{transDocNum}");
                }
            }
            else {
                if (doc.TransDocType == DocumentTypeEnum.dcTypeSale || doc.TransDocType == DocumentTypeEnum.dcTypePurchase) {

                    if (!_bsoItemTransaction.LoadItemTransaction(doc.TransDocType, transSerial, transDoc, transDocNum)) {
                        throw new Exception($"Não foi possível ler o documento {transDoc} {transSerial}/{transDocNum}");
                    }
                    trans = _bsoItemTransaction.Transaction;
                    return trans;
                }
                else {
                    throw new Exception($"O Documento {transDoc} não é um documento de compra/venda");
                }
            }
        }

        public void Save(bool suspended) {

            if (Validate(suspended)) {

                //Calculate document
                _bsoItemTransaction.Calculate(true, true);

                _bsoItemTransaction.EnsureOpenTill(_bsoItemTransaction.Transaction.Till.TillID);
                _bsoItemTransaction.SaveDocument(false, false);

                _editState = EditState.Editing;
            }
        }

        public bool Remove() {
            var transType = ItemTransactionHelper.TransGetType(_bsoItemTransaction.Transaction.TransDocument);

            if (transType != DocumentTypeEnum.dcTypeSale && transType != DocumentTypeEnum.dcTypePurchase) {
                throw new Exception($"O documento indicado [{_bsoItemTransaction.Transaction.TransDocument}] não é um documento de venda/compra");
            }
            else {
                if (_bsoItemTransaction.LoadItemTransaction(transType, _bsoItemTransaction.Transaction.TransSerial, _bsoItemTransaction.Transaction.TransDocument, _bsoItemTransaction.Transaction.TransDocNumber)) {
                    _bsoItemTransaction.Transaction.VoidMotive = "Anulado por: " + Application.ProductName;
                    if (_bsoItemTransaction.DeleteItemTransaction(false)) {
                        return true;
                    }
                    else {
                        throw new Exception($"Não foi possível anular o Documento {_bsoItemTransaction.Transaction.TransDocument} {_bsoItemTransaction.Transaction.TransSerial}/{_bsoItemTransaction.Transaction.TransDocNumber}");
                    }
                }
                else {
                    throw new Exception($"Não foi possível carregar o Documento {_bsoItemTransaction.Transaction.TransDocument} {_bsoItemTransaction.Transaction.TransSerial}/{_bsoItemTransaction.Transaction.TransDocNumber}");
                }
            }
        }

        public void Print(bool printReview, bool printOpt) {
            clsLArrayObject objListPrintSettings;
            PrintSettings oPrintSettings = null;
            S50cSys22.Document oDocument = null;
            PlaceHolders oPlaceHolders = new PlaceHolders();

            try {
                oDocument = _systemSettings.WorkstationInfo.Document[_bsoItemTransaction.Transaction.TransDocument];

                var defaultPrintSettings = new PrintSettings() {
                    AskForPrinter = false,
                    UseIssuingOutput = false,
                    PrintAction = printReview ? PrintActionEnum.prnActPreview : PrintActionEnum.prnActPrint
                };
                if (printOpt) {
                    defaultPrintSettings.PrintAction = PrintActionEnum.prnActExportToFile;
                    defaultPrintSettings.ExportFileType = ExportFileTypeEnum.filePDF;
                    defaultPrintSettings.ExportFileFolder = oPlaceHolders.GetPlaceHolderPath(_systemSettings.WorkstationInfo.PDFDestinationFolder);
                }
                objListPrintSettings = _printingManager.GetTransactionPrintSettings(oDocument, _bsoItemTransaction.Transaction.TransSerial, ref defaultPrintSettings);
                if (objListPrintSettings.getCount() > 0) {
                    oPrintSettings = (PrintSettings)objListPrintSettings.item[0];

                    if (printReview) {
                        _bsoItemTransaction.PrintTransaction(_bsoItemTransaction.Transaction.TransSerial, _bsoItemTransaction.Transaction.TransDocument, _bsoItemTransaction.Transaction.TransDocNumber, PrintJobEnum.jobPreview, oPrintSettings.PrintCopies);
                    }
                    else {
                        _bsoItemTransaction.PrintTransaction(_bsoItemTransaction.Transaction.TransSerial, _bsoItemTransaction.Transaction.TransDocument, _bsoItemTransaction.Transaction.TransDocNumber, PrintJobEnum.jobPrint, oPrintSettings.PrintCopies, oPrintSettings);
                    }
                }
                APIEngine.CoreGlobals.MsgBoxFrontOffice("Concluído", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
            }
            catch (Exception ex) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
            finally {
                oDocument = null;
                oPlaceHolders = null;
            }
        }

        public bool Validate(bool suspended) {

            if (_editState != EditState.New && !suspended) {
                if (!_dsoCache.ItemTransactionProvider.ItemTransactionExists(_bsoItemTransaction.Transaction.TransSerial, _bsoItemTransaction.Transaction.TransDocument, _bsoItemTransaction.Transaction.TransDocNumber)) {
                    throw new Exception($"O documento {_bsoItemTransaction.Transaction.TransDocument} {_bsoItemTransaction.Transaction.TransSerial}/{_bsoItemTransaction.Transaction.TransDocNumber} não existe para ser alterado. Deve criar um novo.");
                }
            }
            else if (_editState == EditState.New && _dsoCache.ItemTransactionProvider.ItemTransactionExists(_bsoItemTransaction.Transaction.TransSerial, _bsoItemTransaction.Transaction.TransDocument, _bsoItemTransaction.Transaction.TransDocNumber)) {
                throw new Exception($"O documento {_bsoItemTransaction.Transaction.TransDocument} {_bsoItemTransaction.Transaction.TransSerial}/{_bsoItemTransaction.Transaction.TransDocNumber} já existe. Deve criar um novo.");
            }

            if (_bsoItemTransaction.Transaction == null) {
                if (_editState == EditState.New) {
                    throw new Exception($"Não foi possível inicializar o documento [{_bsoItemTransaction.Transaction.TransDocument}] da série [{_bsoItemTransaction.Transaction.TransSerial}]");
                }
                else {
                    throw new Exception($"Não foi possível carregar o documento [{_bsoItemTransaction.Transaction.TransDocument}] da série [{_bsoItemTransaction.Transaction.TransSerial}] número [{_bsoItemTransaction.Transaction.TransDocNumber}]");
                }
            }
            else {
                if (!_systemSettings.WorkstationInfo.Document.IsInCollection(_bsoItemTransaction.Transaction.TransDocument)) {
                    throw new Exception("O Documento não se encontra preenchido ou não existe");
                }

                if (_bsoItemTransaction.Transaction.TransDocType != DocumentTypeEnum.dcTypeSale && _bsoItemTransaction.Transaction.TransDocType != DocumentTypeEnum.dcTypePurchase) {
                    throw new Exception($"O documento indicado [{_bsoItemTransaction.Transaction.TransDocument}] não é um documento de venda/compra");
                }
                if (!_systemSettings.DocumentSeries.IsInCollection(_bsoItemTransaction.Transaction.TransSerial)) {
                    throw new Exception("A Série não se encontra preenchida ou não existe");
                }
                if (string.IsNullOrEmpty(_bsoItemTransaction.Transaction.BaseCurrency.CurrencyID)) {
                    _bsoItemTransaction.Transaction.BaseCurrency = _systemSettings.BaseCurrency;
                }
                else {
                    var currency = _dsoCache.CurrencyProvider.GetCurrency(_bsoItemTransaction.Transaction.BaseCurrency.CurrencyID);
                    if (currency != null) {
                        _bsoItemTransaction.Transaction.BaseCurrency = currency;
                    }
                    else {
                        throw new Exception($"A moeda [{_bsoItemTransaction.Transaction.BaseCurrency.CurrencyID}] não existe");
                    }
                }
                if (_bsoItemTransaction.Transaction.Payment.PaymentID == 0) {
                    var paymentId = _dsoCache.PaymentProvider.GetFirstID();
                    _bsoItemTransaction.Transaction.Payment = _dsoCache.PaymentProvider.GetPayment(paymentId);
                }
                else {
                    var payment = _dsoCache.PaymentProvider.GetPayment(_bsoItemTransaction.Transaction.Payment.PaymentID);
                    if (payment != null) {
                        _bsoItemTransaction.Transaction.Payment = payment;
                    }
                    else {
                        throw new Exception($"O pagamento não existe");
                    }
                }
                if (_bsoItemTransaction.Transaction.Tender.TenderID == 0) {
                    var tenderId = _dsoCache.TenderProvider.GetFirstID();
                    _bsoItemTransaction.Transaction.Tender = _dsoCache.TenderProvider.GetTender(tenderId);
                }
                else {
                    var tender = _dsoCache.TenderProvider.GetTender(_bsoItemTransaction.Transaction.Tender.TenderID);
                    if (tender != null) {
                        _bsoItemTransaction.Transaction.Tender = tender;
                    }
                    else {
                        throw new Exception($"O método de pagamento não existe");
                    }
                }
                
                return true;
            }
        }

        public void FillSuggestedValues() {

            _bsoItemTransaction.Transaction.WorkstationStamp.SessionID = APIEngine.SystemSettings.TillSession.SessionID;
            _bsoItemTransaction.Transaction.Comments = "Gerado por: " + Application.ProductName;

            _bsoItemTransaction.Transaction.CreateDate = DateTime.Today;
            _bsoItemTransaction.Transaction.CreateTime = DateTime.Today;
            _bsoItemTransaction.Transaction.ActualDeliveryDate = DateTime.Today;
        }

        public void FillDefaultDetails(ItemTransactionDetail details) {
            details.CreateDate = DateTime.Today;
            details.CreateTime = DateTime.Today;
            details.ActualDeliveryDate = DateTime.Today;
        }

        public void AddDetailsItem(S50cSys22.Document document, double taxPercent, ItemTransactionDetail details) {

            //ItemTransactionDetail itemTransactionDetail = new ItemTransactionDetail();

            //Get item
            Item item = _dsoCache.ItemProvider.GetItem(details.ItemID, details.BaseCurrency);

            details.Description = item.Description;
            details.UnitList = item.UnitList;
            details.Graduation = item.Graduation;
            details.ItemTax = item.ItemTax;
            details.ItemTax2 = item.ItemTax2;
            details.ItemTax3 = item.ItemTax3;
            details.ItemType = item.ItemType;

            short TaxGroupId = 0;
            if (taxPercent == 0 && item.TaxableGroupID != 0) {
                //se não preencher a taxa, carrega o imposto do artigo
                TaxGroupId = item.TaxableGroupID;
            }
            else {
                // Carrega o imposto pela ZONA
                // IMPORTANTE OSS: A transação já deve ter neste ponto a ZONA correta carregada
                TaxGroupId = _bsoItemTransaction.BSOTaxes.GetTaxableGroupIDFromTaxRate(taxPercent, _bsoItemTransaction.Transaction.Zone.CountryID, _bsoItemTransaction.Transaction.Zone.TaxRegionID);
            }
            details.TaxableGroupID = TaxGroupId;
            details.LineItemID = _bsoItemTransaction.Transaction.Details.Count + 1;
            
            if(!string.IsNullOrEmpty(details.UnitOfSaleID)) {
                details.SetUnitOfSaleID(details.UnitOfSaleID.ToUpper());
            }

            //Last purchase price 
            if (document.TransDocType == DocumentTypeEnum.dcTypePurchase) {
                details.ItemExtraInfo.ItemLastCostTaxIncludedPrice = item.SalePrice[0, details.Size.SizeID, string.Empty, 0, item.UnitOfSaleID].TaxIncludedPrice;
                details.ItemExtraInfo.ItemLastCostUnitPrice = item.SalePrice[0, details.Size.SizeID, string.Empty, 0, item.UnitOfSaleID].UnitPrice;
            }

            if (item.ItemType == ItemTypeEnum.itmService || item.ItemType == ItemTypeEnum.itmInterestRate || item.ItemType == ItemTypeEnum.itmOtherProductOrService) {
                details.RetentionTax = item.WithholdingTaxRate;
            }

            var colorId = details.Color.ColorID;
            if (colorId > 0 && item.Colors.Count > 0) {
                ItemColor color = null;
                if (colorId > 0 && item.Colors.IsInCollection(colorId)) {
                    color = item.Colors[ref colorId];
                }
                else {
                    throw new Exception($"A cor indicada [{colorId}] não existe");
                }
                details.Color.Description = color.ColorName;
                details.Color.ColorKey = color.ColorKey;
                details.Color.ColorCode = color.ColorCode;
            }

            var sizeId = details.Size.SizeID;
            if (sizeId > 0 && item.Sizes.Count > 0) {
                ItemSize size = null;
                if (sizeId > 0 && item.Sizes.IsInCollection(sizeId)) {
                    size = item.Sizes[sizeId];
                }
                else {
                    throw new Exception($"A cor indicada [{sizeId}] não existe");
                }
                details.Size.Description = size.SizeName;
                details.Size.SizeKey = size.SizeKey;
                details.Size.SizeID = size.SizeID;
            }

            if (!string.IsNullOrEmpty(details.ItemProperties.PropertyValue1)) {
                if (item.PropertyEnabled) {
                    if (item.PropertyID1.Equals("NS", StringComparison.CurrentCultureIgnoreCase) || item.PropertyID1.Equals("LOT", StringComparison.CurrentCultureIgnoreCase)) {
                        details .ItemProperties.ResetValues();
                        details.ItemProperties.PropertyID1 = item.PropertyID1;
                        details.ItemProperties.PropertyID2 = item.PropertyID2;
                        details.ItemProperties.PropertyID3 = item.PropertyID3;
                        details.ItemProperties.ControlMode = item.PropertyControlMode;
                        details.ItemProperties.ControlType = item.PropertyControlType;
                        details.ItemProperties.UseExpirationDate = item.PropertyUseExpirationDate;
                        details.ItemProperties.UseProductionDate = item.PropertyUseProductionDate;
                        details.ItemProperties.ExpirationDateControl = item.PropertyExpirationDateControl;
                        details.ItemProperties.MaximumQuantity = item.PropertyMaximumQuantity;
                        details.ItemProperties.UsePriceOnProp1 = item.UsePriceOnProp1;
                        details.ItemProperties.UsePriceOnProp2 = item.UsePriceOnProp2;
                        details.ItemProperties.UsePriceOnProp3 = item.UsePriceOnProp3;
                    }
                }
                else {
                    throw new Exception($"O artigo indicado [{item.ItemID}] não possui propriedades");
                }
            }

            item = null;
            _bsoItemTransaction.Transaction.Details.Add(details);
            details = null;
            
        }
    }
}
