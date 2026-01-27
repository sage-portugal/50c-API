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

//private void BtnTest_Click(object sender, EventArgs e) {
//    /// Motor de pagamentos Multibanco (PinpadEthernet)
//    /// </summary>
//    EMVTransactionController emvTransactionController = new EMVTransactionController();
//    try {
//        btnTest.Enabled = false;
//        double payValue = txtTransDocNumber.Text.ToDouble();
//        emvTransactionController.HandleEMV(txtTransSerial.Text, txtTransDoc.Text, payValue);
//        btnTest.Enabled = true;
//    }
//    catch (Exception ex) {
//        APIEngine.CoreGlobals.MsgBoxFrontOffice(ex.Message, VBA.VbMsgBoxStyle.vbExclamation, Application.ProductName);
//    }
//}

namespace Sage50c.API.Sample.Controllers {
    class EMVTransactionController : ControllerBase {
        /// Motor de pagamentos Multibanco (PinpadEthernet)
        /// </summary>
        private BSOEMVManager _bsoEMVManager = null;
        string pRefundId;
        internal bool HandleEMV(string Serial, string DocumentId, double AmountValue, string RefundId) {
            bool bOk = false;
            pRefundId = RefundId;

            _bsoEMVManager = new BSOEMVManager();

            if (HandleEMVPayment(Serial, DocumentId, AmountValue)) {
               
                bOk = true;
            }

            _bsoEMVManager = null;

            return bOk;
        }

        private void BsoEMVManager_RequestRefundTransaction(ref string TransSerial, ref string TransDocument, ref double TransDocNumber) {
            //Document and CardTenderLineItem must exist in the database
            //Ex:
            //TransSerial = "EXT";
            //TransDocument = "FS";
            //TransDocNumber = 1;
                                
            TransactionID refundTransactionID = new TransactionID();
            refundTransactionID.FromString (pRefundId);

            if (refundTransactionID != null) {
                if ((refundTransactionID.TransSerial.Length > 0) && (refundTransactionID.TransDocument.Length > 0) || (refundTransactionID.TransDocNumber > 0)) {
                    TransSerial = refundTransactionID.TransSerial;
                    TransDocument = refundTransactionID.TransDocument;
                    TransDocNumber = refundTransactionID.TransDocNumber;
                }
            }
        }

        private static void POSNotificationManager_WarningFailure(POSNotificationEnum POSNotificationType, EMVResult result) {
            //Fail to commit transaccao
            APIEngine.CoreGlobals.MsgBoxFrontOffice("Detalhes:" + result.ToString(), VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
        }

        private static void POSNotificationManager_ShowDialogMessage(POSNotificationEnum POSNotificationType, clsCollectionAccessKey OptionList, ref VbMsgBoxResult ButtonSelected) {
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

                tpaOk = _bsoEMVManager.Init();
                _bsoEMVManager.RequestRefundTransaction += BsoEMVManager_RequestRefundTransaction;
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
                                strMessage = "CustomerTicket:" + Environment.NewLine + itemTransaction.TenderLineItem.TenderCard.CustomerTicket;
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
            }
            return tpaOk;
        }

    }
}
