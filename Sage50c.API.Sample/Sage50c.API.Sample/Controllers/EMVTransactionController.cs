using S50cBL22;
using S50cBO22;
using S50cDL22;
using S50cSys22;
using S50cUtil22;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBA;
using SageCEMV15;

namespace Sage50c.API.Sample.Controllers {

    class EMVTransactionController : ControllerBase {
        /// Motor de pagamentos Multibanco (PinpadEthernet)
        /// </summary>
        private BSOEMVManager _bsoEMVManager = null;
        
        internal bool HandleEMV(string Serial, string DocumentId, double AmountValue, int hWnd) {
            bool bOk = false;

            _bsoEMVManager = new BSOEMVManager();

            //            var hWnd = fApi.ActiveForm.Handle;
            //_bsoEMVManager.ParentHwnd = hWnd;//.ToInt32();

            if (HandleEMVPayment(Serial, DocumentId, AmountValue)) {
                //if Refund

                bOk = true;
            }

            _bsoEMVManager = null;

            return bOk;
        }

        private void BsoEMVManager_RequestRefundTransaction(string TransSerial, string TransDocument, double TransDocNumber) {


        }
        private static void POSNotificationManager_WarningFailure(POSNotificationEnum POSNotificationType, EMVResult result) {
            //Fail to commit transaccao

            APIEngine.CoreGlobals.MsgBoxFrontOffice("Detalhes:" + result.ToString(), VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
        }

        //private static void POSNotificationManager_DCCQuery(POSNotificationEnum POSNotificationType, EMVDCCEventArgs args) {

        //}
        //private static void POSNotificationManager_CheckTerminalStatus(POSNotificationEnum POSNotificationType, POSPinpad sender, int SIBSFlags, ref bool Retry, ref bool Ignore, ref bool Cancel) {
        //}

        private static void POSNotificationManager_ShowDialogMessage(POSNotificationEnum POSNotificationType, clsCollectionAccessKey OptionList, ref VbMsgBoxResult ButtonSelected) {
            //string msg = null;
            //short i;
            //short vbButtons;
            
            //clsArrayString oKeys = OptionList.listKeys();
            //int iCount = oKeys.getCount() - 1;

            //if (oKeys.itemExists("MainInstructionText")) {
            //    msg = (string)OptionList.item("MainInstructionText");
            //}

            ////if (oKeys.itemExists("ContentText")) {
            ////    if (msg.Length != 0) {
            ////        msg += Environment.NewLine;
            ////    }
            ////    msg += (string)OptionList.item("ContentText");
            ////}

            //if (msg.Length != 0) {
            //    msg += Environment.NewLine;
            //}

            //string sKey;
            //for (i = 0; i< iCount; i++) {
            //    sKey = oKeys.item[i];
            //    switch (sKey) {
            //        case "MainInstructionText":
            //            break;
            //        case "ContentText":
            //            break;
            //        case "MainIcon":
            //            break;

            //        case "1": //vbOK
            //            msg += "(Ok) - " + OptionList.item(oKeys.item[i]);
            //            msg += Environment.NewLine;
            //            vbButtons += VBA.VbMsgBoxResult.vbOK;
            //            break;

            //        //case "2"://vbCancel
            //        //    vbButtons += VBA.VbMsgBoxResult.vbCancel;
            //        //    break;

            //        //case "3"://vbAbort
            //        //    vbButtons += VBA.VbMsgBoxResult.vbAbort;
            //        //    break;
            //        //case "4"://vbRetry
            //        //    vbButtons += VBA.VbMsgBoxResult.vbRetry;
            //        //    break;
            //        //case "5"://vbIgnore
            //        //    vbButtons += VBA.VbMsgBoxResult.vbIgnore;
            //        //    break;
            //        //case "6"://vbYes
            //        //    vbButtons += VBA.VbMsgBoxResult.vbYes;
            //        //    break;
            //        //case "7"://vbNo
            //        //    vbButtons += VBA.VbMsgBoxResult.vbNo;
            //        //    break;

            //    }
            //}

            ButtonSelected = APIEngine.CoreGlobals.MsgBoxDialog(OptionList, ButtonSelected);

        }

        private bool HandleEMVPayment(string DocumentSerial, string DocumentId, double AmountValue) {
            string strMessage = string.Empty;

            TenderLineItemList tenderLineItemList = null;
            DSOTender dsoTender = new DSOTender();
            DSOCurrency dsoCurrency = new DSOCurrency();
            TransactionID transactionID = null;
            bool tpaOk = false;
            TransactionWarningsEnum tpaWarning = 0;

            Tender tender = dsoTender.GetFirstTenderType(TenderTypeEnum.tndCreditDebitCard);
            if (tender != null) {
                ItemTransaction itemTransaction = new ItemTransaction {
                    TransDocType = DocumentTypeEnum.dcTypeSale,
                    TransBehavior = TransBehaviorEnum.BehAlwaysNewDocument,
                    BaseCurrency = dsoCurrency.GetCurrency("EUR"),
                    TransSerial = DocumentSerial,
                    TransDocument = DocumentId,
                    TransDocNumber = 1,
                    PartyTypeCode = PartyTypeEnum.ptCustomer
                };
                tenderLineItemList = new TenderLineItemList();
                TenderLineItem tenderLineItem = new TenderLineItem {
                    Tender = tender,
                    TenderCurrency = itemTransaction.BaseCurrency,
                    Amount = AmountValue
                };

                tenderLineItemList.Add(tenderLineItem);
                itemTransaction.TenderLineItem = tenderLineItemList;
                transactionID = itemTransaction.TransactionID;

                APIEngine.BLGlobals.POSNotificationManager.ShowDialogMessage += POSNotificationManager_ShowDialogMessage;
                APIEngine.BLGlobals.POSNotificationManager.WarningFailure += POSNotificationManager_WarningFailure;
                //APIEngine.BLGlobals.POSNotificationManager.DCCQuery += POSNotificationManager_DCCQuery;
                //APIEngine.BLGlobals.POSNotificationManager.CheckTerminalStatus  += POSNotificationManager_CheckTerminalStatus; 

                tpaOk = _bsoEMVManager.Init();
                if (tpaOk) {

                    tpaOk = _bsoEMVManager.CreateEMVPayment(itemTransaction);
                    if (tpaOk) {
                        tpaOk = _bsoEMVManager.FinishEMVPayment(transactionID, tenderLineItemList, false);
                    }

                    if (tpaOk) {
                        tpaOk = false;
                        if (itemTransaction.TenderLineItem.HasTenderCard()) {
                            if (itemTransaction.TenderLineItem.TenderCard != null) {
                                tpaOk = true;
                                strMessage = "CustomerTicket:" + itemTransaction.TenderLineItem.TenderCard.CustomerTicket;
                                APIEngine.CoreGlobals.MsgBoxFrontOffice(strMessage, VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                            }
                        }
                    }
                }
                if (!tpaOk) {
                    tpaWarning = _bsoEMVManager.TransactionWarning;

                    strMessage = APIEngine.gLng.GS((int)tpaWarning);

                    if (!string.IsNullOrEmpty(strMessage)) {
                        APIEngine.CoreGlobals.MsgBoxFrontOffice(strMessage, VBA.VbMsgBoxStyle.vbInformation,Application.ProductName);
                    }
                }

                APIEngine.BLGlobals.POSNotificationManager.ShowDialogMessage -= POSNotificationManager_ShowDialogMessage;
                APIEngine.BLGlobals.POSNotificationManager.WarningFailure -= POSNotificationManager_WarningFailure;
                //APIEngine.BLGlobals.POSNotificationManager.DCCQuery -= POSNotificationManager_DCCQuery;
                //APIEngine.BLGlobals.POSNotificationManager.CheckTerminalStatus -= POSNotificationManager_CheckTerminalStatus;
            }
            return tpaOk;
        }
    }
}
