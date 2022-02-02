
namespace Sage50c.API.Sample {
    partial class FormMain {
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkAPIDebugMode = new System.Windows.Forms.CheckBox();
            this.txtCompanyId = new System.Windows.Forms.TextBox();
            this.Label40 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmbAPI = new System.Windows.Forms.ComboBox();
            this._Bar1_1 = new System.Windows.Forms.Label();
            this.btnStartAPI = new System.Windows.Forms.Button();
            this.btnTerminateAPI = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtSAFTStart = new System.Windows.Forms.DateTimePicker();
            this.dtSAFTEnd = new System.Windows.Forms.DateTimePicker();
            this.brnmSAFTExport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.progressSAFT = new System.Windows.Forms.ProgressBar();
            this.lblSAFTProgress = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel1.Controls.Add(this.chkAPIDebugMode);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.txtCompanyId);
            this.panel1.Controls.Add(this.Label40);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panel1.Size = new System.Drawing.Size(910, 106);
            this.panel1.TabIndex = 10;
            // 
            // chkAPIDebugMode
            // 
            this.chkAPIDebugMode.AutoSize = true;
            this.chkAPIDebugMode.ForeColor = System.Drawing.Color.Black;
            this.chkAPIDebugMode.Location = new System.Drawing.Point(693, 34);
            this.chkAPIDebugMode.Name = "chkAPIDebugMode";
            this.chkAPIDebugMode.Size = new System.Drawing.Size(97, 17);
            this.chkAPIDebugMode.TabIndex = 305;
            this.chkAPIDebugMode.Text = "Modo DEBUG";
            this.chkAPIDebugMode.UseVisualStyleBackColor = true;
            // 
            // txtCompanyId
            // 
            this.txtCompanyId.AcceptsReturn = true;
            this.txtCompanyId.BackColor = System.Drawing.SystemColors.Window;
            this.txtCompanyId.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCompanyId.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCompanyId.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtCompanyId.Location = new System.Drawing.Point(376, 31);
            this.txtCompanyId.MaxLength = 0;
            this.txtCompanyId.Name = "txtCompanyId";
            this.txtCompanyId.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtCompanyId.Size = new System.Drawing.Size(310, 23);
            this.txtCompanyId.TabIndex = 30;
            this.txtCompanyId.Text = "APIXDEMO";
            // 
            // Label40
            // 
            this.Label40.AutoSize = true;
            this.Label40.BackColor = System.Drawing.Color.Transparent;
            this.Label40.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label40.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label40.ForeColor = System.Drawing.Color.Black;
            this.Label40.Location = new System.Drawing.Point(294, 35);
            this.Label40.Name = "Label40";
            this.Label40.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label40.Size = new System.Drawing.Size(57, 15);
            this.Label40.TabIndex = 294;
            this.Label40.Text = "Empresa:";
            // 
            // panel5
            // 
            this.panel5.BackgroundImage = global::Sage50c.API.Sample.Properties.Resources.Sage50c;
            this.panel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(283, 103);
            this.panel5.TabIndex = 304;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.btnTerminateAPI);
            this.panel3.Controls.Add(this.cmbAPI);
            this.panel3.Controls.Add(this._Bar1_1);
            this.panel3.Controls.Add(this.btnStartAPI);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 345);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(910, 46);
            this.panel3.TabIndex = 12;
            // 
            // cmbAPI
            // 
            this.cmbAPI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAPI.FormattingEnabled = true;
            this.cmbAPI.Items.AddRange(new object[] {
            "CGCO",
            "CRTL"});
            this.cmbAPI.Location = new System.Drawing.Point(12, 11);
            this.cmbAPI.Name = "cmbAPI";
            this.cmbAPI.Size = new System.Drawing.Size(92, 21);
            this.cmbAPI.TabIndex = 181;
            // 
            // _Bar1_1
            // 
            this._Bar1_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(177)))), ((int)(((byte)(158)))));
            this._Bar1_1.Cursor = System.Windows.Forms.Cursors.Default;
            this._Bar1_1.Dock = System.Windows.Forms.DockStyle.Top;
            this._Bar1_1.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Bar1_1.Location = new System.Drawing.Point(0, 0);
            this._Bar1_1.Name = "_Bar1_1";
            this._Bar1_1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._Bar1_1.Size = new System.Drawing.Size(910, 3);
            this._Bar1_1.TabIndex = 180;
            // 
            // btnStartAPI
            // 
            this.btnStartAPI.Location = new System.Drawing.Point(110, 6);
            this.btnStartAPI.Name = "btnStartAPI";
            this.btnStartAPI.Size = new System.Drawing.Size(93, 29);
            this.btnStartAPI.TabIndex = 9;
            this.btnStartAPI.Text = "Iniciar API";
            this.btnStartAPI.UseVisualStyleBackColor = true;
            this.btnStartAPI.Click += new System.EventHandler(this.btnStartAPI_Click);
            // 
            // btnTerminateAPI
            // 
            this.btnTerminateAPI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTerminateAPI.Enabled = false;
            this.btnTerminateAPI.Location = new System.Drawing.Point(799, 6);
            this.btnTerminateAPI.Name = "btnTerminateAPI";
            this.btnTerminateAPI.Size = new System.Drawing.Size(99, 29);
            this.btnTerminateAPI.TabIndex = 182;
            this.btnTerminateAPI.Text = "Terminar API";
            this.btnTerminateAPI.UseVisualStyleBackColor = true;
            this.btnTerminateAPI.Click += new System.EventHandler(this.btnTerminateAPI_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblSAFTProgress);
            this.groupBox1.Controls.Add(this.progressSAFT);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.brnmSAFTExport);
            this.groupBox1.Controls.Add(this.dtSAFTEnd);
            this.groupBox1.Controls.Add(this.dtSAFTStart);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 106);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(228, 239);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SAFT";
            // 
            // dtSAFTStart
            // 
            this.dtSAFTStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtSAFTStart.Location = new System.Drawing.Point(6, 32);
            this.dtSAFTStart.Name = "dtSAFTStart";
            this.dtSAFTStart.Size = new System.Drawing.Size(99, 22);
            this.dtSAFTStart.TabIndex = 0;
            // 
            // dtSAFTEnd
            // 
            this.dtSAFTEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtSAFTEnd.Location = new System.Drawing.Point(111, 32);
            this.dtSAFTEnd.Name = "dtSAFTEnd";
            this.dtSAFTEnd.Size = new System.Drawing.Size(99, 22);
            this.dtSAFTEnd.TabIndex = 1;
            // 
            // brnmSAFTExport
            // 
            this.brnmSAFTExport.Location = new System.Drawing.Point(111, 60);
            this.brnmSAFTExport.Name = "brnmSAFTExport";
            this.brnmSAFTExport.Size = new System.Drawing.Size(99, 29);
            this.brnmSAFTExport.TabIndex = 10;
            this.brnmSAFTExport.Text = "Exporta SAFT";
            this.brnmSAFTExport.UseVisualStyleBackColor = true;
            this.brnmSAFTExport.Click += new System.EventHandler(this.brnmSAFTExport_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Data Inicio:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Data Fim:";
            // 
            // progressSAFT
            // 
            this.progressSAFT.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressSAFT.Location = new System.Drawing.Point(3, 219);
            this.progressSAFT.Name = "progressSAFT";
            this.progressSAFT.Size = new System.Drawing.Size(222, 17);
            this.progressSAFT.TabIndex = 15;
            // 
            // lblSAFTProgress
            // 
            this.lblSAFTProgress.Location = new System.Drawing.Point(3, 203);
            this.lblSAFTProgress.Name = "lblSAFTProgress";
            this.lblSAFTProgress.Size = new System.Drawing.Size(222, 13);
            this.lblSAFTProgress.TabIndex = 16;
            this.lblSAFTProgress.Text = "SAFT Step";
            this.lblSAFTProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 391);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkAPIDebugMode;
        private System.Windows.Forms.Panel panel5;
        public System.Windows.Forms.TextBox txtCompanyId;
        public System.Windows.Forms.Label Label40;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnTerminateAPI;
        private System.Windows.Forms.ComboBox cmbAPI;
        private System.Windows.Forms.Label _Bar1_1;
        private System.Windows.Forms.Button btnStartAPI;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button brnmSAFTExport;
        private System.Windows.Forms.DateTimePicker dtSAFTEnd;
        private System.Windows.Forms.DateTimePicker dtSAFTStart;
        private System.Windows.Forms.ProgressBar progressSAFT;
        private System.Windows.Forms.Label lblSAFTProgress;
    }
}