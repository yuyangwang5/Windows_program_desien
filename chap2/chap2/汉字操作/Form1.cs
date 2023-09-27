using Microsoft.International.Converters.PinYinConverter;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetSpeech;

namespace 汉字操作
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Trim().Length == 0)
            {
                return;
            }
            char one_char = textBox1.Text.Trim().ToCharArray()[0];
            int ch_int = (int)one_char;     // 找到第一个字符的unicode编码
            string str_char_int = string.Format("{0}", ch_int);
            if (ch_int > 127)
            {
                ChineseChar chineseChar = new ChineseChar(one_char);
                IReadOnlyCollection<string> pinyin = chineseChar.Pinyins;
                string pin_str = "";
                foreach (string pin in pinyin)
                {
                    pin_str += pin + "\r\n";
                }
                textBox2.Text = "";
                textBox2.Text = pin_str;
            }
            label1.Text = str_char_int;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 繁体字转为简体
            textBox2.Text = ChineseConverter.Convert(textBox1.Text.Trim(), ChineseConversionDirection.TraditionalToSimplified);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 简体字转为繁体
            textBox2.Text = ChineseConverter.Convert(textBox1.Text.Trim(), ChineseConversionDirection.SimplifiedToTraditional);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SpeechVoiceSpeakFlags spFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            SpVoice voice = new SpVoice();
            voice.Speak(textBox1.Text.Trim(), spFlags);
        }
    }
}
