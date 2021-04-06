using S50cBL18;
using S50cSys18;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Sage50c.API;

namespace Sage50c.API.PromotionPrice {
    internal class APITransactionHelper {
        public static double CheckPrice(string ItemID) {
            double price = 0;
            APIEngine.SystemSettings.StartUpInfo.CacheDiscountPlan = false;

            // Simulate by creating a "dummy" transaction

            var bsoItemTransactionDetail = new BSOItemTransactionDetail();
            var bsoItemTransaction = new BSOItemTransaction();

            if (!string.IsNullOrEmpty(APIEngine.SystemSettings.QuickSearchDefaults.DefaultTransDocumentID)) {
                bsoItemTransaction.TransactionType = DocumentTypeEnum.dcTypeSale;
                bsoItemTransaction.InitNewTransaction(APIEngine.SystemSettings.QuickSearchDefaults.DefaultTransDocumentID,
                                                      APIEngine.SystemSettings.WorkstationInfo.DefaultTransSerial,
                                                      false, false, true, false);
                bsoItemTransaction.TransactionTaxIncluded = true;
            }

            bsoItemTransactionDetail.InitNewTransaction();
            var defaultDoc = APIEngine.SystemSettings.QuickSearchDefaults.DefaultTransDocumentID;
            if (APIEngine.SystemSettings.WorkstationInfo.Document.IsInCollection(defaultDoc)) {
                var document = APIEngine.SystemSettings.WorkstationInfo.Document[defaultDoc];
                bsoItemTransactionDetail.TransactionDocument = document;
                bsoItemTransactionDetail.UserPermissions = APIEngine.SystemSettings.User;
                bsoItemTransactionDetail.BaseCurrency = APIEngine.SystemSettings.BaseCurrency;
                bsoItemTransactionDetail.TransactionDetail.BaseCurrency = APIEngine.SystemSettings.BaseCurrency;
                bsoItemTransactionDetail.createDate = DateTime.Now.Date;
                bsoItemTransactionDetail.CreateTime = new DateTime(DateTime.Now.TimeOfDay.Ticks);
                bsoItemTransactionDetail.SetTransactionTaxIncluded(true);
                bsoItemTransactionDetail.TransactionType = DocumentTypeEnum.dcTypeSale;
                bsoItemTransactionDetail.Reset();
                bsoItemTransactionDetail.BaseCurrency = APIEngine.SystemSettings.BaseCurrency;

                // Ensure it's empty and with defaults
                bsoItemTransaction.AbortTransaction();

                // Set the desired priceline (may not be necessary)
                bsoItemTransactionDetail.PartyPriceLineID = 1;
                //
                // Call HandleItemDetail to fill the detail with default information for the Item
                if (bsoItemTransactionDetail.HandleItemDetail(ItemID, TransDocFieldIDEnum.fldItemID, true)) {
                    if (!string.IsNullOrEmpty(bsoItemTransactionDetail.TransactionDetail.ItemID)) {
                        // Add the detail and calculate the line
                        // Expect warning messages in the DataManager Events
                        bsoItemTransaction.AddDetail(bsoItemTransactionDetail.TransactionDetail);
                        // Ensurea all promotions are applied
                        APIEngine.DiscountManager.ApplyTransactionDiscounts(bsoItemTransactionDetail);      //Aplica Mix-and-Match e/ou descontos que só são aplicados com F10
                                                                                                            // Get your price
                        price = bsoItemTransactionDetail.TransactionDetail.TotalTaxIncludedAmount;
                    }
                }
            }
            return price;
        }
    }
}
