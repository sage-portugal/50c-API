using S50cSys22;
using S50cUtil22;
using S50cBO22;

namespace Sage50c.API {
    public class APIQuickSearch {
        #region QuickSearch

        public enum EntityTypeEnum {
            itZone = 0,
            itPayment = 1,
            itTender = 2,
            itSalesman = 3,
            itCustomer = 4,
            itFamily = 5,
            itWarehouse = 6,
            itItem = 7
        }

        // NOTA: QuickSearch NÃO É SUPORTADO EM .NET 
        public static QuickSearch CreateQuickSearch(EntityTypeEnum entityType) {
            var systemSettings = APIEngine.SystemSettings;
            QuickSearchViews qsvQuickSearchID = QuickSearchViews.QSV_None;
            clsCollection paramValues = null;
            QuickSearch quickSearch = null;

            if ( quickSearch != null)
                if ( quickSearch.isFinding) 
                    return quickSearch;

            switch (entityType) {
                case EntityTypeEnum.itZone:
                    qsvQuickSearchID = QuickSearchViews.QSV_Zone;
                    break;

                case EntityTypeEnum.itPayment:
                    qsvQuickSearchID = QuickSearchViews.QSV_Payment;
                    break;

                case EntityTypeEnum.itTender:
                    qsvQuickSearchID = QuickSearchViews.QSV_TenderNames;
                    break;

                case EntityTypeEnum.itSalesman:
                    qsvQuickSearchID = QuickSearchViews.QSV_Salesman;
                    break;

                case EntityTypeEnum.itCustomer:
                    qsvQuickSearchID = QuickSearchViews.QSV_Customer;
                    break;

                case EntityTypeEnum.itFamily:
                    qsvQuickSearchID = QuickSearchViews.QSV_Family;
                    break;

                case EntityTypeEnum.itWarehouse:
                    qsvQuickSearchID = QuickSearchViews.QSV_Warehouse;
                    break;

                case EntityTypeEnum.itItem:
                    qsvQuickSearchID = QuickSearchViews.QSV_Item;

                    paramValues = new clsCollection();
                    paramValues.add(systemSettings.QuickSearchDefaults.WarehouseID, "@WarehouseID");
                    paramValues.add(systemSettings.QuickSearchDefaults.PriceLineID, "@PriceLineID");
                    paramValues.add(systemSettings.QuickSearchDefaults.LanguageID, "@LanguageID");
                    paramValues.add(systemSettings.QuickSearchDefaults.DisplayDiscontinued, "@Discontinued");
                    if (systemSettings.StartUpInfo.UseItemSearchAlterCurrency)
                        paramValues.add(systemSettings.AlternativeCurrency.SaleExchange, "@ctxBaseCurrency");
                    else
                        paramValues.add(systemSettings.QuickSearchDefaults.EuroConversionRate, "@ctxBaseCurrency");
                    break;
            }
            quickSearch = APIEngine.CreateQuickSearch(qsvQuickSearchID, false);

            if (paramValues != null)
                quickSearch.Parameters = paramValues;

            return quickSearch;
        }
        #endregion
    }
}