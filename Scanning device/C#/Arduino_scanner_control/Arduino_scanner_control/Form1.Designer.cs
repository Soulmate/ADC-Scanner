namespace Arduino_scanner_control
{
    partial class Form1
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
            this.button_home = new System.Windows.Forms.Button();
            this.button_stop = new System.Windows.Forms.Button();
            this.button_move_to = new System.Windows.Forms.Button();
            this.numericUpDown_move_to = new System.Windows.Forms.NumericUpDown();
            this.textBox_port_output = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox_current_pos_steps = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_current_state = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_min_pos_steps = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_max_pos_steps = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_step_per_mm = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.button_move_plus = new System.Windows.Forms.Button();
            this.button_move_minus = new System.Windows.Forms.Button();
            this.numericUpDown_move_by = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_min_pos_mm = new System.Windows.Forms.TextBox();
            this.textBox_max_pos_mm = new System.Windows.Forms.TextBox();
            this.textBox_current_pos_mm = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_move_to)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_move_by)).BeginInit();
            this.SuspendLayout();
            // 
            // button_home
            // 
            this.button_home.Location = new System.Drawing.Point(1, 2);
            this.button_home.Name = "button_home";
            this.button_home.Size = new System.Drawing.Size(75, 23);
            this.button_home.TabIndex = 2;
            this.button_home.Text = "Home";
            this.button_home.UseVisualStyleBackColor = true;
            this.button_home.Click += new System.EventHandler(this.button_home_Click);
            // 
            // button_stop
            // 
            this.button_stop.Location = new System.Drawing.Point(1, 31);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(75, 23);
            this.button_stop.TabIndex = 2;
            this.button_stop.Text = "Stop";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // button_move_to
            // 
            this.button_move_to.Location = new System.Drawing.Point(1, 60);
            this.button_move_to.Name = "button_move_to";
            this.button_move_to.Size = new System.Drawing.Size(75, 23);
            this.button_move_to.TabIndex = 2;
            this.button_move_to.Text = "Move to";
            this.button_move_to.UseVisualStyleBackColor = true;
            this.button_move_to.Click += new System.EventHandler(this.button_move_Click);
            // 
            // numericUpDown_move_to
            // 
            this.numericUpDown_move_to.DecimalPlaces = 2;
            this.numericUpDown_move_to.Location = new System.Drawing.Point(82, 63);
            this.numericUpDown_move_to.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown_move_to.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.numericUpDown_move_to.Name = "numericUpDown_move_to";
            this.numericUpDown_move_to.Size = new System.Drawing.Size(100, 20);
            this.numericUpDown_move_to.TabIndex = 3;
            this.numericUpDown_move_to.ValueChanged += new System.EventHandler(this.numericUpDown_move_to_ValueChanged);
            // 
            // textBox_port_output
            // 
            this.textBox_port_output.Location = new System.Drawing.Point(217, 4);
            this.textBox_port_output.Multiline = true;
            this.textBox_port_output.Name = "textBox_port_output";
            this.textBox_port_output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_port_output.Size = new System.Drawing.Size(222, 145);
            this.textBox_port_output.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox_current_pos_steps
            // 
            this.textBox_current_pos_steps.Location = new System.Drawing.Point(86, 174);
            this.textBox_current_pos_steps.Name = "textBox_current_pos_steps";
            this.textBox_current_pos_steps.ReadOnly = true;
            this.textBox_current_pos_steps.Size = new System.Drawing.Size(65, 20);
            this.textBox_current_pos_steps.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 177);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Curernt pos";
            // 
            // textBox_current_state
            // 
            this.textBox_current_state.Location = new System.Drawing.Point(86, 148);
            this.textBox_current_state.Name = "textBox_current_state";
            this.textBox_current_state.ReadOnly = true;
            this.textBox_current_state.Size = new System.Drawing.Size(100, 20);
            this.textBox_current_state.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Curernt state";
            // 
            // textBox_min_pos_steps
            // 
            this.textBox_min_pos_steps.Location = new System.Drawing.Point(86, 200);
            this.textBox_min_pos_steps.Name = "textBox_min_pos_steps";
            this.textBox_min_pos_steps.ReadOnly = true;
            this.textBox_min_pos_steps.Size = new System.Drawing.Size(65, 20);
            this.textBox_min_pos_steps.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Min pos";
            // 
            // textBox_max_pos_steps
            // 
            this.textBox_max_pos_steps.Location = new System.Drawing.Point(86, 226);
            this.textBox_max_pos_steps.Name = "textBox_max_pos_steps";
            this.textBox_max_pos_steps.ReadOnly = true;
            this.textBox_max_pos_steps.Size = new System.Drawing.Size(65, 20);
            this.textBox_max_pos_steps.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 229);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Max pos";
            // 
            // textBox_step_per_mm
            // 
            this.textBox_step_per_mm.Location = new System.Drawing.Point(86, 252);
            this.textBox_step_per_mm.Name = "textBox_step_per_mm";
            this.textBox_step_per_mm.ReadOnly = true;
            this.textBox_step_per_mm.Size = new System.Drawing.Size(100, 20);
            this.textBox_step_per_mm.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 255);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Step per mm";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(157, 177);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "steps";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(157, 203);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "steps";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(157, 229);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "steps";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(192, 255);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "steps";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(188, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "mm";
            // 
            // button_move_plus
            // 
            this.button_move_plus.Location = new System.Drawing.Point(1, 89);
            this.button_move_plus.Name = "button_move_plus";
            this.button_move_plus.Size = new System.Drawing.Size(75, 23);
            this.button_move_plus.TabIndex = 2;
            this.button_move_plus.Text = "Move plus";
            this.button_move_plus.UseVisualStyleBackColor = true;
            this.button_move_plus.Click += new System.EventHandler(this.button_move_up_Click);
            // 
            // button_move_minus
            // 
            this.button_move_minus.Location = new System.Drawing.Point(1, 118);
            this.button_move_minus.Name = "button_move_minus";
            this.button_move_minus.Size = new System.Drawing.Size(75, 23);
            this.button_move_minus.TabIndex = 2;
            this.button_move_minus.Text = "Move minus";
            this.button_move_minus.UseVisualStyleBackColor = true;
            this.button_move_minus.Click += new System.EventHandler(this.button_move_down_Click);
            // 
            // numericUpDown_move_by
            // 
            this.numericUpDown_move_by.DecimalPlaces = 2;
            this.numericUpDown_move_by.Location = new System.Drawing.Point(82, 102);
            this.numericUpDown_move_by.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown_move_by.Name = "numericUpDown_move_by";
            this.numericUpDown_move_by.Size = new System.Drawing.Size(100, 20);
            this.numericUpDown_move_by.TabIndex = 3;
            this.numericUpDown_move_by.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_move_by.ValueChanged += new System.EventHandler(this.numericUpDown_move_by_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(188, 104);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(23, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "mm";
            // 
            // textBox_min_pos_mm
            // 
            this.textBox_min_pos_mm.Location = new System.Drawing.Point(195, 200);
            this.textBox_min_pos_mm.Name = "textBox_min_pos_mm";
            this.textBox_min_pos_mm.ReadOnly = true;
            this.textBox_min_pos_mm.Size = new System.Drawing.Size(65, 20);
            this.textBox_min_pos_mm.TabIndex = 1;
            // 
            // textBox_max_pos_mm
            // 
            this.textBox_max_pos_mm.Location = new System.Drawing.Point(195, 226);
            this.textBox_max_pos_mm.Name = "textBox_max_pos_mm";
            this.textBox_max_pos_mm.ReadOnly = true;
            this.textBox_max_pos_mm.Size = new System.Drawing.Size(65, 20);
            this.textBox_max_pos_mm.TabIndex = 1;
            // 
            // textBox_current_pos_mm
            // 
            this.textBox_current_pos_mm.Location = new System.Drawing.Point(195, 174);
            this.textBox_current_pos_mm.Name = "textBox_current_pos_mm";
            this.textBox_current_pos_mm.ReadOnly = true;
            this.textBox_current_pos_mm.Size = new System.Drawing.Size(65, 20);
            this.textBox_current_pos_mm.TabIndex = 4;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(266, 177);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 13);
            this.label12.TabIndex = 5;
            this.label12.Text = "mm";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(266, 203);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(23, 13);
            this.label13.TabIndex = 5;
            this.label13.Text = "mm";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(266, 229);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(23, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "mm";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 284);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_current_pos_mm);
            this.Controls.Add(this.textBox_current_pos_steps);
            this.Controls.Add(this.numericUpDown_move_by);
            this.Controls.Add(this.numericUpDown_move_to);
            this.Controls.Add(this.button_move_minus);
            this.Controls.Add(this.button_move_plus);
            this.Controls.Add(this.button_move_to);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.button_home);
            this.Controls.Add(this.textBox_port_output);
            this.Controls.Add(this.textBox_step_per_mm);
            this.Controls.Add(this.textBox_max_pos_mm);
            this.Controls.Add(this.textBox_min_pos_mm);
            this.Controls.Add(this.textBox_max_pos_steps);
            this.Controls.Add(this.textBox_min_pos_steps);
            this.Controls.Add(this.textBox_current_state);
            this.Name = "Form1";
            this.Text = "Scanner";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_move_to)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_move_by)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_home;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Button button_move_to;
        private System.Windows.Forms.NumericUpDown numericUpDown_move_to;
        private System.Windows.Forms.TextBox textBox_port_output;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox_current_pos_steps;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_current_state;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_min_pos_steps;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_max_pos_steps;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_step_per_mm;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button_move_plus;
        private System.Windows.Forms.Button button_move_minus;
        private System.Windows.Forms.NumericUpDown numericUpDown_move_by;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_min_pos_mm;
        private System.Windows.Forms.TextBox textBox_max_pos_mm;
        private System.Windows.Forms.TextBox textBox_current_pos_mm;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
    }
}

