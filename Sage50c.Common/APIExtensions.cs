using System;
using System.ComponentModel;

using S50cBL22;
using S50cBO22;
using S50cSys22;

namespace Sage50c.API {
    public static class APIExtensions {
        #region Value Types Extensions
        public static double ToDouble(this string value) {
            double result = 0;
            if (!double.TryParse(value, out result)) {
                result = 0;
            }
            return result;
        }

        public static short ToShort(this string value) {
            short result = 0;
            if (!short.TryParse(value, out result)) {
                result = 0;
            }
            return result;
        }

        public static DateTime ToDateTime(this string value) {
            DateTime result = DateTime.Now;
            if (!DateTime.TryParse(value, out result)) {
                result = new DateTime(1899, 12, 30);
            }
            return result;
        }

        public static DateTime ToDateTime(this string value, DateTime DefaultValue) {
            DateTime result = DefaultValue;
            if (!DateTime.TryParse(value, out result)) {
                result = DefaultValue;
            }
            return result;
        }

        public static DateTime ToTime(this string value) {
            DateTime result = new DateTime(value.ToDateTime().TimeOfDay.Ticks);
            return result;
        }

        #endregion

        #region Till Extensions
        public static TillSession EnsureOpenTill(this AccountTransactionManager AccountTransManager, string TillId) {
            var tillId = APIEngine.SystemSettings.WorkstationInfo.DefaultTillID;
            var tillManager = new TillManager();
            var tillSetResult = AccountTransManager.SetTillID(tillId);
            var sessions = APIEngine.DSOCache.TillSessionProvider.GetOpenedTillSessions(tillId, AccountTransManager.Transaction.CreateDate);
            TillSession tillSession = null;
            if (sessions.Length == 1) {
                if (!tillManager.CheckTransactionTillSession(AccountTransManager.Transaction, 0, ref tillSession))
                    throw new Exception("Não foi possível abrir o Caixa");
            }
            //
            foreach (TenderLineItem tenderLine in AccountTransManager.Transaction.TenderLineItems) {
                tenderLine.TillId = AccountTransManager.Transaction.Till.TillID;
                tenderLine.CreateDate = AccountTransManager.Transaction.CreateDate;
            }
            //
            return tillSession;
        }
        #endregion

        #region Transaction Extensions
        public static TillSession EnsureOpenTill(this BSOItemTransaction BSOTrans, string TillId) {
            var tillManager = new TillManager();
            var tillSetResult = BSOTrans.SetTillID(TillId);
            var sessions = APIEngine.DSOCache.TillSessionProvider.GetOpenedTillSessions(TillId, BSOTrans.Transaction.CreateDate);
            TillSession tillSession = null;
            if (sessions.Length == 1) {
                if (!tillManager.CheckTransactionTillSession(BSOTrans.Transaction, 0, ref tillSession))
                    throw new Exception("Não foi possível abrir o Caixa");
            }
            //
            foreach (TenderLineItem tenderLine in BSOTrans.Transaction.TenderLineItem) {
                tenderLine.TillId = BSOTrans.Transaction.Till.TillID;
                tenderLine.CreateDate = BSOTrans.Transaction.CreateDate;
            }
            //
            return tillSession;
        }

        public static void SetTransactionTaxIncluded(this BSOItemTransactionDetail BSOTransDetail, bool Value) {
            var tdProp = TypeDescriptor.GetProperties(BSOTransDetail).Find("TransactionTaxIncluded", true);
            tdProp?.SetValue(BSOTransDetail, Value);
        }
        #endregion
    }
}
