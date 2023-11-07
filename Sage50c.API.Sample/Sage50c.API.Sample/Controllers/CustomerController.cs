using S50cDL22;
using S50cSys22;
using System;
using System.Text;

namespace Sage50c.API.Sample.Controllers {
    internal class CustomerController {

        private SystemSettings _systemSettings { get { return APIEngine.SystemSettings; } }

        private DSOFactory _dsoCache { get { return APIEngine.DSOCache; } }

        private S50cBO22.Customer _customer = null;

        public S50cBO22.Customer Customer { get { return _customer; } }

        private EditState _editState = EditState.None;

        /// <summary>
        /// Create a new customer
        /// </summary>
        /// <returns></returns>
        public S50cBO22.Customer Create() {
            _customer = new S50cBO22.Customer();
            //state
            _editState = EditState.New;
            // Suggest values for required fields
            FillSuggestedValues();
            return _customer;
        }

        /// <summary>
        /// Get customer from database
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public S50cBO22.Customer Load(double customerId) {
            if (customerId > 0) {
                _customer = _dsoCache.CustomerProvider.GetCustomer(customerId);
                if (_customer != null) {
                    _editState = EditState.Editing;
                    return _customer;
                }
                else {
                    throw new Exception($"O Cliente [{customerId}] não existe.");
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
                _dsoCache.CustomerProvider.Save(_customer, _customer.CustomerID, _editState == EditState.New);
                _editState = EditState.Editing;
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
                _dsoCache.CustomerProvider.Delete(_customer.CustomerID);
                _editState = EditState.None;
                return true;
            }
            else {
                throw new Exception($"O Cliente [{_customer.CustomerID}] não existe.");
            }

        }

        /// <summary>
        /// Validate customer 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Validate() {
            //String for errors 
            StringBuilder error = new StringBuilder();

            //Check if costumer exists
            var customerExist = _dsoCache.CustomerProvider.CustomerExists(_customer.CustomerID);
            //
            if (!customerExist && _editState == EditState.Editing) {
                throw new Exception($"O Cliente [{_customer.CustomerID}] não existe.");
            }
            else if (customerExist && _editState == EditState.New) {
                throw new Exception($"O Cliente [{_customer.CustomerID}] já existe.");
            }
            else {
                //ID
                if (_customer.CustomerID <= 0 && customerExist) {
                    error.AppendLine("Tem que preencher o código do Cliente!");
                }
                else if (_editState == EditState.New) {
                    _customer.CustomerID = _dsoCache.CustomerProvider.GetNewID();
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
                else if (_dsoCache.SalesmanProvider.SalesmanExists(_customer.SalesmanId) == false) {
                    error.AppendLine($"O Vendedor [{_customer.SalesmanId}] não existe!");
                }
                //Zone
                if (_customer.ZoneID <= 0) {
                    error.AppendLine("Tem que preencher o código da Zona!");
                }
                else if (_dsoCache.ZoneProvider.ZoneExists(_customer.ZoneID) == false) {
                    error.AppendLine($"A Zona [{_customer.ZoneID}] não existe!");
                }
                //Country
                if (string.IsNullOrEmpty(_customer.CountryID)) {
                    error.AppendLine("Tem que preencher o País");
                }
                else if (_dsoCache.CountryProvider.CountryExists(_customer.CountryID) == false) {
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
                _customer.CarrierID = _dsoCache.CarrierProvider.GetFirstCarrierID();
                _customer.TenderID = _dsoCache.TenderProvider.GetFirstTenderCash();
                _customer.PaymentID = _dsoCache.PaymentProvider.GetFirstID();
                _customer.SalesmanId = (int)_dsoCache.SalesmanProvider.GetFirstSalesmanID();
                _customer.ZoneID = _dsoCache.ZoneProvider.FindZone(ZoneTypeEnum.ztNational);
                _customer.CountryID = _systemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID;
                return true;
            }
            else {
                return false;
            }
        }

    }
}
