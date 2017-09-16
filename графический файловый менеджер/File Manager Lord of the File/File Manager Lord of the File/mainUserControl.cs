using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using FilesDirs;

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
                string[] _dirs = System.IO.Directory.GetDirectories(dir);
                string[] _files = System.IO.Directory.GetFiles(dir);

                int _current = 0;
                int _count = 1;
                //определяется количество файлов и каталогов, который будут загруженны на форму.
                if (_dirs.Length + _files.Length != 0)
                     _count = _dirs.Length + _files.Length;
                
                try
                {
                    if (!dir.Equals(System.IO.Directory.GetDirectoryRoot(this.Text)))
                    {
                        printToDataGrid(false, "[ ... ]", null, null, (_current++ * 100 / _count));
                    }
                    foreach (string _dir in _dirs)
                    {
                        printToDataGrid(false, System.IO.Path.GetFileName(_dir), "<dir>", System.IO.Directory.GetCreationTime(_dir), (_current++ * 100 / _count));
                    }
                    foreach (string _f in _files)
                    {
                        File _file = new File(_f);
                        printToDataGrid(false, System.IO.Path.GetFileNameWithoutExtension(_f), System.IO.Path.GetExtension(_f), System.IO.File.GetCreationTime(_f), (_current++ * 100 / _count));
                    }
                }
                catch (Exception _exc)
                {
                    MessageBox.Show(_exc.Message);
                }
            }
            catch
            {

            }
            try
            {
                // назначает сроку с данным индексом выделенной
                mainDataGridView.Rows[_ind].Selected = true;
            }
            catch { }

            if ((createFile!=null) && (deleteFile != null) && (updateForm !=null))
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
                mainDataGridView.Invoke((printDelegate)printToDataGrid, obj, percent, name, ext, count );
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

                //изначально было так loadForm(this.Text + "\\" + value_dgv , this.Text);


                //запоминает из выбранной строки имя файл
                value_dgv = mainDataGridView[1, mainDataGridView.CurrentRow.Index].Value.ToString();
                if (System.IO.Directory.Exists(this.Text  + value_dgv ))
                    loadForm(this.Text + value_dgv + "\\", this.Text);
                else if (value_dgv.Equals("[ ... ]"))
                {
                    string _lastSel = System.IO.Path.GetFileName(this.Text);
                    loadForm(System.IO.Path.GetDirectoryName(this.Text), this.Text);
                }
                else if (System.IO.File.Exists(this.Text  + value_dgv + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString()))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(this.Text + value_dgv + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString());
                    }
                    catch { }
                    }
                else
                {

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
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete '"+ rowNames.Count + "' files ?", "Some Title", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                for (int i = 0; i < rowNames.Count; i++)
                {
                    try
                    {
                        if (System.IO.Directory.Exists(rowNames[i]))
                        {
                            Directory _dr = new Directory(rowNames[i]);
                            _dr.delete();
                            mainDataGridView.Rows.RemoveAt(mainDataGridView.CurrentRow.Index);
                        }
                        else if (System.IO.File.Exists(rowNames[i]))
                        {
                            System.IO.File.Delete(rowNames[i]);
                            mainDataGridView.Rows.RemoveAt(mainDataGridView.CurrentRow.Index);
                        }
                    }
                    catch { }
                }

                MessageBox.Show("'" + rowNames.Count + "' files is delete.");

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
                if (!val.Equals(value_dgv))
                {
                    if (System.IO.Directory.Exists(this.Text + "\\" + value_dgv))
                    {
                        
                            Directory _d = new Directory(this.Text + "\\" + value_dgv);
                            _d.moveto(this.Text + "\\" + val);
                        try
                        {
                            System.IO.Directory.Delete(this.Text + "\\" + value_dgv, true);
                        }
                        catch { }
                    }
                    else if (System.IO.File.Exists(this.Text + "\\" + value_dgv + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString()))
                    {
                        System.IO.File.Move(this.Text + "\\" + value_dgv + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString(), this.Text + "\\" + val + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString());
                        System.IO.File.Delete(this.Text + "\\" + value_dgv + mainDataGridView[2, mainDataGridView.CurrentRow.Index].Value.ToString());
                    }
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
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to " + sen.ToLower()+ " '" + rowNames.Count + "' files ?", "Some Title", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if ((sen != null) && sen.Equals("Move"))
                {
                    for (int i = 0; i < rowNames.Count; i++)
                    {
                        if (System.IO.File.Exists(rowNames[i]))
                        {
                            try
                            {
                                System.IO.File.Move(rowNames[i], this.Text + "\\" + new System.IO.FileInfo(rowNames[i]).Name);

                            }
                            catch (Exception exc) { MessageBox.Show(""+ exc); }
                        }
                        else
                        {
                            try
                            {
                                Directory _d = new Directory(rowNames[i]);
                               _d.moveto(this.Text);
                                System.IO.Directory.Delete(rowNames[i], true);
                            }
                            catch (Exception exc) { MessageBox.Show("" + exc); }
                        }
                    }
                }
                else if ((sen != null) && sen.Equals("Copy"))
                {
                    for (int i = 0; i < rowNames.Count; i++)
                    {
                        if (System.IO.File.Exists(rowNames[i]))
                        {
                            try
                            {
                                System.IO.File.Copy(rowNames[i], this.Text + "\\" + new System.IO.FileInfo(rowNames[i]).Name);
                            }
                            catch (Exception exc) { MessageBox.Show("" + exc); }
                        }
                        else
                        {
                            try
                            {
                                Directory _d = new Directory(rowNames[i]);
                                _d.copyto(this.Text);
                            }
                            catch (Exception exc) { MessageBox.Show("" + exc); }
                        }
                    }
                }
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
            for (int i = 0; i < rowNames.Count; i++)
            {
                if (System.IO.Directory.Exists(rowNames[i]))
                {
                    Directory _dr = new Directory(rowNames[i]);
                    _dr.arhiving();
                    loadForm(this.Text, this.Text);
                }
                else if (System.IO.File.Exists(rowNames[i]))
                {
                    File _dr = new File(rowNames[i]);
                    _dr.arhiving();
                    loadForm(this.Text, this.Text);

                }
            }
            MessageBox.Show("'" + rowNames.Count + "' files is arhiving.");
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
            for (int i = 0; i < rowNames.Count; i++)
            {
                if (System.IO.Directory.Exists(rowNames[i]))
                {
                    Directory _dr = new Directory(rowNames[i]);
                    _dr.desarhiving();
                    loadForm(this.Text, this.Text);
                }
                else if (System.IO.File.Exists(rowNames[i]))
                {
                    File _dr = new File(rowNames[i]);
                    _dr.desarhiving();
                    loadForm(this.Text, this.Text);
                }
            }
            MessageBox.Show("'" + rowNames.Count + "' files is desarhiving.");
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
            for (int i = 0; i < rowNames.Count; i++)
            {
                if (System.IO.Directory.Exists(rowNames[i]))
                {
                    Directory _dr = new Directory(rowNames[i]);
                    _dr.uncod();
                    loadForm(this.Text, this.Text);
                }
                else if (System.IO.File.Exists(rowNames[i]))
                {
                    File _dr = new File(rowNames[i]);
                    _dr.uncod();
                    loadForm(this.Text,this.Text);
                }
            }
            MessageBox.Show("'" + rowNames.Count + "' files is uncoding.");
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
            for (int i = 0; i < rowNames.Count; i++)
            {
                if (System.IO.Directory.Exists(rowNames[i]))
                {
                    Directory _dr = new Directory(rowNames[i]);
                    _dr.encod();
                    loadForm(this.Text, this.Text);
                }
                else if (System.IO.File.Exists(rowNames[i]))
                {
                    File _dr = new File(rowNames[i]);
                    _dr.encod();
                    loadForm(this.Text, this.Text);
                }
            }
            MessageBox.Show("'" + rowNames.Count + "' files is decoding.");
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
                try
                {
                    System.IO.Directory.CreateDirectory(n + "\\" + name.Replace(".", ""));
                    printToDataGrid(false, name.Replace(".", ""), "<dir>", 0, 0);
                    MessageBox.Show("Created directory '" + n + "\\" + name.Replace(".", "") + "'.");
                }
                catch (Exception e)
                {
                    MessageBox.Show("It is impossible to create the current file. Exception:" + e);
                }
            }
            else
            {
                try
                {
                    System.IO.File.Create(n + "\\" + name.Replace(".", "") + ".txt");
                    printToDataGrid(false, name.Replace(".", ""), ".txt", 0, 0);
                    MessageBox.Show("Created file '" + n + "\\" + name.Replace(".", "") + ".txt' .");
                }
                catch (Exception e) 
                {
                    MessageBox.Show("It is impossible to create the current file. Exception:" + e);
                }
            }
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
                for(int i = 0; i < mainDataGridView.Rows.Count; i++)
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
                        value_dgv = (mainDataGridView[1, mainDataGridView.CurrentRow.Index].Value.ToString());
                        loedMUC2(this.Text + "\\" + value_dgv, e);

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
                for (int i = 0; i < contextMenuStrip1.Items.Count-1; i++)
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

        
    }
}
