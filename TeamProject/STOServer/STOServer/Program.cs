using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace STOServer
{
    #region Chat TcpListener
    class Program
    {
        static ServerObject server; // сервер
        static Thread listenThread; // потока для прослушивания
        static void Main(string[] args)
        {
            try
            {
                server = new ServerObject(); //иницализруем объект сервера
                listenThread = new Thread(new ThreadStart(server.Listen)); //готовим поток на прослушивание канала
                listenThread.Start(); //старт потока
            }
            catch (Exception ex)
            {
                server.Disconnect(); //в случае ошибки  освобождаем ресурсе объекта сервера
                Console.WriteLine(ex.Message);
            }
        }
    }
    #endregion
}
