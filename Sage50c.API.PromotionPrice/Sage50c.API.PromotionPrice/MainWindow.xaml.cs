using S50cBL18;
using S50cSys18;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sage50c.API.PromotionPrice {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
        private void btnCalcPrice_Click(object sender, RoutedEventArgs e) {
            try {
                MessageBox.Show($"Price={CheckPrice(txtItemId.Text)}");
            }
            catch( Exception ex) {
                MessageBox.Show(ex.Message, Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnOpenAPI_Click(object sender, RoutedEventArgs e) {
            try {
                this.Cursor = Cursors.Wait;
                APIEngine.Initialize(cmbApp.Text, txtCompany.Text, true);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnCloseAPI_Click(object sender, RoutedEventArgs e) {
            try {
                this.Cursor = Cursors.Wait;
                APIEngine.Terminate();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            APIEngine.APIStarted += (object sr, EventArgs ev) => {
                btnCalcPrice.IsEnabled = true;
                btnCloseAPI.IsEnabled = true;
                btnOpenAPI.IsEnabled = false;
                cmbApp.IsEnabled = false;
                txtCompany.IsEnabled = false;
                txtItemId.IsEnabled = true;
            };

            APIEngine.APIStopped += (object s, EventArgs ev) => {
                btnCalcPrice.IsEnabled = false;
                btnCloseAPI.IsEnabled = false;
                btnOpenAPI.IsEnabled = true;
                cmbApp.IsEnabled = true;
                txtCompany.IsEnabled = true;
                txtItemId.IsEnabled = false;
            };

            APIEngine.Message += (m) => {
                m.Result = MessageBox.Show(m.Prompt, m.Title, m.Buttons, m.Icon, m.Result);
            };

            APIEngine.WarningMessage += (message) => {
                MessageBox.Show(message, Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            };

            APIEngine.WarningError += (number, source, message) => {
                MessageBox.Show(message, Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }

        private double CheckPrice(string ItemID) {
            double price = 0;
            APIEngine.SystemSettings.StartUpInfo.CacheDiscountPlan = false;

            var bsoItemTransactionDetail = new BSOItemTransactionDetail();
            var bsoItemTransaction = new BSOItemTransaction();

            if (!string.IsNullOrEmpty(APIEngine.SystemSettings.QuickSearchDefaults.DefaultTransDocumentID)) {
                bsoItemTransaction.TransactionType = DocumentTypeEnum.dcTypeSale;
                bsoItemTransaction.InitNewTransaction(APIEngine.SystemSettings.QuickSearchDefaults.DefaultTransDocumentID,
                                                      APIEngine.SystemSettings.WorkstationInfo.DefaultTransSerial,
                                                      false, false, true, false);
                bsoItemTransaction.TransactionTaxIncluded = true;
            }

            bsoItemTransactionDetail.InitNewTransaction();
            var document = APIEngine.SystemSettings.WorkstationInfo.Document[APIEngine.SystemSettings.QuickSearchDefaults.DefaultTransDocumentID];
            bsoItemTransactionDetail.TransactionDocument = document;
            bsoItemTransactionDetail.UserPermissions = APIEngine.SystemSettings.User;
            bsoItemTransactionDetail.BaseCurrency = APIEngine.SystemSettings.BaseCurrency;
            bsoItemTransactionDetail.TransactionDetail.BaseCurrency = APIEngine.SystemSettings.BaseCurrency;
            bsoItemTransactionDetail.createDate = DateTime.Now.Date;
            bsoItemTransactionDetail.CreateTime = new DateTime(DateTime.Now.TimeOfDay.Ticks);
            bsoItemTransactionDetail.SetTransactionTaxIncluded(true);
            bsoItemTransactionDetail.TransactionType = DocumentTypeEnum.dcTypeSale;
            bsoItemTransactionDetail.Reset();

            bsoItemTransaction.AbortTransaction();

            bsoItemTransactionDetail.PartyPriceLineID = 1;  // Linha de preço do cliente
            if (bsoItemTransactionDetail.HandleItemDetail(ItemID, TransDocFieldIDEnum.fldItemID, true)) {
                if (!string.IsNullOrEmpty(bsoItemTransactionDetail.TransactionDetail.ItemID)) {
                    bsoItemTransaction.AddDetail(bsoItemTransactionDetail.TransactionDetail);
                    APIEngine.DiscountManager.ApplyTransactionDiscounts(bsoItemTransactionDetail);      //Aplica Mix-and-Match e/ou descontos que só são aplicados com F10
                    price = bsoItemTransactionDetail.TransactionDetail.TotalTaxIncludedAmount;
                }
            }
            return price;
        }

    }
}
