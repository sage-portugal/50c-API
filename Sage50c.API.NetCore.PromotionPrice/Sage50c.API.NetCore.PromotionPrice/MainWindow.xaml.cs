using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                MessageBox.Show($"Promotion Price={ APITransactionHelper.CheckPrice(txtItemId.Text) }", 
                                Application.Current.MainWindow.Title,
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message,
                                Application.Current.MainWindow.Title,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void btnOpenAPI_Click(object sender, RoutedEventArgs e) {
            try {
                APIEngine.Initialize(cmbApp.Text, txtCompany.Text, true);
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message,
                                Application.Current.MainWindow.Title,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void btnCloseAPI_Click(object sender, RoutedEventArgs e) {
            try {
                APIEngine.Terminate();
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message,
                                Application.Current.MainWindow.Title,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            APIEngine.APIStarted += (object sender, EventArgs e) => {
                btnCalcPrice.IsEnabled = true;
                btnCloseAPI.IsEnabled = true;
                btnOpenAPI.IsEnabled = false;
                cmbApp.IsEnabled = false;
                txtCompany.IsEnabled = false;
                txtItemId.IsEnabled = true;
            };

            APIEngine.APIStopped += (object sender, EventArgs e) => {
                btnCalcPrice.IsEnabled = false;
                btnCloseAPI.IsEnabled = false;
                btnOpenAPI.IsEnabled = true;
                cmbApp.IsEnabled = true;
                txtCompany.IsEnabled = true;
                txtItemId.IsEnabled = false;
            };
        }
    }
}
