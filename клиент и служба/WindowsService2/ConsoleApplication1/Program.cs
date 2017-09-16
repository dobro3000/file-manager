using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using eLOG = System.Diagnostics.EventLog;
using WindowsService2;


namespace ConsoleApplication1
{
    class Program
    {
        static Thread t;
        public static eLOG el;

        static void Main(string[] args)
        {
            string jName = "LOTF_MESSAGE";
            string jSource = "LOTF";
            if (!eLOG.SourceExists(jSource))
                eLOG.CreateEventSource(jSource, jName);
            el = new eLOG(jName);
            el.Source = jSource;


            Console.ReadKey();
        }
    }
}
