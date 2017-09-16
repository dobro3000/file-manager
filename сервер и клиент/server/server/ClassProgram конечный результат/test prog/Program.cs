using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilesDirs;
using System.Reflection;


namespace test_prog
{
   public class TestProg
    {
        /// <summary>
        /// Выводит справку.
        /// </summary>
        public static void Help()
        {
            if (mesEvent != null)
                mesEvent( string.Format( "\t\t\t\nДобро пожаловать в программу!\t\t\t\n \n Вы вызвали команду Help.\n\nСписок команд:\n/set= - путь(и) обьектa(ов).\n/GetName - получает имя. \n/GetCreate - получает информацию о существовании обьекта. \n/GetSize - получает размер обьекта. \n/Delete - удаляет обьект. \n/Create - создает обьект. \n/MoveTo= - перемещает обьект по заданному пути. \n/ChangingTheCoding - менят кодировку обьекта. \n/ChangingTheCoding= - менят кодировку обьекта на заданную. \n/Encod - шифрует обьект. \n/Uncod - дешефрует обьект. \n/Arhiving= - архивирует обьект. \n/Desarhiving= - разархивирует обьект. \n/CopyTo= - копирует обьект по заданному пути. \n/View - выводит содержимое обьекта. \n/CompareTo - сортирует обьект. \n/Rename - переименовывает обьект на рандомное имя. \n/Editing - редактирует обьект путем замены символа 'a' на знак '?' \n \t\t\t\nПриятного пользования.\t\t\t\n"));
        }
        /// <summary>
        /// Иницилизирует интерфейс IManager.
        /// </summary>
        public static IPersonalManager _meneger;

        public string mes { get; set; }

        public static void Main(string[] args)
        {

            #region проверка наличия команд set и их подсчет

            int _setCount = 0;
            foreach (string _arg in args)
            {
                if (_arg.ToLower().StartsWith("/set="))
                    _setCount++;
            }
            #endregion

            #region выбор и создание экземпляра класса

            switch (_setCount)
            {
                case 0: Help(); return;
                case 1:
                    foreach (string _arg in args)
                    {
                        if (_arg.StartsWith("/set="))
                        {
                            if (System.IO.File.Exists(_arg.Remove(0, 5).Trim('"')))
                            {
                                _meneger = new File(_arg.Remove(0, 5).Trim('"'));
                            }
                            else if (System.IO.Directory.Exists(_arg.Remove(0, 5).Trim('"')))
                            {
                                _meneger = new Directory(_arg.Remove(0, 5).Trim('"'));
                            }
                            else
                            {
                                if (mesEvent != null)
                                    mesEvent("Текущий путь отсутсвует."); return;
                            }
                        }
                    }
                    break;
                default:
                    List<string> masDir = new List<string>();//хранит имена каталогов
                    List<string> masF = new List<string>();//хранит имена файлов
                    foreach (string _arg in args)
                    {
                        if (System.IO.File.Exists(_arg.Remove(0, 5).Trim('"')))
                        {
                            masF.Add(_arg.Remove(0, 5).Trim('"'));
                        }
                        else if (System.IO.Directory.Exists(_arg.Remove(0, 5).Trim('"')))
                        {
                            masDir.Add(_arg.Remove(0, 5).Trim('"'));
                        }
                    }
                    if (masF.Count != 0)
                    {
                        _meneger = new Files(masF);
                    }
                    else if (masDir.Count != 0)
                    {
                        _meneger = new Directories(masDir);
                    }
                    else
                    {
                        if (mesEvent != null)
                            mesEvent("Текущий путь отсутсвует."); return;
                    }
                    break;
                    
            }
            #endregion

            #region определение и вызов методов

            foreach (string _arg in args)
            {
                switch (_arg.ToLower())
                {
                    case "/help": Help(); return;
                    case "/delete":
                    case "/create":
                    case "/encod":
                    case "/uncod":
                    case "/rename":
                    case "/editing":
                    case "/changingthecoding":
                    case "/view":
                    case "/compareto":
                    case "/info":
                    case "/arhiving":
                    case "/desarhiving":
                        _meneger.Messang += _meneger_Messang;
                        try
                        {
                            MethodInfo metod = _meneger.GetType().GetMethod(_arg.Replace("/", "").Trim('"'));
                            metod.Invoke(_meneger, null);
                            _meneger.Messang -= _meneger_Messang;
                        }
                        catch (Exception e)
                        {
                          
                        }
                        break;
                    case "/getsize":
                    case "/getname":
                    case "/getcreate":
                        try
                        {
                            MethodInfo metod = _meneger.GetType().GetMethod(_arg.Replace("/", "").Trim('"'));
                            Console.WriteLine(metod.Invoke(_meneger, null));
                        }
                        catch { }
                    break;
                    default:
                        if (_arg.StartsWith("/moveto=") || _arg.StartsWith("/copyto=") || _arg.StartsWith("/changingthecoding=") || _arg.StartsWith("/arhiving=")||_arg.StartsWith("/desarhiving="))
                        { 
                        _meneger.Messang += _meneger_Messang;
                        try
                        {
                            object[] _params = { _arg.Remove(_arg.IndexOf("/"), _arg.IndexOf("=")+1).Trim('"') };
                            MethodInfo metod = _meneger.GetType().GetMethod(_arg.Substring(_arg.IndexOf("/"), _arg.IndexOf("=")).Trim('/'));
                            metod.Invoke(_meneger, _params);
                           _meneger.Messang -= _meneger_Messang;
                        }
                            
                        catch (Exception e) 
                        { 
                            throw e;
                        } 
                            break;
                        }
                        if (_arg.ToLower().ToLower().StartsWith("/set="))
                        {

                        }
                        else
                        {
                            if (mesEvent != null)
                                mesEvent("Команда " + _arg + " отсутствует.");
                        }
                        break;
                        #endregion
                }
            }
            Console.ReadKey();

        }
        public delegate void mesDelegate(string message);

        public static event mesDelegate mesEvent;

        /// <summary>
        /// Выводит подписку на событие.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _meneger_Messang(object sender, MyEvenArgs e)
        {
            if (mesEvent != null)
                mesEvent(e.Message.ToString());
        }

    }
}

