using System;
using System.Linq;
using System.Text;

using S50cBL22;
using S50cBO22;
using S50cSys22;

namespace Sage50c.API.Sample.Controllers {
    internal class AccountTransactionController : ControllerBase {

        /// <summary>
        /// Transaction data
        /// </summary>
        private AccountTransactionManager _accountTransManager = new AccountTransactionManager();
        public AccountTransactionManager AccountTransManager { get { return _accountTransManager; } }

        public bool Create(AccountUsedEnum accountUsed, string transSerial, string transDoc, double transDocNumber) {

            // Initialize the account transaction manager
            _accountTransManager.InitManager(accountUsed);

            // Create a new transaction
            _accountTransManager.InitNewTransaction(transSerial, transDoc, transDocNumber);
            if (_accountTransManager.Transaction == null) {
                throw new Exception($"Não foi possível criar o documento {transDoc} {transSerial}/{transDocNumber}.");
            }

            return true;
        }

        public bool Load(string transSerial, string transDoc, double transDocNumber) {

            // Load an existing transaction
            var transLoaded = _accountTransManager.LoadTransaction(transSerial, transDoc, transDocNumber);
            if (!transLoaded) {
                throw new Exception($"Não foi possível carregar o documento {transDoc} {transSerial}/{transDocNumber}.");
            }

            return true;
        }

        public TransactionID Save() {

            string errorMessage = null;

            if (Validate(out errorMessage)) {
                if (_accountTransManager.SaveDocument(false)) {
                    editState = EditState.Editing;
                    return _accountTransManager.Transaction.TransactionID;
                }
                else {
                    throw new Exception("A gravação do recibo falhou!");
                }
            }
            else {
                throw new Exception(errorMessage);
            }
        }

        public TransactionID Remove(string transSerial, string transDoc, double transDocNumber, string ProductName) {

            // Obtain the transaction (receipt or payment)
            var result = _accountTransManager.LoadTransaction(transSerial, transDoc, transDocNumber);
            if (!result) {
                throw new Exception($"O documento {transDoc} {transSerial}/{transDocNumber} não existe ou não é possível carregá-lo.");
            }

            // Set the motive for the removal (may or may not be required depending on the document's definition)
            _accountTransManager.Transaction.VoidMotive = "Anulado por " + ProductName;

            // Remove the document
            if (_accountTransManager.DeleteDocument()) {
                editState = EditState.None;
                return _accountTransManager.Transaction.TransactionID;
            }
            else {
                throw new Exception($"Não foi possível anular o documento {transDoc} {transSerial}/{transDocNumber}.");
            }
        }

        private bool Validate(out string ErrorMessage) {

            bool result = true;
            StringBuilder errorMessage = new StringBuilder();

            result = _accountTransManager.ValidateForSave();

            ErrorMessage = errorMessage.ToString();
            return result;
        }

        public bool SetAccountID(string accountID) {
            return _accountTransManager.SetAccountID(accountID);
        }

        public bool SetBaseCurrencyID(string accountTransDocCurrency) {
            return _accountTransManager.SetBaseCurrencyID(accountTransDocCurrency);
        }

        public bool SetCreateDate(DateTime createDate) {
            return _accountTransManager.SetCreateDate(createDate);
        }

        public bool SetPartyID(double partyId) {
            return _accountTransManager.SetPartyID(partyId);
        }

        public bool SetLedgerAccount(string accountID, double partyId) {

            var transaction = _accountTransManager.Transaction;

            transaction.LedgerAccounts = dsoCache.LedgerAccountProvider.GetLedgerAccountList(_accountTransManager.AccountUsed, accountID, partyId, transaction.BaseCurrency);
            if (transaction.LedgerAccounts.Count == 0) {
                throw new Exception($"A entidade [{partyId}] não tem pendentes na carteira [{accountID}].");
            }

            return true;
        }

        public AccountTransactionDetail AddDetail(string transDoc, string transSerial, double transDocNumber, short transInstallment, double paymentValue) {

            AccountTransactionDetail detail = null;

            if (paymentValue > 0) {
                AccountTransaction accountTrans = _accountTransManager.Transaction;

                if (systemSettings.WorkstationInfo.Document.IsInCollection(transDoc)) {

                    // Obter o pendente. PAra efeito de exemplo consideramos que não há prestações (installmentId=0)
                    var ledger = _accountTransManager.LedgerAccounts.OfType<LedgerAccount>().FirstOrDefault(x => x.TransDocument == transDoc && x.TransSerial == transSerial && x.TransDocNumber == transDocNumber && x.TransInstallmentID == transInstallment);
                    if (ledger != null) {

                        if (paymentValue > ledger.TotalPendingAmount) {
                            throw new Exception($"O valor a pagar é superior ao valor em divida no documento: {transDoc} {transSerial}/{transDocNumber}");
                        }

                        detail = accountTrans.Details.Find(transSerial, transDoc, transDocNumber, transInstallment);
                        if (detail == null) {
                            detail = new AccountTransactionDetail();
                        }

                        _accountTransManager.SetPaymentValue(ledger.Guid, paymentValue);

                        // Copy the ledger details to the payment
                        detail.AccountTypeID = ledger.PartyAccountTypeID;
                        detail.BaseCurrency = accountTrans.BaseCurrency;
                        detail.DocContractReference = ledger.ContractReferenceNumber;
                        detail.DocCreateDate = ledger.CreateDate;
                        detail.DocCurrency = ledger.BaseCurrency;
                        detail.DocDeferredPaymentDate = ledger.DeferredPaymentDate;
                        detail.DocID = ledger.TransDocument;
                        detail.DocInstallmentID = ledger.TransInstallmentID;
                        detail.DocNumber = ledger.TransDocNumber;
                        detail.DocSerial = ledger.TransSerial;
                        //
                        detail.LedgerGUID = ledger.Guid;
                        detail.PartyID = accountTrans.Entity.PartyID;
                        detail.PartyTypeCode = (short)accountTrans.Entity.PartyType;
                        //
                        detail.RetentionOriginalAmount = ledger.RetentionTotalAmount;
                        detail.RetentionPayedAmount = ledger.RetentionPayedAmount;
                        detail.RetentionPendingAmount = ledger.RetentionPendingAmount - ledger.RetentionPayedAmount;
                        //
                        detail.TotalDiscountAmount = ledger.DiscountValue;
                        detail.TotalDiscountPercent = ledger.DiscountPercent;
                        detail.TotalOriginalAmount = ledger.TotalAmount;
                        detail.TotalPayedAmount = ledger.PaymentValue;
                        detail.TotalPendingAmount = ledger.TotalPendingAmount - ledger.PaymentValue;
                        //
                        detail.TransDocNumber = accountTrans.TransDocNumber;
                        detail.TransDocument = accountTrans.TransDocument;
                        detail.TransSerial = accountTrans.TransSerial;
                        //
                        detail.CashAccountingSchemeType = ledger.CashAccountingSchemeType;
                        //
                        accountTrans.Details.Add(ref detail);
                    }
                }
            }

            return detail;
        }

        /// <summary>
        /// Fill the types of payment used when receiving/paying 
        /// </summary>
        public TenderLineItemList GetTenderLineItems(string accountTransPaymentId) {

            var accountTrans = _accountTransManager.Transaction;
            var TenderLines = new TenderLineItemList();

            // Tender payment type(s)
            short tenderId = dsoCache.TenderProvider.GetFirstTenderCash();
            short.TryParse(accountTransPaymentId, out tenderId);
            var tender = dsoCache.TenderProvider.GetTender(tenderId);

            // Add tender line
            var tenderLine = new TenderLineItem();
            tenderLine.Tender = tender;
            tenderLine.Amount = accountTrans.TotalAmount;
            // The till must be open. To simplify the system default has been set
            tenderLine.TillId = systemSettings.WorkstationInfo.DefaultMainTillID;
            tenderLine.TenderCurrency = accountTrans.BaseCurrency;
            tenderLine.PartyTypeCode = accountTrans.PartyTypeCode;
            tenderLine.PartyID = accountTrans.Entity.PartyID;
            tenderLine.CreateDate = DateTime.Today;

            // For the sake of simplicity, this example will only consider check payment
            if (tender.TenderType == TenderTypeEnum.tndCheck) {

                TenderCheck tenderCheck = null;
                if (tenderLine.TenderCheck == null) {
                    tenderLine.TenderCheck = new TenderCheck();
                }

                tenderCheck = tenderLine.TenderCheck;
                tenderCheck.CheckAmount = tenderLine.Amount;
                tenderCheck.CheckDeferredDate = tenderLine.CreateDate;
                tenderCheck.TillId = tenderLine.TillId;

                tenderLine.TenderCheck = tenderCheck;

                var formCheck = new FormTenderCheck();
                if (formCheck.FillTenderCheck(tenderCheck) == System.Windows.Forms.DialogResult.Cancel) {
                    throw new Exception("É necessário preencher os dados do cheque.");
                }
            }

            TenderLines.Add(tenderLine);
            return TenderLines;
        }
    }
}