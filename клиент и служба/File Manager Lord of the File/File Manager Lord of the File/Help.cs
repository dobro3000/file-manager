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

namespace File_Manager_Lord_of_the_File
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
        }

        private void Help_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("help.txt");
            string line;
            label1.Text = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                label1.Text = label1.Text + line + "\n";
            }
        }
    }
}
