using S50cBL18;
using S50cSys18;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sage50c.ExtenderSample {
    internal class SystemHandler : IDisposable {
        // System events handler
        private ExtenderSystemEvents myEvents = null;
        //
        // General child windows interfaces
        private static IChildWindow generalChildWindow = null;
        private static IWorkspaceWindow generalWorkspaceWindow = null;
        private static IDialogWindow generalDialogWindow = null;

        public static IChildWindow GeneralChildWindow { get{ return generalChildWindow ;} }
        public static IWorkspaceWindow GeneralWorkspaceWindow { get { return generalWorkspaceWindow; } }
        public static IDialogWindow GeneralDialogWindow { get { return GeneralDialogWindow; } }

        public void SetEventHandler(ExtenderSystemEvents e) {
            myEvents = e;
            //
            // 1. Menu de utilizador
            myEvents.OnInitialize += SystemEvents_OnInitialize;
            myEvents.OnStartup += MyEvents_OnStartup;
            myEvents.OnMenuItem += SystemEvents_OnMenuItem;
            myEvents.OnDispose += MyEvents_OnDispose;
        }

        private void MyEvents_OnStartup(object Sender, ExtenderEventArgs e) {
            ExtendedPropertyList properties = null; ;
            ExtenderMenuItems       menuItem;

            //MyApp.SystemSettings.WorkstationInfo.Touch.CompanyLogoPosition = 1;

            properties = (ExtendedPropertyList)e.get_data();

            //this property will only be available in the backoffice
            if (properties.PropertyExists("ChildWindow") ) {
                generalChildWindow = (IChildWindow)properties.get_Value("ChildWindow");
            }
            //this property will only be available in the backoffice
            if (properties.PropertyExists("WorkspaceWindow") ) {
                generalWorkspaceWindow = (IWorkspaceWindow)properties.get_Value("WorkspaceWindow");
            }
            //this property will be available in both backoffice and frontoffice
            if (properties.PropertyExists("DialogWindow")) {
                generalDialogWindow = (IDialogWindow)properties.get_Value("DialogWindow");
            }

            
            // CUSTOM MENUS
            // Definir os menus
            menuItem = new ExtenderMenuItems();
            var childItems = menuItem.Add("miExtensibilidade", "&Extensibilidade").ChildItems;
            childItems.Add("miXItem1", "Item &1");
            childItems.Add("miXItem2", "Item &2");
            //
            // COM mandatories
            object oMenuItem = menuItem;
            properties.set_Value("ExtenderMenuItems", ref oMenuItem);

            //Use this property if you want Sage Retail to rebuild the permissions tree...
            //object rebuildPermissionsTree = true;
            //properties.set_Value("RebuildPermissionsTree", rebuildPermissionsTree);

            object oProps = properties;
            e.result.set_data(ref oProps);

            menuItem = null;
        }

        private void MyEvents_OnDispose() {
            //Clean up!
        }

        private void SystemEvents_OnMenuItem(object Sender, ExtenderEventArgs e) {
            string menuItemId = (string)e.get_data();
            switch ( menuItemId ) {
                case "miXItem1":
                    //System.Windows.Forms.MessageBox.Show("miXitem1");
                    var fItem =  new FormItem();
                    fItem.ShowWindow();
                    break;

                case "miXItem2":
                    System.Windows.Forms.MessageBox.Show("miXitem2");
                    break;
           }
        }

        private void SystemEvents_OnInitialize(object Sender, ExtenderEventArgs e) {
        }

        public void Dispose() {
            // House cleanup
        }
    }
}
