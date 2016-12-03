using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Client_App_Forms
{

    class ClientVideo
    {
        PictureBox wc;
        TextBox ipBox;

        public void Run(TextBox ip, PictureBox wc)
        {
            this.wc = wc;
            Console.Out.WriteLine("creating thread");
            ipBox = ip;
            Thread imageT = new Thread(new ParameterizedThreadStart(StartClient));
            imageT.Start(wc);
            while (!imageT.IsAlive)
            {
                Console.Out.WriteLine("spinning");
            }
        }

        public void StartClient(Object obj)
        {
            wc = (PictureBox)obj;
            // Data buffer for incoming data.
            byte[] bytes = new byte[120000];
            byte[] imgsize = new byte[4];
            int port = 12345;

            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                // This example uses port 11000 on the local computer.
                IPHostEntry ipHostInfo = Dns.GetHostEntry(ipBox.Text);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP  socket.
                Socket server = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    Console.WriteLine("Socket connecting");

                    server.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        server.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                    // Send the data through the socket.
                    int bytesSent = server.Send(msg);

                    // Receive the response from the remote device.

                    
                    while (true)
                    {   
                            try
                            {
                            byte[] buffer = new byte[1000000];

                            server.Receive(buffer, buffer.Length, SocketFlags.None);

                            MemoryStream ms = new MemoryStream(buffer);

                            ms.Write(buffer, 0, buffer.Length);
                            Bitmap bitmap = new Bitmap(Image.FromStream(ms));

                            SetImage(bitmap);
                            }
                            catch (Exception e)
                            {
                                
                            }
                            // after we've done all the processing,

                            RefreshImage();
                            
                    }
                    // Release the socket.
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void RefreshImage()
        {
            if (wc.InvokeRequired)
            { // after we've done all the processing, 
                wc.Invoke(new MethodInvoker(delegate
                {
                    wc.Refresh();
                }));
            }
            else {
                wc.Refresh();
            }
        }

        private void SetImage(Bitmap img)
        {
            if (wc.InvokeRequired)
            { // after we've done all the processing, 
                wc.Invoke(new MethodInvoker(delegate
                {
                    wc.Image = img;
                }));
            }
            else {
                wc.Image = img;
            }
        }
    }
}
