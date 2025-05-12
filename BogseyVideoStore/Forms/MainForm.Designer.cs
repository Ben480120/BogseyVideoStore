namespace BogseyVideoStore.Forms
{
    partial class MainForm
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
            this.btnCustomerForm = new System.Windows.Forms.Button();
            this.btnVideoForm = new System.Windows.Forms.Button();
            this.btnRentalForm = new System.Windows.Forms.Button();
            this.pictureBoxShutdown = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShutdown)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCustomerForm
            // 
            this.btnCustomerForm.BackColor = System.Drawing.Color.Transparent;
            this.btnCustomerForm.BackgroundImage = global::BogseyVideoStore.Properties.Resources.Customerbtn;
            this.btnCustomerForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCustomerForm.Font = new System.Drawing.Font("Elephant", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerForm.Location = new System.Drawing.Point(422, 178);
            this.btnCustomerForm.Name = "btnCustomerForm";
            this.btnCustomerForm.Size = new System.Drawing.Size(277, 100);
            this.btnCustomerForm.TabIndex = 36;
            this.btnCustomerForm.Text = "Customers";
            this.btnCustomerForm.UseVisualStyleBackColor = false;
            this.btnCustomerForm.Click += new System.EventHandler(this.btnCustomerForm_Click);
            // 
            // btnVideoForm
            // 
            this.btnVideoForm.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnVideoForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnVideoForm.Font = new System.Drawing.Font("Elephant", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVideoForm.Location = new System.Drawing.Point(422, 284);
            this.btnVideoForm.Name = "btnVideoForm";
            this.btnVideoForm.Size = new System.Drawing.Size(277, 100);
            this.btnVideoForm.TabIndex = 37;
            this.btnVideoForm.Text = "Videos";
            this.btnVideoForm.UseVisualStyleBackColor = false;
            this.btnVideoForm.Click += new System.EventHandler(this.btnVideoForm_Click);
            // 
            // btnRentalForm
            // 
            this.btnRentalForm.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnRentalForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRentalForm.Font = new System.Drawing.Font("Elephant", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRentalForm.Location = new System.Drawing.Point(422, 390);
            this.btnRentalForm.Name = "btnRentalForm";
            this.btnRentalForm.Size = new System.Drawing.Size(277, 100);
            this.btnRentalForm.TabIndex = 38;
            this.btnRentalForm.Text = "Rentals";
            this.btnRentalForm.UseVisualStyleBackColor = false;
            this.btnRentalForm.Click += new System.EventHandler(this.btnRentalForm_Click);
            // 
            // pictureBoxShutdown
            // 
            this.pictureBoxShutdown.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxShutdown.BackgroundImage = global::BogseyVideoStore.Properties.Resources.SDicon;
            this.pictureBoxShutdown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxShutdown.Location = new System.Drawing.Point(939, 2);
            this.pictureBoxShutdown.Name = "pictureBoxShutdown";
            this.pictureBoxShutdown.Size = new System.Drawing.Size(111, 59);
            this.pictureBoxShutdown.TabIndex = 42;
            this.pictureBoxShutdown.TabStop = false;
            this.pictureBoxShutdown.Click += new System.EventHandler(this.pictureBoxShutdown_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::BogseyVideoStore.Properties.Resources.MFBG__1_;
            this.ClientSize = new System.Drawing.Size(1031, 641);
            this.Controls.Add(this.pictureBoxShutdown);
            this.Controls.Add(this.btnRentalForm);
            this.Controls.Add(this.btnVideoForm);
            this.Controls.Add(this.btnCustomerForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShutdown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCustomerForm;
        private System.Windows.Forms.Button btnVideoForm;
        private System.Windows.Forms.Button btnRentalForm;
        private System.Windows.Forms.PictureBox pictureBoxShutdown;
    }
}