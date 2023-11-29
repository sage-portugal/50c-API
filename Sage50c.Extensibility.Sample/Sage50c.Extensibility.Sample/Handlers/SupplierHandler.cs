using S50cBL22;
using S50cBO22;
using S50cSys22;
using Sage50c.API;
using System;

namespace Sage50c.ExtenderSample22.Handlers {

    internal class SupplierHandler : IDisposable {

        private ExtenderEvents _extenderEvents = null;
        private IManagementConsole _managementConsole = null;

        public void SetEventHandler(ExtenderEvents e) {

            _extenderEvents = e;

            _extenderEvents.OnInitialize += myEvents_OnInitialize; // Initialize
            _extenderEvents.OnMenuItem += myEvents_OnMenuItem;     // Side menu clicks
            _extenderEvents.OnDispose += myEvents_OnDispose;       // Free resources

            _extenderEvents.OnNew += myEvents_OnNew;               // New
            _extenderEvents.OnLoad += myEvents_OnLoad;             // Load
            _extenderEvents.OnSave += myEvents_OnSave;             // Save
            _extenderEvents.OnDelete += myEvents_OnDelete;         // Delete
            _extenderEvents.OnValidating += myEvents_OnValidating; // Validate
        }

        public void myEvents_OnInitialize(object sender, ExtenderEventArgs e) {
            var propertyList = (ExtendedPropertyList)e.get_data();

            if (propertyList.PropertyExists("IManagementConsole")) {
                _managementConsole = (IManagementConsole)propertyList.get_Value("IManagementConsole");

                // Add an extra tab
                var formProps = new fSupplierExtra("New tab", "New tab description");
                _managementConsole.AddChildPanel(formProps);
            }

            e.result.Success = true;
            e.result.ResultMessage = string.Empty;
        }

        private void myEvents_OnMenuItem(object sender, ExtenderEventArgs e) {

            var menuId = (string)e.get_data();

            switch (menuId) {
                case "newItem":
                    System.Windows.Forms.MessageBox.Show("Pressionei o novo item!");
                    break;
            }
        }

        private void myEvents_OnDispose() { }

        public void Dispose() {
            _extenderEvents = null;
        }

        #region CRUD

        private void myEvents_OnNew(object sender, ExtenderEventArgs e) {
            APIEngine.CoreGlobals.MsgBoxFrontOffice("Novo", VBA.VbMsgBoxStyle.vbExclamation, "Extensibility");

            var supplier = (Supplier)e.get_data();
            supplier.AddressLine1 = "PLACEHOLDER";
            supplier.PostalCode = "4435-671";
        }

        private void myEvents_OnLoad(object sender, ExtenderEventArgs e) {
            APIEngine.CoreGlobals.MsgBoxFrontOffice("Carregar", VBA.VbMsgBoxStyle.vbExclamation, "Extensibility");

            var supplier = (Supplier)e.get_data();
        }

        private void myEvents_OnSave(object sender, ExtenderEventArgs e) {
            APIEngine.CoreGlobals.MsgBoxFrontOffice("Guardar", VBA.VbMsgBoxStyle.vbExclamation, "Extensibility");

            var propertyList = (ExtendedPropertyList)e.get_data();
            var bIsNew = (bool)propertyList.get_Value("IsNew");
        }

        private void myEvents_OnDelete(object sender, ExtenderEventArgs e) {
            APIEngine.CoreGlobals.MsgBoxFrontOffice("Remover", VBA.VbMsgBoxStyle.vbExclamation, "Extensibility");

            var supplier = (Supplier)e.get_data();
        }

        private void myEvents_OnValidating(object sender, ExtenderEventArgs e) {
            APIEngine.CoreGlobals.MsgBoxFrontOffice("Validar", VBA.VbMsgBoxStyle.vbExclamation, "Extensibility");

            var propertyList = (ExtendedPropertyList)e.get_data();
            var bForDeletion = (bool)propertyList.get_Value("ForDeletion");
        }

        #endregion
    }
}
