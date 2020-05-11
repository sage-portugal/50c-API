namespace Sage50c.ExtenderSample {
    partial class FormCallMenu {
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
            this.txtTransSerial = new System.Windows.Forms.TextBox();
            this.txtTransDocument = new System.Windows.Forms.TextBox();
            this.numTransdocnunber = new System.Windows.Forms.NumericUpDown();
            this.btnShowTransaction = new System.Windows.Forms.Button();
            this.txtItemId = new System.Windows.Forms.TextBox();
            this.btnShowItem = new System.Windows.Forms.Button();
            this.lblItemId = new System.Windows.Forms.Label();
            this.lblDocument = new System.Windows.Forms.Label();
            this.txtMenu = new System.Windows.Forms.TextBox();
            this.btnMenu = new System.Windows.Forms.Button();
            this.lblMenu = new System.Windows.Forms.Label();
            this.grpGroup = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numTransdocnunber)).BeginInit();
            this.grpGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTransSerial
            // 
            this.txtTransSerial.Location = new System.Drawing.Point(125, 45);
            this.txtTransSerial.Name = "txtTransSerial";
            this.txtTransSerial.Size = new System.Drawing.Size(39, 20);
            this.txtTransSerial.TabIndex = 0;
            this.txtTransSerial.Text = "1";
            // 
            // txtTransDocument
            // 
            this.txtTransDocument.Location = new System.Drawing.Point(170, 45);
            this.txtTransDocument.Name = "txtTransDocument";
            this.txtTransDocument.Size = new System.Drawing.Size(60, 20);
            this.txtTransDocument.TabIndex = 1;
            this.txtTransDocument.Text = "FCO";
            // 
            // numTransdocnunber
            // 
            this.numTransdocnunber.Location = new System.Drawing.Point(240, 45);
            this.numTransdocnunber.Name = "numTransdocnunber";
            this.numTransdocnunber.Size = new System.Drawing.Size(52, 20);
            this.numTransdocnunber.TabIndex = 2;
            // 
            // btnShowTransaction
            // 
            this.btnShowTransaction.Location = new System.Drawing.Point(300, 45);
            this.btnShowTransaction.Name = "btnShowTransaction";
            this.btnShowTransaction.Size = new System.Drawing.Size(24, 21);
            this.btnShowTransaction.TabIndex = 3;
            this.btnShowTransaction.UseVisualStyleBackColor = true;
            this.btnShowTransaction.Click += new System.EventHandler(this.btnShowTransaction_Click_1);
            // 
            // txtItemId
            // 
            this.txtItemId.Location = new System.Drawing.Point(125, 84);
            this.txtItemId.Name = "txtItemId";
            this.txtItemId.Size = new System.Drawing.Size(167, 20);
            this.txtItemId.TabIndex = 4;
            // 
            // btnShowItem
            // 
            this.btnShowItem.Location = new System.Drawing.Point(300, 83);
            this.btnShowItem.Name = "btnShowItem";
            this.btnShowItem.Size = new System.Drawing.Size(24, 21);
            this.btnShowItem.TabIndex = 5;
            this.btnShowItem.UseVisualStyleBackColor = true;
            this.btnShowItem.Click += new System.EventHandler(this.btnShowItem_Click);
            // 
            // lblItemId
            // 
            this.lblItemId.AutoSize = true;
            this.lblItemId.Location = new System.Drawing.Point(60, 87);
            this.lblItemId.Name = "lblItemId";
            this.lblItemId.Size = new System.Drawing.Size(59, 13);
            this.lblItemId.TabIndex = 6;
            this.lblItemId.Text = "Referência";
            // 
            // lblDocument
            // 
            this.lblDocument.AutoSize = true;
            this.lblDocument.Location = new System.Drawing.Point(57, 49);
            this.lblDocument.Name = "lblDocument";
            this.lblDocument.Size = new System.Drawing.Size(62, 13);
            this.lblDocument.TabIndex = 7;
            this.lblDocument.Text = "Documento";
            // 
            // txtMenu
            // 
            this.txtMenu.Location = new System.Drawing.Point(126, 121);
            this.txtMenu.Name = "txtMenu";
            this.txtMenu.Size = new System.Drawing.Size(165, 20);
            this.txtMenu.TabIndex = 8;
            this.txtMenu.Text = "miItem";
            // 
            // btnMenu
            // 
            this.btnMenu.Location = new System.Drawing.Point(300, 121);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(24, 21);
            this.btnMenu.TabIndex = 9;
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // lblMenu
            // 
            this.lblMenu.AutoSize = true;
            this.lblMenu.Location = new System.Drawing.Point(85, 125);
            this.lblMenu.Name = "lblMenu";
            this.lblMenu.Size = new System.Drawing.Size(34, 13);
            this.lblMenu.TabIndex = 10;
            this.lblMenu.Text = "Menu";
            // 
            // grpGroup
            // 
            this.grpGroup.Controls.Add(this.txtTransDocument);
            this.grpGroup.Controls.Add(this.lblMenu);
            this.grpGroup.Controls.Add(this.txtTransSerial);
            this.grpGroup.Controls.Add(this.btnMenu);
            this.grpGroup.Controls.Add(this.numTransdocnunber);
            this.grpGroup.Controls.Add(this.txtMenu);
            this.grpGroup.Controls.Add(this.btnShowTransaction);
            this.grpGroup.Controls.Add(this.lblDocument);
            this.grpGroup.Controls.Add(this.txtItemId);
            this.grpGroup.Controls.Add(this.lblItemId);
            this.grpGroup.Controls.Add(this.btnShowItem);
            this.grpGroup.Location = new System.Drawing.Point(170, 62);
            this.grpGroup.Name = "grpGroup";
            this.grpGroup.Size = new System.Drawing.Size(346, 163);
            this.grpGroup.TabIndex = 11;
            this.grpGroup.TabStop = false;
            // 
            // FormCallMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grpGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormCallMenu";
            ((System.ComponentModel.ISupportInitialize)(this.numTransdocnunber)).EndInit();
            this.grpGroup.ResumeLayout(false);
            this.grpGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtTransSerial;
        private System.Windows.Forms.TextBox txtTransDocument;
        private System.Windows.Forms.NumericUpDown numTransdocnunber;
        private System.Windows.Forms.Button btnShowTransaction;
        private System.Windows.Forms.TextBox txtItemId;
        private System.Windows.Forms.Button btnShowItem;
        private System.Windows.Forms.Label lblItemId;
        private System.Windows.Forms.Label lblDocument;
        private System.Windows.Forms.TextBox txtMenu;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Label lblMenu;
        private System.Windows.Forms.GroupBox grpGroup;
    }
}