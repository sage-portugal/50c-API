using S50cSys22;
using S50cUtil22;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sage50c.API.Sample {
    internal class QuickSearchHelper {
        private static bool itemIsFindind = false;
        internal static string ItemFind() {
            QuickSearch quickSearch = null;
            string result = null;

            try {
                if (!itemIsFindind) {
                    itemIsFindind = true;
                    quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_Item, APIEngine.SystemSettings.StartUpInfo.CacheQuickSearchItem);
                    clsCollection qsParams = new clsCollection();
                    qsParams.add(APIEngine.SystemSettings.QuickSearchDefaults.WarehouseID, "@WarehouseID");
                    qsParams.add(APIEngine.SystemSettings.QuickSearchDefaults.PriceLineID, "@PriceLineID");
                    qsParams.add(APIEngine.SystemSettings.QuickSearchDefaults.LanguageID, "@LanguageID");
                    qsParams.add(APIEngine.SystemSettings.QuickSearchDefaults.DisplayDiscontinued, "@Discontinued");
                    if (APIEngine.SystemSettings.StartUpInfo.UseItemSearchAlterCurrency) {
                        qsParams.add(APIEngine.SystemSettings.AlternativeCurrency.SaleExchange, "@ctxBaseCurrency");
                    }
                    else {
                        qsParams.add(APIEngine.SystemSettings.QuickSearchDefaults.EuroConversionRate, "@ctxBaseCurrency");
                    }
                    quickSearch.Parameters = qsParams;

                    if (quickSearch.SelectValue()) {
                        result = quickSearch.ValueSelectedString();
                    }
                    itemIsFindind = false;
                }
            }
            catch (Exception ex) {
                itemIsFindind = false;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally {

            }
            quickSearch = null;

            return result;
        }



        private static bool _customerIsFinding = false;
        internal static double CustomerFind() {
            QuickSearch quickSearch = null;
            double customerId = 0;

            try {
                //show data for view with id=0: the title is fetched by the
                //quick search viewer.
                if (!_customerIsFinding) {
                    _customerIsFinding = true;

                    quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_Customer, false);

                    if (quickSearch.SelectValue()) {
                        customerId = quickSearch.ValueSelectedDouble();
                        //numCustomerId.Value = (decimal)customerId;
                        //CustomerGet(customerId);
                    }
                    else {
                        //Not found... do nothing
                    }
                    _customerIsFinding = false;
                    quickSearch = null;
                }
            }
            catch (Exception ex) {
                _customerIsFinding = false;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally {
            }

            return customerId;
        }


        private static bool _supplierIsFinding = false;
        internal static double SupplierFind() {
            QuickSearch quickSearch = null;
            double supplierId = 0;

            try {
                //show data for view with id=0: the title is fetched by the
                //quick search viewer.
                if (!_supplierIsFinding) {
                    _supplierIsFinding = true;

                    quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_Supplier, false);

                    if (quickSearch.SelectValue()) {
                        supplierId = quickSearch.ValueSelectedDouble();
                    }
                    else {
                        //Not found... do nothing
                    }
                    _supplierIsFinding = false;
                    quickSearch = null;
                }
            }
            catch (Exception ex) {
                _supplierIsFinding = false;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally {
            }

            return supplierId;
        }




        private static bool _itemTransIsFindind = false;

        /// <summary>
        /// Creates a quick search and return the document number selected by the user
        /// </summary>
        /// <param name="TransSerial"></param>
        /// <param name="TransDocument"></param>
        /// <returns>Number of selected document or 0 if cancelled</returns>
        internal static double ItemTransactionFind(string TransSerial, string TransDocument) {
            QuickSearch quickSearch = null;
            double transDocNumber = 0;

            try {
                if (!_itemTransIsFindind) {
                    _itemTransIsFindind = true;

                    var doc = APIEngine.SystemSettings.WorkstationInfo.Document[TransDocument];
                    switch (doc.TransDocType) {
                        case DocumentTypeEnum.dcTypeSale:
                            quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_SaleTransaction, false);
                            break;

                        case DocumentTypeEnum.dcTypePurchase:
                            quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_BuyTransaction, false);
                            break;

                        default:
                            throw new Exception($"Document {TransDocument} not supported.");
                    }
                    // Create without cache
                    clsCollection qsParams = new clsCollection();
                    qsParams.add(TransSerial, "@TransSerial");
                    qsParams.add(TransDocument, "@TransDocument");
                    quickSearch.Parameters = qsParams;

                    if (quickSearch.SelectValue()) {
                        transDocNumber = quickSearch.ValueSelectedDouble();
                    }
                    _itemTransIsFindind = false;
                }
            }
            catch (Exception ex) {
                _itemTransIsFindind = false;
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, APIEngine.SystemSettings.Application.LongName);
            }
            finally {

            }
            quickSearch = null;

            return transDocNumber;
        }



        private static bool _stockTransIsFindind = false;

        /// <summary>
        /// Creates a quick search and return the document number selected by the user
        /// </summary>
        /// <param name="TransSerial"></param>
        /// <param name="TransDocument"></param>
        /// <returns>Number of selected document or 0 if cancelled</returns>
        internal static double StockTransactionFind(string TransSerial, string TransDocument) {
            QuickSearch quickSearch = null;
            double transDocNumber = 0;

            try {
                if (!_stockTransIsFindind) {
                    _stockTransIsFindind = true;

                    // Create without cache
                    quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_StockTransaction, false);
                    clsCollection qsParams = new clsCollection();
                    qsParams.add(TransSerial, "@TransSerial");
                    qsParams.add(TransDocument, "@TransDocument");
                    quickSearch.Parameters = qsParams;

                    if (quickSearch.SelectValue()) {
                        transDocNumber = quickSearch.ValueSelectedDouble();
                    }
                    _stockTransIsFindind = false;
                }
            }
            catch (Exception ex) {
                _stockTransIsFindind = false;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally {

            }
            quickSearch = null;

            return transDocNumber;
        }



        private static bool _accountTransIsFindind = false;
        /// <summary>
        /// Creates a quick search and return the document number selected by the user
        /// </summary>
        /// <param name="TransSerial"></param>
        /// <param name="TransDocument"></param>
        /// <returns>Number of selected document or 0 if cancelled</returns>
        internal static double AccountTransactionFind(string TransSerial, string TransDocument) {
            QuickSearch quickSearch = null;
            double transDocNumber = 0;

            try {
                if (!_accountTransIsFindind) {
                    _accountTransIsFindind = true;

                    // Create without cache
                    quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_AccountTransaction, false);
                    clsCollection qsParams = new clsCollection();
                    qsParams.add(TransDocument, "@TransDocumentID");
                    qsParams.add(TransSerial, "@TransSerial");
                    quickSearch.Parameters = qsParams;

                    if (quickSearch.SelectValue()) {
                        transDocNumber = quickSearch.ValueSelectedDouble();
                    }
                    _accountTransIsFindind = false;
                }
            }
            catch (Exception ex) {
                _accountTransIsFindind = false;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally {

            }
            quickSearch = null;

            return transDocNumber;
        }

        private static bool _colorIsFindind = false;
        /// <summary>
        /// Creates a quick search and returns the id of a color selected by the user
        /// </summary>
        /// <returns>The id of a color or 0 if cancelled</returns>
        internal static long ColorFind() {
            QuickSearch quickSearch = null;
            long result = 0;

            try {
                if (!_colorIsFindind) {
                    _colorIsFindind = true;
                    quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_Color, false);

                    if (quickSearch.SelectValue()) {
                        result = quickSearch.ValueSelectedLong();
                    }
                    _colorIsFindind = false;
                }
            }
            catch (Exception ex) {
                _colorIsFindind = false;
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
            finally {
            }
            quickSearch = null;

            return result;
        }
    }
}
