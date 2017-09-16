using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using System;

namespace FilesDirs
{
    /// <summary>
    /// Содержит методы для работы с каталогами.
    /// </summary>
  public  class Directories : Directory
    {
        /// <summary>
        /// Содержит имена каталога.
        /// </summary>
        private List<string> myListDir = new List<string>();

        /// <summary>
        /// Иницилизирует экземпляр класса Directories.
        /// </summary>
        /// <param name="a"></param>
        public Directories(List<string> a) : base(a)
        {
            myListDir = a;
        }

        #region Свойства

        /// <summary>
        /// Возвращает имя каталога.
        /// </summary>
        public override string getname
        {
            get { return myListDir.ToString(); }
        }

        /// <summary>
        /// Возвращает размер каталогов.
        /// </summary>
        public override string getsize
        {
            get 
            {

                double s = 0;
                string strSize = null;
                for (int j = 0; j < myListDir.Count; j++)
                {
                    try
                    {
                        try
                        {
                            string[] str = System.IO.Directory.GetFiles(myListDir[j], "*", System.IO.SearchOption.AllDirectories);
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
                if (myListDir.Count != 0)
                    return true;
                else return false;
            }
        }

       
        #endregion

        public virtual event EventHandler<MyEvenArgs> Messang;

        /// <summary>
        /// Выводит содерживмое текущего массива каталогов.
        /// </summary>
        /// <returns>Возвращает содержимое текущего массива каталогов.</returns>
        public override void view()
        {
            string _str = "\n";
            string[] str = myListDir.ToArray();
            for (int i = 0; i < str.Length; i++)
            {

                System.IO.FileInfo n = new System.IO.FileInfo(str[i]);
                _str += ">"+str[i];
                messInfo(str[i]);
            }
            messenger(_str);
        }

        /// <summary>
        /// Сортирует текущий массив обьектов.
        /// </summary>
        public override void compareto()
        {
            string _str = ">>>\n";
            string[] str = myListDir.ToArray();
            List<string> names = new List<string>();
            for (int i = 0; i < str.Length; i++)
            { 
                 System.IO.FileInfo n = new System.IO.FileInfo(str[i]);
                 names.Add(n.Name);
            }
            names.Sort();
            foreach (string f in str)
            {
                _str += f + "\n";
                messInfo(f);
            }
            messenger(_str);
        }

        /// <summary>
        /// Выводит информацию о текущем массиве каталогов.
        /// </summary>
        /// <returns>Возвращает информацию о текущем массиве каталогов.</returns>
        public override void info()
        {
            string _str = ">>>\n";
            List<string> str = new List<string>();
            str.Add(string.Format("Каталогов: {0}", myListDir.Count));
            str.Add(string.Format("Размер массива: {0}", getsize));

            foreach (string f in str)
            {
                _str += f + "\n";
                messInfo(f);
            }
            messenger(_str);
        }
    }
}
