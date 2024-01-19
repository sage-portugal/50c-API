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
        public AccountTransaction AccountTransaction { get { return _accountTransManager.Transaction; } }

        public bool Create(AccountUsedEnum AccountUsed, string TransSerial, string TransDoc, double TransDocNumber) {

            // Initialize the account transaction manager
            _accountTransManager.InitManager(AccountUsed);

            // Create a new transaction
            _accountTransManager.InitNewTransaction(TransSerial, TransDoc, TransDocNumber);
            if (_accountTransManager.Transaction == null) {
                throw new Exception($"Não foi possível criar o documento {TransDoc} {TransSerial}/{TransDocNumber}.");
            }

            return true;
        }

        public bool Load(string TransSerial, string TransDoc, double TransDocNumber) {

            // Load an existing transaction
            var transLoaded = _accountTransManager.LoadTransaction(TransSerial, TransDoc, TransDocNumber);
            if (!transLoaded) {
                throw new Exception($"Não foi possível carregar o documento {TransDoc} {TransSerial}/{TransDocNumber}.");
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

        public TransactionID Remove(string TransSerial, string TransDoc, double TransDocNumber, string ProductName) {

            // Obtain the transaction (receipt or payment)
            var result = _accountTransManager.LoadTransaction(TransSerial, TransDoc, TransDocNumber);
            if (!result) {
                throw new Exception($"O documento {TransDoc} {TransSerial}/{TransDocNumber} não existe ou não é possível carregá-lo.");
            }

            // Set the motive for the removal (may or may not be required depending on the document's definition)
            _accountTransManager.Transaction.VoidMotive = "Anulado por " + ProductName;

            // Remove the document
            if (_accountTransManager.DeleteDocument()) {
                editState = EditState.None;
                return _accountTransManager.Transaction.TransactionID;
            }
            else {
                throw new Exception($"Não foi possível anular o documento {TransDoc} {TransSerial}/{TransDocNumber}.");
            }
        }

        public bool Validate(out string ErrorMessage) {

            bool result = true;
            StringBuilder errorMessage = new StringBuilder();

            result = _accountTransManager.ValidateForSave();

            if (_accountTransManager.Locked) {
                errorMessage.AppendLine("O recibo está bloqueado e por isso não pode ser alterado!");
            }

            if (errorMessage.Length != 0) {
                result = false;
            }

            ErrorMessage = errorMessage.ToString();
            return result;
        }

        public bool SetAccountID(string AccountID) {
            return _accountTransManager.SetAccountID(AccountID);
        }

        public bool SetBaseCurrencyID(string AccountTransDocCurrency) {
            return _accountTransManager.SetBaseCurrencyID(AccountTransDocCurrency);
        }

        public bool SetCreateDate(DateTime CreateDate) {
            return _accountTransManager.SetCreateDate(CreateDate);
        }

        public bool SetPartyID(double PartyID) {
            return _accountTransManager.SetPartyID(PartyID);
        }

        public bool SetLedgerAccount(string AccountID, double PartyID) {

            var transaction = _accountTransManager.Transaction;

            transaction.LedgerAccounts = dsoCache.LedgerAccountProvider.GetLedgerAccountList(_accountTransManager.AccountUsed, AccountID, PartyID, transaction.BaseCurrency);
            if (transaction.LedgerAccounts.Count == 0) {
                throw new Exception($"A entidade [{PartyID}] não tem pendentes na carteira [{AccountID}].");
            }

            return true;
        }

        /// <summary>
        /// Erases all transaction details of the current transaction
        /// </summary>
        public void ClearTransactionDetails() {
            while (_accountTransManager.Transaction.Details.Count > 0) {
                _accountTransManager.Transaction.Details.Remove(1);
            }
        }

        /// <summary>
        /// Adds a transaction detail to the current transaction
        /// </summary>
        public AccountTransactionDetail AddDetail(string TransDoc, string TransSerial, double TransDocNumber, short TransInstallment, double PaymentValue) {

            AccountTransactionDetail detail = null;

            if (PaymentValue > 0) {
                AccountTransaction accountTrans = _accountTransManager.Transaction;

                if (systemSettings.WorkstationInfo.Document.IsInCollection(TransDoc)) {

                    // Obter o pendente. PAra efeito de exemplo consideramos que não há prestações (installmentId=0)
                    var ledger = _accountTransManager.LedgerAccounts.OfType<LedgerAccount>().FirstOrDefault(x => x.TransDocument == TransDoc && x.TransSerial == TransSerial && x.TransDocNumber == TransDocNumber && x.TransInstallmentID == TransInstallment);
                    if (ledger != null) {

                        if (PaymentValue > ledger.TotalPendingAmount) {
                            throw new Exception($"O valor a pagar é superior ao valor em divida no documento: {TransDoc} {TransSerial}/{TransDocNumber}");
                        }

                        detail = accountTrans.Details.Find(TransSerial, TransDoc, TransDocNumber, TransInstallment);
                        if (detail == null) {
                            detail = new AccountTransactionDetail();
                        }

                        _accountTransManager.SetPaymentValue(ledger.Guid, PaymentValue);

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
        public TenderLineItemList GetTenderLineItems(string AccountTransPaymentID) {

            var accountTrans = _accountTransManager.Transaction;
            var TenderLines = new TenderLineItemList();

            // Tender payment type(s)
            short tenderId = dsoCache.TenderProvider.GetFirstTenderCash();
            short.TryParse(AccountTransPaymentID, out tenderId);
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

        public TillSession EnsureOpenTill() {
            return _accountTransManager.EnsureOpenTill(_accountTransManager.Transaction.Till.TillID);
        }
    }
}