using S50cBL22;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sage50c.ExtenderSample22 {
    public partial class FormProps : Form, IChildPanel {
        public FormProps() {
            InitializeComponent();
        }

        public string Description {
            get {
                return "My Description";
            }
        }

        /// <summary>
        /// OBRIGATÓRIO: hWnd da janela
        /// </summary>
        public int Handler {
            get {
                return this.Handle.ToInt32();
            }
        }

        public  stdole.StdPicture Picture {
            get {
                //return null;
                return (stdole.StdPicture)ImageConverter.GetIPictureDispFromImage( Icon.ToBitmap() );
            }
        }

        public string Title {
            get {
                return "My Title";
            }
        }

        public bool BeforeCancel() {
            return true;
        }

        public bool BeforeOk() {
            return true;
        }

        public bool CheckIDValue(string value) {
            return true;
        }

        public bool OnMenuItem(string MenuItemID) {
            return true;
        }


        /// <summary>
        /// Cor de fundo
        /// </summary>
        /// <param name="value"></param>
        public void SetBackcolor(int value) {
            this.BackColor = Color.FromArgb(value);
        }

        public void SetFocus() {
            try {
                this.SetFocus();
            }
            catch { }
        }

        public void SetSize(float Width, float Height) {
            try {
                // Traduzir para pixels
                this.SetBounds(0, 0, (int)(Width/16), (int)(Height/16));
                this.Visible = true;
                this.BringToFront();
            }
            catch { }

        }


        public void Validate( ExtendedEventResult result ) {
            result.Success=true;
            result.ResultMessage = "Validate Event: Not done yet, but I'm returning TRUE so you can continue.";
        }
        public void Save() {
            MessageBox.Show("Save event: save done", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information );
        }

        private void FormProps_Load(object sender, EventArgs e) {

        }
    }
}
