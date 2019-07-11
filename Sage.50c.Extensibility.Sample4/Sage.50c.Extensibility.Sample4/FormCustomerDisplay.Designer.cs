namespace Sage50c.ExtenderSample {
    partial class FormCustomerDisplay {
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
            this.btnOK = new System.Windows.Forms.Button();
            this.txtDisplayLine1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkCustomerDisplay = new System.Windows.Forms.CheckBox();
            this.chkAdsViewer = new System.Windows.Forms.CheckBox();
            this.numValue = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(440, 144);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(93, 32);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtDisplayLine1
            // 
            this.txtDisplayLine1.Location = new System.Drawing.Point(104, 44);
            this.txtDisplayLine1.Name = "txtDisplayLine1";
            this.txtDisplayLine1.Size = new System.Drawing.Size(429, 20);
            this.txtDisplayLine1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Text line 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Text line 2";
            // 
            // chkCustomerDisplay
            // 
            this.chkCustomerDisplay.AutoSize = true;
            this.chkCustomerDisplay.Location = new System.Drawing.Point(105, 104);
            this.chkCustomerDisplay.Name = "chkCustomerDisplay";
            this.chkCustomerDisplay.Size = new System.Drawing.Size(107, 17);
            this.chkCustomerDisplay.TabIndex = 6;
            this.chkCustomerDisplay.Text = "Customer Display";
            this.chkCustomerDisplay.UseVisualStyleBackColor = true;
            // 
            // chkAdsViewer
            // 
            this.chkAdsViewer.AutoSize = true;
            this.chkAdsViewer.Location = new System.Drawing.Point(105, 127);
            this.chkAdsViewer.Name = "chkAdsViewer";
            this.chkAdsViewer.Size = new System.Drawing.Size(76, 17);
            this.chkAdsViewer.TabIndex = 7;
            this.chkAdsViewer.Text = "AdsViewer";
            this.chkAdsViewer.UseVisualStyleBackColor = true;
            // 
            // numValue
            // 
            this.numValue.DecimalPlaces = 2;
            this.numValue.Location = new System.Drawing.Point(104, 70);
            this.numValue.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numValue.Name = "numValue";
            this.numValue.Size = new System.Drawing.Size(140, 20);
            this.numValue.TabIndex = 11;
            this.numValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numValue.ThousandsSeparator = true;
            // 
            // FormCustomerDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 195);
            this.Controls.Add(this.numValue);
            this.Controls.Add(this.chkAdsViewer);
            this.Controls.Add(this.chkCustomerDisplay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDisplayLine1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormCustomerDisplay";
            this.Text = "Mensagem para o cliente";
            this.Load += new System.EventHandler(this.FormCustomerDisplay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtDisplayLine1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkCustomerDisplay;
        private System.Windows.Forms.CheckBox chkAdsViewer;
        private System.Windows.Forms.NumericUpDown numValue;
    }
}