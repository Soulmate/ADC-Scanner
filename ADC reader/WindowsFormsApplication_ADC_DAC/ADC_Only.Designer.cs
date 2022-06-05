
namespace WindowsFormsApplication_ADC_DAC
{
    partial class ADC_Only
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBox_savePath = new System.Windows.Forms.TextBox();
            this.Button_Browse = new System.Windows.Forms.Button();
            this.button_ADCStart = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox_Log = new System.Windows.Forms.TextBox();
            this.button_ADCStop = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.numericUpDown_ch = new System.Windows.Forms.NumericUpDown();
            this.button_init = new System.Windows.Forms.Button();
            this.numericUpDown_freq = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_save = new System.Windows.Forms.Button();
            this.numericUpDown_numberOfSamples = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label_info = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_ch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_freq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_numberOfSamples)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_savePath
            // 
            this.textBox_savePath.Location = new System.Drawing.Point(97, 213);
            this.textBox_savePath.Name = "textBox_savePath";
            this.textBox_savePath.Size = new System.Drawing.Size(241, 20);
            this.textBox_savePath.TabIndex = 0;
            this.textBox_savePath.Text = "C:\\Users\\Soulmate\\Desktop\\_temp\\1";
            // 
            // Button_Browse
            // 
            this.Button_Browse.Location = new System.Drawing.Point(344, 211);
            this.Button_Browse.Name = "Button_Browse";
            this.Button_Browse.Size = new System.Drawing.Size(24, 23);
            this.Button_Browse.TabIndex = 1;
            this.Button_Browse.Text = "...";
            this.Button_Browse.UseVisualStyleBackColor = true;
            this.Button_Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // button_ADCStart
            // 
            this.button_ADCStart.Location = new System.Drawing.Point(16, 173);
            this.button_ADCStart.Name = "button_ADCStart";
            this.button_ADCStart.Size = new System.Drawing.Size(75, 23);
            this.button_ADCStart.TabIndex = 1;
            this.button_ADCStart.Text = "Start";
            this.button_ADCStart.UseVisualStyleBackColor = true;
            this.button_ADCStart.Click += new System.EventHandler(this.button_ADCStart_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox_Log
            // 
            this.textBox_Log.Location = new System.Drawing.Point(14, 251);
            this.textBox_Log.Multiline = true;
            this.textBox_Log.Name = "textBox_Log";
            this.textBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Log.Size = new System.Drawing.Size(217, 259);
            this.textBox_Log.TabIndex = 3;
            // 
            // button_ADCStop
            // 
            this.button_ADCStop.Location = new System.Drawing.Point(97, 173);
            this.button_ADCStop.Name = "button_ADCStop";
            this.button_ADCStop.Size = new System.Drawing.Size(75, 23);
            this.button_ADCStop.TabIndex = 1;
            this.button_ADCStop.Text = "Stop";
            this.button_ADCStop.UseVisualStyleBackColor = true;
            this.button_ADCStop.Click += new System.EventHandler(this.button_ADCStop_Click);
            // 
            // numericUpDown_ch
            // 
            this.numericUpDown_ch.Location = new System.Drawing.Point(168, 11);
            this.numericUpDown_ch.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDown_ch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_ch.Name = "numericUpDown_ch";
            this.numericUpDown_ch.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown_ch.TabIndex = 4;
            this.numericUpDown_ch.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_ch.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // button_init
            // 
            this.button_init.Location = new System.Drawing.Point(104, 140);
            this.button_init.Name = "button_init";
            this.button_init.Size = new System.Drawing.Size(75, 23);
            this.button_init.TabIndex = 5;
            this.button_init.Text = "Init";
            this.button_init.UseVisualStyleBackColor = true;
            this.button_init.Click += new System.EventHandler(this.button_init_Click);
            // 
            // numericUpDown_freq
            // 
            this.numericUpDown_freq.Location = new System.Drawing.Point(168, 37);
            this.numericUpDown_freq.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown_freq.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_freq.Name = "numericUpDown_freq";
            this.numericUpDown_freq.Size = new System.Drawing.Size(76, 20);
            this.numericUpDown_freq.TabIndex = 4;
            this.numericUpDown_freq.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDown_freq.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Число каналов";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Частота (Гц)";
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(16, 211);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 7;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // numericUpDown_numberOfSamples
            // 
            this.numericUpDown_numberOfSamples.Location = new System.Drawing.Point(168, 63);
            this.numericUpDown_numberOfSamples.Maximum = new decimal(new int[] {
            2146435071,
            0,
            0,
            0});
            this.numericUpDown_numberOfSamples.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_numberOfSamples.Name = "numericUpDown_numberOfSamples";
            this.numericUpDown_numberOfSamples.Size = new System.Drawing.Size(76, 20);
            this.numericUpDown_numberOfSamples.TabIndex = 4;
            this.numericUpDown_numberOfSamples.Value = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.numericUpDown_numberOfSamples.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Число отсчетов по времени";
            // 
            // label_info
            // 
            this.label_info.AutoSize = true;
            this.label_info.Location = new System.Drawing.Point(16, 98);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(25, 13);
            this.label_info.TabIndex = 8;
            this.label_info.Text = "Info";
            // 
            // ADC_Only
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 532);
            this.Controls.Add(this.label_info);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_init);
            this.Controls.Add(this.numericUpDown_numberOfSamples);
            this.Controls.Add(this.numericUpDown_freq);
            this.Controls.Add(this.numericUpDown_ch);
            this.Controls.Add(this.textBox_Log);
            this.Controls.Add(this.button_ADCStop);
            this.Controls.Add(this.button_ADCStart);
            this.Controls.Add(this.Button_Browse);
            this.Controls.Add(this.textBox_savePath);
            this.Name = "ADC_Only";
            this.Text = "ADC_Only";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_ch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_freq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_numberOfSamples)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_savePath;
        private System.Windows.Forms.Button Button_Browse;
        private System.Windows.Forms.Button button_ADCStart;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox_Log;
        private System.Windows.Forms.Button button_ADCStop;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.NumericUpDown numericUpDown_ch;
        private System.Windows.Forms.Button button_init;
        private System.Windows.Forms.NumericUpDown numericUpDown_freq;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.NumericUpDown numericUpDown_numberOfSamples;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_info;
    }
}