using S50cBL22;
using Sage50c.ExtenderSample22.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Sage50c.ExtenderSample22 {
    [ProgId("Sage50c.ExtenderSample22")]
    public class Extender : ISageExtender, IDisposable {
        private SystemHandler systemHandler = null;                   // System handler, startup, system menus
        private SystemInfoHandler systemInfoHandler = null;           // Parâmetros do sistema
        private WorkstationInfoHandler workstationInfoHandler = null; // Configuração de postos

        private AccountTransactionHandler accountTransactionHandler = null; //AccountTransactionHandler
        private TransactionHandler transactionHandler = null;               // Sales Transaction handler
        private TransactionHandler buyTransactionHandler = null;            // Purchases Transaction handler
        private TenderTransactionHandler tenderTransactionHandler = null;   // Tender Transaction handler
        private StockHandler stockHandler = null;                           // StockTransaction handler
        private ItemHandler itemHandler = null;                             // Items
        private CustomerHandler customerHandler = null;                     // Customer
        private SupplierHandler supplierHandler = null;                     // Supplier
        private SalesmanHandler salesmanHandler = null;                     //Salesman

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
            // Just to enum all EntityIDs....
            Debug.WriteLine($"Requesting handler for... {EntityID}");

            switch (EntityID.ToLower()) {
                case "item":
                    if (itemHandler == null) {
                        itemHandler = new ItemHandler();
                        itemHandler.SetEventHandler(EventHandler);
                    }
                    Debug.WriteLine($"Done.");
                    break;

                case "customer":    //Clientes
                    if (customerHandler == null) {
                        customerHandler = new CustomerHandler();
                        customerHandler.SetEventHandler(EventHandler);
                    }
                    break;

                case "customeraccounttransaction":
                case "supplieraccounttransaction":
                    if (accountTransactionHandler == null) {
                        accountTransactionHandler = new AccountTransactionHandler();
                    }
                    accountTransactionHandler.SetHeaderEventsHandler(EventHandler);
                    break;

                case "buytransaction":  //Compras
                    if (buyTransactionHandler == null) {
                        buyTransactionHandler = new TransactionHandler();
                    }
                    buyTransactionHandler.SetHeaderEventsHandler(EventHandler);
                    break;

                case "buytransactiondetail":  //Compras (detalhes)
                    if (buyTransactionHandler == null) {
                        buyTransactionHandler = new TransactionHandler();
                    }
                    buyTransactionHandler.SetDetailEventsHandler(EventHandler);
                    break;

                case "saletransaction":
                    if (transactionHandler == null) {
                        transactionHandler = new TransactionHandler();
                    }
                    transactionHandler.SetHeaderEventsHandler(EventHandler);
                    break;

                case "saletransactiondetail":
                    if (transactionHandler == null) {
                        transactionHandler = new TransactionHandler();
                    }
                    transactionHandler.SetDetailEventsHandler(EventHandler);
                    break;

                case "stocktransaction":
                    if (stockHandler == null) {
                        stockHandler = new StockHandler();
                    }
                    stockHandler.SetHeaderEventsHandler(EventHandler);
                    break;

                case "stocktransactiondetail":
                    if (stockHandler == null) {
                        stockHandler = new StockHandler();
                    }
                    stockHandler.SetDetailEventsHandler(EventHandler);
                    break;

                case "tendertransaction":
                    if (tenderTransactionHandler == null) {
                        tenderTransactionHandler = new TenderTransactionHandler();
                    }
                    tenderTransactionHandler.SetHeaderEventsHandler(EventHandler);
                    break;

                case "supplier":
                    if (supplierHandler == null) {
                        supplierHandler = new SupplierHandler();
                    }
                    supplierHandler.SetEventHandler(EventHandler);
                    break;

                //case "confstores":  // delegações
                //    break;

                //Parâmetros customizados
                case "systeminfo":
                    if (systemInfoHandler == null) {
                        systemInfoHandler = new SystemInfoHandler();
                    }
                    systemInfoHandler.SetEventHandler(EventHandler);
                    break;

                // Configuração de postos
                case "workstationinfo":
                    if (workstationInfoHandler == null) {
                        workstationInfoHandler = new WorkstationInfoHandler();
                    }
                    workstationInfoHandler.SetEventHandler(EventHandler);
                    break;

                //Vendedor
                case "salesman":
                    if (salesmanHandler == null) {
                        salesmanHandler = new SalesmanHandler();
                    }
                    salesmanHandler.SetEventHandler(EventHandler);
                    
                    break;

                default:
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
