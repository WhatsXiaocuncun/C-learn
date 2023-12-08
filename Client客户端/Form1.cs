using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client客户端
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Socket socketSend;
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                //1.创建负责通信的Socket
                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //获得服务器IP
                IPAddress ip = IPAddress.Parse(txtServer.Text);
                //获得端口
                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
                //2.获得要连接的远程服务器的端口和IP地址
                socketSend.Connect(point);
                ShowMsg("连接成功");
                //开启一个新线程来不停接收服务器发来的消息
                Thread th = new Thread(Recive);
                th.IsBackground = true;
                th.Start();
            }           
            catch { }

        }

        void ShowMsg(String str)
        {
            txtLog.AppendText(str+"\r\n");
        }
        /// <summary>
        /// 3.接收数据
        /// </summary>
        void Recive()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024 * 3];
                    int r = socketSend.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    if (buffer[0] == 0)
                    {
                        string s = Encoding.UTF8.GetString(buffer, 1, r - 1);
                        ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + s);
                    }
                    else if (buffer[0] == 1)
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.InitialDirectory = @"E:\测试文件夹";
                        sfd.Title = "请选择要保存的文件";
                        sfd.Filter = "所有文件|*.*";
                        sfd.ShowDialog(this);
                        string path = sfd.FileName;
                        using (FileStream fsWritle = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            fsWritle.Write(buffer, 1, r - 1);
                        }
                        MessageBox.Show("保存成功");
                    }
                    else if (buffer[0] == 2)
                    {
                        ZD();
                    }
                }              
                catch { }
            }
        }


        /// <summary>
        /// 震动功能
        /// </summary>
        void ZD()
        {
            for (int i = 0; i < 5000; i++)
            {
                this.Location = new Point(200,200);
                this.Location = new Point(280, 280);
            }

        }


        /// <summary>
        /// 给服务器发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            string str = txtMsg.Text.Trim();
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            //4.发送数据
            socketSend.Send(buffer);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
