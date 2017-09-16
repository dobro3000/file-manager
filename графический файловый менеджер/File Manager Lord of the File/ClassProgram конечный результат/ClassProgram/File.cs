using Ionic.Zip;
using System;
using System.Text;
using FileClass = System.IO.File;
using System.Collections.Generic;

namespace FilesDirs
{

    /// <summary>
    /// Содержит методы для работы с файлом.
    /// </summary>
    public class File : AbstractClass
    {

        /// <summary>
        /// Имя файла.
        /// </summary>
        private string fileName;

        /// <summary>
        /// Конструктор для класса Files.
        /// </summary>
        /// <param name="a">Массив файлов.</param>
        internal File(List<string> a) : base()
        {
                fileList = a;
        }

        /// <summary>
        /// Иницилизирует экземпляр класса MyFile.
        /// </summary>
        /// <param name="fname">Полный путь к файлу.</param>
        public File(string fname) : base()
        {
            fileList.Add(fname);
            fileName = fname;
        }

        public override event EventHandler<MyEvenArgs> Messang;

        /// <summary>
        /// Удаляет текущий файл.
        /// </summary>
        public override void delete()
        {
            for (int i = 0; i < fileList.Count; i++)
            {
                try
                {
                    System.IO.File.Delete(fileList[i]);
                    messenger(string.Format("Обьект {0} был удален.\n", fileList[i]));
                }
                catch (Exception e) { throw e; }
                
            }
            fileList.Clear();
        }

        /// <summary>
        /// Создает текущий файл.
        /// </summary>
        public override void create()
        {
            for (int i = 0; i < fileList.Count; i++)
            {
                try
                {
                    string newName = new System.IO.FileInfo(fileList[i]).FullName;
                    System.IO.File.Create(newName.Replace(new System.IO.FileInfo(fileList[i]).Name, "")+System.IO.Path.GetRandomFileName()+".txt");
                    messenger(string.Format("Создан новый обьект с именем {0}", System.IO.Path.GetRandomFileName() + ".txt\n"));
                }
                catch (Exception e) { throw e; }
                
            }
        }

        /// <summary>
        /// Перемещает текущий файл по заданному пути.
        /// </summary>
        /// <param name="newName">Путь, куда переместить файл.</param>
        public override void moveto(string newName)
        {
            for (int i = 0; i < fileList.Count; i++)
            {
                try
                {
                    System.IO.File.Move(fileList[i], newName+i+".txt");
                    messenger(string.Format("Обьект {0} был перемещен в {1} .", fileList[i], newName + i + ".txt\n"));
                }
                catch (Exception e) { throw e; }
                
            }

        }

        /// <summary>
        /// Архивирует текущий файл и сохраняет его по заданному пути.
        /// </summary>
        /// <param name="newName">Путь, куда сохранить.</param>
        public override void arhiving()
        {
            for (int i = 0; i < fileList.Count; i++)
                try
                {
                    {
                        //Создание архива
                        ZipFile zf = new ZipFile(fileList[i]+".zip");
                        zf.AddFile(fileList[i]);
                        zf.Save(); //Сохраняем архив.
                        messenger(string.Format("Обьект " + fileList[i] + " был заархивирован.\n"));
                    }
                }
                catch (Exception e) { throw e; }

        }

        /// <summary>
        /// Дезархивирует текущий файла и сохраняет его по заданному пути.
        /// </summary>
        /// <param name="newName">Путь, куда сохранить.</param>
        public override void desarhiving()
        {
            try
            {
                for (int i = 0; i < fileList.Count; i++)
                {
                    ZipFile Arch = new ZipFile(fileList[i] + ".zip");
                    Arch.ExtractAll(fileList[i]);
                    messenger(string.Format("Обьект " + fileList[i] + " был дезархивирован.\n"));
                }
            }
            catch (Exception e) { throw e; }

        }

        /// <summary>
        /// Копирует текущий файл по заданному пути.
        /// </summary>
        /// <param name="newName">Путь, куда скопировать.</param>
        public override void copyto(string newName)
        {
            for (int i = 0; i < fileList.Count; i++)
            {
                try
                {
                    FileClass.Copy(fileList[i],newName+i+".txt", true);
                    
                }
                catch (Exception e) { throw e; }
                messenger(string.Format("Обьект "+ fileList[i]+" был скопирован.\n"));
            }

        }

        /// <summary>
        /// Переименовывает текущий обьект.
        /// </summary>
        public override void rename()
        {
            for (int i = 0; i < fileList.Count; i++)
            {
                string str = string.Empty;
                System.IO.FileInfo fname = new System.IO.FileInfo(fileList[i]);
                str = fileList[i].Replace(fname.Name, System.IO.Path.GetRandomFileName().Replace(".", ""));
                FileClass.Move(fileList[i], str + ".txt");
                messenger(string.Format("Обьект {0} был переименован на новое имя: {1}.txt .\n", fileList[i], str));
            }
        }
        /// <summary>
        /// Переименовывает текущее имя файла.
        /// </summary>
        /// <param name="str">Новое имя.</param>
        internal override void chengName(string str, string name)
        {

        }

        #region Свойства

        /// <summary>
        /// Возвращает имя файла.
        /// </summary>
        public override string getname
        {
            get
            {
                return (fileName);
            }
        }

        /// <summary>
        /// Возвращает размер файла.
        /// </summary>
        public override string getsize
        {
            get {
                string s = null;
                try
                {
                    s = new System.IO.FileInfo(fileName).Length.ToString() + " byte";
                }
                catch { }
                return s;
            }
        }

        /// <summary>
        /// Возвращает информацию о существовании файла типа bool. 
        /// </summary>
        public override bool getcreate
        {
            get
            {
                return (FileClass.Exists(fileName));
            }
        }

        #endregion

        /// <summary>
        /// Получает информацию о текущем файле.
        /// </summary>
        /// <returns>Возвращает значение в фармате List</returns>
        public override void info()
        {

            System.IO.FileInfo file = new System.IO.FileInfo(fileName);
            List<string> info = new List<string>();
            info.Add(string.Format("Имя текущего файла: {0}", file.Name));
            info.Add(string.Format("Путь к текущему файлу: {0}", file.FullName));
            info.Add(string.Format("Обьем: {0}", getsize));
            info.Add(string.Format("Последний доступ к текущему файлу: {0}", file.LastAccessTime));
            info.Add(string.Format("Расширение текущего файла: {0}\n", file.Extension));
            foreach (string f in info)
                messInfo(f);
        }

        /// <summary>
        /// Получает содержиме текущего файла.
        /// </summary>
        public override void view()
        {
            string[] str;
            str = System.IO.File.ReadAllLines(fileName);
            foreach (string f in str)
                messInfo(f);
        }

        /// <summary>
        /// Сортирует текущий файл по умолчанию.
        /// </summary>
        public override void compareto()
        {
            string[] str;
            str = System.IO.File.ReadAllLines(fileName);
            Array.Sort(str);
            foreach(string f in str)
                messInfo(f);

        }
        /// <summary>
        /// Вызывает событие.
        /// </summary>
        public override void messenger(string name)
        {
            if (Messang != null)
            { Messang(this, new MyEvenArgs(string.Format("Объект '{0}' был изменен.", new System.IO.FileInfo(name).Name))); }
        }

        public override void messInfo(string obj)
        {
            if (Messang != null)
            { Messang(this, new MyEvenArgs(string.Format(obj))); }

        }

    }
}
