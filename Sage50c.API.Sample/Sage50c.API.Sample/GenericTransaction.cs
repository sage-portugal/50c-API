using S50cBO22;
using S50cSys22;
using System;

namespace Sage50c.API.Sample {
    public class GenericTransaction {

        public string TransDocument { get; }

        public CurrencyDefinition BaseCurrency { get; }

        public DateTime CreateDate { get; set; }

        public bool TransactionTaxIncluded { get; }

        public double TransDocNumber { get; }

        private short _PaymentID;
        public string PaymentID { get { return _PaymentID > 0 ? _PaymentID.ToString() : string.Empty; } }

        private short _TenderID;
        public string TenderID { get { return _TenderID > 0 ? _TenderID.ToString() : string.Empty; } }

        public string PaymentDiscountPercent { get; }

        public bool TransGlobalDiscountEnabled { get; }

        public double PartyID { get; }

        public string TransSerial { get; }

        public ItemTransactionDetailList Details { get; }

        public TransStatusEnum TransStatus { get; }

        public DocumentTypeEnum TransDocType { get; }

        public SimpleDocumentList BuyShareOtherCostList { get; }

        public GenericTransaction(ItemTransaction trans) {
            //Common
            TransDocument = trans.TransDocument;
            BaseCurrency = trans.BaseCurrency;
            CreateDate = trans.CreateDate;
            TransactionTaxIncluded = trans.TransactionTaxIncluded;
            TransDocNumber = trans.TransDocNumber;
            PartyID = trans.PartyID;
            TransSerial = trans.TransSerial;
            TransStatus = trans.TransStatus;
            Details = trans.Details;
            TransDocType = trans.TransDocType;

            //Specific
            _PaymentID = trans.Payment.PaymentID;
            _TenderID = trans.Tender.TenderID;
            PaymentDiscountPercent = trans.PaymentDiscountPercent.ToString();
            TransGlobalDiscountEnabled = true;
            BuyShareOtherCostList = trans.BuyShareOtherCostList;
        }

        public GenericTransaction(StockTransaction trans) {
            //Common
            TransDocument = trans.TransDocument;
            BaseCurrency = trans.BaseCurrency;
            CreateDate = trans.CreateDate;
            TransactionTaxIncluded = trans.TransactionTaxIncluded;
            TransDocNumber = trans.TransDocNumber;
            PartyID = trans.PartyID;
            TransSerial = trans.TransSerial;
            TransStatus = trans.TransStatus;
            Details = trans.Details;
            TransDocType = trans.TransDocType;

            //Specific
            _PaymentID = 0;
            _TenderID = 0;
            PaymentDiscountPercent = string.Empty;
            TransGlobalDiscountEnabled = false;
        }
    }
}
