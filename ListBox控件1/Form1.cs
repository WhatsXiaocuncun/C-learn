using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ListBox控件1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string[] path = Directory.GetFiles(@"C:\Users\WuYuc\Desktop\C#案例截图", "*.PNG");
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < path.Length; i++)
            {
                // listBox1.Items.Add(path[i]);//得到路径
               string fileName =  Path.GetFileName(path[i]);
                listBox1.Items.Add(fileName);
            }
        }
        /// <summary>
        /// 双击组件时候播放图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(path[listBox1.SelectedIndex]);
        }
    }
}
