using System;
using System.Collections.Generic;
using System.Text;

namespace FilesDirs
{
    /// <summary>
    /// Содержит общие методы для работы с файлом и каталогом.
    /// </summary>
    public abstract class AbstractClass :  IPersonalManager
    {
        /// <summary>
        /// Массив файлов.
        /// </summary>
        public List<string> fileList { get; internal set;}

        /// <summary>
        /// Переменная, хранящая поток файла.
        /// </summary>
        public System.IO.FileStream stream { get; private set; }

        /// <summary>
        /// Событие класса AbstractClass.
        /// </summary>
        public abstract event EventHandler<MyEvenArgs> Messang;

        /// <summary>
        /// 
        /// </summary>
        public AbstractClass()
        {
            fileList = new List<string>();
        }

        #region Методы, реализующиеся в классах наследниках

        /// <summary>
        /// Удаляет текущий обект.
        /// </summary>
        public abstract void delete();

        /// <summary>
        /// Создает текущий обьект.
        /// </summary>
        public abstract void create();

        /// <summary>
        /// Перемещает текущий обект.
        /// </summary>
        /// <param name="newName"></param>
        public abstract void moveto(string newName);

        /// <summary>
        /// Архивирует текущий обьект по заданному пути.
        /// </summary>
        /// <param name="newName">Путь, куда переместить.</param>
        public abstract void arhiving();

        /// <summary>
        /// Разархивирует текущий обьект и сохраняет егшо по заданному пути.
        /// </summary>
        /// <param name="newName">Путь, куда сохранить.</param>
        public abstract void desarhiving();

        /// <summary>
        /// Копирует текущий обьект по заданному пути.
        /// </summary>
        /// <param name="newName">Путь, куда скопировать.</param>
        public abstract void copyto(string newName);

        /// <summary>
        /// Переименовывает текущий обьект.
        /// </summary>
        public abstract void rename();
        /// <summary>
        /// Вспомогательные метод для переименования обьекта.
        /// </summary>
        /// <param name="str">Новое имя.</param>
        internal abstract void chengName(string str, string name);


        #endregion

        #region Свойства


        /// <summary>
        /// Возвращает имя обьекта.
        /// </summary>
        public abstract string getname { get; }

        /// <summary>
        /// Возвращает размер обьекта.
        /// </summary>
        public abstract string getsize { get; }

        /// <summary>
        /// Возвращает значение типа bool осуществовании обьекта.
        /// </summary>
        public abstract bool getcreate { get; }

        #endregion

        #region Общие методы для классов наследников


        /// <summary>
        /// Выводит информацию об объекте.
        /// </summary>
        /// <returns>Возвращает информацию об объекте</returns>
        public abstract void info();

        /// <summary>
        /// Выводит содержимое обьекта.
        /// </summary>
        /// <returns>Возвращает содержимое обьекта.</returns>
        public abstract void view();

        /// <summary>
        /// Сортирует текущий обьект.
        /// </summary>
        public abstract void compareto();

        /// <summary>
        /// Редактирование обьекта.
        /// </summary>
        public void editing()
        {
            for (int i = 0; i < fileList.Count; i++)
            {
                System.IO.FileStream stream = fileStream(fileList[i]);
                char a = 'a';
                char b = '!';
                {
                    byte[] buffer = new byte[4096];

                    while (stream.Position < stream.Length)
                    {
                        int count = stream.Read(buffer, 0, buffer.Length);

                        for (int j = 0; j < count; j++)
                        {
                            if (buffer[j] == (byte)a)
                            {
                                buffer[j] = (byte)b;
                            }
                        }
                        stream.Write(buffer, 0, buffer.Length);
                        stream.Flush();
                    }
                }
                messenger(fileList[i]);
                stream.Close();
            }
        }

        /// <summary>
        /// Кодирует обьект по умолчанию в кодировке ASCII.
        /// </summary>
        public void changingthecoding()
        {
            for (int i = 0; i < fileList.Count; i++)
            {
                System.IO.FileStream stream = fileStream(fileList[i]);

                byte[] buffer = new byte[4096];
                byte[] newBuffer = new byte[4096];

                while (stream.Position < stream.Length)
                {
                    int count = stream.Read(buffer, 0, buffer.Length);

                    for (int j = 0; j < count - 1; j++)
                    {
                        newBuffer = Encoding.ASCII.GetBytes(buffer[j] + " ");

                    }
                    stream.Write(newBuffer, 0, newBuffer.Length);
                    stream.Flush();
                }
                messenger(fileList[i]);
                stream.Close();
            }
        }

        /// <summary>
        /// Кодирует обьект в заданой кодировке.
        /// </summary>
        /// <param name="a">Название кодировки.</param>
        public void changingthecoding(string a)
        {
            Encoding e;
            for (int i = 0; i < fileList.Count; i++)
            {
                System.IO.FileStream stream = fileStream(fileList[i]);

               // Enum.Parse(typeof (Encoding), a);
                switch (a.ToLower())
                {
                    case "unicode": e = Encoding.Unicode; break;
                    case "ascii": e = Encoding.ASCII; break;
                    case "default": e = Encoding.Default; break;
                    case "utf32": e = Encoding.UTF32; break;
                    case "utf7": e = Encoding.UTF7; break;
                    case "utf8": e = Encoding.UTF8; break;
                    default: throw new Exception(string.Format("Данная кодировка отсутствует."));
                }

                byte[] buffer = new byte[4096];
                byte[] newBuffer = new byte[4096];

                while (stream.Position < stream.Length)
                {
                    int count = stream.Read(buffer, 0, buffer.Length);

                    for (int j = 0; j < count - 1; j++)
                    {
                        newBuffer = e.GetBytes(buffer[j] + " ");

                    }
                    stream.Write(newBuffer, 0, newBuffer.Length);
                    stream.Flush();
                }
                messenger(fileList[i]);
                stream.Close();
            }
        }

        /// <summary>
        /// Шифрует обьект по умолчанию.
        /// </summary>
        public void encod()
        {
            for (int i = 0; i < fileList.Count; i++)
            {
                try
                {
                    System.IO.File.Encrypt(fileList[i]);
                    messenger(fileList[i]);
                }
                catch (Exception e) { messenger(e.Message); }
            }
        }

        /// <summary>
        /// Дешефрует обьект по умолчанию.
        /// </summary>
        public void uncod()
        {
            for (int i = 0; i < fileList.Count; i++)
            {
                try
                {
                    System.IO.File.Decrypt(fileList[i]);
                    messenger(fileList[i]);
                }
                catch (Exception e) { messenger(e.Message); }
            }
        }
        /// <summary>
        /// Вызывает событие.
        /// </summary>
        /// <param name="name">Имя источника события.</param>
        public abstract void messenger(string name);
        /// <summary>
        /// Событие для вывода информации.
        /// </summary>
        /// <param name="obj"></param>
        public abstract void messInfo(string obj);

        /// <summary>
        /// Открывает поток.
        /// </summary>
        /// <param name="a">Файл, который нужно открыть.</param>
        /// <returns>Возвращает поток.</returns>
        internal System.IO.FileStream fileStream(string a)
        {

            if (System.IO.File.Exists(a))
                try
                {
                    stream = System.IO.File.Open(a, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
                }
                catch (System.IO.IOException exc)
                {
                    throw exc;
                }
                catch (UnauthorizedAccessException exc)
                {
                    throw exc;
                }
            else
            {
                throw new MyException();
            }
            return stream;
        }

        #endregion




    }
}
