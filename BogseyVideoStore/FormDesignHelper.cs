using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BogseyVideoStore
{
    public static class FormDesignHelper
    {
        // Style DataGridView
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.FromArgb(28, 28, 28); // Dark Charcoal
            dgv.GridColor = Color.FromArgb(212, 175, 55); // Gold

            // Header Style
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(212, 175, 55); // Gold
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // White
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            // Row Style
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(46, 46, 46); // Dark Gray
            dgv.DefaultCellStyle.ForeColor = Color.White; // White
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60); // Medium Gray

            // Selected Row Style
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 140, 0); // Dark Orange
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black; // Black

            // Border Style
            dgv.BorderStyle = BorderStyle.None; // Remove the white border
        }


        // Style Buttons
        public static void StyleButton(Button button)
        {
            button.BackColor = Color.FromArgb(212, 175, 55); // Gold
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(255, 140, 0); // Dark Orange
            button.FlatAppearance.BorderSize = 1;
        }
    }
}

