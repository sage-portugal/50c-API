using System;
using System.Text;
using System.Windows.Forms;

using S50cBL22;
using S50cBO22;
using S50cDL22;
using S50cSys22;
using S50cUtil22;

namespace Sage50c.API.Sample.Controllers {
    internal class StockTransactionController : ControllerBase {

        private BSOStockTransaction _bsoStockTransaction = null;

        public StockTransaction StockTransaction { get { return _bsoStockTransaction.Transaction; } }

        /// <summary>
        /// Create a new sotck transaction
        /// </summary>
        public StockTransaction Create(string TransDoc, string TransSerial) {

            Initialize(TransDoc, TransSerial);


            editState = EditState.New;
            return _bsoStockTransaction.Transaction;
        }

        /// <summary>
        /// Get transaction from database
        /// </summary>
        public StockTransaction Load(string TransDoc, string TransSerial, double TransDocNum) {

            Document doc = null;
            dynamic trans = null;
            _bsoStockTransaction = new BSOStockTransaction();
            editState = EditState.Editing;
            if (systemSettings.WorkstationInfo.Document.IsInCollection(TransDoc)) {
                doc = systemSettings.WorkstationInfo.Document[TransDoc];
            }
            else {
                throw new Exception($"O documento [{TransDoc}] não existe");
            }
            if (doc.TransDocType == DocumentTypeEnum.dcTypeStock) {
                if (!_bsoStockTransaction.LoadStockTransaction(doc.TransDocType, TransSerial, TransDoc, TransDocNum)) {
                    throw new Exception($"Não foi possível ler o documento [{TransDoc} {TransSerial}/{TransDocNum}]");
                }
                trans = _bsoStockTransaction.Transaction;
                return trans;
            }
            else {
                throw new Exception($"O Documento {TransDoc} não é um documento de stock");
            }
        }

        /// <summary>
        /// Save (insert or update) transaction
        /// </summary>
        public bool Save() {

            if (Validate()) {

                //Calculate document
                _bsoStockTransaction.Calculate(true, true, true);
                _bsoStockTransaction.SaveDocumentEx(true, false);

                editState = EditState.Editing;
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

            var transType = TransGetType(_bsoStockTransaction.Transaction.TransDocument);
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
                        return _bsoStockTransaction.Transaction.TransactionID;
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

        /// <summary>
        /// Initialize new Transaction
        /// </summary>
        public void Initialize(string TransDoc, string TransSerial) {

            _bsoStockTransaction = new BSOStockTransaction();

            _bsoStockTransaction.BSOStockTransactionDetail = new BSOItemTransactionDetail() {
                UserPermissions = systemSettings.User,
                PermissionsType = FrontOfficePermissionEnum.foPermByUser,
                TransactionType = TransGetType(TransDoc),
            };

            _bsoStockTransaction.InitNewTransaction(TransDoc, TransSerial);
            _bsoStockTransaction.Transaction.TransDocType = TransGetType(TransDoc);
        }

        /// <summary>
        /// Validate Transaction
        /// </summary>
        public bool Validate() {

            StringBuilder error = new StringBuilder();

            DSODocument dsoDocument = new DSODocument();

            if (editState == EditState.New && _bsoStockTransaction.Transaction.TransDocNumber==0) {
                if (dsoCache.StockTransactionProvider.TransactionCount(_bsoStockTransaction.Transaction.TransDocType) == 0) {
                    _bsoStockTransaction.Transaction.TransDocNumber = 1;
                }
                else {
                    _bsoStockTransaction.Transaction.TransDocNumber = dsoDocument.GetLastDocNumber(_bsoStockTransaction.Transaction.TransDocType, _bsoStockTransaction.Transaction.TransSerial, _bsoStockTransaction.Transaction.TransDocument, _bsoStockTransaction.Transaction.WorkstationStamp.WorkstationID) + 1;
                }
            }

            if (editState != EditState.New && !dsoCache.StockTransactionProvider.TransactionExists(_bsoStockTransaction.Transaction.TransSerial, _bsoStockTransaction.Transaction.TransDocument, _bsoStockTransaction.Transaction.TransDocNumber)) {
                throw new Exception($"O documento {_bsoStockTransaction.Transaction.TransDocument} {_bsoStockTransaction.Transaction.TransSerial}/{_bsoStockTransaction.Transaction.TransDocNumber} não existe para ser alterado. Deve criar um novo.");
            }
            else if (editState == EditState.New && dsoCache.StockTransactionProvider.TransactionExists(_bsoStockTransaction.Transaction.TransSerial, _bsoStockTransaction.Transaction.TransDocument, _bsoStockTransaction.Transaction.TransDocNumber)) {
                throw new Exception($"O documento {_bsoStockTransaction.Transaction.TransDocument} {_bsoStockTransaction.Transaction.TransSerial}/{_bsoStockTransaction.Transaction.TransDocNumber} já existe para ser alterado. Deve criar um novo.");
            }

            if (_bsoStockTransaction.Transaction == null) {
                if (editState == EditState.New) {
                    throw new Exception($"Não foi possível inicializar o documento [{_bsoStockTransaction.Transaction.TransDocument}] da série [{_bsoStockTransaction.Transaction.TransSerial}]");
                }
                else {
                    throw new Exception($"Não foi possível carregar o documento [{_bsoStockTransaction.Transaction.TransDocument}] da série [{_bsoStockTransaction.Transaction.TransSerial}] número [{_bsoStockTransaction.Transaction.TransDocNumber}]");
                }
            }
            else {
                if (!systemSettings.WorkstationInfo.Document.IsInCollection(_bsoStockTransaction.Transaction.TransDocument)) {
                    error.Append($"O documento [{_bsoStockTransaction.Transaction.TransDocument}] não existe ou não se encontra preenchido.");
                }
                var transType = TransGetType(_bsoStockTransaction.Transaction.TransDocument);
                if (transType != DocumentTypeEnum.dcTypeStock) {
                    error.Append($"O documento indicado [{_bsoStockTransaction.Transaction.TransDocument}] não é um documento de stock");
                }
                DocumentsSeries serie = null;
                if (systemSettings.DocumentSeries.IsInCollection(_bsoStockTransaction.Transaction.TransSerial)) {
                    serie = systemSettings.DocumentSeries[_bsoStockTransaction.Transaction.TransSerial];
                    if (serie.SeriesType != SeriesTypeEnum.SeriesExternal) {
                        error.Append("Apenas são permitidas séries externas.");
                    }
                }
                if (serie == null) {
                    error.Append("A série indicada não existe");
                }
                if (_bsoStockTransaction.Transaction.TransDocNumber == 0) {
                    error.AppendLine("O número de Documento não se encontra preenchido");
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

        /// <summary>
        /// Add stock details 
        /// </summary>
        public void AddDetailStock(double TaxRate, StockQtyRuleEnum StockQtyRule, ItemTransactionDetail Detail) {

            StockTransaction stockTransaction = _bsoStockTransaction.Transaction;

            Detail.BaseCurrency = stockTransaction.BaseCurrency;
            Detail.CreateDate = stockTransaction.CreateDate;
            Detail.ActualDeliveryDate = stockTransaction.ActualDeliveryDate;
            Detail.PartyTypeCode = stockTransaction.PartyTypeCode;
            Detail.PartyID = stockTransaction.PartyID;
            Detail.WarehouseOutgoing = Detail.WarehouseID;
            Detail.WarehouseReceipt = Detail.WarehouseID;
            Detail.PhysicalQtyRule = StockQtyRule;
            Detail.LineItemID = stockTransaction.Details.Count + 1;

            //Get item
            Item item = dsoCache.ItemProvider.GetItemForTransactionDetail(Detail.ItemID, Detail.BaseCurrency);

            if (item != null) {
                Detail.Description = item.Description;
                Detail.TaxableGroupID = item.TaxableGroupID;
                Detail.ItemType = item.ItemType;
                Detail.FamilyID = item.Family.FamilyID;
                Detail.UnitList = item.UnitList.Clone();

                Detail.WeightUnitOfMeasure = item.WeightUnitOfMeasure;
                Detail.WeightMeasure = item.WeightMeasure;
                Detail.Graduation = item.Graduation;
                Detail.ItemTax = item.ItemTax;
                Detail.ItemTax2 = item.ItemTax2;
                Detail.ItemTax3 = item.ItemTax3;
                Detail.ItemExtraInfo.ItemQuantityCalcFormula = item.ItemQuantityCalcFormula;

                string unitOfSaleId = Detail.UnitOfSaleID;
                if (item.UnitList.IsInCollection(unitOfSaleId)) {
                    Detail.UnitOfSaleID = unitOfSaleId;
                }
                else {
                    Detail.UnitOfSaleID = item.GetDefaultUnitForTransaction(DocumentTypeEnum.dcTypeStock);
                }
            }
            else if (Detail.ItemID == "=") {
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
                throw new Exception($"O Artigo [{Detail.ItemID}] não foi entrado.");
            }

            Detail.TaxableGroupID = dsoCache.TaxesProvider.GetTaxableGroupIDFromTaxRate(TaxRate, APIEngine.SystemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID,
                                                                                                           APIEngine.SystemSettings.SystemInfo.TaxRegionID);
            Detail.SetUnitOfSaleID(Detail.UnitOfSaleID);

            //Formulas
            double Quantity1 = 0;
            double Quantity2 = 0;
            double Quantity3 = 0;
            double Quantity4 = 0;

            bool blnHaveSetUnits = false;

            Detail.Quantity1 = Quantity1;
            Detail.Quantity2 = Quantity2;
            Detail.Quantity3 = Quantity3;
            Detail.Quantity4 = Quantity4;
            if (!blnHaveSetUnits) {
                if (!string.IsNullOrEmpty(Detail.ItemExtraInfo.ItemQuantityCalcFormula) && APIEngine.SystemSettings.SystemInfo.UseUnitWithFormulaItems) {
                    Detail.SetQuantity(StockHelper.CalculateQuantity(Detail.ItemExtraInfo.ItemQuantityCalcFormula, Detail, true));
                }
                else {
                    Detail.SetQuantity(StockHelper.CalculateQuantity(null, Detail, true));
                }
            }
            //    
            if (!blnHaveSetUnits) {
                Detail.SetQuantity(Detail.Quantity);
            }
            Detail.Description = item.Description;     // OR "Custom description"
            Detail.Comments = "Observações de linha: Gerada por" + Application.ProductName;

            //*** UnitPrice
            if (stockTransaction.TransactionTaxIncluded) {
                Detail.TaxIncludedPrice = Detail.UnitPrice;
            }

            MathFunctions mathUtil = new MathFunctions();

            if (Detail.DiscountPercent == 0 && (Detail.CumulativeDiscountPercent1 != 0 || Detail.CumulativeDiscountPercent2 != 0 || Detail.CumulativeDiscountPercent3 != 0)) {
                Detail.DiscountPercent = mathUtil.GetCumulativeDiscount(Detail.CumulativeDiscountPercent1, Detail.CumulativeDiscountPercent2, Detail.CumulativeDiscountPercent3);
            }

            if (Detail.DiscountPercent != 0 && (Detail.CumulativeDiscountPercent1 == 0 && Detail.CumulativeDiscountPercent2 == 0 && Detail.CumulativeDiscountPercent3 == 0)) {
                Detail.CumulativeDiscountPercent1 = Detail.DiscountPercent;
            }

            if (Detail.ItemProperties.HasPropertyValues) {
                dsoCache.ItemPropertyProvider.GetItemPropertyStock(Detail.ItemID, Detail.WarehouseID, Detail.ItemProperties);
            }

            bool calculate = true;
            item = null;
            _bsoStockTransaction.AddDetail(Detail, ref calculate);
            Detail = null;
        }

        public void SetPermissions() {
            _bsoStockTransaction.UserPermissions = systemSettings.User;
            _bsoStockTransaction.PermissionsType = FrontOfficePermissionEnum.foPermByUser;
        }

        public void SetPartyType(int selected) {
            _bsoStockTransaction.PartyType = (short)TransGetPartyType(selected);
        }

        public void SetBaseCurrency(string currency) {
            _bsoStockTransaction.BaseCurrency = currency;
        }
    }
}