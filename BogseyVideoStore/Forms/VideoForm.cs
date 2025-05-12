using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BogseyVideoStore.Forms;
using MySql.Data.MySqlClient;

namespace BogseyVideoStore
{
    public partial class VideoForm : Form
    {
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

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void LoadVideos()
        {
            dgvVideos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvVideos.BackgroundColor = Color.FromArgb(28, 28, 28); 
            dgvVideos.GridColor = Color.FromArgb(212, 175, 55); 

            dgvVideos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(212, 175, 55); 
            dgvVideos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; 
            dgvVideos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dgvVideos.DefaultCellStyle.BackColor = Color.FromArgb(46, 46, 46); 
            dgvVideos.DefaultCellStyle.ForeColor = Color.White; 
            dgvVideos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60); 

            dgvVideos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 140, 0); 
            dgvVideos.DefaultCellStyle.SelectionForeColor = Color.Black; 
            
            dgvVideos.BorderStyle = BorderStyle.Fixed3D;

            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "SELECT * FROM videos";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvVideos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {

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
                    string searchQuery = "SELECT * FROM videos WHERE title LIKE @search";
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
                e.SuppressKeyPress = true; // Prevents the ding sound
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
            mainForm.Show();
        }
    }
}
