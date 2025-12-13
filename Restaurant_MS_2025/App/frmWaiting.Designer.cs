namespace Restaurant_MS_UI.App
{
    partial class frmWaiting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWaiting));
            this.pbLoading = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.bgDataLoader = new System.ComponentModel.BackgroundWorker();
            this.pnlDisplay = new System.Windows.Forms.Panel();
            this.pbCompleted = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).BeginInit();
            this.pnlDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCompleted)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLoading
            // 
            this.pbLoading.Image = ((System.Drawing.Image)(resources.GetObject("pbLoading.Image")));
            this.pbLoading.Location = new System.Drawing.Point(184, 50);
            this.pbLoading.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(297, 123);
            this.pbLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbLoading.TabIndex = 0;
            this.pbLoading.TabStop = false;
            this.pbLoading.WaitOnLoad = true;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(156, 202);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(368, 81);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Please wait, we are loading default items for you.\r\n Just finishing in few moment" +
    "s.";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlDisplay
            // 
            this.pnlDisplay.BackColor = System.Drawing.Color.White;
            this.pnlDisplay.Controls.Add(this.pbCompleted);
            this.pnlDisplay.Controls.Add(this.pbLoading);
            this.pnlDisplay.Controls.Add(this.lblStatus);
            this.pnlDisplay.Location = new System.Drawing.Point(5, 5);
            this.pnlDisplay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlDisplay.Name = "pnlDisplay";
            this.pnlDisplay.Size = new System.Drawing.Size(675, 313);
            this.pnlDisplay.TabIndex = 2;
            this.pnlDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDisplay_Paint);
            // 
            // pbCompleted
            // 
            this.pbCompleted.Image = ((System.Drawing.Image)(resources.GetObject("pbCompleted.Image")));
            this.pbCompleted.Location = new System.Drawing.Point(156, 27);
            this.pbCompleted.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbCompleted.Name = "pbCompleted";
            this.pbCompleted.Size = new System.Drawing.Size(368, 171);
            this.pbCompleted.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbCompleted.TabIndex = 2;
            this.pbCompleted.TabStop = false;
            this.pbCompleted.Visible = false;
            // 
            // frmWaiting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(685, 322);
            this.Controls.Add(this.pnlDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmWaiting";
            this.Text = "frmWaiting";
            this.Load += new System.EventHandler(this.frmWaiting_Load);
            this.Shown += new System.EventHandler(this.frmWaiting_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).EndInit();
            this.pnlDisplay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCompleted)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLoading;
        private System.Windows.Forms.Label lblStatus;
        private System.ComponentModel.BackgroundWorker bgDataLoader;
        private System.Windows.Forms.Panel pnlDisplay;
        private System.Windows.Forms.PictureBox pbCompleted;
    }
}