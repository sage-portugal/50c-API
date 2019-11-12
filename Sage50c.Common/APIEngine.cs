using S50cData18;
using S50cDL18;
using S50cPrint18;
using S50cSys18;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sage50c.API {
    public static class APIEngine {
        public enum ApplicationEnum {
            None = 0,
            SageRetail,
            SageGC,
            Sage50c
        }

        public class MessageEventArgs : EventArgs {
            public string Prompt { get; set; }
            public MessageBoxButtons Buttons { get; set; }
            public MessageBoxDefaultButton DefaultButton { get; set; }
            public MessageBoxIcon Icon { get; set; }
            public string Title { get; set; }
            public DialogResult Result { get; set; }
        }

        public delegate void MessageEventHandler(MessageEventArgs Args);
        public delegate void WarningErrorEventHandler(int Number, string Source, string Description);
        public delegate void WarningMessageEventHandler(string Message);

        private static S50cData18.GlobalSettings s50cDataGlobals = null;
        private static S50cCore18.GlobalSettings s50cCoreGlobals = null;
        private static S50cSys18.GlobalSettings s50cSystemGlobals = null;
        private static S50cDL18.GlobalSettings s50cDLGlobals = null;
        private static S50cPrint18.GlobalSettings s50cPrintGlobals = null;
        private static S50cBL18.GlobalSettings s50cBLGlobals = null;
        //
        private static SystemManager s50cSystemManager = null;
        //
#if API
        private static DataManagerEventsClass dataManagerEvents = null;
        public static event WarningErrorEventHandler WarningError;
        public static event WarningMessageEventHandler WarningMessage;
        public static event MessageEventHandler Message;
#endif

        public static event EventHandler APIStarted;
        public static event EventHandler APIStopped;


        /// <summary>
        /// Retail System Settings
        /// </summary>
        public static SystemSettings SystemSettings { get { return s50cSystemGlobals.SystemSettings; } }
        /// <summary>
        /// Retail data providers cache
        /// </summary>
        public static DSOFactory DSOCache { get { return s50cDLGlobals.DSOCache; } }
        /// <summary>
        /// Retail Data manager for low level data access. Not Recommended to use freely
        /// </summary>
        public static DataManager DataManager { get { return s50cDataGlobals.DataManager; } }
        /// <summary>
        /// Retail low level Printing manager. Usage not recomended.
        /// </summary>
        public static PrintingManager PrintingManager { get { return s50cPrintGlobals.PrintingManager; } }
        /// <summary>
        /// Retail Federal Tax Id Validator
        /// </summary>
        public static S50cBL18.FederalTaxValidator FederalTaxValidator { get { return s50cBLGlobals.FederalTaxValidator; } }
        /// <summary>
        /// Tradutor
        /// </summary>
        public static S50cLocalize18._ILocalizer gLng { get { return s50cSystemGlobals.gLng; } }

        public static S50cCore18.GlobalSettings CoreGlobals { get { return s50cCoreGlobals; } }

        /// <summary>
        /// System manager
        /// </summary>
        private static SystemManager SystemManager {
            get {
                if (s50cSystemManager == null) {
                    s50cSystemManager = new SystemManager();
                    s50cSystemManager.Initialize();
                }
                return s50cSystemManager;
            }
        }


        private static bool apiInitialized = false;
        public static bool APIInitialized { get { return apiInitialized; } }

        //// Colocar SEMPRE ao nivel do módulo/class para não ser descarregado indevidamente
        //private static S50cAPICGCO18.SystemStarter systemStarter = null;

        /// <summary>basMain
        /// Inicializa a API da 50c
        /// Lança uma exceção se falhar
        /// </summary>
        /// <param name="companyId">Identificador da empresa a Abrir</param>
        public static void Initialize(string ProductCode, string CompanyId, bool DebugMode) {
            apiInitialized = false;

            //
            Terminate();
            //
            // Init
            // 1. DataProvider (S50cData18)
            // 2. System (S50cSys18)
            // 3. DataLayer (s50cData13)
            // 4. Core (S50cCore18)
            s50cDataGlobals = new S50cData18.GlobalSettings();
            s50cSystemGlobals = new S50cSys18.GlobalSettings();
            s50cDLGlobals = new S50cDL18.GlobalSettings();
            s50cCoreGlobals = new S50cCore18.GlobalSettings();
            s50cPrintGlobals = new S50cPrint18.GlobalSettings();
            s50cBLGlobals = new S50cBL18.GlobalSettings();
            //
            // NEW RECOMMENDED Startup - CGCO / CRTL
            var systemStarter = new S50cAPI18.SystemStarter();
            systemStarter.DebugMode = DebugMode;

            if (systemStarter.Initialize(ProductCode, CompanyId) != 0) {
                string initError = systemStarter.InitializationError;
                systemStarter = null;
                throw new Exception(initError);
            }
            // Eventos de erros e avisos vindos da API
#if API
            dataManagerEvents = (S50cData18.DataManagerEventsClass)s50cDataGlobals.DataManager.Events;
            dataManagerEvents.__DataManagerEvents_Event_WarningMessage += dataManagerEvents___DataManagerEvents_Event_WarningMessage;
            dataManagerEvents.__DataManagerEvents_Event_WarningError += dataManagerEvents___DataManagerEvents_Event_WarningError;
            dataManagerEvents.__DataManagerEvents_Event_Message += DataManagerEvents___DataManagerEvents_Event_Message;

        private static void DataManagerEvents___DataManagerEvents_Event_Message(string Prompt, int Flags, string Title, ref int result) {
            if (Message != null) {
                var args = new MessageEventArgs() {
                    Prompt = Prompt,
                    Result = DialogResult.None,
                    Title = Title,
                    DefaultButton = (MessageBoxDefaultButton)(Flags & 0xF00),
                    Buttons = (MessageBoxButtons)(Flags & 0xF),
                    Icon = (MessageBoxIcon)(Flags & 0xF0)
                };
                Message(args);
                result = (int)args.Result;
            }
        }

        /// <summary>
        /// Trata os eventos vindos da API e dispara um novo evento para ser tratado pelo .NET
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="Source"></param>
        /// <param name="Description"></param>
        static void dataManagerEvents___DataManagerEvents_Event_WarningError(int Number, string Source, string Description) {
            if (WarningError != null)
                WarningError(Number, Source, Description);
        }

        /// <summary>
        /// Trata os eventos vindos da API e dispara um novo evento para ser tratado pelo .NET
        /// </summary>
        /// <param name="MessageID"></param>
        /// <param name="MessageParams"></param>
        static void dataManagerEvents___DataManagerEvents_Event_WarningMessage(int MessageID, ref string[] MessageParams) {
            if (WarningMessage != null) {
                if (MessageID == 0) {
                    if (MessageParams.Length > 0) {
                        WarningMessage(string.Join(Environment.NewLine, MessageParams));
                    }
                }
                else {
                    string msg = s50cSystemGlobals.gLng.GS2(MessageID, ref MessageParams);
                    WarningMessage(msg);
                }
            }
        }
#endif
            //
            apiInitialized = true;
            //
            if (APIStarted != null)
                APIStarted(null, null);
        }



        public static QuickSearch CreateQuickSearch(QuickSearchViews QuickSearchId, bool CacheIt) {
            return s50cSystemGlobals.CreateQuickSearch(QuickSearchId, CacheIt);
        }


        public static CompanyList GetCompanyList() {
            return SystemManager.Companies;
        }

        private static S50cUtil18.StringFunctions _stringFunctions = null;
        public static S50cUtil18.StringFunctions StringFunctions {
            get {
                if (_stringFunctions == null) {
                    _stringFunctions = new S50cUtil18.StringFunctions();
                }
                return _stringFunctions;
            }
        }

        /// <summary>
        /// Termina a ligação à API e liberta todos os recursos
        /// </summary>
        public static void Terminate() {
            if (apiInitialized) {
                //1. QuickSearch
                if (s50cSystemGlobals != null) {
                    s50cSystemGlobals.DisposeQuickSearch();
                }
                //3. Business globals
                if (s50cBLGlobals != null) {
                    s50cBLGlobals.Dispose();
                    s50cBLGlobals = null;
                }
                //4. Dispose CORE Global Settings
                if (s50cCoreGlobals != null) {
                    s50cCoreGlobals.Dispose();
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(s50cCoreGlobals);
                    s50cCoreGlobals = null;
                }
                //2. Dispose Printing Manager
                if (s50cPrintGlobals != null) {
                    s50cPrintGlobals.Dispose();
                    s50cPrintGlobals = null;
                }
                //5. DISPOSE DataLayer Global Settings
                if (s50cDLGlobals != null) {
                    s50cDLGlobals.Dispose();
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(s50cDLGlobals);
                    s50cDLGlobals = null;
                }
                //6. Dispose DataProvider
                if (s50cDataGlobals != null) {
                    s50cDataGlobals.DataManager.CloseConnections();
                    s50cDataGlobals.Dispose();
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(s50cDataGlobals);
                    s50cDataGlobals = null;
                }
                //7. Dispose System
                if (s50cSystemGlobals != null) {
                    s50cSystemGlobals.Dispose();
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(s50cSystemGlobals);
                    s50cSystemGlobals = null;
                }
                // Dispose System manager
                if (s50cSystemManager != null) {
                    s50cSystemManager = null;
                }
                //
                apiInitialized = false;
                //
                // Fire event
                if (APIStopped != null)
                    APIStopped(null, null);
            }
        }
    }
}