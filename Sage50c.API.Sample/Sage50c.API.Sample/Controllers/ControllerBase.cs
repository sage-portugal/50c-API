using S50cDL22;
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

        protected int GetMaxFieldSize(string TableName, string FieldName) {

            int maxSize = 0;

            try {
                maxSize = APIEngine.DataManager.MainProvider.Catalog.Table[TableName].Fields[FieldName].FieldSize;
            }
            catch { }

            return maxSize;
        }
    }
}
