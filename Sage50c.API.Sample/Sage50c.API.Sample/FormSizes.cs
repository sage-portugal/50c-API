using S50cDL22;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        }

        /// <summary>
        /// Realiza o update dos campos apresentados
        /// </summary>
        /// <param name="size"></param>
        private void UpdateForm(S50cBO22.Size size) {
            txtId.Text = size.SizeID.ToString();
            txtDescription.Text = size.Description.ToString();
        }

        /// <summary>
        /// Realiza o reset dos campos apresentados, limpando-os
        /// </summary>
        private void ResetForm() {
            UpdateForm(new S50cBO22.Size() {
                SizeID = sizeProvider.GetNewID(),
                Description = string.Empty,
            });
        }

        private void btnSearch_Click(object sender, EventArgs e) {
            //Abre quick search para procurar um determinado tamanho
            var sizeId = QuickSearchHelper.SizeFind();
            //Atualiza os campos consoante o tamanho selecionado
            if(sizeId>0) {
                var size = sizeProvider.GetSize((short)sizeId);
                UpdateForm(size);
            }
        }

        private void btnFirst_Click(object sender, EventArgs e) {
            //Tamanho com menor id
            var size = sizeProvider.GetSize(1);
            //Atualiza os campos consoante o tamanho selecionado
            UpdateForm(size);
            isLoaded = true;
        }

        private void btnLeft_Click(object sender, EventArgs e) {
            if(Convert.ToInt16(txtId.Text) > 1) {
                //Tamanho com o id anterior ao atual
                var prevSize = sizeProvider.GetPreviousID(Convert.ToInt16(txtId.Text));
                var size = sizeProvider.GetSize(prevSize);
                //Atualiza os campos consoante o tamanho selecionado
                UpdateForm(size);
                isLoaded=true;
            }
        }

        private void btnRight_Click(object sender, EventArgs e) {
            if (Convert.ToInt16(txtId.Text) < sizeProvider.GetLastID()) {
                //Tamanho com id seguinte ao atual
                var prevSize = sizeProvider.GetNextID(Convert.ToInt16(txtId.Text));
                var size = sizeProvider.GetSize(prevSize);
                //Atualiza os campos consoante o tamanho selecionado
                UpdateForm(size);
                isLoaded = true;
            }
        }

        private void btnLast_Click(object sender, EventArgs e) {
            //Tamanho com maior id
            var sizeId = sizeProvider.GetLastID();
            var size = sizeProvider.GetSize((short)sizeId);
            //Atualiza os campos consoante o tamanho selecionado
            UpdateForm(size);
            isLoaded = true;
        }

        private void btnNew_Click(object sender, EventArgs e) {
            isLoaded = false;
            //Limpa todos os campos para que se possa criar um novo tamanho 
            ResetForm();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            //Cria um novo tamanho com os valores apresentados
            var newSize = new S50cBO22.Size() {
                SizeID = Convert.ToInt16(txtId.Text),
                Description = txtDescription.Text,
            };

            //Guarda o novo tamanho
            if(isLoaded) {
                sizeProvider.Save(newSize, newSize.SizeID, false);
            }
            else {
                sizeProvider.Save(newSize, newSize.SizeID, true);
            }

            //Limpa todos os campos para que se possa criar um novo tamanho  
            ResetForm();
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            //Elimina o tamanho apresentado
            sizeProvider.Delete(Convert.ToInt16(txtId.Text));
            //Limpa todos os campos para que se possa criar um novo tamanho 
            ResetForm();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            //Usar F5 para "simular" o click do btnDelete
            if(keyData == Keys.F5){
                btnDelete.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
