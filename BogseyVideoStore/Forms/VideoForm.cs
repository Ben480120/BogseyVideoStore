using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BogseyVideoStore.Forms;
using MySql.Data.MySqlClient;
using BogseyVideoStore.Helpers;


namespace BogseyVideoStore
{
    public partial class VideoForm : Form
    {
        private VideoService videoService = new VideoService();

        string connectionString = "server=localhost;database=bvs_db;uid=root;pwd=;";
        public VideoForm()
        {
            InitializeComponent();

            dgvVideos.CellClick += dgvVideos_CellClick;
            cmbRentalDaysAllowed.Items.AddRange(new object[] { "1", "2", "3" });
            cmbRentalDaysAllowed.SelectedIndex = 0;

            FormDesignHelper.StyleDataGridView(dgvVideos);
            FormDesignHelper.StyleButton(btnAdd);
            FormDesignHelper.StyleButton(btnEdit);
            FormDesignHelper.StyleButton(btnDelete);
            FormDesignHelper.StyleButton(btnClear);
            FormDesignHelper.StyleButton(btnSearch);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string title = txtVideoTitle.Text.Trim();
            string category = cmbCategory.Text.Trim();
            int rentalDaysAllowed;
            int quantityIn, quantityOut;

            if (!int.TryParse(txtQuantityIn.Text.Trim(), out quantityIn))
            {
                MessageBox.Show("Please enter a valid number for Quantity In.");
                return;
            }
            if (!int.TryParse(txtQuantityOut.Text.Trim(), out quantityOut))
            {
                MessageBox.Show("Please enter a valid number for Quantity Out.");
                return;
            }
            if (!int.TryParse(cmbRentalDaysAllowed.Text.Trim(), out rentalDaysAllowed))
            {
                MessageBox.Show("Please enter a valid number for rental days allowed.");
                return;
            }

            if (videoService.AddVideo(title, category, quantityIn, quantityOut, rentalDaysAllowed))
            {
                MessageBox.Show("Video added successfully.");
                LoadVideos();
                ClearForm();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvVideos.SelectedRows.Count > 0)
            {
                int videoId = Convert.ToInt32(dgvVideos.SelectedRows[0].Cells["video_id"].Value);
                string title = txtVideoTitle.Text.Trim();
                string category = cmbCategory.Text.Trim();
                int rentalDaysAllowed, quantityIn, quantityOut;

                if (!int.TryParse(txtQuantityIn.Text.Trim(), out quantityIn))
                {
                    MessageBox.Show("Please enter a valid number for Quantity In.");
                    return;
                }
                if (!int.TryParse(txtQuantityOut.Text.Trim(), out quantityOut))
                {
                    MessageBox.Show("Please enter a valid number for Quantity Out.");
                    return;
                }
                if (!int.TryParse(cmbRentalDaysAllowed.Text.Trim(), out rentalDaysAllowed))
                {
                    MessageBox.Show("Please enter a valid number for rental days allowed.");
                    return;
                }

                if (videoService.UpdateVideo(videoId, title, category, quantityIn, quantityOut, rentalDaysAllowed))
                {
                    MessageBox.Show("Video updated successfully.");
                    LoadVideos();
                    ClearForm();
                }
            }
            else
            {
                MessageBox.Show("Please select a video to update.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvVideos.SelectedRows.Count > 0)
            {
                int videoId = Convert.ToInt32(dgvVideos.SelectedRows[0].Cells["video_id"].Value);
                DialogResult result = MessageBox.Show("Are you sure you want to delete this video?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    videoService.DeleteVideo(videoId);
                    LoadVideos();
                    ClearForm();
                }
            }
            else
            {
                MessageBox.Show("Please select a video to delete.");
            }
        }


        private void LoadVideos()
        {
            dgvVideos.DataSource = videoService.GetAllVideos();

            if (dgvVideos.Columns.Contains("video_id"))
            {
                dgvVideos.Columns["video_id"].Visible = false;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void txtRentalDaysAllowed_TextChanged(object sender, EventArgs e)
        {

        }

        private void VideoForm_Load(object sender, EventArgs e)
        {
            LoadVideos();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string searchQuery = "SELECT video_id, title, category, quantity_in, quantity_out, rental_days_allowed FROM videos WHERE title LIKE @search";
                    MySqlCommand cmd = new MySqlCommand(searchQuery, connection);
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearchTitle.Text + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvVideos.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search error: " + ex.Message);
                }
            }
        }


        private void txtSearchTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void dgvVideos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvVideos.Rows[e.RowIndex];

                txtVideoTitle.Text = row.Cells["title"].Value.ToString();
                cmbCategory.SelectedItem = row.Cells["category"].Value.ToString();
                txtQuantityIn.Text = row.Cells["quantity_in"].Value.ToString();
                txtQuantityOut.Text = row.Cells["quantity_out"].Value.ToString();
                cmbRentalDaysAllowed.SelectedItem = row.Cells["rental_days_allowed"].Value.ToString();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxShutdown_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBoxShutdown_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtSearchTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBoxBack_Click(object sender, EventArgs e)
        {
            var mainForm = new MainForm();
            mainForm.FormClosed += (s, args) => this.Show();
            mainForm.Show();
            this.Hide();
        }

        private void ClearForm()
        {
            txtVideoTitle.Clear();
            cmbCategory.SelectedIndex = -1;
            txtQuantityIn.Clear();
            txtQuantityOut.Clear();
            cmbRentalDaysAllowed.SelectedIndex = 0;
        }

    }
}
