using System;

namespace FilesDirs
{
    /// <summary>
    /// Содержит методы для обработки события.
    /// </summary>
   public class MyEvenArgs : EventArgs
    {
        /// <summary>
        /// Иницилизирует экземпляр класса класса MyEvenArgs.
        /// </summary>
        /// <param name="message">Переменная, передаваемая параметры свойства.</param>
        public MyEvenArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Свойствo класса MyEvenArgs.
        /// </summary>
        public string Message { get; private set; }



    }
}
