namespace Sage50c.API.Sample
{
    partial class SystemUsers
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
            this.PSystemUsers = new System.Windows.Forms.Panel();
            this.PSystemUsersBody = new System.Windows.Forms.Panel();
            this.TbSystemUsersList = new System.Windows.Forms.TextBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.BGetSystemUsers = new System.Windows.Forms.Button();
            this.PSystemUsers.SuspendLayout();
            this.PSystemUsersBody.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // PSystemUsers
            // 
            this.PSystemUsers.Controls.Add(this.PSystemUsersBody);
            this.PSystemUsers.Controls.Add(this.panel8);
            this.PSystemUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PSystemUsers.Location = new System.Drawing.Point(0, 0);
            this.PSystemUsers.Name = "PSystemUsers";
            this.PSystemUsers.Size = new System.Drawing.Size(742, 399);
            this.PSystemUsers.TabIndex = 1;
            // 
            // PSystemUsersBody
            // 
            this.PSystemUsersBody.Controls.Add(this.TbSystemUsersList);
            this.PSystemUsersBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PSystemUsersBody.Location = new System.Drawing.Point(0, 46);
            this.PSystemUsersBody.Name = "PSystemUsersBody";
            this.PSystemUsersBody.Size = new System.Drawing.Size(742, 353);
            this.PSystemUsersBody.TabIndex = 1;
            // 
            // TbSystemUsersList
            // 
            this.TbSystemUsersList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbSystemUsersList.Location = new System.Drawing.Point(0, 0);
            this.TbSystemUsersList.Multiline = true;
            this.TbSystemUsersList.Name = "TbSystemUsersList";
            this.TbSystemUsersList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbSystemUsersList.Size = new System.Drawing.Size(742, 353);
            this.TbSystemUsersList.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.BGetSystemUsers);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(742, 46);
            this.panel8.TabIndex = 0;
            // 
            // BGetSystemUsers
            // 
            this.BGetSystemUsers.Location = new System.Drawing.Point(8, 13);
            this.BGetSystemUsers.Name = "BGetSystemUsers";
            this.BGetSystemUsers.Size = new System.Drawing.Size(198, 23);
            this.BGetSystemUsers.TabIndex = 0;
            this.BGetSystemUsers.Text = "Obter Lista Utilizadores Sistema";
            this.BGetSystemUsers.UseVisualStyleBackColor = true;
            this.BGetSystemUsers.Click += new System.EventHandler(this.BGetSystemUsers_Click);
            // 
            // SystemUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PSystemUsers);
            this.Name = "SystemUsers";
            this.Size = new System.Drawing.Size(742, 399);
            this.PSystemUsers.ResumeLayout(false);
            this.PSystemUsersBody.ResumeLayout(false);
            this.PSystemUsersBody.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PSystemUsers;
        private System.Windows.Forms.Panel PSystemUsersBody;
        private System.Windows.Forms.TextBox TbSystemUsersList;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button BGetSystemUsers;
    }
}
