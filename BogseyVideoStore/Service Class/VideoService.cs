using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BogseyVideoStore.Helpers
{
    public class VideoService
    {
        private string connectionString = "server=localhost;database=bvs_db;uid=root;pwd=;";

        public DataTable GetAllVideos()
        {
            DataTable table = new DataTable();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM videos";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    adapter.Fill(table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            return table;
        }

        public bool AddVideo(string title, string category, int quantityIn, int quantityOut, int rentalDaysAllowed)
        {
            if (string.IsNullOrWhiteSpace(title) || !Regex.IsMatch(title, @"^[a-zA-Z0-9\s]+$"))
            {
                MessageBox.Show("Video title must contain only letters, numbers, and spaces, and cannot be empty.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(category))
            {
                MessageBox.Show("Category cannot be empty.");
                return false;
            }
            if (quantityIn < 0)
            {
                MessageBox.Show("Quantity In must be 0 or greater.");
                return false;
            }
            if (quantityOut < 0)
            {
                MessageBox.Show("Quantity Out must be 0 or greater.");
                return false;
            }
            if (rentalDaysAllowed <= 0)
            {
                MessageBox.Show("Rental days allowed must be greater than 0.");
                return false;
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM videos WHERE title = @title AND category = @category";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@title", title);
                    checkCmd.Parameters.AddWithValue("@category", category);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("A video with this title and category already exists.");
                        return false;
                    }

                    string query = "INSERT INTO videos (title, category, quantity_in, quantity_out, rental_days_allowed) VALUES (@title, @category, @quantity_in, @quantity_out, @rental_days_allowed)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@quantity_in", quantityIn);
                    cmd.Parameters.AddWithValue("@quantity_out", quantityOut);
                    cmd.Parameters.AddWithValue("@rental_days_allowed", rentalDaysAllowed);
                    cmd.ExecuteNonQuery();

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }

        public bool UpdateVideo(int id, string title, string category, int quantityIn, int quantityOut, int rentalDaysAllowed)
        {
            if (string.IsNullOrWhiteSpace(title) || !Regex.IsMatch(title, @"^[a-zA-Z0-9\s]+$"))
            {
                MessageBox.Show("Video title must contain only letters, numbers, and spaces, and cannot be empty.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(category))
            {
                MessageBox.Show("Category cannot be empty.");
                return false;
            }
            if (quantityIn < 0)
            {
                MessageBox.Show("Quantity In must be 0 or greater.");
                return false;
            }
            if (quantityOut < 0)
            {
                MessageBox.Show("Quantity Out must be 0 or greater.");
                return false;
            }
            if (rentalDaysAllowed <= 0)
            {
                MessageBox.Show("Rental days allowed must be greater than 0.");
                return false;
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM videos WHERE title = @title AND category = @category AND video_id <> @id";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@title", title);
                    checkCmd.Parameters.AddWithValue("@category", category);
                    checkCmd.Parameters.AddWithValue("@id", id);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("A video with this title and category already exists.");
                        return false;
                    }

                    string query = @"UPDATE videos 
                             SET title = @title, 
                                 category = @category, 
                                 quantity_in = @quantityIn, 
                                 quantity_out = @quantityOut, 
                                 rental_days_allowed = @rentalDays 
                             WHERE video_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@quantityIn", quantityIn);
                    cmd.Parameters.AddWithValue("@quantityOut", quantityOut);
                    cmd.Parameters.AddWithValue("@rentalDays", rentalDaysAllowed);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }


        public void DeleteVideo(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM rentals WHERE video_id = @id";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@id", id);
                    int rentalCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (rentalCount > 0)
                    {
                        MessageBox.Show(
                            "Cannot delete this video because rental records are associated with it.\n" +
                            "Please delete the related rental records first.",
                            "Delete Not Allowed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }

                    string query = "DELETE FROM videos WHERE video_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Video deleted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        public AutoCompleteStringCollection GetVideoTitles()
        {
            var collection = new AutoCompleteStringCollection();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT title FROM videos";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        collection.Add(reader.GetString("title"));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            return collection;
        }
    }
}
