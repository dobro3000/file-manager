using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zip;
using System.Collections;

namespace FilesDirs
{
   /// <summary>
   /// Содержит методы для работы с файломи.
   /// </summary>
  public  class Files : File
    {

        /// <summary>
        /// Иницилизирует экземпляр класса Files.
        /// </summary>
        /// <param name="a"></param>
        public Files(List<string> a) : base(a)
        {
            fileList = a;
        }

        public virtual event EventHandler<MyEvenArgs> Messang;

        #region Свойства

        /// <summary>
        /// Возвращает имена файлов.
        /// </summary>
        public override string getname
        {

            get
            {
                 return (fileList.GetEnumerator().ToString());
            }
        }

        /// <summary>
        /// Возвращает размер файлов.
        /// </summary>
        public override string getsize
        {
            get
            {
                double s = 0;

                string strSize = null;
                for (int i = 0; i < fileList.Count; i++)
                {
                    s = new System.IO.FileInfo(fileList[i]).Length;
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
                
                return strSize;
            }
        }

        /// <summary>
        /// Возвращает значение существует ли массив или нет.
        /// </summary>
        public override bool getcreate
        {
            get
            {
                if (fileList.Count != 0) return true;
                else return false;
            }
        }

        #endregion

        /// <summary>
        /// Просматривает текущий массив файлов.
        /// </summary>
        public override void view()
        {
            for (int i = 0; i < fileList.Count; i++)
            {
                System.IO.FileInfo n = new System.IO.FileInfo(fileList[i]);
                messInfo(n.Name);
            }
        }

        /// <summary>
        /// Сортирует текущий массив файлов.
        /// </summary>
        public override void compareto()
        {

           string[] str = fileList.ToArray();
           List<string> names = new List<string>();
           for (int i = 0; i < str.Length; i++)
           { 
               System.IO.FileInfo n = new System.IO.FileInfo(str[i]);
               names.Add(n.Name);
           }
           names.Sort();
            foreach (string f in names)
                messInfo(f);
        }

        /// <summary>
      /// Выводит информацию о текущем массиве файлов.
      /// </summary>
      /// <returns>Возвращает информацию о текущем массиве файлов.</returns>
        public override void info()
        {
            List<string> str = new List<string>();
            str.Add(string.Format("Файлов: {0}", fileList.Count));
            str.Add(string.Format("Размер массива: {0}", getsize));
            foreach (string f in str)
                messInfo(f);
        }
    }
}
