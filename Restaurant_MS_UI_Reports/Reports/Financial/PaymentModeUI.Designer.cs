namespace Pharmacy.UI.Reports.Financial
{
    partial class PaymentModeUI
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            label3 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            lblTotalRevenue = new Label();
            label4 = new Label();
            grdData = new DataGridView();
            colInvoiceId = new DataGridViewTextBoxColumn();
            colPatient = new DataGridViewTextBoxColumn();
            colDescription = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colPaid = new DataGridViewTextBoxColumn();
            colPaymentMode = new DataGridViewTextBoxColumn();
            colPaymentDate = new DataGridViewTextBoxColumn();
            colIsProcedureInvoice = new DataGridViewTextBoxColumn();
            colPrint = new DataGridViewButtonColumn();
            btnPrint = new Button();
            btnExcel = new Button();
            dlgSavePdf = new SaveFileDialog();
            dlgSaveExcel = new SaveFileDialog();
            dtpFrom = new DateTimePicker();
            dtpTo = new DateTimePicker();
            label1 = new Label();
            cmbPaymentMode = new ComboBox();
            label5 = new Label();
            panel5 = new Panel();
            panel2 = new Panel();
            txtGotoPage = new TextBox();
            label2 = new Label();
            btnLastPage = new Button();
            btnFirstPage = new Button();
            btnNext = new Button();
            lblPageNo = new Label();
            btnPrevious = new Button();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((ISupportInitialize)pictureBox1).BeginInit();
            ((ISupportInitialize)grdData).BeginInit();
            panel5.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(277, 9);
            label3.Name = "label3";
            label3.Size = new Size(43, 20);
            label3.TabIndex = 2;
            label3.Text = "From";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.36331F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Location = new Point(15, 65);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(409, 121);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(lblTotalRevenue);
            panel1.Controls.Add(label4);
            panel1.Location = new Point(10, 3);
            panel1.Margin = new Padding(10, 3, 10, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(389, 115);
            panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox1.Location = new Point(279, 16);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(91, 80);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // lblTotalRevenue
            // 
            lblTotalRevenue.AutoSize = true;
            lblTotalRevenue.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0, true);
            lblTotalRevenue.ForeColor = Color.FromArgb(42, 196, 244);
            lblTotalRevenue.Location = new Point(26, 51);
            lblTotalRevenue.Name = "lblTotalRevenue";
            lblTotalRevenue.Size = new Size(27, 29);
            lblTotalRevenue.TabIndex = 0;
            lblTotalRevenue.Text = "0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(14, 16);
            label4.Name = "label4";
            label4.Size = new Size(133, 24);
            label4.TabIndex = 0;
            label4.Text = "Total Revenue";
            // 
            // grdData
            // 
            grdData.AllowUserToAddRows = false;
            grdData.AllowUserToDeleteRows = false;
            grdData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grdData.BackgroundColor = Color.White;
            grdData.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(230, 230, 230);
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            grdData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            grdData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdData.Columns.AddRange(new DataGridViewColumn[] { colInvoiceId, colPatient, colDescription, colTotal, colPaid, colPaymentMode, colPaymentDate, colIsProcedureInvoice, colPrint });
            grdData.EnableHeadersVisualStyles = false;
            grdData.Location = new Point(3, 3);
            grdData.Name = "grdData";
            grdData.RowHeadersVisible = false;
            grdData.RowHeadersWidth = 51;
            grdData.RowTemplate.Height = 35;
            grdData.Size = new Size(1092, 422);
            grdData.TabIndex = 5;
            grdData.CellContentClick += grdData_CellContentClick;
            // 
            // colInvoiceId
            // 
            colInvoiceId.HeaderText = "INVOICE NO";
            colInvoiceId.MinimumWidth = 6;
            colInvoiceId.Name = "colInvoiceId";
            colInvoiceId.ReadOnly = true;
            colInvoiceId.Width = 125;
            // 
            // colPatient
            // 
            colPatient.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colPatient.HeaderText = "PATIENT";
            colPatient.MinimumWidth = 6;
            colPatient.Name = "colPatient";
            colPatient.ReadOnly = true;
            // 
            // colDescription
            // 
            colDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDescription.HeaderText = "Description";
            colDescription.MinimumWidth = 6;
            colDescription.Name = "colDescription";
            colDescription.ReadOnly = true;
            // 
            // colTotal
            // 
            colTotal.HeaderText = "TOTAL";
            colTotal.MinimumWidth = 6;
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            colTotal.Width = 125;
            // 
            // colPaid
            // 
            colPaid.HeaderText = "PAID";
            colPaid.MinimumWidth = 6;
            colPaid.Name = "colPaid";
            colPaid.ReadOnly = true;
            colPaid.Width = 125;
            // 
            // colPaymentMode
            // 
            colPaymentMode.HeaderText = "Mode Of Payment";
            colPaymentMode.MinimumWidth = 6;
            colPaymentMode.Name = "colPaymentMode";
            colPaymentMode.ReadOnly = true;
            colPaymentMode.Width = 125;
            // 
            // colPaymentDate
            // 
            colPaymentDate.HeaderText = "PAYMENT DATE";
            colPaymentDate.MinimumWidth = 6;
            colPaymentDate.Name = "colPaymentDate";
            colPaymentDate.ReadOnly = true;
            colPaymentDate.Width = 125;
            // 
            // colIsProcedureInvoice
            // 
            colIsProcedureInvoice.HeaderText = "Is Procedure Invoice";
            colIsProcedureInvoice.MinimumWidth = 6;
            colIsProcedureInvoice.Name = "colIsProcedureInvoice";
            colIsProcedureInvoice.ReadOnly = true;
            colIsProcedureInvoice.Visible = false;
            colIsProcedureInvoice.Width = 125;
            // 
            // colPrint
            // 
            colPrint.HeaderText = "PRINT";
            colPrint.MinimumWidth = 6;
            colPrint.Name = "colPrint";
            colPrint.ReadOnly = true;
            colPrint.Resizable = DataGridViewTriState.True;
            colPrint.SortMode = DataGridViewColumnSortMode.Automatic;
            colPrint.Text = "Print";
            colPrint.UseColumnTextForButtonValue = true;
            colPrint.Width = 50;
            // 
            // btnPrint
            // 
            btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPrint.Location = new Point(1049, 422);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(58, 33);
            btnPrint.TabIndex = 6;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Click += btnPrint_Click;
            // 
            // btnExcel
            // 
            btnExcel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExcel.Location = new Point(985, 422);
            btnExcel.Name = "btnExcel";
            btnExcel.Size = new Size(58, 33);
            btnExcel.TabIndex = 6;
            btnExcel.Text = "Excel";
            btnExcel.UseVisualStyleBackColor = true;
            btnExcel.Click += btnExcel_Click;
            // 
            // dlgSavePdf
            // 
            dlgSavePdf.FileName = "Financial Payment Mode";
            dlgSavePdf.Filter = "PDF|*.pdf";
            // 
            // dlgSaveExcel
            // 
            dlgSaveExcel.FileName = "Financial Payment Mode";
            dlgSaveExcel.Filter = "Excel|*.xls";
            // 
            // dtpFrom
            // 
            dtpFrom.Location = new Point(277, 28);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(200, 27);
            dtpFrom.TabIndex = 0;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // dtpTo
            // 
            dtpTo.Location = new Point(483, 28);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(200, 27);
            dtpTo.TabIndex = 0;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(480, 9);
            label1.Name = "label1";
            label1.Size = new Size(25, 20);
            label1.TabIndex = 2;
            label1.Text = "To";
            // 
            // cmbPaymentMode
            // 
            cmbPaymentMode.FormattingEnabled = true;
            cmbPaymentMode.Items.AddRange(new object[] { "All", "Cash", "Cheque", "Debit/Credit Card", "Online" });
            cmbPaymentMode.Location = new Point(25, 27);
            cmbPaymentMode.Name = "cmbPaymentMode";
            cmbPaymentMode.Size = new Size(249, 28);
            cmbPaymentMode.TabIndex = 12;
            cmbPaymentMode.SelectedIndexChanged += cmbPaymentMode_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(22, 9);
            label5.Name = "label5";
            label5.Size = new Size(108, 20);
            label5.TabIndex = 2;
            label5.Text = "Payment Mode";
            // 
            // panel5
            // 
            panel5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel5.Controls.Add(panel2);
            panel5.Controls.Add(grdData);
            panel5.Location = new Point(12, 458);
            panel5.Name = "panel5";
            panel5.Size = new Size(1098, 467);
            panel5.TabIndex = 13;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Bottom;
            panel2.BackColor = Color.White;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(txtGotoPage);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(btnLastPage);
            panel2.Controls.Add(btnFirstPage);
            panel2.Controls.Add(btnNext);
            panel2.Controls.Add(lblPageNo);
            panel2.Controls.Add(btnPrevious);
            panel2.Location = new Point(295, 430);
            panel2.Name = "panel2";
            panel2.Size = new Size(526, 31);
            panel2.TabIndex = 17;
            // 
            // txtGotoPage
            // 
            txtGotoPage.Location = new Point(443, 5);
            txtGotoPage.Name = "txtGotoPage";
            txtGotoPage.Size = new Size(75, 27);
            txtGotoPage.TabIndex = 1;
            txtGotoPage.KeyDown += txtGotoPage_KeyDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(373, 8);
            label2.Name = "label2";
            label2.Size = new Size(83, 20);
            label2.TabIndex = 0;
            label2.Text = "go to page";
            // 
            // btnLastPage
            // 
            btnLastPage.BackColor = Color.White;
            btnLastPage.FlatAppearance.BorderSize = 0;
            btnLastPage.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 152, 210);
            btnLastPage.FlatStyle = FlatStyle.Flat;
            btnLastPage.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLastPage.ForeColor = Color.Black;
            btnLastPage.Location = new Point(333, 2);
            btnLastPage.Name = "btnLastPage";
            btnLastPage.Size = new Size(35, 25);
            btnLastPage.TabIndex = 11;
            btnLastPage.Text = ">>";
            btnLastPage.UseVisualStyleBackColor = false;
            btnLastPage.Click += btnLastPage_Click;
            // 
            // btnFirstPage
            // 
            btnFirstPage.BackColor = Color.White;
            btnFirstPage.FlatAppearance.BorderSize = 0;
            btnFirstPage.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 152, 210);
            btnFirstPage.FlatStyle = FlatStyle.Flat;
            btnFirstPage.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnFirstPage.ForeColor = Color.Black;
            btnFirstPage.Location = new Point(26, 2);
            btnFirstPage.Name = "btnFirstPage";
            btnFirstPage.Size = new Size(33, 25);
            btnFirstPage.TabIndex = 14;
            btnFirstPage.Text = "<<";
            btnFirstPage.UseVisualStyleBackColor = false;
            btnFirstPage.Click += btnFirstPage_Click;
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.White;
            btnNext.Enabled = false;
            btnNext.FlatAppearance.BorderSize = 0;
            btnNext.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 152, 210);
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNext.ForeColor = Color.Black;
            btnNext.Location = new Point(302, 2);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(27, 25);
            btnNext.TabIndex = 10;
            btnNext.Text = "> ";
            btnNext.UseVisualStyleBackColor = false;
            btnNext.Click += btnNext_Click;
            // 
            // lblPageNo
            // 
            lblPageNo.BackColor = Color.White;
            lblPageNo.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPageNo.ForeColor = Color.Black;
            lblPageNo.Location = new Point(97, 3);
            lblPageNo.MinimumSize = new Size(0, 22);
            lblPageNo.Name = "lblPageNo";
            lblPageNo.Size = new Size(199, 22);
            lblPageNo.TabIndex = 7;
            lblPageNo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnPrevious
            // 
            btnPrevious.BackColor = Color.White;
            btnPrevious.Enabled = false;
            btnPrevious.FlatAppearance.BorderSize = 0;
            btnPrevious.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 152, 210);
            btnPrevious.FlatStyle = FlatStyle.Flat;
            btnPrevious.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPrevious.ForeColor = Color.Black;
            btnPrevious.Location = new Point(63, 2);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(28, 25);
            btnPrevious.TabIndex = 13;
            btnPrevious.Text = "<";
            btnPrevious.UseVisualStyleBackColor = false;
            btnPrevious.Click += btnPrevious_Click;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "INVOICE NO";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn2.HeaderText = "PATIENT";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn3.HeaderText = "TOTAL";
            dataGridViewTextBoxColumn3.MinimumWidth = 6;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn4.HeaderText = "PAID";
            dataGridViewTextBoxColumn4.MinimumWidth = 6;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "PAYMENT DATE";
            dataGridViewTextBoxColumn5.MinimumWidth = 6;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            dataGridViewTextBoxColumn5.Width = 125;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "CREATE DATE";
            dataGridViewTextBoxColumn6.MinimumWidth = 6;
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            dataGridViewTextBoxColumn6.Width = 125;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "PRINT";
            dataGridViewTextBoxColumn7.MinimumWidth = 6;
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.ReadOnly = true;
            dataGridViewTextBoxColumn7.Width = 125;
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.HeaderText = "EDIT";
            dataGridViewTextBoxColumn8.MinimumWidth = 6;
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            dataGridViewTextBoxColumn8.ReadOnly = true;
            dataGridViewTextBoxColumn8.Width = 125;
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewTextBoxColumn9.HeaderText = "RETURN";
            dataGridViewTextBoxColumn9.MinimumWidth = 6;
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            dataGridViewTextBoxColumn9.ReadOnly = true;
            dataGridViewTextBoxColumn9.Visible = false;
            dataGridViewTextBoxColumn9.Width = 125;
            // 
            // PaymentModeUI
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoScroll = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.White;
            ClientSize = new Size(1240, 738);
            Controls.Add(panel5);
            Controls.Add(cmbPaymentMode);
            Controls.Add(btnExcel);
            Controls.Add(btnPrint);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(dtpTo);
            Controls.Add(dtpFrom);
            Name = "PaymentModeUI";
            Text = "Payment Mode";
            Load += TransactionUI_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((ISupportInitialize)pictureBox1).EndInit();
            ((ISupportInitialize)grdData).EndInit();
            panel5.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView grdData;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTotalRevenue;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.SaveFileDialog dlgSavePdf;
        private System.Windows.Forms.SaveFileDialog dlgSaveExcel;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPaymentMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel5;
        //private System.Windows.Forms.DataVisualization.Charting.Chart ChartPayments;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtGotoPage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblPageNo;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaymentMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaymentDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsProcedureInvoice;
        private System.Windows.Forms.DataGridViewButtonColumn colPrint;
    }
}