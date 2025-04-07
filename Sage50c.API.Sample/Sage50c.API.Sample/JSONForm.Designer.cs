namespace Sage50c.API.Sample
{
    partial class JSONForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.BCancel = new System.Windows.Forms.Button();
            this.BOk = new System.Windows.Forms.Button();
            this.TbJSON = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BCancel);
            this.panel1.Controls.Add(this.BOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 304);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(638, 54);
            this.panel1.TabIndex = 0;
            // 
            // BCancel
            // 
            this.BCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BCancel.Location = new System.Drawing.Point(460, 12);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(85, 32);
            this.BCancel.TabIndex = 1;
            this.BCancel.Text = "Cancelar";
            this.BCancel.UseVisualStyleBackColor = true;
            this.BCancel.Click += new System.EventHandler(this.BCancel_Click);
            // 
            // BOk
            // 
            this.BOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BOk.Location = new System.Drawing.Point(551, 12);
            this.BOk.Name = "BOk";
            this.BOk.Size = new System.Drawing.Size(75, 32);
            this.BOk.TabIndex = 0;
            this.BOk.Text = "Ok";
            this.BOk.UseVisualStyleBackColor = true;
            this.BOk.Click += new System.EventHandler(this.BOk_Click);
            // 
            // TbJSON
            // 
            this.TbJSON.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbJSON.Location = new System.Drawing.Point(0, 0);
            this.TbJSON.MaxLength = 999999999;
            this.TbJSON.Multiline = true;
            this.TbJSON.Name = "TbJSON";
            this.TbJSON.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbJSON.Size = new System.Drawing.Size(638, 304);
            this.TbJSON.TabIndex = 1;
            // 
            // JSONForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 358);
            this.Controls.Add(this.TbJSON);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "JSONForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Texto JSON";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BCancel;
        private System.Windows.Forms.Button BOk;
        private System.Windows.Forms.TextBox TbJSON;
    }
}