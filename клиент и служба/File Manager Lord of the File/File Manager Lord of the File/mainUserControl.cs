using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Text;
using System.Net.Sockets;
using System.Linq;

namespace File_Manager_Lord_of_the_File
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s">Массив выбранных элементов.</param>
    /// <param name="n">Источник события.</param>
    public delegate void Insetr(List<string> s, string n);

    /// <summary>
    /// Содержит методы для загрузки файлов и каталогов на форму.
    /// </summary>
    public partial class mainUserControl : UserControl
    {
        public mainUserControl()
        {
            InitializeComponent();
        }

        #region peremen

        /// <summary>
        /// Хранит данные для отправки на сервер.
        /// </summary>
        private string _textToSend;


        //хранит содержимое выбранной ячейки во второй калонке
        public string value_dgv { get; set; }
        //хранит индекс выбранной ячейки
        int _ind = 0;
        //поток, для загрузки формы
        Thread _lth;
        /// <summary>
        /// Возникает при загрузке файлов или каталогов на форму.
        /// </summary>
        public event EventHandler progressBar;
        /// <summary>
        /// Возникает при одноразово нажатии на ячейку datagridview.
        /// </summary>
        public event EventHandler loedMUC2;
        /// <summary>
        /// Возникае при выборе команды createFile.
        /// </summary>
        public event EventHandler createFile;
        /// <summary>
        /// Возникае при выборе команды deleteFile.
        /// </summary>
        public event EventHandler deleteFile;
        /// <summary>
        /// Возникае при выборе команды updateForm.
        /// </summary>
        public event EventHandler updateForm;
        /// <summary>
        /// Возникае при изменении/смене имени текущего открытого каталога на форме.
        /// </summary>
        public event EventHandler chanName;
        /// <summary>
        /// Возникает при вызове команды copy/move.
        /// </summary>
        public event Insetr insertFile;
        /// <summary>
        /// Хранит источник события(move или copy).
        /// </summary>
        string sen = null;

        /// <summary>
        /// Хранит пути выбранных каталогов и файлов.
        /// </summary>
        List<string> rowNames = new List<string>();

        #endregion

        #region Load

        private void mainUserControl_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Подключается к серверу.
        /// </summary>
        internal void conectServer()
        {
            try
            {

                string _address = "127.0.0.1";
                int _port = 12345;

                _client = new TcpClient();
                _receiveThread = new Thread(_receiveBody);
                _receiveThread.Start();

                try
                {
                    _client.Connect(_address, _port);
                    if (getClient != null)
                        getClient(_dataOfSerevr, new EventArgs());
                }
                catch (Exception _e)
                {
                    MessageBox.Show("Идет подключение к серверу. Пожалуйста, подождите.");
                    Thread.Sleep(5000);
                }
            }
            catch { }
        }



        /// <summary>
        /// Загружает текущий каталог на форму.
        /// </summary>
        /// <param name="name">Полное имя загружаемого каталога.</param>
        internal void loadForm(string name, string sender)
        {

            //присваивает новое имя
            this.Text = name;
            //убирает левый столбец в таблице
            mainDataGridView.RowHeadersVisible = false;
            //задает свойство : только для чтения
            mainDataGridView.ReadOnly = true;
            //убирает лишнюю строку
            mainDataGridView.AllowUserToAddRows = false;
            //очищает содержимое таблици
            mainDataGridView.Rows.Clear();

            mainDataGridView.Columns[3].Visible = false;

            //создается поток для загрузки файлов и каталогов на форму
            if ((_lth != null) && _lth.IsAlive)
            {
                try
                {
                    _lth.IsBackground = true;
                    _lth.Start(this.Text);
                }
                catch { }
            }
            else
            {
                try
                {
                    _lth = new Thread(_loadDir);
                    _lth.IsBackground = true;
                    _lth.Start(this.Text);
                }
                catch { }
            }



            if (sender.Equals("mainUserControl2"))
            {
                mainDataGridView.Columns[0].Visible = false;
                for (int i = 3; i < contextMenuStrip1.Items.Count; i++)
                    contextMenuStrip1.Items[i].Visible = false;
                contextMenuStrip1.Items[0].Visible = false;
                contextMenuStrip1.Items[10].Visible = true;
            }


            if (chanName != null)
                chanName(name, null);
        }


        /// <summary>
        /// Поток для загрузки текущего каталога на форму.
        /// </summary>
        /// <param name="_directory"></param>
        internal void _loadDir(object _directory)
        {
            try
            {
                //хранит путь к каталогу, который будет загружен на форму
                string dir = (string)_directory;
                //получает файлы и директории текущего каталога
                _textToSend = "/set=" + "\"" + dir + "\"" + "#" + "/view";
                sendDataToServer(_textToSend);
                string[] tempArr = _dataOfSerevr.Split('\n');
                var _dirs = from d in tempArr where !(d).Contains(".") && !d.Equals("") select d;
                var _files = from d in tempArr where (d).Contains(".") && !d.Equals("") select d;


                int _current = 0;
                int _count = 1;
                //определяется количество файлов и каталогов, который будут загруженны на форму.
                if (tempArr.Length != 0)
                    _count = tempArr.Length;

                try
                {
                    if (!dir.Equals(dir.Remove(0, dir.IndexOf("\\") + 1)))
                    {
                        printToDataGrid(false, "[ ... ]", null, null, (_current++ * 100 / _count));
                    }
                    foreach (var _dir in _dirs)
                    {
                        if (!_dir.StartsWith(">"))
                            printToDataGrid(false, _dir.Remove(0, _dir.LastIndexOf("\\") + 1), "<dir>", 0, (_current++ * 100 / _count));
                    }
                    foreach (var _f in _files)
                    {
                        if (!_f.StartsWith(">"))
                            printToDataGrid(false, System.IO.Path.GetFileNameWithoutExtension(_f), System.IO.Path.GetExtension(_f), 0, (_current++ * 100 / _count));
                    }
                }
                catch (Exception _exc)
                {
                    MessageBox.Show(_exc.Message);
                }
            }
            catch (Exception _exc)
            {
                MessageBox.Show(_exc.Message);
            }
            try
            {
                // назначает сроку с данным индексом выделенной
                mainDataGridView.Rows[_ind].Selected = true;
            }
            catch { }

            if ((createFile != null) && (deleteFile != null) && (updateForm != null))
            {
                EventArgs e = null;
                createFile(_directory, e);
            }

        }

        /// <summary>
        /// Для метода printToDataGrid.
        /// </summary>
        /// <param name="obj">Состояние первой колонки.</param>
        /// <param name="p1">Имя каталога или файл.а</param>
        /// <param name="p2">Расширение каталога или файла.</param>
        /// <param name="p3">Дата создания каталога или файла.</param>
        /// <param name="p4">Коеффициент загрузки для prograssbar.</param>
        private delegate void printDelegate(bool obj, object p1, object p2, object p3, object p4);

        /// <summary>
        /// Выводит на форму содержимое текущего каталога.
        /// </summary>
        /// <param name="obj">Состояние первой колонки.</param>
        /// <param name="p1">Имя каталога или файл.а</param>
        /// <param name="p2">Расширение каталога или файла.</param>
        /// <param name="p3">Дата создания каталога или файла.</param>
        /// <param name="p4">Коеффициент загрузки для prograssbar.</param>
        private void printToDataGrid(bool obj, object percent, object name, object ext, object count)
        {
            //если ничего не изменяется то обращается снова в этот метод
            if (mainDataGridView.InvokeRequired)
            {
                mainDataGridView.Invoke((printDelegate)printToDataGrid, obj, percent, name, ext, count);
                return;
            }

            Color col;
            try
            {
                if (percent.Equals("[ ... ]"))
                {
                    col = Color.BurlyWood;
                }
                else if (name.Equals("<dir>"))
                {
                    col = Color.LemonChiffon;
                }
                else if (name.Equals(".txt"))
                {
                    col = Color.LightCyan;
                }
                else
                {
                    col = Color.Linen;
                }

                if (progressBar != null)
                {
                    EventArgs e = null;
                    progressBar(count, e);
                }
                mainDataGridView.Rows[mainDataGridView.Rows.Add(obj, percent, name, ext)].DefaultCellStyle.BackColor = col;
            }
            catch { }
        }

        /// <summary>
        /// Возникает при двойном нажатии по строке.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!value_dgv.Equals("[ ... ]"))
                {
                    _ind = mainDataGridView.CurrentRow.Index;
                }

                value_dgv = mainDataGridView[1, mainDataGridView.CurrentRow.Index].Value.ToString();

                if (value_dgv.Equals("[ ... ]"))
                {
                    string temp = this.Text.Substring(0, this.Text.LastIndexOf("\\"));
                    if (temp.Contains("\\"))
                    loadForm(this.Text.Substring(0, this.Text.LastIndexOf("\\")), this.Text);
                    else loadForm(this.Text.Substring(0, this.Text.LastIndexOf("\\")+1), this.Text);
                }
                else if (!value_dgv.Equals("[ ... ]") && mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString().Equals("<dir>"))
                {
                    loadForm(this.Text + value_dgv + "\\", this.Text);
                }
                else if (!value_dgv.Equals("[ ... ]") && !mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString().Equals("<dir>"))
                {
                    _textToSend = "/set=" + "\"" + this.Text + value_dgv + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString() + "\"" + "#" + "/view";
                    sendDataToServer(_textToSend);
                    string tname = System.IO.Path.GetRandomFileName().Replace(".", "");
                    // System.IO.File.Create("C:\\Windows\\temp\\" + tname + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString());
                    System.IO.File.WriteAllText(("C:\\Windows\\temp\\" + tname + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString()), _dataOfSerevr);
                    System.Diagnostics.Process.Start("C:\\Windows\\temp\\" + tname + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString());
                    MessageBox.Show("Данные текущего файла предназначенны только для просмотра! Любое изменение содержимого данного файла не приведет к изменениям его на сервере!");
                }
            }
            catch { }
        }

        #endregion

        #region ContextMenu

        /// <summary>
        /// Возникает при вызове команды delete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fnames = from d in rowNames where d.Contains(".") select d;
            var dnames = from d in rowNames where !d.Contains(".") select d;

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete '" + rowNames.Count + "' files ?", "Some Title", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {

                try
                {
                    foreach (var d in dnames)
                    {
                        _textToSend = "/set=\"" + d + "\"#" + "/delete";
                        sendDataToServer(_textToSend);
                        mainDataGridView.Rows.RemoveAt(mainDataGridView.CurrentRow.Index);
                    }
                    foreach (var f in fnames)
                    {
                        _textToSend = "/set=\"" + f + "\"#" + "/delete";
                        sendDataToServer(_textToSend);
                        mainDataGridView.Rows.RemoveAt(mainDataGridView.CurrentRow.Index);
                    }
                }
                catch { }
                MessageBox.Show("Delete " + rowNames.Count + " objects.");

                rowNames.Clear();
                if (deleteFile != null)
                    deleteFile(rowNames, e);

            }
            else if (dialogResult == DialogResult.No)
            {

            }

        }

        /// <summary>
        /// Возникает при вызове команды rename.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainDataGridView.ReadOnly = false;
        }

        /// <summary>
        /// Возникает при редактировании текущей ячейки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //запоминает новое имя ячейки
            string val = mainDataGridView[1, mainDataGridView.CurrentRow.Index].Value.ToString();
            try
            {
                if (!val.Equals(value_dgv) && !mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString().Equals("<dir>"))
                {
                    _textToSend = "/set=\"" + this.Text + "\\" + value_dgv + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString() + "\"#" + "/moveto=" + this.Text + "\\" + val + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString();
                    sendDataToServer(_textToSend);
                }
                else if (!val.Equals(value_dgv) && mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString().Equals("<dir>"))
                {
                    _textToSend = "/set=\"" + this.Text + "\\" + value_dgv + "\"#" + "/moveto=" + this.Text + "\\" + val;
                    sendDataToServer(_textToSend);
                }
            }
            catch { }

            mainDataGridView.ReadOnly = true;
        }



        /// <summary>
        /// Возникает при выборе команды copy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sen = sender.ToString();
            if (insertFile != null)
                insertFile(rowNames, sen);
        }

        /// <summary>
        /// Получает источник события и массив выбранных путей.
        /// </summary>
        /// <param name="s">Массив данных.</param>
        /// <param name="n">Источник события.</param>
        internal void tempInfo(List<string> s, string n)
        {
            sen = n;
            rowNames = s;
            contextMenuStrip1.Items[10].Visible = true;
        }

        /// <summary>
        /// Возникает при вызое команды insert.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var fnames = from d in rowNames where d.Contains(".") select d;
            var dnames = from d in rowNames where !d.Contains(".") select d;

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to " + sen.ToLower() + " '" + rowNames.Count + "' files ?", "Some Title", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    if ((sen != null) && sen.Equals("Move"))
                    {
                        foreach (var d in dnames)
                        {
                            _textToSend = "/set=\"" + d + "\"#" + "/moveto=" + this.Text + "\\" + new System.IO.DirectoryInfo(d).Name;
                            sendDataToServer(_textToSend);
                        }
                        foreach (var f in fnames)
                        {
                            _textToSend = "/set=\"" + f + "\"#" + "/moveto=" + this.Text + "\\" + new System.IO.FileInfo(f).Name;
                            sendDataToServer(_textToSend);
                        }
                    }
                    else if ((sen != null) && sen.Equals("Copy"))
                    {
                        foreach (var d in dnames)
                        {
                            _textToSend = "/set=\"" + d + "\"#" + "/copyto=" + this.Text + "\\" + new System.IO.DirectoryInfo(d).Name;
                            sendDataToServer(_textToSend);
                        }
                        foreach (var f in fnames)
                        {
                            _textToSend = "/set=\"" + f + "\"#" + "/copyto=" + this.Text + "\\" + new System.IO.FileInfo(f).Name;
                            sendDataToServer(_textToSend);
                        }
                    }
                }
                catch { }
                contextMenuStrip1.Items[10].Visible = false;
                loadForm(this.Text, this.Text);

                rowNames.Clear();
                if (deleteFile != null)
                    deleteFile(rowNames, e);
            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }

        /// <summary>
        /// Возникает при вызове команды arhiving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void arhivindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var fnames = from d in rowNames where d.Contains(".") select d;
                var dnames = from d in rowNames where !d.Contains(".") select d;

                foreach (var d in dnames)
                {
                    _textToSend = "/set=\"" + d + "\"#" + "/arhiving";
                    sendDataToServer(_textToSend);
                    loadForm(this.Text, this.Text);
                }
                foreach (var f in fnames)
                {
                    _textToSend = "/set=\"" + f + "\"#" + "/arhiving";
                    sendDataToServer(_textToSend);
                    loadForm(this.Text, this.Text);
                }
            }
            catch { }
            MessageBox.Show("Archiving " + rowNames.Count + " objects.");
            rowNames.Clear();
            if (deleteFile != null)
                deleteFile(rowNames, e);
        }

        /// <summary>
        /// Возникает при вызове команды desarhiving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void desarhivingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var fnames = from d in rowNames where d.Contains(".") select d;
                var dnames = from d in rowNames where !d.Contains(".") select d;

                foreach (var d in dnames)
                {
                    _textToSend = "/set=\"" + d + "\"#" + "/desarhiving";
                    sendDataToServer(_textToSend);
                    loadForm(this.Text, this.Text);
                }
                foreach (var f in fnames)
                {
                    _textToSend = "/set=\"" + f + "\"#" + "/desarhiving";
                    sendDataToServer(_textToSend);
                    loadForm(this.Text, this.Text);
                }
            }
            catch { }
            MessageBox.Show("Desarhiving " + rowNames.Count + " objects.");
            rowNames.Clear();
            if (deleteFile != null)
                deleteFile(rowNames, e);
        }

        /// <summary>
        /// Возникает при вызове команды uncoding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var fnames = from d in rowNames where d.Contains(".") select d;
                var dnames = from d in rowNames where !d.Contains(".") select d;

                foreach (var d in dnames)
                {
                    _textToSend = "/set=\"" + d + "\"#" + "/uncod";
                    sendDataToServer(_textToSend);
                    loadForm(this.Text, this.Text);
                }
                foreach (var f in fnames)
                {
                    _textToSend = "/set=\"" + f + "\"#" + "/uncod";
                    sendDataToServer(_textToSend);
                    loadForm(this.Text, this.Text);
                }
            }
            catch { }
            MessageBox.Show("Decoding " + rowNames.Count + " objects.");
            rowNames.Clear();
            if (deleteFile != null)
                deleteFile(rowNames, e);
        }

        /// <summary>
        /// Возникает при вызове команды decoding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uncodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var fnames = from d in rowNames where d.Contains(".") select d;
                var dnames = from d in rowNames where !d.Contains(".") select d;

                foreach (var d in dnames)
                {
                    _textToSend = "/set=\"" + d + "\"#" + "/encod";
                    sendDataToServer(_textToSend);
                    loadForm(this.Text, this.Text);
                }
                foreach (var f in fnames)
                {
                    _textToSend = "/set=\"" + f + "\"#" + "/encod";
                    sendDataToServer(_textToSend);
                    loadForm(this.Text, this.Text);
                }
            }
            catch { }
            MessageBox.Show("Encoding " + rowNames.Count + " objects.");
            rowNames.Clear();
            if (deleteFile != null)
                deleteFile(rowNames, e);
        }

        /// <summary>
        /// Создает новую папку с рандомным именем.
        /// </summary>
        public void create(object sender, string n)
        {

            string name = System.IO.Path.GetRandomFileName();
            if (sender.ToString().Equals("button_Create"))
            {
                _textToSend = "/set=\"" + n + "\\" + name.Replace(".", "") + "\"#" + "/create";
                sendDataToServer(_textToSend);
                printToDataGrid(false, name.Replace(".", ""), "<dir>", 0, 0);
            }
            else
            {
                _textToSend = "/set=\"" + n + "\\" + name.Replace(".", "") + ".txt" + "\"#" + "/create";
                sendDataToServer(_textToSend);
                printToDataGrid(false, name.Replace(".", ""), ".txt", 0, 0);
            }
            MessageBox.Show(_dataOfSerevr);
            // назначает выделенную строку
            try
            {
                mainDataGridView.Rows[mainDataGridView.Rows.Count - 1].Selected = true;
            }
            catch { }


        }

        /// <summary>
        /// Возникает при вызове команды на contextMenu create file или create directory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender.ToString().Equals("Create directory"))
            {
                create((object)"button_Create", this.Text);
            }
            else
            {
                create((object)"Create file", this.Text);
            }
        }
        #endregion


        /// <summary>
        /// Получает значение ячейки при одноразовом нажатии на нее.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void mainDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (sender.ToString().Equals("CancelButton"))
            {
                rowNames.Clear();
                for (int i = 0; i < mainDataGridView.Rows.Count; i++)
                    mainDataGridView[0, i].Value = false;

            }
            else if (sender.ToString().Equals("selectAll"))
            {
                for (int i = 0; i < mainDataGridView.Rows.Count; i++)
                {
                    mainDataGridView[0, i].Value = true;
                    rowNames.Add((this.Text + "\\" + mainDataGridView[1, i].Value.ToString() + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString()).Replace("<dir>", "").Replace("[ ... ]", ""));
                }
            }
            else if (e.ColumnIndex == 0)
            {
                try
                {
                    if ((bool)mainDataGridView[0, mainDataGridView.CurrentRow.Index].Value == true)
                    {
                        mainDataGridView[0, mainDataGridView.CurrentRow.Index].Value = false;
                        rowNames.Remove((this.Text + "\\" + mainDataGridView[1, mainDataGridView.CurrentRow.Index].Value.ToString() + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString()).Replace("<dir>", "").Replace("[ ... ]", ""));
                        if (deleteFile != null)
                            deleteFile(rowNames, e);
                    }
                    else
                    {
                        mainDataGridView[0, mainDataGridView.CurrentRow.Index].Value = true;
                        rowNames.Add((this.Text + "\\" + mainDataGridView[1, mainDataGridView.CurrentRow.Index].Value.ToString() + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString()).Replace("<dir>", "").Replace("[ ... ]", ""));
                        if (deleteFile != null)
                            deleteFile(rowNames, e);
                    }
                }
                catch { }
            }
            else
            {

                if (loedMUC2 != null)
                {
                    try
                    {
                        if (value_dgv.Contains("[ ... ]"))
                            value_dgv.Replace(("[ ... ]"), "");
                        value_dgv = (mainDataGridView[1, mainDataGridView.CurrentRow.Index].Value.ToString());
                        loedMUC2(this.Text + value_dgv, e);

                    }
                    catch { }

                }

            }


            if (rowNames.Count > 1)
            {
                contextMenuStrip1.Items[1].Visible = false;
                contextMenuStrip1.Items[2].Visible = false;
                contextMenuStrip1.Items[4].Visible = false;
            }
            else if (rowNames.Count == 1)
            {
                for (int i = 0; i < contextMenuStrip1.Items.Count - 1; i++)
                    contextMenuStrip1.Items[i].Visible = true;
            }
            else
            {
                for (int i = 0; i < contextMenuStrip1.Items.Count; i++)
                    contextMenuStrip1.Items[i].Visible = false;

                contextMenuStrip1.Items[1].Visible = true;
                contextMenuStrip1.Items[2].Visible = true;
                contextMenuStrip1.Items[4].Visible = true;
            }

            try
            {
                value_dgv = (mainDataGridView[1, mainDataGridView.CurrentRow.Index].Value.ToString());
            }
            catch { }
        }


        #region server

        /// <summary>
        /// Отправляет запросы на сервер.
        /// </summary>
        /// <param name="data">Данные, передаваемые на сервер.</param>
        private void sendDataToServer(string data)
        {
            try
            {
                _client.GetStream().Flush();
                _client.GetStream().Write(Encoding.UTF8.GetBytes(_textToSend), 0, _textToSend.Length);
                Thread.Sleep(1500);
            }
            catch (Exception _exc)
            {
                MessageBox.Show(_exc.Message);
                // В большинстве случаев причина ошибки - отвалился канал передачи данных или рухнул сервер
                // Требуется повторное переподключение
            }
        }



        /// <summary>
        /// Содержит имя текущего клиента.
        /// </summary>
        static TcpClient _client;
        /// <summary>
        /// Поток для получения данных с сервера.
        /// </summary>
        private static Thread _receiveThread;

        /// <summary>
        /// Возникает при подключении клиента к серверу.
        /// </summary>
        public event EventHandler getClient;

        private string _dataOfSerevr;

        /// <summary>
        /// Получение данных с сервера текущим пользователем.
        /// </summary>
        private void _receiveBody()
        {

            byte[] _buf = new byte[8192 * 4];
            int _offset = 0;
            int _available = 0;
            string _data;
            while (true)
            {
                try
                {
                    _available = _client.Available;
                    if (_available > 0)
                    {
                        _available = _client.GetStream().Read(_buf, _offset, _available);
                        _data = Encoding.UTF8.GetString(_buf, _offset, _available);
                        if ((!_data.Equals("\0")))
                        {
                            _dataOfSerevr = _data;
                            _client.GetStream().Flush();
                        }
                    }
                }
                catch (Exception exc)
                {

                }
                Thread.Sleep(1);
            }
        }
        #endregion
    }
}
