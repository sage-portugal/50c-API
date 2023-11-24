using S50cBL22;
using Sage50c.Extensibility.CustomerTab.Handlers.CustomerHandler;
using Sage50c.Extensibility.CustomerTab.Handlers.SupplierHandler;
using Sage50c.Extensibility.CustomerTab.Handlers.SalesmanHandler;
using Sage50c.Extensibility.CustomerTab.Handlers.OtherContactHandler;
using Sage50c.Extensibility.CustomerTab.Handlers.SystemHandler;
using Sage50c.Extensibility.CustomerTab.Handlers.SaleTransactionHandler;
using Sage50c.Extensibility.CustomerTab.Handlers.BuyTransactionHandler;

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
        private SupplierHandler supplierHandler = null;
        private SalesmanHandler salesmanHandler = null;
        private OtherContactHandler otherContactHandler = null;
        private SaleTransactionHandler saleTransactionHandler = null;
        private BuyTransactionHandler buyTransactionHandler = null;

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
                case "saletransaction":
                    if (saleTransactionHandler == null)
                    {
                        saleTransactionHandler = new SaleTransactionHandler();
                        saleTransactionHandler.SetEventHandler(EventHandler);
                    }
                    break;

                case "buytransaction":
                    if (buyTransactionHandler == null)
                    {
                        buyTransactionHandler = new BuyTransactionHandler();
                        buyTransactionHandler.SetEventHandler(EventHandler);
                    }
                    break;

                case "customer":    //Clientes
                    if (customerHandler == null) {
                        customerHandler = new CustomerHandler();
                        customerHandler.SetEventHandler(EventHandler);
                    }
                    break;

                case "supplier":    //Fornecedores
                    if (supplierHandler == null) {
                        supplierHandler = new SupplierHandler();
                    }
                    supplierHandler.SetEventHandler(EventHandler);
                    break;

                case "salesman":    //Vendedores
                    if (salesmanHandler == null) {
                        salesmanHandler = new SalesmanHandler();
                    }
                    salesmanHandler.SetEventHandler(EventHandler);
                    break;
                    
                case "othercontact":    //Vendedores
                    if (otherContactHandler == null) {
                        otherContactHandler = new OtherContactHandler();
                    }
                    otherContactHandler.SetEventHandler(EventHandler);
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

            if (customerHandler != null) {
                customerHandler.Dispose();
                customerHandler = null;
            }

            if (supplierHandler != null) {
                supplierHandler.Dispose();
                supplierHandler = null;
            }

            if (salesmanHandler != null) {
                salesmanHandler.Dispose();
                salesmanHandler = null;
            }

            if (otherContactHandler != null) {
                otherContactHandler.Dispose();
                otherContactHandler = null;
            }

            if (saleTransactionHandler != null)
            {
                saleTransactionHandler.Dispose();
                saleTransactionHandler = null;
            }

            if (buyTransactionHandler != null)
            {
                buyTransactionHandler.Dispose();
                buyTransactionHandler = null;
            }
        }
    }
}