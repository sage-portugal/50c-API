using S50cBO22;
using S50cBL22;
using S50cSys22;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sage50c.API;

namespace Sage50c.Extensibility.CustomerTab.Handlers.SupplierHandler {
    class SupplierHandler : IDisposable {
        private IManagementConsole _managementConsole = null;   //Consola de gestão dos parâmetros
        private ExtenderEvents _myEvents = null;
        private FormSupplierTab _formTab = null;                     //Form das propriedades

        public void SetEventHandler(ExtenderEvents e) {
            _myEvents = e;

            _myEvents.OnDelete += myEvents_OnDelete;         // Delete  Supplier
            _myEvents.OnDispose += myEvents_OnDispose;       // Limpar recursos
            _myEvents.OnInitialize += myEvents_OnInitialize; // Inicializar, adicionar menus de utilizador
            _myEvents.OnLoad += myEvents_OnLoad;             // Ao carregar um artigo e preencher o form. Pode ser cancelado
            _myEvents.OnMenuItem += myEvents_OnMenuItem;     // Menu do utilizador foi pressionado
            _myEvents.OnNew += myEvents_OnNew;               // Novo  Supplier
            _myEvents.OnSave += myEvents_OnSave;             // Gravar Items
            _myEvents.OnValidating += myEvents_OnValidating; // Validar. Pode ser cancelado.

        }

        /// <summary>
        /// Chamado no momento da gravação
        /// </summary>
        /// <param name="Sender">GenericExtensibilityController</param>
        /// <param name="e">
        /// IN:
        ///     e.get_Data(): ExtendedPropertyList
        ///         "Data":  Supplier
        ///         "PreviousID": Identificador anterior (ItemId). Pode não estar presente
        ///         "IsNew": O Artigo é novo
        ///    
        /// OUT:
        ///     result.Sucess: Ignorado
        ///     result.ResultMessage: Mensagem a apresentar
        /// </param>
        void myEvents_OnSave(object Sender, ExtenderEventArgs e) {
            var proplist = (ExtendedPropertyList)e.get_data();
            var Supplier = (Supplier)proplist.get_Value("Data");    // The  Supplier
            var isNew = (bool)proplist.get_Value("IsNew");  // Is new?
        }

        /// <summary>
        /// Chamado se o utilizador utililzar um dos menus extendidos, passados no Initialize
        /// </summary>
        /// <param name="Sender">GenericExtensibilityController</param>
        /// <param name="e">
        /// IN:
        ///     e.get_Data(): Id do menu (string)
        ///     
        /// OUT:
        ///     Ignorado
        /// </param>
        void myEvents_OnMenuItem(object Sender, ExtenderEventArgs e) {
            var menuId = (string)e.get_data();

            switch (menuId) {
                case "mniXSupplier1":
                    System.Windows.Forms.MessageBox.Show("Pressionei  Supplier 1");
                    break;

                case "mniXSupplier2":
                    System.Windows.Forms.MessageBox.Show("Pressionei  Supplier 2");
                    break;
            }
        }

        /// <summary>
        /// Chamado quando um artigo é carregado da base de dados
        /// </summary>
        /// <param name="Sender">GenericExtensibilityController</param>
        /// <param name="e">
        /// IN:
        ///     e.get_Data():  Supplier
        ///     
        /// OUT:
        ///     Sucess: true or false
        ///     ResultMessage: caso preenchida, apresenta a mensagem
        /// </param>
        void myEvents_OnLoad(object Sender, ExtenderEventArgs e) {
            var supplier = (Supplier)e.get_data();

            if (supplier != null) {
                _formTab.OnLoad(supplier);
            }
        }


        /// <summary>
        /// Inicializa a extensão nos Artigos ( Supplier)
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
        ///     e.set_Data(): Passar um ExtenderMenuItems para extender os menus
        ///     
        /// Não mostra mensagens
        ///</param>
        void myEvents_OnInitialize(object Sender, ExtenderEventArgs e) {
            var propertyList = (ExtendedPropertyList)e.get_data();

            if (propertyList.PropertyExists("IManagementConsole")) {
                _managementConsole = (IManagementConsole)propertyList.get_Value("IManagementConsole");

                // Form a colocar no TAB dos fornecedores
                _formTab = new FormSupplierTab();
                _managementConsole.AddChildPanel(_formTab);
            }

            // Acrescentar Items ao menu
            var newMenu = new ExtenderMenuItems();

            var menuGroup = newMenu.Add("mniXFormacao", "Formação X");
            menuGroup.GroupType = ExtenderGroupType.ExtenderGroupTypeExtraOptions;   //Opções de menu
            menuGroup.BeginGroup = true;                                             //Novo grupo
            //
            var menuItem = menuGroup.ChildItems.Add("mniXSupplier1", "Meu menu 1");
            menuItem.GroupType = ExtenderGroupType.ExtenderGroupTypeExtraOptions;   //Opções de menu

            menuItem = menuGroup.ChildItems.Add("mniXSupplier2", "Meu menu 2");
            menuItem.GroupType = ExtenderGroupType.ExtenderGroupTypeExtraOptions;   //Opções de menu

            object oMenu = newMenu;
            e.result.set_data(ref oMenu);

            e.result.Success = true;
            e.result.ResultMessage = string.Empty;
        }

        /// <summary>
        /// Ocorre ao fechar o Form dos artigos.
        /// Serve para fazer a limpeza de recursos que já não sejam necessários.
        /// Não é possivel cancelar
        /// Não mostra mensagens
        /// </summary>
        void myEvents_OnDispose() {
            if (_formTab != null) {
                _formTab.Dispose();
                _formTab = null;
            }
        }

        /// <summary>
        /// Chamado quando o artigo vai ser eliminado
        /// </summary>
        /// <param name="Sender">GenericExtensibilityController</param>
        /// <param name="e">e.get_Data():  Supplier</param>
        void myEvents_OnDelete(object Sender, ExtenderEventArgs e) {
        }

        /// <summary>
        /// Ocorre ao criar um artigo novo
        /// IN:
        ///     e.get_data():  Supplier a ser criado. Pode ser alterado
        /// 
        /// OUT:
        ///     e.result.ResultMessage: Mensagem a apresentar ao utilizador. Se vazia, não mostra nada
        ///     e.result.Success: devolver true para continuar; false cancela a operação
        /// </summary>
        /// <param name="Sender">ExtensibilityController</param>
        /// <param name="e">Event parameters</param>
        void myEvents_OnNew(object Sender, ExtenderEventArgs e) {
            var supplier = (Supplier)e.get_data();

            _formTab.ResetInterface();

            //supplier.OrganizationName = "My name";
            //e.result.ResultMessage = "O nome foi alterado.";
            //e.result.Success = true;

            //e.result.ResultMessage = "New Event: Estou a criar um fornecedor novo";
            e.result.Success = true;
        }

        /// <summary>
        /// Chamado na validação do artigo, antes de gravar
        /// </summary>
        /// <param name="Sender">GenericExtensibilityController</param>
        /// <param name="e">
        /// IN:
        ///  e.get_Data(): ExtendedPropertyList
        ///     "Data":  Supplier,
        ///     "ForDeletion": bool que indica se o  Supplier vai ser apagado
        ///
        /// OUT:
        ///     result.Success: true para continuar; false para falhar a validação
        ///     result.ResultMessage: Mensagem a apresentar
        /// </param>
        void myEvents_OnValidating(object Sender, ExtenderEventArgs e) {
            var proplist = (ExtendedPropertyList)e.get_data();
            var Supplier = (Supplier)proplist.get_Value("Data");
            var forDeletion = (bool)proplist.get_Value("ForDeletion");

            e.result.Success = true;

        }

        public void Dispose() {
            // House cleanup
            _myEvents = null;
            if (_formTab != null) {
                _formTab.Dispose();
                _formTab = null;
            }
        }
    }

}
