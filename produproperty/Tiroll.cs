using System.Diagnostics;
using System.Linq;
using System.Text;

namespace produproperty
{
    public class Tiroll
    {
        //生成标题目录

        /// <summary>
        /// 生成标题目录
        /// </summary>
        /// <param name="str"></param>
        public static void Guidanceroll(string str)
        {
            StringBuilder sivera = new StringBuilder();
            foreach (var temp in str.Split('\n').Select(temp => temp.Replace("\r", "")))
            {
                sivera.Append(Guidanoll(temp));
            }
        }

        private static string Guidanoll(string temp)
        {
            int n = 0;

            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == '#')
                {
                    n++;
                }
                else
                {
                    break;
                }
            }
            if (n > 0)
            {
                string str = temp.Replace("#", "").Trim();
                str = "[" + str + "]" + "(#" + str + ")";
                str = "- " + str;
                for (int i = 0; i < n; i++)
                {
                    str = " " + str;
                }
                str = str + "\r\n\r\n";
                return str;
            }
            else
            {
                return "";
            }
        }
    }
}