using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EasyGPIO
{
    public partial class Form1 : Form
    {
        private Socket Server;
        private IPHostEntry hostAddr;
        bool continueRecv = false;

        public Form1()
        {
            InitializeComponent();
            Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void ConnectToServer()
        {
            try
            {
                hostAddr = Dns.GetHostEntry(AddressBox.Text);
                Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Server.Connect(hostAddr.AddressList[0].ToString(), 5005);

                StateObject state = new StateObject();
                state.client = Server;

                Server.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                StatusLabel.Text = "Connected To Server @ " + AddressBox.Text;
                
            }
            catch (Exception)
            {
                StatusLabel.Text = "Could Not Connect To " + AddressBox.Text;
            }
        }

        private void DisconnectFromServer()
        {
            Server.Close();
           
        }

        public void ReceiveCallback(IAsyncResult asyncResult)
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                StateObject state = (StateObject)asyncResult.AsyncState;
                client = state.client;
                int bytesRead = client.EndReceive(asyncResult);

                if(bytesRead.Equals(160))
                    SetPins(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                string message = GetMessage();
                Send(client, message);

                if (continueRecv)
                {
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    client.Close();
                    StatusLabel.Text = "Disconnected From Server";
                }
            }
            catch (Exception E)
            {
                StatusLabel.Text = "Disconnected by Host";
            }
        }

        private void Send(Socket server, string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);

            lock (server)
            {
                server.Send(data);
            }
        }

        public class StateObject
        {
            public Socket client = null;
            public const int BufferSize = 160;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder data = new StringBuilder();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            continueRecv = true;
            ConnectToServer();
            
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            continueRecv = false;
        }

        private string GetMessage()
        {
            string message = "";

            message += "01XL";

            message += "02XL";

            if (Input3.Checked)
                message += "03IL";
            else if (OutHigh3.Checked)
                message += "03OH";
            else if (OutLow3.Checked)
                message += "03OL";
            else
                message += "03XL";

            message += "04XL";

            if (Input5.Checked)
                message += "05IL";
            else if (OutHigh5.Checked)
                message += "05OH";
            else if (OutLow5.Checked)
                message += "05OL";
            else
                message += "05XL";

            message += "06XL";

            if (Input7.Checked)
                message += "07IL";
            else if (OutHigh7.Checked)
                message += "07OH";
            else if (OutLow7.Checked)
                message += "07OL";
            else
                message += "07XL";

            if (Input8.Checked)
                message += "08IL";
            else if (OutHigh8.Checked)
                message += "08OH";
            else if (OutLow8.Checked)
                message += "08OL";
            else
                message += "08XL";

            message += "09XL";

            if (Input10.Checked)
                message += "10IL";
            else if (OutHigh10.Checked)
                message += "10OH";
            else if (OutLow10.Checked)
                message += "10OL";
            else
                message += "10XL";

            if (Input11.Checked)
                message += "11IL";
            else if (OutHigh11.Checked)
                message += "11OH";
            else if (OutLow11.Checked)
                message += "11OL";
            else
                message += "11XL";

            if (Input12.Checked)
                message += "12IL";
            else if (OutHigh12.Checked)
                message += "12OH";
            else if (OutLow12.Checked)
                message += "12OL";
            else
                message += "12XL";

            if (Input13.Checked)
                message += "13IL";
            else if (OutHigh13.Checked)
                message += "13OH";
            else if (OutLow13.Checked)
                message += "13OL";
            else
                message += "13XL";

            message += "14XL";

            if (Input15.Checked)
                message += "15IL";
            else if (OutHigh15.Checked)
                message += "15OH";
            else if (OutLow15.Checked)
                message += "15OL";
            else
                message += "15XL";

            if (Input16.Checked)
                message += "16IL";
            else if (OutHigh16.Checked)
                message += "16OH";
            else if (OutLow16.Checked)
                message += "16OL";
            else
                message += "16XL";

            message += "17XL";

            if (Input18.Checked)
                message += "18IL";
            else if (OutHigh18.Checked)
                message += "18OH";
            else if (OutLow18.Checked)
                message += "18OL";
            else
                message += "18XL";

            if (Input19.Checked)
                message += "19IL";
            else if (OutHigh19.Checked)
                message += "19OH";
            else if (OutLow19.Checked)
                message += "19OL";
            else
                message += "19XL";

            message += "20XL";

            if (Input21.Checked)
                message += "21IL";
            else if (OutHigh21.Checked)
                message += "21OH";
            else if (OutLow21.Checked)
                message += "21OL";
            else
                message += "21XL";

            if (Input22.Checked)
                message += "22IL";
            else if (OutHigh22.Checked)
                message += "22OH";
            else if (OutLow22.Checked)
                message += "22OL";
            else
                message += "22XL";

            if (Input23.Checked)
                message += "23IL";
            else if (OutHigh23.Checked)
                message += "23OH";
            else if (OutLow23.Checked)
                message += "23OL";
            else
                message += "23XL";

            if (Input24.Checked)
                message += "24IL";
            else if (OutHigh24.Checked)
                message += "24OH";
            else if (OutLow24.Checked)
                message += "24OL";
            else
                message += "24XL";

            message += "25XL";

            if (Input26.Checked)
                message += "26IL";
            else if (OutHigh26.Checked)
                message += "26OH";
            else if (OutLow26.Checked)
                message += "26OL";
            else
                message += "26XL";

            message += "27XL";

            message += "28XL";

            if (Input29.Checked)
                message += "29IL";
            else if (OutHigh29.Checked)
                message += "29OH";
            else if (OutLow29.Checked)
                message += "29OL";
            else
                message += "29XL";

            message += "30XL";

            if (Input31.Checked)
                message += "31IL";
            else if (OutHigh31.Checked)
                message += "31OH";
            else if (OutLow31.Checked)
                message += "31OL";
            else
                message += "31XL";

            if (Input32.Checked)
                message += "32IL";
            else if (OutHigh32.Checked)
                message += "32OH";
            else if (OutLow32.Checked)
                message += "32OL";
            else
                message += "32XL";

            if (Input33.Checked)
                message += "33IL";
            else if (OutHigh33.Checked)
                message += "33OH";
            else if (OutLow33.Checked)
                message += "33OL";
            else
                message += "33XL";

            message += "34XL";

            if (Input35.Checked)
                message += "35IL";
            else if (OutHigh35.Checked)
                message += "35OH";
            else if (OutLow35.Checked)
                message += "35OL";
            else
                message += "35XL";

            if (Input36.Checked)
                message += "36IL";
            else if (OutHigh36.Checked)
                message += "36OH";
            else if (OutLow36.Checked)
                message += "36OL";
            else
                message += "36XL";

            if (Input37.Checked)
                message += "37IL";
            else if (OutHigh37.Checked)
                message += "37OH";
            else if (OutLow37.Checked)
                message += "37OL";
            else
                message += "37XL";

            if (Input38.Checked)
                message += "38IL";
            else if (OutHigh38.Checked)
                message += "38OH";
            else if (OutLow38.Checked)
                message += "38OL";
            else
                message += "38XL";

            message += "39XL";

            if (Input40.Checked)
                message += "40IL";
            else if (OutHigh40.Checked)
                message += "40OH";
            else if (OutLow40.Checked)
                message += "40OL";
            else
                message += "40XL";

            return message;
        }

        private void SetPins(string data)
        {

            if (data.Contains("03IH"))
                groupBox3.BackColor = Color.Red;
            else
                groupBox3.BackColor = Color.Orchid;

            if (data.Contains("05IH"))
                groupBox5.BackColor = Color.Red;
            else
                groupBox5.BackColor = Color.Orchid;

            if (data.Contains("07IH"))
                groupBox7.BackColor = Color.Red;
            else
                groupBox7.BackColor = Color.LimeGreen;

            if (data.Contains("08IH"))
                groupBox8.BackColor = Color.Red;
            else
                groupBox8.BackColor = Color.BlueViolet;

            if (data.Contains("10IH"))
                groupBox10.BackColor = Color.Red;
            else
                groupBox10.BackColor = Color.BlueViolet;

            if (data.Contains("11IH"))
                groupBox11.BackColor = Color.Red;
            else
                groupBox11.BackColor = Color.LimeGreen;

            if (data.Contains("12IH"))
                groupBox12.BackColor = Color.Red;
            else
                groupBox12.BackColor = Color.Silver;

            if (data.Contains("13IH"))
                groupBox13.BackColor = Color.Red;
            else
                groupBox13.BackColor = Color.LimeGreen;
            
            if (data.Contains("15IH"))
                groupBox15.BackColor = Color.Red;
            else
                groupBox15.BackColor = Color.LimeGreen;

            if (data.Contains("16IH"))
                groupBox16.BackColor = Color.Red;
            else
                groupBox16.BackColor = Color.LimeGreen;
            
            if (data.Contains("18IH"))
                groupBox18.BackColor = Color.Red;
            else
                groupBox18.BackColor = Color.LimeGreen;

            if (data.Contains("19IH"))
                groupBox19.BackColor = Color.Red;
            else
                groupBox19.BackColor = Color.MediumBlue;
            
            if (data.Contains("21IH"))
                groupBox21.BackColor = Color.Red;
            else
                groupBox21.BackColor = Color.MediumBlue;

            if (data.Contains("22IH"))
                groupBox22.BackColor = Color.Red;
            else
                groupBox22.BackColor = Color.LimeGreen;

            if (data.Contains("23IH"))
                groupBox23.BackColor = Color.Red;
            else
                groupBox23.BackColor = Color.MediumBlue;

            if (data.Contains("24IH"))
                groupBox24.BackColor = Color.Red;
            else
                groupBox24.BackColor = Color.MediumBlue;
            
            if (data.Contains("26IH"))
                groupBox26.BackColor = Color.Red;
            else
                groupBox26.BackColor = Color.MediumBlue;

            if (data.Contains("27IH"))
                groupBox27.BackColor = Color.Red;
            else
                groupBox27.BackColor = Color.Gold;

            if (data.Contains("28IH"))
                groupBox28.BackColor = Color.Red;
            else
                groupBox28.BackColor = Color.Gold;

            if (data.Contains("29IH"))
                groupBox29.BackColor = Color.Red;
            else
                groupBox29.BackColor = Color.LimeGreen;
            
            if (data.Contains("31IH"))
                groupBox31.BackColor = Color.Red;
            else
                groupBox31.BackColor = Color.LimeGreen;

            if (data.Contains("32IH"))
                groupBox32.BackColor = Color.Red;
            else
                groupBox32.BackColor = Color.LimeGreen;

            if (data.Contains("33IH"))
                groupBox33.BackColor = Color.Red;
            else
                groupBox33.BackColor = Color.LimeGreen;
            
            if (data.Contains("35IH"))
                groupBox35.BackColor = Color.Red;
            else
                groupBox35.BackColor = Color.Silver;

            if (data.Contains("36IH"))
                groupBox36.BackColor = Color.Red;
            else
                groupBox36.BackColor = Color.LimeGreen;

            if (data.Contains("37IH"))
                groupBox37.BackColor = Color.Red;
            else
                groupBox37.BackColor = Color.LimeGreen;

            if (data.Contains("38IH"))
                groupBox38.BackColor = Color.Red;
            else
                groupBox38.BackColor = Color.Silver;
            
            if (data.Contains("40IH"))
                groupBox40.BackColor = Color.Red;
            else
                groupBox40.BackColor = Color.Silver;

        }
    }
}
