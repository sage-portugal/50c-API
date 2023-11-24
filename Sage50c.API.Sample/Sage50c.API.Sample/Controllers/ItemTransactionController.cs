using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using S50cBL22;
using S50cBO22;
using S50cDL22;
using S50cPrint22;
using S50cSys22;
using S50cUtil22;

namespace Sage50c.API.Sample.Controllers {
    internal class ItemTransactionController : ControllerBase {

        private BSOItemTransaction _bsoItemTransaction = null;

        public ItemTransaction Transaction { get { return _bsoItemTransaction.Transaction; } }

        private Document _document = null;
        
        public ItemTransactionController() {
            _bsoItemTransaction = new BSOItemTransaction();
        } 

        /// <summary>
        /// Create a new buy/sale transaction
        /// </summary>
        public ItemTransaction Create(string TransDoc, string TransSerial) {
            Initialize(TransDoc, TransSerial);

            FillSuggestedValues();
            editState = EditState.New;

            return _bsoItemTransaction.Transaction;
        }

        /// <summary>
        /// Get transaction from database
        /// </summary>
        public ItemTransaction Load(bool Suspended, string TransDoc, string TransSerial, double TransDocNum) {

            dynamic trans = null;
            editState = EditState.Editing;
            if (systemSettings.WorkstationInfo.Document.IsInCollection(TransDoc)) {
                _document = systemSettings.WorkstationInfo.Document[TransDoc];
                _bsoItemTransaction.Transaction.TransDocType = TransGetType(TransDoc);
            }
            else {
                throw new Exception($"O documento [{TransDoc}] não existe");
            }

            if (Suspended) {
                if (_bsoItemTransaction.LoadSuspendedTransaction(TransSerial, TransDoc, TransDocNum)) {  
                    trans = _bsoItemTransaction.Transaction;
                    return trans;
                }
                else {
                    throw new Exception($"O documento {TransDoc} {TransSerial}/{TransDocNum} não existe em preparação.");
                }
            }
            else {
                if (_document.TransDocType == DocumentTypeEnum.dcTypeSale || _document.TransDocType == DocumentTypeEnum.dcTypePurchase) {

                    if (!_bsoItemTransaction.LoadItemTransaction(_document.TransDocType, TransSerial, TransDoc, TransDocNum)) {
                        throw new Exception($"Não foi possível ler o documento {TransDoc} {TransSerial}/{TransDocNum}");
                    }
                    trans = _bsoItemTransaction.Transaction;
                    return trans;
                }
                else {
                    throw new Exception($"O Documento {TransDoc} não é um documento de compra/venda");
                }
            }
        }

        /// <summary>
        /// Save (insert or update) transaction
        /// </summary>
        public bool Save(bool Suspended) {

            if (Validate(Suspended)) {
                SetUserPermissions();

                _bsoItemTransaction.EnsureOpenTill(_bsoItemTransaction.Transaction.Till.TillID);

                _bsoItemTransaction.SaveDocument(false, false);

                editState = EditState.Editing;
                _document = null;
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Delete transaction 
        /// </summary>
        public TransactionID Remove() {

            var transType = TransGetType(_bsoItemTransaction.Transaction.TransDocument);

            if (transType != DocumentTypeEnum.dcTypeSale && transType != DocumentTypeEnum.dcTypePurchase) {
                throw new Exception($"O documento indicado [{_bsoItemTransaction.Transaction.TransDocument}] não é um documento de venda/compra");
            }
            else {
                if (_bsoItemTransaction.LoadItemTransaction(transType, _bsoItemTransaction.Transaction.TransSerial, _bsoItemTransaction.Transaction.TransDocument, _bsoItemTransaction.Transaction.TransDocNumber)) {
                    _bsoItemTransaction.Transaction.VoidMotive = "Anulado por: " + Application.ProductName;
                    if (_bsoItemTransaction.DeleteItemTransaction(false)) {
                        _document = null;
                        return _bsoItemTransaction.Transaction.TransactionID;
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

        /// <summary>
        /// Initialize new transaction
        /// </summary>
        public void Initialize(string TransDoc, string TransSerial) {

            _bsoItemTransaction = new BSOItemTransaction() {
                TransactionType = TransGetType(TransDoc),
            };

            _bsoItemTransaction.BSOItemTransactionDetail = new BSOItemTransactionDetail() {
                UserPermissions = systemSettings.User,
                PermissionsType = FrontOfficePermissionEnum.foPermByUser,
                TransactionType = TransGetType(TransDoc),
            };

            _bsoItemTransaction.InitNewTransaction(TransDoc, TransSerial);
            _document = systemSettings.WorkstationInfo.Document[TransDoc];
        }

        /// <summary>
        /// Validate Transaction 
        /// </summary>
        public bool Validate(bool Suspended) {

            StringBuilder error = new StringBuilder();

            DSODocument dsoDocument = new DSODocument();

            if (editState == EditState.New && _bsoItemTransaction.Transaction.TransDocNumber == 0) {

                if (dsoCache.ItemTransactionProvider.TransactionCount(_bsoItemTransaction.Transaction.TransDocType) == 0) {
                    _bsoItemTransaction.Transaction.TransDocNumber = 1;
                }
                else {
                    _bsoItemTransaction.Transaction.TransDocNumber = dsoDocument.GetLastDocNumber(_bsoItemTransaction.Transaction.TransDocType, _bsoItemTransaction.Transaction.TransSerial, _bsoItemTransaction.Transaction.TransDocument, _bsoItemTransaction.Transaction.WorkstationStamp.WorkstationID) + 1;
                }
            }

            if (editState != EditState.New && !Suspended) {
                if (!dsoCache.ItemTransactionProvider.ItemTransactionExists(_bsoItemTransaction.Transaction.TransSerial, _bsoItemTransaction.Transaction.TransDocument, _bsoItemTransaction.Transaction.TransDocNumber)) {
                    throw new Exception($"O documento {_bsoItemTransaction.Transaction.TransDocument} {_bsoItemTransaction.Transaction.TransSerial}/{_bsoItemTransaction.Transaction.TransDocNumber} não existe para ser alterado. Deve criar um novo.");
                }
            }
            else if (editState == EditState.New && dsoCache.ItemTransactionProvider.ItemTransactionExists(_bsoItemTransaction.Transaction.TransSerial, _bsoItemTransaction.Transaction.TransDocument, _bsoItemTransaction.Transaction.TransDocNumber)) {
                throw new Exception($"O documento {_bsoItemTransaction.Transaction.TransDocument} {_bsoItemTransaction.Transaction.TransSerial}/{_bsoItemTransaction.Transaction.TransDocNumber} já existe. Deve criar um novo.");
            }

            if (_bsoItemTransaction.Transaction == null) {
                if (editState == EditState.New) {
                    throw new Exception($"Não foi possível inicializar o documento [{_bsoItemTransaction.Transaction.TransDocument}] da série [{_bsoItemTransaction.Transaction.TransSerial}]");
                }
                else {
                    throw new Exception($"Não foi possível carregar o documento [{_bsoItemTransaction.Transaction.TransDocument}] da série [{_bsoItemTransaction.Transaction.TransSerial}] número [{_bsoItemTransaction.Transaction.TransDocNumber}]");
                }
            }
            else {

                if (!systemSettings.WorkstationInfo.Document.IsInCollection(_bsoItemTransaction.Transaction.TransDocument)) {
                    error.AppendLine("O Documento não se encontra preenchido ou não existe");
                }

                _bsoItemTransaction.Transaction.TransDocType = TransGetType(_bsoItemTransaction.Transaction.TransDocument);
                if (_bsoItemTransaction.Transaction.TransDocType != DocumentTypeEnum.dcTypeSale && _bsoItemTransaction.Transaction.TransDocType != DocumentTypeEnum.dcTypePurchase) {
                    error.AppendLine($"O documento indicado [{_bsoItemTransaction.Transaction.TransDocument}] não é um documento de venda/compra");
                }

                if (!systemSettings.DocumentSeries.IsInCollection(_bsoItemTransaction.Transaction.TransSerial)) {
                    error.AppendLine("A Série não se encontra preenchida ou não existe");
                }

                if (_bsoItemTransaction.Transaction.TransDocNumber == 0) {
                    error.AppendLine("O número de Documento não se encontra preenchido");
                }

                if (string.IsNullOrEmpty(_bsoItemTransaction.Transaction.BaseCurrency.CurrencyID)) {
                    _bsoItemTransaction.Transaction.BaseCurrency = systemSettings.BaseCurrency;
                }
                else {
                    var currency = dsoCache.CurrencyProvider.GetCurrency(_bsoItemTransaction.Transaction.BaseCurrency.CurrencyID);
                    if (currency != null) {
                        _bsoItemTransaction.Transaction.BaseCurrency = currency;
                    }
                    else {
                        throw new Exception($"A moeda [{_bsoItemTransaction.Transaction.BaseCurrency.CurrencyID}] não existe");
                    }
                }
                if (_bsoItemTransaction.Transaction.Payment.PaymentID == 0) {
                    var paymentId = dsoCache.PaymentProvider.GetFirstID();
                    _bsoItemTransaction.Transaction.Payment = dsoCache.PaymentProvider.GetPayment(paymentId);
                }
                else {
                    var payment = dsoCache.PaymentProvider.GetPayment(_bsoItemTransaction.Transaction.Payment.PaymentID);
                    if (payment != null) {
                        _bsoItemTransaction.Transaction.Payment = payment;
                    }
                    else {
                        error.AppendLine($"O pagamento não existe");
                    }
                }
                if (_bsoItemTransaction.Transaction.Tender.TenderID == 0) {
                    var tenderId = dsoCache.TenderProvider.GetFirstID();
                    _bsoItemTransaction.Transaction.Tender = dsoCache.TenderProvider.GetTender(tenderId);
                }
                else {
                    var tender = dsoCache.TenderProvider.GetTender(_bsoItemTransaction.Transaction.Tender.TenderID);
                    if (tender != null) {
                        _bsoItemTransaction.Transaction.Tender = tender;
                    }
                    else {
                        error.AppendLine($"O método de pagamento não existe");
                    }
                }
                if (_bsoItemTransaction.Transaction.Salesman.SalesmanID == 0) {
                    var salesman = dsoCache.SalesmanProvider.GetFirstSalesmanID();
                    _bsoItemTransaction.Transaction.Salesman = dsoCache.SalesmanProvider.GetSalesman(salesman);
                }
                else {
                    var salesman = dsoCache.SalesmanProvider.GetSalesman(_bsoItemTransaction.Transaction.Salesman.SalesmanID);
                    if (salesman != null) {
                        _bsoItemTransaction.Transaction.Salesman = salesman;
                    }
                    else {
                        error.AppendLine($"O vendedor não existe ou necessita de password");
                    }
                }
                // Error message
                if (error.Length > 0) {
                    throw new Exception(error.ToString());
                }
                else {
                    return true;
                }
            }
        }

        /// <summary>
        /// Add item details 
        /// </summary>
        public void AddDetail(double TaxPercent, ItemTransactionDetail Detail) {

            _document = systemSettings.WorkstationInfo.Document[_bsoItemTransaction.Transaction.TransDocument];
            //Get item
            Item item = dsoCache.ItemProvider.GetItem(Detail.ItemID, systemSettings.BaseCurrency);

            if (_bsoItemTransaction.Transaction.BaseCurrency == null) {
                Detail.BaseCurrency = systemSettings.BaseCurrency;
            }
            else {
                CurrencyDefinition currencyDefinition = new CurrencyDefinition();
                currencyDefinition = dsoCache.CurrencyProvider.GetCurrency(_bsoItemTransaction.Transaction.BaseCurrency.CurrencyID);
                if (currencyDefinition != null) {
                    Detail.BaseCurrency = currencyDefinition;
                }
                else {
                    Detail.BaseCurrency = systemSettings.BaseCurrency;
                }
            }

            Detail.CreateDate = _bsoItemTransaction.Transaction.CreateDate;
            Detail.CreateTime = _bsoItemTransaction.Transaction.CreateTime;
            Detail.ActualDeliveryDate = _bsoItemTransaction.Transaction.ActualDeliveryDate;

            Detail.Description = item.Description;
            Detail.UnitList = item.UnitList;

            short TaxGroupId = 0;
            if (TaxPercent == 0 && item.TaxableGroupID != 0) {
                //se não preencher a taxa, carrega o imposto do artigo
                TaxGroupId = item.TaxableGroupID;
            }
            else {
                // Carrega o imposto pela ZONA
                // IMPORTANTE OSS: A transação já deve ter neste ponto a ZONA correta carregada
                TaxGroupId = _bsoItemTransaction.BSOTaxes.GetTaxableGroupIDFromTaxRate(TaxPercent, _bsoItemTransaction.Transaction.Zone.CountryID, _bsoItemTransaction.Transaction.Zone.TaxRegionID);
            }
            Detail.TaxableGroupID = TaxGroupId;

            short warehouse = Detail.WarehouseID;
            if (dsoCache.WarehouseProvider.WarehouseExists(warehouse)) {
                Detail.WarehouseID = warehouse;
            }
            else {
                Detail.WarehouseID = _document.Defaults.Warehouse;
            }

            Detail.LineItemID = _bsoItemTransaction.Transaction.Details.Count + 1;

            //Last purchase price 
            if (_document.TransDocType == DocumentTypeEnum.dcTypePurchase) {
                Detail.ItemExtraInfo.ItemLastCostTaxIncludedPrice = item.SalePrice[0, Detail.Size.SizeID, string.Empty, 0, item.UnitOfSaleID].TaxIncludedPrice;
                Detail.ItemExtraInfo.ItemLastCostUnitPrice = item.SalePrice[0, Detail.Size.SizeID, string.Empty, 0, item.UnitOfSaleID].UnitPrice;
            }

            var colorId = Detail.Color.ColorID;
            if (colorId > 0 && item.Colors.Count > 0) {
                ItemColor color = null;
                if (colorId > 0 && item.Colors.IsInCollection(colorId)) {
                    color = item.Colors[ref colorId];
                }
                else {
                    throw new Exception($"A cor indicada [{colorId}] não existe");
                }
                Detail.Color.Description = color.ColorName;
                Detail.Color.ColorKey = color.ColorKey;
                Detail.Color.ColorCode = color.ColorCode;
            }

            var sizeId = Detail.Size.SizeID;
            if (sizeId > 0 && item.Sizes.Count > 0) {
                ItemSize size = null;
                if (sizeId > 0 && item.Sizes.IsInCollection(sizeId)) {
                    size = item.Sizes[sizeId];
                }
                else {
                    throw new Exception($"O tamanho indicado [{sizeId}] não existe");
                }
                Detail.Size.Description = size.SizeName;
                Detail.Size.SizeKey = size.SizeKey;
                Detail.Size.SizeID = size.SizeID;
            }

            if (!string.IsNullOrEmpty(Detail.ItemProperties.PropertyValue1)) {
                if (item.PropertyEnabled) {
                    if (item.PropertyID1.Equals("NS", StringComparison.CurrentCultureIgnoreCase) || item.PropertyID1.Equals("LOT", StringComparison.CurrentCultureIgnoreCase)) {
                        Detail.ItemProperties.PropertyID1 = item.PropertyID1;
                        Detail.ItemProperties.PropertyID2 = item.PropertyID2;
                        Detail.ItemProperties.PropertyID3 = item.PropertyID3;
                        Detail.ItemProperties.ControlMode = item.PropertyControlMode;
                        Detail.ItemProperties.ControlType = item.PropertyControlType;
                        Detail.ItemProperties.UseExpirationDate = item.PropertyUseExpirationDate;
                        Detail.ItemProperties.UseProductionDate = item.PropertyUseProductionDate;
                        Detail.ItemProperties.ExpirationDateControl = item.PropertyExpirationDateControl;
                        Detail.ItemProperties.MaximumQuantity = item.PropertyMaximumQuantity;
                        Detail.ItemProperties.UsePriceOnProp1 = item.UsePriceOnProp1;
                        Detail.ItemProperties.UsePriceOnProp2 = item.UsePriceOnProp2;
                        Detail.ItemProperties.UsePriceOnProp3 = item.UsePriceOnProp3;
                    }
                }
                else {
                    throw new Exception($"O artigo indicado [{item.ItemID}] não possui propriedades");
                }
            }

            Detail.Graduation = item.Graduation;
            Detail.ItemTax = item.ItemTax;
            Detail.ItemTax2 = item.ItemTax2;
            Detail.ItemTax3 = item.ItemTax3;

            Detail.ItemType = item.ItemType;
            if (item.ItemType == ItemTypeEnum.itmService || item.ItemType == ItemTypeEnum.itmInterestRate || item.ItemType == ItemTypeEnum.itmOtherProductOrService) {
                Detail.RetentionTax = item.WithholdingTaxRate;
            }

            item = null;
            _bsoItemTransaction.Transaction.Details.Add(Detail);
            Detail = null;
        }

        public bool FillSuggestedValues() {

            if (_bsoItemTransaction.Transaction != null) {
                _bsoItemTransaction.Transaction.WorkstationStamp.SessionID = APIEngine.SystemSettings.TillSession.SessionID;
                _bsoItemTransaction.Transaction.Comments = "Gerado por: " + Application.ProductName;
                _bsoItemTransaction.Transaction.CreateDate = DateTime.Today.Date;
                _bsoItemTransaction.Transaction.CreateTime = DateTime.Today;
                _bsoItemTransaction.Transaction.ActualDeliveryDate = DateTime.Today.Date;
                _bsoItemTransaction.Transaction.TransDocType = TransGetType(_bsoItemTransaction.Transaction.TransDocument);
                return true;
            }
            else {
                return false;
            }
        }

        public void CreateCostShare(SimpleDocumentList simpleDocumentList) {
            _bsoItemTransaction.Transaction.BuyShareOtherCostList = null;
            _bsoItemTransaction.Transaction.BuyShareOtherCostList = simpleDocumentList;
        }

        public void SetPartyID(double PartyID) {
            _bsoItemTransaction.PartyID = PartyID;
        }

        public void SetPaymentDiscountPercent(double PaymentDiscountPercent) {

            _bsoItemTransaction.PaymentDiscountPercent1 = PaymentDiscountPercent;

        }

        public void SetUserPermissions() {
            _bsoItemTransaction.UserPermissions = systemSettings.User;
            _bsoItemTransaction.PermissionsType = FrontOfficePermissionEnum.foPermByUser;
        }

        public TransactionID SuspendTransaction() {
            return _bsoItemTransaction.SuspendCurrentTransaction();
        }

        public bool FinalizeTransaction(string TransSerial, string TransDoc, double TransDocNumber) {
            return _bsoItemTransaction.FinalizeSuspendedTransaction(TransSerial, TransDoc, TransDocNumber);
        }

        public void Calculate() {
            _bsoItemTransaction.Calculate(true, true);
        }

    }
}