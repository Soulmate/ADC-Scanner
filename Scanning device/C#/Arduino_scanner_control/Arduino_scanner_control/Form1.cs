using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Text.RegularExpressions;

using System.Threading;
using System.Globalization;

namespace Arduino_scanner_control
{
    public partial class Form1 : Form
    {
        Serial_connection serial_connection = new Serial_connection();
        TCP_connection tcp_connection = new TCP_connection();

        int? current_pos = null;
        int? target_pos_steps = null;


        double scanner_MIN_STEPS = 0;
        double scanner_MAX_STEPS = 0;
        double scanner_STEPS_PER_MM = 1;
        double scanner_MIN_mm = 0;
        double scanner_MAX_mm = 0;

        enum state
        {
            invalid,        // 0 не инициализирован, позиция неизвестра
            is_ready,       // 1 стоит, позиция известра, готов к дввижению
            is_moving,      // 2 движется
            is_stopping,    // 3 останавливается, прерывая движение
            is_going_home,  // 4 поиск концевика
        };
        state current_state = state.invalid;

        public Form1()
        {
            InitializeComponent();

            tcp_connection.Start();

            button_Connect_Click(this, new EventArgs());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!serial_connection.is_connected)
            {
                textBox_port_output.Text = "Устройство не подключено";
                return;
            }



            var pos_packages = serial_connection.GetPackages("pos:");
            if (pos_packages.Any())
            {
                current_pos = int.Parse(pos_packages.Last());
                textBox_current_pos_steps.Text = current_pos.ToString();
                textBox_current_pos_mm.Text = (current_pos / scanner_STEPS_PER_MM).ToString();
                //SendToTCPClient("pos " + current_pos.ToString());
            }

            var state_packages = serial_connection.GetPackages("state:");
            if (state_packages.Any())
            {
                current_state = (state)int.Parse(state_packages.Last());
                textBox_current_state.Text = current_state.ToString();
            }

            var err_packages = serial_connection.GetPackages("err:");
            if (err_packages.Any())
            {
                if (err_packages.Contains("1"))
                    textBox_port_output.Text = textBox_port_output.Text + "Неверный параметр команды\r\n";
                if (err_packages.Contains("1"))
                    textBox_port_output.Text = textBox_port_output.Text + "Неизвестная инструкция\r\n";
                if (err_packages.Contains("16"))
                    textBox_port_output.Text = textBox_port_output.Text + "Выход за границу\r\n";
                if (err_packages.Contains("17"))
                    textBox_port_output.Text = textBox_port_output.Text + "Устройство не готово\r\n";
            }

            serial_connection.Write("<gp>");



            if (tcp_connection.is_connected)
            {
                List<string> packages;
                packages = tcp_connection.GetPackages("m:");
                if (packages.Any())
                {
                    Console.WriteLine(packages.Last());
                    double pos = double.Parse(packages.Last().Replace('.', ',')); //можно и точки и запятые
                    if (pos >= scanner_MIN_mm && pos <= scanner_MAX_mm)
                    {
                        numericUpDown_move_to.Value = (decimal)pos;
                        button_move_Click(this, new EventArgs());
                    }
                    else
                    {
                        tcp_connection.Write($"<error:out_of_bounds>");
                    }
                }
                packages = tcp_connection.GetPackages("h");
                if (packages.Any())
                {
                    button_home_Click(this, new EventArgs());
                }

                //все стейты пересылаем в tcp
                foreach (var p in state_packages)
                {
                    state s = (state)int.Parse(p);
                    tcp_connection.Write($"<status:{s}>");
                }
            }
        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            if (serial_connection.is_connected)
                serial_connection.Dispose();
            serial_connection.Connect();
            if (serial_connection.is_connected)
            {
                textBox_port_output.Text = "Устройство подключено\r\n";
                textBox_port_output.Text += serial_connection.info_str;
                try
                {
                    Match m1 = Regex.Match(serial_connection.info_str, "(MIN_STEPS:)(.*?),");
                    Match m2 = Regex.Match(serial_connection.info_str, "(MAX_STEPS:)(.*?),");
                    Match m3 = Regex.Match(serial_connection.info_str, "(STEPS_PER_MM:)(.*?)$");
                    scanner_MIN_STEPS = int.Parse(m1.Groups[2].Value);
                    scanner_MAX_STEPS = int.Parse(m2.Groups[2].Value);
                    scanner_STEPS_PER_MM = (double)Convert.ToDecimal(m3.Groups[2].Value, new CultureInfo("en-US")); //чтобы точка нормально воспринималась
                    if (scanner_STEPS_PER_MM > 0)
                    {
                        scanner_MIN_mm = scanner_MIN_STEPS / scanner_STEPS_PER_MM;
                        scanner_MAX_mm = scanner_MAX_STEPS / scanner_STEPS_PER_MM;
                    }
                    else
                    {
                        scanner_MIN_mm = scanner_MAX_STEPS / scanner_STEPS_PER_MM;
                        scanner_MAX_mm = scanner_MIN_STEPS / scanner_STEPS_PER_MM;
                    }


                    textBox_min_pos_steps.Text = scanner_MIN_STEPS.ToString();
                    textBox_max_pos_steps.Text = scanner_MAX_STEPS.ToString();
                    textBox_step_per_mm.Text = scanner_STEPS_PER_MM.ToString();

                    numericUpDown_move_to.Minimum = (decimal)scanner_MIN_mm;
                    numericUpDown_move_to.Maximum = (decimal)scanner_MAX_mm;
                    textBox_min_pos_mm.Text = scanner_MIN_mm.ToString();
                    textBox_max_pos_mm.Text = scanner_MAX_mm.ToString();

                    target_pos_steps = 0;
                }
                catch
                {
                    MessageBox.Show("Ошибка чтения параметров сканирующего устройства");
                }
            }
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            serial_connection.Dispose();
            tcp_connection.Dispose();
        }

        private void button_home_Click(object sender, EventArgs e)
        {
            serial_connection.Write("<h>");
            target_pos_steps = 0;
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            serial_connection.Write("<s>");
        }

        private void button_move_Click(object sender, EventArgs e)
        {
            target_pos_steps = (int)Math.Round((double)numericUpDown_move_to.Value * scanner_STEPS_PER_MM);
            serial_connection.Write($"<m {target_pos_steps}>");
        }



        private void numericUpDown_move_to_ValueChanged(object sender, EventArgs e)
        {
            int steps = (int)Math.Round((double)numericUpDown_move_to.Value * scanner_STEPS_PER_MM);
            numericUpDown_move_to.Value = (decimal)(steps / scanner_STEPS_PER_MM);
        }

        private void numericUpDown_move_by_ValueChanged(object sender, EventArgs e)
        {
            int steps = (int)Math.Round((double)numericUpDown_move_by.Value * scanner_STEPS_PER_MM);
            numericUpDown_move_by.Value = (decimal)(steps / scanner_STEPS_PER_MM);
        }

        private void button_move_up_Click(object sender, EventArgs e)
        {
            if (target_pos_steps.HasValue)
            {
                int target_pos_steps_new = target_pos_steps.Value + (int)Math.Round((double)numericUpDown_move_by.Value * scanner_STEPS_PER_MM);
                if (target_pos_steps_new <= scanner_MAX_STEPS && target_pos_steps_new >= scanner_MIN_STEPS)
                {
                    target_pos_steps = target_pos_steps_new;
                    serial_connection.Write($"<m {target_pos_steps}>");
                }
            }
        }

        private void button_move_down_Click(object sender, EventArgs e)
        {
            if (target_pos_steps.HasValue)
            {
                int target_pos_steps_new = target_pos_steps.Value - +(int)Math.Round((double)numericUpDown_move_by.Value * scanner_STEPS_PER_MM);
                if (target_pos_steps_new <= scanner_MAX_STEPS && target_pos_steps_new >= scanner_MIN_STEPS)
                {
                    target_pos_steps = target_pos_steps_new;
                    serial_connection.Write($"<m {target_pos_steps}>");
                }
            }
        }
    }
}