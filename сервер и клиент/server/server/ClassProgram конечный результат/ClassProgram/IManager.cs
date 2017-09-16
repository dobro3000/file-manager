using System.Collections.Generic;

 namespace FilesDirs
{
    /// <summary>
    /// Содержит описание общих методов для работы с файлами и каталогами.
    /// </summary>
   public interface IManager
    {

        /// <summary>
        /// Удаляет обьект.
        /// </summary>
        void delete();
        
        /// <summary>
        /// Создает обьект.
        /// </summary>
        void create();
       
        /// <summary>
        /// Перемещает обьект по указанному пути.
        /// </summary>
        /// <param name="newName">Путь, куда переместить.</param>
        void moveto(string newName);
        
        /// <summary>
        ///  Меняет кодировку файла по умолчанию.
        /// </summary>
        void changingthecoding();

        /// <summary>
        /// Шифрует файл по умолчанию.
        /// </summary>
        void encod();
        
        /// <summary>
        /// Дешефрует файл по умолчанию.
        /// </summary>
        void uncod();

        /// <summary>
        /// Архивирует обьект и сохраняет по заданному пути.
        /// </summary>
        /// <param name="newName">Путь, куда сохранить.</param>
        void arhiving();
        
        /// <summary>
        /// Дезархивирует обьект и сохраняет по заданному пути.
        /// </summary>
        /// <param name="newName">Путь, куда сохранить.</param>
        void desarhiving();

        /// <summary>
        /// Копирует обьект по заданному пути.
        /// </summary>
        /// <param name="newName">Путь, куда скопировать.</param>
        void copyto(string newName);

        /// <summary>
        /// Получает содержиме текущего обьекта.
        /// </summary>
        /// <returns>Возвращает значение в фармате List</returns>
        void view();

        /// <summary>
        /// Сортирует текущий обьект.
        /// </summary>
        /// <returns>Возвращает значение в фармате List</returns>
        void compareto();

        /// <summary>
        /// Переименовывает обьект на рандомное имя.
        /// </summary>
        void rename();
        
        /// <summary>
        /// Редактирует обьект.
        /// </summary>
        void editing();

       /// <summary>
       /// Получает информацию об обьекте.
       /// </summary>
       /// <returns>Возвращает информацию об объекте в формате List.</returns>
        void info();

    }
}
