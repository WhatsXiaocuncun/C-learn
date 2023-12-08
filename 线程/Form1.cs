using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 线程
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Thread th;
        private void button1_Click(object sender, EventArgs e)
        {
            //新创建一个线程去执行这个方法
            th = new Thread(test);
            th.IsBackground = true;//设置为后台线程，前台关闭后即关闭该线程
            th.Start();
            //前台线程和后台线程的概念：前：只有所有的前台线程都关闭后才能完成程序关闭；后：只要前台线程关闭，后台线程立即结束
        }

        private void test()
        {
            for (int i = 0; i < 99999; i++)
            {
                //Console.WriteLine(i);
                textBox1.Text = i.ToString();//.Net平台下不允许跨线程访问
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //取消检查跨线程的访问，
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //当关闭窗体的时候，判断新线程是不是为null
            if (th != null)
            {
                th.Abort();//线程被关闭后就不能重新再开始了
            }
        }
    }
}
