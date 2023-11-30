using S50cBL22;
using S50cBO22;
using S50cSys22;
using Sage50c.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VBA;

namespace Sage50c.ExtenderSample22 {
    internal class SalesmanHandler {
        private ExtenderEvents myEvents = null;
        private IManagementConsole managementConsole = null;   //Consola de gestão dos parâmetros
        private FormSalesman formSalesman = null;
        public void SetEventHandler(ExtenderEvents e) {
            myEvents = e;

            myEvents.OnDelete += myEvents_OnDelete;         // Delete  Salesman
            myEvents.OnDispose += myEvents_OnDispose;       // Limpar recursos
            myEvents.OnInitialize += myEvents_OnInitialize; // Inicializar, adicionar menus de utilizador
            myEvents.OnLoad += myEvents_OnLoad;             // Ao carregar um salesman e preencher o form. Pode ser cancelado
            myEvents.OnMenuItem += myEvents_OnMenuItem;     // Menu do utilizador foi pressionado
            myEvents.OnNew += myEvents_OnNew;               // Novo  Salesman
            myEvents.OnSave += myEvents_OnSave;             // Gravar Salesman
            myEvents.OnValidating += myEvents_OnValidating; // Validar. Pode ser cancelado.

        }

        /// <summary>
        /// Chamado no momento da gravação
        /// </summary>
        /// <param name="Sender">GenericExtensibilityController</param>
        /// <param name="e">
        /// IN:
        ///     e.get_data(): ExtendedPropertyList
        ///         "Data":  Customer
        ///         "PreviousID": Identificador anterior (ItemId). Pode não estar presente
        ///         "IsNew": O Artigo é novo
        ///    
        /// OUT:
        ///     result.Sucess: Ignorado
        ///     result.ResultMessage: Mensagem a apresentar
        /// </param>
        void myEvents_OnSave(object Sender, ExtenderEventArgs e) {
            var proplist = (ExtendedPropertyList)e.get_data();
            var Salesman = (Customer)proplist.get_Value("Data");    // The  Salesman
            var isNew = (bool)proplist.get_Value("IsNew");  // Is new?
        }

        /// <summary>
        /// Chamado se o utilizador utililzar um dos menus extendidos, passados no Initialize
        /// </summary>
        /// <param name="Sender">GenericExtensibilityController</param>
        /// <param name="e">
        /// IN:
        ///     e.get_data(): Id do menu (string)
        ///     
        /// OUT:
        ///     Ignorado
        /// </param>
        void myEvents_OnMenuItem(object Sender, ExtenderEventArgs e) {
            var menuId = (string)e.get_data();

            switch (menuId) {
                case "mniXSalesman1":
                    System.Windows.Forms.MessageBox.Show("Pressionei  Salesman 1");
                    break;

                case "mniXSalesman2":
                    System.Windows.Forms.MessageBox.Show("Pressionei  Salesman 2");
                    break;
            }
        }

        /// <summary>
        /// Chamado quando um salesman é carregado da base de dados
        /// </summary>
        /// <param name="Sender">GenericExtensibilityController</param>
        /// <param name="e">
        /// IN:
        ///     e.get_data():  Salesman
        ///     
        /// OUT:
        ///     Sucess: true or false
        ///     ResultMessage: caso preenchida, apresenta a mensagem
        /// </param>
        void myEvents_OnLoad(object Sender, ExtenderEventArgs e) {
            var Salesman = (Salesman)e.get_data();
            
            formSalesman.UpdateNumSales(Salesman.SalesmanID);

            // Salesman.Description = "My description";

            //e.result.Success = false;
            //e.result.ResultMessage = "A descrição foi alterada.";
        }


        /// <summary>
        /// Inicializa a extensão nos Salesman
        /// Não mostra mensagens
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e">
        /// IN:
        /// PropertyList:
        ///     "Window": Form, 
        ///     "IManagementConsole": IManagementConsole
        /// 
        /// OUT:
        ///     e.result:     true: para extender os menus, e sinalizar sucesso
        ///     e.set_data(): Passar um ExtenderMenuItems para extender os menus
        ///     
        /// Não mostra mensagens
        ///</param>
        void myEvents_OnInitialize(object Sender, ExtenderEventArgs e) {
            var propertyList = (ExtendedPropertyList)e.get_data();

            if (propertyList.PropertyExists("IManagementConsole")) {
                managementConsole = (IManagementConsole)propertyList.get_Value("IManagementConsole");

                // Form a colocar no TAB dos clientes
                formSalesman = new FormSalesman();
                managementConsole.AddChildPanel(formSalesman);
            }
            // Acrescentar Salesman ao menu
            var newMenu = new ExtenderMenuItems();

            var menuGroup = newMenu.Add("mniXFormacao", "Formação X");
            menuGroup.GroupType = ExtenderGroupType.ExtenderGroupTypeExtraOptions;   //Opções de menu
            menuGroup.BeginGroup = true;                                             //Novo grupo
            //
            var menuItem = menuGroup.ChildItems.Add("mniXSalesman1", "Meu menu 1");
            menuItem.GroupType = ExtenderGroupType.ExtenderGroupTypeExtraOptions;   //Opções de menu

            menuItem = menuGroup.ChildItems.Add("mniXSalesman2", "Meu menu 2");
            menuItem.GroupType = ExtenderGroupType.ExtenderGroupTypeExtraOptions;   //Opções de menu

            object oMenu = newMenu;
            e.result.set_data(ref oMenu);

            e.result.Success = true;
            e.result.ResultMessage = string.Empty;
        }

        /// <summary>
        /// Ocorre ao fechar o Form dos salemans.
        /// Serve para fazer a limpeza de recursos que já não sejam necessários.
        /// Não é possivel cancelar
        /// Não mostra mensagens
        /// </summary>
        void myEvents_OnDispose() {
        }

        /// <summary>
        /// Chamado quando o salesman vai ser eliminado
        /// </summary>
        /// <param name="Sender">GenericExtensibilityController</param>
        /// <param name="e">e.get_data():  Customer</param>
        void myEvents_OnDelete(object Sender, ExtenderEventArgs e) {
        }

        /// <summary>
        /// Ocorre ao criar um artigo novo
        /// IN:
        ///     e.get_data():  Salesman a ser criado. Pode ser alterado
        /// 
        /// OUT:
        ///     e.result.ResultMessage: Mensagem a apresentar ao utilizador. Se vazia, não mostra nada
        ///     e.result.Success: devolver true para continuar; false cancela a operação
        /// </summary>
        /// <param name="Sender">ExtensibilityController</param>
        /// <param name="e">Event parameters</param>
        void myEvents_OnNew(object Sender, ExtenderEventArgs e) {
            var salesman = (Salesman)e.get_data();

            var extraFields = APIEngine.DSOCache.ConfExtraFieldsProvider.GetConfExtraFieldList("Salesman");
            foreach (ConfExtraFields extraField in extraFields) {
                salesman.PartyInfo.ExtraFields.Add(new ExtraField() {
                    ExtraFieldID = (int)extraField.ExtraFieldID
                });
            }

            e.result.ResultMessage = "New Event: Estou a criar um vendedor novo";
            e.result.Success = true;
        }

        /// <summary>
        /// Chamado na validação do artigo, antes de gravar
        /// </summary>
        /// <param name="Sender">GenericExtensibilityController</param>
        /// <param name="e">
        /// IN:
        ///  e.get_data(): ExtendedPropertyList
        ///     "Data":  Salesman,
        ///     "ForDeletion": bool que indica se o  Salesman vai ser apagado
        ///
        /// OUT:
        ///     result.Success: true para continuar; false para falhar a validação
        ///     result.ResultMessage: Mensagem a apresentar
        /// </param>
        void myEvents_OnValidating(object Sender, ExtenderEventArgs e) {
            var proplist = (ExtendedPropertyList)e.get_data();
            var Salesman = (Salesman)proplist.get_Value("Data");
            var forDeletion = (bool)proplist.get_Value("ForDeletion");

            e.result.Success = true;

            var extraFields = APIEngine.DSOCache.ConfExtraFieldsProvider.GetConfExtraFieldList("Salesman");
            foreach (ConfExtraFields extraField in extraFields) {
                if (Salesman.PartyInfo.ExtraFields.Find((int)extraField.ExtraFieldID) == null) {
                    e.result.Success = false;
                    e.result.ResultMessage = string.Format("Validate Event: Campo extra {0} não está preenchido", extraField.Description);
                    break;
                }
            }


            //var extraField = Salesman.PartyInfo.ExtraFields.Find(2);
            //if (extraField == null) {
            //    extraField = new ExtraField();
            //    extraField.PartyID = Customer.PartyID;
            //    var confExtraField = APIEngine.DSOCache.ConfExtraFieldsProvider.GetConfExtraField(2);
            //    extraField.ExtraFieldID = (int)confExtraField.ExtraFieldID;

            //    Salesman.PartyInfo.ExtraFields.Add(extraField);
            //}
            //extraField.TextAnswer = "Informático";

            //if (string.IsNullOrEmpty( Customer.AddressLine1 )) {
            //    e.result.ResultMessage = "P.f. preencha a linha da morada";
            //    e.result.Success = false;
            //}
            //else {
            //    e.result.Success = true;
            //}
        }

        public void Dispose() {
            myEvents = null;
            // House cleanup
        }
    }
}
