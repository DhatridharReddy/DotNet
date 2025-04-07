using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Include MySQL library

namespace To_Do_List_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        DataTable todoList = new DataTable();
        bool isEditing = false;

        // Database connection string
        string connString = "Server=localhost; Database=vp; Uid=root; Pwd=Siddhardha@1;";

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create columns
            todoList.Columns.Add("Title");
            todoList.Columns.Add("Description");

            // Point our datagridview to our datasource
            dataGridView1.DataSource = todoList;

            LoadDataFromDatabase(); // Load data from the database when form loads
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isEditing)
            {
                todoList.Rows[dataGridView1.CurrentCell.RowIndex]["Title"] = textBox1.Text;
                todoList.Rows[dataGridView1.CurrentCell.RowIndex]["Description"] = textBox2.Text;

                UpdateToDatabase(dataGridView1.CurrentCell.RowIndex); // Update record in the database
            }
            else
            {
                todoList.Rows.Add(textBox1.Text, textBox2.Text);

                // Save to the database
                InsertToDatabase(textBox1.Text, textBox2.Text);
            }

            // Clear fields
            textBox1.Text = "";
            textBox2.Text = "";
            isEditing = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isEditing = true;
            // Fill text fields with data from table
            textBox1.Text = todoList.Rows[dataGridView1.CurrentCell.RowIndex].ItemArray[0].ToString();
            textBox2.Text = todoList.Rows[dataGridView1.CurrentCell.RowIndex].ItemArray[1].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Remove from DataTable
                todoList.Rows[dataGridView1.CurrentCell.RowIndex].Delete();

                // Delete from the database
                DeleteFromDatabase(dataGridView1.CurrentCell.RowIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        // Method to load data from the database into the DataGridView
        private void LoadDataFromDatabase()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT Title, Description FROM todolist";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    todoList.Rows.Add(reader["Title"].ToString(), reader["Description"].ToString());
                }
            }
        }

        // Method to insert new row into the database
        private void InsertToDatabase(string title, string description)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string query = "INSERT INTO ToDoList (Title, Description) VALUES (@Title, @Description)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.ExecuteNonQuery();
            }
        }

        // Method to update an existing row in the database
        private void UpdateToDatabase(int rowIndex)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string title = todoList.Rows[rowIndex]["Title"].ToString();
                string description = todoList.Rows[rowIndex]["Description"].ToString();

                string query = "UPDATE ToDoList SET Title = @Title, Description = @Description WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@ID", rowIndex + 1); // Assuming rowIndex corresponds to ID
                cmd.ExecuteNonQuery();
            }
        }

        // Method to delete a row from the database
        private void DeleteFromDatabase(int rowIndex)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string query = "DELETE FROM ToDoList WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", rowIndex + 1); // Assuming rowIndex corresponds to ID
                cmd.ExecuteNonQuery();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
