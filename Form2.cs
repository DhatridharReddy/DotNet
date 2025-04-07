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
using System.Data.SqlClient;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient;


namespace To_Do_List_App
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create an instance of Form3
            Form3 form3 = new Form3();

            // Show Form3
            form3.Show();

            // Hide the current form (Form2)
            this.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                string connString = "Server=localhost; Database=vp; Uid=root; Pwd=Siddhardha@1;";
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        string query = "SELECT * FROM LOGIN WHERE username = @username AND password = @password";
                        MySqlCommand cmd = new MySqlCommand(query, conn);

                        // Use parameters to prevent SQL Injection
                        cmd.Parameters.AddWithValue("@username", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", textBox2.Text.Trim());

                        MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                        DataTable dta = new DataTable();
                        sda.Fill(dta);

                        if (dta.Rows.Count == 1)
                        {
                            Form1 todo = new Form1();
                            this.Hide();
                            todo.Show();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }

        }

        private bool isValid()
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Enter a valid Username", "Error");
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Enter a valid Password", "Error");
                return false;
            }

            return true;
        }

    }
}
