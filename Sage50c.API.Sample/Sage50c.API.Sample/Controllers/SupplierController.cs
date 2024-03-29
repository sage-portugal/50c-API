﻿using System;
using System.Text;
using S50cBO22;
using S50cSys22;

namespace Sage50c.API.Sample.Controllers {
    internal class SupplierController : ControllerBase {

        private Supplier _supplier { get; set; } = null;
        public Supplier Supplier { get { return _supplier; } }

        /// <summary>
        /// Create a new supplier
        /// </summary>
        public Supplier Create() {

            _supplier = new Supplier();
            //state
            editState = EditState.New;
            //Suggest values for required fields
            FillDefaultValues();
            return _supplier;
        }

        /// <summary>
        /// Get supplier from database
        /// </summary>
        public Supplier Load(double SupplierId) {

            if (SupplierId > 0) {
                _supplier = dsoCache.SupplierProvider.GetSupplier(SupplierId);
                if (_supplier != null) {
                    editState = EditState.Editing;
                    return _supplier;
                }
                else {
                    throw new Exception($"O Fornecedor [{SupplierId}] não existe.");
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
            bool result = false;
            if (Validate()) {
                //Save supplier 
                dsoCache.SupplierProvider.Save(_supplier, _supplier.SupplierID, editState == EditState.New);
                editState = EditState.Editing;
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Delete supplier
        /// </summary>
        public bool Remove() {
            bool result = false;
            if (_supplier != null) {

                if (_supplier == null || !dsoCache.SupplierProvider.SupplierExists(_supplier.SupplierID)) {
                    throw new Exception($"O Fornecedor [{_supplier.SupplierID}] não existe.");
                }
                else {
                    dsoCache.SupplierProvider.Delete(_supplier.SupplierID);
                    editState = EditState.None;
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Validate supplier
        /// </summary>
        public bool Validate() {

            //String for errors 
            StringBuilder error = new StringBuilder();

            //Check if supplier exists
            var supplierExist = dsoCache.SupplierProvider.SupplierExists(_supplier.SupplierID);
            //
            if (!supplierExist && editState == EditState.Editing) {
                throw new Exception($"O Fornecedor [{_supplier.SupplierID}] não existe.");
            }
            else if (supplierExist && editState == EditState.New) {
                throw new Exception($"O Fornecedor [{_supplier.SupplierID}] já existe.");
            }
            else {
                //ID
                if (_supplier.SupplierID <= 0 && supplierExist) {
                    error.AppendLine("Tem que preencher o código do Fornecedor!");
                }
                else if (editState == EditState.New && _supplier.SupplierID == 0) {
                    _supplier.SupplierID = dsoCache.SupplierProvider.GetNewID();
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
                else if (dsoCache.ZoneProvider.ZoneExists(_supplier.ZoneID) == false) {
                    error.AppendLine($"A Zona [{_supplier.ZoneID}] não existe!");
                }
                //Country
                if (string.IsNullOrEmpty(_supplier.CountryID)) {
                    error.AppendLine("Tem que preencher o País");
                }
                else if (dsoCache.CountryProvider.CountryExists(_supplier.CountryID) == false) {
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

        public bool FillDefaultValues() {
            bool result = false;
            if (_supplier != null) {
                _supplier.PaymentID = dsoCache.PaymentProvider.GetFirstID();
                _supplier.TenderID = dsoCache.TenderProvider.GetFirstTenderCash();
                _supplier.ZoneID = dsoCache.ZoneProvider.FindZone(ZoneTypeEnum.ztNational);
                _supplier.CountryID = systemSettings.SystemInfo.LocalDefinitionsSettings.DefaultCountryID;
                result = true;
            }
            return result;
        }
    }
}