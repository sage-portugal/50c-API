using S50cBL18;
using Sage50c.Extensibility.CustomerTab.Handlers.CustomerHandler;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Sage50c.Extensibility.CustomerTab {
    [ProgId("Sage50c.Extensibility.CustomerTab")]
    public class Extender : ISageExtender, IDisposable {
        private SystemHandler systemHandler = null;       // System handler, startup, system menus
        //private SystemInfoHandler systemInfoHandler = null;   // Parâmetros do sistema

        private CustomerHandler customerHandler = null;

        public string Initialize(string ApplicationKey) {
            //Do Nothing for now
            return string.Empty;
        }

        /// <summary>
        /// Event handlers SETUP
        /// </summary>
        /// <param name="EntityID"></param>
        /// <param name="EventHandler"></param>
        public void SetExtenderEventHandler(string EntityID, ExtenderEvents EventHandler) {
            switch (EntityID.ToLower()) {
                case "customer":    //Clientes
                    if (customerHandler == null) {
                        customerHandler = new CustomerHandler();
                        customerHandler.SetEventHandler(EventHandler);
                    }
                    break;


                //Parâmetros customizados
                case "systeminfo":
                    if (systemInfoHandler == null) {
                        systemInfoHandler = new SystemInfoHandler();
                    }
                    systemInfoHandler.SetEventHandler(EventHandler);
                    break;

            }
        }

        public object SetExtenderSystemEventHandler(ExtenderSystemEvents EventHandler) {
            if (systemHandler == null) {
                systemHandler = new SystemHandler();
            }
            systemHandler.SetEventHandler(EventHandler);
            return null;
        }

        public void Dispose() {
            if (systemHandler != null) {
                systemHandler.Dispose();
                systemHandler = null;
            }
            if (itemHandler != null) {
                itemHandler.Dispose();
                itemHandler = null;
            }
            //if (transactionHandler != null) {
            //    transactionHandler.Dispose();
            //    transactionHandler = null;
            //}
        }
    }
}