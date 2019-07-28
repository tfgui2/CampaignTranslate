using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CampaignTranslate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        TextParse tp = new TextParse();

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (s.Length <= 0)
            {
                return;
            }

            if (tp.Open(s[0]))
            {
                this.ClearText();
                this.SetPage(0);
                if (tp.m_middleNumber > 0)
                {
                    buttonNext.Enabled = true;
                    MessageBox.Show("Text over " + textBox1.Text.Length.ToString() + " and total " + tp.m_totalchars);
                }

                this.Text = s[0];
            }
        }

        private void SetPage(int page)
        {
            Dictionary<int, string> dic = tp.m_targetLines;
            int minNumber = 0;
            int maxNumber = 10000;

            if (tp.m_middleNumber > 0)
            {
                if (page == 0)
                {
                    maxNumber = tp.m_middleNumber;
                }
                else
                {
                    minNumber = tp.m_middleNumber;
                }
            }

            textBox1.Clear();
            foreach (KeyValuePair<int, string> temp in dic)
            {
                if (minNumber <= temp.Key && temp.Key < maxNumber)
                {

                    if (textBox1.Text.Length > 0)
                    {
                        textBox1.AppendText(Environment.NewLine);
                    }
                    textBox1.AppendText(temp.Value);
                }
            }
            
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ClearText()
        {
            textBox1.Clear();
            textBox2.Clear();
            buttonNext.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClearText();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            this.SetPage(1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tp.SetTransle(textBox2.Lines))
            {

                tp.Save();
                MessageBox.Show("End");
            }
            else
            {
                MessageBox.Show("fail");
            }

            
        }
    }
}
