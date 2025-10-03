namespace PrinterManager
{
    partial class PrinterManagerApp
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            actionsGrp = new GroupBox();
            propertiesButton = new Button();
            openPrintQueueBtn = new Button();
            refreshBtn = new Button();
            changePortBtn = new Button();
            renamePrinterBtn = new Button();
            removePrinterBtn = new Button();
            addPrinterBtn = new Button();
            setDefaultBtn = new Button();
            printerDataGrid = new DataGridView();
            isDefaultCol = new DataGridViewCheckBoxColumn();
            portCol = new DataGridViewTextBoxColumn();
            displayNameCol = new DataGridViewTextBoxColumn();
            driverNameCol = new DataGridViewTextBoxColumn();
            actionsGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)printerDataGrid).BeginInit();
            SuspendLayout();
            // 
            // actionsGrp
            // 
            actionsGrp.Controls.Add(propertiesButton);
            actionsGrp.Controls.Add(openPrintQueueBtn);
            actionsGrp.Controls.Add(refreshBtn);
            actionsGrp.Controls.Add(changePortBtn);
            actionsGrp.Controls.Add(renamePrinterBtn);
            actionsGrp.Controls.Add(removePrinterBtn);
            actionsGrp.Controls.Add(addPrinterBtn);
            actionsGrp.Controls.Add(setDefaultBtn);
            actionsGrp.Location = new Point(12, 12);
            actionsGrp.Name = "actionsGrp";
            actionsGrp.Size = new Size(920, 57);
            actionsGrp.TabIndex = 0;
            actionsGrp.TabStop = false;
            actionsGrp.Text = "Actions";
            // 
            // propertiesButton
            // 
            propertiesButton.Location = new Point(557, 22);
            propertiesButton.Name = "propertiesButton";
            propertiesButton.Size = new Size(114, 23);
            propertiesButton.TabIndex = 6;
            propertiesButton.Text = "Open Properties";
            propertiesButton.UseVisualStyleBackColor = true;
            propertiesButton.Click += propertiesButton_Click;
            // 
            // openPrintQueueBtn
            // 
            openPrintQueueBtn.Location = new Point(426, 22);
            openPrintQueueBtn.Name = "openPrintQueueBtn";
            openPrintQueueBtn.Size = new Size(125, 23);
            openPrintQueueBtn.TabIndex = 5;
            openPrintQueueBtn.Text = "Open Print Queue";
            openPrintQueueBtn.UseVisualStyleBackColor = true;
            openPrintQueueBtn.Click += openPrintQueueBtn_Click;
            // 
            // refreshBtn
            // 
            refreshBtn.Location = new Point(839, 22);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Size = new Size(75, 23);
            refreshBtn.TabIndex = 99;
            refreshBtn.Text = "Refresh";
            refreshBtn.UseVisualStyleBackColor = true;
            refreshBtn.Click += refreshBtn_Click;
            // 
            // changePortBtn
            // 
            changePortBtn.Location = new Point(249, 22);
            changePortBtn.Name = "changePortBtn";
            changePortBtn.Size = new Size(90, 23);
            changePortBtn.TabIndex = 3;
            changePortBtn.Text = "Change Port";
            changePortBtn.UseVisualStyleBackColor = true;
            changePortBtn.Click += changePortBtn_Click;
            // 
            // renamePrinterBtn
            // 
            renamePrinterBtn.Location = new Point(168, 22);
            renamePrinterBtn.Name = "renamePrinterBtn";
            renamePrinterBtn.Size = new Size(75, 23);
            renamePrinterBtn.TabIndex = 2;
            renamePrinterBtn.Text = "Rename";
            renamePrinterBtn.UseVisualStyleBackColor = true;
            renamePrinterBtn.Click += renamePrinterBtn_Click;
            // 
            // removePrinterBtn
            // 
            removePrinterBtn.Location = new Point(87, 22);
            removePrinterBtn.Name = "removePrinterBtn";
            removePrinterBtn.Size = new Size(75, 23);
            removePrinterBtn.TabIndex = 1;
            removePrinterBtn.Text = "Remove";
            removePrinterBtn.UseVisualStyleBackColor = true;
            removePrinterBtn.Click += removePrinterBtn_Click;
            // 
            // addPrinterBtn
            // 
            addPrinterBtn.Location = new Point(6, 22);
            addPrinterBtn.Name = "addPrinterBtn";
            addPrinterBtn.Size = new Size(75, 23);
            addPrinterBtn.TabIndex = 0;
            addPrinterBtn.Text = "Add";
            addPrinterBtn.UseVisualStyleBackColor = true;
            addPrinterBtn.Click += addPrinterBtn_Click;
            // 
            // setDefaultBtn
            // 
            setDefaultBtn.Location = new Point(345, 22);
            setDefaultBtn.Name = "setDefaultBtn";
            setDefaultBtn.Size = new Size(75, 23);
            setDefaultBtn.TabIndex = 4;
            setDefaultBtn.Text = "Set Default";
            setDefaultBtn.UseVisualStyleBackColor = true;
            setDefaultBtn.Click += setDefaultBtn_Click;
            // 
            // printerDataGrid
            // 
            printerDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            printerDataGrid.Columns.AddRange(new DataGridViewColumn[] { isDefaultCol, portCol, displayNameCol, driverNameCol });
            printerDataGrid.Location = new Point(12, 75);
            printerDataGrid.Name = "printerDataGrid";
            printerDataGrid.Size = new Size(920, 514);
            printerDataGrid.TabIndex = 1;
            printerDataGrid.CellContentClick += dataGridView1_CellContentClick;
            // 
            // isDefaultCol
            // 
            isDefaultCol.DataPropertyName = "IsDefault";
            isDefaultCol.HeaderText = "Is Default";
            isDefaultCol.MinimumWidth = 62;
            isDefaultCol.Name = "isDefaultCol";
            isDefaultCol.ReadOnly = true;
            isDefaultCol.Width = 62;
            // 
            // portCol
            // 
            portCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            portCol.DataPropertyName = "Port";
            portCol.HeaderText = "Port*";
            portCol.Name = "portCol";
            portCol.ReadOnly = true;
            // 
            // displayNameCol
            // 
            displayNameCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            displayNameCol.DataPropertyName = "DisplayName";
            displayNameCol.HeaderText = "Display Name";
            displayNameCol.Name = "displayNameCol";
            displayNameCol.ReadOnly = true;
            // 
            // driverNameCol
            // 
            driverNameCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            driverNameCol.DataPropertyName = "DriverName";
            driverNameCol.HeaderText = "Driver Name";
            driverNameCol.Name = "driverNameCol";
            driverNameCol.ReadOnly = true;
            // 
            // PrinterManagerApp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(944, 601);
            Controls.Add(printerDataGrid);
            Controls.Add(actionsGrp);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "PrinterManagerApp";
            Text = "Printer Manager";
            actionsGrp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)printerDataGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox actionsGrp;
        private Button changePortBtn;
        private Button renamePrinterBtn;
        private Button removePrinterBtn;
        private Button addPrinterBtn;
        private Button setDefaultBtn;
        private Button refreshBtn;
        private DataGridView printerDataGrid;
        private DataGridViewCheckBoxColumn isDefaultCol;
        private DataGridViewTextBoxColumn portCol;
        private DataGridViewTextBoxColumn displayNameCol;
        private DataGridViewTextBoxColumn driverNameCol;
        private Button openPrintQueueBtn;
        private Button propertiesButton;
    }
}
