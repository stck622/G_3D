﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Mono
{
    public partial class Form1 : Form
    {
        Control[] gcodeControls;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gcodeControls =
            new Control[] {
                btn_center, btn_right, btn_left, btn_down, btn_up, btn_Print
            };

            btn_center.Enabled = false;
            btn_right.Enabled = false;
            btn_left.Enabled = false;
            btn_down.Enabled = false;
            btn_up.Enabled = false;
            btn_Print.Enabled = false;
            btn_Pause.Enabled = false;
            btn_Stop.Enabled = false;
            btn_application.Enabled = false;
            numericupdow_dstc.Enabled = false;
            numericupdow_spd.Enabled = false;

        }
        string path;
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string gcodes;

            openGcode.DefaultExt = "Gcode files";
            openGcode.Filter = "Gcode files (*.gcode*)|*.gcode*";
            openGcode.Multiselect = false;
            openGcode.ShowDialog();

            if (openGcode.FileName.Length > 0)
            {
                foreach (string filename in openGcode.FileNames)
                {
                    this.txtbox_file.Text = filename;
                }
            }

            path = this.txtbox_file.Text;
        }

        Thread Print_thread;
        bool Printing = false;

        
        private void Print()
        {
            string gcodes;

            try
            {
                gcodes = System.IO.File.ReadAllText(path);
            }
            catch
            {
                return;
            }

            Invoke(new MethodInvoker(delegate()
            {
                txtbox_temp.Text = gcodes;

                txtbox_temp.Text = Convert.ToString(txtbox_temp.Lines.Length);  //gcode 줄 수 세고 텍스트 박스에 띄우기
                int leng = Convert.ToInt32(txtbox_temp.Text);
                int max = gcodes.Length;                                       //max에 gcode의 길이를 저장
                progbar_ReTime.Maximum = max;                                  //프로그레스바 최대값을 gcode 줄 수로 설정            
                txtbox_temp.Text = "max : " + max.ToString() + "\n leng : " + leng.ToString();

            }));
            
            try
            {
                foreach (string line in gcodes.Replace('\r','\n').Split('\n'))
                {
                    CP2102.Write(line);
                }
            }
            catch
            {
                MessageBox.Show("현재 포트가 연결되어 있지 않습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Invoke(new MethodInvoker(delegate ()
            {
                foreach (Control ctrl in gcodeControls)
                {
                    ctrl.Enabled = true;
                }
            }));
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in gcodeControls)
            {
                ctrl.Enabled = false;
            }

            Printing = true;
            Print_thread = new Thread(new ThreadStart(Print));
            Print_thread.Start();
            /*
            for (int i = 0; i < max; i++)
            {
                progbar_ReTime.Value = i;
                //--------------------------------------------------------------------G
                if (gcodes[i] == 'G')
                {
                    char[] g_char = new char[5];

                    for (int j = i + 1; j < max; j++)
                    {
                        if (gcodes[j] == ' ' || gcodes[j] == '\r')
                        {
                            count_g++;
                            break;
                        }
                        else
                        {
                            g_char[k] = gcodes[j];
                            k++;
                        }
                    }
                    g_int = Int32.Parse(new string(g_char));                    
                    Console.Write("G" + g_int.ToString());
                    i += k + 1;
                    k = 0;
                }
                //--------------------------------------------------------------------X
                if (gcodes[i] == 'X')
                {
                    char[] x_char = new char[10];

                    for (int j = i + 1; j < max; j++)
                    {
                        if (gcodes[j] == ' ' || gcodes[j] == '\r')
                        {

                            break;
                        }
                        else
                        {
                            x_char[k] = gcodes[j];
                            k++;
                        }
                    }
                    x_float = Single.Parse(new string(x_char));
                    Console.Write(" X" + x_float.ToString());
                    i += k + 1;
                    k = 0;
                }
                //--------------------------------------------------------------------Y
                if (gcodes[i] == 'Y')
                {
                    char[] y_char = new char[10];

                    for (int j = i + 1; j < max; j++)
                    {
                        if (gcodes[j] == '\0')
                        {
                            Console.WriteLine("gcodes[j] blank");
                        }

                        if (gcodes[j] == ' ' || gcodes[j] == '\r')
                        {
                            count_y++;
                            break;
                        }
                        else
                        {
                            y_char[k] = gcodes[j];
                            k++;
                        }
                    }
                    if (y_char[0] == '\0')
                    {
                        y_char[0] = '0';
                    }

                    y_float = float.Parse(new string(y_char));
                    Console.Write(" Y" + y_float.ToString());
                    i += k + 1;
                    k = 0;
                }
                //--------------------------------------------------------------------E
                if (gcodes[i] == 'E')
                {
                    char[] e_char = new char[10];
                    for (int j = i + 1; j < max; j++)
                    {
                        if (gcodes[j] == ' ' || gcodes[j] == '\r')
                            break;
                        else
                        {
                            e_char[k] = gcodes[j];
                            k++;
                        }
                    }
                    e_float = float.Parse(new string(e_char));
                    Console.WriteLine(" E" + e_float.ToString());
                    i += k + 1;
                    k = 0;
                }
                //--------------------------------------------------------------------F
                if (gcodes[i] == 'F')
                {
                    char[] f_char = new char[10];

                    for (int j = i + 1; j < max; j++)
                    {
                        if (gcodes[j] == ' ' || gcodes[j] == '\r')
                            break;
                        else
                        {
                            f_char[k] = gcodes[j];
                            k++;
                        }
                    }
                    if (!('0' <= f_char[0] && f_char[0] <= '9'))    //F 뒤에 있는 문자가 정수라면
                        break;
                    f_int = Int32.Parse(new string(f_char));
                    Console.WriteLine(" F" + f_int.ToString());
                    i += k + 1;
                    k = 0;
                }
                //--------------------------------------------------------------------
                txtbox_gcode.Text = ("G" + g_int + " X" + x_float + " Y" + y_float + " E" + e_float + " F" + f_int + " count " + i + " bar " + progbar_ReTime.Value);
                if (progbar_ReTime.Value == max)
                {
                    progbar_ReTime.Value = 0;
                    MessageBox.Show("출력이 완료되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                
                
                //txtbox_gcode.Text += "G = " + Convert.ToString(g_int) + ", X = " + Convert.ToString(x_float) + ", Y = " + Convert.ToString(y_float) + ", E = " + Convert.ToString(e_float) + ", F = " + Convert.ToString(f_int);
            }
            */
        }

        private void sendGcode(String gcode)
        {
            CP2102.WriteLine(gcode.ToString());
            lb_xy.Text = gcode;
        }

        private void btn_left_Click(object sender, EventArgs e)
        {
            sendGcode("G0 X300 Y0 F");
            //serialPort1.WriteLine("a");
        }

        private void btn_right_Click(object sender, EventArgs e)
        {
            sendGcode("G0 X-300 Y0 F");
            //serialPort1.WriteLine("d");
        }

        private void btn_down_Click(object sender, EventArgs e)
        {
            sendGcode("G0 X0 Y300 F");
            //serialPort1.WriteLine("s");
        }

        private void btn_up_Click(object sender, EventArgs e)
        {
            sendGcode("G0 X0 Y-300 F");
            //serialPort1.WriteLine("w");
        }

        private void btn_center_Click(object sender, EventArgs e)
        {
            sendGcode("G28 X0 Y0 F");

        }

        private void btn_Pause_Click(object sender, EventArgs e)
        {
            CP2102.WriteLine("Pause Print");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            try
            {
                Print_thread.Abort();
            }
            catch
            {
                if (MessageBox.Show("출력을 정지할 수 없습니다.\n출력을 정지하려면 프로그램을 강제종료 하시겠습니까?", "예기치 못한 오류", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    this.Close();
                }
                else
                {
                    return;
                }
            }
            
            foreach (Control ctrl in gcodeControls)
            {
                ctrl.Enabled = true;
            }


        }

        private void combox_port_SelectedIndexChanged(object sender, EventArgs e)
        {
            CP2102.Close();

            CP2102.PortName = combox_port.Text;
            //serialPort1.Open();
            try
            {
                CP2102.Open();
            }
            catch
            {
                MessageBox.Show(combox_port.Text + " 포트는 연결되지 않은 포트입니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btn_center.Enabled = true;
            btn_right.Enabled = true;
            btn_left.Enabled = true;
            btn_down.Enabled = true;
            btn_up.Enabled = true;
            btn_Print.Enabled = true;
            btn_Pause.Enabled = true;
            btn_Stop.Enabled = true;
            btn_application.Enabled = true;
            numericupdow_dstc.Enabled = true;
            numericupdow_spd.Enabled = true;
        }

        private void btn_application_Click(object sender, EventArgs e)
        {
            CP2102.WriteLine(numericupdow_dstc.Value.ToString() + numericupdow_spd.Value.ToString());
        }

        private void numericupdow_dstc_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericupdow_spd_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CP2102.BaudRate = Convert.ToInt32(combo_baudrate.Text);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Print_thread.Abort();
        }
    }
}
