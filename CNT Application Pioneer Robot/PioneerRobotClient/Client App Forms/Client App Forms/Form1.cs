using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Client_App_Forms
{
    public partial class Form1 : Form
    {

        ClientCommands cc = new ClientCommands();
        ClientVideo cv = new ClientVideo();

        byte[] toSend = null;
        String userIn = null;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Pioneer Client Control Console";
        }

        public void Connect(object sender, EventArgs e)
        {
            cc.Run(ipAddr, status, camView);
            cv.Run(ipAddr, camView);
            Console.In.ReadLine();
            disconnect.Enabled = true;
            forward.Enabled = true;
            back.Enabled = true;
            reset.Enabled = true;
            left.Enabled = true;
            right.Enabled = true;
        }

        public void SendCommands(object sender, EventArgs e)
        {
            int dist = 0, rot = 0;
       
            if (sender.Equals(forward))
            {
                try
                {
                    dist = Int32.Parse(distance.Text);
                    userIn = String.Format("forward|{0}", dist);
                    toSend = Encoding.ASCII.GetBytes(userIn);
                    cc.SendCommand(toSend);
                    status.AppendText("Moving Robot Forward " + dist + " cm.\n");
                }
                catch(FormatException e2)
                {
                    DistOrRot(true);
                }
            }
            else if (sender.Equals(back))
            {
                try
                {
                    dist = Int32.Parse(distance.Text);
                    userIn = String.Format("backward|{0}", dist);
                    toSend = Encoding.ASCII.GetBytes(userIn);
                    cc.SendCommand(toSend);
                    status.AppendText("Moving Robot Back " + dist + " cm.\n");
                }
                catch (FormatException e2)
                {
                    DistOrRot(true);
                }
            }
            else if (sender.Equals(left))
            {
                try
                {
                    rot = Int32.Parse(rotation.Text);
                    userIn = String.Format("left|{0}", rot);
                    toSend = Encoding.ASCII.GetBytes(userIn);
                    cc.SendCommand(toSend);
                    status.AppendText("Rotating Robot Left " + rot + " degrees.\n");
                }
                catch (FormatException e2)
                {
                    DistOrRot(true);
                }
            }
            else if (sender.Equals(right))
            {
                try
                {
                    rot = Int32.Parse(rotation.Text);
                    userIn = String.Format("right|{0}", rot);
                    toSend = Encoding.ASCII.GetBytes(userIn);
                    cc.SendCommand(toSend);
                    status.AppendText("Rotating Robot Right " + rot + " degrees.\n");
                }
                catch (FormatException e2)
                {
                    DistOrRot(false);
                }
            }
            else if (sender.Equals(reset))
            {
                toSend = Encoding.ASCII.GetBytes("reset");
                cc.SendCommand(toSend);
                status.AppendText("Resetting Robot\n");
                distance.Text = "";
                rotation.Text = "";
            }

        }

        public void DistOrRot(bool isDistance)
        {   
            if (isDistance)
            {
                MessageBox.Show("Please enter a number for the distance of the robot.");
                distance.Text = "";
            }
            else
            {
                MessageBox.Show("Please enter a number for the rotation of the robot.");
                rotation.Text = "";
            }
        }

        public void Disconnect(object sender, EventArgs e)
        {
            
            cc.Close();
            this.Close();
        }

       
    }
}