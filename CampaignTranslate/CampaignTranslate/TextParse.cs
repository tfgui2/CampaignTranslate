using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignTranslate
{
    class TextParse
    {
        public string[] m_lines;
        public string m_filename;
        private bool m_isDesc = false;
        public int m_totalchars = 0;
        public Dictionary<int, string> m_targetLines = new Dictionary<int, string>();
        public Dictionary<int, string> m_newTargetLines = new Dictionary<int, string>();
        public int m_middleNumber = 0;

        private void Clear()
        {
            m_isDesc = false;
            m_targetLines.Clear();
            m_newTargetLines.Clear();
            m_totalchars = 0;
            m_middleNumber = 0;
        }

        public bool Open(string filename)
        {
            this.Clear();

            m_lines = System.IO.File.ReadAllLines(filename);
            if (m_lines.Length <= 0)
            {
                return false;
            }

            m_filename = filename;

            for (int i = 0; i < m_lines.Length; i++)
            {
                CheckTarget(i, m_lines[i]);
            }

            return true;
        }


        private void CheckTarget(int linenumber, string line)
        {
            if (line.Contains("<Description>"))
            {
                m_isDesc = true;
                return;
            }
            if (line.StartsWith("<") || line.StartsWith("["))
            {
                m_isDesc = false;
                return;
            }

            if (m_isDesc)
            {
                m_totalchars += line.Length;
                m_targetLines[linenumber] = line;
                if (m_middleNumber == 0 && m_totalchars > 4500)
                {
                    m_middleNumber = linenumber;
                }
            }
        }

        public bool SetTransle(string [] newlines)
        {
            if ( m_targetLines.Count != newlines.Length)
            {
                return false;
            }

            int i = 0;
            foreach (KeyValuePair<int,string> tmp in m_targetLines)
            {
                m_newTargetLines[tmp.Key] = newlines[i]; i++;
            }

            return true;
        }

        public bool Save()
        {
            List<string> sb = new List<string>();

            for (int i = 0; i < m_lines.Length; i++)
            {
                sb.Add(m_lines[i]);

                if (m_newTargetLines.ContainsKey(i))
                {
                    if (m_newTargetLines[i].Length > 1)
                        sb.Add(m_newTargetLines[i]);
                }
            }

            string newfilename = m_filename + ".new";
            System.IO.File.WriteAllLines(newfilename, sb);
            return true;
        }


    }
}
