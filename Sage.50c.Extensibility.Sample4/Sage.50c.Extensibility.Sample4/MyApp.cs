using S50cDL18;
using S50cSys18;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sage50c.ExtenderSample {
    public static class MyApp {
        #region Global multi Uses
        private static S50cSys18.GlobalSettings _rtlSysGlobalSettings = null;
        private static S50cData18.GlobalSettings _rtlDataGlobalSettings = null;
        private static S50cDL18.GlobalSettings _rtlDLGlobalSettings = null;
        private static S50cBL18.GlobalSettings _rtlBLGlobals = null;
        
        private static S50cSys18.GlobalSettings rtlGlobalSettings {
            get {
                if( _rtlSysGlobalSettings == null) {
                    _rtlSysGlobalSettings = new S50cSys18.GlobalSettings();
                }
                return _rtlSysGlobalSettings;
            }
        }

        private static S50cBL18.GlobalSettings rtlBLGlobals {
            get {
                if (_rtlBLGlobals == null) {
                    _rtlBLGlobals = new S50cBL18.GlobalSettings();
                }
                return _rtlBLGlobals;
            }
        }


        public static SystemSettings SystemSettings {
            get {
                return rtlGlobalSettings.SystemSettings;
            }
        }
        public static S50cData18.DataManager DataManager {
            get {
                if (_rtlDataGlobalSettings == null) {
                    _rtlDataGlobalSettings = new S50cData18.GlobalSettings();
                }
                return _rtlDataGlobalSettings.DataManager;
            }
        }
        public static DSOFactory DSOCache {
            get {
                if (_rtlDLGlobalSettings == null) {
                    _rtlDLGlobalSettings = new S50cDL18.GlobalSettings();
                }
                return _rtlDLGlobalSettings.DSOCache;
            }
        }
        /// <summary>
        /// Retail Federal Tax Id Validator
        /// </summary>
        public static S50cBL18.FederalTaxValidator FederalTaxValidator { get { return rtlBLGlobals.FederalTaxValidator; } }
        /// <summary>
        /// Tradutor
        /// </summary>
        public static S50cLocalize18._ILocalizer gLng { get { return rtlGlobalSettings.gLng; } }

        public static QuickSearch CreateQuickSearch(QuickSearchViews QuickSearchId, bool CacheIt) {
            return _rtlSysGlobalSettings.CreateQuickSearch(QuickSearchId, CacheIt);
        }
        #endregion
    }
}
