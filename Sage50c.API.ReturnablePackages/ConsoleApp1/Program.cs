using S50cSys22;
using S50cSys22;
using Sage50c.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sage50c.API.ReturnablePackages.Sample {
    class Program {
        private S50cDL22.DSOItem itemProvider { get { return APIEngine.DSOCache.ItemProvider; } }

        static void Main(string[] args) {
            //Config. Connect to Database
            string ProductCode = "CRTL";
            string CompanyID = "EMPRESA";
            bool Isdebug = true;

            try {

                //Inicialize API
                APIEngine.Initialize(ProductCode, CompanyID, Isdebug);
            }
            catch (Exception ex) {
                Console.Error.Write(ex.Message);
            }

            if (APIEngine.APIInitialized) {

                //1º  stock entry
                //ES : 10 7UPs | 10 TARAS
                //StockTransactionSave();

                //2º Purchase
                //FS: 20 7UPs | 20 TARAS
                //ItemTransactionSavePurchase();

                //3º Sale
                //FS: 5 7UPs | 5 TARAS
                ItemTransactionSaveSale();

                //4º Sale sale with returnable return
                //FS: 5 7UPs | 5 TARAS
                //ItemTransactionSaveSaleWithReturnOfReturnable();


            }

            //Terminate API
            APIEngine.Terminate();

        }

        private static void StockTransactionSave() {
            string transDoc = "ES";
            string transSerial = "1";
            double partyID = 1;

            var Transaction = new S50cBL22.BSOStockTransaction();
            var currency = APIEngine.SystemSettings.BaseCurrency;
            var NewTransDocNumber = APIEngine.DSOCache.DocumentProvider.GetNewDocNumber(transSerial, transDoc);

//-------------------------------------------------------------
// *** Header
//-------------------------------------------------------------
            // Motor do documento
            Transaction.Transaction.TransDocType = S50cSys22.DocumentTypeEnum.dcTypeStock;
            Transaction.Transaction.TransDocNumber = NewTransDocNumber.DocNumber;
            Transaction.PermissionsType = S50cSys22.FrontOfficePermissionEnum.foPermByUser;

            //New Documento
            Transaction.InitNewTransaction(transDoc, transSerial);

            // Motor dos detalhes (linhas)
            var bsoStockTransDetail = new S50cBL22.BSOItemTransactionDetail();
            bsoStockTransDetail.PermissionsType = S50cSys22.FrontOfficePermissionEnum.foPermByUser;
            bsoStockTransDetail.TransactionType = Transaction.Transaction.TransDocType;
            bsoStockTransDetail.UserPermissions = APIEngine.SystemSettings.User;
            Transaction.BSOStockTransactionDetail = bsoStockTransDetail;
            //

            Transaction.PartyID = partyID;
            Transaction.TransactionTaxIncluded = true;
            Transaction.createDate = DateTime.Now;
            Transaction.CreateTime = new DateTime(DateTime.Now.TimeOfDay.Ticks);
            Transaction.ActualDeliveryDate = DateTime.Now;
            Transaction.PartyType = (short)S50cSys22.PartyTypeEnum.ptNothing; //igual a documento ja criado
            Transaction.BaseCurrency = currency.CurrencyID;
            Transaction.BaseCurrencyExchange = currency.BuyExchange;
            Transaction.Transaction.Comments = "Returnable Packaging";
//-------------------------------------------------------------
// *** Header
//-------------------------------------------------------------


//-------------------------------------------------------------
// *** DETAILS
//-------------------------------------------------------------
            if (Transaction.Transaction.Details == null) {
                Transaction.Transaction.Details = new S50cBO22.ItemTransactionDetailList();
            }

            //Add Linha 1 : 10 7upLitro
            var StockTransactionDetail = AddStockDetail(Transaction,1 , "7upLitro", 10);
            Transaction.AddDetail(StockTransactionDetail, false);

            //Add Linha 2 : 10 TARAS
            StockTransactionDetail = AddStockDetail(Transaction, 1, "TARAS", 10);
            Transaction.AddDetail(StockTransactionDetail, false);

//-------------------------------------------------------------
// *** DETAILS
//-------------------------------------------------------------

            //*** SAVE DOCUMENT
            if (Transaction.Transaction.Details.Count > 0) {
                try {
                    Transaction.Calculate(true, true, true);
                    Transaction.SaveDocumentEx(true, false);
                    //Console.Error.Write(Transaction.Transaction.TransactionID.ToString());
                }
                catch (Exception ex) {
                    Console.Error.Write(ex.Message);
                }
            }
        }

        private static void ItemTransactionSavePurchase() {
            string transDoc = "FCO";
            string transSerial = "1";
            double PartyID = 1;

            var currency = APIEngine.SystemSettings.BaseCurrency;
            var ItemTransaction = new S50cBL22.BSOItemTransaction();
            DocumentTypeEnum DocType = DocumentTypeEnum.dcTypePurchase;
            var NewTransDocNumber = APIEngine.DSOCache.DocumentProvider.GetNewDocNumber(transSerial, transDoc);


//-------------------------------------------------------------
// *** Header
//-------------------------------------------------------------
            // Motor do documento
            ItemTransaction.Transaction.TransDocType = DocType;
            ItemTransaction.Transaction.TransDocNumber = NewTransDocNumber.DocNumber;
            ItemTransaction.PermissionsType = FrontOfficePermissionEnum.foPermByUser;
            ItemTransaction.InitNewTransaction(transDoc, transSerial);
            // Motor dos detalhes (linhas)
            var bsoItemTransDetail = new S50cBL22.BSOItemTransactionDetail();
            bsoItemTransDetail.PermissionsType = FrontOfficePermissionEnum.foPermByUser;
            bsoItemTransDetail.TransactionType = ItemTransaction.Transaction.TransDocType;
            bsoItemTransDetail.UserPermissions = APIEngine.SystemSettings.User;
            ItemTransaction.BSOItemTransactionDetail = bsoItemTransDetail;
            //

            ItemTransaction.PartyID = PartyID;
            ItemTransaction.TransactionTaxIncluded = true;
            ItemTransaction.createDate = DateTime.Now;
            ItemTransaction.CreateTime = new DateTime(DateTime.Now.TimeOfDay.Ticks);
            ItemTransaction.ActualDeliveryDate = DateTime.Now;
            ItemTransaction.BaseCurrency = currency.CurrencyID;
            ItemTransaction.BaseCurrencyExchange = currency.BuyExchange;
            ItemTransaction.Transaction.Comments = "Returnable Packaging";   
//-------------------------------------------------------------
// *** Header
//-------------------------------------------------------------


//-------------------------------------------------------------
// *** DETAILS
//-------------------------------------------------------------
            if (ItemTransaction.Transaction.Details == null) {
                ItemTransaction.Transaction.Details = new S50cBO22.ItemTransactionDetailList();
            }

            //Add Line_1: art1 type normal
            var StockTransactionDetail = AddItemDetail(ItemTransaction, 1, "7upLitro", 20,2);
            ItemTransaction.AddDetail(StockTransactionDetail, true);

            ////Add Line_2: EmbRetorn Type "Embalagens retornáveis" 
            StockTransactionDetail = AddItemDetail(ItemTransaction, 1, "TARAS", 20,0.30);
            ItemTransaction.AddDetail(StockTransactionDetail, true);
//-------------------------------------------------------------
// *** DETAILS
//-------------------------------------------------------------


//-------------------------------------------------------------
// *** SAVE DOCUMENT
//-------------------------------------------------------------

            //*** SAVE only if document have Lines
            if (ItemTransaction.Transaction.Details.Count > 0) {
                try {
                    //Ensure Till is open
                    ItemTransaction.EnsureOpenTill(ItemTransaction.Transaction.Till.TillID);
                    //Calculate 
                    ItemTransaction.Calculate(true, true);
                    //Save Document
                    ItemTransaction.SaveDocument(false, false);
                    //Console.Error.Write(ItemTransaction.Transaction.TransactionID.ToString());
                }
                catch (Exception ex) {
                    Console.Error.Write(ex.Message);
                }
            }
//-------------------------------------------------------------
// *** SAVE DOCUMENT
//-------------------------------------------------------------
        }

        private static void ItemTransactionSaveSale() {
            string transDoc = "FS";
            string transSerial = "1";
            double PartyID = 1;

            var currency = APIEngine.SystemSettings.BaseCurrency;
            var ItemTransaction = new S50cBL22.BSOItemTransaction();
            DocumentTypeEnum DocType = DocumentTypeEnum.dcTypeSale;
            var NewTransDocNumber = APIEngine.DSOCache.DocumentProvider.GetNewDocNumber(transSerial, transDoc);

//-------------------------------------------------------------
// *** Header
//-------------------------------------------------------------
            // Motor do documento
            ItemTransaction.Transaction.TransDocType = DocType;
            ItemTransaction.Transaction.TransDocNumber = NewTransDocNumber.DocNumber;
            ItemTransaction.PermissionsType = FrontOfficePermissionEnum.foPermByUser;
            ItemTransaction.InitNewTransaction(transDoc, transSerial);
            // Motor dos detalhes (linhas)
            var bsoItemTransDetail = new S50cBL22.BSOItemTransactionDetail();
            bsoItemTransDetail.PermissionsType = FrontOfficePermissionEnum.foPermByUser;
            bsoItemTransDetail.TransactionType = ItemTransaction.Transaction.TransDocType;
            bsoItemTransDetail.UserPermissions = APIEngine.SystemSettings.User;
            ItemTransaction.BSOItemTransactionDetail = bsoItemTransDetail;
            //

            ItemTransaction.PartyID = PartyID;
            ItemTransaction.TransactionTaxIncluded = true;
            ItemTransaction.createDate = DateTime.Now;
            ItemTransaction.CreateTime = new DateTime(DateTime.Now.TimeOfDay.Ticks);
            ItemTransaction.ActualDeliveryDate = DateTime.Now;
            ItemTransaction.BaseCurrency = currency.CurrencyID;
            ItemTransaction.BaseCurrencyExchange = currency.BuyExchange;
            ItemTransaction.Transaction.Comments = "Returnable Packaging";
//-------------------------------------------------------------
// *** Header
//-------------------------------------------------------------


//-------------------------------------------------------------
// *** DETAILS
//-------------------------------------------------------------
            if (ItemTransaction.Transaction.Details == null) {
                ItemTransaction.Transaction.Details = new S50cBO22.ItemTransactionDetailList();
            }

            //Add Line_1: art1 type normal
            var transDetail = AddItemDetail(ItemTransaction, 1, "1", 5, 2.71);
            ItemTransaction.AddDetail(transDetail, true);

            ////Add Line_2: EmbRetorn Type "Embalagens retornáveis" 
            transDetail = AddItemDetail(ItemTransaction, 1, "TARAS", 5, 0.30);
            ItemTransaction.AddDetail(transDetail, true);
//-------------------------------------------------------------
// *** DETAILS
//-------------------------------------------------------------

//-------------------------------------------------------------
// *** SAVE DOCUMENT
//-------------------------------------------------------------
            //*** SAVE only if document have Lines
            if (ItemTransaction.Transaction.Details.Count > 0) {
                try {
                    //Ensure Till is open
                    ItemTransaction.EnsureOpenTill(ItemTransaction.Transaction.Till.TillID);
                    //Calculate 
                    ItemTransaction.Calculate(true, true);
                    //Save Document
                    ItemTransaction.SaveDocument(false, false);
                    //Console.Error.Write(ItemTransaction.Transaction.TransactionID.ToString());
                }
                catch (Exception ex) {
                    Console.Error.Write(ex.Message);
                }
            }
//-------------------------------------------------------------
// *** SAVE DOCUMENT
//-------------------------------------------------------------

        }

        private static void ItemTransactionSaveSaleWithReturnOfReturnable() {
            string transDoc = "FS";
            string transSerial = "1";
            double PartyID = 1;

            var currency = APIEngine.SystemSettings.BaseCurrency;
            var ItemTransaction = new S50cBL22.BSOItemTransaction();
            DocumentTypeEnum DocType = DocumentTypeEnum.dcTypeSale;
            var NewTransDocNumber = APIEngine.DSOCache.DocumentProvider.GetNewDocNumber(transSerial, transDoc);

//-------------------------------------------------------------
// *** Header
//-------------------------------------------------------------
            // Motor do documento
            ItemTransaction.Transaction.TransDocType = DocType;
            ItemTransaction.Transaction.TransDocNumber = NewTransDocNumber.DocNumber;
            ItemTransaction.PermissionsType = FrontOfficePermissionEnum.foPermByUser;
            ItemTransaction.InitNewTransaction(transDoc, transSerial);
            // Motor dos detalhes (linhas)
            var bsoItemTransDetail = new S50cBL22.BSOItemTransactionDetail();
            bsoItemTransDetail.PermissionsType = FrontOfficePermissionEnum.foPermByUser;
            bsoItemTransDetail.TransactionType = ItemTransaction.Transaction.TransDocType;
            bsoItemTransDetail.UserPermissions = APIEngine.SystemSettings.User;
            ItemTransaction.BSOItemTransactionDetail = bsoItemTransDetail;
            //

            ItemTransaction.PartyID = PartyID;
            ItemTransaction.TransactionTaxIncluded = true;
            ItemTransaction.createDate = DateTime.Now;
            ItemTransaction.CreateTime = new DateTime(DateTime.Now.TimeOfDay.Ticks);
            ItemTransaction.ActualDeliveryDate = DateTime.Now;
            ItemTransaction.BaseCurrency = currency.CurrencyID;
            ItemTransaction.BaseCurrencyExchange = currency.BuyExchange;
            ItemTransaction.Transaction.Comments = "Returnable Packaging";
            //-------------------------------------------------------------
            // *** Header
            //-------------------------------------------------------------


            //-------------------------------------------------------------
            // *** DETAILS
            //-------------------------------------------------------------
            if (ItemTransaction.Transaction.Details == null) {
                ItemTransaction.Transaction.Details = new S50cBO22.ItemTransactionDetailList();
            }

            //Add Line_1: art1 type normal
            var StockTransactionDetail = AddItemDetail(ItemTransaction, 1, "Bacalhau", 1, 33);
            ItemTransaction.AddDetail(StockTransactionDetail, true);

            ////Add Line_2: Enter customer returnables ,Customer input value is negative
            StockTransactionDetail = AddItemDetail(ItemTransaction, 1, "TARAS", -3, 0.30);
            ItemTransaction.AddDetail(StockTransactionDetail, true);


//-------------------------------------------------------------
// *** DETAILS
//-------------------------------------------------------------

//-------------------------------------------------------------
// *** SAVE DOCUMENT
//-------------------------------------------------------------
            //*** SAVE only if document have Lines
            if (ItemTransaction.Transaction.Details.Count > 0) {
                try {
                    //Ensure Till is open
                    ItemTransaction.EnsureOpenTill(ItemTransaction.Transaction.Till.TillID);
                    //Calculate 
                    ItemTransaction.Calculate(true, true);
                    //Save Document
                    ItemTransaction.SaveDocument(false, false);
                    //Console.Error.Write(ItemTransaction.Transaction.TransactionID.ToString());
                }
                catch (Exception ex) {
                    Console.Error.Write(ex.Message);
                }
            }
//-------------------------------------------------------------
// *** SAVE DOCUMENT
//-------------------------------------------------------------

        }

        internal static S50cBO22.ItemTransactionDetail AddStockDetail(S50cBL22.BSOStockTransaction StockTransaction, short warehouseId, string itemId, double Quantity) {
            double unitPrice = APIEngine.DSOCache.ItemProvider.GetItemAverageCostPrice(itemId, StockTransaction.Transaction.BaseCurrency);

            var transDetail = new S50cBO22.ItemTransactionDetail();
            transDetail.LineItemID = StockTransaction.Transaction.Details.Count + 1;

            transDetail.BaseCurrency = StockTransaction.Transaction.BaseCurrency;
            transDetail.CreateDate = StockTransaction.Transaction.CreateDate;
            transDetail.ActualDeliveryDate = StockTransaction.Transaction.ActualDeliveryDate;
            transDetail.PartyTypeCode = StockTransaction.Transaction.PartyTypeCode;
            transDetail.PartyID = StockTransaction.Transaction.PartyID;
            transDetail.WarehouseID = warehouseId;
            transDetail.WarehouseOutgoing = transDetail.WarehouseID;
            transDetail.WarehouseReceipt = transDetail.WarehouseID;
            transDetail.PhysicalQtyRule = StockQtyRuleEnum.stkQtyNone;

            var item = APIEngine.DSOCache.ItemProvider.GetItemForTransactionDetail(itemId, transDetail.BaseCurrency, "", true);

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

                transDetail.PhysicalQtyRule = S50cSys22.StockQtyRuleEnum.stkQtyReceipt;

                transDetail.ProductCategory = item.ProductCategory;
                transDetail.ItemExtraInfo.ItemQuantityCalcFormula = item.ItemQuantityCalcFormula;

                if (item.UnitList.IsInCollection(transDetail.UnitOfSaleID)) {
                    transDetail.UnitOfSaleID = transDetail.UnitOfSaleID;
                }
                else {
                    transDetail.UnitOfSaleID = item.GetDefaultUnitForTransaction(StockTransaction.Transaction.TransDocType);
                }
            }

            transDetail.SetUnitOfSaleID(transDetail.UnitOfSaleID);
            transDetail.SetQuantity(Quantity);
            transDetail.QntyAvailableBalanceCount = Quantity;
            //transDetail.Quantity1 = 0;
            //transDetail.Quantity2 = 0;
            //transDetail.Quantity3 = 0;
            //transDetail.Quantity4 = 0;


            if (StockTransaction.TransactionTaxIncluded) {
                transDetail.TaxIncludedPrice = unitPrice;
                transDetail.UnitPrice = APIEngine.DSOCache.TaxesProvider.GetItemNetPrice(
                                                  transDetail.TaxIncludedPrice,
                                                   transDetail.TaxableGroupID,
                                                   APIEngine.SystemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID,
                                                  APIEngine.SystemSettings.SystemInfo.TaxRegionID);
            }
            else {
                transDetail.UnitPrice = unitPrice;
                transDetail.TaxIncludedPrice = APIEngine.DSOCache.TaxesProvider.GetItemTaxIncludePrice(
                                                 transDetail.UnitPrice,
                                                   transDetail.TaxableGroupID,
                                                   APIEngine.SystemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID,
                                                  APIEngine.SystemSettings.SystemInfo.TaxRegionID);
            }

            S50cUtil22.MathFunctions mathUtil = new S50cUtil22.MathFunctions();

            if (transDetail.DiscountPercent == 0 && (transDetail.CumulativeDiscountPercent1 != 0 || transDetail.CumulativeDiscountPercent2 != 0 || transDetail.CumulativeDiscountPercent3 != 0)) {
                transDetail.DiscountPercent = mathUtil.GetCumulativeDiscount(transDetail.CumulativeDiscountPercent1, transDetail.CumulativeDiscountPercent2, transDetail.CumulativeDiscountPercent3);
            }

            if (transDetail.DiscountPercent != 0 && (transDetail.CumulativeDiscountPercent1 == 0 && transDetail.CumulativeDiscountPercent2 == 0 && transDetail.CumulativeDiscountPercent3 == 0)) {
                transDetail.CumulativeDiscountPercent1 = transDetail.DiscountPercent;
            }


            item = null;

            return transDetail;
        }

        internal static S50cBO22.ItemTransactionDetail AddItemDetail(S50cBL22.BSOItemTransaction Transaction, short warehouseId, string itemId, double Quantity, double Price) {
            var transDetail = new S50cBO22.ItemTransactionDetail();

            transDetail.LineItemID = Transaction.Transaction.Details.Count + 1;

            transDetail.BaseCurrency = Transaction.Transaction.BaseCurrency;
            transDetail.CreateDate = Transaction.Transaction.CreateDate;
            transDetail.ActualDeliveryDate = Transaction.Transaction.ActualDeliveryDate;
            transDetail.PartyTypeCode = Transaction.Transaction.PartyTypeCode;
            transDetail.PartyID = Transaction.Transaction.PartyID;
            transDetail.WarehouseID = warehouseId;
            transDetail.WarehouseOutgoing = transDetail.WarehouseID;
            transDetail.WarehouseReceipt = transDetail.WarehouseID;
            transDetail.PhysicalQtyRule = StockQtyRuleEnum.stkQtyNone;

            var item = APIEngine.DSOCache.ItemProvider.GetItemForTransactionDetail(itemId, transDetail.BaseCurrency, "", true);

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
                transDetail.UnitPrice = Price;

                if (Transaction.Transaction.TransDocType == DocumentTypeEnum.dcTypeSale) {
                    transDetail.PhysicalQtyRule = StockQtyRuleEnum.stkQtyOutgoing;
                }
                else {
                    transDetail.PhysicalQtyRule = StockQtyRuleEnum.stkQtyReceipt;
                }

                transDetail.ProductCategory = item.ProductCategory;
                transDetail.ItemExtraInfo.ItemQuantityCalcFormula = item.ItemQuantityCalcFormula;

                if (item.UnitList.IsInCollection(transDetail.UnitOfSaleID)) {
                    transDetail.UnitOfSaleID = transDetail.UnitOfSaleID;
                }
                else {
                    transDetail.UnitOfSaleID = item.GetDefaultUnitForTransaction(Transaction.Transaction.TransDocType);
                }
            }

            transDetail.SetUnitOfSaleID(transDetail.UnitOfSaleID);
            transDetail.SetQuantity(Quantity);
            transDetail.QntyAvailableBalanceCount = Quantity;


            if (Transaction.TransactionTaxIncluded) {
                transDetail.TaxIncludedPrice = Price;
                transDetail.UnitPrice = APIEngine.DSOCache.TaxesProvider.GetItemNetPrice(
                                                  transDetail.TaxIncludedPrice,
                                                   transDetail.TaxableGroupID,
                                                   APIEngine.SystemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID,
                                                  APIEngine.SystemSettings.SystemInfo.TaxRegionID);
            }
            else {
                transDetail.UnitPrice = Price; 
                transDetail.TaxIncludedPrice = APIEngine.DSOCache.TaxesProvider.GetItemTaxIncludePrice(
                                                 transDetail.UnitPrice,
                                                   transDetail.TaxableGroupID,
                                                   APIEngine.SystemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID,
                                                  APIEngine.SystemSettings.SystemInfo.TaxRegionID);
            }
            return transDetail;
        }

        internal static double CalculateQuantity(string strFormula, S50cBO22.ItemTransactionDetail TransactionDetail, bool UseQuantityFactor) {
            S50cUtil22.MathFunctions mathUtil = new S50cUtil22.MathFunctions();
            UnitOfMeasure oUnit;
            S50cBL22.BSOExpressionParser objBSOExpressionParser = new S50cBL22.BSOExpressionParser();
            double result = 0;

            if (!string.IsNullOrEmpty(strFormula)) {
                result = 0;
                string tempres = objBSOExpressionParser.ParseFormula(strFormula, TransactionDetail);
                double.TryParse(tempres, out result);
            }
            else {
                result = TransactionDetail.Quantity;
            }


            if (UseQuantityFactor) {
                if (TransactionDetail.QuantityFactor != 1 && TransactionDetail.QuantityFactor != 0)
                    result = result / TransactionDetail.QuantityFactor;
            }

            oUnit = APIEngine.DSOCache.UnitOfMeasureProvider.GetUnitOfMeasure(TransactionDetail.UnitOfSaleID);

            if (oUnit != null) {
                result = mathUtil.MyRoundEx(result, oUnit.MaximumDecimals);
            }

            oUnit = null;
            objBSOExpressionParser = null;
            mathUtil = null;

            return result;
        }

    }
}
