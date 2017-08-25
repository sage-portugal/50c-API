using S50cBL18;
using S50cSys18;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sage50c.ExtenderSample {
    internal class SystemInfoHandler : IDisposable {
        private IManagementConsole managementConsole = null;   //Consola de gestão dos parâmetros
        private ExtenderEvents myEvents = null;
        private FormProps formProps = null;                     //Form das propriedades

        public void SetEventHandler( ExtenderEvents e) {
            myEvents = e;
            //Events
            myEvents.OnDispose += MyEvents_OnDispose;
            myEvents.OnInitialize += MyEvents_OnInitialize;
            myEvents.OnSave += MyEvents_OnSave;
            myEvents.OnValidating += MyEvents_OnValidating;
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
            if (formProps != null) {
                formProps.Save();
            }
        }

        private void MyEvents_OnValidating(object Sender, ExtenderEventArgs e) {
            //Validate properties
            if (formProps != null) {
                formProps.Validate(e.result);
            }
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
