using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
namespace File_Manager_Lord_of_the_File
{
    /// <summary>
    /// Содержит методы для работы с формой.
    /// </summary>
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// хранинит путь для кнопки обновления
        /// </summary>
        string tempNameDisk;

        /// <summary>
        /// Получает список дисков и создает для каждого диска соответсвущую кнопку.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainForm_Load(object sender, EventArgs e)
        {

            try
            {
                mainUserControl1.getClient += MainUserControl1_getClient;
                mainUserControl1.conectServer();
                mainUserControl2.conectServer();

                if (disk == null)
                    mainUserControl1.conectServer();
                string[] disks = disk.Split('#');


                for (int i = 0; i < disks.Length - 1; i++)
                {
                    ToolStripButton _btn = new ToolStripButton();
                    mainToolStrip.Items.Add(disks[i]).Click += _btn_Click; ;

                    ToolStripSeparator _spr = new ToolStripSeparator();
                    mainToolStrip.Items.Add(_spr);

                    Button_Disk.DropDownItems.Add(disks[i]).Click += MainUserControl2_loedMUC2; ;
                }

            }
            catch { }



            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Maximum = 100;

            //подпика на событие для полосы загрузки
            mainUserControl1.progressBar += _muc_myEvent;
            mainUserControl2.progressBar += _muc_myEvent;

            //подпика на событие для загрузки второй формы
            mainUserControl1.loedMUC2 += MainUserControl2_loedMUC2;
            //подпика на событие при вызове команды create file.
            mainUserControl1.createFile += MainUserControl1_createFile;
            //подпика на событие вызове команды delete file.
            mainUserControl1.deleteFile += MainUserControl1_deleteFile;
            //подпика на событие при вызове команды update.
            mainUserControl1.updateForm += MainUserControl1_updateForm;
            //подпика на событие при изменении имени текущего открытого каталога.
            mainUserControl1.chanName += MainUserControl1_chanName;
            //подписка на событие при вызове команды insert.
            mainUserControl1.insertFile += MainUserControl1_insertFile;
            mainUserControl2.insertFile += MainUserControl1_insertFile;
        }

        /// <summary>
        /// Хранит имена дисков.
        /// </summary>
        private string disk;

        /// <summary>
        /// Получает имена дисков с сервера.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainUserControl1_getClient(object sender, EventArgs e)
        {
            disk = sender.ToString();
        }

        /// <summary>
        /// Возникает при вызове команды insert.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="n"></param>
        private void MainUserControl1_insertFile(List<string> s, string n)
        {
            mainUserControl2.tempInfo(s, n);
            mainUserControl1.tempInfo(s, n);
        }


        /// <summary>
        /// Возникает при изменении текущего открытого каталога на форме.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainUserControl1_chanName(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = sender.ToString();
            }
            catch { }
        }

        #region event

        /// <summary>
        /// Возникает при вызове команды update.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainUserControl1_updateForm(object sender, EventArgs e)
        {
            tempNameDisk = sender.ToString();
            str.Clear();
        }

        /// <summary>
        /// Хранит список файлов, которые помеченны галочкой.
        /// </summary>
        List<string> str = new List<string>();
        private void MainUserControl1_deleteFile(object sender, EventArgs e)
        {
            str = (List<string>)sender;
            if (str.Count != 0)
            {
                toolStripButton1.Visible = true;
                toolStripselect.Visible = true;
            }
            else if (str.Count == 0)
            {
                toolStripButton1.Visible = false;
                toolStripselect.Visible = false;
            }
        }

        /// <summary>
        /// Возникает при вызове команды create file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainUserControl1_createFile(object sender, EventArgs e)
        {
            tempNameDisk = sender.ToString();
        }
        #endregion

        /// <summary>
        /// Заружает вторую форму loedMUC2.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainUserControl2_loedMUC2(object sender, EventArgs e)
        {
            mainUserControl2.loadForm(sender.ToString(), "mainUserControl2");
        }

        /// <summary>
        /// Возникает, при нажатии на кнопку на меню mainToolStrip.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _btn_Click(object sender, EventArgs e)
        {
            mainUserControl1.loadForm(sender.ToString(), "mainUserControl1");
        }

        #region ToolStripMenu

        /// <summary>
        /// Возникает при нажатии на кнопку Exit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Возникает при нажати на кнопку About.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About _a = new About();
            _a.ShowDialog();
        }

        /// <summary>
        /// Возникает при нажатии на кнопку Help.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void heloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help _hf = new Help();
            _hf.ShowDialog();
        }


        /// <summary>
        /// Возникает при нажатии на кнопку Open.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            openFileDialog1.Multiselect = false;
            openFileDialog1.InitialDirectory = tempNameDisk;
            openFileDialog1.FileName = tempNameDisk;
        }


        #endregion

        #region menuStoop

        /// <summary>
        /// Возникает при вызове команды create file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCreate_file_Click(object sender, EventArgs e)
        {
            mainUserControl1.create(sender, tempNameDisk);
        }

        /// <summary>
        /// Возникает при нажатии кнопки button_Create.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Create_Click(object sender, EventArgs e)
        {
            mainUserControl1.create(sender, tempNameDisk);
        }

        /// <summary>
        /// Возникает при нажатии на кнопку Button_Delete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Delete_Click(object sender, EventArgs e)
        {
            if (str.Count != 0)
            {
                try
                {
                    foreach (string s in str)
                    {
                        mainUserControl1.deleteToolStripMenuItem_Click(s, e);
                    }
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Select the files.");
            }
        }

        /// <summary>
        /// Возникает при нажатии на кнопку Button_update.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_update_Click(object sender, EventArgs e)
        {
            mainUserControl1.loadForm(tempNameDisk, "mainUserControl1");
            toolStripButton1.Visible = false;
            toolStripselect.Visible = false;
            toolStripButton1_Click(sender, e);
        }

        #endregion

        /// <summary>
        /// Загружает progressBar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _muc_myEvent(object sender, EventArgs e)
        {
            if (toolStripProgressBar1.Value != (int)sender)
                toolStripProgressBar1.Value = (int)sender;
            if (toolStripProgressBar1.Value > 90)
                toolStripProgressBar1.Value = 0;
        }

        /// <summary>
        /// возникает когда идет запись в поиске
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (System.IO.Directory.Exists(sender.ToString()))
                mainUserControl1.loadForm(sender.ToString(), "mainUserControl1");
        }

        /// <summary>
        /// возникает при вызове команды сброс выделенных файлов.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            str.Clear();
            mainUserControl1.mainDataGridView_CellContentClick("CancelButton", null);
            toolStripButton1.Visible = false;
            toolStripselect.Visible = false;
        }

        /// <summary>
        /// Возникает при вызове команды отметить все файлы галочкой.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripselect_Click(object sender, EventArgs e)
        {
            mainUserControl1.mainDataGridView_CellContentClick("selectAll", null);
            toolStripButton1.Visible = true;
        }
    }
}
