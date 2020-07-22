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
using System.Security.Policy;

namespace Sage50c.ExtenderSample {
    public partial class FormItem : Form, IChildPanel {

        private IChildWindow2 childWindow = null;
        private Timer _progressTimer = null;

        public FormItem() {
            InitializeComponent();
        }

        public void ShowWindow() {
            if (childWindow == null) {
                childWindow = SystemHandler.GeneralChildWindow.GetNewInstance();
            }

            //childWindow.BorderStyle = VBRUN.FormBorderStyleConstants.vbFixedSingle;
            childWindow.Caption = "Custom Item";
            childWindow.IDFieldCaption = "Artigo:";
            childWindow.SearchButtonVisible = false;
            childWindow.NavigationButtonsVisible = false;
            childWindow.IDFieldVisible = true;

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

            childWindow.MenuItems = xternderMenuItems;
            childWindow.Init(this);
            //childWindow.Init("Artigo:", false, false, xternderMenuItems, this);

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

        private void btnProgressStart_Click(object sender, EventArgs e) {
            if(_progressTimer == null) {
                _progressTimer = new Timer();
            }
            _progressTimer.Interval = 1000;
            _progressTimer.Tick += _progressTimer_Tick;
            _progressTimer.Tag = 0;
            _progressTimer.Start();

            //OwnerID = replace by your own identifier. Make sure it's unique per progress
            //MessageID = 0 or get the message Id from "C:\Program Files (x86)\sage\Sage 50c\Sage.Localize.PTG.XML"
            APIEngine.DataManager.Events.DBStatusStart("use-your-own-unique-identifier", 2030006);
        }

        private void _progressTimer_Tick(object sender, EventArgs e) {
            var progress = (int)_progressTimer.Tag;
            APIEngine.DataManager.Events.DBStatusProgress(0, $"{++progress}/100");
            if(progress > 100) {
                progress = 0;
            }
            _progressTimer.Tag = progress;
            //Application.DoEvents();
        }

        private void btnProgressEnd_Click(object sender, EventArgs e) {
            _progressTimer.Stop();
            _progressTimer = null;
            APIEngine.DataManager.Events.DBStatusFinish("use-your-own-unique-identifier");

//            APIEngine.DataManager.Events.SingleStatusProgressValues(1, 10);

        }


        Timer _timer2 = null;
        private void btnProgressStart2_Click(object sender, EventArgs e) {
            _timer2 = new Timer();
            _timer2.Interval = 1000;
            _timer2.Tick += (s, ee) => {
                var progress = (int)_timer2.Tag;
                APIEngine.DataManager.Events.SingleStatusProgressValues( ++progress, 100);
                if (progress > 100) {
                    progress = 0;
                }
                _timer2.Tag = progress;
            };
            _timer2.Tag = 0;
            _timer2.Start();

            //OwnerID = replace by your own identifier. Make sure it's unique per progress
            //MessageID = 0 or get the message Id from "C:\Program Files (x86)\sage\Sage 50c\Sage.Localize.PTG.XML"
            APIEngine.DataManager.Events.SingleStatusStart("use-your-own-unique-identifier-2", 2030006, 100);
        }

        private void btnProgressEnd2_Click(object sender, EventArgs e) {
            _timer2.Stop();
            _timer2 = null;
            APIEngine.DataManager.Events.SingleStatusEnd("use-your-own-unique-identifier-2");
        }


        Timer _timerDoubleProgress = null;
        int _detailProgress = 0; int _globalProgress = 0;
        private void btnProgressStart3_Click(object sender, EventArgs e) {
            _timerDoubleProgress = new Timer();
            _timerDoubleProgress.Interval = 100;
            _timerDoubleProgress.Tick += (s, ee) => {
                ++_detailProgress;
                APIEngine.DataManager.Events.DoubleStatusDetailProgress(_detailProgress, 100);
                if ( _detailProgress > 100) {
                    ++_globalProgress;
                    _detailProgress = 0;
                    APIEngine.DataManager.Events.DoubleStatusGlobalTaskUpdate(0, APIEngine.StringFunctions.ParamArrayToString($"My Title {_globalProgress}"));
                }
            };
            _timerDoubleProgress.Tag = 0;
            _timerDoubleProgress.Start();
            //
            APIEngine.DataManager.Events.DoubleStatusGlobalTaskSet(0, APIEngine.StringFunctions.ParamArrayToString("My Title"), 100);
            //OwnerID = replace by your own identifier. Make sure it's unique per progress
            //MessageID = 0 or get the message Id from "C:\Program Files (x86)\sage\Sage 50c\Sage.Localize.PTG.XML"
            APIEngine.DataManager.Events.DoubleStatusStart("use-your-own-unique-identifier-double", 2030006);
        }

        private void btnProgressEnd3_Click(object sender, EventArgs e) {
            _timerDoubleProgress.Stop();
            _timerDoubleProgress = null;
            APIEngine.DataManager.Events.DoubleStatusFinish("use-your-own-unique-identifier-double");

        }
    }
}
