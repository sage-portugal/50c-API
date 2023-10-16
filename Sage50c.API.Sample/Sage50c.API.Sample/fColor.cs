using S50cDL22;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sage50c.API.Sample {
    public partial class fColor : Form {

        /// <summary>
        /// Motor de dados para as cores.
        /// </summary>
        private DSOColor colorProvider { get { return APIEngine.DSOCache.ColorProvider; } }

        /// <summary>
        /// Permite saber se a color selecionada foi carregada da base de dados
        /// </summary>
        private bool isLoaded = false;

        public fColor() {
            InitializeComponent();

            txtColorID.Text = colorProvider.GetNewID().ToString();
        }

        private void btnSearchColor_Click(object sender, EventArgs e) {
            var colorId = QuickSearchHelper.ColorFind();
            if (colorId > 0) {
                var color = colorProvider.GetColor((short)colorId);
                UpdateUI(color);
            }
        }

        private void btnFirst_Click(object sender, EventArgs e) {
            var color = colorProvider.GetColor(1);
            UpdateUI(color);
            isLoaded = true;
        }

        private void btnLeft_Click(object sender, EventArgs e) {

            //TODO: Create a GetFirst() method
            var firstColorID = 1;
            if (Convert.ToInt16(txtColorID.Text) > firstColorID) {

                var previousID = colorProvider.GetPreviousID(Convert.ToInt16(txtColorID.Text));
                var color = colorProvider.GetColor(previousID);
                UpdateUI(color);
                isLoaded = true;
            }
        }

        private void btnRight_Click(object sender, EventArgs e) {

            var lastColorID = colorProvider.GetLastID();
            if (Convert.ToInt16(txtColorID.Text) < lastColorID) {

                var nextID = colorProvider.GetNextID(Convert.ToInt16(txtColorID.Text));
                var color = colorProvider.GetColor(nextID);
                UpdateUI(color);
                isLoaded = true;
            }
        }

        private void btnLast_Click(object sender, EventArgs e) {
            var lastColorID = colorProvider.GetLastID();
            var color = colorProvider.GetColor(lastColorID);
            UpdateUI(color);
            isLoaded = true;
        }

        private void btnPickColor_Click(object sender, EventArgs e) {
            colorDialog.Color = Color.Gray;
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                panelColorUI.BackColor = colorDialog.Color;
            }
        }

        private void btnNew_Click(object sender, EventArgs e) {
            isLoaded = false;
            ResetUI();
        }

        private void btnSave_Click(object sender, EventArgs e) {

            var newColor = new S50cBO22.Color() {
                ColorID = Convert.ToInt16(txtColorID.Text),
                Description = txtColorDescription.Text,
                ColorCode = ColorTranslator.ToOle(colorDialog.Color)
            };

            if (isLoaded) {
                colorProvider.Save(newColor, newColor.ColorID, false);
            }
            else {
                colorProvider.Save(newColor, newColor.ColorID, true);
            }

            ResetUI();
        }

        private void btnDelete_Click(object sender, EventArgs e) {

            colorProvider.Delete(Convert.ToInt16(txtColorID.Text));
            ResetUI();
        }

        private void ResetUI() {
            UpdateUI(new S50cBO22.Color() {
                ColorID = colorProvider.GetNewID(),
                Description = string.Empty,
                ColorCode = APIEngine.SystemSettings.Application.UI.Colors.TabBackColor
            });
        }

        private void UpdateUI(S50cBO22.Color color) {
            txtColorID.Text = color.ColorID.ToString();
            txtColorDescription.Text = color.Description;
            panelColorUI.BackColor = ColorTranslator.FromOle((int)color.ColorCode);
        }
    }
}
