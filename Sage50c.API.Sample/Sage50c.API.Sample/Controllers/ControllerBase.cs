using S50cDL22;
using S50cPrint22;
using S50cSys22;

namespace Sage50c.API.Sample.Controllers {

    internal enum EditState {
        None = 0,
        New = 1,
        Editing = 2,
    }

    internal abstract class ControllerBase {

        /// <summary>
        /// State of the item
        /// </summary>
        protected EditState editState = EditState.None;
        /// <summary>
        /// Cache for data engines for all common data
        /// </summary>
        protected DSOFactory dsoCache { get { return APIEngine.DSOCache; } }
        /// <summary>
        /// System parameters
        /// </summary>
        protected SystemSettings systemSettings { get { return APIEngine.SystemSettings; } }
        /// <summary>
        /// 
        /// </summary>
        protected PrintingManager printingManager { get { return APIEngine.PrintingManager; } }

        protected int GetMaxFieldSize(string TableName, string FieldName) {

            int maxSize = 0;

            try {
                maxSize = APIEngine.DataManager.MainProvider.Catalog.Table[TableName].Fields[FieldName].FieldSize;
            }
            catch { }

            return maxSize;
        }

        protected static DocumentTypeEnum TransGetType(string DocumentId) {
            DocumentTypeEnum transType = DocumentTypeEnum.dcTypeNone;
            string transDoc = DocumentId;

            if (APIEngine.SystemSettings.WorkstationInfo.Document.IsInCollection(transDoc)) {
                var doc = APIEngine.SystemSettings.WorkstationInfo.Document[transDoc];
                transType = doc.TransDocType;
            }
            return transType;
        }

        protected static PartyTypeEnum TransGetPartyType(int cmbTransPartyTypeSelectedIndex) {
            switch (cmbTransPartyTypeSelectedIndex) {
                case 0: return PartyTypeEnum.ptSupplier;
                case 1: return PartyTypeEnum.ptCustomer;
                case 2: return PartyTypeEnum.ptNothing;

                default: return PartyTypeEnum.ptNothing;
            }
        }
    }
}
