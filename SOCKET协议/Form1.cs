using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOCKET协议
{
    //socket套接字，用来描述ip地址和端口
    //TCP:需要请求，回应，确认三次握手，安全稳定但是效率相对低
    //UDP：快速。效率高，但是不稳定，容易发生数据丢失。一般用于视频传输的时候。
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    }
}
