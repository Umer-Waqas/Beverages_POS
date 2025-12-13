namespace Restaurant_MS_UI.Menu.Main
{
    partial class frmUsers
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
            this.tabUsers = new System.Windows.Forms.TabControl();
            this.tpAdmin = new System.Windows.Forms.TabPage();
            this.grdAdmins = new System.Windows.Forms.DataGridView();
            this.tpPharmacist = new System.Windows.Forms.TabPage();
            this.grdPharmacists = new System.Windows.Forms.DataGridView();
            this.colUserIdPh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserNamePh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmailPh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhonePh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEditPh = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDeletePh = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tpAccountant = new System.Windows.Forms.TabPage();
            this.grdAccountants = new System.Windows.Forms.DataGridView();
            this.colUserIdAc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNameAc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmailAc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhoneAc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEditAc = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDeleteAc = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tpDoctor = new System.Windows.Forms.TabPage();
            this.grdDoctors = new System.Windows.Forms.DataGridView();
            this.colUserIdDoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNameDoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmailDoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhoneDoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEditDoc = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDeleteDoc = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.cxtMenuUser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.colUserIdAd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNameAd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmailAd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhoneAd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRoleId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEditAd = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDeleteAd = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tabUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAdmins)).BeginInit();
            this.tpPharmacist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPharmacists)).BeginInit();
            this.tpAccountant.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAccountants)).BeginInit();
            this.tpDoctor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDoctors)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabUsers
            // 
            this.tabUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabUsers.Controls.Add(this.tpAdmin);
            this.tabUsers.Controls.Add(this.tpPharmacist);
            this.tabUsers.Controls.Add(this.tpAccountant);
            this.tabUsers.Controls.Add(this.tpDoctor);
            this.tabUsers.Location = new System.Drawing.Point(556, 357);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.SelectedIndex = 0;
            this.tabUsers.Size = new System.Drawing.Size(274, 110);
            this.tabUsers.TabIndex = 1;
            this.tabUsers.Visible = false;
            // 
            // tpAdmin
            // 
            this.tpAdmin.Location = new System.Drawing.Point(4, 27);
            this.tpAdmin.Name = "tpAdmin";
            this.tpAdmin.Size = new System.Drawing.Size(266, 79);
            this.tpAdmin.TabIndex = 0;
            this.tpAdmin.Text = "Admin";
            this.tpAdmin.UseVisualStyleBackColor = true;
            // 
            // grdAdmins
            // 
            this.grdAdmins.AllowUserToAddRows = false;
            this.grdAdmins.AllowUserToDeleteRows = false;
            this.grdAdmins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdAdmins.BackgroundColor = System.Drawing.Color.White;
            this.grdAdmins.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdAdmins.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdAdmins.ColumnHeadersHeight = 35;
            this.grdAdmins.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUserIdAd,
            this.colNameAd,
            this.colEmailAd,
            this.colPhoneAd,
            this.colRoleId,
            this.colRole,
            this.colEditAd,
            this.colDeleteAd});
            this.grdAdmins.EnableHeadersVisualStyles = false;
            this.grdAdmins.Location = new System.Drawing.Point(3, 3);
            this.grdAdmins.Name = "grdAdmins";
            this.grdAdmins.RowHeadersVisible = false;
            this.grdAdmins.RowTemplate.Height = 35;
            this.grdAdmins.Size = new System.Drawing.Size(915, 484);
            this.grdAdmins.TabIndex = 0;
            this.grdAdmins.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdAdmins_CellContentClick);
            // 
            // tpPharmacist
            // 
            this.tpPharmacist.Controls.Add(this.grdPharmacists);
            this.tpPharmacist.Location = new System.Drawing.Point(4, 27);
            this.tpPharmacist.Name = "tpPharmacist";
            this.tpPharmacist.Size = new System.Drawing.Size(913, 430);
            this.tpPharmacist.TabIndex = 1;
            this.tpPharmacist.Text = "Pharmacist";
            this.tpPharmacist.UseVisualStyleBackColor = true;
            // 
            // grdPharmacists
            // 
            this.grdPharmacists.AllowUserToAddRows = false;
            this.grdPharmacists.AllowUserToDeleteRows = false;
            this.grdPharmacists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdPharmacists.BackgroundColor = System.Drawing.Color.White;
            this.grdPharmacists.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdPharmacists.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdPharmacists.ColumnHeadersHeight = 35;
            this.grdPharmacists.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUserIdPh,
            this.colUserNamePh,
            this.colEmailPh,
            this.colPhonePh,
            this.colEditPh,
            this.colDeletePh});
            this.grdPharmacists.EnableHeadersVisualStyles = false;
            this.grdPharmacists.Location = new System.Drawing.Point(8, 25);
            this.grdPharmacists.Name = "grdPharmacists";
            this.grdPharmacists.RowHeadersVisible = false;
            this.grdPharmacists.RowTemplate.Height = 35;
            this.grdPharmacists.Size = new System.Drawing.Size(897, 471);
            this.grdPharmacists.TabIndex = 1;
            this.grdPharmacists.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdPharmacists_CellContentClick);
            // 
            // colUserIdPh
            // 
            this.colUserIdPh.HeaderText = "UserId";
            this.colUserIdPh.Name = "colUserIdPh";
            this.colUserIdPh.ReadOnly = true;
            this.colUserIdPh.Visible = false;
            // 
            // colUserNamePh
            // 
            this.colUserNamePh.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colUserNamePh.HeaderText = "Name";
            this.colUserNamePh.Name = "colUserNamePh";
            this.colUserNamePh.ReadOnly = true;
            // 
            // colEmailPh
            // 
            this.colEmailPh.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colEmailPh.HeaderText = "Email";
            this.colEmailPh.Name = "colEmailPh";
            this.colEmailPh.ReadOnly = true;
            // 
            // colPhonePh
            // 
            this.colPhonePh.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPhonePh.HeaderText = "Phone";
            this.colPhonePh.Name = "colPhonePh";
            this.colPhonePh.ReadOnly = true;
            // 
            // colEditPh
            // 
            this.colEditPh.HeaderText = "Edit";
            this.colEditPh.Name = "colEditPh";
            this.colEditPh.ReadOnly = true;
            this.colEditPh.Width = 50;
            // 
            // colDeletePh
            // 
            this.colDeletePh.HeaderText = "Delete";
            this.colDeletePh.Name = "colDeletePh";
            this.colDeletePh.ReadOnly = true;
            this.colDeletePh.Width = 50;
            // 
            // tpAccountant
            // 
            this.tpAccountant.Controls.Add(this.grdAccountants);
            this.tpAccountant.Location = new System.Drawing.Point(4, 27);
            this.tpAccountant.Name = "tpAccountant";
            this.tpAccountant.Size = new System.Drawing.Size(913, 430);
            this.tpAccountant.TabIndex = 2;
            this.tpAccountant.Text = "Accountant";
            this.tpAccountant.UseVisualStyleBackColor = true;
            // 
            // grdAccountants
            // 
            this.grdAccountants.AllowUserToAddRows = false;
            this.grdAccountants.AllowUserToDeleteRows = false;
            this.grdAccountants.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdAccountants.BackgroundColor = System.Drawing.Color.White;
            this.grdAccountants.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdAccountants.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdAccountants.ColumnHeadersHeight = 35;
            this.grdAccountants.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUserIdAc,
            this.colNameAc,
            this.colEmailAc,
            this.colPhoneAc,
            this.colEditAc,
            this.colDeleteAc});
            this.grdAccountants.EnableHeadersVisualStyles = false;
            this.grdAccountants.Location = new System.Drawing.Point(8, 25);
            this.grdAccountants.Name = "grdAccountants";
            this.grdAccountants.RowHeadersVisible = false;
            this.grdAccountants.RowTemplate.Height = 35;
            this.grdAccountants.Size = new System.Drawing.Size(897, 471);
            this.grdAccountants.TabIndex = 1;
            this.grdAccountants.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdAccountants_CellContentClick);
            // 
            // colUserIdAc
            // 
            this.colUserIdAc.HeaderText = "UserId";
            this.colUserIdAc.Name = "colUserIdAc";
            this.colUserIdAc.ReadOnly = true;
            this.colUserIdAc.Visible = false;
            // 
            // colNameAc
            // 
            this.colNameAc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNameAc.HeaderText = "Name";
            this.colNameAc.Name = "colNameAc";
            this.colNameAc.ReadOnly = true;
            // 
            // colEmailAc
            // 
            this.colEmailAc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colEmailAc.HeaderText = "Email";
            this.colEmailAc.Name = "colEmailAc";
            this.colEmailAc.ReadOnly = true;
            // 
            // colPhoneAc
            // 
            this.colPhoneAc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPhoneAc.HeaderText = "Phone";
            this.colPhoneAc.Name = "colPhoneAc";
            this.colPhoneAc.ReadOnly = true;
            // 
            // colEditAc
            // 
            this.colEditAc.HeaderText = "Edit";
            this.colEditAc.Name = "colEditAc";
            this.colEditAc.ReadOnly = true;
            this.colEditAc.Width = 50;
            // 
            // colDeleteAc
            // 
            this.colDeleteAc.HeaderText = "Delete";
            this.colDeleteAc.Name = "colDeleteAc";
            this.colDeleteAc.ReadOnly = true;
            this.colDeleteAc.Width = 50;
            // 
            // tpDoctor
            // 
            this.tpDoctor.Controls.Add(this.grdDoctors);
            this.tpDoctor.Location = new System.Drawing.Point(4, 27);
            this.tpDoctor.Name = "tpDoctor";
            this.tpDoctor.Padding = new System.Windows.Forms.Padding(3);
            this.tpDoctor.Size = new System.Drawing.Size(913, 430);
            this.tpDoctor.TabIndex = 3;
            this.tpDoctor.Text = "Doctor";
            this.tpDoctor.UseVisualStyleBackColor = true;
            // 
            // grdDoctors
            // 
            this.grdDoctors.AllowUserToAddRows = false;
            this.grdDoctors.AllowUserToDeleteRows = false;
            this.grdDoctors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdDoctors.BackgroundColor = System.Drawing.Color.White;
            this.grdDoctors.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grdDoctors.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdDoctors.ColumnHeadersHeight = 35;
            this.grdDoctors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUserIdDoc,
            this.colNameDoc,
            this.colEmailDoc,
            this.colPhoneDoc,
            this.colEditDoc,
            this.colDeleteDoc});
            this.grdDoctors.EnableHeadersVisualStyles = false;
            this.grdDoctors.Location = new System.Drawing.Point(8, 25);
            this.grdDoctors.Name = "grdDoctors";
            this.grdDoctors.RowHeadersVisible = false;
            this.grdDoctors.RowTemplate.Height = 35;
            this.grdDoctors.Size = new System.Drawing.Size(897, 449);
            this.grdDoctors.TabIndex = 2;
            this.grdDoctors.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdDoctors_CellContentClick);
            // 
            // colUserIdDoc
            // 
            this.colUserIdDoc.HeaderText = "UserId";
            this.colUserIdDoc.Name = "colUserIdDoc";
            this.colUserIdDoc.ReadOnly = true;
            this.colUserIdDoc.Visible = false;
            // 
            // colNameDoc
            // 
            this.colNameDoc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNameDoc.HeaderText = "Name";
            this.colNameDoc.Name = "colNameDoc";
            this.colNameDoc.ReadOnly = true;
            // 
            // colEmailDoc
            // 
            this.colEmailDoc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colEmailDoc.HeaderText = "Email";
            this.colEmailDoc.Name = "colEmailDoc";
            this.colEmailDoc.ReadOnly = true;
            // 
            // colPhoneDoc
            // 
            this.colPhoneDoc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPhoneDoc.HeaderText = "Phone";
            this.colPhoneDoc.Name = "colPhoneDoc";
            this.colPhoneDoc.ReadOnly = true;
            // 
            // colEditDoc
            // 
            this.colEditDoc.HeaderText = "Edit";
            this.colEditDoc.Name = "colEditDoc";
            this.colEditDoc.ReadOnly = true;
            this.colEditDoc.Width = 50;
            // 
            // colDeleteDoc
            // 
            this.colDeleteDoc.HeaderText = "Delete";
            this.colDeleteDoc.Name = "colDeleteDoc";
            this.colDeleteDoc.ReadOnly = true;
            this.colDeleteDoc.Width = 50;
            // 
            // btnAddUser
            // 
            this.btnAddUser.AllowDrop = true;
            this.btnAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddUser.AutoEllipsis = true;
            this.btnAddUser.ContextMenuStrip = this.cxtMenuUser;
            this.btnAddUser.Location = new System.Drawing.Point(788, 5);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(121, 33);
            this.btnAddUser.TabIndex = 2;
            this.btnAddUser.Text = "AddUser";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // cxtMenuUser
            // 
            this.cxtMenuUser.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cxtMenuUser.Name = "cxtMenuUser";
            this.cxtMenuUser.Size = new System.Drawing.Size(61, 4);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "UserId";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "Email";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "Phone";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "UserId";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.HeaderText = "Name";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn7.HeaderText = "Email";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.HeaderText = "Phone";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "UserId";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Visible = false;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn10.HeaderText = "Name";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn11.HeaderText = "Email";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn12.HeaderText = "Phone";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "UserId";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Visible = false;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn14.HeaderText = "Name";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn15.HeaderText = "Email";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn16.HeaderText = "Phone";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.ReadOnly = true;
            // 
            // line1
            // 
            //this.line1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            //this.line1.Location = new System.Drawing.Point(4, 42);
            //this.line1.Name = "line1";
            //this.line1.Size = new System.Drawing.Size(909, 11);
            //this.line1.TabIndex = 102;
            //this.line1.Text = "line1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(63, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 20);
            this.label5.TabIndex = 103;
            this.label5.Text = ">";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(166)))), ((int)(((byte)(90)))));
            this.label4.Location = new System.Drawing.Point(76, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 20);
            this.label4.TabIndex = 104;
            this.label4.Text = "All UserRoles";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 20);
            this.label3.TabIndex = 105;
            this.label3.Text = "UserRoles";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.panel1.Controls.Add(this.grdAdmins);
            this.panel1.Controls.Add(this.tabUsers);
            this.panel1.Location = new System.Drawing.Point(0, 58);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(921, 490);
            this.panel1.TabIndex = 106;
            // 
            // colUserIdAd
            // 
            this.colUserIdAd.HeaderText = "UserId";
            this.colUserIdAd.Name = "colUserIdAd";
            this.colUserIdAd.ReadOnly = true;
            this.colUserIdAd.Visible = false;
            // 
            // colNameAd
            // 
            this.colNameAd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNameAd.HeaderText = "Name";
            this.colNameAd.Name = "colNameAd";
            this.colNameAd.ReadOnly = true;
            // 
            // colEmailAd
            // 
            this.colEmailAd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colEmailAd.HeaderText = "Email";
            this.colEmailAd.Name = "colEmailAd";
            this.colEmailAd.ReadOnly = true;
            // 
            // colPhoneAd
            // 
            this.colPhoneAd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPhoneAd.HeaderText = "Phone";
            this.colPhoneAd.Name = "colPhoneAd";
            this.colPhoneAd.ReadOnly = true;
            // 
            // colRoleId
            // 
            this.colRoleId.HeaderText = "RoleId";
            this.colRoleId.Name = "colRoleId";
            this.colRoleId.ReadOnly = true;
            // 
            // colRole
            // 
            this.colRole.HeaderText = "Role";
            this.colRole.Name = "colRole";
            this.colRole.ReadOnly = true;
            // 
            // colEditAd
            // 
            this.colEditAd.HeaderText = "Edit";
            this.colEditAd.Name = "colEditAd";
            this.colEditAd.ReadOnly = true;
            this.colEditAd.Width = 50;
            // 
            // colDeleteAd
            // 
            this.colDeleteAd.HeaderText = "Delete";
            this.colDeleteAd.Name = "colDeleteAd";
            this.colDeleteAd.ReadOnly = true;
            this.colDeleteAd.Width = 50;
            // 
            // frmUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(921, 547);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            //this.Controls.Add(this.line1);
            this.Controls.Add(this.btnAddUser);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmUsers";
            this.Text = "UserRoles";
            this.Load += new System.EventHandler(this.frmUsers_Load);
            this.tabUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAdmins)).EndInit();
            this.tpPharmacist.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPharmacists)).EndInit();
            this.tpAccountant.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAccountants)).EndInit();
            this.tpDoctor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDoctors)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabUsers;
        private System.Windows.Forms.TabPage tpAdmin;
        private System.Windows.Forms.TabPage tpPharmacist;
        private System.Windows.Forms.TabPage tpAccountant;
        private System.Windows.Forms.DataGridView grdAdmins;
        private System.Windows.Forms.DataGridView grdPharmacists;
        private System.Windows.Forms.DataGridView grdAccountants;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.TabPage tpDoctor;
        private System.Windows.Forms.DataGridView grdDoctors;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserIdPh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserNamePh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmailPh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhonePh;
        private System.Windows.Forms.DataGridViewButtonColumn colEditPh;
        private System.Windows.Forms.DataGridViewButtonColumn colDeletePh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserIdAc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNameAc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmailAc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhoneAc;
        private System.Windows.Forms.DataGridViewButtonColumn colEditAc;
        private System.Windows.Forms.DataGridViewButtonColumn colDeleteAc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserIdDoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNameDoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmailDoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhoneDoc;
        private System.Windows.Forms.DataGridViewButtonColumn colEditDoc;
        private System.Windows.Forms.DataGridViewButtonColumn colDeleteDoc;
        private System.Windows.Forms.ContextMenuStrip cxtMenuUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        //private DevComponents.DotNetBar.Controls.Line line1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserIdAd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNameAd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmailAd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhoneAd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRoleId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRole;
        private System.Windows.Forms.DataGridViewButtonColumn colEditAd;
        private System.Windows.Forms.DataGridViewButtonColumn colDeleteAd;

    }
}