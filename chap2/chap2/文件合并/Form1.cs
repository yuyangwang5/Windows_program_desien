using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 文件合并
{
    public partial class Form1 : Form
    {
        string folder_path;
        string dest_file;
        public static string[] folder_files;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK )
            {
                folder_path = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(Directory.Exists(folder_path))
            {
                folder_files = Directory.GetFiles(folder_path, textBox1.Text, SearchOption.AllDirectories);
                listBox1.Items.Clear();
                int idx = 0;
                foreach( string file in folder_files )
                {
                    idx = listBox1.Items.Add( file );
                    listBox1.SetSelected(0, true);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int sel_index = listBox2.SelectedIndex;
            string sel_str = listBox2.SelectedItem.ToString();
            if (sel_index > 0)
            {
                listBox2.Items[sel_index] = listBox2.Items[sel_index - 1];
                listBox2.Items[sel_index - 1] = sel_str;
                listBox2.SetSelected(sel_index, false);
                listBox2.SetSelected(sel_index - 1, true);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int sel_index = listBox2.SelectedIndex;
            string sel_str = listBox2.SelectedItem.ToString();
            if (sel_index < listBox2.Items.Count - 1)
            {
                listBox2.Items[sel_index] = listBox2.Items[sel_index + 1];
                listBox2.Items[sel_index + 1] = sel_str;
                listBox2.SetSelected(sel_index, false);
                listBox2.SetSelected(sel_index + 1, true);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "选择要合并后的文件";
            saveFileDialog1.InitialDirectory = Environment.SpecialFolder.DesktopDirectory.ToString();
            saveFileDialog1.OverwritePrompt = false;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                dest_file = saveFileDialog1.FileName;
                label2.Text = dest_file;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var item in listBox1.Items)
            {
                listBox2.Items.Add(item.ToString());
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if(File.Exists(dest_file))
            {
                File.Delete(dest_file);
            }
            FileStream fs_dest = new FileStream(dest_file, FileMode.CreateNew, FileAccess.Write);
            byte[] DataBuffer = new byte[1024*1024];
            byte[] file_name_buf;

            FileStream fs_source = null;
            int read_len;
            FileInfo fi_a = null;
            
            for (int i = 0;i < listBox2.Items.Count; ++i)
            {
                fi_a = new FileInfo(listBox2.Items[i].ToString());
                file_name_buf = Encoding.Default.GetBytes(fi_a.Name);
                // 写入文件名
                fs_dest.Write(file_name_buf, 0, file_name_buf.Length);
                // 换行
                fs_dest.WriteByte((byte)13);
                fs_dest.WriteByte((byte)10);

                fs_source = new FileStream(fi_a.FullName, FileMode.Open, FileAccess.Read);
                read_len = fs_source.Read(DataBuffer, 0, 1024 * 1024);
                while(read_len > 0)
                {
                    fs_dest.Write(DataBuffer, 0, read_len);
                    read_len = fs_source.Read(DataBuffer, 0, 1024 * 1024);
                }

                // 换行
                fs_dest.WriteByte((byte)13);
                fs_dest.WriteByte((byte)10);

                fs_source.Close();
            }
            fs_source.Dispose();
            fs_dest.Flush();
            fs_dest.Close();
            fs_dest.Dispose();
            //Process.Start(dest_file);
        }
    }
}
