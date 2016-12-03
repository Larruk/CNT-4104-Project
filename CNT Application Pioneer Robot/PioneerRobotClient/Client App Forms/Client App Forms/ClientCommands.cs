using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Client_App_Forms
{
    class ClientCommands
    {
        private int port = 12346;

        Socket client = null;
        IPAddress serverAddr = null;
        IPEndPoint server = null;


        public void Run(TextBox ip, RichTextBox rtb, PictureBox wc)
        {
            try
            {
                rtb.AppendText("Connecting to Command Server...\n");



                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPHostEntry ipHostInfo = Dns.Resolve(ip.Text);
                serverAddr = ipHostInfo.AddressList[0];
                server = new IPEndPoint(serverAddr, port);

                client.Connect(server);
                rtb.AppendText("Connection to Command Server Successful!\n");
            }
            catch (Exception exception)
            {
                MessageBox.Show("You have not entered the correct IP.\nPlease enter the IP on the Pioneer Server.");
                rtb.AppendText("Connection Failed!\n");
                ip.Text = "";
            }

        }

        public void SendCommand(byte[] sending)
        {
            client.Send(sending);
        }

        public void Close()
        {
            client.Close();
        }

    }
}