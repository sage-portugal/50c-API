namespace Sage50c.API.COM.Serialization
{
    partial class FormMain
    {
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnCloseCompany = new System.Windows.Forms.Button();
            this.btnOpenCompany = new System.Windows.Forms.Button();
            this.cmbAppCode = new System.Windows.Forms.ComboBox();
            this.txtCompanyID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.Panel();
            this.txtTransactionID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnTransSerialize = new System.Windows.Forms.Button();
            this.btnTransDeserialize = new System.Windows.Forms.Button();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.txtJSONBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.panelMiddle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnTransDeserialize);
            this.panelTop.Controls.Add(this.btnTransSerialize);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.txtTransactionID);
            this.panelTop.Controls.Add(this.btnCloseCompany);
            this.panelTop.Controls.Add(this.btnOpenCompany);
            this.panelTop.Controls.Add(this.cmbAppCode);
            this.panelTop.Controls.Add(this.txtCompanyID);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(675, 83);
            this.panelTop.TabIndex = 0;
            // 
            // btnCloseCompany
            // 
            this.btnCloseCompany.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseCompany.Location = new System.Drawing.Point(531, 9);
            this.btnCloseCompany.Name = "btnCloseCompany";
            this.btnCloseCompany.Size = new System.Drawing.Size(89, 30);
            this.btnCloseCompany.TabIndex = 5;
            this.btnCloseCompany.Text = "Close";
            this.btnCloseCompany.UseVisualStyleBackColor = true;
            this.btnCloseCompany.Click += new System.EventHandler(this.btnCloseCompany_Click);
            // 
            // btnOpenCompany
            // 
            this.btnOpenCompany.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenCompany.Location = new System.Drawing.Point(436, 9);
            this.btnOpenCompany.Name = "btnOpenCompany";
            this.btnOpenCompany.Size = new System.Drawing.Size(89, 30);
            this.btnOpenCompany.TabIndex = 4;
            this.btnOpenCompany.Text = "Open";
            this.btnOpenCompany.UseVisualStyleBackColor = true;
            this.btnOpenCompany.Click += new System.EventHandler(this.btnOpenCompany_Click);
            // 
            // cmbAppCode
            // 
            this.cmbAppCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAppCode.FormattingEnabled = true;
            this.cmbAppCode.Items.AddRange(new object[] {
            "CRTL",
            "CGCO"});
            this.cmbAppCode.Location = new System.Drawing.Point(199, 13);
            this.cmbAppCode.Name = "cmbAppCode";
            this.cmbAppCode.Size = new System.Drawing.Size(58, 23);
            this.cmbAppCode.TabIndex = 3;
            // 
            // txtCompanyID
            // 
            this.txtCompanyID.Location = new System.Drawing.Point(264, 14);
            this.txtCompanyID.Name = "txtCompanyID";
            this.txtCompanyID.Size = new System.Drawing.Size(166, 23);
            this.txtCompanyID.TabIndex = 2;
            this.txtCompanyID.Text = "ERIDIAN";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "App/Company:";
            // 
            // panelRight
            // 
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(675, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(86, 519);
            this.panelRight.TabIndex = 2;
            // 
            // txtTransactionID
            // 
            this.txtTransactionID.Location = new System.Drawing.Point(264, 50);
            this.txtTransactionID.Name = "txtTransactionID";
            this.txtTransactionID.Size = new System.Drawing.Size(166, 23);
            this.txtTransactionID.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "SAFT Format Transaction:";
            // 
            // btnTransSerialize
            // 
            this.btnTransSerialize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTransSerialize.Location = new System.Drawing.Point(436, 45);
            this.btnTransSerialize.Name = "btnTransSerialize";
            this.btnTransSerialize.Size = new System.Drawing.Size(89, 30);
            this.btnTransSerialize.TabIndex = 8;
            this.btnTransSerialize.Text = "Serialize";
            this.btnTransSerialize.UseVisualStyleBackColor = true;
            this.btnTransSerialize.Click += new System.EventHandler(this.btnTransSerialize_Click);
            // 
            // btnTransDeserialize
            // 
            this.btnTransDeserialize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTransDeserialize.Location = new System.Drawing.Point(531, 45);
            this.btnTransDeserialize.Name = "btnTransDeserialize";
            this.btnTransDeserialize.Size = new System.Drawing.Size(89, 30);
            this.btnTransDeserialize.TabIndex = 9;
            this.btnTransDeserialize.Text = "Deserialize";
            this.btnTransDeserialize.UseVisualStyleBackColor = true;
            this.btnTransDeserialize.Click += new System.EventHandler(this.btnTransDeserialize_Click);
            // 
            // panelMiddle
            // 
            this.panelMiddle.Controls.Add(this.txtJSONBox);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(0, 83);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(675, 420);
            this.panelMiddle.TabIndex = 3;
            // 
            // txtJSONBox
            // 
            this.txtJSONBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJSONBox.Location = new System.Drawing.Point(0, 0);
            this.txtJSONBox.Multiline = true;
            this.txtJSONBox.Name = "txtJSONBox";
            this.txtJSONBox.Size = new System.Drawing.Size(675, 420);
            this.txtJSONBox.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 503);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(675, 16);
            this.panel1.TabIndex = 11;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 519);
            this.Controls.Add(this.panelMiddle);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelRight);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelMiddle.ResumeLayout(false);
            this.panelMiddle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Button btnCloseCompany;
        private System.Windows.Forms.Button btnOpenCompany;
        private System.Windows.Forms.ComboBox cmbAppCode;
        private System.Windows.Forms.TextBox txtCompanyID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnTransDeserialize;
        private System.Windows.Forms.Button btnTransSerialize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTransactionID;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.TextBox txtJSONBox;
        private System.Windows.Forms.Panel panel1;
    }
}