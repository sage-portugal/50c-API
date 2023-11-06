using S50cBL22;
using S50cBO22;
using S50cDL22;
using S50cSys22;
using S50cUtil22;
using SageCoreSaft60;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sage50c.API.Sample.Controllers {
    internal class StockTransactionController {

        private SystemSettings _systemSettings { get { return APIEngine.SystemSettings; } }

        private DSOFactory _dsoCache { get { return APIEngine.DSOCache; } }

        private BSOStockTransaction _bsoStockTransaction = null;

        public BSOStockTransaction BsoStockTransaction { get { return _bsoStockTransaction; } }

        private EditState _editState = EditState.None;

        public StockTransaction Create(string transDoc, string transSerial) {
            Initialize(transDoc);
            _bsoStockTransaction.InitNewTransaction(transDoc, transSerial);

            _editState = EditState.New;
            return _bsoStockTransaction.Transaction;

        }

        public void Initialize(string transDoc) {
            _bsoStockTransaction = new BSOStockTransaction();

            _bsoStockTransaction.BSOStockTransactionDetail = new BSOItemTransactionDetail() {
                UserPermissions = _systemSettings.User,
                PermissionsType = FrontOfficePermissionEnum.foPermByUser,
                TransactionType = ItemTransactionHelper.TransGetType(transDoc),
            };
           
            _bsoStockTransaction.Transaction = new StockTransaction();
        }

        public StockTransaction Load(string transDoc, string transSerial, double transDocNum) {
            S50cSys22.Document doc = null;
            dynamic trans = null;
            _bsoStockTransaction = new BSOStockTransaction();
            _editState = EditState.Editing;
            if (_systemSettings.WorkstationInfo.Document.IsInCollection(transDoc)) {
                doc = _systemSettings.WorkstationInfo.Document[transDoc];
            }
            else {
                throw new Exception($"O documento [{transDoc}] não existe");
            }
            if (doc.TransDocType == DocumentTypeEnum.dcTypeStock) {
                if (!_bsoStockTransaction.LoadStockTransaction(doc.TransDocType, transSerial, transDoc, transDocNum)) {
                    throw new Exception($"Não foi possível ler o documento [{transDoc} {transSerial}/{transDocNum}]");
                }
                trans = _bsoStockTransaction.Transaction;
                return trans;
            }
            else {
                throw new Exception($"O Documento {transDoc} não é um documento de stock");
            }

        }

        public void Save() {

            if (Validate()) {

                //Calculate document
                _bsoStockTransaction.Calculate(true, true, true);
                _bsoStockTransaction.SaveDocumentEx(true,false);

                _editState = EditState.Editing;
            }
        }

        public bool Validate() {
            StringBuilder error = new StringBuilder();

            if(_editState !=  EditState.New && !_dsoCache.ItemTransactionProvider.ItemTransactionExists(_bsoStockTransaction.Transaction.TransSerial, _bsoStockTransaction.Transaction.TransDocument, _bsoStockTransaction.Transaction.TransDocNumber)) {
                throw new Exception($"O documento {_bsoStockTransaction.Transaction.TransDocument} {_bsoStockTransaction.Transaction.TransSerial}/{_bsoStockTransaction.Transaction.TransDocNumber} não existe para ser alterado. Deve criar um novo.");
            } else if (_editState == EditState.New && _dsoCache.ItemTransactionProvider.ItemTransactionExists(_bsoStockTransaction.Transaction.TransSerial, _bsoStockTransaction.Transaction.TransDocument, _bsoStockTransaction.Transaction.TransDocNumber)) {
                throw new Exception($"O documento {_bsoStockTransaction.Transaction.TransDocument} {_bsoStockTransaction.Transaction.TransSerial}/{_bsoStockTransaction.Transaction.TransDocNumber} já existe para ser alterado. Deve criar um novo.");
            }

            if (_bsoStockTransaction.Transaction == null) {
                if (_editState == EditState.New) {
                    throw new Exception($"Não foi possível inicializar o documento [{_bsoStockTransaction.Transaction.TransDocument}] da série [{_bsoStockTransaction.Transaction.TransSerial}]");
                }
                else {
                    throw new Exception($"Não foi possível carregar o documento [{_bsoStockTransaction.Transaction.TransDocument}] da série [{_bsoStockTransaction.Transaction.TransSerial}] número [{_bsoStockTransaction.Transaction.TransDocNumber}]");
                }
            }
            else {
                if (!_systemSettings.WorkstationInfo.Document.IsInCollection(_bsoStockTransaction.Transaction.TransDocument)) {
                    error.Append($"O documento [{_bsoStockTransaction.Transaction.TransDocument}] não existe ou não se encontra preenchido.");
                }
                var transType = ItemTransactionHelper.TransGetType(_bsoStockTransaction.Transaction.TransDocument);
                if (transType != DocumentTypeEnum.dcTypeStock) {
                    error.Append($"O documento indicado [{_bsoStockTransaction.Transaction.TransDocument}] não é um documento de stock");
                }
                DocumentsSeries serie = null;
                if (_systemSettings.DocumentSeries.IsInCollection(_bsoStockTransaction.Transaction.TransSerial)) {
                    serie = _systemSettings.DocumentSeries[_bsoStockTransaction.Transaction.TransSerial];
                    if (serie.SeriesType != SeriesTypeEnum.SeriesExternal) {
                        error.Append("Apenas são permitidas séries externas.");
                    }
                }
                if (serie == null) {
                    error.Append("A série indicada não existe");
                }
                if (_bsoStockTransaction.Transaction.Details.Count <= 0) {
                    error.Append("O Documento não tem linhas");
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

        public bool Remove() {
            var transType = ItemTransactionHelper.TransGetType(_bsoStockTransaction.Transaction.TransDocument);
            if (transType != DocumentTypeEnum.dcTypeStock) {
                throw new Exception($"O documento indicado [{_bsoStockTransaction.Transaction.TransDocument}] não é um documento de stock.");
            }
            else {
                if (_bsoStockTransaction.LoadStockTransaction(transType, _bsoStockTransaction.Transaction.TransSerial, _bsoStockTransaction.Transaction.TransDocument, _bsoStockTransaction.Transaction.TransDocNumber)) {
                    // O motivo de anulação deve ser sempre preenchido.
                    // Se for obrigatório, o documento não é anulado sem que esteja preenchido
                    _bsoStockTransaction.Transaction.VoidMotive = "Anulado por: " + Application.ProductName;
                    //
                    if (_bsoStockTransaction.DeleteStockTransaction()) {
                        return true;
                    }
                    else {
                        throw new Exception($"Não foi possível anular o Documento {_bsoStockTransaction.Transaction.TransDocument} {_bsoStockTransaction.Transaction.TransSerial}/{_bsoStockTransaction.Transaction.TransDocNumber}");
                    }
                }
                else {
                    throw new Exception($"Não foi possível carregar o Documento {_bsoStockTransaction.Transaction.TransDocument} {_bsoStockTransaction.Transaction.TransSerial}/{_bsoStockTransaction.Transaction.TransDocNumber}");
                }
            }

        }

        public void AddDetailsStock(double taxRate, StockQtyRuleEnum StockQtyRule, ItemTransactionDetail details) {
            StockTransaction stockTransaction = _bsoStockTransaction.Transaction;

            details.BaseCurrency = stockTransaction.BaseCurrency;
            details.CreateDate = stockTransaction.CreateDate;
            details.ActualDeliveryDate = stockTransaction.ActualDeliveryDate;
            details.PartyTypeCode = stockTransaction.PartyTypeCode;
            details.PartyID = stockTransaction.PartyID;
            details.WarehouseOutgoing = details.WarehouseID;
            details.WarehouseReceipt = details.WarehouseID;
            details.PhysicalQtyRule = StockQtyRule;
            details.LineItemID = stockTransaction.Details.Count + 1;


            //Get item
            Item item = _dsoCache.ItemProvider.GetItemForTransactionDetail(details.ItemID, details.BaseCurrency);

            if (item != null) {
                details.Description = item.Description;
                details.TaxableGroupID = item.TaxableGroupID;
                details.ItemType = item.ItemType;
                details.FamilyID = item.Family.FamilyID;
                details.UnitList = item.UnitList.Clone();

                details.WeightUnitOfMeasure = item.WeightUnitOfMeasure;
                details.WeightMeasure = item.WeightMeasure;
                details.Graduation = item.Graduation;
                details.ItemTax = item.ItemTax;
                details.ItemTax2 = item.ItemTax2;
                details.ItemTax3 = item.ItemTax3;
                details.ItemExtraInfo.ItemQuantityCalcFormula = item.ItemQuantityCalcFormula;

                string unitOfSaleId = details.UnitOfSaleID;
                if (item.UnitList.IsInCollection(unitOfSaleId)) {
                    details.UnitOfSaleID = unitOfSaleId;
                }
                else {
                    details.UnitOfSaleID = item.GetDefaultUnitForTransaction(DocumentTypeEnum.dcTypeStock);
                }
            }
            else if (details.ItemID == "=") {
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
                throw new Exception($"O Artigo [{details.ItemID}] não foi entrado.");
            }

            details.TaxableGroupID = _dsoCache.TaxesProvider.GetTaxableGroupIDFromTaxRate(taxRate, APIEngine.SystemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID,
                                                                                                           APIEngine.SystemSettings.SystemInfo.TaxRegionID);
            details.SetUnitOfSaleID(details.UnitOfSaleID);

            //Formulas
            double Quantity1 = 0;
            double Quantity2 = 0;
            double Quantity3 = 0;
            double Quantity4 = 0;

            bool blnHaveSetUnits = false;

            details.Quantity1 = Quantity1;
            details.Quantity2 = Quantity2;
            details.Quantity3 = Quantity3;
            details.Quantity4 = Quantity4;
            if (!blnHaveSetUnits) {
                if (!string.IsNullOrEmpty(details.ItemExtraInfo.ItemQuantityCalcFormula) && APIEngine.SystemSettings.SystemInfo.UseUnitWithFormulaItems) {
                    details.SetQuantity(StockHelper.CalculateQuantity(details.ItemExtraInfo.ItemQuantityCalcFormula, details, true));
                }
                else {
                    details.SetQuantity(StockHelper.CalculateQuantity(null, details , true));
                }
            }
            //    
            if (!blnHaveSetUnits) {
                details.SetQuantity(details.Quantity);
            }
            details.Description = item.Description;     // OR "Custom description"
            details.Comments = "Observações de linha: Gerada por" + Application.ProductName;

            //*** UnitPrice
            if (stockTransaction.TransactionTaxIncluded) {
                details.TaxIncludedPrice = details.UnitPrice;
            }

            S50cUtil22.MathFunctions mathUtil = new MathFunctions();

            if (details.DiscountPercent == 0 && (details.CumulativeDiscountPercent1 != 0 || details.CumulativeDiscountPercent2 != 0 || details.CumulativeDiscountPercent3 != 0)) {
                details.DiscountPercent = mathUtil.GetCumulativeDiscount(details.CumulativeDiscountPercent1, details.CumulativeDiscountPercent2, details.CumulativeDiscountPercent3);
            }

            if (details.DiscountPercent != 0 && (details.CumulativeDiscountPercent1 == 0 && details.CumulativeDiscountPercent2 == 0 && details.CumulativeDiscountPercent3 == 0)) {
                details.CumulativeDiscountPercent1 = details.DiscountPercent;
            }

            if (details.ItemProperties.HasPropertyValues) {
                _dsoCache.ItemPropertyProvider.GetItemPropertyStock(details.ItemID, details.WarehouseID, details.ItemProperties);
            }


            bool calculate = true;
            item = null;
            _bsoStockTransaction.AddDetail(details, ref calculate);
            details = null;
        }

    }
}
