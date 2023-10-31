using System;
using System.Text;

using S50cSys22;

namespace Sage50c.API.Sample.Controllers {
    internal class UnitOfMeasureController : ControllerBase {

        /// <summary>
        /// Current unit of measure data
        /// </summary>
        private UnitOfMeasure _unitOfMeasure = null;
        public UnitOfMeasure UnitOfMeasure { get { return _unitOfMeasure; } }

        public UnitOfMeasure Create() {

            _unitOfMeasure = new UnitOfMeasure();

            FillDefaultValues();

            editState = EditState.New;
            return _unitOfMeasure;
        }

        public UnitOfMeasure Load(string unitOfMeasureID) {

            if (string.IsNullOrEmpty(unitOfMeasureID)) {
                throw new Exception("O código da unidade de medição não está preenchido.");
            }

            _unitOfMeasure = dsoCache.UnitOfMeasureProvider.GetUnitOfMeasure(unitOfMeasureID);
            editState = _unitOfMeasure != null ? EditState.Editing : editState;
            return _unitOfMeasure;
        }

        public bool Save() {

            string errorMessage = null;

            if (Validate(out errorMessage)) {
                dsoCache.UnitOfMeasureProvider.Save(_unitOfMeasure, _unitOfMeasure.UnitOfMeasureID, editState == EditState.New);
                editState = EditState.Editing;
            }
            else {
                throw new Exception(errorMessage);
            }

            return true;
        }

        public void Remove(string unitOfMeasureID) {

            if (string.IsNullOrEmpty(unitOfMeasureID)) {
                throw new Exception("O código da unidade de medição não está preenchido.");
            }

            dsoCache.UnitOfMeasureProvider.Delete(unitOfMeasureID);
            editState = EditState.None;
        }

        public bool Validate(out string ErrorMessage) {

            bool result = true;
            StringBuilder errorMessage = new StringBuilder();

            if (string.IsNullOrEmpty(_unitOfMeasure.UnitOfMeasureID)) {
                errorMessage.AppendLine("O código da unidade de medição não está preenchido.");
            }
            else {
                var bUnitOfMeasureExists = dsoCache.UnitOfMeasureProvider.GetUnitOfMeasure(_unitOfMeasure.UnitOfMeasureID) != null ? true : false;
                if (editState == EditState.New && bUnitOfMeasureExists) {
                    errorMessage.AppendLine($"A unidade de medição [{_unitOfMeasure.UnitOfMeasureID}] já existe.");
                }
                if (editState == EditState.Editing && !bUnitOfMeasureExists) {
                    errorMessage.AppendLine($"A unidade de medição [{_unitOfMeasure.UnitOfMeasureID}] não existe.");
                }

                var maxIDSize = GetMaxFieldSize("UnitOfMeasure", "UnitOfMeasureID");
                if (_unitOfMeasure.UnitOfMeasureID.Length > maxIDSize) {
                    errorMessage.AppendLine($"O tamanho máximo do código é {maxIDSize}.");
                }
            }

            if (string.IsNullOrEmpty(_unitOfMeasure.Description)) {
                errorMessage.AppendLine("Tem de preencher a descrição da unidade de medição.");
            }

            if (errorMessage.Length != 0) {
                result = false;
            }

            ErrorMessage = errorMessage.ToString();
            return result;
        }

        public bool FillDefaultValues() {
            return true;
        }
    }
}
