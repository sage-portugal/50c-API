using S50cBL22;
using S50cSys22;
using Sage50c.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sage50c.ExtenderSample22 {
    internal class WorkstationInfoHandler : IDisposable {
        private IManagementConsole managementConsole = null;   //Consola de gestão dos parâmetros
        private ExtenderEvents myEvents = null;
        private FormProps formProps = null;                     //Form das propriedades

        public void SetEventHandler( ExtenderEvents e) {
            myEvents = e;
            //Events
            
            myEvents.OnInitialize += MyEvents_OnInitialize;
            myEvents.OnSave += MyEvents_OnSave;
            myEvents.OnValidating += MyEvents_OnValidating;
            myEvents.OnDispose += MyEvents_OnDispose;
        }
        
        private void MyEvents_OnInitialize(object Sender, ExtenderEventArgs e) {
            ExtendedPropertyList propList = null;
            propList = (ExtendedPropertyList)e.get_data();


            if (propList.PropertyExists("IManagementConsole")) {
                managementConsole = (IManagementConsole)propList.get_Value("IManagementConsole");

                formProps = new FormProps();
                managementConsole.AddChildPanel(formProps);
            }
        }

        private void MyEvents_OnDispose() {
            if (formProps != null) {
                formProps.Dispose();
                formProps = null;
            }
        }


        private void MyEvents_OnSave(object Sender, ExtenderEventArgs e) {
            //Save custom params
            APIEngine.CoreGlobals.MsgBoxFrontOffice("Implement your SAVE here!", VBA.VbMsgBoxStyle.vbInformation, APIEngine.SystemSettings.Application.LongName);
        }

        private void MyEvents_OnValidating(object Sender, ExtenderEventArgs e) {
            //Validate properties
            //APIEngine.CoreGlobals.MsgBoxFrontOffice("Implement your VALIDATE here!", VBA.VbMsgBoxStyle.vbInformation, APIEngine.SystemSettings.Application.LongName);
            e.result.Success = true;
            e.result.ResultMessage = "Implement your VALIDATE here!";
        }

        public void Dispose() {
            myEvents = null;
            if (formProps != null) {
                formProps.Dispose();
                formProps = null;
            }
        }
    }
}
