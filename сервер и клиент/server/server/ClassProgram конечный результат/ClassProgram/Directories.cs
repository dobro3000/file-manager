using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesDirs
{
    /// <summary>
    /// Содержит методы для работы с директориями.
    /// </summary>
   public class Directories : AbstractClass, IPersonalManager, IList<Directory>, IList<Files>
    {
       /// <summary>
       /// Содержит массив каталогов.
       /// </summary>
        List<Directory> myListDir = new List<Directory>();

       /// <summary>
       /// Иницилизирует экземпляр класса Directories
       /// </summary>
       /// <param name="a">Массив каталогов.</param>
        public Directories(List<Directory> a)
        {
            myListDir = a;
        }

       /// <summary>
       /// Удаляет текущий массив каталогов.
       /// </summary>
        public void Delete()
        {
            for (int i = 0; i < myListDir.Count; i++)
                myListDir[i].Delete();
            myListDir.Clear();
        }

       /// <summary>
       /// Создает текущий массив каталогов.
       /// </summary>
        public void Create()
        {
            for (int i = 0; i < myListDir.Count; i++)
                myListDir[i].Create();
        }

        #region Свойства
        public string GetName
        {
            get { throw new NotImplementedException(); }
        }

        public long GetSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool GetCreate
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime GetTime
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

       /// <summary>
       /// Перемещает текущий массив каталогов.
       /// </summary>
       /// <param name="newName">Место, куда переместить.</param>
        public void MoveTo(string newName)
        {
            for (int i = 0; i < myListDir.Count; i++)
                myListDir[i].MoveTo(newName);
        }

       /// <summary>
       /// Архивирует текущий массив каталогов.
       /// </summary>
       /// <param name="newName">Имя архиватора.</param>
        public void Arhiving(string newName)
        {
            for (int i = 0; i < myListDir.Count; i++)
                myListDir[i].Arhiving(newName);
        }

       /// <summary>
       /// Разархивирует текущий массив каталогов.
       /// </summary>
       /// <param name="newName">Имя файла, который нужно разархивировать.</param>
        public void Desarhiving(string newName)
        {
            for (int i = 0; i < myListDir.Count; i++)
                myListDir[i].Desarhiving(newName);
        }

       /// <summary>
       /// Копирует текущий массив каталогов в заданое место.
       /// </summary>
       /// <param name="newName">Место, куда скопировать.</param>
        public void CopyTo(string newName)
        {
            for (int i = 0; i < myListDir.Count; i++)
                myListDir[i].CopyTo(newName);
        }

       /// <summary>
       /// Выводит содержимое текущего массива каталогов.
       /// </summary>
       /// <returns></returns>
        public void View()
        {
            for (int i = 0; i < myListDir.Count; i++)
                myListDir[i].View();
        }

       /// <summary>
       /// Сортирует текщий массив каталогов.
       /// </summary>
        public void CompareTo()
        {
            for (int i = 0; i < myListDir.Count; i++)
                myListDir[i].CompareTo();
        }

       /// <summary>
       /// Переименвывает текущий массив каталогов на рандомное имя.
       /// </summary>
        public void Rename()
        {
            for (int i = 0; i < myListDir.Count; i++)
                myListDir[i].Rename();
        }

    }
}
