namespace Sage50c.API.Sample {
    partial class fColor {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fColor));
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblColorID = new System.Windows.Forms.Label();
            this.txtColorID = new System.Windows.Forms.TextBox();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.lblColorDescription = new System.Windows.Forms.Label();
            this.lblColorUI = new System.Windows.Forms.Label();
            this.txtColorDescription = new System.Windows.Forms.TextBox();
            this.btnPickColor = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.panelColorUI = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::Sage50c.API.Sample.Properties.Resources.Search;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Location = new System.Drawing.Point(172, 9);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(25, 25);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblColorID
            // 
            this.lblColorID.AutoSize = true;
            this.lblColorID.Location = new System.Drawing.Point(12, 15);
            this.lblColorID.Name = "lblColorID";
            this.lblColorID.Size = new System.Drawing.Size(47, 13);
            this.lblColorID.TabIndex = 1;
            this.lblColorID.Text = "Número:";
            // 
            // txtColorID
            // 
            this.txtColorID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtColorID.Location = new System.Drawing.Point(66, 12);
            this.txtColorID.Name = "txtColorID";
            this.txtColorID.Size = new System.Drawing.Size(100, 20);
            this.txtColorID.TabIndex = 2;
            this.txtColorID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtColorID_PressEnter);
            this.txtColorID.Leave += new System.EventHandler(this.txtColorID_Leave);
            // 
            // btnFirst
            // 
            this.btnFirst.BackgroundImage = global::Sage50c.API.Sample.Properties.Resources.caretDoubleLeft;
            this.btnFirst.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFirst.FlatAppearance.BorderSize = 0;
            this.btnFirst.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFirst.Location = new System.Drawing.Point(203, 9);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(25, 25);
            this.btnFirst.TabIndex = 3;
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.BackgroundImage = global::Sage50c.API.Sample.Properties.Resources.CaretLeft;
            this.btnLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLeft.FlatAppearance.BorderSize = 0;
            this.btnLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeft.Location = new System.Drawing.Point(234, 9);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(25, 25);
            this.btnLeft.TabIndex = 4;
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnLast
            // 
            this.btnLast.BackgroundImage = global::Sage50c.API.Sample.Properties.Resources.caretDoubleRight;
            this.btnLast.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLast.FlatAppearance.BorderSize = 0;
            this.btnLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLast.Location = new System.Drawing.Point(296, 9);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(25, 25);
            this.btnLast.TabIndex = 6;
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnRight
            // 
            this.btnRight.BackgroundImage = global::Sage50c.API.Sample.Properties.Resources.caretRight;
            this.btnRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRight.FlatAppearance.BorderSize = 0;
            this.btnRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRight.Location = new System.Drawing.Point(265, 9);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(25, 25);
            this.btnRight.TabIndex = 5;
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // lblColorDescription
            // 
            this.lblColorDescription.AutoSize = true;
            this.lblColorDescription.Location = new System.Drawing.Point(12, 50);
            this.lblColorDescription.Name = "lblColorDescription";
            this.lblColorDescription.Size = new System.Drawing.Size(38, 13);
            this.lblColorDescription.TabIndex = 7;
            this.lblColorDescription.Text = "Nome:";
            // 
            // lblColorUI
            // 
            this.lblColorUI.AutoSize = true;
            this.lblColorUI.Location = new System.Drawing.Point(12, 85);
            this.lblColorUI.Name = "lblColorUI";
            this.lblColorUI.Size = new System.Drawing.Size(26, 13);
            this.lblColorUI.TabIndex = 8;
            this.lblColorUI.Text = "Cor:";
            // 
            // txtColorDescription
            // 
            this.txtColorDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtColorDescription.Location = new System.Drawing.Point(66, 47);
            this.txtColorDescription.Name = "txtColorDescription";
            this.txtColorDescription.Size = new System.Drawing.Size(255, 20);
            this.txtColorDescription.TabIndex = 9;
            // 
            // btnPickColor
            // 
            this.btnPickColor.BackgroundImage = global::Sage50c.API.Sample.Properties.Resources.PaleteCores;
            this.btnPickColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPickColor.FlatAppearance.BorderSize = 0;
            this.btnPickColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPickColor.Location = new System.Drawing.Point(172, 79);
            this.btnPickColor.Name = "btnPickColor";
            this.btnPickColor.Size = new System.Drawing.Size(25, 25);
            this.btnPickColor.TabIndex = 11;
            this.btnPickColor.UseVisualStyleBackColor = true;
            this.btnPickColor.Click += new System.EventHandler(this.btnPickColor_Click);
            // 
            // btnNew
            // 
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Location = new System.Drawing.Point(341, 9);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(94, 23);
            this.btnNew.TabIndex = 12;
            this.btnNew.Text = "Novo";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(341, 44);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Gravar (F5)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Location = new System.Drawing.Point(341, 79);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(94, 23);
            this.btnDelete.TabIndex = 14;
            this.btnDelete.Text = "Eliminar";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // panelColorUI
            // 
            this.panelColorUI.BackColor = System.Drawing.SystemColors.Window;
            this.panelColorUI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColorUI.Location = new System.Drawing.Point(66, 82);
            this.panelColorUI.Name = "panelColorUI";
            this.panelColorUI.Size = new System.Drawing.Size(100, 20);
            this.panelColorUI.TabIndex = 15;
            // 
            // fColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 116);
            this.Controls.Add(this.panelColorUI);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnPickColor);
            this.Controls.Add(this.txtColorDescription);
            this.Controls.Add(this.lblColorUI);
            this.Controls.Add(this.lblColorDescription);
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.txtColorID);
            this.Controls.Add(this.lblColorID);
            this.Controls.Add(this.btnSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fColor";
            this.Text = "Create color";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblColorID;
        private System.Windows.Forms.TextBox txtColorID;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Label lblColorDescription;
        private System.Windows.Forms.Label lblColorUI;
        private System.Windows.Forms.TextBox txtColorDescription;
        private System.Windows.Forms.Button btnPickColor;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Panel panelColorUI;
    }
}