namespace Restaurant_MS_UI.Menu.Main
{
    partial class frmShowSerialKey
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
            txtSerialNo = new TextBox();
            btnGenerateKey = new Button();
            txtKey = new TextBox();
            btnGet = new Button();
            bntCheck = new Button();
            SuspendLayout();
            // 
            // txtSerialNo
            // 
            txtSerialNo.Location = new Point(72, 68);
            txtSerialNo.Margin = new Padding(4, 5, 4, 5);
            txtSerialNo.Name = "txtSerialNo";
            txtSerialNo.Size = new Size(267, 27);
            txtSerialNo.TabIndex = 0;
            // 
            // btnGenerateKey
            // 
            btnGenerateKey.Location = new Point(240, 254);
            btnGenerateKey.Margin = new Padding(4, 5, 4, 5);
            btnGenerateKey.Name = "btnGenerateKey";
            btnGenerateKey.Size = new Size(100, 35);
            btnGenerateKey.TabIndex = 2;
            btnGenerateKey.Text = "Generate Key";
            btnGenerateKey.UseVisualStyleBackColor = true;
            btnGenerateKey.Click += btnGenerateKey_Click;
            // 
            // txtKey
            // 
            txtKey.Location = new Point(72, 214);
            txtKey.Margin = new Padding(4, 5, 4, 5);
            txtKey.Name = "txtKey";
            txtKey.Size = new Size(267, 27);
            txtKey.TabIndex = 3;
            // 
            // btnGet
            // 
            btnGet.Location = new Point(240, 122);
            btnGet.Margin = new Padding(4, 5, 4, 5);
            btnGet.Name = "btnGet";
            btnGet.Size = new Size(100, 35);
            btnGet.TabIndex = 1;
            btnGet.Text = "Get";
            btnGet.UseVisualStyleBackColor = true;
            btnGet.Click += btnGet_Click;
            // 
            // bntCheck
            // 
            bntCheck.Location = new Point(240, 328);
            bntCheck.Margin = new Padding(4, 5, 4, 5);
            bntCheck.Name = "bntCheck";
            bntCheck.Size = new Size(100, 35);
            bntCheck.TabIndex = 4;
            bntCheck.Text = "Check Key";
            bntCheck.UseVisualStyleBackColor = true;
            bntCheck.Click += bntCheck_Click;
            // 
            // frmShowSerialKey
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(459, 382);
            Controls.Add(bntCheck);
            Controls.Add(txtKey);
            Controls.Add(btnGenerateKey);
            Controls.Add(btnGet);
            Controls.Add(txtSerialNo);
            Margin = new Padding(4, 5, 4, 5);
            Name = "frmShowSerialKey";
            Text = "frmShowSerialKey";
            Load += frmShowSerialKey_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSerialNo;
        private System.Windows.Forms.Button btnGenerateKey;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.Button bntCheck;
    }
}