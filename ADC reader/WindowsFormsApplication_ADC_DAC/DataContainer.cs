using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace WindowsFormsApplication_ADC_DAC
{/// <summary>
/// класс для хранения эквидистантных многоканальных данных double
/// </summary>
    public class DataContainer
    {
        public static string version = "v1.0";
        public double[,] Data { get => _data; } //данные
        public DateTime t0 = DateTime.Now; //время начала записи. После изменения сделать WriteInfoFile, оно сохраняется в нем
        public double deltaT; //время между отсчетами
        public readonly int channelsQuantity; //число каналов
        public readonly int numberOfSamples; //размер данных по числу отсчетов для каждого канала        
        public readonly string[] channelNames; //имена каналов               
        double[,] _data; //время, канал
        int currentTimeIndex = 0; //индекс по времени начиная с которого пишутся новые данные
        //int timeIndexToWriteToFile = 0; //индекс по времени начиная с которого данные будут сохраняться в файл
        public bool isFull = false; //полностью заполнен

        public DataContainer(
            int channelsQuantity,
            double deltaT,
            int numberOfSamples,
            //int sizeMB = 1,
            string[] channelNames = null)
        {
            this.channelsQuantity = channelsQuantity;

            this.numberOfSamples = numberOfSamples; 
            _data = new double[this.numberOfSamples, channelsQuantity];
            this.deltaT = deltaT;

            if (channelNames == null)
            {
                channelNames = new string[channelsQuantity];
                for (int ch_i = 0; ch_i < channelsQuantity; ch_i++)
                    channelNames[ch_i] = $"Ch {ch_i}";
            }
            else if (channelNames.Length != channelsQuantity)
                throw new Exception("DataContainer channelNames count error");
            this.channelNames = channelNames;
        }

        /// <summary>
        /// Добавить данные в виде ch1 ch2 ch3 ch4 ch1 ch2 ch3 ch4 ch1 ...
        /// </summary>
        /// <param name="d"></param>
        public void AddDataInterleaved(double[] d)
        {
            if (d.Length % channelsQuantity != 0)
                throw new Exception("AddDataInterleaved data size error");
            int dataTimeLength = d.Length / channelsQuantity;

            if (currentTimeIndex + dataTimeLength >= numberOfSamples) //если не хватает места записать в буффер
            {
                Console.WriteLine($"Buffer is full");
                isFull = true;
                dataTimeLength = numberOfSamples - currentTimeIndex;
            }

            for (int time_i = 0; time_i < dataTimeLength; time_i++)
                for (int ch_i = 0; ch_i < channelsQuantity; ch_i++)
                    _data[currentTimeIndex + time_i, ch_i] = d[time_i * channelsQuantity + ch_i];

            currentTimeIndex += dataTimeLength;
        }

        /// <summary>
        /// Дописать данные в файл
        /// </summary>
        public void WriteToNewFile(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            //инфо файл
            using (StreamWriter sr = File.CreateText(filePath + ".adc_info"))
            {
                sr.WriteLine($"E14-140 ADC_DAC ver {DataContainer.version}");
                sr.WriteLine(DateTime.Now);
                sr.WriteLine($"channelsQuantity {channelsQuantity}");
                sr.WriteLine($"deltaT {deltaT}");
                sr.WriteLine("channelNames:");
                foreach (var chn in channelNames)
                    sr.WriteLine(chn);
            }

            //основной файл с данными
            //using (StreamWriter sr = File.AppendText(filePath)) //todo заменить на бинарные файлы
            using (StreamWriter sr = File.CreateText(filePath)) //todo заменить на бинарные файлы
            {
                //for (int time_i = timeIndexToWriteToFile; time_i < currentTimeIndex; time_i++)
                for (int time_i = 0; time_i < currentTimeIndex; time_i++)
                {
                    double time = deltaT * time_i;
                    sr.Write(time.ToString() + "\t");
                    for (int ch_i = 0; ch_i < channelsQuantity; ch_i++)
                    {
                        sr.Write(_data[time_i, ch_i].ToString());
                        if (ch_i < channelsQuantity - 1)
                            sr.Write("\t");
                        else
                            sr.Write("\r\n");
                    }
                }
            }
            //timeIndexToWriteToFile = currentTimeIndex;
        }


        public double[] GetLastDataValues()
        {
            if (currentTimeIndex - 1 < 0)
                return new double[channelsQuantity];
            return Enumerable.Range(0, channelsQuantity)
                .Select(ch => _data[currentTimeIndex - 1, ch])
                .ToArray();
        }

        public double[,] GetValuesTimeInterval(double t0, double t1) //данные только из интервала по времени
        {
            int t0_i = (int)(t0 / deltaT);
            int t1_i = (int)(t1 / deltaT);
            t0_i = Math.Max(0, t0_i);
            t1_i = Math.Min(currentTimeIndex, t0_i);
            if (t1_i <= t0_i)
                return null;

            var d = new double[t1_i-t0_i, channelsQuantity];
            for (int i = t0_i; i < t1_i; i++)
                for (int ch = 0; i < channelsQuantity; ch++)
                    d[i, ch] = _data[i + t0_i, ch];
            return d;
        }

        public double GetLastTimeSec()
        {
            double lastTime = (currentTimeIndex - 1) * deltaT;
            return lastTime;
        }

        //public GraphData_dubleArray GetLastChannelData(int ch, double duration)
        //{
        //    if (ch < 0 || ch >= channelsQuantity)
        //        throw new Exception("ch number error");

        //    int start_i = (int)(currentTimeIndex - duration / deltaT);
        //    var gd = new GraphData_dubleArray(channelNames[ch], start_i * deltaT, deltaT);
        //    var d = Enumerable.Range(start_i, currentTimeIndex)
        //        .Select(i => _data[i, ch])
        //        .ToList();
        //    gd.Add(d);
        //    return gd;
        //}
    }
}
