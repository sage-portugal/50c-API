using S50cData22;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sage50c.API {
    public static partial class APIEngine {
        private static DataManagerEventsClass dataManagerEvents = null;
        public static event WarningErrorEventHandler WarningError;
        public static event WarningMessageEventHandler WarningMessage;
        public static event MessageEventHandler Message;

        private static bool apiInitialized = false;
        public static bool APIInitialized {
            get {
                return apiInitialized;
            }
        }
        //// Colocar SEMPRE ao nivel do módulo/class para não ser descarregado indevidamente
        //private static S50cAPICGCO22.SystemStarter systemStarter = null;

        /// <summary>
        /// Inicializa a API da 50c. Lança uma exceção se falhar
        /// </summary>
        /// <param name="companyId">Identificador da empresa a Abrir</param>
        public static void Initialize(string ProductCode, string CompanyId, bool DebugMode) {
            apiInitialized = false;

            //
            Terminate();
            //
            // NEW RECOMMENDED Startup - CGCO / CRTL
            var systemStarter = new S50cAPI22.SystemStarter();
            systemStarter.DebugMode = DebugMode;

            if (systemStarter.Initialize(ProductCode, CompanyId) != 0) {
                string initError = systemStarter.InitializationError;
                systemStarter = null;
                throw new Exception(initError);
            }
            // Eventos de erros e avisos vindos da API
            dataManagerEvents = (S50cData22.DataManagerEventsClass)s50cDataGlobals.DataManager.Events;
            dataManagerEvents.__DataManagerEvents_Event_WarningMessage += dataManagerEvents___DataManagerEvents_Event_WarningMessage;
            dataManagerEvents.__DataManagerEvents_Event_WarningError += dataManagerEvents___DataManagerEvents_Event_WarningError;
            dataManagerEvents.__DataManagerEvents_Event_Message += DataManagerEvents___DataManagerEvents_Event_Message;

            apiInitialized = true;
            //
            if (APIStarted != null) {
                APIStarted(null, null);
            }
        }

#if WPF
        private static void DataManagerEvents___DataManagerEvents_Event_Message(string Prompt, int Flags, string Title, ref int result) {
            if (Message != null) {
                var args = new MessageEventArgs() {
                    Prompt = Prompt,
                    Result = System.Windows.MessageBoxResult.None,
                    Title = Title,
                    DefaultButton = ((Flags & 0xF00)>>8),
                    Buttons = (System.Windows.MessageBoxButton)(Flags & 0xF),
                    Icon = (System.Windows.MessageBoxImage)(Flags & 0xF0)
                };
                Message(args);
                result = (int)args.Result;
            }
        }

#else
        private static void DataManagerEvents___DataManagerEvents_Event_Message(string Prompt, int Flags, string Title, ref int result) {
            if (Message != null) {
                var args = new MessageEventArgs() {
                    Prompt = Prompt,
                    Result = System.Windows.Forms.DialogResult.None,
                    Title = Title,
                    DefaultButton = (System.Windows.Forms.MessageBoxDefaultButton)(Flags & 0xF00),
                    Buttons = (System.Windows.Forms.MessageBoxButtons)(Flags & 0xF),
                    Icon = (System.Windows.Forms.MessageBoxIcon)(Flags & 0xF0)
                };
                Message(args);
                result = (int)args.Result;
            }
        }
#endif

        /// <summary>
        /// Trata os eventos vindos da API e dispara um novo evento para ser tratado pelo .NET
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="Source"></param>
        /// <param name="Description"></param>
        static void dataManagerEvents___DataManagerEvents_Event_WarningError(int Number, string Source, string Description) {
            if (WarningError != null) {
                WarningError(Number, Source, Description);
            }
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

        /// <summary>
        /// Termina a ligação à API e liberta todos os recursos
        /// </summary>
        public static void Terminate() {
            if (apiInitialized) {
                //1. QuickSearch
                if (_s50cSystemGlobals != null) {
                    _s50cSystemGlobals.DisposeQuickSearch();
                }
                //3. Business globals
                if (_s50cBLGlobals != null) {
                    _s50cBLGlobals.Dispose();
                    _s50cBLGlobals = null;
                }
                //4. Dispose CORE Global Settings
                if (_s50cCoreGlobals != null) {
                    _s50cCoreGlobals.Dispose();
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(s50cCoreGlobals);
                    _s50cCoreGlobals = null;
                }
                //2. Dispose Printing Manager
                if (_s50cPrintGlobals != null) {
                    _s50cPrintGlobals.Dispose();
                    _s50cPrintGlobals = null;
                }
                //5. DISPOSE DataLayer Global Settings
                if (_s50cDLGlobals != null) {
                    _s50cDLGlobals.Dispose();
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(s50cDLGlobals);
                    _s50cDLGlobals = null;
                }
                //6. Dispose DataProvider
                if (_s50cDataGlobals != null) {
                    _s50cDataGlobals.DataManager.CloseConnections();
                    _s50cDataGlobals.Dispose();
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(s50cDataGlobals);
                    _s50cDataGlobals = null;
                }
                //7. Dispose System
                if (_s50cSystemGlobals != null) {
                    _s50cSystemGlobals.Dispose();
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(s50cSystemGlobals);
                    _s50cSystemGlobals = null;
                }
                // Dispose System manager
                if (_s50cSystemManager != null) {
                    _s50cSystemManager = null;
                }
                //
                apiInitialized = false;
                //
                // Fire event
                if (APIStopped != null) {
                    APIStopped(null, null);
                }
            }
        }
    }
}
