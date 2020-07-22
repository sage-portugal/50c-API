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
            this.btnProgressStart = new System.Windows.Forms.Button();
            this.btnProgressEnd = new System.Windows.Forms.Button();
            this.btnProgressEnd2 = new System.Windows.Forms.Button();
            this.btnProgressStart2 = new System.Windows.Forms.Button();
            this.btnProgressEnd3 = new System.Windows.Forms.Button();
            this.btnProgressStart3 = new System.Windows.Forms.Button();
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
            this.btnItemQuickSearch.Location = new System.Drawing.Point(12, 36);
            this.btnItemQuickSearch.Name = "btnItemQuickSearch";
            this.btnItemQuickSearch.Size = new System.Drawing.Size(155, 32);
            this.btnItemQuickSearch.TabIndex = 1;
            this.btnItemQuickSearch.Text = "Item QuickSearch";
            this.btnItemQuickSearch.UseVisualStyleBackColor = true;
            this.btnItemQuickSearch.Click += new System.EventHandler(this.btnItemQuickSearch_Click);
            // 
            // btnProgressStart
            // 
            this.btnProgressStart.Location = new System.Drawing.Point(480, 36);
            this.btnProgressStart.Name = "btnProgressStart";
            this.btnProgressStart.Size = new System.Drawing.Size(107, 32);
            this.btnProgressStart.TabIndex = 2;
            this.btnProgressStart.Text = "Progress Start";
            this.btnProgressStart.UseVisualStyleBackColor = true;
            this.btnProgressStart.Click += new System.EventHandler(this.btnProgressStart_Click);
            // 
            // btnProgressEnd
            // 
            this.btnProgressEnd.Location = new System.Drawing.Point(609, 36);
            this.btnProgressEnd.Name = "btnProgressEnd";
            this.btnProgressEnd.Size = new System.Drawing.Size(106, 32);
            this.btnProgressEnd.TabIndex = 3;
            this.btnProgressEnd.Text = "Progress End";
            this.btnProgressEnd.UseVisualStyleBackColor = true;
            this.btnProgressEnd.Click += new System.EventHandler(this.btnProgressEnd_Click);
            // 
            // btnProgressEnd2
            // 
            this.btnProgressEnd2.Location = new System.Drawing.Point(609, 99);
            this.btnProgressEnd2.Name = "btnProgressEnd2";
            this.btnProgressEnd2.Size = new System.Drawing.Size(106, 32);
            this.btnProgressEnd2.TabIndex = 5;
            this.btnProgressEnd2.Text = "Progress End 2";
            this.btnProgressEnd2.UseVisualStyleBackColor = true;
            this.btnProgressEnd2.Click += new System.EventHandler(this.btnProgressEnd2_Click);
            // 
            // btnProgressStart2
            // 
            this.btnProgressStart2.Location = new System.Drawing.Point(480, 99);
            this.btnProgressStart2.Name = "btnProgressStart2";
            this.btnProgressStart2.Size = new System.Drawing.Size(107, 32);
            this.btnProgressStart2.TabIndex = 4;
            this.btnProgressStart2.Text = "Progress Start 2";
            this.btnProgressStart2.UseVisualStyleBackColor = true;
            this.btnProgressStart2.Click += new System.EventHandler(this.btnProgressStart2_Click);
            // 
            // btnProgressEnd3
            // 
            this.btnProgressEnd3.Location = new System.Drawing.Point(609, 152);
            this.btnProgressEnd3.Name = "btnProgressEnd3";
            this.btnProgressEnd3.Size = new System.Drawing.Size(106, 32);
            this.btnProgressEnd3.TabIndex = 7;
            this.btnProgressEnd3.Text = "Progress End 3";
            this.btnProgressEnd3.UseVisualStyleBackColor = true;
            this.btnProgressEnd3.Click += new System.EventHandler(this.btnProgressEnd3_Click);
            // 
            // btnProgressStart3
            // 
            this.btnProgressStart3.Location = new System.Drawing.Point(480, 152);
            this.btnProgressStart3.Name = "btnProgressStart3";
            this.btnProgressStart3.Size = new System.Drawing.Size(107, 32);
            this.btnProgressStart3.TabIndex = 6;
            this.btnProgressStart3.Text = "Progress Start 3";
            this.btnProgressStart3.UseVisualStyleBackColor = true;
            this.btnProgressStart3.Click += new System.EventHandler(this.btnProgressStart3_Click);
            // 
            // FormItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 419);
            this.Controls.Add(this.btnProgressEnd3);
            this.Controls.Add(this.btnProgressStart3);
            this.Controls.Add(this.btnProgressEnd2);
            this.Controls.Add(this.btnProgressStart2);
            this.Controls.Add(this.btnProgressEnd);
            this.Controls.Add(this.btnProgressStart);
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
        private System.Windows.Forms.Button btnProgressStart;
        private System.Windows.Forms.Button btnProgressEnd;
        private System.Windows.Forms.Button btnProgressEnd2;
        private System.Windows.Forms.Button btnProgressStart2;
        private System.Windows.Forms.Button btnProgressEnd3;
        private System.Windows.Forms.Button btnProgressStart3;
    }
}