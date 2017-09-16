using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesDirs
{
   public abstract class AbstractClass : IManager
    {
       protected virtual void Delete();

        protected virtual void Create();

        protected virtual void MoveTo(string newName);

        protected virtual void Arhiving(string newName);

        protected virtual void Desarhiving(string newName);

        protected virtual void CopyTo(string newName);
        /// <summary>
        /// Метод для перемещения каталога.
        /// </summary>
        /// <param name="begin_dir">Директория источник</param>
        /// <param name="end_dir">Директория приёмник.</param>
        internal void copydir(string begin_dir, string end_dir)
        {
            System.IO.DirectoryInfo dir_inf = new System.IO.DirectoryInfo(begin_dir);
            try
            {
                foreach (System.IO.DirectoryInfo dir in dir_inf.GetDirectories())
                {
                    if (System.IO.Directory.Exists(end_dir + "\\" + dir.Name) != true)
                    {
                        System.IO.Directory.CreateDirectory(end_dir + "\\" + dir.Name);
                    }
                    copydir(dir.FullName, end_dir + "\\" + dir.Name);
                }
            }
            catch { }

            try
            {
                foreach (string file in System.IO.Directory.GetFiles(begin_dir))
                {
                    string filik = file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'));
                    System.IO.File.Copy(file, end_dir + "\\" + filik, true);
                }
            }
            catch { }
        }

        public void ChangingTheCoding()
        {
            
        }

        public void Encod()
        {
            
        }

        public void Uncod()
        {
            
        }

        public List<string> View()
        {
            
        }

        public void CompareTo()
        {
            
        }

        public void Rename()
        {
            string newName = System.IO.Path.GetRandomFileName();
            string str = string.Empty;
            string[] MasStructure = GetName.Split('\\');
            for (int i = 0; i < MasStructure.Length; i++)
            {
                if (MasStructure[i] == newName)
                    MasStructure[i] = newName;
            }
            for (int i = 0; i < MasStructure.Length; i++)
            {
                if (MasStructure[i] != newName)
                    str = str + MasStructure[i] + @"\";
            }
            str = str + newName;
            getNewName(str);
        }
        /// <summary>
        /// Передает новое имя.
        /// </summary>
        /// <param name="newName"></param>
        protected virtual void getNewName(string newName);

        public void Editing()
        {
            
        }
    }
}
