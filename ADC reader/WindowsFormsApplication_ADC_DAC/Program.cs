using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace WindowsFormsApplication_ADC_DAC
{
    static class Program
    {

        public static AdcReader adcReader;
        public static Automation automation = new Automation();



        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [STAThread]
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            //парсинг входных парамеров
            if (args.Length == 0)
                Console.WriteLine("Запуск без параметров");
            else if (args.Length == 4) // channels, freq_Hz, numberOfSamples, savePath
            {
                int channels;
                int freq_Hz;
                int numberOfSamples;
                bool test0 = int.TryParse(args[0], out channels);
                bool test1 = int.TryParse(args[1], out freq_Hz);
                bool test2 = int.TryParse(args[2], out numberOfSamples);

                if (!test0 || !test1 || !test2)
                {
                    Console.WriteLine("Параметры не верные");
                    return;
                }
                
                automation.autoStart = true;
                automation.autoSaveAtFinish = true;
                automation.channels = channels;
                automation.freq_Hz = freq_Hz;
                automation.numberOfSamples = numberOfSamples;
                automation.savePath = args[3];
                automation.Start();
            }
            else
            {
                Console.WriteLine("Неправильное число параметров");
                return;
            }

            Application.Run(new ADC_Only());
        }
    }
}
