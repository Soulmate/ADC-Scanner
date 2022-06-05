using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication_ADC_DAC
{
    class Automation
    {
        public bool autoStart = false;
        public bool autoSaveAtFinish = false;

        public DateTime timeStart;

        public int channels = 1;
        public int freq_Hz = 1;
        public int numberOfSamples = 100;
        public string savePath = null;


        public void Start() //вызывается при старте
        {
            if (autoStart)
            {
                timeStart = DateTime.Now;

                try
                {                    
                    double adcRate_kHz = freq_Hz * channels / 1000.0;
                    Program.adcReader = new AdcReader(channels, adcRate_kHz, numberOfSamples);
                    Program.adcReader.Start();
                }
                catch
                {
                    Console.WriteLine("ERROR: Не удалось начать запись");
                    Application.Exit();
                }
                Console.WriteLine("OK: Запись началась");
            }
        }
        public void RunAutomationLoop() //вызывается регулярно
        {
            if (autoSaveAtFinish && Program.adcReader.isFinished)
            {
                try
                {                    
                    Program.adcReader.WriteToFile(savePath);
                }
                catch
                {
                    Console.WriteLine("ERROR: Не удалось сохранить данные");
                    Application.Exit();
                }
                Console.WriteLine("OK: Запись закончилась");
                Application.Exit();
            }
        }
    }
}
