﻿using System;
using System.Windows.Forms;

using S50cSys22;
using S50cUtil22;

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

        private static bool _sizeIsFindind = false;
        /// <summary>
        /// Creates a quick search and returns the id of a size selected by the user
        /// </summary>
        /// <returns>The id of a size or 0 if cancelled</returns>
        internal static long SizeFind() {
            QuickSearch quickSearch = null;
            long result = 0;

            try {
                if (!_sizeIsFindind) {
                    _sizeIsFindind = true;
                    quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_Sizes, false);

                    if (quickSearch.SelectValue()) {
                        result = quickSearch.ValueSelectedLong();
                    }
                    _sizeIsFindind = false;
                }
            }
            catch (Exception ex) {
                _sizeIsFindind = false;
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
            finally {

            }
            quickSearch = null;

            return result;
        }

        private static bool _unitOfMeasureIsFindind = false;
        /// <summary>
        /// Creates a quick search and returns the id of a unit of measure selected by the user
        /// </summary>
        /// <returns>The id of a unit of measure or 0 if cancelled</returns>
        internal static string UnitOfMeasureFind() {
            QuickSearch quickSearch = null;
            string result = null;

            try {
                if (!_unitOfMeasureIsFindind) {
                    _unitOfMeasureIsFindind = true;
                    quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_UnitOfMeasure, false);

                    if (quickSearch.SelectValue()) {
                        result = quickSearch.ValueSelectedString();
                    }
                    _unitOfMeasureIsFindind = false;
                }
            }
            catch (Exception ex) {
                _unitOfMeasureIsFindind = false;
                APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
            }
            finally {
            }
            quickSearch = null;

            return result;
        }

        private static bool _salesmanIsFinding = false;
        internal static double SalesmanFind() {
            QuickSearch quickSearch = null;
            double salesmanId = 0;

            try {
                //show data for view with id=0: the title is fetched by the
                //quick search viewer.
                if (!_salesmanIsFinding) {
                    _salesmanIsFinding = true;

                    quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_Salesman, false);

                    if (quickSearch.SelectValue()) {
                        salesmanId = quickSearch.ValueSelectedDouble();
                    }
                    else {
                        //Not found... do nothing
                    }
                    _salesmanIsFinding = false;
                    quickSearch = null;
                }
            }
            catch (Exception ex) {
                _supplierIsFinding = false;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally {
            }

            return salesmanId;
        }

        private static bool _zoneIsFinding = false;
        internal static double ZoneFind() {
            QuickSearch quickSearch = null;
            double zoneId = 0;

            try {
                //show data for view with id=0: the title is fetched by the
                //quick search viewer.
                if (!_zoneIsFinding) {
                    _zoneIsFinding = true;

                    quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_Zone, false);

                    if (quickSearch.SelectValue()) {
                        zoneId = quickSearch.ValueSelectedDouble();
                    }
                    else {
                        //Not found... do nothing
                    }
                    _zoneIsFinding = false;
                    quickSearch = null;
                }
            }
            catch (Exception ex) {
                _zoneIsFinding = false;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally {
            }

            return zoneId;
        }
    
        private static bool _tenderIsFinding = false;
        internal static double TenderFind() {
            QuickSearch quickSearch = null;
            double tenderId = 0;

            try {
                //show data for view with id=0: the title is fetched by the
                //quick search viewer.
                if (!_tenderIsFinding) {
                    _tenderIsFinding = true;

                    quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_TenderNames, false);

                    if (quickSearch.SelectValue()) {
                        tenderId = quickSearch.ValueSelectedDouble();
                    }
                    else {
                        //Not found... do nothing
                    }
                    _tenderIsFinding = false;
                    quickSearch = null;
                }
            }
            catch (Exception ex) {
                _tenderIsFinding = false;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally {
            }

            return tenderId;
        }

        private static bool _paymentIsFinding = false;
        internal static double PaymentFind() {
            QuickSearch quickSearch = null;
            double paymentId = 0;

            try {
                //show data for view with id=0: the title is fetched by the
                //quick search viewer.
                if (!_paymentIsFinding) {
                    _paymentIsFinding = true;

                    quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_Payment, false);

                    if (quickSearch.SelectValue()) {
                        paymentId = quickSearch.ValueSelectedDouble();
                    }
                    else {
                        //Not found... do nothing
                    }
                    _paymentIsFinding = false;
                    quickSearch = null;
                }
            }
            catch (Exception ex) {
                _paymentIsFinding = false;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally {
            }

            return paymentId;
        }

    }
}
