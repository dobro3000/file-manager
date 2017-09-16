using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    /// <summary>
    /// Содержит методы для отправки и получение данных с сервера.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Содержит имя текущего клиента.
        /// </summary>
        static TcpClient _client;
        /// <summary>
        /// Поток для получения данных с сервера.
        /// </summary>
        private static Thread _receiveThread;

        /// <summary>
        /// Проверяет вводимые данные на корректность.
        /// </summary>
        /// <param name="_ip">IP сервера.</param>
        /// <param name="_port">Адресс порта.</param>
        public static void check(out string _address, out int _port)
        {
            _address = null;
            _port = 0;
            bool ch = true;
            while (ch)
            try
            {
                Console.Write("Укажите IP адрес сервера: ");
                _address = Console.ReadLine();
                Console.Write("Укажите порт сервера: ");
                _port = Convert.ToInt32(Console.ReadLine());
                ch = false;
            }
            catch (Exception exc) { Console.WriteLine(exc.Message + "\n"); }

        }

       
        static void Main(string[] args)
        {
            try
            {
               
                string _address;
                int _port;
                check(out _address, out _port);

                _client = new TcpClient();
                _receiveThread = new Thread(_receiveBody);
                _receiveThread.Start();
                string _textToSend;
                while (true) // Цикл подключений
                {
                    try
                    {
                        _client.Connect(_address, _port);
                    }
                    catch (Exception _e)
                    {
                        Console.WriteLine("Сервер недоступен. Следующая попытка подключения через 5 секунд.");
                        Thread.Sleep(5000);
                        continue;
                    }
                    Console.Write("Введите данные для отправки на сервер: ");
                    while (true) // Цикл передачи данных, введенных пользователем
                    {
                        //Console.Write("Введите данные для отправки на сервер: ");
                        _textToSend = Console.ReadLine();
                        try
                        {
                            _client.GetStream().Write(Encoding.UTF8.GetBytes(_textToSend), 0, _textToSend.Length);
                        }
                        catch (Exception _exc)
                        {
                            break;
                            // В большинстве случаев причина ошибки - отвалился канал передачи данных или рухнул сервер
                            // Требуется повторное переподключение
                        }
                    }
                }
            }

            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

        }

        /// <summary>
        /// Получение данных с сервера текущим пользователем.
        /// </summary>
        private static void _receiveBody()
        {
            byte[] _buf = new byte[8192*4];
            int _offset = 0;
            int _available = 0;
            string _data;
            while (true)
            {
                try
                {
                    _available = _client.Available;
                    if (_available > 0)
                    {
                        _available = _client.GetStream().Read(_buf, _offset, _available);
                        _data = Encoding.UTF8.GetString(_buf, _offset, _available);
                        if (!_data.Equals("\0"))
                            Console.WriteLine(_data);
                    }
                }
                catch (Exception exc)
                {

                }
                Thread.Sleep(1);
            }
        }

    }
}
