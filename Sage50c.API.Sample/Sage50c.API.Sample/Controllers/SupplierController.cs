using S50cDL22;
using S50cSys22;
using System;
using System.Text;

namespace Sage50c.API.Sample.Controllers {
    internal class SupplierController {

        private SystemSettings _systemSettings { get { return APIEngine.SystemSettings;  } }

        private DSOFactory _dsoCache { get { return APIEngine.DSOCache; } }

        private S50cBO22.Supplier _supplier { get; set; } = null;

        public S50cBO22.Supplier Supplier { get { return _supplier; } }

        private EditState _editState = EditState.None;

        /// <summary>
        /// Create a new supplier
        /// </summary>
        /// <returns></returns>
        public S50cBO22.Supplier Create() {
            _supplier = new S50cBO22.Supplier();
            //state
            _editState = EditState.New;
            //Suggest values for required fields
            FillSuggestedValues();
            return _supplier;
        }

        /// <summary>
        /// Get supplier from database
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public S50cBO22.Supplier Load(double supplierId) {
            if (supplierId > 0) {
                _supplier = _dsoCache.SupplierProvider.GetSupplier(supplierId);
                if (_supplier != null) {
                    _editState = EditState.Editing;
                    return _supplier;
                }
                else {
                    throw new Exception($"O Fornecedor [{supplierId}] não existe.");
                }
            }
            else {
                throw new Exception("O código do Fornecedor não está preenchido.");
            }
        }

        /// <summary>
        /// Save (insert or update) supplier
        /// </summary>
        public bool Save() {
            if (Validate()) {
                //Save customer 
                _dsoCache.SupplierProvider.Save(_supplier, _supplier.SupplierID, _editState == EditState.New);
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
            if (_supplier != null) {
                _dsoCache.SupplierProvider.Delete(_supplier.SupplierID);
                _editState = EditState.None;
                return true;
            }
            else {
                throw new Exception($"O Fornecedor [{_supplier.SupplierID}] não existe.");
            }

        }

        /// <summary>
        /// Validate supplier
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Validate() {
            //String for errors 
            StringBuilder error = new StringBuilder();

            //Check if costumer exists
            var supplierExist = _dsoCache.CustomerProvider.CustomerExists(_supplier.SupplierID);
            //
            if (!supplierExist && _editState == EditState.Editing) {
                throw new Exception($"O Fornecedor [{_supplier.SupplierID}] não existe.");
            }
            else if (supplierExist && _editState == EditState.New) {
                throw new Exception($"O Fornecedor [{_supplier.SupplierID}] já existe.");
            }
            else {
                //ID
                if (_supplier.SupplierID <= 0 && supplierExist) {
                    error.AppendLine("Tem que preencher o código do Fornecedor!");
                }
                else if (_editState == EditState.New) {
                    _supplier.SupplierID = _dsoCache.SupplierProvider.GetNewID();
                }
                //Name
                if (string.IsNullOrEmpty(_supplier.OrganizationName)) {
                    error.AppendLine("Tem que preencher o nome do Fornecedor!");
                }
                if (_supplier.EntityFiscalStatusID <= 0) {
                    error.AppendLine("Tem que preencher o imposto do Fornecedor!");
                }
                //Zone
                if (_supplier.ZoneID <= 0) {
                    error.AppendLine("Tem que preencher o código da Zona!");
                }
                else if (_dsoCache.ZoneProvider.ZoneExists(_supplier.ZoneID) == false) {
                    error.AppendLine($"A Zona [{_supplier.ZoneID}] não existe!");
                }
                //Country
                if (string.IsNullOrEmpty(_supplier.CountryID)) {
                    error.AppendLine("Tem que preencher o País");
                }
                else if (_dsoCache.CountryProvider.CountryExists(_supplier.CountryID) == false) {
                    error.AppendLine($"O País [{_supplier.CountryID}] não existe!");
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
            if(_supplier != null ) {
                _supplier.PaymentID = _dsoCache.PaymentProvider.GetFirstID();
                _supplier.TenderID = _dsoCache.TenderProvider.GetFirstTenderCash();
                _supplier.ZoneID = _dsoCache.ZoneProvider.FindZone(ZoneTypeEnum.ztNational);
                _supplier.CountryID = _systemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID;
                return true;
            }
            else {
                return false;
            }
        }

    }
}
