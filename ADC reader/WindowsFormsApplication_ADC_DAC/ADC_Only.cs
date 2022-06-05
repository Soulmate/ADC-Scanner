using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication_ADC_DAC
{
    public partial class ADC_Only : Form
    {
        //public GrapherControl grapherControl1;


        public ADC_Only()
        {
            InitializeComponent();

            if (Program.automation.autoStart)
            {
                textBox_savePath.Text = Program.automation.savePath;
                numericUpDown_freq.Value = Program.automation.freq_Hz;
                numericUpDown_ch.Value = Program.automation.channels;
                numericUpDown_numberOfSamples.Value = Program.automation.numberOfSamples;
                textBox_savePath.Enabled = false;
                textBox_savePath.Enabled = false;
                numericUpDown_freq.Enabled = false;
                numericUpDown_ch.Enabled = false;
                numericUpDown_numberOfSamples.Enabled = false;
                button_init.Enabled = false;
                button_ADCStart.Enabled = false;
                button_ADCStop.Enabled = false;
                button_save.Enabled = false;
            }

            UpdateInfo();

            //if (Program.automation.autoStartSequence)
            //    this.WindowState = FormWindowState.Minimized;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Program.adcReader == null)
                return;

            Program.automation.RunAutomationLoop();

            textBox_Log.Text = "";

            if (Program.adcReader.isStarted)
            {
                var dc = Program.adcReader.dataContainer;
                lock (dc)
                {
                    textBox_Log.Text = "";
                    textBox_Log.Text += $"{dc.GetLastTimeSec():F2} с\r\n";
                    var values = dc.GetLastDataValues();
                    for (int i = 0; i < dc.channelsQuantity; i++)
                        textBox_Log.Text += $"{dc.channelNames[i]}: {values[i]:F2}\r\n";
                }
            }
            else
                textBox_Log.Text += "Stop\r\n";

            if (Program.automation.autoStart)
                textBox_Log.Text += "autoStart\r\n";
            if (Program.automation.autoSaveAtFinish)
                textBox_Log.Text += "autoSaveAtFinish\r\n";
        }

        private void button_init_Click(object sender, EventArgs e)
        {
            if (Program.adcReader != null)
                Program.adcReader.Dispose();            

            int ch = (int)numericUpDown_ch.Value;
            double adcRate_kHz = (double)numericUpDown_freq.Value * ch / 1000.0;
            int numberOfSamples = (int)numericUpDown_numberOfSamples.Value;
            Program.adcReader = new AdcReader(ch, adcRate_kHz, numberOfSamples);
        }

        private void button_ADCStart_Click(object sender, EventArgs e)
        {
            if (Program.adcReader == null)
                return;


            Program.adcReader.Start();
        }

        private void button_ADCStop_Click(object sender, EventArgs e)
        {
            if (Program.adcReader == null)
                return;
            Program.adcReader.Stop();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            string savePath = textBox_savePath.Text;
            if (savePath != "")
                saveFileDialog1.FileName = savePath;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                savePath = saveFileDialog1.FileName;
                textBox_savePath.Text = savePath;

            }
        }


        private void button_save_Click(object sender, EventArgs e)
        {
            if (Program.adcReader == null)
                return;
            Program.adcReader.WriteToFile(textBox_savePath.Text);
        }

        private void numericUpDownValueChanged(object sender, EventArgs e)
        {
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            int ch = (int)numericUpDown_ch.Value;
            double adcRate_kHz = (double)numericUpDown_freq.Value / 1000.0;
            int numberOfSamples = (int)numericUpDown_numberOfSamples.Value;

            double rec_time__s = numberOfSamples / adcRate_kHz / 1000.0;
            double memory__mb = numberOfSamples * ch / 1024.0 / 1024.0;

            label_info.Text = $"{rec_time__s:F1} c, {memory__mb:F2} Мб памяти";
        }
    }
}
