using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;

namespace Arduino_scanner_control
{
    class Serial_connection:IDisposable
    {
        SerialPort port;
        string port_output = ""; //временная строка, куда приходит всё что приходит в порт, и из которой мы извлекаем (удаляя) пакеты
        List<string> packages = new List<string>();
        public bool is_connected = false;
        public string info_str; //строка-идентификатор, полученная с устройства

        public void Connect()
        {
            try
            {
                //TODO автоматический поиск порта
                foreach (string port_name in SerialPort.GetPortNames())
                {
                    port = new SerialPort(port_name, 115200, Parity.None, 8, StopBits.One);
                    port.DtrEnable = true;
                    port.Open();
                    port.DataReceived += Port_DataReceived;
                    Console.WriteLine("Port opened {0}", port_name);

                    //проверка что это наш сканер:
                    System.Threading.Thread.Sleep(200); //дадим ему время загрузиться
                    port.Write("<gi>");
                    for (int i = 0; i < 5; i++)
                    {
                        Console.WriteLine("Connecting... {0}", i);
                        
                        System.Threading.Thread.Sleep(200);                    // тут должен отработать Port_DataReceived
                        var info_list = GetPackages("info:");
                        if (info_list.Any())
                        {
                            info_str = info_list.Last();
                            if (info_str.StartsWith("FIRMWARE_ID:Arduino scaner v0.1"))
                            {
                                is_connected = true;
                                Console.WriteLine("Connected");
                                return;
                            }
                            else
                            {
                                Console.WriteLine("Connection error: wrong firmware (" + info_str + ")");
                                port.Close();
                                is_connected = false;
                                return;
                            }
                        }
                    }
                }
                Console.WriteLine("Device not found");
                if (port != null && port.IsOpen)
                    port.Close();
                is_connected = false;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Connection error:" + exception.Message);
                if (port != null && port.IsOpen)
                    port.Close();
                is_connected = false;
            }
        }

        public void Write(string s)
        {
            try
            {
                if (!is_connected) return;
                port.Write(s);
                //Console.WriteLine("com port => {0}", s); 
            }
            catch (Exception exception)
            {
                Console.WriteLine("Connection error:" + exception.Message);
                is_connected = false;
            }
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


        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string s = port.ReadExisting();
            port_output += s;
            lock (packages)
                packages.AddRange(CommandConverter.Extract_all_packages(ref port_output));

            //Console.WriteLine(s);
            //foreach (var p in packages)
            //    Console.WriteLine(p);
            //Console.WriteLine("====");
        }


        public void Dispose()
        {
            if (port != null && port.IsOpen)
                port.Close();
        }
    }
}
