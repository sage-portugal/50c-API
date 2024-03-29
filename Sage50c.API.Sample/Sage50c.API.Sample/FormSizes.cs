﻿using System;
using System.Drawing;
using System.Windows.Forms;

using S50cDL22;

namespace Sage50c.API.Sample {
    public partial class FormSizes : Form {

        /// <summary>
        /// Provedor de tamanhos
        /// </summary>
        private DSOSize sizeProvider = new DSOSize();
        /// <summary>
        /// Permite identificar se o tamanho foi carregado a partir da database
        /// </summary>
        private bool isLoaded = false;

        public FormSizes() {
            InitializeComponent();
            //Insere o new id na textbox na inicalização do form
            txtId.Text = sizeProvider.GetNewID().ToString();

            FormatForm();
        }

        public void FormatForm() {
            btnSearch.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);
            btnFirst.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);
            btnRight.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);
            btnLeft.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);
            btnLast.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);

            btnNew.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);
            btnNew.FlatAppearance.BorderColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBorderColor);
            btnSave.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.PrimaryBackColor);
            btnSave.ForeColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.PrimaryForeColor);
            btnSave.FlatAppearance.BorderColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBorderColor);
            btnDelete.BackColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBackColor);
            btnDelete.FlatAppearance.BorderColor = ColorTranslator.FromOle((int)APIEngine.SystemSettings.Application.UI.Button.SecondaryBorderColor);
        }

        /// <summary>
        /// Realiza o update dos campos apresentados
        /// </summary>
        private void UpdateUI(S50cBO22.Size size) {

            if (size != null) {
                txtId.Text = size.SizeID.ToString();
                txtDescription.Text = size.Description.ToString();
                isLoaded = true;
            }
            else {
                txtDescription.Text = string.Empty;
                isLoaded = false;
            }
        }

        /// <summary>
        /// Realiza o reset dos campos apresentados, limpando-os
        /// </summary>
        private void ResetUI() {

            EnableComp(false);
            UpdateUI(new S50cBO22.Size() {
                SizeID = sizeProvider.GetNewID(),
                Description = string.Empty,
            });
        }

        private void btnSearch_Click(object sender, EventArgs e) {

            if (sizeProvider != null) {
                //Abre quick search para procurar um determinado tamanho
                var sizeId = QuickSearchHelper.SizeFind();
                //Atualiza os campos consoante o tamanho selecionado
                if (sizeId > 0) {
                    var size = sizeProvider.GetSize((short)sizeId);
                    UpdateUI(size);
                    EnableComp(true);
                    isLoaded = true;
                }
            }
        }

        private void btnFirst_Click(object sender, EventArgs e) {

            if (sizeProvider != null) {
                //Tamanho com menor id
                var size = sizeProvider.GetSize(sizeProvider.GetNextID(0));
                //Atualiza os campos consoante o tamanho selecionado
                UpdateUI(size);
                EnableComp(true);
                isLoaded = true;
            }
        }

        private void btnLeft_Click(object sender, EventArgs e) {

            if (sizeProvider != null) {
                var sizeId = txtId.Text.ToShort();
                //Tamanho com o id anterior ao atual
                var prevSize = sizeProvider.GetPreviousID(sizeId);

                if (prevSize != sizeId) {
                    var size = sizeProvider.GetSize(prevSize);
                    //Atualiza os campos consoante o tamanho selecionado
                    UpdateUI(size);
                    EnableComp(true);
                    isLoaded = true;
                }
            }
        }

        private void btnRight_Click(object sender, EventArgs e) {

            if (sizeProvider != null) {
                var sizeId = txtId.Text.ToShort();
                //Tamanho com id seguinte ao atual
                var prevSize = sizeProvider.GetNextID(sizeId);

                if (prevSize != sizeId) {
                    var size = sizeProvider.GetSize(prevSize);
                    //Atualiza os campos consoante o tamanho selecionado
                    UpdateUI(size);
                    EnableComp(true);
                    isLoaded = true;
                }
            }
        }

        private void btnLast_Click(object sender, EventArgs e) {

            if (sizeProvider != null) {
                //Tamanho com maior id
                var sizeId = sizeProvider.GetLastID();
                var size = sizeProvider.GetSize(sizeId);
                //Atualiza os campos consoante o tamanho selecionado
                UpdateUI(size);
                EnableComp(true);
                isLoaded = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e) {

            isLoaded = false;
            //Limpa todos os campos para que se possa criar um novo tamanho 
            ResetUI();
        }

        private void btnSave_Click(object sender, EventArgs e) {

            //Confirmar a existência de uma descrição
            if (txtDescription.Text == string.Empty) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice("Introduza uma descrição para adicionar", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
            }
            else {
                //Cria um novo tamanho com os valores apresentados
                var newSize = new S50cBO22.Size() {
                    SizeID = txtId.Text.ToShort(),
                    Description = txtDescription.Text,
                };

                //Guarda o novo tamanho
                if (isLoaded) {
                    sizeProvider.Save(newSize, newSize.SizeID, false);
                }
                else {
                    sizeProvider.Save(newSize, newSize.SizeID, true);
                }

                //Limpa todos os campos para que se possa criar um novo tamanho  
                ResetUI();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {

            if (!isLoaded) {
                APIEngine.CoreGlobals.MsgBoxFrontOffice("Não pode eliminar durante uma inserção!", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
            }
            else {
                var result = APIEngine.CoreGlobals.MsgBoxFrontOffice("Confirma a anulação deste registo?", VBA.VbMsgBoxStyle.vbQuestion | VBA.VbMsgBoxStyle.vbYesNo, Application.ProductName);
                if (result == VBA.VbMsgBoxResult.vbYes) {
                    try {
                        //Elimina o tamanho apresentado
                        sizeProvider.Delete(txtId.Text.ToShort());

                        //Limpa todos os campos para que se possa criar um novo tamanho 
                        ResetUI();
                    }
                    catch {
                        var opt = APIEngine.CoreGlobals.MsgBoxFrontOffice("Existem registos relacionados com este Tamanho. Para manter a integridade referencial e poder apagar este tamanho terá que indicar um código que o substitua.", VBA.VbMsgBoxStyle.vbQuestion | VBA.VbMsgBoxStyle.vbYesNo, Application.ProductName);
                        if (opt == VBA.VbMsgBoxResult.vbYes) {
                            S50cCore22.POSInputBox box = new S50cCore22.POSInputBox();
                            var x = box.Show("Qual é o código do tamanho?", "Integridade Referencial");
                            if (x != null) {
                                var newSizeId = x.ToString().ToShort();
                                if (newSizeId != txtId.Text.ToShort()) {
                                    if (newSizeId > 0 && sizeProvider.SizeExists(newSizeId)) {
                                        sizeProvider.Delete(txtId.Text.ToShort(), newSizeId);
                                        ResetUI();
                                    }
                                    else {
                                        APIEngine.CoreGlobals.MsgBoxFrontOffice("Não existe um tamanho correspondente ao código inserido!", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                                    }
                                }
                                else {
                                    APIEngine.CoreGlobals.MsgBoxFrontOffice("Não é possível substituir um tamanho por ele mesmo!", VBA.VbMsgBoxStyle.vbInformation, Application.ProductName);
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {

            //Usar F5 para "simular" o click do btnDelete
            if (keyData == Keys.F5) {
                btnDelete.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void txtId_KeyPress(object sender, KeyPressEventArgs e) {

            if (e.KeyChar == (char)Keys.Enter) {
                EnableComp(true);
                CheckTextId();
                txtDescription.Select();
            }
        }

        private void txtId_Leave(object sender, EventArgs e) {

            var sizeId = txtId.Text.ToShort();
            if (sizeId > 0) {
                var size = sizeProvider.GetSize(sizeId);
                UpdateUI(size);
                EnableComp(true);
            }
            else {
                CheckTextId();
            }
        }

        private void EnableComp(bool action) {

            btnSave.Enabled = action;
            txtDescription.Enabled = action;
        }

        private void CheckTextId() {

            if (txtId.Text.ToShort() == 0) {
                btnSearch.PerformClick();
            }
        }
    }
}