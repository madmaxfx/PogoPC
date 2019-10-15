using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PogoPC
{
    public partial class Form1 : Form
    {
        bool connected = false;
        PogoSerial pogoSerial;

        public string[] T0Data;

        public Form1()
        {
            InitializeComponent();
            pogoSerial = new PogoSerial(this);
            comboBox1.DataSource = pogoSerial.GetPorts();
            //disconnectPogo();
            //pogoSerial.SoilTypeChanged += new EventHandler(pogoSerial_SoilTypeChanged);
        }


        public void ClearSoilTYpe()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateSoilType));
            }
            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;

            }
        }
        
        public void UpdateSoilType()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateSoilType));
            }
            else
            {
                switch (pogoSerial.GetSoilType())
                {
                    case 1:
                        radioButton1.Select();
                        break;
                    case 2:
                        radioButton2.Select();
                        break;
                    case 3:
                        radioButton3.Select();
                        break;
                    case 4:
                        radioButton4.Select();
                        break;
                }
            }
        }

        void connectPogo()
        {
            // pogo serial connect
            if (pogoSerial.PogoConnect(comboBox1.Text))
            {
                connected = true;
                button1.Image = Properties.Resources.conectado;
                button1.Text = "Desconectar";
                comboBox1.Enabled = false;
                groupBox4.Enabled = true;
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                groupBox3.Enabled = true;
                button2.Enabled = true;
                label2.Text = "-.-";
                label3.Text = "-.-";
                label4.Text = "-.-";
            }
        }

        void disconnectPogo()
        {
            // pogo serial disconnect
            if (pogoSerial.PogoDisconnect())
            {
                connected = false;
                button1.Image = Properties.Resources.desconectado;
                button1.Text = "Conectar";
                comboBox1.Enabled = true;
                groupBox4.Enabled = false;
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;
                button2.Enabled = false;
                ClearSoilTYpe();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                disconnectPogo();
            }
            else
            {
                connectPogo();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label2.Text = "-.-";
            label3.Text = "-.-";
            label4.Text = "-.-";
            
            pogoSerial.Measure();

        }

        internal void UpdateWithT0()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateWithT0));
            }
            else
            {
                label2.Text = (Decimal.Parse(T0Data[0]) * 100).ToString("F2");
                label3.Text = (Decimal.Parse(T0Data[1]) * 10).ToString("F2");
                label4.Text = Decimal.Parse(T0Data[2]).ToString();

            }
            
        }
    }
}
