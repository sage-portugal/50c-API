using S50cData18;
using S50cDL18;
using S50cPrint18;
using System;

namespace Sage50c.API {
    public static partial class APIEngine {
        public enum ApplicationEnum {
            None = 0,
            SageRetail,
            SageGC,
            Sage50c
        }

#if WPF
        public class MessageEventArgs : EventArgs {
            public string Prompt { get; set; }
            public System.Windows.MessageBoxButton Buttons { get; set; }
            public int DefaultButton { get; set; }
            public System.Windows.MessageBoxImage Icon { get; set; }
            public string Title { get; set; }
            public System.Windows.MessageBoxResult Result { get; set; }
        }
#else
        public class MessageEventArgs : EventArgs {
            public string Prompt { get; set; }
            public System.Windows.Forms.MessageBoxButtons Buttons { get; set; }
            public System.Windows.Forms.MessageBoxDefaultButton DefaultButton { get; set; }
            public System.Windows.Forms.MessageBoxIcon Icon { get; set; }
            public string Title { get; set; }
            public System.Windows.Forms.DialogResult Result { get; set; }
        }
#endif

        public delegate void MessageEventHandler(MessageEventArgs Args);
        public delegate void WarningErrorEventHandler(int Number, string Source, string Description);
        public delegate void WarningMessageEventHandler(string Message);
        //
        // Local API globals
        private static S50cData18.GlobalSettings _s50cDataGlobals = null;
        private static S50cCore18.GlobalSettings _s50cCoreGlobals = null;
        private static S50cSys18.GlobalSettings _s50cSystemGlobals = null;
        private static S50cDL18.GlobalSettings _s50cDLGlobals = null;
        private static S50cPrint18.GlobalSettings _s50cPrintGlobals = null;
        private static S50cBL18.GlobalSettings _s50cBLGlobals = null;
        private static S50cSys18.SystemManager _s50cSystemManager = null;
        //
        //
        private static S50cData18.GlobalSettings s50cDataGlobals {
            get {
                if (_s50cDataGlobals == null) {
                    _s50cDataGlobals = new S50cData18.GlobalSettings();
                }
                return _s50cDataGlobals;
            }
        }
        //
        private static S50cCore18.GlobalSettings s50cCoreGlobals {
            get {
                if (_s50cCoreGlobals == null) {
                    _s50cCoreGlobals = new S50cCore18.GlobalSettings();
                }
                return _s50cCoreGlobals;
            }
        }
        //
        private static S50cSys18.GlobalSettings s50cSystemGlobals {
            get {
                if (_s50cSystemGlobals == null) {
                    _s50cSystemGlobals = new S50cSys18.GlobalSettings();
                }
                return _s50cSystemGlobals;
            }
        }

        private static S50cDL18.GlobalSettings s50cDLGlobals {
            get {
                if (_s50cDLGlobals == null) {
                    _s50cDLGlobals = new S50cDL18.GlobalSettings();
                }
                return _s50cDLGlobals;
            }
        }
        //
        private static S50cPrint18.GlobalSettings s50cPrintGlobals {
            get { 
                if(_s50cPrintGlobals == null) {
                    _s50cPrintGlobals = new S50cPrint18.GlobalSettings();
                }
                return _s50cPrintGlobals;
            }
        }
        //
        private static S50cBL18.GlobalSettings s50cBLGlobals {
            get { 
                if(_s50cBLGlobals == null) {
                    _s50cBLGlobals = new S50cBL18.GlobalSettings();
                }
                return _s50cBLGlobals;
            }
        }
        //
        /// <summary>
        /// System manager
        /// </summary>
        private static S50cSys18.SystemManager SystemManager {
            get {
                if (_s50cSystemManager == null) {
                    _s50cSystemManager = new S50cSys18.SystemManager();
                    _s50cSystemManager.Initialize();
                }
                return _s50cSystemManager;
            }
        }

        public static event EventHandler APIStarted;
        public static event EventHandler APIStopped;


        /// <summary>
        /// Retail System Settings
        /// </summary>
        public static S50cSys18.SystemSettings SystemSettings { get { return s50cSystemGlobals.SystemSettings; } }
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

        public static S50cSys18.QuickSearch CreateQuickSearch(S50cSys18.QuickSearchViews QuickSearchId, bool CacheIt) {
            return s50cSystemGlobals.CreateQuickSearch(QuickSearchId, CacheIt);
        }

        public static S50cSys18.CompanyList GetCompanyList() {
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

        public static S50cBL18.BSODiscountManager DiscountManager {
            get {
                return s50cBLGlobals.DiscountManager;
            }
        }
    }
}