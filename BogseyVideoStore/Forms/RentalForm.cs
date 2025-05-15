using BogseyVideoStore;
using BogseyVideoStore.Forms;
using BogseyVideoStore.Service_Class;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Imaging;

namespace BogseyVideoStore
{
    public partial class RentalForm : Form
    {
        private PrintDocument printDocument;
        private string receiptText;

        string connectionString = "server=localhost;database=bvs_db;uid=root;pwd=;";
        string videoCategory = "";
        int rentalDaysAllowed = 0;
        decimal basePrice = 0;

        private int rentalDaysLimit = 1;

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
            dgvRentals.DataSource = RentalService.GetAllRentals(connectionString);

            if (dgvRentals.Columns.Contains("rental_id"))
            {
                dgvRentals.Columns["rental_id"].Visible = false;
            }
            if (dgvRentals.Columns.Contains("video_id"))
            {
                dgvRentals.Columns["video_id"].Visible = false;
            }
        }

        private void LoadRentalsForCustomer(int customerId)
        {
            dgvRentals.DataSource = RentalService.GetRentalsByCustomer(connectionString, customerId);

            if (dgvRentals.Columns.Contains("rental_id"))
            {
                dgvRentals.Columns["rental_id"].Visible = false;
            }
            if (dgvRentals.Columns.Contains("video_id"))
            {
                dgvRentals.Columns["video_id"].Visible = false;
            }
        }


        private void LoadVideos()
        {
            cmbVideo.Items.Clear();
            string query = "SELECT video_id, title, category, quantity_in FROM videos";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32("video_id");
                    string title = reader.GetString("title");
                    string category = reader.GetString("category");
                    int quantityIn = reader.GetInt32("quantity_in");
                    string displayText = $"{title} ({category}) | Stock: {quantityIn}";
                    cmbVideo.Items.Add(new ComboBoxItem(displayText, id.ToString()));
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
            FormDesignHelper.StyleButton(btnPrintReceipt);
            LoadCustomers();
            LoadVideos();
            dgvRentals.CellClick += dgvRentals_CellClick;
        }


        private void btnRent_Click(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedItem == null || cmbVideo.SelectedItem == null)
            {
                MessageBox.Show("Please select both a customer and a video.");
                return;
            }

            var customer = (ComboBoxItem)cmbCustomer.SelectedItem;
            var video = (ComboBoxItem)cmbVideo.SelectedItem;

            int availableStock = 0;
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

            RentalService.AddRental(
                connectionString,
                int.Parse(customer.Value),
                int.Parse(video.Value),
                rentDate,
                dueDate,
                totalPrice,
                overduePrice
            );

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var updateStockCmd = new MySqlCommand("UPDATE videos SET quantity_in = quantity_in - 1, quantity_out = quantity_out + 1 WHERE video_id = @id", conn);
                updateStockCmd.Parameters.AddWithValue("@id", video.Value);
                updateStockCmd.ExecuteNonQuery();
            }

            MessageBox.Show("Rental recorded successfully!");

            LoadRentalsForCustomer(int.Parse(customer.Value));
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (dgvRentals.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a rental to return.");
                return;
            }

            int rentalId = Convert.ToInt32(dgvRentals.SelectedRows[0].Cells["rental_id"].Value);

            DateTime rentDate, dueDate;
            int videoId;

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string getDetailsQuery = "SELECT rent_date, due_date, video_id FROM rentals WHERE rental_id = @id";
                var getCmd = new MySqlCommand(getDetailsQuery, conn);
                getCmd.Parameters.AddWithValue("@id", rentalId);
                using (var reader = getCmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        MessageBox.Show("Rental details not found.");
                        return;
                    }
                    rentDate = Convert.ToDateTime(reader["rent_date"]);
                    dueDate = Convert.ToDateTime(reader["due_date"]);
                    videoId = Convert.ToInt32(reader["video_id"]);
                }
            }

            LoadVideoDetails(videoId);

            DateTime returnDate = DateTime.Now.Date;
            int overdueDays = (returnDate - dueDate.Date).Days;
            decimal overduePrice = (overdueDays > 0) ? overdueDays * 5 : 0;

            RentalService.ReturnRental(connectionString, rentalId, returnDate, overduePrice);

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var updateStockCmd = new MySqlCommand("UPDATE videos SET quantity_in = quantity_in + 1, quantity_out = quantity_out - 1 WHERE video_id = @id", conn);
                updateStockCmd.Parameters.AddWithValue("@id", videoId);
                updateStockCmd.ExecuteNonQuery();
            }

            MessageBox.Show($"Video returned.\nOverdue charge: ₱{overduePrice}");
            LoadRentals();
        }


        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedItem is ComboBoxItem selectedCustomer)
            {
                LoadRentalsForCustomer(int.Parse(selectedCustomer.Value));
            }
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

                SetRentalDaysLimit();

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

            RentalService.EditRental(
                connectionString,
                rentalId,
                int.Parse(customer.Value),
                int.Parse(video.Value),
                rentDate,
                dueDate
            );

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

            RentalService.DeleteRental(connectionString, rentalId);
            MessageBox.Show("Rental deleted.");
            LoadRentals();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void SearchRentals(string searchText)
        {
            dgvRentals.DataSource = RentalService.SearchRentals(connectionString, searchText);

            if (dgvRentals.Columns.Contains("rental_id"))
            {
                dgvRentals.Columns["rental_id"].Visible = false;
            }
            if (dgvRentals.Columns.Contains("video_id"))
            {
                dgvRentals.Columns["video_id"].Visible = false;
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

        private void PrintSelectedReceipt()
        {
            if (dgvRentals.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select at least one rental to print a receipt.");
                return;
            }

            string customer = dgvRentals.SelectedRows[0].Cells["customer"].Value.ToString();

            var rentalDetails = dgvRentals.SelectedRows
            .Cast<DataGridViewRow>()
            .Select(row =>
            {
                string video = row.Cells["video"].Value.ToString();
                string rentDate = DateTime.Parse(row.Cells["rent_date"].Value.ToString()).ToString("dd/MM/yyyy");
                string dueDate = DateTime.Parse(row.Cells["due_date"].Value.ToString()).ToString("dd/MM/yyyy");
                string totalPrice = row.Cells["total_price"].Value.ToString();
                string overduePrice = row.Cells["overdue_price"].Value.ToString();
                return $"Video: {video}\nRent Date: {rentDate}\nDue Date: {dueDate}\nTotal Price: {totalPrice}\nOverdue Charge: {overduePrice}";
            })
            .ToArray();

            receiptText = string.Join("\n\n-----------------------------\n\n", rentalDetails);


            _printCustomerName = customer;

            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;

            PrintPreviewDialog previewDialog = new PrintPreviewDialog();
            previewDialog.Document = printDocument;
            previewDialog.ShowDialog();
        }

        private string _printCustomerName = "";

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            Image logo = Properties.Resources.BVS_Logo;
            int logoWidth = 1000;
            int logoHeight = 1000;
            int logoX = e.MarginBounds.Left + (e.MarginBounds.Width - logoWidth) / 2;
            int logoY = e.MarginBounds.Top + (e.MarginBounds.Height - logoHeight) / 2;

            float transparency = 0.15f;
            ColorMatrix colorMatrix = new ColorMatrix();
            colorMatrix.Matrix33 = transparency;
            ImageAttributes imgAttributes = new ImageAttributes();
            imgAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            g.DrawImage(
                logo,
                new Rectangle(logoX, logoY, logoWidth, logoHeight),
                0, 0, logo.Width, logo.Height,
                GraphicsUnit.Pixel,
                imgAttributes
            );

            Font titleFont = new Font("Arial", 18, FontStyle.Bold);
            Font subtitleFont = new Font("Arial", 12, FontStyle.Bold);
            Font bodyFont = new Font("Arial", 10, FontStyle.Regular);
            Font boldBodyFont = new Font("Arial", 10, FontStyle.Bold);
            Font italicFont = new Font("Arial", 9, FontStyle.Italic);

            StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };

            float lineSpacing = 3f;
            float blockSpacing = 8f;

            var receiptBlocks = receiptText.Split(new[] { "\n\n-----------------------------\n\n" }, StringSplitOptions.None);
            decimal finalTotal = 0m;
            decimal totalOverdue = 0m;

            // Calculate totals
            foreach (var block in receiptBlocks)
            {
                var lines = block.Split('\n');
                foreach (var line in lines)
                {
                    if (line.StartsWith("Total Price:"))
                    {
                        string priceStr = line.Replace("Total Price:", "").Replace("₱", "").Trim();
                        decimal price;
                        if (decimal.TryParse(priceStr, out price))
                            finalTotal += price;
                    }
                    else if (line.StartsWith("Overdue Charge:"))
                    {
                        string overdueStr = line.Replace("Overdue Charge:", "").Replace("₱", "").Trim();
                        decimal overdue;
                        if (decimal.TryParse(overdueStr, out overdue))
                            totalOverdue += overdue;
                    }
                }
            }

            float totalHeight = 0;
            totalHeight += titleFont.GetHeight(g) + lineSpacing;
            totalHeight += subtitleFont.GetHeight(g) + lineSpacing;
            totalHeight += bodyFont.GetHeight(g) + lineSpacing * 2;

            float separatorHeight = bodyFont.GetHeight(g) + blockSpacing;
            totalHeight += separatorHeight;

            foreach (var block in receiptBlocks)
            {
                var lines = block.Split('\n');
                foreach (var line in lines)
                {
                    if (line.StartsWith("Total Price:") || line.StartsWith("Overdue Charge:"))
                    {
                        totalHeight += boldBodyFont.GetHeight(g) + lineSpacing;
                    }
                    else
                    {
                        totalHeight += bodyFont.GetHeight(g) + lineSpacing;
                    }
                }
                totalHeight += blockSpacing;
            }

            totalHeight += separatorHeight;
            totalHeight += boldBodyFont.GetHeight(g) + lineSpacing; // Final Total
            totalHeight += boldBodyFont.GetHeight(g) + lineSpacing; // Total Overdue
            totalHeight += boldBodyFont.GetHeight(g) + lineSpacing; // Extra space
            totalHeight += italicFont.GetHeight(g) + 10;

            float y = e.MarginBounds.Top + (e.MarginBounds.Height - totalHeight) / 2;
            float rectX = e.MarginBounds.Left;
            float rectWidth = e.MarginBounds.Width;

            g.DrawString("Bogsey Video Store", titleFont, Brushes.Black, new RectangleF(rectX, y, rectWidth, titleFont.GetHeight(g)), sf);
            y += titleFont.GetHeight(g) + lineSpacing;
            g.DrawString("Rental Receipt", subtitleFont, Brushes.Black, new RectangleF(rectX, y, rectWidth, subtitleFont.GetHeight(g)), sf);
            y += subtitleFont.GetHeight(g) + lineSpacing;
            g.DrawString($"Customer: {_printCustomerName}", bodyFont, Brushes.Black, new RectangleF(rectX, y, rectWidth, bodyFont.GetHeight(g)), sf);
            y += bodyFont.GetHeight(g) + lineSpacing * 2;

            g.DrawString("----------------------------------------", bodyFont, Brushes.Black, new RectangleF(rectX, y, rectWidth, bodyFont.GetHeight(g)), sf);
            y += separatorHeight;

            for (int i = 0; i < receiptBlocks.Length; i++)
            {
                var lines = receiptBlocks[i].Split('\n');
                foreach (var line in lines)
                {
                    if (line.StartsWith("Total Price:") || line.StartsWith("Overdue Charge:"))
                    {
                        g.DrawString(line, boldBodyFont, Brushes.Black, new RectangleF(rectX, y, rectWidth, boldBodyFont.GetHeight(g)), sf);
                        y += boldBodyFont.GetHeight(g) + lineSpacing;
                    }
                    else
                    {
                        g.DrawString(line, bodyFont, Brushes.Black, new RectangleF(rectX, y, rectWidth, bodyFont.GetHeight(g)), sf);
                        y += bodyFont.GetHeight(g) + lineSpacing;
                    }
                }
                y += blockSpacing;
            }

            g.DrawString("----------------------------------------", bodyFont, Brushes.Black, new RectangleF(rectX, y, rectWidth, bodyFont.GetHeight(g)), sf);
            y += separatorHeight;

            // Final Total 
            g.DrawString($"Final Total: ₱{(finalTotal + totalOverdue):0.00}", boldBodyFont, Brushes.Black, new RectangleF(rectX, y, rectWidth, boldBodyFont.GetHeight(g)), sf);
            y += boldBodyFont.GetHeight(g) + lineSpacing;

            // Total Overdue 
            g.DrawString($"Total Overdue: ₱{totalOverdue:0.00}", boldBodyFont, Brushes.Black, new RectangleF(rectX, y, rectWidth, boldBodyFont.GetHeight(g)), sf);
            y += boldBodyFont.GetHeight(g) + lineSpacing * 2;

            g.DrawString("Thank you for renting with us!", italicFont, Brushes.Black, new RectangleF(rectX, y, rectWidth, italicFont.GetHeight(g)), sf);

            titleFont.Dispose();
            subtitleFont.Dispose();
            bodyFont.Dispose();
            boldBodyFont.Dispose();
            italicFont.Dispose();
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            PrintSelectedReceipt();
        }
    }
}
