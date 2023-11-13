using System;
using System.Text;

using S50cBO22;
using S50cSys22;

namespace Sage50c.API.Sample.Controllers {
    internal class CustomerController : ControllerBase {

        private Customer _customer = null;
        public Customer Customer { get { return _customer; } }

        /// <summary>
        /// Create a new customer
        /// </summary>
        public Customer Create() {

            _customer = new Customer();
            //state
            editState = EditState.New;
            // Suggest values for required fields
            FillSuggestedValues();
            return _customer;
        }

        /// <summary>
        /// Get customer from database
        /// </summary>
        public Customer Load(double CustomerId) {

            if (CustomerId > 0) {
                _customer = dsoCache.CustomerProvider.GetCustomer(CustomerId);
                if (_customer != null) {
                    editState = EditState.Editing;
                    return _customer;
                }
                else {
                    throw new Exception($"O Cliente [{CustomerId}] não existe.");
                }
            }
            else {
                throw new Exception("O código do Cliente não está preenchido.");
            }
        }

        /// <summary>
        /// Save (insert or update) customer
        /// </summary>
        public bool Save() {

            if (Validate()) {
                //Save customer 
                dsoCache.CustomerProvider.Save(_customer, _customer.CustomerID, editState == EditState.New);
                editState = EditState.Editing;
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Delete customer
        /// </summary>
        public bool Remove() {

            if (_customer != null) {
                dsoCache.CustomerProvider.Delete(_customer.CustomerID);
                editState = EditState.None;
                return true;
            }
            else {
                throw new Exception($"O Cliente [{_customer.CustomerID}] não existe.");
            }

        }

        /// <summary>
        /// Validate customer 
        /// </summary>
        public bool Validate() {

            //String for errors 
            StringBuilder error = new StringBuilder();

            //Check if costumer exists
            var customerExist = dsoCache.CustomerProvider.CustomerExists(_customer.CustomerID);
            //
            if (!customerExist && editState == EditState.Editing) {
                throw new Exception($"O Cliente [{_customer.CustomerID}] não existe.");
            }
            else if (customerExist && editState == EditState.New) {
                throw new Exception($"O Cliente [{_customer.CustomerID}] já existe.");
            }
            else {
                //ID
                if (_customer.CustomerID <= 0 && customerExist) {
                    error.AppendLine("Tem que preencher o código do Cliente!");
                }
                else if (editState == EditState.New) {
                    _customer.CustomerID = dsoCache.CustomerProvider.GetNewID();
                }
                //Name
                if (string.IsNullOrEmpty(_customer.OrganizationName)) {
                    error.AppendLine("Tem que preencher o nome do Cliente!");
                }
                if (_customer.EntityFiscalStatusID <= 0) {
                    error.AppendLine("Tem que preencher o imposto do Cliente!");
                }
                //Salesman
                if (_customer.SalesmanId <= 0) {
                    error.AppendLine("Tem que preencher o código do Vendedor!");
                }
                else if (dsoCache.SalesmanProvider.SalesmanExists(_customer.SalesmanId) == false) {
                    error.AppendLine($"O Vendedor [{_customer.SalesmanId}] não existe!");
                }
                //Zone
                if (_customer.ZoneID <= 0) {
                    error.AppendLine("Tem que preencher o código da Zona!");
                }
                else if (dsoCache.ZoneProvider.ZoneExists(_customer.ZoneID) == false) {
                    error.AppendLine($"A Zona [{_customer.ZoneID}] não existe!");
                }
                //Country
                if (string.IsNullOrEmpty(_customer.CountryID)) {
                    error.AppendLine("Tem que preencher o País");
                }
                else if (dsoCache.CountryProvider.CountryExists(_customer.CountryID) == false) {
                    error.AppendLine($"O País [{_customer.CountryID}] não existe!");
                }
                //Error message
                if (error.Length > 0) {
                    throw new Exception(error.ToString());
                }
                else {
                    return true;
                }
            }
        }

        public bool FillSuggestedValues() {

            if (_customer != null) {
                _customer.CarrierID = dsoCache.CarrierProvider.GetFirstCarrierID();
                _customer.TenderID = dsoCache.TenderProvider.GetFirstTenderCash();
                _customer.PaymentID = dsoCache.PaymentProvider.GetFirstID();
                _customer.SalesmanId = (int)dsoCache.SalesmanProvider.GetFirstSalesmanID();
                _customer.ZoneID = dsoCache.ZoneProvider.FindZone(ZoneTypeEnum.ztNational);
                _customer.CountryID = systemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID;
                return true;
            }
            else {
                return false;
            }
        }
    }
}