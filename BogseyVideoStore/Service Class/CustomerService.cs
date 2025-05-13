using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace BogseyVideoStore.Helpers
{
    public class CustomerService
    {
        private string connectionString = "server=localhost;database=bvs_db;uid=root;pwd=;";

        public DataTable GetAllCustomers()
        {
            DataTable table = new DataTable();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM customers";
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

        public bool AddCustomer(string name, string phone)
        {
            if (string.IsNullOrWhiteSpace(name) || !System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Customer name must contain only letters and spaces, and cannot be empty.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(phone) || !System.Text.RegularExpressions.Regex.IsMatch(phone, @"^09\d{9}$"))
            {
                MessageBox.Show("Phone number must be 11 digits, start with '09', and contain only numbers.");
                return false;
            }           
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM customers WHERE customer_name = @name";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@name", name);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("A customer with this name already exists.");
                        return false;
                    }

                    string query = "INSERT INTO customers (customer_name, phone) VALUES (@name, @phone)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@phone", phone);
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

        public bool UpdateCustomer(int id, string name, string phone)
        {
            if (string.IsNullOrWhiteSpace(phone) || !System.Text.RegularExpressions.Regex.IsMatch(phone, @"^09\d{9}$"))
            {
                MessageBox.Show("Phone number must be 11 digits, start with '09', and contain only numbers.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(name) || !System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Customer name must contain only letters and spaces, and cannot be empty.");
                return false;
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check for duplicate name (excluding current customer)
                    string checkQuery = "SELECT COUNT(*) FROM customers WHERE customer_name = @name AND customer_id <> @id";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@name", name);
                    checkCmd.Parameters.AddWithValue("@id", id);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("A customer with this name already exists.");
                        return false;
                    }

                    string query = "UPDATE customers SET customer_name = @name, phone = @phone WHERE customer_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@phone", phone);
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

        public void DeleteCustomer(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM rentals WHERE customer_id = @id";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@id", id);
                    int rentalCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (rentalCount > 0)
                    {
                        MessageBox.Show(
                            "Cannot delete this customer because there is rental information associated with them.\n" +
                            "Please delete all rental records for this customer before deleting the customer.",
                            "Delete Not Allowed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }

                    string query = "DELETE FROM customers WHERE customer_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Customer deleted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        public AutoCompleteStringCollection GetCustomerNames()
        {
            var collection = new AutoCompleteStringCollection();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT customer_name FROM customers";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        collection.Add(reader.GetString("customer_name"));
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
