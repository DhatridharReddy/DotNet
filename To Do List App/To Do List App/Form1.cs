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
using System.Media;
using static System.Net.Mime.MediaTypeNames;
namespace To_Do_List_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();



        }





        DataTable todoList = new DataTable();
        bool isEditing = false;
        string selectedImagePath = "";
        string selectedAudioPath = "";

        // Database connection string
        string connString = "Server=localhost; Database=vp; Uid=root; Pwd=Siddhardha@1;";
        Timer taskTimer = new Timer();

        private void Form1_Load(object sender, EventArgs e)
        {
            taskTimer.Interval = 3000; // Check every minute
            taskTimer.Tick += new EventHandler(CheckScheduledTasks);
            taskTimer.Start();
            todoList.Columns.Add("ID");
            // Create columns
            todoList.Columns.Add("Title");
            todoList.Columns.Add("Description");
            todoList.Columns.Add("ScheduledTime");
            todoList.Columns.Add("ReminderTriggered", typeof(bool)); // New column for reminder status
            todoList.Columns.Add("ImagePath"); // Add ImagePath column
            todoList.Columns.Add("AudioPath");
            // Point our datagridview to our datasource
            dataGridView1.DataSource = todoList;
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["ImagePath"].Visible = false; // Hide ImagePath
            dataGridView1.Columns["AudioPath"].Visible = false; // Hide AudioPath

            LoadDataFromDatabase(); // Load data from the database when form loads
        }
        private void CheckScheduledTasks(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            foreach (DataRow row in todoList.Rows)
            {
                if (row["ScheduledTime"] != DBNull.Value) // Only check rows with a valid ScheduledTime
                {
                    DateTime scheduledTime = Convert.ToDateTime(row["ScheduledTime"]);
                    if (scheduledTime <= now && !Convert.ToBoolean(row["ReminderTriggered"])) // Check if reminder has not already been shown
                    {
                        // Popup message if the time is reached or passed
                        MessageBox.Show($"Reminder for task: {row["Title"]}\nDescription: {row["Description"]}", "Task Reminder");
                        if (row["ImagePath"] != DBNull.Value)
                        {
                            string title = row["Title"].ToString();
                            string description = row["Description"].ToString();
                            string imagePath = row["ImagePath"] != DBNull.Value ? row["ImagePath"].ToString() : null;

                            // Show custom popup form with image, title, and description
                            ReminderPopupForm reminderPopup = new ReminderPopupForm(title, description, imagePath);
                            reminderPopup.ShowDialog();// Show image in PictureBox
                        }

                        // Play the audio if available
                        if (row["AudioPath"] != DBNull.Value)
                        {
                            string audioPath = row["AudioPath"].ToString();
                            SoundPlayer player = new SoundPlayer(audioPath);
                            player.Play(); // Play the audio
                        }
                        // Mark this reminder as triggered, so it won't show again
                        row["ReminderTriggered"] = true;

                        UpdateReminderTriggered(Convert.ToInt32(row["ID"]), true);
                    }
                }
            }
        }
        private void UpdateReminderTriggered(int id, bool triggered)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string query = "UPDATE ToDoList SET ReminderTriggered = @ReminderTriggered WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ReminderTriggered", triggered);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
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
                InsertToDatabase(textBox1.Text, textBox2.Text, null); // Insert record with scheduled time

                // Reload the data from the database to ensure everything is up to date
                LoadDataFromDatabase();

            }

            // Rebind the DataGridView to reflect changes
            dataGridView1.DataSource = todoList;

            textBox1.Text = "";
            textBox2.Text = "";
            dateTimePicker1.Value = DateTime.Now; // Reset DateTimePicker

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
            textBox1.Text = todoList.Rows[dataGridView1.CurrentCell.RowIndex].ItemArray[1].ToString();
            textBox2.Text = todoList.Rows[dataGridView1.CurrentCell.RowIndex].ItemArray[2].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure a row is selected
                if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.RowIndex >= 0)
                {
                    // Get the ID of the row to be deleted before removing it from the DataTable
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    int id = Convert.ToInt32(todoList.Rows[rowIndex]["ID"]);

                    // Delete from the database first
                    DeleteFromDatabase(id);

                    // Remove the row from the DataTable
                    todoList.Rows[rowIndex].Delete();

                    // Refresh the DataGridView to reflect the deletion
                    dataGridView1.DataSource = null;  // Clear the binding
                    dataGridView1.DataSource = todoList;  // Rebind the updated DataTable


                }
                else
                {
                    MessageBox.Show("Please select a row to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Method to load data from the database into the DataGridView
        private void LoadDataFromDatabase()
        {
            todoList.Clear();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT ID, Title, Description, ScheduledTime, ReminderTriggered, ImagePath, AudioPath FROM ToDoList";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    todoList.Rows.Add(
  reader["ID"],
  reader["Title"],
  reader["Description"],
  reader["ScheduledTime"],
  reader["ReminderTriggered"],
  reader["ImagePath"],      // Add ImagePath
  reader["AudioPath"]);
                }
            }
        }

        // Method to insert new row into the database
        private void InsertToDatabase(string title, string description, DateTime? scheduledTime)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string query = "INSERT INTO ToDoList (Title, Description, ScheduledTime, ImagePath, AudioPath) VALUES (@Title, @Description, @ScheduledTime, @ImagePath, @AudioPath)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Description", description);
                if (scheduledTime.HasValue)
                    cmd.Parameters.AddWithValue("@ScheduledTime", scheduledTime);
                else
                    cmd.Parameters.AddWithValue("@ScheduledTime", DBNull.Value);  // Insert NULL if no scheduled time
                cmd.Parameters.AddWithValue("@ImagePath", !string.IsNullOrEmpty(selectedImagePath) ? (object)selectedImagePath : DBNull.Value);
                cmd.Parameters.AddWithValue("@AudioPath", string.IsNullOrEmpty(selectedAudioPath) ? DBNull.Value : (object)selectedAudioPath);

                cmd.ExecuteNonQuery();
            }
        }

        // Method to update an existing row in the database
        private void UpdateToDatabase(int rowIndex)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                int id = Convert.ToInt32(todoList.Rows[rowIndex]["ID"]);
                string title = todoList.Rows[rowIndex]["Title"].ToString();
                string description = todoList.Rows[rowIndex]["Description"].ToString();
                DateTime? scheduledTime = Convert.ToDateTime(todoList.Rows[rowIndex]["ScheduledTime"]);

                string query = "UPDATE ToDoList SET Title = @Title, Description = @Description, ScheduledTime = @ScheduledTime WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Description", description);
                if (scheduledTime.HasValue)
                    cmd.Parameters.AddWithValue("@ScheduledTime", scheduledTime);
                else
                    cmd.Parameters.AddWithValue("@ScheduledTime", DBNull.Value);  // Update with NULL if no scheduled time


                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
        }

        // Method to delete a row from the database
        private void DeleteFromDatabase(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();

                string query = "DELETE FROM ToDoList WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonSchedule_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.RowIndex >= 0)
            {
                // Get the index of the selected row
                int rowIndex = dataGridView1.CurrentCell.RowIndex;

                // Check if the DateTimePicker has a valid selected time
                if (dateTimePicker1.Value.TimeOfDay != TimeSpan.Zero)
                {
                    // Schedule time selected by the user
                    DateTime scheduledTime = dateTimePicker1.Value;

                    // Update the scheduled time in the DataTable
                    todoList.Rows[rowIndex]["ScheduledTime"] = scheduledTime;

                    // Update the scheduled time in the database
                    UpdateScheduledTimeInDatabase(rowIndex, scheduledTime);

                    // Refresh the DataGridView to reflect changes
                    dataGridView1.DataSource = null; // Clear the binding
                    dataGridView1.DataSource = todoList; // Rebind the updated DataTable

                    MessageBox.Show("Task scheduled successfully.");
                }
                else
                {
                    MessageBox.Show("Please select a valid date and time to schedule the task.");
                }
            }
            else
            {
                MessageBox.Show("Please select a task to schedule.");
            }
        }
        private void UpdateScheduledTimeInDatabase(int rowIndex, DateTime scheduledTime)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                int id = Convert.ToInt32(todoList.Rows[rowIndex]["ID"]);

                string query = "UPDATE ToDoList SET ScheduledTime = @ScheduledTime WHERE ID = @ID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ScheduledTime", scheduledTime);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void buttonSelectImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedImagePath = openFileDialog1.FileName;

                // Use System.Drawing.Image to avoid ambiguity
                pictureBoxPreview.Image = System.Drawing.Image.FromFile(selectedImagePath);

                pictureBoxPreview.SizeMode = PictureBoxSizeMode.StretchImage; // Adjusts image to fit PictureBox size
            }
        }

        private void buttonSelectAudio_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "Audio Files|*.wav;*.mp3";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                selectedAudioPath = openFileDialog2.FileName;
            }
        }
    }
}
