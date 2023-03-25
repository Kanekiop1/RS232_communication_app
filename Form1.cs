using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace aplikacja_robot
{

    public partial class Form1 : Form
    {
        string dataOUT;
        string sendWith;
        string dataIn;
        Int32 t;
        Int32 t1;


        public Form1()
        {
            InitializeComponent();

        }



        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cBoxCOMPORT.Items.AddRange(ports);
            btnOpen.Enabled = true;
            btnClose.Enabled = false;
            chBoxDrtEnable.Checked = false;
            serialPort1.DtrEnable = false;
            chBoxRTSEnable.Checked = false;
            serialPort1.RtsEnable = false;
            btnSend.Enabled = false;
            chBoxWriteLine.Checked = false;
            chBoxWrite.Checked = true;
            sendWith = "Write";
            chBoxAddToOldData.Checked = true;
            chBoxAlwaysUpdate.Checked = false;

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {

            try
            {

                serialPort1.PortName = cBoxCOMPORT.Text;
                serialPort1.BaudRate = Convert.ToInt32(cBoxBaudRate.Text);
                serialPort1.DataBits = Convert.ToInt32(cBoxDataBits.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cBoxStopBits.Text);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), cBoxParityBits.Text);
                serialPort1.Open();
                progressBar1.Value = 100;
                btnOpen.Enabled = false;
                btnClose.Enabled = true;
                lblStatusCom.Text = "ON";
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnOpen.Enabled = true;
                btnClose.Enabled = false;
                lblStatusCom.Text = "OFF";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                progressBar1.Value = 0;
                btnOpen.Enabled = true;
                btnClose.Enabled = false;
                lblStatusCom.Text = "OFF";

            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                dataOUT = tBoxDataOut.Text;
                if (sendWith == "WriteLine")
                {
                    serialPort1.WriteLine(dataOUT + "\r");
                }
                else if (sendWith == "Write")
                {
                    serialPort1.Write(dataOUT + "\r");
                }

            }
        }



        private void chBoxDrtEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxDrtEnable.Checked)
            {
                serialPort1.DtrEnable = true;
                MessageBox.Show("DRT Enable", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else { serialPort1.DtrEnable = false; }
        }

        private void chBoxRTSEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxRTSEnable.Checked)
            {
                serialPort1.RtsEnable = true;
                MessageBox.Show("RTS Enable", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else { serialPort1.RtsEnable = false; }
        }

        private void btnClearDataOut_Click(object sender, EventArgs e)
        {
            if (tBoxDataOut.Text != "")
            {
                tBoxDataOut.Text = "";

            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int dataOUTLength = tBoxDataOut.TextLength;
            lblDataOutLength.Text = string.Format("{0:00}", dataOUTLength);
            if (chBoxUsingEnter.Checked)
            {

                tBoxDataOut.Text = tBoxDataOut.Text.Replace(Environment.NewLine, "");
            }
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

            if (chBoxUsingButton.Checked)
            {
                btnSend.Enabled = true;
            }
            else { btnSend.Enabled = false; }
        }

        private void chBoxUsingEnter_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chBoxUsingEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if (chBoxUsingEnter.Checked)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (serialPort1.IsOpen)
                    {
                        dataOUT = tBoxDataOut.Text;
                        if (sendWith == "WriteLine")
                        {
                            serialPort1.WriteLine(dataOUT);
                        }
                        else if (sendWith == "Write")
                        {
                            serialPort1.Write(dataOUT);
                        }


                    }
                }

            }
        }

        private void chBoxWriteLine_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxWriteLine.Checked)
            {
                sendWith = "WriteLine";
                chBoxWrite.Checked = false;
                chBoxWriteLine.Checked = true;
            }

        }

        private void chBoxWrite_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxWrite.Checked)
            {
                sendWith = "Write";
                chBoxWrite.Checked = true;
                chBoxWriteLine.Checked = false;

            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            dataIn = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(ShowData));

        }

        private void ShowData(object sender, EventArgs e)
        {
            int dataINLength = dataIn.Length;
            lblDataInLength.Text = string.Format("{0:00}", dataINLength);

            if (chBoxAlwaysUpdate.Checked)
            {
                tBoxDataIN.Text = dataIn;
            }

            else if (chBoxAddToOldData.Checked)
            {
                tBoxDataIN.Text += dataIn;
            }
        }

        private void chBoxAddToOldData_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxAddToOldData.Checked)
            {
                chBoxAlwaysUpdate.Checked = false;
                chBoxAddToOldData.Checked = true;
            }
            else { chBoxAlwaysUpdate.Checked = true; }
        }

        private void chBoxAlwaysUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxAlwaysUpdate.Checked)
            {
                chBoxAlwaysUpdate.Checked = true;
                chBoxAddToOldData.Checked = false;
            }
            else { chBoxAddToOldData.Checked = true; }
        }

        private void btnDataClearIN_Click(object sender, EventArgs e)
        {
            if (tBoxDataIN.Text != "")
            {
                tBoxDataIN.Text = "";
            }
        }

        private void cBoxBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void btnIOC1m_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DJ 1, -10 \r");

            }


        }

        private void btnIOC1p_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DJ 1, 10 \r");

            }
        }

        private void bntIOC2m_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DJ 2, -10 \r");

            }

        }

        private void btnIOC2p_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DJ 2, 10 \r");

            }

        }

        private void btnIOC3m_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DJ 3, -10 \r");

            }

        }

        private void btnIOC3p_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DJ 3, 10 \r");

            }

        }

        private void btnIOC4m_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DJ 4, -10 \r");

            }

        }

        private void btnIOC4p_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DJ 4, 10 \r");

            }

        }

        private void btnIOC5m_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DJ 5, -10 \r");

            }

        }

        private void btnIOC5p_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DJ 5, 10 \r");

            }

        }

        private void btnIOC6m_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DJ 6, -10 \r");

            }

        }

        private void btnIOC6p_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DJ 6, 10 \r");

            }

        }

        private void btnILXm_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DS -10, 0, 0\r");

            }

        }

        private void btnILXp_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DS 10, 0, 0\r");

            }

        }

        private void btnILYm_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DS 0, -10, 0\r");

            }

        }

        private void btnILYp_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DS 0, 10, 0\r");

            }

        }

        private void btnILZm_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DS 0, 0, -10\r");

            }

        }

        private void btnILZp_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("DS 0, 0, 10\r");

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("GO\r");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("GC\r");

            }
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("WH\r");

            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("MT 1,-10\r");

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("MT 1,10\r");

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("MTS 1,-10\r");

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("MTS 1,10\r");

            }
        }

        private void groupBox14_Enter(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            t = cBoxZMP.SelectedIndex + 1;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                t = Convert.ToInt32(cBoxKP.Text);
                if (t == 1)
                    serialPort1.Write("PC 1\r");
                else if (t == 2)
                {
                    serialPort1.Write("PC 2\r");
                }
                else if (t == 3)
                {
                    serialPort1.Write("PC 3\r");
                }
                else if (t == 4)
                {
                    serialPort1.Write("PC 4\r");
                }
                else if (t == 5)
                {
                    serialPort1.Write("PC 5\r");
                }
                else if (t == 6)
                {
                    serialPort1.Write("PC 6\r");
                }
                else if (t == 7)
                {
                    serialPort1.Write("PC 7\r");
                }
                else if (t == 8)
                {
                    serialPort1.Write("PC 8\r");
                }
                else if (t == 9)
                {
                    serialPort1.Write("PC 9\r");
                }
                else if (t == 10)
                {
                    serialPort1.Write("PC 10\r");
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                t1 = Convert.ToInt32(cBoxOP.Text);
                if (t1 == 1)
                    serialPort1.Write("PR 1\r");
                else if (t1 == 2)
                {
                    serialPort1.Write("PR 2\r");
                }
                else if (t1 == 3)
                {
                    serialPort1.Write("PR 3\r");
                }
                else if (t1 == 4)
                {
                    serialPort1.Write("PR 4\r");
                }
                else if (t1 == 5)
                {
                    serialPort1.Write("PR 5\r");
                }
                else if (t1 == 6)
                {
                    serialPort1.Write("PR 6\r");
                }
                else if (t1 == 7)
                {
                    serialPort1.Write("PR 7\r");
                }
                else if (t1 == 8)
                {
                    serialPort1.Write("PR 8\r");
                }
                else if (t1 == 9)
                {
                    serialPort1.Write("PR 9\r");
                }
                else if (t1 == 10)
                {
                    serialPort1.Write("PR 10\r");
                }

            }

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
              
                   serialPort1.Write("PL "+ cBoxZMP.Text+"," +comboBox5.Text+"\r");
             
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

                serialPort1.Write("HE " + cBoxZAP.Text + "\r");

            }
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tBoxDataIN_TextChanged(object sender, EventArgs e)
        {

        }
    }

  }
