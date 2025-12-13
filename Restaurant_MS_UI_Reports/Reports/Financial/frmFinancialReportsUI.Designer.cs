namespace Pharmacy.UI.Reports.Financial
{
    partial class frmFinancialReportsUI
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
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmFinancialReportsUI));
            imageList1 = new ImageList(components);
            btnTransaction = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            btnPaymentMode = new Button();
            btnStaff = new Button();
            btnShift = new Button();
            btnRefundReport = new Button();
            btnDeletedInvoices = new Button();
            btnPendingPayments = new Button();
            panel1 = new Panel();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth8Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "default-patient.png");
            imageList1.Images.SetKeyName(1, "Refresh_16.png");
            // 
            // btnTransaction
            // 
            btnTransaction.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnTransaction.AutoSize = true;
            btnTransaction.BackColor = Color.White;
            btnTransaction.FlatAppearance.BorderColor = Color.FromArgb(42, 196, 244);
            btnTransaction.FlatAppearance.BorderSize = 0;
            btnTransaction.FlatStyle = FlatStyle.Flat;
            btnTransaction.Font = new Font("Microsoft Sans Serif", 11F);
            btnTransaction.Image = (Image)resources.GetObject("btnTransaction.Image");
            btnTransaction.ImageAlign = ContentAlignment.MiddleRight;
            btnTransaction.Location = new Point(27, 8);
            btnTransaction.Margin = new Padding(27, 8, 27, 5);
            btnTransaction.Name = "btnTransaction";
            btnTransaction.Size = new Size(304, 152);
            btnTransaction.TabIndex = 0;
            btnTransaction.Text = "Transaction";
            btnTransaction.TextAlign = ContentAlignment.TopLeft;
            btnTransaction.UseCompatibleTextRendering = true;
            btnTransaction.UseVisualStyleBackColor = false;
            btnTransaction.Click += btnTransaction_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(btnPaymentMode, 3, 0);
            tableLayoutPanel1.Controls.Add(btnStaff, 2, 0);
            tableLayoutPanel1.Controls.Add(btnShift, 1, 0);
            tableLayoutPanel1.Controls.Add(btnTransaction, 0, 0);
            tableLayoutPanel1.Controls.Add(btnRefundReport, 2, 1);
            tableLayoutPanel1.Controls.Add(btnDeletedInvoices, 1, 1);
            tableLayoutPanel1.Controls.Add(btnPendingPayments, 0, 1);
            tableLayoutPanel1.Location = new Point(16, 149);
            tableLayoutPanel1.Margin = new Padding(4, 5, 4, 5);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(1435, 331);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // btnPaymentMode
            // 
            btnPaymentMode.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnPaymentMode.AutoSize = true;
            btnPaymentMode.BackColor = Color.White;
            btnPaymentMode.FlatAppearance.BorderColor = Color.FromArgb(42, 196, 244);
            btnPaymentMode.FlatAppearance.BorderSize = 0;
            btnPaymentMode.FlatStyle = FlatStyle.Flat;
            btnPaymentMode.Font = new Font("Microsoft Sans Serif", 11F);
            btnPaymentMode.Image = (Image)resources.GetObject("btnPaymentMode.Image");
            btnPaymentMode.ImageAlign = ContentAlignment.MiddleRight;
            btnPaymentMode.Location = new Point(1101, 8);
            btnPaymentMode.Margin = new Padding(27, 8, 27, 5);
            btnPaymentMode.Name = "btnPaymentMode";
            btnPaymentMode.Size = new Size(307, 152);
            btnPaymentMode.TabIndex = 3;
            btnPaymentMode.Text = "Payment Mode";
            btnPaymentMode.TextAlign = ContentAlignment.TopLeft;
            btnPaymentMode.UseCompatibleTextRendering = true;
            btnPaymentMode.UseVisualStyleBackColor = false;
            btnPaymentMode.Click += btnPaymentMode_Click;
            // 
            // btnStaff
            // 
            btnStaff.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnStaff.AutoSize = true;
            btnStaff.BackColor = Color.White;
            btnStaff.FlatAppearance.BorderColor = Color.FromArgb(42, 196, 244);
            btnStaff.FlatAppearance.BorderSize = 0;
            btnStaff.FlatStyle = FlatStyle.Flat;
            btnStaff.Font = new Font("Microsoft Sans Serif", 11F);
            btnStaff.Image = (Image)resources.GetObject("btnStaff.Image");
            btnStaff.ImageAlign = ContentAlignment.MiddleRight;
            btnStaff.Location = new Point(743, 8);
            btnStaff.Margin = new Padding(27, 8, 27, 5);
            btnStaff.Name = "btnStaff";
            btnStaff.Size = new Size(304, 152);
            btnStaff.TabIndex = 2;
            btnStaff.Text = "Staff";
            btnStaff.TextAlign = ContentAlignment.TopLeft;
            btnStaff.UseCompatibleTextRendering = true;
            btnStaff.UseVisualStyleBackColor = false;
            // 
            // btnShift
            // 
            btnShift.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnShift.AutoSize = true;
            btnShift.BackColor = Color.White;
            btnShift.FlatAppearance.BorderColor = Color.FromArgb(42, 196, 244);
            btnShift.FlatAppearance.BorderSize = 0;
            btnShift.FlatStyle = FlatStyle.Flat;
            btnShift.Font = new Font("Microsoft Sans Serif", 11F);
            btnShift.Image = (Image)resources.GetObject("btnShift.Image");
            btnShift.ImageAlign = ContentAlignment.MiddleRight;
            btnShift.Location = new Point(385, 8);
            btnShift.Margin = new Padding(27, 8, 27, 5);
            btnShift.Name = "btnShift";
            btnShift.Size = new Size(304, 152);
            btnShift.TabIndex = 1;
            btnShift.Text = "Shift";
            btnShift.TextAlign = ContentAlignment.TopLeft;
            btnShift.UseCompatibleTextRendering = true;
            btnShift.UseVisualStyleBackColor = false;
            btnShift.Click += btnShift_Click;
            // 
            // btnRefundReport
            // 
            btnRefundReport.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnRefundReport.AutoSize = true;
            btnRefundReport.BackColor = Color.White;
            btnRefundReport.FlatAppearance.BorderColor = Color.FromArgb(42, 196, 244);
            btnRefundReport.FlatAppearance.BorderSize = 0;
            btnRefundReport.FlatStyle = FlatStyle.Flat;
            btnRefundReport.Font = new Font("Microsoft Sans Serif", 11F);
            btnRefundReport.Image = (Image)resources.GetObject("btnRefundReport.Image");
            btnRefundReport.ImageAlign = ContentAlignment.MiddleRight;
            btnRefundReport.Location = new Point(743, 173);
            btnRefundReport.Margin = new Padding(27, 8, 27, 5);
            btnRefundReport.Name = "btnRefundReport";
            btnRefundReport.Size = new Size(304, 152);
            btnRefundReport.TabIndex = 5;
            btnRefundReport.Text = "Refund Report";
            btnRefundReport.TextAlign = ContentAlignment.TopLeft;
            btnRefundReport.UseCompatibleTextRendering = true;
            btnRefundReport.UseVisualStyleBackColor = false;
            btnRefundReport.Click += btnRefundReport_Click;
            // 
            // btnDeletedInvoices
            // 
            btnDeletedInvoices.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnDeletedInvoices.AutoSize = true;
            btnDeletedInvoices.BackColor = Color.White;
            btnDeletedInvoices.FlatAppearance.BorderColor = Color.FromArgb(42, 196, 244);
            btnDeletedInvoices.FlatAppearance.BorderSize = 0;
            btnDeletedInvoices.FlatStyle = FlatStyle.Flat;
            btnDeletedInvoices.Font = new Font("Microsoft Sans Serif", 11F);
            btnDeletedInvoices.Image = (Image)resources.GetObject("btnDeletedInvoices.Image");
            btnDeletedInvoices.ImageAlign = ContentAlignment.MiddleRight;
            btnDeletedInvoices.Location = new Point(385, 173);
            btnDeletedInvoices.Margin = new Padding(27, 8, 27, 5);
            btnDeletedInvoices.Name = "btnDeletedInvoices";
            btnDeletedInvoices.Size = new Size(304, 152);
            btnDeletedInvoices.TabIndex = 4;
            btnDeletedInvoices.Text = "Deleted Invoice";
            btnDeletedInvoices.TextAlign = ContentAlignment.TopLeft;
            btnDeletedInvoices.UseCompatibleTextRendering = true;
            btnDeletedInvoices.UseVisualStyleBackColor = false;
            // 
            // btnPendingPayments
            // 
            btnPendingPayments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnPendingPayments.AutoSize = true;
            btnPendingPayments.BackColor = Color.White;
            btnPendingPayments.FlatAppearance.BorderColor = Color.FromArgb(42, 196, 244);
            btnPendingPayments.FlatAppearance.BorderSize = 0;
            btnPendingPayments.FlatStyle = FlatStyle.Flat;
            btnPendingPayments.Font = new Font("Microsoft Sans Serif", 11F);
            btnPendingPayments.Image = (Image)resources.GetObject("btnPendingPayments.Image");
            btnPendingPayments.ImageAlign = ContentAlignment.MiddleRight;
            btnPendingPayments.Location = new Point(27, 173);
            btnPendingPayments.Margin = new Padding(27, 8, 27, 5);
            btnPendingPayments.Name = "btnPendingPayments";
            btnPendingPayments.Size = new Size(304, 152);
            btnPendingPayments.TabIndex = 4;
            btnPendingPayments.Text = "Pending Payments";
            btnPendingPayments.TextAlign = ContentAlignment.TopLeft;
            btnPendingPayments.UseCompatibleTextRendering = true;
            btnPendingPayments.UseVisualStyleBackColor = false;
            btnPendingPayments.Click += btnPendingPayments_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.White;
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label8);
            panel1.Location = new Point(-1, 0);
            panel1.Margin = new Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(1468, 112);
            panel1.TabIndex = 3;
            panel1.Paint += panel1_Paint;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(95, 23);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(19, 20);
            label6.TabIndex = 109;
            label6.Text = ">";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.FromArgb(0, 166, 90);
            label7.Location = new Point(112, 22);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(140, 20);
            label7.TabIndex = 110;
            label7.Text = "Financial Reports";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(20, 20);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(68, 20);
            label8.TabIndex = 111;
            label8.Text = "Reports";
            // 
            // frmFinancialReportsUI
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 246, 246);
            ClientSize = new Size(1467, 632);
            Controls.Add(panel1);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(4, 5, 4, 5);
            Name = "frmFinancialReportsUI";
            Text = "Financial Reports";
            Load += frmFinancialReportsUI_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnTransaction;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnRefundReport;
        private System.Windows.Forms.Button btnDeletedInvoices;
        private System.Windows.Forms.Button btnPaymentMode;
        private System.Windows.Forms.Button btnStaff;
        private System.Windows.Forms.Button btnShift;
        private System.Windows.Forms.Button btnPendingPayments;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        //private DevComponents.DotNetBar.Controls.Line line1;
    }
}