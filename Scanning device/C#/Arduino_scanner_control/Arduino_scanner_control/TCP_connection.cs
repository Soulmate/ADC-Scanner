using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net;      // потребуется
using System.Net.Sockets;    // потребуется

using System.Threading;

namespace Arduino_scanner_control
{
    class TCP_connection : IDisposable
    {
        TcpListener server;
        List<string> tcp_2send = new List<string>();
        string port_output = ""; //временная строка, куда приходит всё что приходит в порт, и из которой мы извлекаем (удаляя) пакеты
        List<string> packages = new List<string>(); //пакеты, которые извлекли из этой строки        
        bool flag_run_server = false;
        public bool is_connected = false;

        Thread InstanceCaller;

        public void Start()
        {
            // устанавливаем IP-адрес сервера и номер порта
            server = new TcpListener(IPAddress.Any, 5573);
            server.Start();  // запускаем сервер

            flag_run_server = true;
            InstanceCaller = new Thread(new ThreadStart(ServerLoop));
            InstanceCaller.Start();  // Start the thread.
        }

        public void Dispose()
        {
            flag_run_server = false;
            if (server != null)
                server.Stop();
        }

        public void Write(string s)
        {
            lock (tcp_2send)
                tcp_2send.Add(s);
        }

        public List<string> GetPackages(string s) //отдать содержимое и удалить пакеты, начинающиеся со строки
        {
            lock (packages)
            {
                var output = packages.Where(x => x.StartsWith(s)).Select(x => x.Substring(s.Length)).ToList();
                packages.RemoveAll(x => x.StartsWith(s));
                return output;
            }
        }

        private void ServerLoop()
        {
            //СЕРВЕР:
            while (flag_run_server)   // бесконечный цикл обслуживания клиентов
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();  // ожидаем подключение клиента
                    Console.WriteLine("Клиент подключился");
                    NetworkStream ns = client.GetStream(); // для получения и отправки сообщений
                    byte[] tcp_message; // = new byte[100];   // любое сообщение должно быть сериализовано
                    tcp_message = Encoding.Default.GetBytes("<Server is ready>\r\n");  // конвертируем строку в массив байт
                    ns.Write(tcp_message, 0, tcp_message.Length);     // отправляем сообщение
                    Console.WriteLine("tcp port => {0}", "<Server is ready>");
                    while (client.Connected && flag_run_server)  // пока клиент подключен, и пока не пора закругляться
                    {
                        is_connected = true;

                        //if (!IsConnected(client.Client)) //тут висим, пока что-то не придет в порт
                        //    break;

                        lock (tcp_2send)
                        {
                            foreach (var s in tcp_2send)
                            {
                                tcp_message = Encoding.Default.GetBytes(s + "\r\n");
                                ns.Write(tcp_message, 0, tcp_message.Length);     // отправляем сообщение
                                Console.WriteLine("tcp port => {0}", s);
                            }
                            tcp_2send.Clear();
                        }

                        lock (port_output)
                        {
                            byte[] myReadBuffer = new byte[1024];
                            int numberOfBytesRead = 0;

                            while (ns.DataAvailable)
                            {
                                numberOfBytesRead = ns.Read(myReadBuffer, 0, myReadBuffer.Length);  //тут висим, пока что-то не придет в порт
                                port_output += Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead);
                            }
                        }

                        lock (packages)
                        {
                            packages.AddRange(CommandConverter.Extract_all_packages(ref port_output));
                            if (GetPackages("exit").Count > 0)
                            {
                                is_connected = false;
                                Console.WriteLine("Получили <exit>");
                                break;
                            }
                        }
                    }
                    Console.WriteLine("Клиент отключился"); // выводим на экран полученное сообщение в виде строки
                    is_connected = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        /*
                static bool IsConnected(Socket _nSocket) //https://social.msdn.microsoft.com/Forums/en-US/c857cad5-2eb6-4b6c-b0b5-7f4ce320c5cd/c-how-to-determine-if-a-tcpclient-has-been-disconnected?forum=netfxnetcom
                {
                    if (_nSocket.Connected)
                    {
                        if ((_nSocket.Poll(0, SelectMode.SelectWrite)) && (!_nSocket.Poll(0, SelectMode.SelectError)))
                        {
                            byte[] buffer = new byte[1];
                            if (_nSocket.Receive(buffer, SocketFlags.Peek) == 0)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                */


    }
}
