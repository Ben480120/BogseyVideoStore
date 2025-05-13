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
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check for duplicate title and category
                    string checkQuery = "SELECT COUNT(*) FROM videos WHERE title = @title AND category = @category";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@title", txtVideoTitle.Text);
                    checkCmd.Parameters.AddWithValue("@category", cmbCategory.SelectedItem?.ToString() ?? "");

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("A video with the same title and category already exists.");
                        return;
                    }

                    // Insert if not duplicate
                    string query = "INSERT INTO videos (title, category, quantity_in, quantity_out, rental_days_allowed) VALUES (@title, @category, @quantity_in, @quantity_out, @rental_days_allowed)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@title", txtVideoTitle.Text);
                    cmd.Parameters.AddWithValue("@category", cmbCategory.SelectedItem?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@quantity_in", txtQuantityIn.Text);
                    cmd.Parameters.AddWithValue("@quantity_out", txtQuantityOut.Text);
                    cmd.Parameters.AddWithValue("@rental_days_allowed", cmbRentalDaysAllowed.SelectedItem.ToString());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Video added successfully!");

                    LoadVideos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                if (dgvVideos.SelectedRows.Count > 0)
                {
                    int videoId = Convert.ToInt32(dgvVideos.SelectedRows[0].Cells[0].Value);
                    string query = "UPDATE videos SET title = @title, category = @category, quantity_in = @quantity_in, quantity_out = @quantity_out, rental_days_allowed = @rental_days_allowed WHERE video_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@title", txtVideoTitle.Text);
                    cmd.Parameters.AddWithValue("@category", cmbCategory.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@quantity_in", txtQuantityIn.Text);
                    cmd.Parameters.AddWithValue("@quantity_out", txtQuantityOut.Text);
                    cmd.Parameters.AddWithValue("@rental_days_allowed", cmbRentalDaysAllowed.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@id", videoId);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Video updated successfully!");

                    LoadVideos();
                }
                else
                {
                    MessageBox.Show("Please select a video to edit.");
                }
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    if (dgvVideos.SelectedRows.Count > 0)
                    {
                        int videoId = Convert.ToInt32(dgvVideos.SelectedRows[0].Cells[0].Value);

                        // Check if the video has rental records
                        string checkQuery = "SELECT COUNT(*) FROM rentals WHERE video_id = @id";
                        MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                        checkCmd.Parameters.AddWithValue("@id", videoId);

                        connection.Open();
                        int rentalCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (rentalCount > 0)
                        {
                            MessageBox.Show(
                                "Cannot delete this video because there is rental information associated with it.\n" +
                                "Please delete all rental records for this video before deleting the video.",
                                "Delete Not Allowed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                            return;
                        }

                        // Proceed with deletion if no rentals exist
                        string query = "DELETE FROM videos WHERE video_id = @id";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@id", videoId);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Video deleted successfully!");

                        LoadVideos();
                    }
                    else
                    {
                        MessageBox.Show("Please select a video to delete.");
                    }
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
        }


        private void LoadVideos()
        {
            dgvVideos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                string query = "SELECT video_id, title, category, quantity_in, quantity_out, rental_days_allowed FROM videos";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvVideos.DataSource = dt;

                if (dgvVideos.Columns.Contains("video_id"))
                {
                    dgvVideos.Columns["video_id"].Visible = false;
                }
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
            txtVideoTitle.Clear();
            cmbCategory.SelectedIndex = -1;
            txtQuantityIn.Clear();
            txtQuantityOut.Clear();
            cmbRentalDaysAllowed.SelectedIndex = 0;
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

    }
}
