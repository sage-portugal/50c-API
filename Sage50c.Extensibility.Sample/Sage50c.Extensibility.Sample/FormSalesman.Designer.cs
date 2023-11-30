namespace Sage50c.ExtenderSample22 {
    partial class FormSalesman {
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
            this.txtNumSales = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridSales = new System.Windows.Forms.DataGridView();
            this.CreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Client = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTotalValue = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSales)).BeginInit();
            this.SuspendLayout();
            // 
            // txtNumSales
            // 
            this.txtNumSales.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtNumSales.Location = new System.Drawing.Point(208, 35);
            this.txtNumSales.Name = "txtNumSales";
            this.txtNumSales.ReadOnly = true;
            this.txtNumSales.Size = new System.Drawing.Size(100, 26);
            this.txtNumSales.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Número de vendas:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataGridSales
            // 
            this.dataGridSales.AllowUserToAddRows = false;
            this.dataGridSales.AllowUserToDeleteRows = false;
            this.dataGridSales.AllowUserToResizeColumns = false;
            this.dataGridSales.AllowUserToResizeRows = false;
            this.dataGridSales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridSales.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridSales.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridSales.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSales.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CreateDate,
            this.TotalAmount,
            this.Client});
            this.dataGridSales.EnableHeadersVisualStyles = false;
            this.dataGridSales.Location = new System.Drawing.Point(26, 112);
            this.dataGridSales.MultiSelect = false;
            this.dataGridSales.Name = "dataGridSales";
            this.dataGridSales.ReadOnly = true;
            this.dataGridSales.RowHeadersVisible = false;
            this.dataGridSales.RowHeadersWidth = 51;
            this.dataGridSales.RowTemplate.Height = 24;
            this.dataGridSales.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridSales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridSales.Size = new System.Drawing.Size(346, 189);
            this.dataGridSales.TabIndex = 2;
            // 
            // CreateDate
            // 
            this.CreateDate.HeaderText = "Data";
            this.CreateDate.MinimumWidth = 6;
            this.CreateDate.Name = "CreateDate";
            this.CreateDate.ReadOnly = true;
            this.CreateDate.Width = 64;
            // 
            // TotalAmount
            // 
            this.TotalAmount.HeaderText = "Valor";
            this.TotalAmount.MinimumWidth = 6;
            this.TotalAmount.Name = "TotalAmount";
            this.TotalAmount.ReadOnly = true;
            this.TotalAmount.Width = 67;
            // 
            // Client
            // 
            this.Client.HeaderText = "Cliente";
            this.Client.MinimumWidth = 6;
            this.Client.Name = "Client";
            this.Client.ReadOnly = true;
            this.Client.Width = 76;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(28, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Lucro total:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTotalValue
            // 
            this.txtTotalValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtTotalValue.Location = new System.Drawing.Point(208, 69);
            this.txtTotalValue.Name = "txtTotalValue";
            this.txtTotalValue.ReadOnly = true;
            this.txtTotalValue.Size = new System.Drawing.Size(100, 26);
            this.txtTotalValue.TabIndex = 4;
            // 
            // FormSalesman
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 563);
            this.Controls.Add(this.txtTotalValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridSales);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNumSales);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSalesman";
            this.Text = "FormSalesman";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSales)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNumSales;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridSales;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Client;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTotalValue;
    }
}