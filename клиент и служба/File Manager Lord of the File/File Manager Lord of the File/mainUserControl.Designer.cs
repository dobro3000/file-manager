namespace File_Manager_Lord_of_the_File
{
    partial class mainUserControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.arhivindToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.desarhivingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncodingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decodingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainDataGridView = new System.Windows.Forms.DataGridView();
            this.ColumnCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnExpansion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem,
            this.createFileToolStripMenuItem,
            this.createDirectoryToolStripMenuItem,
            this.moveToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.arhivindToolStripMenuItem,
            this.desarhivingToolStripMenuItem,
            this.uncodingToolStripMenuItem,
            this.decodingToolStripMenuItem,
            this.insertToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(159, 246);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // createFileToolStripMenuItem
            // 
            this.createFileToolStripMenuItem.Name = "createFileToolStripMenuItem";
            this.createFileToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.createFileToolStripMenuItem.Text = "Create file";
            this.createFileToolStripMenuItem.Click += new System.EventHandler(this.createFileToolStripMenuItem_Click);
            // 
            // createDirectoryToolStripMenuItem
            // 
            this.createDirectoryToolStripMenuItem.Name = "createDirectoryToolStripMenuItem";
            this.createDirectoryToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.createDirectoryToolStripMenuItem.Text = "Create directory";
            this.createDirectoryToolStripMenuItem.Click += new System.EventHandler(this.createFileToolStripMenuItem_Click);
            // 
            // moveToolStripMenuItem
            // 
            this.moveToolStripMenuItem.Name = "moveToolStripMenuItem";
            this.moveToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.moveToolStripMenuItem.Text = "Move";
            this.moveToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // arhivindToolStripMenuItem
            // 
            this.arhivindToolStripMenuItem.Name = "arhivindToolStripMenuItem";
            this.arhivindToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.arhivindToolStripMenuItem.Text = "Arhiving";
            this.arhivindToolStripMenuItem.Click += new System.EventHandler(this.arhivindToolStripMenuItem_Click);
            // 
            // desarhivingToolStripMenuItem
            // 
            this.desarhivingToolStripMenuItem.Name = "desarhivingToolStripMenuItem";
            this.desarhivingToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.desarhivingToolStripMenuItem.Text = "Desarhiving";
            this.desarhivingToolStripMenuItem.Click += new System.EventHandler(this.desarhivingToolStripMenuItem_Click);
            // 
            // uncodingToolStripMenuItem
            // 
            this.uncodingToolStripMenuItem.Name = "uncodingToolStripMenuItem";
            this.uncodingToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.uncodingToolStripMenuItem.Text = "Uncoding";
            this.uncodingToolStripMenuItem.Click += new System.EventHandler(this.uncodingToolStripMenuItem_Click);
            // 
            // decodingToolStripMenuItem
            // 
            this.decodingToolStripMenuItem.Name = "decodingToolStripMenuItem";
            this.decodingToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.decodingToolStripMenuItem.Text = "Decoding";
            this.decodingToolStripMenuItem.Click += new System.EventHandler(this.decodingToolStripMenuItem_Click);
            // 
            // insertToolStripMenuItem
            // 
            this.insertToolStripMenuItem.Name = "insertToolStripMenuItem";
            this.insertToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.insertToolStripMenuItem.Text = "Insert";
            this.insertToolStripMenuItem.Visible = false;
            this.insertToolStripMenuItem.Click += new System.EventHandler(this.insertToolStripMenuItem_Click);
            // 
            // mainDataGridView
            // 
            this.mainDataGridView.AllowUserToResizeRows = false;
            this.mainDataGridView.BackgroundColor = System.Drawing.Color.LemonChiffon;
            this.mainDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            this.mainDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.mainDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mainDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCheck,
            this.ColumnName,
            this.ColumnExpansion,
            this.ColumnSize});
            this.mainDataGridView.ContextMenuStrip = this.contextMenuStrip1;
            this.mainDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainDataGridView.Location = new System.Drawing.Point(0, 0);
            this.mainDataGridView.MultiSelect = false;
            this.mainDataGridView.Name = "mainDataGridView";
            this.mainDataGridView.ReadOnly = true;
            this.mainDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.LemonChiffon;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mainDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.mainDataGridView.RowHeadersVisible = false;
            this.mainDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.mainDataGridView.Size = new System.Drawing.Size(575, 430);
            this.mainDataGridView.TabIndex = 0;
            this.mainDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mainDataGridView_CellContentClick);
            this.mainDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mainDataGridView_CellContentClick);
            this.mainDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mainDataGridView_CellDoubleClick);
            this.mainDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.mainDataGridView_CellEndEdit);
            // 
            // ColumnCheck
            // 
            this.ColumnCheck.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnCheck.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Bisque;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.ColumnCheck.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnCheck.FalseValue = "";
            this.ColumnCheck.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ColumnCheck.HeaderText = "✓";
            this.ColumnCheck.IndeterminateValue = "";
            this.ColumnCheck.Name = "ColumnCheck";
            this.ColumnCheck.ReadOnly = true;
            this.ColumnCheck.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnCheck.TrueValue = "";
            this.ColumnCheck.Width = 25;
            // 
            // ColumnName
            // 
            this.ColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnName.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Bisque;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.ColumnName.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnName.HeaderText = "Name";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            // 
            // ColumnExpansion
            // 
            this.ColumnExpansion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnExpansion.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Bisque;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.ColumnExpansion.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnExpansion.HeaderText = "Expansion";
            this.ColumnExpansion.Name = "ColumnExpansion";
            this.ColumnExpansion.ReadOnly = true;
            // 
            // ColumnSize
            // 
            this.ColumnSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnSize.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Bisque;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.ColumnSize.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColumnSize.HeaderText = "Date";
            this.ColumnSize.Name = "ColumnSize";
            this.ColumnSize.ReadOnly = true;
            // 
            // mainUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainDataGridView);
            this.Name = "mainUserControl";
            this.Size = new System.Drawing.Size(575, 430);
            this.Load += new System.EventHandler(this.mainUserControl_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem arhivindToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem desarhivingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncodingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decodingToolStripMenuItem;
        private System.Windows.Forms.DataGridView mainDataGridView;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createDirectoryToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnExpansion;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSize;
    }
}
