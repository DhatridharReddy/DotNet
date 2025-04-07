using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
        private bool isValid()
        {
            string username = textBox3.Text.Trim();
            string password = textBox2.Text.Trim();
            string confirmPassword = textBox1.Text.Trim();

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

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match!", "Error");
                return false;
            }

            return true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                string connString = "Server=localhost; Database=vp; Uid=root; Pwd=Siddhardha@1;";
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    try
                    {
                        conn.Open();

                        // Check if the username already exists
                        string checkQuery = "SELECT COUNT(*) FROM LOGIN WHERE username = @username";
                        MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@username", textBox1.Text.Trim());

                        int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (userExists > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose a different username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // Stop the registration process
                        }

                        // Proceed with the registration if username is unique
                        string query = "INSERT INTO LOGIN (username, password) VALUES (@username, @password)";
                        MySqlCommand cmd = new MySqlCommand(query, conn);

                        cmd.Parameters.AddWithValue("@username", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", textBox2.Text.Trim());

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            Form2 form2 = new Form2();
                            form2.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed.", "Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();

        }
    }
}
