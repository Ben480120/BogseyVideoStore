using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BogseyVideoStore;
using BogseyVideoStore.Forms;
using MySql.Data.MySqlClient;


namespace BogseyVideoStore
{
    public partial class CustomerForm : Form
    {
        string connectionString = "server=localhost;database=bvs_db;uid=root;pwd=;";
        DataTable customerTable = new DataTable();

        public CustomerForm()
        {
            InitializeComponent();

            FormDesignHelper.StyleDataGridView(dgvCustomers);
            FormDesignHelper.StyleButton(btnAdd);
            FormDesignHelper.StyleButton(btnEdit);
            FormDesignHelper.StyleButton(btnDelete);
            FormDesignHelper.StyleButton(btnClear);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {

                connection.Open();
                string query = "INSERT INTO customers (customer_name, phone) VALUES (@name, @phone)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", txtCustomerName.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Customer added successfully!");


                LoadCustomers();
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

        private void btnEdit_Click(object sender, EventArgs e)
        {

            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                if (dgvCustomers.SelectedRows.Count > 0)
                {
                    int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells[0].Value);
                    string query = "UPDATE customers SET customer_name = @name, phone = @phone WHERE customer_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@name", txtCustomerName.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@id", customerId);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer updated successfully!");


                    LoadCustomers();
                }
                else
                {
                    MessageBox.Show("Please select a customer to edit.");
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                if (dgvCustomers.SelectedRows.Count > 0)
                {
                    int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells[0].Value);
                    string query = "DELETE FROM customers WHERE customer_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", customerId);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer deleted successfully!");


                    LoadCustomers();
                }
                else
                {
                    MessageBox.Show("Please select a customer to delete.");
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

        private void LoadCustomers()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "SELECT * FROM customers";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);

                customerTable.Clear();
                adapter.Fill(customerTable);

                dgvCustomers.DataSource = customerTable.DefaultView;
                dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

        private void CustomerForm_Load(object sender, EventArgs e)
        {

            LoadCustomers();

            AutoCompleteStringCollection customerNames = new AutoCompleteStringCollection();
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                string query = "SELECT customer_name FROM customers";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    customerNames.Add(reader.GetString("customer_name"));
                }

                txtCustomerName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtCustomerName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtCustomerName.AutoCompleteCustomSource = customerNames;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading autocomplete: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvCustomers.SelectedRows[0];
                txtCustomerName.Text = row.Cells[1]?.Value?.ToString() ?? string.Empty;
                txtPhone.Text = row.Cells[2]?.Value?.ToString() ?? string.Empty;
            }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCustomerName.Clear();
            txtPhone.Clear();
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            if (customerTable.Columns.Contains("customer_name"))
            {
                string filterText = txtCustomerName.Text.Trim().Replace("'", "''");
                DataView dv = customerTable.DefaultView;

                dv.RowFilter = $"customer_name LIKE '%{filterText}%'";
                dgvCustomers.DataSource = dv;
            }
        }

        private void pictureBoxShutdown_Click_1(object sender, EventArgs e)
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
