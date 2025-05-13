namespace BogseyVideoStore
{
    partial class VideoForm
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
            this.pictureBoxShutdown = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearchTitle = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbRentalDaysAllowed = new System.Windows.Forms.ComboBox();
            this.dgvVideos = new System.Windows.Forms.DataGridView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtQuantityOut = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtQuantityIn = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.txtVideoTitle = new System.Windows.Forms.TextBox();
            this.pictureBoxBack = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShutdown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVideos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBack)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxShutdown
            // 
            this.pictureBoxShutdown.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxShutdown.BackgroundImage = global::BogseyVideoStore.Properties.Resources.SDicon;
            this.pictureBoxShutdown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxShutdown.Location = new System.Drawing.Point(941, -2);
            this.pictureBoxShutdown.Name = "pictureBoxShutdown";
            this.pictureBoxShutdown.Size = new System.Drawing.Size(111, 59);
            this.pictureBoxShutdown.TabIndex = 41;
            this.pictureBoxShutdown.TabStop = false;
            this.pictureBoxShutdown.Click += new System.EventHandler(this.pictureBoxShutdown_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Elephant", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(168, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 25);
            this.label1.TabIndex = 23;
            this.label1.Text = "Video Title:";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSearch.Font = new System.Drawing.Font("Elephant", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(769, 223);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(122, 42);
            this.btnSearch.TabIndex = 40;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearchTitle
            // 
            this.txtSearchTitle.BackColor = System.Drawing.SystemColors.Window;
            this.txtSearchTitle.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchTitle.Location = new System.Drawing.Point(708, 188);
            this.txtSearchTitle.Name = "txtSearchTitle";
            this.txtSearchTitle.Size = new System.Drawing.Size(255, 29);
            this.txtSearchTitle.TabIndex = 39;
            this.txtSearchTitle.TextChanged += new System.EventHandler(this.txtSearchTitle_TextChanged);
            this.txtSearchTitle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchTitle_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Elephant", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label7.Location = new System.Drawing.Point(618, 188);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 25);
            this.label7.TabIndex = 38;
            this.label7.Text = "Search:";
            // 
            // cmbRentalDaysAllowed
            // 
            this.cmbRentalDaysAllowed.FormattingEnabled = true;
            this.cmbRentalDaysAllowed.Location = new System.Drawing.Point(398, 312);
            this.cmbRentalDaysAllowed.Name = "cmbRentalDaysAllowed";
            this.cmbRentalDaysAllowed.Size = new System.Drawing.Size(44, 21);
            this.cmbRentalDaysAllowed.TabIndex = 37;
            // 
            // dgvVideos
            // 
            this.dgvVideos.BackgroundColor = System.Drawing.Color.SeaShell;
            this.dgvVideos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVideos.Location = new System.Drawing.Point(170, 412);
            this.dgvVideos.Name = "dgvVideos";
            this.dgvVideos.Size = new System.Drawing.Size(789, 217);
            this.dgvVideos.TabIndex = 36;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDelete.Font = new System.Drawing.Font("Elephant", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(580, 345);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(122, 42);
            this.btnDelete.TabIndex = 35;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClear.Font = new System.Drawing.Font("Elephant", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(708, 345);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(122, 42);
            this.btnClear.TabIndex = 34;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnEdit.Font = new System.Drawing.Font("Elephant", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Location = new System.Drawing.Point(452, 345);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(122, 42);
            this.btnEdit.TabIndex = 33;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAdd.Font = new System.Drawing.Font("Elephant", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(324, 345);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(122, 42);
            this.btnAdd.TabIndex = 32;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Elephant", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label5.Location = new System.Drawing.Point(168, 311);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(214, 25);
            this.label5.TabIndex = 31;
            this.label5.Text = "Rental Days Allowed:";
            // 
            // txtQuantityOut
            // 
            this.txtQuantityOut.BackColor = System.Drawing.SystemColors.Window;
            this.txtQuantityOut.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuantityOut.Location = new System.Drawing.Point(359, 268);
            this.txtQuantityOut.Name = "txtQuantityOut";
            this.txtQuantityOut.Size = new System.Drawing.Size(50, 29);
            this.txtQuantityOut.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Elephant", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(168, 272);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 25);
            this.label4.TabIndex = 29;
            this.label4.Text = "Quantity Out:";
            // 
            // txtQuantityIn
            // 
            this.txtQuantityIn.BackColor = System.Drawing.SystemColors.Window;
            this.txtQuantityIn.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuantityIn.Location = new System.Drawing.Point(359, 227);
            this.txtQuantityIn.Name = "txtQuantityIn";
            this.txtQuantityIn.Size = new System.Drawing.Size(50, 29);
            this.txtQuantityIn.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Elephant", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(168, 231);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 25);
            this.label3.TabIndex = 27;
            this.label3.Text = "Quantity in Stock:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Elephant", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(168, 191);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 25);
            this.label2.TabIndex = 26;
            this.label2.Text = "Video Category:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            "VCD",
            "DVD"});
            this.cmbCategory.Location = new System.Drawing.Point(359, 187);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(123, 30);
            this.cmbCategory.TabIndex = 25;
            // 
            // txtVideoTitle
            // 
            this.txtVideoTitle.BackColor = System.Drawing.SystemColors.Window;
            this.txtVideoTitle.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVideoTitle.Location = new System.Drawing.Point(359, 145);
            this.txtVideoTitle.Name = "txtVideoTitle";
            this.txtVideoTitle.Size = new System.Drawing.Size(255, 29);
            this.txtVideoTitle.TabIndex = 24;
            // 
            // pictureBoxBack
            // 
            this.pictureBoxBack.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxBack.BackgroundImage = global::BogseyVideoStore.Properties.Resources.BackIcon;
            this.pictureBoxBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxBack.Location = new System.Drawing.Point(-23, -2);
            this.pictureBoxBack.Name = "pictureBoxBack";
            this.pictureBoxBack.Size = new System.Drawing.Size(111, 59);
            this.pictureBoxBack.TabIndex = 42;
            this.pictureBoxBack.TabStop = false;
            this.pictureBoxBack.Click += new System.EventHandler(this.pictureBoxBack_Click);
            // 
            // VideoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::BogseyVideoStore.Properties.Resources.bg1;
            this.ClientSize = new System.Drawing.Size(1031, 641);
            this.Controls.Add(this.pictureBoxBack);
            this.Controls.Add(this.pictureBoxShutdown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearchTitle);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbRentalDaysAllowed);
            this.Controls.Add(this.dgvVideos);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtQuantityOut);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtQuantityIn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.txtVideoTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VideoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VideoForm";
            this.Load += new System.EventHandler(this.VideoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShutdown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVideos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBack)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxShutdown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearchTitle;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbRentalDaysAllowed;
        private System.Windows.Forms.DataGridView dgvVideos;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtQuantityOut;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtQuantityIn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.TextBox txtVideoTitle;
        private System.Windows.Forms.PictureBox pictureBoxBack;
    }
}