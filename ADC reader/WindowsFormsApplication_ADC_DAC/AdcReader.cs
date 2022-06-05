using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;



namespace WindowsFormsApplication_ADC_DAC
{
    class AdcReader : IDisposable
    {
        //работает с 16 каналами и при частоте до 12 500 Гц

        public LusbApi_Wrapper.Module module;

        public readonly int channelsQuantity = 1;
        public readonly double adcRate_kHz = 1; //(кГц) частота работы АЦП - ДЕЛИТСЯ МЕЖДУ КАНАЛАМИ
        public readonly double adcRange = 10000; //(mВ) Максимальное входное напряжение АЦП       
        public readonly int numberOfSamples = 1048576;

        public double updateRate = 25; //25 Гц   частота вычитываения данных из кольцевого буфера, можно менять походу
        public DataContainer dataContainer;
        public Thread readingThread;
        public bool stopFlag = true;

        public bool isStarted = false;
        public bool isFinished = false;

        /// <summary>
        /// Конструктор подключается к устройству
        /// </summary>
        /// <param name="channelsQuantity"></param>
        /// <param name="adcRate_kHz"></param>
        public AdcReader(int channelsQuantity, double adcRate_kHz, int numberOfSamples)
        {
            this.channelsQuantity = channelsQuantity;
            this.adcRate_kHz = adcRate_kHz;
            this.numberOfSamples = numberOfSamples;
            //updateRate * dataStep (сколько забираем из буффера в секунду) должно быть больше чем adcRate_kHz * 1000 (сколько приходит в буффер с ацп)
            int dataStep = 2 * (int)(adcRate_kHz * 1000.0 / updateRate); //3200; //число отсчетов, вычитываемое за один раз   

            try
            {
                module = new LusbApi_Wrapper.Module(false, channelsQuantity, adcRate_kHz, adcRange, dataStep);
            }
            catch
            {
                Console.WriteLine("Error: no device found");
                Environment.Exit(0);
            }
        }

        public void Start()
        {
            if (isStarted && readingThread != null && readingThread.IsAlive) //если цикл чтения работает, то остановим его
                Stop();

            dataContainer = new DataContainer(channelsQuantity, module.deltaT, this.numberOfSamples);

            stopFlag = false;
            readingThread = new Thread(new ThreadStart(ReadingLoop));
            readingThread.Name = "ReadingLoop";
            //generationThread.IsBackground = true;
            readingThread.Start();
            isStarted = true;
        }
        public void Stop() //todo защита от нажатия когда уже всё стоит
        {
            if (!isStarted)
                return;
            stopFlag = true;
            //подождем удвоенное время цикла чтения пока остановится ReadingLoop //todo переделать на ожидание завершения потока
            readingThread.Join();
            //Thread.Sleep((int)(1000 / updateRate) * 2);
        }

        private void ReadingLoop()
        {
            dataContainer.t0 = DateTime.Now;
            while (!stopFlag)
            {
                double[] d = module.ReadOutVoltageArray();
                if (d.Length > 0)
                {
                    lock (dataContainer)
                        dataContainer.AddDataInterleaved(d);
                    if (dataContainer.isFull)
                    {
                        isFinished = true;
                        break;
                    }
                }
                //Thread.Sleep((int)(1000.0 / updateRate));
            }
            isStarted = false;
        }


        public void WriteToFile(string filePath)
        {
            if (dataContainer == null)
                return;
            dataContainer.WriteToNewFile(filePath);
        }

        public void Dispose()
        {
            if (isStarted)
                Stop();
            module.Dispose();
        }
    }
}
