using BogseyVideoStore;
using BogseyVideoStore.Forms;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace BogseyVideoStore
{
    public partial class RentalForm : Form
    {
        string connectionString = "server=localhost;database=bvs_db;uid=root;pwd=;";
        string videoCategory = "";
        int rentalDaysAllowed = 0;
        decimal basePrice = 0;


        private int rentalDaysLimit = 1;

        //set rental days limit
        private void SetRentalDaysLimit()
        {
            if (cmbVideo.SelectedItem is ComboBoxItem selectedVideo)
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT rental_days_allowed FROM videos WHERE video_id = @id";
                    var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", selectedVideo.Value);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        rentalDaysLimit = Convert.ToInt32(result);
                        numDays.Maximum = rentalDaysLimit;
                        numDays.Value = Math.Min(numDays.Value, rentalDaysLimit);
                    }
                }
            }
        }

        private void cmbVideo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRentalDaysLimit();
        }

        private void RentalForm_Load(object sender, EventArgs e)
        {
            LoadCustomers();
            LoadVideos();
            LoadRentals();
        }

        private void LoadCustomers()
        {
            cmbCustomer.Items.Clear();
            string query = "SELECT customer_id, customer_name FROM customers";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32("customer_id");
                    string name = reader.GetString("customer_name");
                    cmbCustomer.Items.Add(new ComboBoxItem(name, id.ToString()));
                }

                conn.Close();
            }

            cmbCustomer.SelectedIndex = -1;
        }

        private void LoadRentals()
        {
            dgvRentals.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRentals.Rows.Clear();
            dgvRentals.Columns.Clear();

            dgvRentals.Columns.Add("rental_id", "Rental ID");
            dgvRentals.Columns["rental_id"].Visible = false;
            dgvRentals.Columns.Add("customer", "Customer");
            dgvRentals.Columns.Add("video", "Video");
            dgvRentals.Columns.Add("video_id", "Video ID");
            dgvRentals.Columns["video_id"].Visible = false;
            dgvRentals.Columns.Add("rent_date", "Rent Date");
            dgvRentals.Columns.Add("due_date", "Due Date");
            dgvRentals.Columns.Add("return_date", "Return Date");
            dgvRentals.Columns.Add("total_price", "Total Price");
            dgvRentals.Columns.Add("overdue_price", "Overdue Price");

            string query = @"
        SELECT 
            r.rental_id,
            c.customer_name AS customer,
            v.title AS video, 
            v.video_id,                    
            r.rent_date, 
            r.due_date, 
            r.return_date,
            r.total_price,
            r.overdue_price
        FROM rentals r
        JOIN customers c ON r.customer_id = c.customer_id
        JOIN videos v ON r.video_id = v.video_id
        ORDER BY r.rent_date DESC";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dgvRentals.Rows.Add(
                    reader["rental_id"].ToString(),
                    reader["customer"].ToString(),
                    reader["video"].ToString(),
                    reader["video_id"].ToString(),
                    Convert.ToDateTime(reader["rent_date"]).ToString("yyyy-MM-dd"),
                    Convert.ToDateTime(reader["due_date"]).ToString("yyyy-MM-dd"),
                    reader["return_date"] == DBNull.Value
                        ? ""
                        : Convert.ToDateTime(reader["return_date"]).ToString("yyyy-MM-dd"),
                    reader["total_price"] == DBNull.Value
                        ? "0.00"
                        : Convert.ToDecimal(reader["total_price"]).ToString("F2"),
                    reader["overdue_price"] == DBNull.Value
                        ? "0.00"
                        : Convert.ToDecimal(reader["overdue_price"]).ToString("F2")
                );
                }
            }
        }

        private void LoadVideos()
        {
            cmbVideo.Items.Clear();
            string query = "SELECT video_id, title FROM videos";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32("video_id");
                    string title = reader.GetString("title");
                    cmbVideo.Items.Add(new ComboBoxItem(title, id.ToString()));
                }

                conn.Close();
            }

            cmbVideo.SelectedIndex = -1;
        }

        public RentalForm()
        {
            InitializeComponent();

            FormDesignHelper.StyleDataGridView(dgvRentals);
            FormDesignHelper.StyleButton(btnEdit);
            FormDesignHelper.StyleButton(btnDelete);
            FormDesignHelper.StyleButton(btnSearch);
            FormDesignHelper.StyleButton(btnRent);
            FormDesignHelper.StyleButton(btnReturn);
            LoadCustomers();
            LoadVideos();
            dgvRentals.CellClick += dgvRentals_CellClick;
        }


        private void btnRent_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            // Check if both a customer and a video are selected first
            if (cmbCustomer.SelectedItem == null || cmbVideo.SelectedItem == null)
            {
                MessageBox.Show("Please select both a customer and a video.");
                return;
            }

            var customer = (ComboBoxItem)cmbCustomer.SelectedItem;
            var video = (ComboBoxItem)cmbVideo.SelectedItem;

            int availableStock = 0;

            // Check available stock for the selected video
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var checkStockCmd = new MySqlCommand("SELECT quantity_in FROM videos WHERE video_id = @id", conn);
                checkStockCmd.Parameters.AddWithValue("@id", video.Value);
                var result = checkStockCmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    availableStock = Convert.ToInt32(result);
            }

            if (availableStock <= 0)
            {
                MessageBox.Show("This video is out of stock and cannot be rented.");
                return;
            }

            LoadVideoDetails(int.Parse(video.Value));

            DateTime rentDate = DateTime.Now;
            int days = (int)numDays.Value;
            DateTime dueDate = rentDate.AddDays(days);

            decimal totalPrice = CalculateTotalPrice(rentDate, dueDate);
            decimal overduePrice = totalPrice - basePrice;
            if (overduePrice < 0) overduePrice = 0;

            var confirmResult = MessageBox.Show($"Total Price: ₱{totalPrice}\nProceed with rental?", "Confirm Rental", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.No) return;

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO rentals 
                 (customer_id, video_id, rent_date, due_date, total_price, overdue_price)
                 VALUES (@customer_id, @video_id, @rent_date, @due_date, @total_price, @overdue_price)";

                var cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@customer_id", customer.Value);
                cmd.Parameters.AddWithValue("@video_id", video.Value);
                cmd.Parameters.AddWithValue("@rent_date", rentDate);
                cmd.Parameters.AddWithValue("@due_date", dueDate);
                cmd.Parameters.AddWithValue("@total_price", totalPrice);
                cmd.Parameters.AddWithValue("@overdue_price", overduePrice);
                cmd.ExecuteNonQuery();

                var updateStockCmd = new MySqlCommand("UPDATE videos SET quantity_in = quantity_in - 1, quantity_out = quantity_out + 1 WHERE video_id = @id", conn);
                updateStockCmd.Parameters.AddWithValue("@id", video.Value);
                updateStockCmd.ExecuteNonQuery();
            }

            MessageBox.Show("Rental recorded successfully!");
            LoadRentals();
        }
        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (dgvRentals.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a rental to return.");
                return;
            }

            int rentalId = Convert.ToInt32(dgvRentals.SelectedRows[0].Cells["rental_id"].Value);

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string getDetailsQuery = "SELECT rent_date, due_date, video_id FROM rentals WHERE rental_id = @id";
                var getCmd = new MySqlCommand(getDetailsQuery, conn);
                getCmd.Parameters.AddWithValue("@id", rentalId);
                using (var reader = getCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DateTime rentDate = Convert.ToDateTime(reader["rent_date"]);
                        DateTime dueDate = Convert.ToDateTime(reader["due_date"]);
                        int videoId = Convert.ToInt32(reader["video_id"]);

                        reader.Close(); 
                      
                        LoadVideoDetails(videoId);

                        DateTime returnDate = DateTime.Now;
                        int totalDays = (returnDate - rentDate).Days;
                        int overdueDays = totalDays - rentalDaysAllowed;
                        decimal overduePrice = (overdueDays > 0) ? overdueDays * 5 : 0;

                        string updateQuery = @"UPDATE rentals 
                                       SET return_date = @return_date, overdue_price = @overdue_price
                                       WHERE rental_id = @id";
                        var updateCmd = new MySqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@return_date", returnDate);
                        updateCmd.Parameters.AddWithValue("@overdue_price", overduePrice);
                        updateCmd.Parameters.AddWithValue("@id", rentalId);
                        updateCmd.ExecuteNonQuery();

                        var updateStockCmd = new MySqlCommand("UPDATE videos SET quantity_in = quantity_in + 1, quantity_out = quantity_out - 1 WHERE video_id = @id", conn);
                        updateStockCmd.Parameters.AddWithValue("@id", videoId);
                        updateStockCmd.ExecuteNonQuery();

                        MessageBox.Show($"Video returned.\nOverdue charge: ₱{overduePrice}");
                    }
                    else
                    {
                        MessageBox.Show("Rental details not found.");
                    }
                }
            }

            LoadRentals();
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvRentals_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string selectedCustomer = dgvRentals.Rows[e.RowIndex].Cells["customer"].Value.ToString();
                string selectedVideo = dgvRentals.Rows[e.RowIndex].Cells["video"].Value.ToString();

                cmbCustomer.SelectedItem = cmbCustomer.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Text == selectedCustomer);

                cmbVideo.SelectedItem = cmbVideo.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Text == selectedVideo);
            }
        }
        private void dgvRentals_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvRentals.Rows[e.RowIndex];

                string selectedCustomer = row.Cells["customer"].Value.ToString();
                string selectedVideo = row.Cells["video"].Value.ToString();

                cmbCustomer.SelectedItem = cmbCustomer.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Text == selectedCustomer);

                cmbVideo.SelectedItem = cmbVideo.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Text == selectedVideo);

                // Call limiter after setting video
                SetRentalDaysLimit();

                // Calculate number of rental days based on rent date and due date
                DateTime rentDate = DateTime.Parse(row.Cells["rent_date"].Value.ToString());
                DateTime dueDate = DateTime.Parse(row.Cells["due_date"].Value.ToString());
                int days = (dueDate - rentDate).Days;

                numDays.Value = Math.Min(days, numDays.Maximum);
            }
        }



        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvRentals.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a rental to edit.");
                return;
            }

            int rentalId = Convert.ToInt32(dgvRentals.SelectedRows[0].Cells["rental_id"].Value);

            var customer = (ComboBoxItem)cmbCustomer.SelectedItem;
            var video = (ComboBoxItem)cmbVideo.SelectedItem;
            int days = (int)numDays.Value;

            DateTime rentDate = DateTime.Now;
            DateTime dueDate = rentDate.AddDays(days);

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE rentals SET customer_id = @cust, video_id = @vid, rent_date = @rent, due_date = @due WHERE rental_id = @id", conn);
                cmd.Parameters.AddWithValue("@cust", customer.Value);
                cmd.Parameters.AddWithValue("@vid", video.Value);
                cmd.Parameters.AddWithValue("@rent", rentDate);
                cmd.Parameters.AddWithValue("@due", dueDate);
                cmd.Parameters.AddWithValue("@id", rentalId);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Rental updated.");
            LoadRentals();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRentals.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a rental to delete.");
                return;
            }

            int rentalId = Convert.ToInt32(dgvRentals.SelectedRows[0].Cells["rental_id"].Value);
            int videoId = 0;
            bool notReturned = false;

            // Get video_id and return_date for the selected rental
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var getCmd = new MySqlCommand("SELECT video_id, return_date FROM rentals WHERE rental_id = @id", conn);
                getCmd.Parameters.AddWithValue("@id", rentalId);
                using (var reader = getCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        videoId = Convert.ToInt32(reader["video_id"]);
                        notReturned = reader["return_date"] == DBNull.Value;
                    }
                }
            }

            // If rental not returned, update stock
            if (notReturned)
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var updateStockCmd = new MySqlCommand(
                        "UPDATE videos SET quantity_in = quantity_in + 1, quantity_out = quantity_out - 1 WHERE video_id = @id", conn);
                    updateStockCmd.Parameters.AddWithValue("@id", videoId);
                    updateStockCmd.ExecuteNonQuery();
                }
            }

            // Delete the rental
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM rentals WHERE rental_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", rentalId);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Rental deleted.");
            LoadRentals();
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        //Search Function
        private void SearchRentals(string searchText)
        {
            dgvRentals.Rows.Clear();
            dgvRentals.Columns.Clear();

            dgvRentals.Columns.Add("rental_id", "Rental ID");
            dgvRentals.Columns["rental_id"].Visible = false;
            dgvRentals.Columns.Add("customer", "Customer");
            dgvRentals.Columns.Add("video", "Video");
            dgvRentals.Columns.Add("rent_date", "Rent Date");
            dgvRentals.Columns.Add("due_date", "Due Date");
            dgvRentals.Columns.Add("return_date", "Return Date");
            dgvRentals.Columns.Add("total_price", "Total Price");
            dgvRentals.Columns.Add("overdue_price", "Overdue Price");

            string query = @"SELECT r.rental_id, c.customer_name AS customer,
         v.title AS video, r.rent_date, r.due_date, r.return_date,
         r.total_price, r.overdue_price
         FROM rentals r
         JOIN customers c ON r.customer_id = c.customer_id
         JOIN videos v ON r.video_id = v.video_id
         WHERE c.customer_name LIKE @search OR v.title LIKE @search";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dgvRentals.Rows.Add(
                        reader["rental_id"].ToString(),
                        reader["customer"].ToString(),
                        reader["video"].ToString(),
                        Convert.ToDateTime(reader["rent_date"]).ToString("yyyy-MM-dd"),
                        Convert.ToDateTime(reader["due_date"]).ToString("yyyy-MM-dd"),
                        reader["return_date"] == DBNull.Value ? "" : Convert.ToDateTime(reader["return_date"]).ToString("yyyy-MM-dd"),
                        reader["total_price"] == DBNull.Value ? "0.00" : Convert.ToDecimal(reader["total_price"]).ToString("F2"),
                        reader["overdue_price"] == DBNull.Value ? "0.00" : Convert.ToDecimal(reader["overdue_price"]).ToString("F2")
                    );
                }
            }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                SearchRentals(keyword);
            }
            else
            {
                LoadRentals();
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void RentalForm_Load_1(object sender, EventArgs e)
        {
            LoadRentals();
        }

        private void cmbVideo_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            SetRentalDaysLimit();
        }

        private void LoadVideoDetails(int videoId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT category, rental_days_allowed FROM videos WHERE video_id = @videoId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@videoId", videoId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            videoCategory = reader["category"].ToString();
                            rentalDaysAllowed = Convert.ToInt32(reader["rental_days_allowed"]);

                            if (videoCategory == "DVD")
                                basePrice = 50;
                            else if (videoCategory == "VCD")
                                basePrice = 25;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading video details: " + ex.Message);
                }
            }
        }

        private decimal CalculateTotalPrice(DateTime rentDate, DateTime returnDate)
        {
            decimal lateFee = 0;

            int daysRented = (returnDate - rentDate).Days;
            if (daysRented > rentalDaysAllowed)
            {
                int overdueDays = daysRented - rentalDaysAllowed;
                lateFee = overdueDays * 5;
            }

            return basePrice + lateFee;
        }

        private void pictureBoxShutdown_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
