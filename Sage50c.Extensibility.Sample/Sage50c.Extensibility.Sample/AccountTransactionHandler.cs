using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S50cBL22;
using S50cSys22;
using S50cBO22;
using System.Windows.Forms;
using Sage50c.API;

namespace Sage50c.ExtenderSample22 {
    class AccountTransactionHandler : IDisposable {
        private ExtenderEvents headerEvents = null;
        private ExtenderEvents detailEvents = null;

        private AccountTransactionManager transactionManager = null;
        private PropertyChangeNotifier propChangeNotifier = null;
        private AccountTransaction _transaction = null;


        /// <summary>
        /// Eventos disparados pelo Retail:
        /// OnInitialize:   Uma vez no arranque da aplicação
        /// OnNew:          Sempre que se inicializa uma nova linha
        /// OnValidating:   Ao validar uima linha. Pode ser cancelada a introdução da linha
        /// 
        /// Restantes eventos não são disparados.
        /// </summary>
        /// <param name="e"></param>
        public void SetDetailEventsHandler(ExtenderEvents e) {
            detailEvents = e;

            detailEvents.OnInitialize += DetailEvents_OnInitialize;
            detailEvents.OnValidating += DetailEvents_OnValidating;
            detailEvents.OnNew += DetailEvents_OnNew;
        }

        private void DetailEvents_OnDispose() {
            System.Windows.Forms.MessageBox.Show("DetailEvents_OnDispose", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DetailEvents_OnInitialize(object Sender, ExtenderEventArgs e) {
            //System.Windows.Forms.MessageBox.Show("DetailEvents_OnInitialize");
        }

        private void DetailEvents_OnNew(object Sender, ExtenderEventArgs e) {
            var detail = (ItemTransactionDetail)e.get_data();

            //detail.ItemID = "7up4";

            //e.result.ResultMessage = "Alterei a descrição de um artigo novo";
            //e.result.Success = true;
        }

        public void SetHeaderEventsHandler(ExtenderEvents e) {
            headerEvents = e;

            headerEvents.OnInitialize += HeaderEvents_OnInitialize;
            headerEvents.OnMenuItem += HeaderEvents_OnMenuItem;
            headerEvents.OnValidating += HeaderEvents_OnValidating;
            headerEvents.OnSave += HeaderEvents_OnSave;
            headerEvents.OnDelete += HeaderEvents_OnDelete;
            headerEvents.OnNew += HeaderEvents_OnNew;
            headerEvents.OnLoad += HeaderEvents_OnLoad;
            headerEvents.OnDispose += HeaderEvents_OnDispose;
        }

        private void HeaderEvents_OnDispose() {
            // Dispose your objects
        }

        private void HeaderEvents_OnLoad(object Sender, ExtenderEventArgs e) {
            var trans = (AccountTransaction)e.get_data();

            ///... Code here

            e.result.Success = true;
        }

        private void HeaderEvents_OnNew(object Sender, ExtenderEventArgs e) {
            _transaction = (AccountTransaction)e.get_data();
        }

        private void HeaderEvents_OnDelete(object Sender, ExtenderEventArgs e) {
            System.Windows.Forms.MessageBox.Show("HeaderEvents_OnDelete: Acabei de anular.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HeaderEvents_OnSave(object Sender, ExtenderEventArgs e) {
            System.Windows.Forms.MessageBox.Show("HeaderEvents_OnSave: Gravou.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HeaderEvents_OnValidating(object Sender, ExtenderEventArgs e) {
            var propList = (ExtendedPropertyList)e.get_data();
            var forDeletion = (bool)propList.get_Value("forDeletion");
            var transaction = (AccountTransaction) propList.get_Value("Data");

            if (!forDeletion) {
                e.result.ResultMessage = "HeaderEvents_OnValidating: Não pode anular documentos! Mas vou devolver TRUE para deixar anular.";
                e.result.Success = true;
            }
            else {
                e.result.ResultMessage = "HeaderEvents_OnValidating: Não pode gravar documentos! Mas vou devolver TRUE para deixar anular.";
                e.result.Success = true;
            }
        }

        void OnPropertyChanged(string PropertyID, ref object value, ref bool Cancel) {
            // HANDLE BSOItemTransaction PROPERTY CHANGES HERE

            Console.WriteLine("OnPropertyChanged {0}={1}; Cancel={2}", PropertyID, value, Cancel);
            //Cancel = false;
        }


        #region HEADER EVENTS


        /// <summary>
        /// Inicialização
        /// Podemos adicionar novas opções de menu aqui
        /// IN:
        ///     e.get_data(): ExtendedPropertyList
        ///     "PropertyChangeNotifier" = Evento que podemos subscrever para controlar quando uma propriedade é alterada
        ///     "TransactionManager" = BSOItemTransaction; Controlador da transação em curso
        /// 
        /// OUT:
        ///     result.Sucess: true para sinalizar sucesso e carregar novos menus; false para cancelar
        ///     result.ResultMessage: Ignorado
        ///     result.set_data( ExtenderMenuItems ): Items de menu a carregar 
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        void HeaderEvents_OnInitialize(object Sender, ExtenderEventArgs e) {
            var propList = (ExtendedPropertyList)e.get_data();
            propChangeNotifier = (PropertyChangeNotifier)propList.get_Value("PropertyChangeNotifier");
            propChangeNotifier.PropertyChanged += OnPropertyChanged;

            var transactionManager = (AccountTransactionManager)propList.get_Value("TransactionManager");
            
            // Colocar o caminho para o icone. 
            // Não usar os nomes de ficheiro da Sage em:
            //      TARGETDIR\Icons50c
            //      TARGETDIR\Images
            var myTargetDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            myTargetDir = System.IO.Path.Combine(myTargetDir, "Icons");

            var newMenus = new ExtenderMenuItems();
            //
            //Criar o grupo: Tab
            var mnuGroup = newMenus.Add("mniXCustomTools", "Custom Tools");
            //criar item1
            var mnuItem1 = mnuGroup.ChildItems.Add("mniXTrans1", "Custom Item 1");
            mnuItem1.GroupType = ExtenderGroupType.ExtenderGroupTypeExtraOptions;
            mnuItem1.PictureName = System.IO.Path.Combine(myTargetDir, "icon-save.ico");
            //mnuItem1.PictureName = System.IO.Path.Combine(myTargetDir, "icon-sample-01.png");

            //criar item2
            mnuItem1 = mnuGroup.ChildItems.Add("mniXTrans2", "Custom Item 2");
            mnuItem1.GroupType = ExtenderGroupType.ExtenderGroupTypeExtraOptions;
            mnuItem1.PictureName = System.IO.Path.Combine(myTargetDir, "icon-sample-02.png");

            object returnMenu = newMenus;
            e.result.set_data(returnMenu);

        }

        void HeaderEvents_OnMenuItem(object Sender, ExtenderEventArgs e) {
            var menuId = (string)e.get_data();
            var rnd = new Random();

            switch (menuId.ToUpper()) {
                case "MNIXTRANS1":
                    APIEngine.CoreGlobals.MsgBoxFrontOffice($"{menuId}: YAY... mniXTrans1", VBA.VbMsgBoxStyle.vbInformation, APIEngine.SystemSettings.Application.LongName);
                    break;
                case "XFUNCTIONA":
                    APIEngine.CoreGlobals.MsgBoxFrontOffice($"{menuId}: Your function here... Your function extra parameter: {e.ExtraData}", VBA.VbMsgBoxStyle.vbInformation, APIEngine.SystemSettings.Application.LongName);
                    break;

                case "MNIXTRANS2":
                    APIEngine.CoreGlobals.MsgBoxFrontOffice($"{menuId}: Your function here... Your function extra parameter: {e.ExtraData}", VBA.VbMsgBoxStyle.vbInformation, APIEngine.SystemSettings.Application.LongName);
                    break;
            }
        }

        #endregion


        /// <summary>
        /// EXEMPLO DE VALIDAÇÃO NA LINHA   
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        void DetailEvents_OnValidating(object Sender, ExtenderEventArgs e) {
            var properties = (ExtendedPropertyList)e.get_data();
            var transactionDetail = (AccountTransactionDetail)properties.get_Value("Data");
            string errorMessage = string.Empty;

            //TODO: Detail validation code HERE
            e.result.Success = true;
            e.result.ResultMessage = "Sucesso.";

            properties = null;
            transactionDetail = null;
        }


        /// <summary>
        /// Adiciona um detalhe (linha) à transação
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="itemId"></param>
        /// <param name="qty"></param>
        /// <param name="unitOfMeasureId"></param>
        /// <param name="unitPrice"></param>
        /// <param name="taxPercent"></param>
        /// <param name="whareHouseId"></param>

        public void Dispose() {
            headerEvents = null;
            detailEvents = null;
            if (transactionManager != null) {
                transactionManager = null;
            }
            propChangeNotifier = null;
        }
    }
}