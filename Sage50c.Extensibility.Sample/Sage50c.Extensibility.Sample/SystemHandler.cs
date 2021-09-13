using S50cBL22;
using S50cSys22;
using Sage50c.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sage50c.ExtenderSample22 {
    internal class SystemHandler : IDisposable {



        // System events handler
        private ExtenderSystemEvents myEvents = null;
        //
        // General child windows interfaces
        private static IChildWindow generalChildWindow = null;

        private static IWorkspaceWindow generalWorkspaceWindow = null;
        private static IDialogWindow generalDialogWindow = null;
        private static IUITransaction generalUITransactionManager = null;
        private static IUIMasterTable generalUIMasterTableManager = null;
        private static IUIMenu generalUIMenuManager = null;

        public static IChildWindow GeneralChildWindow { get { return generalChildWindow; } }
        public static IWorkspaceWindow GeneralWorkspaceWindow { get { return generalWorkspaceWindow; } }
        public static IDialogWindow GeneralDialogWindow { get { return generalDialogWindow; } }
        public static IUITransaction GeneralUITransaction { get { return generalUITransactionManager; } }
        public static IUIMasterTable GeneralUIMasterTable { get { return generalUIMasterTableManager; } }
        public static IUIMenu GeneralUIMenu { get { return generalUIMenuManager; } }


        public void SetEventHandler(ExtenderSystemEvents e) {
            myEvents = e;
            //
            // 1. Menu de utilizador
            myEvents.OnInitialize += SystemEvents_OnInitialize;
            myEvents.OnStartup += MyEvents_OnStartup;
            myEvents.OnMenuItem += SystemEvents_OnMenuItem;
            myEvents.OnDispose += MyEvents_OnDispose;
        }

        #region Private methods

        /// <summary>
        /// Add name of your function
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="language"></param>
        private void AddMyFunction(string functionName, string language = "PTG") {
            var tmpFfunc = new S50cBO22.FuncPOS();
            tmpFfunc.POSFunctionID = functionName;
            tmpFfunc.LocalizedFunction = functionName;
            tmpFfunc.LanguageID = language;
            APIEngine.DSOCache.FuncPOSProvider.AddToCache(tmpFfunc);
        }
        #endregion 

        private void MyEvents_OnStartup(object Sender, ExtenderEventArgs e) {
            ExtendedPropertyList properties = null; ;
            ExtenderMenuItems menuItem;

            //APIEngine.SystemSettings.WorkstationInfo.Touch.CompanyLogoPosition = 1;

            properties = (ExtendedPropertyList)e.get_data();

            //this property will only be available in the backoffice
            if (properties.PropertyExists("ChildWindow")) {
                generalChildWindow = (IChildWindow)properties.get_Value("ChildWindow");
            }

            //this property will only be available in the backoffice
            if (properties.PropertyExists("WorkspaceWindow")) {
                generalWorkspaceWindow = (IWorkspaceWindow)properties.get_Value("WorkspaceWindow");
            }
            //this property will be available in both backoffice and frontoffice
            if (properties.PropertyExists("DialogWindow")) {
                generalDialogWindow = (IDialogWindow)properties.get_Value("DialogWindow");
            }

            //this property will be available in both backoffice and frontoffice
            if (properties.PropertyExists("UITransactionManager")) {
                generalUITransactionManager = (IUITransaction)properties.get_Value("UITransactionManager");
            }

            //this property will be available in both backoffice and frontoffice
            if (properties.PropertyExists("UIMasterTableManager")) {
                generalUIMasterTableManager = (IUIMasterTable)properties.get_Value("UIMasterTableManager");
            }

            if (properties.PropertyExists("UIMenuManager")) {
                generalUIMenuManager = (IUIMenu)properties.get_Value("UIMenuManager");
            }

            // CUSTOM MENUS
            // Definir os menus
            // Botão simples
            menuItem = new ExtenderMenuItems();
            var simpleButton = menuItem.Add("miSimpleButton", "Exemplo de janela");

            // Colocar o caminho para o icone. 
            // Não usar os nomes de ficheiro da Sage em:
            //      TARGETDIR\Icons50c
            //      TARGETDIR\Images
            var myTargetDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            myTargetDir = System.IO.Path.Combine(myTargetDir, "Icons");
            simpleButton.PictureName = System.IO.Path.Combine(myTargetDir, "icon-sample-01.png");
            var simpleButton2 = menuItem.Add("miSimpleButton1", "Exemplo 2 de janela");
            simpleButton2.PictureName = System.IO.Path.Combine(myTargetDir, "icon-sample-02.png");

            // Botão com submenu
            var parentButton = menuItem.Add("miComplexButton", "Grupo");
            var parentBtn1 = parentButton.ChildItems.Add("miComplexButtonItem1", "Clique aqui para ver uma mensagem do Grupo!");
            parentBtn1.PictureName = System.IO.Path.Combine(myTargetDir, "icon-sample-03.png");
            //menuItem.Add("miItemView", "Alterar Artigos");

            var parentButton2 = menuItem.Add("miComplexButton2", "Grupo");
            var child1 = parentButton2.ChildItems.Add("miComplexButtonItem2", "SubGrupo");
            var childItem1 = child1.ChildItems.Add("miComplexButtonItem3", "Clique aqui para ver uma mensagem do SubGrupo!", "miComplexButtonItem3");
            childItem1.PictureName = System.IO.Path.Combine(myTargetDir, "icon-sample-03.png");
            var childItem2 = child1.ChildItems.Add("miComplexButtonItem4", "Clique aqui para ver outra mensagem do SubGrupo!", "miComplexButtonItem3");
            childItem2.PictureName = System.IO.Path.Combine(myTargetDir, "icon-save.ico");

            // Custom Functions
            // Remember, all functions declared here will not recorded on physical base
            AddMyFunction("XFunctionA", "PTG");
            AddMyFunction("XPosDisplay", "PTG");

            //
            // COM mandatories
            object oMenuItem = menuItem;
            properties.set_Value("ExtenderMenuItems", ref oMenuItem);

            //Change RibbonTab Title
            object oRibbonCaption = "My caption";
            properties.set_Value("RibbonCaption", ref oRibbonCaption);

            //Use this property if you want Sage Retail to rebuild the permissions tree...
            //object rebuildPermissionsTree = true;
            //properties.set_Value("RebuildPermissionsTree", rebuildPermissionsTree);

            object oProps = properties;
            e.result.set_data(ref oProps);

            menuItem = null;
        }

        private void MyEvents_OnDispose() {
            //Clean up!
        }

        private void SystemEvents_OnMenuItem(object Sender, ExtenderEventArgs e) {

            string menuItemId = (string)e.get_data();
            switch (menuItemId) {
                case "miSimpleButton":
                    //System.Windows.Forms.MessageBox.Show("miXitem1");
                    var fItem = new FormItem();

                    fItem.ShowWindow();
                    break;

                case "miSimpleButton1":
                    //System.Windows.Forms.MessageBox.Show("Parabéns! Conseguiu implementar um botão simples", "Botão simples", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //break;
                    var fMenu = new FormCallMenu();

                    fMenu.ShowWindow();

                    break;

                case "miComplexButtonItem1":
                    System.Windows.Forms.MessageBox.Show("Parabéns! Conseguiu implementar um botão com Grupo", "Grupo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "miComplexButtonItem3":
                    System.Windows.Forms.MessageBox.Show("Parabéns! Conseguiu implementar um botão com Grupo e SubGrupo", "Sub Grupo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;

                case "miComplexButtonItem4":
                    System.Windows.Forms.MessageBox.Show("Parabéns! Conseguiu implementar um segundo botão com Grupo e SubGrupo", "Sub Grupo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }

        private void SystemEvents_OnInitialize(object Sender, ExtenderEventArgs e) {
        }

        public void Dispose() {
            // House cleanup
        }
    }
}
