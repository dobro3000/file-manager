using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using test_prog;

namespace ConsoleApplication15
{
    /// <summary>
    /// Содержит методы для работы с данными, введенными клиентами на сервер.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Поток для подключения клиентов.
        /// </summary>
        private static Thread _aceptThread;
        /// <summary>
        /// Поток для получения данных от клиентов.
        /// </summary>
        private static Thread _receiveThread;
        /// <summary>
        /// Поток для отправки данных клиентам.
        /// </summary>
        private static Thread _sendThread;
        /// <summary>
        /// Поток для проверки на существование подключения клиента.
        /// </summary>
        private static Thread _checkThread;
        /// <summary>
        /// Хранит путь серверу.
        /// </summary>
        private static TcpListener _server;
        /// <summary>
        /// Хранит список подключенных клиентов.
        /// </summary>
        private static List<TcpClient> _clients;

        /// <summary>
        /// Проверяет вводимые данные на корректность.
        /// </summary>
        /// <param name="_ip">IP адресс сервера.</param>
        /// <param name="_port">Адресс порта.</param>
        public static void check(out IPAddress _ip, out int _port)
        {
            _ip = null;
            _port = 0;
            bool ch = true;
            while (ch)
                try
                {
                    Console.Write("Укажите IP адрес сервера: ");
                    _ip = IPAddress.Parse(Console.ReadLine());
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
                IPAddress _ip;
                int _port;
                check(out _ip, out _port);

            _server = new TcpListener(_ip, _port);
            _server.Start();
            _clients = new List<TcpClient>();
            _aceptThread = new Thread(_aceptBody);
            _aceptThread.Start();
            _receiveThread = new Thread(_receiveBody);
            _receiveThread.Start();
            _checkThread = new Thread(_checkBody);
            _checkThread.Start();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            while (true) { Thread.Sleep(1000); }
        }

        /// <summary>
        /// Подключает клиента к серверу и добавляет его в очередь.
        /// </summary>
        private static void _aceptBody()
        {
            TcpClient _client;
            while (true)
            {
                if (_server.Pending())
                {
                    _client = _server.AcceptTcpClient();
                    _clients.Add(_client);
                    Console.WriteLine("Подключился клиент: {0}", _client.Client.RemoteEndPoint);
                }
                Thread.Sleep(1);
            }
        }


        /// <summary>
        /// Проверяет клиентов на подключение к серверу.
        /// </summary>
        private static void _checkBody()
        {
            while (true)
            {
                lock (_clients)
                {
                    for (int _i = 0; _i < _clients.Count; _i++)
                    {
                        try
                        {
                            _clients[_i].GetStream().Write(new byte[1], 0, 1);
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine("Клиент: {0} более недоступен.", _clients[_i].Client.RemoteEndPoint);
                            _clients.Remove(_clients[_i--]);
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Обрабатывает данные, посланные клиентами на сервер.
        /// </summary>
        private static void _receiveBody()
        {
            byte[] _buf = new byte[8192*4];
            int _offset = 0;
            int _available = 0;
            string _data;
            while (true)
            {
                for (int _i = 0; _i < _clients.Count; _i++)
                {
                    try
                    {
                        _available = _clients[_i].Available;
                        if (_available > 0)
                        {
                            _available = _clients[_i].GetStream().Read(_buf, _offset, _available);
                            _data = Encoding.UTF8.GetString(_buf, _offset, _available);
                            Console.WriteLine("{0} > {1}", _clients[_i].Client.RemoteEndPoint, _data);
                            _sendThread = new Thread(_sendData);
                            _sendThread.Start(new ParamData() { client = _clients[_i], message = _data });

                        }
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Клиент: {0} более недоступен.", _clients[_i].Client.RemoteEndPoint);
                        _clients.Remove(_clients[_i--]);
                    }
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Содержит поля для хранения клиента и его сообщения на сервер.
        /// </summary>
        struct ParamData
        {
            /// <summary>
            /// Имя текущего клиента.
            /// </summary>
            public TcpClient client;
            /// <summary>
            /// Сообщение на сервер текущего объекта.
            /// </summary>
            public string message;
        }

        /// <summary>
        /// Отправляет данные текущему пользователю.
        /// </summary>
        /// <param name="data">Имя клиента и его сообщение.</param>
        private static void _sendData(object data)
        {
           TestProg _ts = new TestProg();
           ParamData _d = (ParamData)data;
           TcpClient client = _d.client;
           string[] message = _d.message.Split('#');

           TestProg.mesEvent += TestProg_mesEvent;
           TestProg.Main(message);

           string _fmess = "\nВаш ответ: \n" + messageMy + "\nВведите данные для отправки на сервер: " ;
           byte[] _buf = new byte[8192*4];
           _buf = Encoding.UTF8.GetBytes(_fmess);
           int _offset = 0;
           int _available = _buf.Length;
           try
           {
               client.GetStream().Write(_buf, _offset, _available);
           }
           catch (Exception exc)
           {
               Console.WriteLine("Клиент: {0} более недоступен.", client.Client.RemoteEndPoint);
               _clients.Remove(client);
           }
        }

        /// <summary>
        /// Хранит сообщение о текущем состоянии объекта.
        /// </summary>
        static string messageMy;

        /// <summary>
        /// Принимает сообщения текущем о состоянии объекта из библиотеки FilesDirs.
        /// </summary>
        /// <param name="message">Сообщение о текущем состоянии объекта.</param>
        private static void TestProg_mesEvent(string message)
        {
            messageMy = message;
        }
    }
}

















