using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

namespace STOServer
{
    #region ChatClientObjcet
    public class ClientObject
    {
        protected internal string Id { get; private set; } //уникальный иденификатор пользователя
        protected internal NetworkStream Stream { get; private set; }//у каждого пользователя свой поток
        TcpClient client; //объект клиента
        ServerObject server; // объект сервера

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString(); //задаем пользователю уникальный ID
            client = tcpClient; //инициализируем клиентский сокет 
            server = serverObject; //инициализируем серверный сокет клиента
            serverObject.AddConnection(this); //передаем объект клиента серверному объекту для подключения
        }

        //метод обработки информации с клиента
        public void Process()
        {
            try
            {
                Stream = client.GetStream(); //получаем поток
                // получаем имя пользователя
                string message = "";//GetMessage();
                while (true)
                {
                    try
                    {
                        message = GetMessage();  //получаем сообщение от клиента
                        if (message == "")
                            throw new Exception("Выход");
                        foreach (string _message in message.Split('£'))
                        {
                            switch (_message.Split(';')[0])
                            {
                                
                            }
                            Console.WriteLine(_message);
                        }

                    }
                    catch
                    {
                        message = String.Format("Один из клиентов разорвал соединение");
                        Console.WriteLine(message);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id); //особождаем все ресурсы клиентского объекта на сервере
                Close(); //закрываем соединение
            }
        }

        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            //цикл получения данных из потока
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
    #endregion
}
