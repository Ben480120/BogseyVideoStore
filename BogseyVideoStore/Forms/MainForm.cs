using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BogseyVideoStore.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            FormDesignHelper.StyleButton(btnCustomerForm);
            FormDesignHelper.StyleButton(btnVideoForm);
            FormDesignHelper.StyleButton(btnRentalForm);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnCustomerForm_Click(object sender, EventArgs e)
        {
            var customerForm = new CustomerForm();
            customerForm.Show();
        }

        private void btnVideoForm_Click(object sender, EventArgs e)
        {
            var videoForm = new VideoForm();
            videoForm.Show();
        }

        private void btnRentalForm_Click(object sender, EventArgs e)
        {
            var rentalForm = new RentalForm();
            rentalForm.Show();
        }

        private void pictureBoxShutdown_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
