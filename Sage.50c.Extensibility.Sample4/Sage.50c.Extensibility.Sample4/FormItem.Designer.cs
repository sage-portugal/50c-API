namespace Sage50c.ExtenderSample {
    partial class FormItem {
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
            this.btnItemQuickSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(636, 375);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(93, 32);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnItemQuickSearch
            // 
            this.btnItemQuickSearch.Location = new System.Drawing.Point(288, 163);
            this.btnItemQuickSearch.Name = "btnItemQuickSearch";
            this.btnItemQuickSearch.Size = new System.Drawing.Size(155, 32);
            this.btnItemQuickSearch.TabIndex = 1;
            this.btnItemQuickSearch.Text = "Item QuickSearch";
            this.btnItemQuickSearch.UseVisualStyleBackColor = true;
            this.btnItemQuickSearch.Click += new System.EventHandler(this.btnItemQuickSearch_Click);
            // 
            // FormItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 419);
            this.Controls.Add(this.btnItemQuickSearch);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormItem";
            this.Text = "FormItem";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormItem_FormClosed);
            this.Load += new System.EventHandler(this.FormItem_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnItemQuickSearch;
    }
}