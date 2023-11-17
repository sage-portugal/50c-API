using System;
using System.Text;

using S50cBO22;

namespace Sage50c.API.Sample.Controllers {
    internal class ItemController : ControllerBase {

        /// <summary>
        /// Item data
        /// </summary>
        private Item _item = null;
        public Item Item { get { return _item; } }

        public Item Create() {

            _item = new Item();

            FillDefaultValues();

            editState = EditState.New;
            return _item;
        }

        public Item Load(string ItemID) {

            if (string.IsNullOrEmpty(ItemID)) {
                throw new Exception("O código do artigo não está preenchido.");
            }

            _item = dsoCache.ItemProvider.GetItem(ItemID, systemSettings.BaseCurrency);
            editState = _item != null ? EditState.Editing : editState;
            return _item;
        }

        public bool Save() {

            string errorMessage = null;

            if (Validate(out errorMessage)) {
                dsoCache.ItemProvider.Save(_item, _item.ItemID, editState == EditState.New);
                editState = EditState.Editing;
            }
            else {
                throw new Exception(errorMessage);
            }

            return true;
        }

        public void Remove(string ItemID) {

            if (string.IsNullOrEmpty(ItemID)) {
                throw new Exception("O código do artigo não está preenchido.");
            }

            dsoCache.ItemProvider.Delete(ItemID);
            editState = EditState.None;
        }

        private bool Validate(out string ErrorMessage) {

            bool result = true;
            StringBuilder errorMessage = new StringBuilder();

            if (string.IsNullOrEmpty(_item.ItemID)) {
                errorMessage.AppendLine("O código do artigo não está preenchido.");
            }
            else {
                var bItemExists = dsoCache.ItemProvider.ItemExist(_item.ItemID);
                if (editState == EditState.New && bItemExists) {
                    errorMessage.AppendLine($"O artigo [{_item.ItemID}] já existe.");
                }
                if (editState == EditState.Editing && !bItemExists) {
                    errorMessage.AppendLine($"O artigo [{_item.ItemID}] não existe.");
                }
            }

            if (string.IsNullOrEmpty(_item.Description)) {
                errorMessage.AppendLine("Tem de preencher a descrição do artigo.");
            }

            if (errorMessage.Length != 0) {
                result = false;
            }

            ErrorMessage = errorMessage.ToString();
            return result;
        }

        private bool FillDefaultValues() {
            // Set default taxable group
            _item.TaxableGroupID = systemSettings.SystemInfo.ItemDefaultsSettings.DefaultTaxableGroupID;
            // Get the first available supplier
            _item.SupplierID = dsoCache.SupplierProvider.GetFirstSupplierEx();
            // Get the first available family
            double familyId = dsoCache.FamilyProvider.GetFirstLeafFamilyID();
            _item.Family = dsoCache.FamilyProvider.GetFamily(familyId);

            return true;
        }

        /// <summary>
        /// Erases all item's colors
        /// </summary>
        public void ClearItemColors() {
            _item.Colors.Clear();
        }

        /// <summary>
        /// Erases all item's sizes
        /// </summary>
        public void ClearItemSizes() {
            _item.Sizes.Clear();
        }

        /// <summary>
        /// Adds a new color to the item
        /// </summary>
        public int AddColor(int ColorID) {

            var color = dsoCache.ColorProvider.GetColor((short)ColorID);
            var newColor = new ItemColor() {
                ColorID = color.ColorID,
                ColorName = color.Description,
                ColorCode = (int)color.ColorCode,
                SequenceNumber = (short)(_item.Colors.Count + 1)
            };

            _item.Colors.Add(newColor);
            return _item.Colors.Count;
        }

        /// <summary>
        /// Adds a new size to the item
        /// </summary>
        public int AddSize(int SizeID) {

            var size = dsoCache.SizeProvider.GetSize((short)SizeID);
            var newSize = new ItemSize() {
                SizeID = size.SizeID,
                SizeName = size.Description,
                Quantity = 1,
                Units = 1,
                SequenceNumber = (short)(_item.Sizes.Count + 1)
            };

            _item.Sizes.Add(newSize);
            return _item.Sizes.Count;
        }
    }
}