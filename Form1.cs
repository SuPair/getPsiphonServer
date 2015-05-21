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
using Microsoft.Win32;

namespace getPsiphonServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();

            textBox1.Text = fbd.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strServerContent = getServerContent();
            if (strServerContent.Length > 1)
            {
                string strSavePath = textBox1.Text;

                using (StreamWriter sw = new StreamWriter(new FileStream(strSavePath + "\\psi_client.dat", FileMode.Create)))
                {
                    sw.Write("{");
                    sw.Write("\"propagation_channel_id\"");
                    sw.Write(":");
                    sw.Write("\"FFFFFFFFFFFFFFFF\",");

                    sw.Write("\"sponsor_id\"");
                    sw.Write(":");
                    sw.Write("\"FFFFFFFFFFFFFFFF\",");

                    sw.Write("\"servers\"");
                    sw.Write(":");
                    sw.Write("[" + strServerContent.Remove(strServerContent.Length - 1) + "]");

                    sw.Write("}");
                }
                MessageBox.Show("生成完成！");
            }
            else
            {
                MessageBox.Show("没有安装psiphon 3，或者路径不正确，请检查注册表路径");
            }
        }
        private string getServerContent()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Psiphon3");

            StringBuilder sbContent = new StringBuilder();
            if(key!=null)
            {
                string value = key.GetValue("ServersSSH", "").ToString().Replace("\r", ",").Replace("\n", ",");
                formatServerContent(sbContent, value);
                
                value = key.GetValue("ServersVPN", "").ToString().Replace("\r", ",").Replace("\n", ",");
                formatServerContent(sbContent, value);
                
                value = key.GetValue("ServersOSSH", "").ToString().Replace("\r", ",").Replace("\n", ",");
                formatServerContent(sbContent, value);
            }
            return sbContent.ToString();
        }

        private void formatServerContent(StringBuilder sbContent, string str)
        {
            string[] arr = str.Split(',');

            for (int i = 0; i < arr.Length - 1; i++)
            {
                sbContent.Append("\"");
                sbContent.Append(arr[i]);
                sbContent.Append("\",");
            }
        }
    }
}
