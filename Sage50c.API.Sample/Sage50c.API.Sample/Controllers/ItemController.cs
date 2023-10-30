using System;
using System.Text;

using S50cBO22;
using S50cDL22;
using S50cSys22;

namespace Sage50c.API.Sample.Controllers {
    internal class ItemController {

        /// <summary>
        /// State of the item
        /// </summary>
        private EditState _editState = EditState.None;
        /// <summary>
        /// System parameters
        /// </summary>
        private SystemSettings _systemSettings { get { return APIEngine.SystemSettings; } }
        /// <summary>
        /// Cache for data engines for all common data
        /// </summary>
        private DSOFactory _dsoCache { get { return APIEngine.DSOCache; } }
        /// <summary>
        /// Current item data
        /// </summary>
        private Item _item = null;
        public Item Item { get { return _item; } }

        public void Create() {

            _item = new Item();

            // Set default taxable group
            _item.TaxableGroupID = _systemSettings.SystemInfo.ItemDefaultsSettings.DefaultTaxableGroupID;
            // Get the first available supplier
            _item.SupplierID = APIEngine.DSOCache.SupplierProvider.GetFirstSupplierEx();
            // Get the first available family
            double familyId = APIEngine.DSOCache.FamilyProvider.GetFirstLeafFamilyID();
            _item.Family = APIEngine.DSOCache.FamilyProvider.GetFamily(familyId);

            _editState = EditState.New;
        }

        public void Load(string itemID) {

            if (string.IsNullOrEmpty(itemID)) {
                throw new Exception("O código do artigo não está preenchido.");
            }

            _item = _dsoCache.ItemProvider.GetItem(itemID, _systemSettings.BaseCurrency);
            _editState = _item != null ? EditState.Editing : _editState;
        }

        public void Update() {

            DataValidation();

            _dsoCache.ItemProvider.Save(_item, _item.ItemID, _editState == EditState.New);
            _editState = EditState.Editing;
        }

        public void Remove(string itemID) {

            if (string.IsNullOrEmpty(itemID)) {
                throw new Exception("O código do artigo não está preenchido.");
            }

            _dsoCache.ItemProvider.Delete(itemID);
            _editState = EditState.None;
        }

        public void DataValidation() {

            if (string.IsNullOrEmpty(_item.ItemID)) {
                throw new Exception("O código do artigo não está preenchido.");
            }

            var bItemExists = _dsoCache.ItemProvider.ItemExist(_item.ItemID);

            if (_editState == EditState.New && bItemExists) {
                throw new Exception($"O artigo [{_item.ItemID}] já existe.");
            }
            if (_editState == EditState.Editing && !bItemExists) {
                throw new Exception($"O artigo [{_item.ItemID}] não existe.");
            }

            StringBuilder errorMessage = new StringBuilder();

            if (string.IsNullOrEmpty(_item.Description)) {
                errorMessage.Append($"Tem de preencher a descrição do artigo.{Environment.NewLine}");
            }
            if (string.IsNullOrEmpty(_item.ShortDescription)) {
                errorMessage.Append($"Tem de preencher a descrição abreviada do artigo.{Environment.NewLine}");
            }

            if (errorMessage.Length != 0) {
                throw new Exception(errorMessage.ToString());
            }
        }

        public int AddColor(int ColorID) {

            var color = APIEngine.DSOCache.ColorProvider.GetColor((short)ColorID);
            var newItemColor = new ItemColor() {
                ColorID = color.ColorID,
                ColorName = color.Description,
                ColorCode = (int)color.ColorCode,
                SequenceNumber = (short)(_item.Colors.Count + 1)
            };

            _item.Colors.Add(newItemColor);
            return _item.Colors.Count;
        }

        public int AddSize(int SizeID) {

            var size = APIEngine.DSOCache.SizeProvider.GetSize((short)SizeID);
            var newItemSize = new ItemSize() {
                SizeID = size.SizeID,
                SizeName = size.Description,
                Quantity = 1,
                Units = 1,
                SequenceNumber = (short)(_item.Sizes.Count + 1)
            };

            _item.Sizes.Add(newItemSize);
            return _item.Sizes.Count;
        }
    }
}