using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Linq;

namespace STOServer
{
    public class ServerObject
    {
        MyDataContext datacontext;
        static TcpListener tcpListener; // сервер для прослушивания (серверный сокет)
        List<ClientObject> clients = new List<ClientObject>(); // все подключенияpublic 
        //метод "подключения" клиента - добавляем пользователя в список активных пользователей
        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }
        protected internal void RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            ClientObject client = clients.FirstOrDefault(c => c.Id == id);
            // и удаляем его из списка подключений
            if (client != null)
                clients.Remove(client);
        }
        // прослушивание входящих подключений
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 6553);//инициализируем объект серверного сокета
                tcpListener.Start();//запуск сервер на прослушивание
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient(); //принимаем запрос от клиента(сокета) на подключение

                    ClientObject clientObject = new ClientObject(tcpClient, this); //создаем на сервере объект клиента (ClientObject)
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process)); //запускаем работу с клиентом в отдельном потоке
                    clientThread.Start(); //старт потока
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect(); //освобождение ресурсов сервера
            }
        }

        // трансляция сообщения подключенным клиентам
        protected internal void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id) // если id клиента не равно id отправляющего
                {
                    clients[i].Stream.Write(data, 0, data.Length); //передача данных
                }
            }
        }
        // отключение всех клиентов
        protected internal void Disconnect()
        {
            tcpListener.Stop(); //остановка сервера

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }
        public bool FindConnection()
        {
            SqlConnection connection;
            for (int i = 1; i < ConfigurationManager.ConnectionStrings.Count; i++)
            {
                try
                {
                    connection = new SqlConnection(ConfigurationManager.ConnectionStrings[i].ConnectionString);
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        datacontext = new MyDataContext(connection.ConnectionString);
                        return true;
                    }
                }
                catch (Exception)
                { }

            }
            return false;

        }
        public bool CheckLogin(string login)
        {
            return Convert.ToBoolean(datacontext.CheckLogin(login));
        }
        public bool CheckPassword(string login, string password)
        {
            return Convert.ToBoolean(datacontext.CheckPassword(login, password));
        }
    }
}
