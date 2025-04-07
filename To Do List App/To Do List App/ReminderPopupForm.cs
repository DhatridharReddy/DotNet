using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace To_Do_List_App
{
    public partial class ReminderPopupForm : Form
    {
        public ReminderPopupForm(string title, string description, string imagePath)
        {
            InitializeComponent();
            labelTitle.Text = title;
            labelDescription.Text = description;

            // Load image if path is valid
            labelTitle.Text = title;
            labelDescription.Text = description;

            // Load image if path is valid
            if (!string.IsNullOrEmpty(imagePath))
            {
                pictureBoxImage.Image = System.Drawing.Image.FromFile(imagePath);
                pictureBoxImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
    }

     
}
