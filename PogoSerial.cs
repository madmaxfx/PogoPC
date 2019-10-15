using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PogoPC
{
    class PogoSerial
    {

        System.IO.Ports.SerialPort port;
        string buffer;
        string lastCommand;
        int soilType;
        Form1 form;

        public PogoSerial(Form1 form)
        {
            this.form = form;

            buffer = "";
            
        }

        public event EventHandler SoilTypeChanged;

        public string[] GetPorts()
        {
            return System.IO.Ports.SerialPort.GetPortNames();
        }

        public bool PogoConnect(string portName)
        {
            port = new System.IO.Ports.SerialPort(portName, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            port.ReceivedBytesThreshold = 1;
            port.ReadTimeout = 1000;
            port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived);
            port.Open();
            //System.Threading.Thread.Sleep(2000);
            if (port.IsOpen)
            {
                // inicializar pogo
                //System.Threading.Thread.Sleep(2000);
                lastCommand = "ST";
                PogoSend("0");
                PogoSend("0");
                PogoSend("0");
                PogoSend("S");
                PogoSend("T");
                PogoSend("=");
                PogoSend("?");
                PogoSend("\r");
                PogoSend("\n");
                //System.Threading.Thread.Sleep(2000);
 
                return true;
            }

            return false;
        }

        void port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (e.EventType == System.IO.Ports.SerialData.Chars)
            {
                buffer = ((System.IO.Ports.SerialPort)sender).ReadLine();
                processResponse();
                /*
                string c = port.ReadExisting();
                //Console.Out.Write(Convert.ToChar(c));

                if (c == "\r")
                {
                }

                else if (c == "\n")
                {
                    // process response
                    processResponse();
                    //Console.Out.WriteLine(buffer);
                    //buffer = new StringBuilder();
                }
                else
                {
                    buffer.Append(c); 
                }*/

                
            }

        }

        public bool PogoDisconnect()
        {
            port.Close();
            if (!port.IsOpen) {
                return true;
            }
            return false;
        }

        public void PogoSend(string c)
        {
            port.Write(c);
            System.Threading.Thread.Sleep(10);
        }



        private void processResponse()
        {
            if (lastCommand.Equals("ST")) {
                Console.WriteLine("Response: " + buffer.ToString());
                lastCommand = "";
                setSoilType(Int16.Parse(buffer.ToString()));
                form.UpdateSoilType();
                buffer = "";
                return;
            }

            if (lastCommand.Equals("T0"))
            {
                Console.WriteLine("Response: " + buffer.ToString());
                string d = buffer.ToString().Substring(3);
                string[] data = d.Split(',');

                lastCommand = "";
                form.T0Data = data;
                form.UpdateWithT0();

                /*setSoilType(Int16.Parse(buffer.ToString()));
                form.UpdateSoilType();*/
                buffer = "";
                return;
            }

        }

        private void setSoilType(int soilType) {
            this.soilType = soilType;
            //SoilTypeChanged(this, new EventArgs());
        }

        public int GetSoilType()
        {
            return this.soilType;
        }


        internal void Measure()
        {
            //lastCommand = "TR";
            PogoSend("0");
            PogoSend("0");
            PogoSend("0");
            PogoSend("T");
            PogoSend("R");
            PogoSend("\r");
            PogoSend("\n");
            System.Threading.Thread.Sleep(2000);

            lastCommand = "T0";
            PogoSend("0");
            PogoSend("0");
            PogoSend("0");
            PogoSend("T");
            PogoSend("0");
            PogoSend("\r");
            PogoSend("\n");
        }
    }
}
