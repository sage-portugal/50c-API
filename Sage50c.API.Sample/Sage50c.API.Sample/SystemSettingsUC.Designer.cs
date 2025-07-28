namespace Sage50c.API.Sample
{
    partial class SystemSettingsUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PSystemSettings = new System.Windows.Forms.Panel();
            this.PSystemSettingsBody = new System.Windows.Forms.Panel();
            this.TbSystemSettingsList = new System.Windows.Forms.TextBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.BGetSystemSettings = new System.Windows.Forms.Button();
            this.PSystemSettings.SuspendLayout();
            this.PSystemSettingsBody.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // PSystemSettings
            // 
            this.PSystemSettings.Controls.Add(this.PSystemSettingsBody);
            this.PSystemSettings.Controls.Add(this.panel8);
            this.PSystemSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PSystemSettings.Location = new System.Drawing.Point(0, 0);
            this.PSystemSettings.Name = "PSystemSettings";
            this.PSystemSettings.Size = new System.Drawing.Size(742, 399);
            this.PSystemSettings.TabIndex = 1;
            // 
            // PSystemSettingsBody
            // 
            this.PSystemSettingsBody.Controls.Add(this.TbSystemSettingsList);
            this.PSystemSettingsBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PSystemSettingsBody.Location = new System.Drawing.Point(0, 46);
            this.PSystemSettingsBody.Name = "PSystemSettingsBody";
            this.PSystemSettingsBody.Size = new System.Drawing.Size(742, 353);
            this.PSystemSettingsBody.TabIndex = 1;
            // 
            // TbSystemSettingsList
            // 
            this.TbSystemSettingsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbSystemSettingsList.Location = new System.Drawing.Point(0, 0);
            this.TbSystemSettingsList.Multiline = true;
            this.TbSystemSettingsList.Name = "TbSystemSettingsList";
            this.TbSystemSettingsList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbSystemSettingsList.Size = new System.Drawing.Size(742, 353);
            this.TbSystemSettingsList.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.BGetSystemSettings);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(742, 46);
            this.panel8.TabIndex = 0;
            // 
            // BGetSystemSettings
            // 
            this.BGetSystemSettings.Location = new System.Drawing.Point(8, 13);
            this.BGetSystemSettings.Name = "BGetSystemSettings";
            this.BGetSystemSettings.Size = new System.Drawing.Size(198, 23);
            this.BGetSystemSettings.TabIndex = 0;
            this.BGetSystemSettings.Text = "Obter Configurações do Sistema";
            this.BGetSystemSettings.UseVisualStyleBackColor = true;
            this.BGetSystemSettings.Click += new System.EventHandler(this.BGetSystemSettings_Click);
            // 
            // SystemSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PSystemSettings);
            this.Name = "SystemSettings";
            this.Size = new System.Drawing.Size(742, 399);
            this.PSystemSettings.ResumeLayout(false);
            this.PSystemSettingsBody.ResumeLayout(false);
            this.PSystemSettingsBody.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PSystemSettings;
        private System.Windows.Forms.Panel PSystemSettingsBody;
        private System.Windows.Forms.TextBox TbSystemSettingsList;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button BGetSystemSettings;
    }
}
