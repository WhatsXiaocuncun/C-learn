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

namespace Server服务器
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                //1.当点击开始监听的时候，在服务器端口创建一个监听IP个端口的SOCKET
                //设置属性IPV4,字节流，TCP协议
                Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //任意IP可连，如果用文本连接可以使用：IPAddress ip = IPAddress.Parse(txtServe.Text);
                IPAddress ip = IPAddress.Any;
                //创建端口号对象
                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
                //2.bind监听
                socketWatch.Bind(point);
                //创建日志文本的方法实例化
                ShowMsg("监听成功");
                //3.Listen设置监听队列为10
                socketWatch.Listen(10);
                //开启监听新线程
                Thread th = new Thread(Listen);
                //设置为后台线程
                th.IsBackground = true;
                //新线程开启
                th.Start(socketWatch);
            }
            catch { }
        }


        /// <summary>
        /// 在Log框内显示文本消息
        /// </summary>
        /// <param name="str">需要显示文本内容</param>
        void ShowMsg(string str)
        {
            //AppendText追加文本
            txtLog.AppendText(str+"\r\n");
        }

        //创建一个Socket类型变量，客户端的Socket
        Socket socketSend;


        /// <summary>
        /// 创建一个监听Listen，线程只能接收Object类型变量
        /// </summary>
        /// <param name="o">Object类型</param>
        void Listen(object o)
        {
            //创建一个负责通信的Socket，用as将O强转为socket
            Socket socketwatch = o as Socket;
            while (true)
            {
                try
                {
                    //4.负责和客户端通信的Socket
                    socketSend = socketwatch.Accept();
                    //将远程连接的客户端的IP和Socket存入一个集合中,socketSend.RemoteEndPoint.ToString(),socketSend
                    dicSocket.Add(socketSend.RemoteEndPoint.ToString(), socketSend);
                    //将IP显示在下拉列表框内
                    cboUsers.Items.Add(socketSend.RemoteEndPoint.ToString());
                    //显示在日志文本框内
                    ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + "连接成功");
                    //开启一个新线程来不停接收客户端发来的消息
                    Thread th = new Thread(Recive);
                    //设置为后台线程
                    th.IsBackground = true;
                    //开启新线程
                    th.Start(socketSend);
                }               
                catch { }
            }
        }
        //创建一个存客户端IP和Socket的集合
        Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();


        /// <summary>
        /// 5.服务器不停接收来自客户端发来的消息
        /// </summary>
        /// <param name="o"></param>
        void Recive(object o)
        {
            Socket socketsend = o as Socket;
            while (true)
            {
                try
                {
                    //服务器接收客户端发来的消息
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    //实际接收到的有效字节数
                    int r = socketsend.Receive(buffer);
                    //判断接收到的字节数是否是0，空也会占用字节数
                    if (r == 0)
                    {
                        break;
                    }
                    //收到的数据转换为string
                    string str = Encoding.UTF8.GetString(buffer, 0, r);
                    //显示在文本日志中
                    ShowMsg(socketsend.RemoteEndPoint.ToString() + ":" + str);
                }
                catch { }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //不去检查错误线程报错
            Control.CheckForIllegalCrossThreadCalls = false;
        }


        /// <summary>
        /// 服务器给客户端发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            //获取文本框内要发送的内容
            string str = txtMsg.Text;
            //转换成byte格式
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            //发送的数据存在集合内
            List<byte> list = new List<byte>();
            //0作为识别内容类型的标准
            list.Add(0);
            //存入文本内容
            list.AddRange(buffer);
            //将泛型集合转换为数组
            byte[] newBuffer = list.ToArray();
            try
            {
                //在下拉框中获得地址IP
                string ip = cboUsers.SelectedItem.ToString();
                //6.发送给客户端
                dicSocket[ip].Send(newBuffer);
            }
            catch
            {
                MessageBox.Show("下拉框中未选择制定的IP，请重新选择！");
            }
        }

        /// <summary>
        /// 选择要发送的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnSelect_Click(object sender, EventArgs e)
        {
            //创建一个打开对话框对象
            OpenFileDialog ofd = new OpenFileDialog();
            //设置打开的原始界面
            ofd.InitialDirectory = @"C:\Users\WuYuc\Desktop";
            //设置界面名称
            ofd.Title = "请选择要发送的文件";
            //选择文件类型
            ofd.Filter = "所有文件|*.*";
            //设置通用选择对话框
            ofd.ShowDialog();
            //将包含文件名的路径赋值给txtPath
            txtPath.Text = ofd.FileName;

        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            //获取文件发送路径
            string path = txtPath.Text;
            //发送文件,设置发送文件的路径，方式，读取属性
            using(FileStream fsRead = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                //可传输文件大小
                byte[] buffer = new byte[1024 * 1024 * 5];
                //读取数量
                int r = fsRead.Read(buffer, 0, buffer.Length);
                //创建缓存集合
                List<byte> list = new List<byte>();
                //1表示发送的是文件，上面0表示发送的文本
                list.Add(1);
                //发送的文件
                list.AddRange(buffer);
                //转换成数组
                byte[] newBuffer = list.ToArray();
                //发送格式
                dicSocket[cboUsers.SelectedItem.ToString()].Send(newBuffer,0,r+1,SocketFlags.None);
            }
        }


        /// <summary>
        /// 发送震动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZD_Click(object sender, EventArgs e)
        {
            //新建一个BYTE
            byte[] buffer = new byte[1];
            //2表示震动
            buffer[0] = 2;
            //发送格式
            dicSocket[cboUsers.SelectedItem.ToString()].Send(buffer);
        }
    }
}
