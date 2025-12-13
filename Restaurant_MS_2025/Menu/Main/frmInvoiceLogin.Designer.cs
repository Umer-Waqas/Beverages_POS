namespace Restaurant_MS_UI.Menu.Main
{
    partial class frmInvoiceLogin
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
            txtUserName = new TextBox();
            btnLogin = new Button();
            label1 = new Label();
            label2 = new Label();
            txtPassword = new TextBox();
            ErrMessage = new Label();
            ErrUserName = new Label();
            errPswd = new Label();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // txtUserName
            // 
            txtUserName.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUserName.Location = new Point(38, 50);
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(310, 30);
            txtUserName.TabIndex = 0;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(12, 173, 223);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(138, 135);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(102, 29);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(35, 29);
            label1.Name = "label1";
            label1.Size = new Size(51, 20);
            label1.TabIndex = 2;
            label1.Text = "Email";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(35, 79);
            label2.Name = "label2";
            label2.Size = new Size(83, 20);
            label2.TabIndex = 4;
            label2.Text = "Password";
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassword.Location = new Point(38, 100);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(310, 30);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // ErrMessage
            // 
            ErrMessage.BackColor = Color.Transparent;
            ErrMessage.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ErrMessage.ForeColor = Color.Coral;
            ErrMessage.Location = new Point(5, 0);
            ErrMessage.Name = "ErrMessage";
            ErrMessage.Size = new Size(393, 29);
            ErrMessage.TabIndex = 35;
            ErrMessage.Text = "Alert : Incorrect User Name OR Password. Please Try Again";
            ErrMessage.TextAlign = ContentAlignment.MiddleCenter;
            ErrMessage.Visible = false;
            // 
            // ErrUserName
            // 
            ErrUserName.AutoSize = true;
            ErrUserName.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ErrUserName.ForeColor = Color.Red;
            ErrUserName.Location = new Point(354, 51);
            ErrUserName.Name = "ErrUserName";
            ErrUserName.Size = new Size(24, 29);
            ErrUserName.TabIndex = 36;
            ErrUserName.Text = "*";
            ErrUserName.Visible = false;
            // 
            // errPswd
            // 
            errPswd.AutoSize = true;
            errPswd.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            errPswd.ForeColor = Color.Red;
            errPswd.Location = new Point(353, 98);
            errPswd.Name = "errPswd";
            errPswd.Size = new Size(24, 29);
            errPswd.TabIndex = 37;
            errPswd.Text = "*";
            errPswd.Visible = false;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(12, 200, 221);
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(246, 135);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(102, 29);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmInvoiceLogin
            // 
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(8F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PaleTurquoise;
            CancelButton = btnCancel;
            ClientSize = new Size(403, 183);
            Controls.Add(errPswd);
            Controls.Add(ErrUserName);
            Controls.Add(ErrMessage);
            Controls.Add(label2);
            Controls.Add(txtPassword);
            Controls.Add(label1);
            Controls.Add(btnCancel);
            Controls.Add(btnLogin);
            Controls.Add(txtUserName);
            Font = new Font("Microsoft Tai Le", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            MaximumSize = new Size(832, 358);
            Name = "frmInvoiceLogin";
            Text = "User Login";
            Load += frmInvoiceLogin_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.Label ErrUserName;
        private System.Windows.Forms.Label errPswd;
        private System.Windows.Forms.Button btnCancel;
    }
}