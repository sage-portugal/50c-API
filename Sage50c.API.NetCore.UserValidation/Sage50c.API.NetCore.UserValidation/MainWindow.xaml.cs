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

        private void btnLogon_Click(object sender, RoutedEventArgs e) {
            bool validUser = true;
            bool validPasswd = false;
            try {
                this.Cursor = Cursors.Wait;
                var sysManager = new S50cSys22.SystemManager();
                if( sysManager.Initialize() ) {
                    var user = sysManager.Users.OfType<S50cSys22.SystemUser>()
                                               .FirstOrDefault(u => u.Id.Equals(txtUser.Text, StringComparison.CurrentCultureIgnoreCase));
                    validUser = (user != null);
                    if( user != null) {
                        validPasswd = user.CheckPassword(txtPassword.Text);
                    }
                }
                sysManager = null;

                MessageBox.Show($"Valid User={validUser}{Environment.NewLine}Logon result={validPasswd}", 
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
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnOpenAPI_Click(object sender, RoutedEventArgs e) {
            try {
                this.Cursor = Cursors.Wait;
                APIEngine.Initialize(cmbApp.Text, txtCompany.Text, true);
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message,
                                Application.Current.MainWindow.Title,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
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
            catch(Exception ex) {
                MessageBox.Show(ex.Message,
                                Application.Current.MainWindow.Title,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            APIEngine.APIStarted += (object sender, EventArgs e) => {
                btnLogon.IsEnabled = true;
                btnCloseAPI.IsEnabled = true;
                btnOpenAPI.IsEnabled = false;
                cmbApp.IsEnabled = false;
                txtCompany.IsEnabled = false;
                txtUser.IsEnabled = true;
                txtPassword.IsEnabled = true;
            };

            APIEngine.APIStopped += (object sender, EventArgs e) => {
                btnLogon.IsEnabled = false;
                btnCloseAPI.IsEnabled = false;
                btnOpenAPI.IsEnabled = true;
                cmbApp.IsEnabled = true;
                txtCompany.IsEnabled = true;
                txtUser.IsEnabled = false;
                txtPassword.IsEnabled = false;
            };
        }
    }
}
