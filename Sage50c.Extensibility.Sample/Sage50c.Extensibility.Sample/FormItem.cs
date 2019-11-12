using S50cBL18;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using stdole;
using S50cSys18;
using S50cUtil18;
using Sage50c.API;

namespace Sage50c.ExtenderSample {
    public partial class FormItem : Form, IChildPanel {

        private IChildWindow childWindow = null;

        public FormItem() {
            InitializeComponent();
        }

        public void ShowWindow() {
            if (childWindow == null) {
                childWindow = SystemHandler.GeneralChildWindow.GetNewInstance();
            }
            childWindow.Caption = "Custom Item";
            //childWindow.Init("Artigo:", true, true, null, this);

            //
            // Construir menus da janela
            // 1. Botões
            var xternderMenuItems = new ExtenderMenuItems();
            var m= xternderMenuItems.Add("xAction1", "Acção 1");
            m.ActionType = ExtenderActionType.ExtenderActionPrimary;

            m = xternderMenuItems.Add("xAction2", "Acção 2");
            m.ActionType = ExtenderActionType.ExtenderActionSecondary;

            //
            //2. Opções
            m = xternderMenuItems.Add("xOpcoes", "Opções");
            m.GroupType = ExtenderGroupType.ExtenderGroupTypeExtraOptions;
            m.BeginGroup = true;
            m.ChildItems.Add("xOpcoes1", "Opção 1");
            m.ChildItems.Add("xOpcoes2", "Opção 2");


            childWindow.Init("Artigo:", false, false, xternderMenuItems, this);

            //Translate to twips
            childWindow.SetClientArea(this.Width * 15, this.Height * 15);
            childWindow.CenterOnScreen();

            this.Visible = true;
            childWindow.Show();

            //Enable panel
            if (childWindow.EditState == EditStateType.esNull) {
                childWindow.EditState = EditStateType.esNew;
            }
        }

        public string Description {
            get {
                return "My Form Description";
            }
        }

        public int Handler {
            get {
                return this.Handle.ToInt32();
            }
        }

        public StdPicture Picture {
            get {
                return null;
            }
        }

        public string Title {
            get {
                return "My Title";
            }
        }

        public bool BeforeCancel() {
            MessageBox.Show("BeforeCancel");
            return true;
            //throw new NotImplementedException();
        }

        public bool BeforeOk() {
            MessageBox.Show("BeforeOK");
            return true;
            //throw new NotImplementedException();
        }

        public bool CheckIDValue(string value) {
            if (!string.IsNullOrEmpty(value)) {
                if (APIEngine.DSOCache.ItemProvider.ItemExist(value)) {
                    var item = APIEngine.DSOCache.ItemProvider.GetItem(value, APIEngine.SystemSettings.BaseCurrency);
                    MessageBox.Show(string.Format("ItemId={0}\r\nItem Name={1}", item.ItemID, item.Description));
                }
            }

            //MessageBox.Show("CheckIDValue: " + value );
            return true;
            //throw new NotImplementedException();
        }

        public bool OnMenuItem(string MenuItemID) {
            MessageBox.Show("MenuItemID: " + MenuItemID);
            return true;
            //throw new NotImplementedException();
        }

        public void SetBackcolor(int value) {
            this.BackColor = Color.FromArgb(value);
            //throw new NotImplementedException();
        }

        public void SetFocus() {
            //this.SetFocus();
        }

        public void SetSize(float Width, float Height) {
            SetBounds(0, 0, (int)(Width / 15), (int)(Height / 15));
        }

        private void FormItem_Load(object sender, EventArgs e) {
        }

        private void btnOK_Click(object sender, EventArgs e) {
            MessageBox.Show("Hello!");
        }
        private static bool itemIsFindind = false;
        private bool ItemFind() {
            QuickSearch quickSearch = null;
            bool result = false;

            try {
                if (!itemIsFindind) {
                    itemIsFindind = true;
                    quickSearch = APIEngine.CreateQuickSearch(QuickSearchViews.QSV_Item, APIEngine.SystemSettings.StartUpInfo.CacheQuickSearchItem);
                    clsCollection qsParams = new clsCollection();
                    qsParams.add(APIEngine.SystemSettings.QuickSearchDefaults.WarehouseID, "@WarehouseID");
                    qsParams.add(APIEngine.SystemSettings.QuickSearchDefaults.PriceLineID, "@PriceLineID");
                    qsParams.add(APIEngine.SystemSettings.QuickSearchDefaults.LanguageID, "@LanguageID");
                    qsParams.add(APIEngine.SystemSettings.QuickSearchDefaults.DisplayDiscontinued, "@Discontinued");
                    if (APIEngine.SystemSettings.StartUpInfo.UseItemSearchAlterCurrency) {
                        qsParams.add(APIEngine.SystemSettings.AlternativeCurrency.SaleExchange, "@ctxBaseCurrency");
                    }
                    else {
                        qsParams.add(APIEngine.SystemSettings.QuickSearchDefaults.EuroConversionRate, "@ctxBaseCurrency");
                    }
                    quickSearch.Parameters = qsParams;

                    if (quickSearch.SelectValue()) {
                        result = true;
                        var itemId = quickSearch.ValueSelectedString();
                        //APIEngine.ItemGet(itemId);
                    }
                    itemIsFindind = false;
                }
            }
            catch (Exception ex) {
                itemIsFindind = false;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally {

            }
            quickSearch = null;

            return result;
        }

        private void FormItem_FormClosed(object sender, FormClosedEventArgs e) {
            if (childWindow != null) {
                childWindow = null;
            }
        }

        private void btnItemQuickSearch_Click(object sender, EventArgs e) {
            ItemFind();
        }
    }
}
