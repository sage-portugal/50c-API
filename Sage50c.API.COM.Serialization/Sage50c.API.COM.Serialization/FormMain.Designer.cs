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
            this.btnItemDeserialize = new System.Windows.Forms.Button();
            this.btnItemSerialize = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtItemID = new System.Windows.Forms.TextBox();
            this.btnCloseCompany = new System.Windows.Forms.Button();
            this.btnOpenCompany = new System.Windows.Forms.Button();
            this.cmbAppCode = new System.Windows.Forms.ComboBox();
            this.txtCompanyID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.txtJSONBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCustomerDeserialize = new System.Windows.Forms.Button();
            this.btnCustomerSerialize = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCustomerID = new System.Windows.Forms.TextBox();
            this.panelTop.SuspendLayout();
            this.panelMiddle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnCustomerDeserialize);
            this.panelTop.Controls.Add(this.btnCustomerSerialize);
            this.panelTop.Controls.Add(this.label3);
            this.panelTop.Controls.Add(this.txtCustomerID);
            this.panelTop.Controls.Add(this.btnItemDeserialize);
            this.panelTop.Controls.Add(this.btnItemSerialize);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.txtItemID);
            this.panelTop.Controls.Add(this.btnCloseCompany);
            this.panelTop.Controls.Add(this.btnOpenCompany);
            this.panelTop.Controls.Add(this.cmbAppCode);
            this.panelTop.Controls.Add(this.txtCompanyID);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(761, 155);
            this.panelTop.TabIndex = 0;
            // 
            // btnItemDeserialize
            // 
            this.btnItemDeserialize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnItemDeserialize.Enabled = false;
            this.btnItemDeserialize.Location = new System.Drawing.Point(617, 45);
            this.btnItemDeserialize.Name = "btnItemDeserialize";
            this.btnItemDeserialize.Size = new System.Drawing.Size(89, 30);
            this.btnItemDeserialize.TabIndex = 9;
            this.btnItemDeserialize.Text = "Deserialize";
            this.btnItemDeserialize.UseVisualStyleBackColor = true;
            this.btnItemDeserialize.Click += new System.EventHandler(this.btnItemDeserialize_Click);
            // 
            // btnItemSerialize
            // 
            this.btnItemSerialize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnItemSerialize.Enabled = false;
            this.btnItemSerialize.Location = new System.Drawing.Point(522, 45);
            this.btnItemSerialize.Name = "btnItemSerialize";
            this.btnItemSerialize.Size = new System.Drawing.Size(89, 30);
            this.btnItemSerialize.TabIndex = 8;
            this.btnItemSerialize.Text = "Serialize";
            this.btnItemSerialize.UseVisualStyleBackColor = true;
            this.btnItemSerialize.Click += new System.EventHandler(this.btnItemSerialize_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Item ID";
            // 
            // txtItemID
            // 
            this.txtItemID.Location = new System.Drawing.Point(134, 50);
            this.txtItemID.Name = "txtItemID";
            this.txtItemID.Size = new System.Drawing.Size(230, 23);
            this.txtItemID.TabIndex = 6;
            // 
            // btnCloseCompany
            // 
            this.btnCloseCompany.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseCompany.Location = new System.Drawing.Point(617, 9);
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
            this.btnOpenCompany.Location = new System.Drawing.Point(522, 9);
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
            this.cmbAppCode.Location = new System.Drawing.Point(134, 14);
            this.cmbAppCode.Name = "cmbAppCode";
            this.cmbAppCode.Size = new System.Drawing.Size(58, 23);
            this.cmbAppCode.TabIndex = 3;
            // 
            // txtCompanyID
            // 
            this.txtCompanyID.Location = new System.Drawing.Point(198, 14);
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
            // panelMiddle
            // 
            this.panelMiddle.Controls.Add(this.txtJSONBox);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(0, 155);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(761, 348);
            this.panelMiddle.TabIndex = 3;
            // 
            // txtJSONBox
            // 
            this.txtJSONBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJSONBox.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJSONBox.Location = new System.Drawing.Point(0, 0);
            this.txtJSONBox.Multiline = true;
            this.txtJSONBox.Name = "txtJSONBox";
            this.txtJSONBox.Size = new System.Drawing.Size(761, 348);
            this.txtJSONBox.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 503);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(761, 16);
            this.panel1.TabIndex = 11;
            // 
            // btnCustomerDeserialize
            // 
            this.btnCustomerDeserialize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCustomerDeserialize.Enabled = false;
            this.btnCustomerDeserialize.Location = new System.Drawing.Point(617, 74);
            this.btnCustomerDeserialize.Name = "btnCustomerDeserialize";
            this.btnCustomerDeserialize.Size = new System.Drawing.Size(89, 30);
            this.btnCustomerDeserialize.TabIndex = 13;
            this.btnCustomerDeserialize.Text = "Deserialize";
            this.btnCustomerDeserialize.UseVisualStyleBackColor = true;
            this.btnCustomerDeserialize.Click += new System.EventHandler(this.btnCustomerDeserialize_Click);
            // 
            // btnCustomerSerialize
            // 
            this.btnCustomerSerialize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCustomerSerialize.Enabled = false;
            this.btnCustomerSerialize.Location = new System.Drawing.Point(522, 74);
            this.btnCustomerSerialize.Name = "btnCustomerSerialize";
            this.btnCustomerSerialize.Size = new System.Drawing.Size(89, 30);
            this.btnCustomerSerialize.TabIndex = 12;
            this.btnCustomerSerialize.Text = "Serialize";
            this.btnCustomerSerialize.UseVisualStyleBackColor = true;
            this.btnCustomerSerialize.Click += new System.EventHandler(this.btnCustomerSerialize_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "Customer ID";
            // 
            // txtCustomerID
            // 
            this.txtCustomerID.Location = new System.Drawing.Point(134, 79);
            this.txtCustomerID.Name = "txtCustomerID";
            this.txtCustomerID.Size = new System.Drawing.Size(230, 23);
            this.txtCustomerID.TabIndex = 10;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 519);
            this.Controls.Add(this.panelMiddle);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelTop);
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
        private System.Windows.Forms.Button btnCloseCompany;
        private System.Windows.Forms.Button btnOpenCompany;
        private System.Windows.Forms.ComboBox cmbAppCode;
        private System.Windows.Forms.TextBox txtCompanyID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnItemDeserialize;
        private System.Windows.Forms.Button btnItemSerialize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtItemID;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.TextBox txtJSONBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCustomerDeserialize;
        private System.Windows.Forms.Button btnCustomerSerialize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCustomerID;
    }
}