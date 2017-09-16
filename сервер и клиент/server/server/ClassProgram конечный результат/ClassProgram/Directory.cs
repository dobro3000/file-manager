using System;
using System.Collections.Generic;
using System.IO;
using ClassDir = System.IO.Directory;
using Ionic.Zip;

namespace FilesDirs
{
    /// <summary>
    /// Содержит методы для работы с каталогом.
    /// </summary>
    public class Directory : AbstractClass
    {
        /// <summary>
        /// Имя каталога.
        /// </summary>
        private string dirName;

        /// <summary>
        /// Хранит имена каталогов.
        /// </summary>
        List<string> dirList = new List<string>();

        /// <summary>
        /// Конструктор для класса Directories.
        /// </summary>
        /// <param name="a">Массив каталогов.</param>
        internal Directory(List<string> a) : base()
        {
            for (int i = 0; i < a.Count; i++)
            {
                dirList.Add(a[i]);
                string[] str = System.IO.Directory.GetFiles(a[i], "*", SearchOption.AllDirectories);
                for (int j = 0; j < str.Length; j++)
                    fileList.Add(str[j]);
            }
        }

        /// <summary>
        /// Иницилизирует экземпляр класса MyDirectory.
        /// </summary>
        /// <param name="dname">Полное имя каталога.</param>
        public Directory(string dname) : base()
        {
            dirName = dname;
            dirList.Add(dname);
            try
            {
                string[] str = System.IO.Directory.GetFiles(dname, "*", SearchOption.AllDirectories);
                for (int i = 0; i < str.Length; i++)
                    fileList.Add(str[i]);
            }
            catch { }
        }

        /// <summary>
        /// Событие класса Directory.
        /// </summary>
        public override event EventHandler<MyEvenArgs> Messang;

        /// <summary>
        /// Удаляет текущий каталог.
        /// </summary>
        public override void delete()
        {
            for (int i = 0; i < dirList.Count; i++)
            {
                try
                {
                    ClassDir.Delete(dirList[i], true);
                    System.IO.File.Delete(dirList[i]);
                    messenger(string.Format("Обьект {0} был удален.\n", dirList[i]));
                }
                catch (Exception e) { messenger(e.Message); }
            
            }
            dirList.Clear();
        }

        /// <summary>
        /// Создает новый каталог.
        /// </summary>
        public override void create()
        {
            for (int i = 0; i < dirList.Count; i++)
            {
                try
                {
                    string newName = new System.IO.DirectoryInfo(dirList[i]).FullName;
                    ClassDir.CreateDirectory(newName.Replace("/"+new System.IO.DirectoryInfo(dirList[i]).Name, "")+ System.IO.Path.GetRandomFileName().Replace(".", ""));
                    messenger(string.Format("Создан новый обьект с именем {0}. \n", dirList[i]));
                }
                catch (Exception e) { messenger(e.Message); }
            }
        }

        /// <summary>
        /// Перемещает текущий каталог по заданному пути.
        /// </summary>
        /// <param name="newName">Путь, куда переместить.</param>
        public override void moveto(string newName)
        {
            for (int i = 0; i < dirList.Count; i++)
            {
                if ((System.IO.Directory.GetDirectories(dirList[i]).Length == 0) && (System.IO.Directory.GetFiles(dirList[i]).Length == 0))
                {
                    try
                    {
                        System.IO.Directory.Delete(dirList[i]);
                        System.IO.Directory.CreateDirectory(newName);
                        
                    }
                    catch (Exception e) { messenger(e.Message); }
                }
                else
                {
                     perebor_updates(dirList[i], newName + "\\" + dirList[i]);
                }
            }
        }
        /// <summary>
        /// Вспомогательный метод для перемещения каталога.
        /// </summary>
        /// <param name="name">Директория источник</param>
        /// <param name="newName">Директория приёмник.</param>
        internal void peremech_updates(string name, string newName)
        {
            System.IO.DirectoryInfo dir_inf = new System.IO.DirectoryInfo(name);
            try
            {
                foreach (System.IO.DirectoryInfo dir in dir_inf.GetDirectories())
                {
                    if (System.IO.Directory.Exists(newName + "\\" + dir.Name) != true)
                    {
                        System.IO.Directory.CreateDirectory(newName + "\\" + dir.Name);
                    }
                    peremech_updates(dir.FullName, newName + "\\" + dir.Name);
                }
            }
            catch (Exception e) { messenger(e.Message); }

            try
            {
                foreach (string file in System.IO.Directory.GetFiles(name))
                {
                    string filik = file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'));
                    System.IO.File.Move(file, newName + "\\" + filik);
                }
            }
            catch (Exception e) { messenger(e.Message); }
            for (int i = 0; i < dirList.Count; i++)
            {
                messInfo(string.Format("Обьект {0} был перемещен в {1} .\n",new System.IO.DirectoryInfo( dirList[i]).Name, new System.IO.DirectoryInfo( newName).Name));
            }
            System.IO.Directory.Delete(name, true);

        }

        /// <summary>
        /// Архивирует текущий каталог и сохраняет его по заданному пути.
        /// </summary>
        /// <param name="newName">Путь, куда сохранить.</param>
        public override void arhiving()
        {
            for (int i = 0; i < dirList.Count; i++)
            {
                ZipFile zf = new ZipFile(dirList[i]+".zip");
                zf.AddDirectory(dirList[i]);
                zf.Save();
                messenger(string.Format("Обьект " + dirList[i] + " был заархивирован.\n"));
            }
        }

        /// <summary>
        /// Дезорхивирует текущий каталог и сохраняет его по заданному пути.
        /// </summary>
        /// <param name="newName">Путь, куда сохранить.</param>
        public override void desarhiving()
        {
            for (int i = 0; i < dirList.Count; i++)
            {
                ZipFile Arch = new ZipFile(dirList[i]+".zip");
                Arch.ExtractAll(dirList[i]);
                messenger(string.Format("Обьект " + dirList[i] + " был дезархивирован.\n"));
            }
        }

        /// <summary>
        /// Копирует текущий каталог по указанному пути.
        /// </summary>
        /// <param name="newName">Путь, куда скопировать.</param>
        public override void copyto(string newName)
        {
            for (int i = 0; i < dirList.Count; i++)
            {
                if ((System.IO.Directory.GetDirectories(dirList[i]).Length == 0) && (System.IO.Directory.GetFiles(dirList[i]).Length == 0))
                {
                    System.IO.Directory.CreateDirectory(newName + "\\" + new System.IO.DirectoryInfo(dirList[i]).Name);
                }
                else
                {
                    System.IO.Directory.CreateDirectory(newName + "\\" + new System.IO.DirectoryInfo(dirList[i]).Name);
                    perebor_updates(dirList[i], newName + "\\" + new System.IO.DirectoryInfo(dirList[i]).Name);
                }
            }
        }
        /// <summary>
        /// Вспомогательный метод для копирования каталога.
        /// </summary>
        /// <param name="name">Директория источник</param>
        /// <param name="newName">Директория приёмник.</param>
        internal void perebor_updates(string name, string newName)
        {
            System.IO.DirectoryInfo dir_inf = new System.IO.DirectoryInfo(name);
            try
            {
                foreach (System.IO.DirectoryInfo dir in dir_inf.GetDirectories())
                {
                    if (System.IO.Directory.Exists(newName + "\\" + dir.Name) != true)
                    {
                        System.IO.Directory.CreateDirectory(newName + "\\" + dir.Name);
                    }
                    perebor_updates(dir.FullName, newName + "\\" + dir.Name);
                }
            }
            catch (Exception e) { messenger(e.Message); }

            try
            {
                foreach (string file in System.IO.Directory.GetFiles(name))
                {
                    string filik = file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'));
                    System.IO.File.Copy(file, newName + "\\" + filik, true);
                }
            }
            catch (Exception e) { messenger(e.Message); }
            for (int i = 0; i < dirList.Count; i++)
                messInfo(string.Format("Обьект {0} был перемещен в {1}.\n", new System.IO.DirectoryInfo(dirList[i]).Name, new System.IO.DirectoryInfo( newName).Name));
        }

        /// <summary>
        /// Переименовывает текущий обьект.
        /// </summary>
        public override void rename()
        {

            for (int i = 0; i < dirList.Count; i++)
            {
                string str = string.Empty;
                str = dirList[i].Replace("/" + new System.IO.DirectoryInfo(dirList[i]).Name, "") + System.IO.Path.GetRandomFileName().Replace(".", "");
                chengName( dirList[i], str);
            }
        }
        /// <summary>
        /// Переименовывет текущий каталог.
        /// </summary>
        /// <param name="str">Новое имя каталога.</param>
        internal override void chengName(string str, string name)
        {
            System.IO.Directory.CreateDirectory(name);
                peremech_updates(str, name);
        }

        #region Свойства

        /// <summary>
        /// Возвращает имя каталога.
        /// </summary>
        public override string getname
        {
            get { return (dirName); }
        }

        /// <summary>
        /// Возвращает размер каталога.
        /// </summary>
        public override string getsize
        {
            get
            {
                double s = 0;
                string strSize = null;
                for (int j = 0; j < dirList.Count; j++)
                {
                    try
                    {
                        try
                        {
                            string[] str = System.IO.Directory.GetFiles(dirList[j], "*", System.IO.SearchOption.AllDirectories);
                            for (int i = 0; i < str.Length; i++)
                                s += new System.IO.FileInfo(str[i]).Length;
                            if (s / 1024 <= 1024)
                            {
                                strSize = (string)(Math.Round(s / 1024, 2) + " Kb");
                            }
                            else if ((s / 1024) / 1024 <= 1024)
                            {
                                strSize = (string)(Math.Round((s / 1024) / 1024, 2) + " Mb");
                            }
                            else if (((s / 1024) / 1024) / 1024 <= 1024)
                            {
                                strSize = (string)(Math.Round(((s / 1024) / 1024) / 1024, 2) + " Gb");
                            }
                        }
                        catch { }

                    }
                    catch { }
                }
                return strSize;
            }
            }

        /// <summary>
        /// Возвращает существование каталога типа bool.
        /// </summary>
        public override bool getcreate
        {
            get
            {
                return (ClassDir.Exists(dirName));
            }


        }

        #endregion

        /// <summary>
        /// Получает информацию о текущем каталоге.
        /// </summary>
        /// <returns>Возвращает информацию о текущем каталоге.</returns>
        public override void info()
        {
            System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(dirName);
            List<string> info = new List<string>();
            info.Add(("Имя текущего каталога: " + d.Name));
            info.Add(("Путь к текущему каталогу: " + d.FullName));
            info.Add(("Обьем: " + getsize));
            info.Add(("Последний доступ к текущему каталогу: " + d.LastAccessTime));
            info.Add(("Каталог, в котором храниться текущий каталог: " + d.Parent));
            info.Add(("Диск, на котором храниться текущий каталог: " + d.Root));

            string _str = ">>>\n";
            for (int i = 0; i < info.Count; i++)
            {
                _str += info[i] + "\n";
                messInfo(info[i]);
            }
            messenger(_str);
        }

        /// <summary>
        /// Возвращает содержимое текущего каталога.
        /// </summary>
        public override void view()
        {
            string _str = ">>>\n";
            string[] dir = System.IO.Directory.GetDirectories(dirName);
            string[] f = System.IO.Directory.GetFiles(dirName);
          foreach(string d in dir)
            {
                _str += d + "\n";
            }
          foreach (string file in f)
          {
                _str += file + "\n";
          }
            messenger(_str);
        }

        /// <summary>
        /// Сортирует текущий каталог.
        /// </summary>
        public override void compareto()
        {
            string _str = ">>>\n";
            string[] dir = System.IO.Directory.GetDirectories(dirName);
            string[] file = System.IO.Directory.GetFiles(dirName);
            Array.Sort(dir);
            Array.Sort(file);
            foreach (string f in dir)
            {
                _str += f + "\n";
                messInfo(f);
            }
            foreach (string f in file)
            {
                _str += f + "\n";
                messInfo(f);
            }
            messenger(_str);
        }

        /// <summary>
        /// Вызывает событие.
        /// </summary>
        /// <param name="name">Имя источника события.</param>
        public override void messenger(string name)
        {
            if (Messang != null)
            { Messang(this, new MyEvenArgs(string.Format(name))); }
        }

        /// <summary>
        /// Сообщение для выводы информации.
        /// </summary>
        /// <param name="obj"></param>
        public override void messInfo(string obj)
        {
            if (Messang != null)
            { Messang(this, new MyEvenArgs(string.Format(obj))); }
       
        }
    }
}