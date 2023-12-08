using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 串口调试助手
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();//初始化所有零件
        }
        SerialPort sp = null;//声明一个串口类
        bool isOpen = false;//打开串口标志位
        bool isSetProperty = false;//属性设置标志位
        bool isHEX = false;//十六进制显示标志位

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;//隐藏最大化按钮
            this.MaximumSize = this.Size;//固定最大尺寸
            this.MinimumSize = this.Size;//固定最小尺寸

            for (int i = 0; i < 10; i++)//最大支持10个串口
            {
                cbxCOMPort.Items.Add("COM" + (i + 1).ToString());
            }
            cbxCOMPort.SelectedIndex = 0;
            //列出常用波特率
            cbxBaudRate.Items.Add("1200");
            cbxBaudRate.Items.Add("2400");
            cbxBaudRate.Items.Add("4800");
            cbxBaudRate.Items.Add("9600");
            cbxBaudRate.Items.Add("19200");
            cbxBaudRate.Items.Add("38400");
            cbxBaudRate.Items.Add("115200");
            cbxBaudRate.SelectedIndex = 5;

            //列出停止位
            cbxStopBits.Items.Add("0");
            cbxStopBits.Items.Add("1");
            cbxStopBits.Items.Add("1.5");
            cbxStopBits.Items.Add("2");
            cbxStopBits.SelectedIndex = 0;

            //列出数据位
            cbxDataBits.Items.Add("8");
            cbxDataBits.Items.Add("7");
            cbxDataBits.Items.Add("6");
            cbxDataBits.Items.Add("5");
            cbxDataBits.SelectedIndex = 0;

            //列出奇偶校验位
            cbxParity.Items.Add("无");
            cbxParity.Items.Add("奇校验");
            cbxParity.Items.Add("偶校验");
            cbxParity.SelectedIndex = 0;

            //默认Char显示
            rbnChar.Checked = true;
        }

        private void btnCheckCom_Click(object sender, EventArgs e)
        {
            bool comExistence = false;//可用串口标志位
            cbxCOMPort.Items.Clear();//清除串口号中的串口名称
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    SerialPort sp = new SerialPort("COM" + (i + 1).ToString());
                    sp.Open();
                    sp.Close();
                    cbxCOMPort.Items.Add("COM" + (i + 1).ToString());
                    comExistence = true;
                }
                catch (Exception)
                {
                    continue;
                }

            }
            if (comExistence)
            {
                cbxCOMPort.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("没有找到可用串口！","错误提示");
            }
        }
        /// <summary>
        /// 检测串口是否设置
        /// </summary>
        /// <returns></returns>
        private bool CheckPortSetting()
        {
            if (cbxCOMPort.Text.Trim() == "") return false;
            if (cbxBaudRate.Text.Trim() == "") return false;
            if (cbxDataBits.Text.Trim() == "") return false;
            if (cbxParity.Text.Trim() == "") return false;
            if (cbxStopBits.Text.Trim() == "") return false;
            return true;
        }
        /// <summary>
        /// 检查发送数据是否输入
        /// </summary>
        /// <returns></returns>
        private bool CheckSendData()
        {
            if (tbxSendData.Text.Trim() == "") return false;
            return true;
        }
        /// <summary>
        /// 设置串口属性
        /// </summary>
        private void SetPortProperty()
        {
            sp = new SerialPort();
            sp.PortName = cbxCOMPort.Text.Trim();//设置串口名
            sp.BaudRate = Convert.ToInt32(cbxBaudRate.Text.Trim());//设置波特率

            if (cbxStopBits.Text.Trim() == "0")//设置停止位
            {
                // sp.StopBits = StopBits.None;
                MessageBox.Show("停止位没有0位","错误提示");
            }
            else if (cbxStopBits.Text.Trim() == "1.5")
            {
                sp.StopBits = StopBits.OnePointFive;
            }
            else if (cbxStopBits.Text.Trim() == "2")
            {
                sp.StopBits = StopBits.Two;
            }
            else
            {
                sp.StopBits = StopBits.One;
            }
            sp.DataBits = Convert.ToInt16(cbxDataBits.Text.Trim());//设置数据位

            if (cbxParity.Text.Trim() == "奇校验")//设置奇偶校验
            {
                sp.Parity = Parity.Odd;
            }
            else if (cbxParity.Text.Trim() == "偶校验")
            {
                sp.Parity = Parity.Even;
            }
            else
            {
                sp.Parity = Parity.None;
            }
            //设置超时读取时间
            sp.ReadTimeout = -1;
            sp.RtsEnable = true;
            //定义DataReceived事件，当串口收到数据后触发时间
            sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            if (rbnHEX.Checked)
            {
                isHEX = true;
            }
            else
            {
                isHEX = false;
            }
        }

        private void btnOpenCom_Click(object sender, EventArgs e)
        {
            if (isOpen == false)
            {
                if (!CheckPortSetting())//检测串口设置
                {
                    MessageBox.Show("串口未设置", "错误提示");
                    return;
                }
                if (!isSetProperty)
                {
                    SetPortProperty();
                    isSetProperty = true;
                }
                try
                {
                    sp.Open();
                    isOpen = true;
                    btnOpenCom.Text = "关闭窗口";
                    //串口打开后，相关的串口设置按钮不可用
                    cbxCOMPort.Enabled = false;
                    cbxBaudRate.Enabled = false;
                    cbxDataBits.Enabled = false;
                    cbxStopBits.Enabled = false;
                    cbxParity.Enabled = false;
                    rbnChar.Enabled = false;
                    rbnHEX.Enabled = false;
                }
                catch (Exception)
                {
                    //打开串口失败后，相应的标志位取消
                    isSetProperty = false;
                    isOpen = false;
                    MessageBox.Show("串口无效或已被占用", "错误提示");
                }
            }
            else
            {
                try//打开串口
                {
                    sp.Close();
                    isOpen = false;
                    isSetProperty = false;
                    btnOpenCom.Text = "打开串口";
                    //关闭串口后，串口设置选项可以继续使用
                    cbxCOMPort.Enabled = true;
                    cbxBaudRate.Enabled = true;
                    cbxDataBits.Enabled = true;
                    cbxStopBits.Enabled = true;
                    cbxParity.Enabled = true;
                    rbnChar.Enabled = true;
                    rbnHEX.Enabled = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("关闭串口时发生错误","错误提示");
                }
            }
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            if (isOpen)
            {
                try
                {
                    sp.WriteLine(tbxSendData.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("发送数据时发生错误", "错误提示");
                    return;
                }
            }
            else
            {
                MessageBox.Show("串口未打开","错误提示");
                return;
            }
            if (!CheckSendData())
            {
                MessageBox.Show("请输入要发送的数据","错误提示");
                return;
            }
        }
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(100);//延时100ms等待接收完数据
            //this,Invoke是跨线程访问UI的方法，也是文本的范例
            this.Invoke((EventHandler)delegate
            {
                if (isHEX == false)
                {
                    tbxRecvData.Text += sp.ReadLine();
                }
                else
                {
                    byte[] ReceiveData = new byte[sp.BytesToRead];
                    sp.Read(ReceiveData, 0, ReceiveData.Length);
                    string RecvDataText = null;
                    for (int i = 0; i < ReceiveData.Length; i++)
                    {
                        RecvDataText += ("0x" + ReceiveData[i].ToString("X2") + " ");
                    }
                    tbxRecvData.Text += RecvDataText;
                }
                sp.DiscardInBuffer();
            });
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {
            tbxRecvData.Text = "";
            tbxSendData.Text = "";
        }
    }
}
