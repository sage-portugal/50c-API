using S50cBO22;
using S50cDL22;
using S50cSys22;
using SageCoreSaft60;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sage50c.API.Sample.Controllers {
    internal class CustomerController {
        private SystemSettings systemSettings { get { return APIEngine.SystemSettings; } }

        private DSOFactory dsoCache { get { return APIEngine.DSOCache; } }

        private DSOCustomer _customerProvider { get { return APIEngine.DSOCache.CustomerProvider; } }

        public S50cBO22.Customer _customer { get; set; } = null;

        /// <summary>
        /// Save (insert or update) customer
        /// </summary>
        /// <param name="isNew"></param>
        /// <exception cref="Exception"></exception>
        public void CustomerUpdate(bool isNew) {
            S50cBO22.Customer myCustomer = null;
            //Check if costumer exists
            myCustomer = CustomerGet(_customer.CustomerID);
            //
            if (myCustomer == null && !isNew) {
                throw new Exception(string.Format("O cliente [{0}] não existe.", _customer.CustomerID));
            }
            else if (myCustomer != null && isNew) {
                throw new Exception(string.Format("O cliente [{0}] já existe.", _customer.CustomerID));
            }
            //
            if(myCustomer == null) {
                //If customer doesn't exist use new id
                myCustomer = new S50cBO22.Customer();
                myCustomer.CustomerID = _customer.CustomerID;
                myCustomer.LimitType = _customer.LimitType;
            }
            myCustomer.OrganizationName = _customer.OrganizationName;
            myCustomer.FederalTaxId = _customer.FederalTaxId;
            myCustomer.Comments = _customer.Comments;
            myCustomer.CarrierID = dsoCache.CarrierProvider.GetFirstCarrierID();
            myCustomer.TenderID = dsoCache.TenderProvider.GetFirstTenderCash();
            //Check for Entity Fiscal
            if(_customer.EntityFiscalStatusID != 0) {
                myCustomer.EntityFiscalStatusID = _customer.EntityFiscalStatusID;
            }
            //If salesman doesn't exist use the first available
            if(!dsoCache.SalesmanProvider.SalesmanExists(_customer.SalesmanId)) {
                myCustomer.SalesmanId = (int)dsoCache.SalesmanProvider.GetFirstSalesmanID();
            } else {
                myCustomer.SalesmanId = _customer.SalesmanId;
            }
            //If currency doesn't exist use base currency
            if(!dsoCache.CurrencyProvider.CurrencyExists(_customer.CurrencyID)) {
                myCustomer.CurrencyID = systemSettings.BaseCurrency.CurrencyID;
            } else {
                myCustomer.CurrencyID = _customer.CurrencyID;
            }
            //If country doesn't exist use default country
            if(!dsoCache.CountryProvider.CountryExists(_customer.CountryID)) {
                myCustomer.CountryID = systemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID;
            } else {
                myCustomer.CountryID = _customer.CountryID;
            }
            //If zone is empty use the first national zone
            if(_customer.ZoneID == 0) {
                myCustomer.ZoneID = dsoCache.ZoneProvider.FindZone(ZoneTypeEnum.ztNational);
            }
            else {
                myCustomer.ZoneID = _customer.ZoneID;
            }
            //If payment mode is empty use the first available
            if(myCustomer.PaymentID == 0) {
                myCustomer.PaymentID = dsoCache.PaymentProvider.GetFirstID(); ;
            }
            //Save customer 
            _customerProvider.Save(myCustomer, myCustomer.CustomerID, isNew);
        }

        /// <summary>
        /// Delete customer
        /// </summary>
        public void CustomerRemove() {
            _customerProvider.Delete(_customer.CustomerID);
        }

        /// <summary>
        /// Get customer from database
        /// </summary>
        /// <returns>customer</returns>
        public S50cBO22.Customer CustomerGet(double customerId ) {
            return _customerProvider.GetCustomer(customerId);
        }
    }
}
