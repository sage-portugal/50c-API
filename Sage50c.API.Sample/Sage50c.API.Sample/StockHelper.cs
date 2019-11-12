using S50cBO18;
using S50cBL18;
using S50cDL18;
using S50cSys18;
using S50cUtil18;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sage50c.API.Sample {
    internal static class StockHelper {
        #region Stock Transaction

        internal static double CalculateQuantity(string strFormula, ItemTransactionDetail TransactionDetail, bool UseQuantityFactor) {
            MathFunctions mathUtil = new MathFunctions();
            UnitOfMeasure oUnit;
            BSOExpressionParser objBSOExpressionParser = new BSOExpressionParser();
            double result = 0;

            if (!string.IsNullOrEmpty(strFormula)) {
                result = 0;
                string tempres = objBSOExpressionParser.ParseFormula(strFormula, TransactionDetail);
                double.TryParse(tempres, out result);
            }
            else
                result = TransactionDetail.Quantity;

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

        #endregion
    }
}
