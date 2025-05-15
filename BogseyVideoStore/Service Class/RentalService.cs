using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace BogseyVideoStore.Service_Class
{
    internal static class RentalService
    {
        public static DataTable GetAllRentals(string connectionString)
        {
            var table = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT r.rental_id, c.customer_name AS customer, v.title AS video, v.video_id,
                           r.rent_date, r.due_date, r.return_date, r.total_price, r.overdue_price
                    FROM rentals r
                    JOIN customers c ON r.customer_id = c.customer_id
                    JOIN videos v ON r.video_id = v.video_id
                    ORDER BY r.rent_date DESC";
                using (var cmd = new MySqlCommand(query, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }
            return table;
        }

        public static DataTable GetRentalsByCustomer(string connectionString, int customerId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT r.rental_id, c.customer_name AS customer, v.title AS video, r.rent_date, r.due_date, r.total_price
                         FROM rentals r
                         JOIN customers c ON r.customer_id = c.customer_id
                         JOIN videos v ON r.video_id = v.video_id
                         WHERE r.customer_id = @customerId";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public static DataTable SearchRentals(string connectionString, string searchText)
        {
            var table = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT r.rental_id, c.customer_name AS customer, v.title AS video,
                           r.rent_date, r.due_date, r.return_date, r.total_price, r.overdue_price
                    FROM rentals r
                    JOIN customers c ON r.customer_id = c.customer_id
                    JOIN videos v ON r.video_id = v.video_id
                    WHERE c.customer_name LIKE @search OR v.title LIKE @search";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(table);
                    }
                }
            }
            return table;
        }

        public static void AddRental(string connectionString, int customerId, int videoId, DateTime rentDate, DateTime dueDate, decimal totalPrice, decimal overduePrice)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO rentals 
                                 (customer_id, video_id, rent_date, due_date, total_price, overdue_price)
                                 VALUES (@customer_id, @video_id, @rent_date, @due_date, @total_price, @overdue_price)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@customer_id", customerId);
                    cmd.Parameters.AddWithValue("@video_id", videoId);
                    cmd.Parameters.AddWithValue("@rent_date", rentDate);
                    cmd.Parameters.AddWithValue("@due_date", dueDate);
                    cmd.Parameters.AddWithValue("@total_price", totalPrice);
                    cmd.Parameters.AddWithValue("@overdue_price", overduePrice);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void ReturnRental(string connectionString, int rentalId, DateTime returnDate, decimal overduePrice)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE rentals 
                                 SET return_date = @return_date, overdue_price = @overdue_price
                                 WHERE rental_id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@return_date", returnDate);
                    cmd.Parameters.AddWithValue("@overdue_price", overduePrice);
                    cmd.Parameters.AddWithValue("@id", rentalId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void EditRental(string connectionString, int rentalId, int customerId, int videoId, DateTime rentDate, DateTime dueDate)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE rentals 
                                 SET customer_id = @cust, video_id = @vid, rent_date = @rent, due_date = @due
                                 WHERE rental_id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@cust", customerId);
                    cmd.Parameters.AddWithValue("@vid", videoId);
                    cmd.Parameters.AddWithValue("@rent", rentDate);
                    cmd.Parameters.AddWithValue("@due", dueDate);
                    cmd.Parameters.AddWithValue("@id", rentalId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteRental(string connectionString, int rentalId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM rentals WHERE rental_id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", rentalId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static string GetReceiptText(string customer, string video, string rentDate, string dueDate, string totalPrice, string overduePrice)
        {
            return
                "Bogsey Video Store\n" +
                "Rental Receipt\n\n" +
                $"Customer: {customer}\n" +
                $"Video: {video}\n" +
                $"Rent Date: {rentDate}\n" +
                $"Due Date: {dueDate}\n" +
                $"Total Price: ₱{totalPrice}\n" +
                $"Overdue Charge: ₱{overduePrice}\n\n";
        }
    }
}
