namespace Pharmacy.UI.Reports.Accounts
{
    partial class frmAccountsReportsUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccountsReportsUI));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnAccoutns = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBillPayments = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            //this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "default-patient.png");
            this.imageList1.Images.SetKeyName(1, "Refresh_16.png");
            // 
            // btnAccoutns
            // 
            this.btnAccoutns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccoutns.AutoSize = true;
            this.btnAccoutns.BackColor = System.Drawing.Color.White;
            this.btnAccoutns.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnAccoutns.FlatAppearance.BorderSize = 0;
            this.btnAccoutns.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccoutns.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnAccoutns.Image = ((System.Drawing.Image)(resources.GetObject("btnAccoutns.Image")));
            this.btnAccoutns.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAccoutns.Location = new System.Drawing.Point(20, 5);
            this.btnAccoutns.Margin = new System.Windows.Forms.Padding(20, 5, 20, 3);
            this.btnAccoutns.Name = "btnAccoutns";
            this.btnAccoutns.Size = new System.Drawing.Size(268, 107);
            this.btnAccoutns.TabIndex = 0;
            this.btnAccoutns.Text = "Accounts";
            this.btnAccoutns.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnAccoutns.UseCompatibleTextRendering = true;
            this.btnAccoutns.UseVisualStyleBackColor = false;
            this.btnAccoutns.Click += new System.EventHandler(this.btnTransaction_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.btnBillPayments, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAccoutns, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 97);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(616, 115);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnBillPayments
            // 
            this.btnBillPayments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBillPayments.AutoSize = true;
            this.btnBillPayments.BackColor = System.Drawing.Color.White;
            this.btnBillPayments.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnBillPayments.FlatAppearance.BorderSize = 0;
            this.btnBillPayments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBillPayments.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnBillPayments.Image = ((System.Drawing.Image)(resources.GetObject("btnBillPayments.Image")));
            this.btnBillPayments.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBillPayments.Location = new System.Drawing.Point(328, 5);
            this.btnBillPayments.Margin = new System.Windows.Forms.Padding(20, 5, 20, 3);
            this.btnBillPayments.Name = "btnBillPayments";
            this.btnBillPayments.Size = new System.Drawing.Size(268, 107);
            this.btnBillPayments.TabIndex = 1;
            this.btnBillPayments.Text = "Bill Payments";
            this.btnBillPayments.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnBillPayments.UseCompatibleTextRendering = true;
            this.btnBillPayments.UseVisualStyleBackColor = false;
            this.btnBillPayments.Click += new System.EventHandler(this.btnShift_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label8);
            //this.panel1.Controls.Add(this.line1);
            this.panel1.Location = new System.Drawing.Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1101, 73);
            this.panel1.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(71, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 17);
            this.label6.TabIndex = 109;
            this.label6.Text = ">";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(166)))), ((int)(((byte)(90)))));
            this.label7.Location = new System.Drawing.Point(84, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 17);
            this.label7.TabIndex = 110;
            this.label7.Text = "Account Reports";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(15, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 17);
            this.label8.TabIndex = 111;
            this.label8.Text = "Reports";
            // 
            // line1
            // 
            //this.line1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            //this.line1.Location = new System.Drawing.Point(1, 46);
            //this.line1.Name = "line1";
            //this.line1.Size = new System.Drawing.Size(1103, 10);
            //this.line1.TabIndex = 106;
            //this.line1.Text = "line1";
            // 
            // frmAccountsReportsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.ClientSize = new System.Drawing.Size(1100, 411);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmAccountsReportsUI";
            this.Text = "Account Reports";
            this.Load += new System.EventHandler(this.frmFinancialReportsUI_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnAccoutns;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnBillPayments;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        //private DevComponents.DotNetBar.Controls.Line line1;
    }
}