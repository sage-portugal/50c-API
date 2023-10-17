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

            FormatButtons();
            ResetUI();
        }

        private void btnSearch_Click(object sender, EventArgs e) {

            var colorId = QuickSearchHelper.ColorFind();
            if (colorId > 0) {

                var color = colorProvider.GetColor((short)colorId);
                UpdateUI(color);
                isLoaded = true;
                ToggleButtons(true);
            }
        }

        private void btnFirst_Click(object sender, EventArgs e) {

            var color = colorProvider.GetColor(1);
            UpdateUI(color);
            isLoaded = true;
            ToggleButtons(true);
        }

        private void btnLeft_Click(object sender, EventArgs e) {

            //TODO: Create a GetFirst() method
            var firstColorID = 1;
            if (txtColorID.Text.ToShort() > firstColorID) {

                var previousID = colorProvider.GetPreviousID(txtColorID.Text.ToShort());
                var color = colorProvider.GetColor(previousID);
                UpdateUI(color);
                isLoaded = true;
                ToggleButtons(true);
            }
        }

        private void btnRight_Click(object sender, EventArgs e) {

            var lastColorID = colorProvider.GetLastID();
            if (txtColorID.Text.ToShort() < lastColorID) {

                var nextID = colorProvider.GetNextID(txtColorID.Text.ToShort());
                var color = colorProvider.GetColor(nextID);
                UpdateUI(color);
                isLoaded = true;
                ToggleButtons(true);
            }
        }

        private void btnLast_Click(object sender, EventArgs e) {
            var lastColorID = colorProvider.GetLastID();
            var color = colorProvider.GetColor(lastColorID);
            UpdateUI(color);
            isLoaded = true;
            ToggleButtons(true);
        }

        private void btnPickColor_Click(object sender, EventArgs e) {
            colorDialog.Color = Color.Gray;
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                panelColorUI.BackColor = colorDialog.Color;
            }
        }

        private void btnNew_Click(object sender, EventArgs e) {
            ResetUI();
            isLoaded = false;
        }

        private void btnSave_Click(object sender, EventArgs e) {

            var newColor = new S50cBO22.Color() {
                ColorID = txtColorID.Text.ToShort(),
                Description = txtColorDescription.Text,
                ColorCode = ColorTranslator.ToOle(panelColorUI.BackColor)
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

            if (isLoaded) {
                var result = APIEngine.CoreGlobals.MsgBoxFrontOffice("Confirma a anulação deste registo?", VBA.VbMsgBoxStyle.vbQuestion | VBA.VbMsgBoxStyle.vbYesNo, Application.ProductName);
                if (result == VBA.VbMsgBoxResult.vbYes) {
                    colorProvider.Delete(txtColorID.Text.ToShort());
                }
            }
            else {
                APIEngine.CoreGlobals.MsgBoxFrontOffice("Não pode eliminar durante uma inserção.", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
            }

            ResetUI();
        }

        private void FormatButtons() {
            btnSearch.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);
            btnFirst.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);
            btnRight.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);
            btnLeft.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);
            btnLast.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);
            btnPickColor.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);

            btnNew.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);
            btnNew.FlatAppearance.BorderColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBorderColor);
            btnSave.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.PrimaryBackColor);
            btnSave.FlatAppearance.BorderColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBorderColor);
            btnSave.ForeColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.PrimaryForeColor);
            btnDelete.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);
            btnDelete.FlatAppearance.BorderColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBorderColor);
        }

        private void ResetUI() {
            ToggleButtons(false);

            UpdateUI(new S50cBO22.Color() {
                ColorID = colorProvider.GetNewID(),
                Description = string.Empty,
                ColorCode = APIEngine.SystemSettings.Application.UI.Colors.TabBackColor
            });
        }

        private void UpdateUI(S50cBO22.Color color) {
            if (color != null) {
                txtColorID.Text = color.ColorID.ToString();
                txtColorDescription.Text = color.Description;
                panelColorUI.BackColor = ColorTranslator.FromOle((int)color.ColorCode);
            }
        }

        private void txtColorID_Leave(object sender, EventArgs e) {

            var colorID = txtColorID.Text.ToShort();
            if (colorID > 0) {

                ToggleButtons(true);

                var color = colorProvider.GetColor(colorID);
                if (color != null) {
                    UpdateUI(color);
                    isLoaded = true;
                }
                else {
                    txtColorDescription.Text = string.Empty;
                    panelColorUI.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Colors.TabBackColor);
                    isLoaded = false;
                }
            }
        }

        private void txtColorID_PressEnter(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                txtColorDescription.Select();
            }
        }

        private void ToggleButtons(bool isEnabled) {
            txtColorDescription.Enabled = isEnabled;
            btnSave.Enabled = isEnabled;
        }
    }
}
